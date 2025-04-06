using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class Taryfikator
    {
        public int ID { get; set; }
        public int Liczba_PKT { get; set; }
        public int Miesi¹ceWstrzymania { get; set; }
        public string Tytul { get; set; }

    }
}

