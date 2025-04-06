using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class Historia
    {
        public int ID { get; set; }
        public string PESEL { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Opis { get; set; }

    }
}

