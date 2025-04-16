using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using ApiP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiP.Data;

namespace ApiP.Controllers
{
    [Route("/road")]
    [ApiController]
    [Authorize(Roles = "Ruch_Drogowy,Komendant")]
    public class MainController : ControllerBase
    {
        public IMainService _Service { get; }

        public MainController(IMainService service)
        {
            _Service = service;
        }

        [HttpPost("add")]
        public ActionResult<string[]> GetA([FromBody] AddTicketDto mandat)
        {
            var Wystawiajacy = int.Parse(User.FindFirst(a => a.Type == ClaimTypes.NameIdentifier).Value);
            string[] ret = _Service.AddTicketF(mandat,Wystawiajacy); 
            return Ok(ret);
        }

        [HttpPost("driver")]
        public ActionResult<ReturnDriverDto> GetKierowcaInforamtion([FromBody] string PESEL)
        {
            if (PESEL.Length != 11)
                return BadRequest("PESEL POWINNIEN MIEÆ 11 ZNAKÓW");
            var ret = _Service.DriverInfoF(PESEL);     
            return Ok(ret);
        }

        [HttpGet("payment/{id}")]
        public ActionResult<string> Oplata([FromRoute] string id)
        {
            _Service.PaymentF(id);
            return Ok("OPLACONO");
        }

        [HttpPost("driverTicket")]
        public ActionResult<ReturnTicketDto[]> GetKierowcaMandats([FromBody] string PESEL)
        {
            if (PESEL.Length != 11)
                return BadRequest("PESEL POWINNIEN MIEÆ 11 ZNAKÓW");
            var ret = _Service.DriverTickets(PESEL);
            return Ok(ret);
        }

        [HttpPost("driverHist")]
        public ActionResult<string> GetHistory([FromBody] string PESEL)
        {
            if (PESEL.Length != 11)
                return BadRequest("PESEL POWINNIEN MIEÆ 11 ZNAKÓW");
            var ret = _Service.History(PESEL);
            return Ok(ret);
        }

        [HttpGet("GetTaryfikator")]
        public ActionResult<Taryfikator> GetTar() {
            return Ok(_Service.GetTar());
        }

    }
}