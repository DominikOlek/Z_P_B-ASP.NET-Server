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
    public class UserInfoDto
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Nr_Sluzbowy { get; set; }
        public string Email { get; set; }

    }
}

