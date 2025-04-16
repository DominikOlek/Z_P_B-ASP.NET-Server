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
        public int BadgeNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime BirthDate { get; set; }
        public string Pesel { get; set; }
        public bool IsActive { get; set; } = false;
        public int RoleID { get; set; } = 3;
        public virtual Roles Role { get; set; }
    }
}

