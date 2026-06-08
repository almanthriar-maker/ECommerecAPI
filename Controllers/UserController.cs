using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_CommerceSystem_API.Controllers
{


    [ApiController]
    [Route("api/User")]
    public class UserController: ControllerBase
    {
        public ApplicationDbContext _context;
        public LoggingService _log;
        public UserController(ApplicationDbContext context, LoggingService log)

        {
            _context = context;
            _log = log;

        }



        [HttpGet("GetUserById")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserById(int id)
        {
            _log.Log($"GetUserById called. UserId={id}");

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                _log.Log($"User not found. UserId={id}");
                return NotFound("User not found");
            }

            var OutPutUsers = new List<UserOutputDTO>();

            OutPutUsers.Add(new UserOutputDTO
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            });

            _log.Log($"User returned successfully. UserId={id}");

            return Ok(OutPutUsers);
        }



        


    }
}
