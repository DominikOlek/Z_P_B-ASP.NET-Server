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
    [Route("/account")]
    public class UsersController : ControllerBase
    {
        public IUsersService _Service { get; }

        public UsersController(IUsersService service)
        {
            _Service = service;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterDto regDto) {
            _Service.Register(regDto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto dto) {
            if (dto.BadgeNumber != 0)
            {
                string token = _Service.GenerateJwt(dto);
                return Ok(token);
            }
            else
                return NotFound("Nie podano wszystkich danych");
        }

        [HttpGet("getUserRole")]
        [Authorize]
        public ActionResult<string> GetUserRole()
        {
            var a = User.FindFirst(a => a.Type == ClaimTypes.Role).Value.ToString();
            return Ok(a);
        }

        [HttpGet("refresh")]
        [Authorize]
        public ActionResult Ref()
        {
            var id = User.FindFirst(a => a.Type == ClaimTypes.NameIdentifier).Value;
            var name = User.FindFirst(a => a.Type == ClaimTypes.Name).Value;
            var email = User.FindFirst(a => a.Type == ClaimTypes.Email).Value;
            var rola = User.FindFirst(a => a.Type == ClaimTypes.Role).Value;
            string JWT =_Service.RefreshJWT(new Dictionary<string, string> { { "id", id }, { "name", name }, { "email", email }, { "role", rola } });
            return Ok(JWT);
        }

        [HttpGet("userinfo")]
        [Authorize]
        public ActionResult<UserInfoDto> GetInfo() {
            var id = int.Parse(User.FindFirst(a => a.Type == ClaimTypes.NameIdentifier).Value);
            return _Service.GetInformation(id);
        }

        [HttpGet("checkDriver")]
        public ActionResult<DriverInfoDto> GetInformation([FromBody]DriverInfoLog dto) {
            return _Service.GetDriverInfo(dto);
        }
    }
}