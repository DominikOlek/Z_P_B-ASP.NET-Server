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
    public class ReturnTicketDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Pkt { get; set; }
        public int MonthsOfLost { get; set; }
        public DateTime DateOfTicket { get; set; }
    }
}

