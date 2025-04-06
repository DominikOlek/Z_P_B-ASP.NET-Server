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
using ApiP.Exceptions;
using Newtonsoft.Json;
using System.IO;

namespace ApiP.Services
{
    public interface IPrimaryService
    {
        PageResult<ReturnUserDto> GetAll(Query search, bool all);
        void SetUser(SetUserDto dto);
        PageResult<ReturnBigMandatDto> GetMandats(Query search, DateTime date,bool oplata);
        List<Role> GetRoles();
        void Addtar(AddTaryfikatorDto dto);
        void Removetar(int ID);
        void MOdtar(AddTaryfikatorDto dto, int id);
        PageResult<Taryfikator> GetTar(Query search, int pkt);
    }
    public class PrimaryService : IPrimaryService
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapp;
        private IEmailSender _emailSender { get; set; }
        private string TarFilePatch = @"D:\Nowy folder\ApiP\JSON\Taryfikaty.json";

        public PrimaryService(MainDbContext DbContext, IMapper Mapp, IEmailSender EmailSender)
        {
            _dbContext = DbContext;
            _mapp = Mapp;
            _emailSender = EmailSender;
        }

        public PageResult<ReturnUserDto> GetAll(Query search,bool all)
        {
            var list =_dbContext.UsersBd.Include(a=>a.Rola).Where(a => (a.Aktywny == all)
            &&(search == null || a.Imie.ToLower().Contains(search.Search.ToLower())
            || a.Nazwisko.ToLower().Contains(search.Search.ToLower()) || 
            a.Nr_Sluzbowy.ToString().Contains(search.Search))).ToList();
            var a= _mapp.Map<List<ReturnUserDto>>(list);

            List<ReturnUserDto> pagging = new List<ReturnUserDto>();
            if (search.PageSize * (search.PageNumber - 1) > a.Count()){
                pagging = a.Take(search.PageSize).ToList();
                search.PageNumber = 1;
            } else{
                pagging = a.Skip(search.PageSize * (search.PageNumber - 1)).Take(search.PageSize).ToList();
            }
            var result = new PageResult<ReturnUserDto>(pagging, list.Count(), search.PageSize, search.PageNumber);
            return result;
        }

        public PageResult<ReturnBigMandatDto> GetMandats(Query search, DateTime date,bool oplata)
        {
            var list = _dbContext.MandatyBd.Include(a => a.Przez).Include(a=>a.Powod).Include(a=>a.Kierowcy).Where(a=>(date == DateTime.MinValue || a.DataWydania.Date == date.Date)&&(search.Search==null||
            (a.Przez.Nr_Sluzbowy.ToString().Contains(search.Search)|| a.Kierowcy.Imie.ToLower().Contains(search.Search.ToLower()) || a.Kierowcy.Nazwisko.ToLower().Contains(search.Search.ToLower())))
            &&(oplata?true:a.DataOplacenia == DateTime.MinValue));

            var a = _mapp.Map<List<ReturnBigMandatDto>>((SortEnum)search.Sort == SortEnum.ASC? list.OrderBy(a=>a.DataWydania): list.OrderByDescending(a=>a.DataWydania));

            List<ReturnBigMandatDto> pagging = new List<ReturnBigMandatDto>();
            if (search.PageSize * (search.PageNumber - 1) > a.Count()){
                pagging = a.Take(search.PageSize).ToList();
                search.PageNumber = 1;
            }else{
                pagging = a.Skip(search.PageSize * (search.PageNumber - 1)).Take(search.PageSize).ToList();
            }
            var result = new PageResult<ReturnBigMandatDto>(pagging, list.Count(), search.PageSize, search.PageNumber);
            return result;
        }

        public void SetUser(SetUserDto dto)
        {
            var user = _dbContext.UsersBd.FirstOrDefault(a => a.Nr_Sluzbowy == dto.Nr_Sluzbowy);
            if (user == null)
                throw new NotFoundExceptions("Błądne dane");
            if (dto.Rola == 0)
            {
                _dbContext.UsersBd.Remove(user);
                _dbContext.SaveChanges();
                _emailSender.SendEmailAsync(user.Email, "Blokada konta", "Twoje konto zostało zablokowane przez administratora.");
            }
            else
            {
                user.Aktywny = true;
                user.RolaID = dto.Rola;
                if (!_dbContext.RolesBd.Any(a => a.ID == dto.Rola))
                    throw new NotFoundExceptions("Taka rola nie istnieje");
                _emailSender.SendEmailAsync(user.Email, "Aktywacja konta", "Twoje konto zostało zatwierdzone przez administratora. Miłej pracy");
                _dbContext.SaveChanges();
            }
        }

