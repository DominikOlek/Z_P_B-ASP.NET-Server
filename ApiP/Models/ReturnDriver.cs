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
    public class ReturnDriverDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateOfPassLicense { get; set; }
        public string Pesel { get; set; }
        public bool IsTimelyLost { get; set; }
        public bool IsPermanentLost { get; set; }
        public int Pkt { get; set; }

    }
}

