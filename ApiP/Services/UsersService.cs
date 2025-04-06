using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Models;
using ApiP.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ApiP.Exceptions;

namespace ApiP.Services
{
    public interface IUsersService
    {
        void Register(RegisterDto dto);
        string GenerateJwt(LoginDto dto);
        string RefreshJWT(Dictionary<string, string> user);
        UserInfoDto GetInformation(int id);
        DriverInfoDto GetDriverInfo(DriverInfoLog dto);
    }
    public class UsersService : IUsersService
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapp;
        private readonly IPasswordHasher<Users> _hasher;
        private readonly AuthencticationSettings _authenticationSettings;
        private readonly IMainService _mService;

        private IEmailSender _emailSender { get; set; }

        public UsersService(MainDbContext DbContext, IMapper Mapp, IEmailSender EmailSender,IPasswordHasher<Users> Hasher,AuthencticationSettings AuthenticationSettings,IMainService MService)
        {
            _dbContext = DbContext;
            _mapp = Mapp;
            _emailSender = EmailSender;
            _hasher = Hasher;
            _authenticationSettings = AuthenticationSettings;
            _mService = MService;
        }

        public void Register(RegisterDto dto) {
            if (_dbContext.UsersBd.FirstOrDefault(a => a.Nr_Sluzbowy == dto.Nr_Sluzbowy || a.Pesel == dto.Pesel) == null)
            {
                if (dto.Password == dto.ConfirmPassword)
                {
                    var a = _mapp.Map<Users>(dto);
                    var Password = _hasher.HashPassword(a, dto.Password);
                    a.PasswordHash = Password;
                    _dbContext.UsersBd.Add(a);
                    _dbContext.SaveChanges();
                }
                else throw new ApiP.Exceptions.OverflowException("Hasła nie są takie same");

            }
            else
            {
                throw new ApiP.Exceptions.OverflowException("Ktoś posiada już taki numer służbowy lub pesel");
            }
        }

        public string GenerateJwt(LoginDto dto) {
            var user = _dbContext.UsersBd.Include(a=>a.Rola).FirstOrDefault(a => a.Nr_Sluzbowy == dto.Nr_Sluzbowy && a.Aktywny && a.Email == dto.Email);
            if (user == null)
                throw new ApiP.Exceptions.OverflowException("Błędne dane logowania lub to konto jest nieaktywne");
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
                throw new ApiP.Exceptions.OverflowException("Błędne dane logowania");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                new Claim(ClaimTypes.Name,$"{user.Imie + user.Nazwisko }"),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Rola.Nazwa_Roli),
            };
            return CodeJwt(claims);
        }

        public string RefreshJWT(Dictionary<string,string> user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user["id"].ToString()),
                new Claim(ClaimTypes.Name,user["name"]),
                new Claim(ClaimTypes.Email,user["email"]),
                new Claim(ClaimTypes.Role,user["role"]),
            };
            return CodeJwt(claims);
        }

        public string CodeJwt(List<Claim> claims) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireTime = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMinutes);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expireTime,
                signingCredentials: cred);
            var TokenHandler = new JwtSecurityTokenHandler();
            return TokenHandler.WriteToken(token);
        }

        public UserInfoDto GetInformation(int id) {
            var user = _dbContext.UsersBd.FirstOrDefault(a => a.ID == id);
            if (user == null)
                throw new NotFoundExceptions("Nie znaleziono użytkownika");
            return _mapp.Map<UserInfoDto>(user);
        }

        public DriverInfoDto GetDriverInfo(DriverInfoLog dto) {
            var driver =_dbContext.KierowcyBd.FirstOrDefault(a => a.Pesel == dto.Pesel && a.Imie == dto.Imie && a.Nazwisko == dto.Nazwisko);
            var driverHist = _dbContext.HistBd.FirstOrDefault(a=>a.PESEL == dto.Pesel && a.Imie == dto.Imie && a.Nazwisko == dto.Nazwisko);
            DriverInfoDto Info = new DriverInfoDto();
            if (driver == null && driverHist == null)
                throw new NotFoundExceptions("Kierowca nie posiada wpisów");
            else if (driver.CzyUtracil)
            {
                Info.ImportantInfo = "KIEROWCA OBECNIE NIE POSIADA UPRAWNIEŃ DO PROWADZENIA POJAZDÓW";
                Info.History = _mService.History(dto.Pesel).Split('|');
            }
            else {
                if (_dbContext.CzasowoBd.Include(a => a.Kierowca).FirstOrDefault(a => a.Kierowca == driver) != null)
                    Info.ImportantInfo = "OBECNIE KIEROWCA MA WSTRZYMANE UPRAWNIENIA DO PROWADZENIA POJAZDÓW";
                Info.Mandats = _mService.MandatyKierowcy(dto.Pesel);
                Info.History = _mService.History(dto.Pesel).Split('|');
            }
            Info.ImportantInfo = Info.ImportantInfo.Insert(0, $"Obecna liczba punktów karnych:{driver.Pkt} ");
            return Info;

        }

    }
}
