using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using ApiP.Exceptions;

namespace ApiP
{
    public class AuthencticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpireMinutes { get; set; }
        public string JwtIssuer { get; set; }
        public int RefreshTokenExpireDays { get; set; }
    }
}
