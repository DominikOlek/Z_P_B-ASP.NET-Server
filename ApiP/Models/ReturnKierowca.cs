using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using System.ComponentModel.DataAnnotations;

namespace ApiP.Models
{
    public class ReturnKierowcaDto
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime Data_ur { get; set; }
        public DateTime Data_orzymania { get; set; }
        public string Pesel { get; set; }
        public bool CzyOdebrano { get; set; }
        public bool CzyUtracil { get; set; }
        public int Pkt { get; set; }

    }
}

