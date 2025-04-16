using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class History
    {
        public int ID { get; set; }
        public string PESEL { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }

    }
}

