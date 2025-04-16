using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class TimelyLost
    {
        public int ID { get; set; }
        public int DriverID { get; set; }
        public virtual Drivers Driver { get; set; }
        public DateTime DateOfTicket { get; set; }
        public DateTime DateOfLost { get; set; }
        public DateTime DateOfGiveBack { get; set; }

    }
}

