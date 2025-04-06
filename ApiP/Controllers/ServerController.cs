using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using ApiP.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiP.Controllers
{
    [ApiController]
    [Route("/server")]
    public class ServerControler : ControllerBase
    {
        public IServerService _Service { get; }

        public ServerControler(IServerService service)
        {
            _Service = service;
        }

        [HttpGet("aktualizacja")]
        public ActionResult<string> Aktualizacja() {
            return Ok(_Service.GetActualization());
        }

        [HttpGet("przypominanie")]
        public ActionResult<string> Przypomnienie()
        {
            return Ok(_Service.SendReminder());
        }

        [HttpGet("aktualizacjamandatow")]
        public ActionResult<string> Aktyalizacjamandat()
        {
            return Ok(_Service.MandatActualization());
        }
    }
}