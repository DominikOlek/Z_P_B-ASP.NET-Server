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
    public class AddTaryfikatorDto
    {
        [Required]
        public int Liczba_PKT { get; set; }
        public int MiesiąceWstrzymania { get; set; } = 0;
        [Required]
        public string Tytul { get; set; }

    }
}

