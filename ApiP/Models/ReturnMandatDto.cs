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
    public class ReturnMandatDto
    {
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public int PktZaMandat { get; set; }
        public int MWstrzymania { get; set; }
        public DateTime DataWydania { get; set; }
    }
}

