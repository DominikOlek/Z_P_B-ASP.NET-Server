using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class Kierowcy
    {
        public int ID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
       // public string ActivationCodeHash { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime Data_ur { get; set; }
        public DateTime Data_orzymania { get; set; }
        public string Pesel { get; set; }
        public bool CzyOdebrano { get; set; }
        public bool CzyUtracil { get; set; }
        public int Pkt { get; set; }
        public virtual List<Mandaty> Mandaty { get; set; }

    }
}

