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
    public class ReturnUserDto
    {
        public int Nr_Sluzbowy { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime Data_ur { get; set; }
        public string Pesel { get; set; }
        public bool Aktywny { get; set; }
        public string Rola { get; set; }
    }
}

