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
    public class Query
    {
        public string Search { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Sort { get; set; }

    }
}

