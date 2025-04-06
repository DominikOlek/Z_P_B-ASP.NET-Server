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

namespace ApiP.Services
{
    public interface IServerService
    {
        public string GetActualization();
        public string SendReminder();
        public string MandatActualization();
    }
    public class ServerService : IServerService
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapp;
        private readonly ILogger<ServerService> _logger;

        private IEmailSender _emailSender { get; set; }

        public ServerService(MainDbContext DbContext, IMapper Mapp, IEmailSender EmailSender, ILogger<ServerService> Logger)
        {
            _dbContext = DbContext;
            _mapp = Mapp;
            _emailSender = EmailSender;
            _logger = Logger;
        }

        public string GetActualization() {
            var Czasowe =_dbContext.CzasowoBd.Include(a => a.Kierowca).Where(a=>a.DataOddania == DateTime.Today);
            if (Czasowe.Count() > 0) {
                foreach (CzasowoOdebrane i in Czasowe) {
                    i.Kierowca.CzyOdebrano = false;
                    _dbContext.CzasowoBd.Remove(i);
                    //wyslanie maila i dodanie wpisu historii
                }
                _dbContext.SaveChanges();
                return DateTime.Today.Date+"-Uprawnienia do prowadzenia pojazdów zostały przywrócone " + Czasowe.Count() + " kierowcą";
            }
            return DateTime.Today.Date + "-Nie znaleziono kierowców do przywrócenia uprawnien";
        }

        public string SendReminder()
        {
            var NieOplacone = _dbContext.MandatyBd.Include(a => a.Kierowcy).Where(a => a.DataOplacenia < a.DataWydania);
            if (NieOplacone.Count() > 0)
            {
                foreach (Mandaty i in NieOplacone)
                {
                    var a = i.DataWydania.AddDays(7);
                    if (DateTime.Compare(a,DateTime.Today) == 1)
                        Send(i.Kierowcy.Email, "Przypomnienie o uregulowaniu zapłaty", string.Join(' ', i.Kierowcy.Imie, i.Kierowcy.Nazwisko)+" przypominamy o uregulowaniu mandatu otrzymanego "+i.DataWydania+". W przypadku nieuregulowania do dnia "+i.DataWydania.AddDays(7)+" zapłata zostanie pobrana przez odpowiedni urządz skarbowy lub komornika.");
                    else
                        Send(i.Kierowcy.Email, "Okres uregulowania przeminął", string.Join(' ', i.Kierowcy.Imie, i.Kierowcy.Nazwisko) + " minął okres 7 dni do zapłaty otrzymanego w dniu " + i.DataWydania + " mandatu. Zapłata zostanie pobrana przez odpowiedni urządz skarbowy lub komornika.");
                }
                return DateTime.Today.Date + "-Wiadomość przypominająca została przekazana " + NieOplacone.Count() + " kierowcą";
            }
            return DateTime.Today.Date + "-Nie znaleziono kierowców z mandatami";
        }
        public void DodanieWpisuHistorii(string tekst, Kierowcy tenkierowca)
        {
            Historia Hist = null;
            Hist = _dbContext.HistBd.FirstOrDefault(a => a.PESEL == tenkierowca.Pesel);
            if (Hist == null)
            {
                Hist = new Historia { PESEL = tenkierowca.Pesel, Imie = tenkierowca.Imie, Nazwisko = tenkierowca.Nazwisko, Opis = " " };
                _dbContext.HistBd.Add(Hist);
                _dbContext.SaveChanges();
            }

            var a = Hist.Opis;
            a = a.Insert(0, tekst);
            Hist.Opis = a;
            _dbContext.SaveChanges();
        }
        public string MandatActualization() {
            var Mandaty = _dbContext.MandatyBd.Include(a=>a.Kierowcy).Include(a=>a.Powod).Where(a=>DateTime.Compare(a.DataOplacenia,DateTime.Today.AddYears(-2))==0);
            foreach (Mandaty i in Mandaty) {
                DodanieWpisuHistorii("Kierowca otrzymał mandat za " + i.Powod.Tytul+"| ", i.Kierowcy);
                i.Kierowcy.Pkt -= i.Powod.Liczba_PKT;
                _dbContext.MandatyBd.Remove(i);
                _dbContext.SaveChanges();
            }
            // _dbContext.SaveChanges(); jest w wpisach do historii
            return DateTime.Today.Date + $"-Przedawniono {Mandaty.Count()} mandatów";
        }

        public async void Send(string toAddress, string title, string body)
        {
            await _emailSender.SendEmailAsync(toAddress, title, body);
        }
    }
}
