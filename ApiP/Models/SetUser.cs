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
    public class SetUserDto
    {
        public int Nr_Sluzbowy { get; set; }
        public int Rola { get; set; } = 7;
    }
}

