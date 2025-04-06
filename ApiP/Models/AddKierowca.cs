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
    public class AddKierowcaDto
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(15)]
        public string Imie { get; set; }
        [Required]
        [MaxLength(20)]
        public string Nazwisko { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(9)]
        public string Nr_tel { get; set; }
        [Required]
        public DateTime Data_ur { get; set; }
        [Required]
        [StringLength(11)]
        public string Pesel { get; set; }

    }
}

