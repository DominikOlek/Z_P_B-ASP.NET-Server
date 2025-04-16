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
            var Czasowe =_dbContext.TimelyBd.Include(a => a.Driver).Where(a=>a.DateOfGiveBack == DateTime.Today);
            if (Czasowe.Count() > 0) {
                foreach (TimelyLost i in Czasowe) {
                    i.Driver.IsTimelyLost = false;
                    _dbContext.TimelyBd.Remove(i);
                    //wyslanie maila i dodanie wpisu historii
                }
                _dbContext.SaveChanges();
                return DateTime.Today.Date+"-Uprawnienia do prowadzenia pojazdów zostały przywrócone " + Czasowe.Count() + " kierowcą";
            }
            return DateTime.Today.Date + "-Nie znaleziono kierowców do przywrócenia uprawnien";
        }

        public string SendReminder()
        {
            var NieOplacone = _dbContext.TicketsBd.Include(a => a.Driver).Where(a => a.DateOfPayment < a.DateOfTicket);
            if (NieOplacone.Count() > 0)
            {
                foreach (Tickets i in NieOplacone)
                {
                    var a = i.DateOfTicket.AddDays(7);
                    if (DateTime.Compare(a,DateTime.Today) == 1)
                        Send(i.Driver.Email, "Przypomnienie o uregulowaniu zapłaty", string.Join(' ', i.Driver.Name, i.Driver.LastName)+" przypominamy o uregulowaniu mandatu otrzymanego "+i.DateOfTicket+". W przypadku nieuregulowania do dnia "+i.DateOfTicket.AddDays(7)+" zapłata zostanie pobrana przez odpowiedni urządz skarbowy lub komornika.");
                    else
                        Send(i.Driver.Email, "Okres uregulowania przeminął", string.Join(' ', i.Driver.Name, i.Driver.LastName) + " minął okres 7 dni do zapłaty otrzymanego w dniu " + i.DateOfTicket + " mandatu. Zapłata zostanie pobrana przez odpowiedni urządz skarbowy lub komornika.");
                }
                return DateTime.Today.Date + "-Wiadomość przypominająca została przekazana " + NieOplacone.Count() + " kierowcą";
            }
            return DateTime.Today.Date + "-Nie znaleziono kierowców z mandatami";
        }
        public void DodanieWpisuHistorii(string tekst, Drivers tenkierowca)
        {
            History Hist = null;
            Hist = _dbContext.HistBd.FirstOrDefault(a => a.PESEL == tenkierowca.Pesel);
            if (Hist == null)
            {
                Hist = new History { PESEL = tenkierowca.Pesel, Name = tenkierowca.Name, LastName = tenkierowca.LastName, Description = " " };
                _dbContext.HistBd.Add(Hist);
                _dbContext.SaveChanges();
            }

            var a = Hist.Description;
            a = a.Insert(0, tekst);
            Hist.Description = a;
            _dbContext.SaveChanges();
        }
        public string MandatActualization() {
            var Mandaty = _dbContext.TicketsBd.Include(a=>a.Driver).Include(a=>a.Reason).Where(a=>DateTime.Compare(a.DateOfPayment,DateTime.Today.AddYears(-2))==0);
            foreach (Tickets i in Mandaty) {
                DodanieWpisuHistorii("Driver otrzymał mandat za " + i.Reason.Title+"| ", i.Driver);
                i.Driver.Pkt -= i.Reason.PointNumber;
                _dbContext.TicketsBd.Remove(i);
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
