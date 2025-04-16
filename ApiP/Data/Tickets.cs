using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class Tickets
    {
        public int ID { get; set; }
        public int ReasonID { get; set; }
        public virtual Taryfikator Reason { get; set; }
        public string Description { get; set; }
        public DateTime DateOfTicket { get; set; }
        public DateTime DateOfPayment { get; set; }

        public int DriverID { get; set; }
        public virtual Drivers Driver { get; set; }

        public int CopID { get; set; }
        public virtual Users Cop { get; set; }
        public int Cost { get; set; }
    }
}

