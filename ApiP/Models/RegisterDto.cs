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
    public class RegisterDto
    {
        [Required]
        public int Nr_Sluzbowy { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Pesel { get; set; }
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }  
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime Data_ur { get; set; }

    }
}

