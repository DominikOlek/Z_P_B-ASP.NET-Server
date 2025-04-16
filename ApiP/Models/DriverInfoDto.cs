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
    public class DriverInfoDto
    {
        public ReturnTicketDto[] Tickets { get; set; }
        public string[] History { get; set; }
        public string ImportantInfo { get; set; } = " ";
    }
}

