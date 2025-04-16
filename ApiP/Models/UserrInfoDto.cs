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
        public string Name { get; set; }
        public string LastName { get; set; }
        public int BadgeNumber { get; set; }
        public string Email { get; set; }

    }
}

