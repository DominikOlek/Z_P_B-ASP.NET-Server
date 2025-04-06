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
    public class AddMandatDto
    {
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Pesel { get; set; }
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Nr_tel { get; set; }
        [Required]
        public DateTime Data_ur { get; set; }
        public DateTime Data_orzymania { get; set; }
        [Required]
        public int IDPowodu { get; set; }
        [MaxLength(255)]
        public string OpisPowodu { get; set; }

    }
}

