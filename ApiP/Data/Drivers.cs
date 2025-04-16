using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class Drivers
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
       // public string ActivationCodeHash { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateOfPassLicense { get; set; }
        public string Pesel { get; set; }
        public bool IsTimelyLost { get; set; }
        public bool IsPermanentLost { get; set; }
        public int Pkt { get; set; }
        public virtual List<Tickets> Tickets { get; set; }

    }
}

