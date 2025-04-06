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
    [ApiController]
    [Route("/app/account")]
    public class AppUsersController : ControllerBase
    {
        public IAppUsersService _Service { get; }
        public IUsersService _ServiceUsers { get; }

        public AppUsersController(IAppUsersService service, IUsersService usersService)
        {
            _Service = service;
            _ServiceUsers = usersService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterAppDto regDto)
        {
            if (_Service.Register(regDto))
                return Ok();
            else
                return Conflict();
        }

        [HttpPost("login")]
        public ActionResult<TokensModels> Login([FromBody] LoginDto dto)
        {
            TokensModels token = _Service.GenerateJwt(dto);
            if (token != null)
                return Ok(token);
            else
                return Conflict();
        }

        [HttpPost("refresh")]
        public ActionResult<TokensModels> Ref([FromBody] TokensModels tokens)
        {
            var Models =_Service.RefreshJWT(tokens);
            if (Models != null)
                return Ok(Models);
            else return Unauthorized();
        }
        [Authorize]
        [HttpPost("deleteRefresh")]
        public ActionResult Login()
        {
            var identificator = HttpContext.User.Identity as ClaimsIdentity;
            string mail = identificator.FindFirst(ClaimTypes.Email).Value;
            if (_Service.deleteRef(mail))
                return NoContent();
            else
                return BadRequest();
        }

        [HttpGet("userinfo")]
        [Authorize]
        public ActionResult<UserInfoDto> GetInfo()
        {
            var id = int.Parse(User.FindFirst(a => a.Type == ClaimTypes.NameIdentifier).Value);
            return null;//_Service.GetInformation(id);
        }

        [HttpGet("mandats")]
        [Authorize]
        public ActionResult<DriverInfoDto> GetDriverInfo()
        {
            var identificator = HttpContext.User.Identity as ClaimsIdentity;
            if (identificator != null)
            {
                return Ok(_ServiceUsers.GetDriverInfo(new DriverInfoLog
                {
                    Pesel = identificator.FindFirst(ClaimTypes.PrimarySid).Value,
                    Imie = identificator.FindFirst(ClaimTypes.Name).Value,
                    Nazwisko = identificator.FindFirst(ClaimTypes.Surname).Value,
                }));
            } 
            return null;
        }

    }
}