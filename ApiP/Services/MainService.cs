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

namespace ApiP.Services {
    public interface IMainService
    {
        string Test(int liczba);
        string[] AddTicketF(AddTicketDto mandatDto, int IDWystaw);
        public ReturnDriverDto DriverInfoF(string PESEL);
        public void PaymentF(string id);
        public ReturnTicketDto[] DriverTickets(string PESEL);
        public string History(string PESEL);
        List<Taryfikator> GetTar();
    }
    public class MainService : IMainService
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapp;
        private IEmailSender _emailSender { get; set; }

        public MainService(MainDbContext DbContext, IMapper Mapp, IEmailSender EmailSender)
        {
            _dbContext = DbContext;
            _mapp = Mapp;
            _emailSender = EmailSender;
        }
        public string Test(int liczba)
        {
            return "Wszystko spoko" + liczba;
        }

        public string[] AddTicketF(AddTicketDto mandatDto,int IDWystaw) {
            var tenkierowca =_dbContext.DriversBd.FirstOrDefault(a => a.Pesel == mandatDto.Pesel);
            if (tenkierowca == null)
                DodanieKierowcy(mandatDto);
            else if ((tenkierowca == null && _dbContext.HistBd.FirstOrDefault(a => a.PESEL == mandatDto.Pesel) != null) || tenkierowca.IsTimelyLost || tenkierowca.IsPermanentLost)
                return new string[] { "", "TEN KIEROWCA NIE POSIADA UPRAWNIEÑ DO PROWADZENIA POJAZDAMI" };
            tenkierowca = _dbContext.DriversBd.FirstOrDefault(a => a.Pesel == mandatDto.Pesel);
            var Mandat = _mapp.Map<Tickets>(mandatDto);
            Mandat.DriverID = tenkierowca.ID;
            Mandat.DateOfTicket = DateTime.Today;
            Mandat.CopID = IDWystaw;
            _dbContext.TicketsBd.Add(Mandat);

            var taryfikatMandatu = _dbContext.TaryfikatBd.FirstOrDefault(a => a.ID == Mandat.ReasonID);
            tenkierowca.Pkt += taryfikatMandatu.PointNumber;
            string TytylMail = "OTRZYMA£EŒ MANDAT";
            string Tekst = $"Driver { string.Join(' ', tenkierowca.Name, tenkierowca.LastName)} Otrzyma³ mandat za: {taryfikatMandatu.Title}. Description zdarzenia: {Mandat.Description}. Pamiêtaj o uragulowaniu p³atnoœci. Obecna liczba punktów karnych: {tenkierowca.Pkt}";
            Send("romaine.gorczany23@ethereal.email", Tekst, TytylMail);
            if (((DateTime.Today - tenkierowca.DateOfPassLicense).TotalDays / 365 < 1 && tenkierowca.Pkt >= 20) || tenkierowca.Pkt >= 24)
            {
                //odebranie
                var MandatyKier =_dbContext.TicketsBd.Include(u=>u.Reason).Where(a => a.DriverID == tenkierowca.ID && DateTime.Compare(a.DateOfPayment , a.DateOfTicket)==1).OrderByDescending(b=>b.DateOfTicket);
                DodanieWpisuHistorii("Driver straci³ uprawnienia do kierowania pojazdami| ", tenkierowca);
                foreach (Tickets i in MandatyKier) {
                    DodanieWpisuHistorii("Driver otrzyma³ mandat za "+i.Reason.Title+"| " , tenkierowca);
                }
                tenkierowca.IsPermanentLost = true;
                _dbContext.TicketsBd.RemoveRange(MandatyKier);
                _dbContext.SaveChanges();
                TytylMail = "UTRACENIE PRAWA JAZDY";
                Tekst = "Driver " + string.Join(' ', tenkierowca.Name, tenkierowca.LastName) + " utraci³ prawo jazdy za zbyt du¿¹ liczbê punktów " + tenkierowca.Pkt+". Aby odzyskaæ uprawnienia musisz ponowanie zdaæ egzaminy";
                Send("romaine.gorczany23@ethereal.email", Tekst, TytylMail);
                return new string[] { Mandat.ID+"",Tekst };
            }
            if (Mandat.Reason.MonthsOfLost > 0) {
                tenkierowca.IsTimelyLost = true;
                var czasBd = _dbContext.TimelyBd.FirstOrDefault(a => a.DriverID == tenkierowca.ID);
                if (czasBd != null)
                {
                    czasBd.DateOfGiveBack.AddMonths(Mandat.Reason.MonthsOfLost);
                    DodanieWpisuHistorii("Driver otrzyma³ przed³u¿enie utraty prawa jazdy do dnia " +czasBd.DateOfGiveBack.Date , tenkierowca);
                    Tekst = "Driver " + string.Join(' ', tenkierowca.Name, tenkierowca.LastName) + " otrzyma³ przed³u¿enie utraty prawa jazdy do dnia " + czasBd.DateOfGiveBack.Date + " z powodu " + Mandat.Reason.Title;
                }
                else
                {
                    var czas = new TimelyLost { DriverID = tenkierowca.ID, DateOfTicket = DateTime.Today };
                    _dbContext.TimelyBd.Add(czas);
                    DodanieWpisuHistorii("Driver straci³ uprawnienia do kierowania pojazdami na okres" + Mandat.Reason.MonthsOfLost + "| ", tenkierowca);
                    Tekst = "Driver " + string.Join(' ', tenkierowca.Name, tenkierowca.LastName) + " utraci³ prawo jazdy na okres " + Mandat.Reason.MonthsOfLost + " miesiêcy za " + Mandat.Reason.Title;
                }
                _dbContext.SaveChanges();               
                TytylMail = "UTRACENIE PRAWA JAZDY";
                Send("romaine.gorczany23@ethereal.email", Tekst, TytylMail);
                return new string[] { Mandat.ID + "", Tekst };
            }
            _dbContext.SaveChanges();


            return new string[] { Mandat.ID + "", Tekst };
        }
        public async void Send(string toAddress,string body,string title)
        {
            await _emailSender.SendEmailAsync(toAddress, title, body);
        }
        public void DodanieKierowcy(AddTicketDto mandatDto) {
            var Kierowca =_mapp.Map<Drivers>(mandatDto);
            _dbContext.DriversBd.Add(Kierowca);
            _dbContext.SaveChanges();
        }
        public void DodanieWpisuHistorii(string tekst,Drivers tenkierowca) {
            History Hist = null;
            if (_dbContext.HistBd.Count() > 0) Hist = _dbContext.HistBd.FirstOrDefault(a => a.PESEL == tenkierowca.Pesel);
            if (Hist == null)
            {
                Hist = new History { PESEL = tenkierowca.Pesel, Name = tenkierowca.Name, LastName = tenkierowca.LastName, Description = " " };
                _dbContext.HistBd.Add(Hist);
                _dbContext.SaveChanges();
            }

            var a = Hist.Description;
            a=a.Insert(0, tekst);
            Hist.Description = a;
            _dbContext.SaveChanges();
        }

