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
    public class ReturnBigMandatDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PointsNumber { get; set; }
        public DateTime DateOfTicket { get; set; }
        public bool isCost { get; set; }
        public int Cost { get; set; }

        public string NameOfRecipient { get; set; }
        public string LastNameOfRecipient { get; set; }

        public string NameOfCop { get; set; }
        public string LastNameOfCop { get; set; }
        public string BadgeNumberOfCop { get; set; }
    }
}

