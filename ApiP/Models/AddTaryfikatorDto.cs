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
        public int PointNumber { get; set; }
        public int MonthsOfLost { get; set; } = 0;
        [Required]
        public string Title { get; set; }

    }
}

