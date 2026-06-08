using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
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
    [Route("api/[Controller]")]
    public class AuthController: ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public emailSendingService _emailService;
        public JwtService _jwtService;
        public LoggingService _log;

        public AuthController(ApplicationDbContext context, IConfiguration config, emailSendingService emailService, JwtService jwtService, LoggingService log)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
            _jwtService = jwtService;
            _log = log;
        }


        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(UserRegisterDTO userDto)
        {

            _log.Log($"Register attempt for email: {userDto.Email}");
            User u = new User();
            u.Name = userDto.Name;
            u.Email = userDto.Email;
            u.Phone = userDto.Phone;
            u.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            u.Role = "user";
            u.CreatedAt = DateTime.Now;

            _context.Users.Add(u);
            _context.SaveChanges();

            _log.Log($"User registered successfully. UserId={u.UserId}");


            _emailService.SendEmail(userDto.Email, "Welcome!" + userDto.Name, "Thank you for registering.");

            return Ok("User registered succefully with ID = " + u.UserId);

        }


        [HttpPost("Login")]
        public IActionResult Login(string Email, string Password)
        {
            _log.Log($"Login attempt: {Email}");

            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                _log.Log($"Login failed. User not found: {Email}");
                return BadRequest("Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                _log.Log($"Login failed. Wrong password for: {Email}");
                return BadRequest("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);

            _log.Log($"Login successful. UserId={user.UserId}");

            return Ok(token);
        }






    }
}
