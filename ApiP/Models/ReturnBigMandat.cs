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
    public class ReturnBigMandatDto
    {
        public int ID { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public int PktZaMandat { get; set; }
        public DateTime DataWydania { get; set; }
        public bool oplata { get; set; }
        public int kwota { get; set; }

        public string ImieKaranego { get; set; }
        public string NazwiskoKaranego { get; set; }

        public string ImieKarającego { get; set; }
        public string NazwiskoKarającego { get; set; }
        public string Nr_Karającego { get; set; }
    }
}

