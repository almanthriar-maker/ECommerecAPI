using E_CommerceSystem_API;
using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Services;
using E_CommerceSystem_API.Models;


namespace ECommerecAPI.Services
{
    public class UserServices
        {
            private readonly ApplicationDbContext _context;
            private readonly LoggingService _log;

            public UserServices(ApplicationDbContext context,LoggingService log)
            {
                _context = context;
                _log = log;
            }

            public List<UserOutputDTO> GetUserById(int id)
            {
                _log.Log($"GetUserById called. UserId={id}");

                var user = _context.Users.FirstOrDefault(u => u.UserId == id);

                if (user == null)
                {
                    _log.Log($"User not found. UserId={id}");
                    return null;
                }

                var outputUsers = new List<UserOutputDTO>
            {
                new UserOutputDTO
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone
                }
            };

                _log.Log($"User returned successfully. UserId={id}");

                return outputUsers;
            }
        }
    }