        public ReturnDriverDto DriverInfoF(string PESEL) {
            var a = _dbContext.DriversBd.FirstOrDefault(a => a.Pesel == PESEL);
            if (a == null)
                throw new NotFoundExceptions("Nie znaleziono takiego kierowcy");
            if (a.IsPermanentLost)
            {
                var retNo = new ReturnDriverDto { IsPermanentLost = true };
                return retNo;
            }
            return _mapp.Map<ReturnDriverDto>(a);
        }


        public void PaymentF(string id) {
            var Mandat = _dbContext.TicketsBd.Include(a => a.Driver).Include(a=>a.Reason).FirstOrDefault(a => a.ID == int.Parse(id));
            if (Mandat != null)
            {
                Mandat.DateOfPayment = DateTime.Today;
                if (Mandat.Driver.IsTimelyLost && !Mandat.Driver.IsPermanentLost)
                {
                    var Czasowo = _dbContext.TimelyBd.FirstOrDefault(a => a.DriverID == Mandat.DriverID);
                    if (Czasowo != null && Czasowo.DateOfGiveBack == DateTime.MinValue)
                    {
                        Czasowo.DateOfLost = DateTime.Today;
                        Czasowo.DateOfGiveBack = DateTime.Today.AddMonths(Mandat.Reason.MonthsOfLost);
                    }
                    else
                    {
                        _dbContext.SaveChanges();
                        throw new NotFoundExceptions("Nie znaleziono takiego mandatu");
                    }
                }
                if (Mandat.Driver.IsPermanentLost)
                {
                    _dbContext.TicketsBd.Remove(Mandat);
                    DodanieWpisuHistorii("Driver otrzyma³ mandat za: "+Mandat.Reason.Title+"| ", Mandat.Driver);
                    var Mandaty = _dbContext.TicketsBd.Where(a => a.DriverID == Mandat.DriverID && a.DateOfPayment >= a.DateOfTicket).ToList();
                    if (Mandaty.Count() == 0)
                    {
                        _dbContext.DriversBd.Remove(Mandat.Driver);
                    }
                }
                _dbContext.SaveChanges();
            }else
                throw new NotFoundExceptions("Nie znaleziono takiego mandatu");
        }

        public ReturnTicketDto[] DriverTickets(string PESEL)
        {
            var a = _dbContext.TicketsBd.Include(a=>a.Driver).Include(a=>a.Reason).Where(a => a.Driver.Pesel == PESEL).ToArray();
            if (a != null)
                return _mapp.Map<ReturnTicketDto[]>(a);
            throw new NotFoundExceptions("Nie znaleziono takiego kierowcy");
        }

        public string History(string PESEL)
        {
            var a = _dbContext.HistBd.FirstOrDefault(a => a.PESEL == PESEL);
            if (a != null)
                return a.Description;
            return "Ten kierowca nie ma wpisów w historii";
        }

        public List<Taryfikator> GetTar() {
            return _dbContext.TaryfikatBd.OrderBy(a => a.PointNumber).ToList();
        }
    }
}
