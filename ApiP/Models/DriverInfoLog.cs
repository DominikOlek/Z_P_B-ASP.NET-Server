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
    public class DriverInfoLog
    {
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Pesel { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }

    }
}

