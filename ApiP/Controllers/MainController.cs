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
    [Route("/drogowe")]
    [ApiController]
    [Authorize(Roles = "Ruch_Drogowy,Komendant")]
    public class MainController : ControllerBase
    {
        public IMainService _Service { get; }

        public MainController(IMainService service)
        {
            _Service = service;
        }

        [HttpPost("dodaj")]
        public ActionResult<string[]> GetA([FromBody] AddMandatDto mandat)
        {
            var Wystawiajacy = int.Parse(User.FindFirst(a => a.Type == ClaimTypes.NameIdentifier).Value);
            string[] ret = _Service.DodanieMandatu(mandat,Wystawiajacy); 
            return Ok(ret);
        }

        [HttpPost("kierowca")]
        public ActionResult<ReturnKierowcaDto> GetKierowcaInforamtion([FromBody] string PESEL)
        {
            if (PESEL.Length != 11)
                return BadRequest("PESEL POWINNIEN MIEÆ 11 ZNAKÓW");
            var ret = _Service.InformacjeKierowcy(PESEL);     
            return Ok(ret);
        }

        [HttpGet("Oplacenie/{id}")]
        public ActionResult<string> Oplata([FromRoute] string id)
        {
            _Service.Oplacanie(id);
            return Ok("OPLACONO");
        }

        [HttpPost("MandatyKier")]
        public ActionResult<ReturnMandatDto[]> GetKierowcaMandats([FromBody] string PESEL)
        {
            if (PESEL.Length != 11)
                return BadRequest("PESEL POWINNIEN MIEÆ 11 ZNAKÓW");
            var ret = _Service.MandatyKierowcy(PESEL);
            return Ok(ret);
        }

        [HttpPost("HistoriaKier")]
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