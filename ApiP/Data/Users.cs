using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiP.Data{
    public class Users
    {
        public int ID { get; set; }
        public int Nr_Sluzbowy { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime Data_ur { get; set; }
        public string Pesel { get; set; }
        public bool Aktywny { get; set; } = false;
        public int RolaID { get; set; } = 7;
        public virtual Role Rola { get; set; }
    }
}

