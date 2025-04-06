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
        string[] DodanieMandatu(AddMandatDto mandatDto, int IDWystaw);
        public ReturnKierowcaDto InformacjeKierowcy(string PESEL);
        public void Oplacanie(string id);
        public ReturnMandatDto[] MandatyKierowcy(string PESEL);
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

        public string[] DodanieMandatu(AddMandatDto mandatDto,int IDWystaw) {
            var tenkierowca =_dbContext.KierowcyBd.FirstOrDefault(a => a.Pesel == mandatDto.Pesel);
            if (tenkierowca == null)
                DodanieKierowcy(mandatDto);
            else if ((tenkierowca == null && _dbContext.HistBd.FirstOrDefault(a => a.PESEL == mandatDto.Pesel) != null) || tenkierowca.CzyOdebrano || tenkierowca.CzyUtracil)
                return new string[] { "", "TEN KIEROWCA NIE POSIADA UPRAWNIEÑ DO PROWADZENIA POJAZDAMI" };
            tenkierowca = _dbContext.KierowcyBd.FirstOrDefault(a => a.Pesel == mandatDto.Pesel);
            var Mandat = _mapp.Map<Mandaty>(mandatDto);
            Mandat.KierowcyID = tenkierowca.ID;
            Mandat.DataWydania = DateTime.Today;
            Mandat.PrzezID = IDWystaw;
            _dbContext.MandatyBd.Add(Mandat);

            var taryfikatMandatu = _dbContext.TaryfikatorBd.FirstOrDefault(a => a.ID == Mandat.PowodID);
            tenkierowca.Pkt += taryfikatMandatu.Liczba_PKT;
            string TytylMail = "OTRZYMA£EŒ MANDAT";
            string Tekst = $"Kierowca { string.Join(' ', tenkierowca.Imie, tenkierowca.Nazwisko)} Otrzyma³ mandat za: {taryfikatMandatu.Tytul}. Opis zdarzenia: {Mandat.Opis}. Pamiêtaj o uragulowaniu p³atnoœci. Obecna liczba punktów karnych: {tenkierowca.Pkt}";
            Send("romaine.gorczany23@ethereal.email", Tekst, TytylMail);
            if (((DateTime.Today - tenkierowca.Data_orzymania).TotalDays / 365 < 1 && tenkierowca.Pkt >= 20) || tenkierowca.Pkt >= 24)
            {
                //odebranie
                var MandatyKier =_dbContext.MandatyBd.Include(u=>u.Powod).Where(a => a.KierowcyID == tenkierowca.ID && DateTime.Compare(a.DataOplacenia , a.DataWydania)==1).OrderByDescending(b=>b.DataWydania);
                DodanieWpisuHistorii("Kierowca straci³ uprawnienia do kierowania pojazdami| ", tenkierowca);
                foreach (Mandaty i in MandatyKier) {
                    DodanieWpisuHistorii("Kierowca otrzyma³ mandat za "+i.Powod.Tytul+"| " , tenkierowca);
                }
                tenkierowca.CzyUtracil = true;
                _dbContext.MandatyBd.RemoveRange(MandatyKier);
                _dbContext.SaveChanges();
                TytylMail = "UTRACENIE PRAWA JAZDY";
                Tekst = "Kierowca " + string.Join(' ', tenkierowca.Imie, tenkierowca.Nazwisko) + " utraci³ prawo jazdy za zbyt du¿¹ liczbê punktów " + tenkierowca.Pkt+". Aby odzyskaæ uprawnienia musisz ponowanie zdaæ egzaminy";
                Send("romaine.gorczany23@ethereal.email", Tekst, TytylMail);
                return new string[] { Mandat.ID+"",Tekst };
            }
            if (Mandat.Powod.Miesi¹ceWstrzymania > 0) {
                tenkierowca.CzyOdebrano = true;
                var czasBd = _dbContext.CzasowoBd.FirstOrDefault(a => a.KierowcaID == tenkierowca.ID);
                if (czasBd != null)
                {
                    czasBd.DataOddania.AddMonths(Mandat.Powod.Miesi¹ceWstrzymania);
                    DodanieWpisuHistorii("Kierowca otrzyma³ przed³u¿enie utraty prawa jazdy do dnia " +czasBd.DataOddania.Date , tenkierowca);
                    Tekst = "Kierowca " + string.Join(' ', tenkierowca.Imie, tenkierowca.Nazwisko) + " otrzyma³ przed³u¿enie utraty prawa jazdy do dnia " + czasBd.DataOddania.Date + " z powodu " + Mandat.Powod.Tytul;
                }
                else
                {
                    var czas = new CzasowoOdebrane { KierowcaID = tenkierowca.ID, DataWystawienia = DateTime.Today };
                    _dbContext.CzasowoBd.Add(czas);
                    DodanieWpisuHistorii("Kierowca straci³ uprawnienia do kierowania pojazdami na okres" + Mandat.Powod.Miesi¹ceWstrzymania + "| ", tenkierowca);
                    Tekst = "Kierowca " + string.Join(' ', tenkierowca.Imie, tenkierowca.Nazwisko) + " utraci³ prawo jazdy na okres " + Mandat.Powod.Miesi¹ceWstrzymania + " miesiêcy za " + Mandat.Powod.Tytul;
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
        public void DodanieKierowcy(AddMandatDto mandatDto) {
            var Kierowca =_mapp.Map<Kierowcy>(mandatDto);
            _dbContext.KierowcyBd.Add(Kierowca);
            _dbContext.SaveChanges();
        }
        public void DodanieWpisuHistorii(string tekst,Kierowcy tenkierowca) {
            Historia Hist = null;
            if (_dbContext.HistBd.Count() > 0) Hist = _dbContext.HistBd.FirstOrDefault(a => a.PESEL == tenkierowca.Pesel);
            if (Hist == null)
            {
                Hist = new Historia { PESEL = tenkierowca.Pesel, Imie = tenkierowca.Imie, Nazwisko = tenkierowca.Nazwisko, Opis = " " };
                _dbContext.HistBd.Add(Hist);
                _dbContext.SaveChanges();
            }

            var a = Hist.Opis;
            a=a.Insert(0, tekst);
            Hist.Opis = a;
            _dbContext.SaveChanges();
        }

        public ReturnKierowcaDto InformacjeKierowcy(string PESEL) {
            var a = _dbContext.KierowcyBd.FirstOrDefault(a => a.Pesel == PESEL);
            if (a == null)
                throw new NotFoundExceptions("Nie znaleziono takiego kierowcy");
            if (a.CzyUtracil)
            {
                var retNo = new ReturnKierowcaDto { CzyUtracil = true };
                return retNo;
            }
            return _mapp.Map<ReturnKierowcaDto>(a);
        }


        public void Oplacanie(string id) {
            var Mandat = _dbContext.MandatyBd.Include(a => a.Kierowcy).Include(a=>a.Powod).FirstOrDefault(a => a.ID == int.Parse(id));
            if (Mandat != null)
            {
                Mandat.DataOplacenia = DateTime.Today;
                if (Mandat.Kierowcy.CzyOdebrano && !Mandat.Kierowcy.CzyUtracil)
                {
                    var Czasowo = _dbContext.CzasowoBd.FirstOrDefault(a => a.KierowcaID == Mandat.KierowcyID);
                    if (Czasowo != null && Czasowo.DataOddania == DateTime.MinValue)
                    {
                        Czasowo.DataOdebrania = DateTime.Today;
                        Czasowo.DataOddania = DateTime.Today.AddMonths(Mandat.Powod.Miesi¹ceWstrzymania);
                    }
                    else
                    {
                        _dbContext.SaveChanges();
                        throw new NotFoundExceptions("Nie znaleziono takiego mandatu");
                    }
                }
                if (Mandat.Kierowcy.CzyUtracil)
                {
                    _dbContext.MandatyBd.Remove(Mandat);
                    DodanieWpisuHistorii("Kierowca otrzyma³ mandat za: "+Mandat.Powod.Tytul+"| ", Mandat.Kierowcy);
                    var Mandaty = _dbContext.MandatyBd.Where(a => a.KierowcyID == Mandat.KierowcyID && a.DataOplacenia >= a.DataWydania).ToList();
                    if (Mandaty.Count() == 0)
                    {
                        _dbContext.KierowcyBd.Remove(Mandat.Kierowcy);
                    }
                }
                _dbContext.SaveChanges();
            }else
                throw new NotFoundExceptions("Nie znaleziono takiego mandatu");
        }

        public ReturnMandatDto[] MandatyKierowcy(string PESEL)
        {
            var a = _dbContext.MandatyBd.Include(a=>a.Kierowcy).Include(a=>a.Powod).Where(a => a.Kierowcy.Pesel == PESEL).ToArray();
            if (a != null)
                return _mapp.Map<ReturnMandatDto[]>(a);
            throw new NotFoundExceptions("Nie znaleziono takiego kierowcy");
        }

        public string History(string PESEL)
        {
            var a = _dbContext.HistBd.FirstOrDefault(a => a.PESEL == PESEL);
            if (a != null)
                return a.Opis;
            return "Ten kierowca nie ma wpisów w historii";
        }

        public List<Taryfikator> GetTar() {
            return _dbContext.TaryfikatorBd.OrderBy(a => a.Liczba_PKT).ToList();
        }
    }
}
