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
using ApiP.Services;
using System.Security.Cryptography;

namespace ApiP.Services
{
    public interface IAppUsersService
    {
        bool Register(RegisterAppDto dto);
        TokensModels GenerateJwt(LoginDto dto);
        TokensModels RefreshJWT(TokensModels tokens);
        bool deleteRef(string email);
    }
    public class AppUsersService : IAppUsersService
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapp;
        private readonly IPasswordHasher<AppUsers> _hasher;
        private readonly AuthencticationSettings _authenticationSettings;
        private readonly IMainService _mService;

        private IEmailSender _emailSender { get; set; }

        public AppUsersService(MainDbContext DbContext, IMapper Mapp, IEmailSender EmailSender, IPasswordHasher<AppUsers> Hasher, AuthencticationSettings AuthenticationSettings, IMainService MService)
        {
            _dbContext = DbContext;
            _mapp = Mapp;
            _emailSender = EmailSender;
            _hasher = Hasher;
            _authenticationSettings = AuthenticationSettings;
            _mService = MService;
        }

        public bool Register(RegisterAppDto dto)
        {
            if (_dbContext.AppUsersBd.FirstOrDefault(a=> a.Pesel == dto.Pesel || a.Email == dto.Email) == null)
            {
                if (dto.Password == dto.ConfirmPassword)
                {
                    var a = _mapp.Map<AppUsers>(dto);
                    var Password = _hasher.HashPassword(a, dto.Password);
                    a.PasswordHash = Password;
                    _dbContext.AppUsersBd.Add(a);
                    _dbContext.SaveChanges();
                    return true;
                }
                else { throw new ApiP.Exceptions.OverflowException("Hasła nie są takie same"); };

            }
            else
            {
                throw new ApiP.Exceptions.OverflowException("Konto już istnieje");
            }
        }

        public TokensModels GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.AppUsersBd.FirstOrDefault(a => a.Email == dto.Email);
            if (user == null)
            {
                throw new ApiP.Exceptions.NotFoundExceptions("Błędne dane logowania");
            }
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new ApiP.Exceptions.NotFoundExceptions("Błędne dane logowania");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                new Claim(ClaimTypes.Name,user.Imie),
                new Claim(ClaimTypes.Surname,user.Nazwisko),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.PrimarySid,user.Pesel)
            };
            var refToken = GenerateRefreshToken();
            user.RefreshToken = refToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_authenticationSettings.RefreshTokenExpireDays);
            _dbContext.SaveChanges();
            return new TokensModels { AccessToken = CodeJwt(claims), RefreshToken = refToken};
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public TokensModels RefreshJWT(TokensModels tokens)
        {
            if (tokens is null)
            {
                return null;
            }

            string accessToken = tokens.AccessToken;
            string refreshToken = tokens.RefreshToken;

            var User = GetOwner(accessToken);
            if (User == null)
            {
                return null;
            }
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _dbContext.AppUsersBd.FirstOrDefault(a => a.ID == int.Parse(userID));

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }

            var newAccessToken = CodeJwt(User.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _dbContext.SaveChanges();

            return new TokensModels
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        private ClaimsPrincipal GetOwner(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)),
                ValidateLifetime = false
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var owner = TokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return owner;

        }

        public string CodeJwt(List<Claim> claims)
        {
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

        public bool deleteRef(string email)
        {
            var user = _dbContext.AppUsersBd.FirstOrDefault(a => a.Email== email);
            if (user == null)
                return false;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }
    }
}