        public List<Role> GetRoles() {
            var role = _dbContext.RolesBd.ToList();
            return role;
        }

        public void Addtar(AddTaryfikatorDto dto) {
            var tarrappend = _mapp.Map<Taryfikator>(dto);
            if (_dbContext.TaryfikatorBd.FirstOrDefault(a => a.Tytul == tarrappend.Tytul) != null)
                throw new Exception("Taki taryfikator już istnieje");


            _dbContext.TaryfikatorBd.Add(tarrappend);
            _dbContext.SaveChanges();

            string TaryfFile = File.ReadAllText(TarFilePatch);
            List<Taryfikator> tarrlist = JsonConvert.DeserializeObject<List<Taryfikator>>(TaryfFile);
            tarrlist.Add(tarrappend);
            var ser = JsonConvert.SerializeObject(tarrlist);
            File.WriteAllText(TarFilePatch, ser);
        }

        public void Removetar(int ID)
        {
            var removeTar =_dbContext.TaryfikatorBd.FirstOrDefault(a => a.ID == ID);
            if (removeTar == null)
                throw new NotFoundExceptions("Błędne dane taryfikatora");

            string TaryfFile = File.ReadAllText(TarFilePatch);
            List<Taryfikator> tarrlist = JsonConvert.DeserializeObject<List<Taryfikator>>(TaryfFile);
            var a =tarrlist.RemoveAll(a=>a.Liczba_PKT == removeTar.Liczba_PKT && a.MiesiąceWstrzymania==removeTar.MiesiąceWstrzymania && a.Tytul==removeTar.Tytul);
            if (a > 1)
                tarrlist.Add(new Taryfikator { Tytul=removeTar.Tytul,MiesiąceWstrzymania=removeTar.MiesiąceWstrzymania,Liczba_PKT=removeTar.Liczba_PKT});
            var ser = JsonConvert.SerializeObject(tarrlist);
            File.WriteAllText(TarFilePatch, ser);


            _dbContext.TaryfikatorBd.Remove(removeTar);
            _dbContext.SaveChanges();
        }

        public void MOdtar(AddTaryfikatorDto dto,int id) {
            var tarrMod = _mapp.Map<Taryfikator>(dto);
            string TaryfFile = File.ReadAllText(TarFilePatch);
            List<Taryfikator> tarrlist = JsonConvert.DeserializeObject<List<Taryfikator>>(TaryfFile);
            var toMod = tarrlist.FirstOrDefault(a => a.ID == id);
            if (toMod == null)
                throw new NotFoundExceptions("Taki taryfikator nie istnieje");
            tarrlist[tarrlist.IndexOf(toMod)] = tarrMod;
            tarrlist[tarrlist.IndexOf(tarrMod)].ID = id;
            var ser = JsonConvert.SerializeObject(tarrlist);
            File.WriteAllText(TarFilePatch, ser);


            toMod =_dbContext.TaryfikatorBd.FirstOrDefault(a => a.ID == id);
            toMod.Tytul = tarrMod.Tytul;
            toMod.Liczba_PKT = tarrMod.Liczba_PKT;
            toMod.MiesiąceWstrzymania = tarrMod.MiesiąceWstrzymania;
            _dbContext.SaveChanges();
        }

        public PageResult<Taryfikator> GetTar(Query search, int pkt)
        {
            var list = _dbContext.TaryfikatorBd.Where(a => (pkt == 0 || a.Liczba_PKT == pkt) && (search.Search == null || (a.Tytul.ToLower().Contains(search.Search.ToLower()))));
            var a = (SortEnum)search.Sort == SortEnum.ASC ? pkt==0?list.OrderBy(a => a.Liczba_PKT).ThenBy(a=>a.MiesiąceWstrzymania): list.OrderBy(a => a.Tytul) : pkt == 0 ? list.OrderByDescending(a => a.Liczba_PKT).ThenByDescending(a=>a.MiesiąceWstrzymania) : list.OrderByDescending(a => a.Tytul);
            List<Taryfikator> pagging = new List<Taryfikator>();
            if (search.PageSize * (search.PageNumber - 1) > a.Count())
            {
                pagging = a.Take(search.PageSize).ToList();
                search.PageNumber = 1;
            }else{
                pagging = a.Skip(search.PageSize * (search.PageNumber - 1)).Take(search.PageSize).ToList();
            }
            var result = new PageResult<Taryfikator>(pagging, list.Count(), search.PageSize, search.PageNumber);
            return result;
        }
    }
}
