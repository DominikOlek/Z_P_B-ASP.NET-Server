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
    public class LoginDto
    {
        public int Nr_Sluzbowy { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

    }
}

