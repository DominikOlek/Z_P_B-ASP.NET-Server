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
    public class AddTicketDto
    {
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Pesel { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Nr_tel { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public DateTime DateOfTicket { get; set; }
        [Required]
        public int ReasonID { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }

    }
}

