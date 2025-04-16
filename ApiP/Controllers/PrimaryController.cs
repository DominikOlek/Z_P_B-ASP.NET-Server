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
    [Route("/drogowe/primary")]
    [ApiController]
    [Authorize(Roles = "Komendant")]
    public class PrimaryController : ControllerBase
    {
        public IPrimaryService _Service { get; }

        public PrimaryController(IPrimaryService service)
        {
            _Service = service;
        }

        [HttpPost("AllWaitUser")]
        public ActionResult<ReturnUserDto> GetAll([FromBody] Query search,[FromQuery] bool all) {
            return Ok(_Service.GetAll(search,all));
        }

        [HttpPost("SetUser")]
        public ActionResult<ReturnUserDto> UserSet([FromBody] SetUserDto dto)
        {
            _Service.SetUser(dto);
            return Ok();
        }

        [HttpPost("AllMandat")]
        public ActionResult<ReturnBigMandatDto> GetMandats([FromBody] Query search, [FromQuery] DateTime date,[FromQuery] bool oplata)
        {
            return Ok(_Service.GetMandats(search, date, oplata));
        }

        [HttpGet("GetRoles")]
        public ActionResult<Roles> GetRoles() {
            return Ok(_Service.GetRoles());
        }

        [HttpPost("AddTaryfikat")]
        public ActionResult AddTar([FromBody] AddTaryfikatorDto tar) {
            _Service.Addtar(tar);
            return Ok();
        }

        [HttpPost("RemoveTaryfikat")]
        public ActionResult RemTar([FromBody] int ID)
        {
            _Service.Removetar(ID);
            return Ok();
        }

        [HttpPost("ModifyTaryfikat")]
        public ActionResult MOdTar([FromBody] AddTaryfikatorDto tar,[FromQuery] int id)
        {
            _Service.EditTar(tar,id);
            return Ok();
        }

        [HttpPost("AllTar")]
        public ActionResult<PageResult<Taryfikator>> GetTaryfik([FromBody] Query search, [FromQuery] int pkt)
        {
            if (pkt < 0)
                throw new Exception("B³êdne dane");
            return Ok(_Service.GetTar(search, pkt));
        }
    }
}