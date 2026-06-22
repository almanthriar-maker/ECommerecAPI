using E_CommerceSystem_API.Services;
using ECommerecAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem_API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("GetUserById")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserById(int id)
        {
            var result = _userServices.GetUserById(id);

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
    }
}