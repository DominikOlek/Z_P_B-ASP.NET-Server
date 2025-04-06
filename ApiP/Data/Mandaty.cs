using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class Mandaty
    {
        public int ID { get; set; }
        public int PowodID { get; set; }
        public virtual Taryfikator Powod { get; set; }
        public string Opis { get; set; }
        public DateTime DataWydania { get; set; }
        public DateTime DataOplacenia { get; set; }

        public int KierowcyID { get; set; }
        public virtual Kierowcy Kierowcy { get; set; }

        public int PrzezID { get; set; }
        public virtual Users Przez { get; set; }
        public int Kwota { get; set; }
    }
}

