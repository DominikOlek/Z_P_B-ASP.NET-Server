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
        List<Roles> GetRoles();
        void Addtar(AddTaryfikatorDto dto);
        void Removetar(int ID);
        void EditTar(AddTaryfikatorDto dto, int id);
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
            var list =_dbContext.UsersBd.Include(a=>a.Role).Where(a => (a.IsActive == all)
            &&(search == null || a.Name.ToLower().Contains(search.Search.ToLower())
            || a.LastName.ToLower().Contains(search.Search.ToLower()) || 
            a.BadgeNumber.ToString().Contains(search.Search))).ToList();
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
            var list = _dbContext.TicketsBd.Include(a => a.Cop).Include(a=>a.Reason).Include(a=>a.Driver).Where(a=>(date == DateTime.MinValue || a.DateOfTicket.Date == date.Date)&&(search.Search==null||
            (a.Cop.BadgeNumber.ToString().Contains(search.Search)|| a.Driver.Name.ToLower().Contains(search.Search.ToLower()) || a.Driver.LastName.ToLower().Contains(search.Search.ToLower())))
            &&(oplata?true:a.DateOfPayment == DateTime.MinValue));

            var a = _mapp.Map<List<ReturnBigMandatDto>>((SortEnum)search.Sort == SortEnum.ASC? list.OrderBy(a=>a.DateOfTicket): list.OrderByDescending(a=>a.DateOfTicket));

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
            var user = _dbContext.UsersBd.FirstOrDefault(a => a.BadgeNumber == dto.BadgeNumber);
            if (user == null)
                throw new NotFoundExceptions("Błądne dane");
            if (dto.Role == 0)
            {
                _dbContext.UsersBd.Remove(user);
                _dbContext.SaveChanges();
                _emailSender.SendEmailAsync(user.Email, "Blokada konta", "Twoje konto zostało zablokowane przez administratora.");
            }
            else
            {
                user.IsActive = true;
                user.RoleID = dto.Role;
                if (!_dbContext.RolesBd.Any(a => a.ID == dto.Role))
                    throw new NotFoundExceptions("Taka rola nie istnieje");
                _emailSender.SendEmailAsync(user.Email, "Aktywacja konta", "Twoje konto zostało zatwierdzone przez administratora. Miłej pracy");
                _dbContext.SaveChanges();
            }
        }

        public List<Roles> GetRoles() {
            var role = _dbContext.RolesBd.ToList();
            return role;
        }

        public void Addtar(AddTaryfikatorDto dto) {
            var tarrappend = _mapp.Map<Taryfikator>(dto);
            if (_dbContext.TaryfikatBd.FirstOrDefault(a => a.Title == tarrappend.Title) != null)
                throw new Exception("Taki taryfikator już istnieje");


            _dbContext.TaryfikatBd.Add(tarrappend);
            _dbContext.SaveChanges();

            string TaryfFile = File.ReadAllText(TarFilePatch);
            List<Taryfikator> tarrlist = JsonConvert.DeserializeObject<List<Taryfikator>>(TaryfFile);
            tarrlist.Add(tarrappend);
            var ser = JsonConvert.SerializeObject(tarrlist);
            File.WriteAllText(TarFilePatch, ser);
        }

        public void Removetar(int ID)
        {
            var removeTar =_dbContext.TaryfikatBd.FirstOrDefault(a => a.ID == ID);
            if (removeTar == null)
                throw new NotFoundExceptions("Błędne dane taryfikatora");

            string TaryfFile = File.ReadAllText(TarFilePatch);
            List<Taryfikator> tarrlist = JsonConvert.DeserializeObject<List<Taryfikator>>(TaryfFile);
            var a =tarrlist.RemoveAll(a=>a.PointNumber == removeTar.PointNumber && a.MonthsOfLost==removeTar.MonthsOfLost && a.Title==removeTar.Title);
            if (a > 1)
                tarrlist.Add(new Taryfikator { Title=removeTar.Title,MonthsOfLost=removeTar.MonthsOfLost,PointNumber=removeTar.PointNumber});
            var ser = JsonConvert.SerializeObject(tarrlist);
            File.WriteAllText(TarFilePatch, ser);


            _dbContext.TaryfikatBd.Remove(removeTar);
            _dbContext.SaveChanges();
        }

        public void EditTar(AddTaryfikatorDto dto,int id) {
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


            toMod =_dbContext.TaryfikatBd.FirstOrDefault(a => a.ID == id);
            toMod.Title = tarrMod.Title;
            toMod.PointNumber = tarrMod.PointNumber;
            toMod.MonthsOfLost = tarrMod.MonthsOfLost;
            _dbContext.SaveChanges();
        }

        public PageResult<Taryfikator> GetTar(Query search, int pkt)
        {
            var list = _dbContext.TaryfikatBd.Where(a => (pkt == 0 || a.PointNumber == pkt) && (search.Search == null || (a.Title.ToLower().Contains(search.Search.ToLower()))));
            var a = (SortEnum)search.Sort == SortEnum.ASC ? pkt==0?list.OrderBy(a => a.PointNumber).ThenBy(a=>a.MonthsOfLost): list.OrderBy(a => a.Title) : pkt == 0 ? list.OrderByDescending(a => a.PointNumber).ThenByDescending(a=>a.MonthsOfLost) : list.OrderByDescending(a => a.Title);
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
