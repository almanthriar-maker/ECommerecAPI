using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
using ECommerecAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_CommerceSystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;

        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }



        
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(UserRegisterDTO dto)
        {
            var result = _authServices.Register(dto);

            if (result == null)
                return BadRequest("Email already exists");

            return Ok(result);
        }


        [HttpPost("Login")]
        public IActionResult Login(string Email, string Password)
        {
            var result = _authServices.Login(Email, Password);

            if (result == null)
                return BadRequest("Invalid email or password");

            return Ok(result);
        }
    }
}
