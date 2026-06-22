using E_CommerceSystem_API;
using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;

namespace ECommerecAPI.Services
{
    public class AuthServices
    {
       private readonly ApplicationDbContext _context;
       private readonly JwtService _jwtService;
       private readonly emailSendingService _emailService;
       private readonly LoggingService _log;

       public AuthServices( ApplicationDbContext context,JwtService jwtService, emailSendingService emailService,LoggingService log)
       {
           _context = context;
           _jwtService = jwtService;
           _emailService = emailService;
           _log = log;
        }

        public object Register(UserRegisterDTO userDto)
          {
             _log.Log($"Register attempt for email: {userDto.Email}");

              bool emailExists = _context.Users.Any(u => u.Email == userDto.Email);

               if (emailExists)
                {
                    _log.Log($"Registration failed. Email already exists: {userDto.Email}");
                    return null;
                }

         User user = new User
           {
              Name = userDto.Name,
              Email = userDto.Email,
              Phone = userDto.Phone,
              Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
              Role = "user",
              CreatedAt = DateTime.Now
           };

              _context.Users.Add(user);
              _context.SaveChanges();

              _emailService.SendEmail(user.Email,$"Welcome {user.Name}","Thank you for registering.");

                _log.Log($"User registered successfully. UserId={user.UserId}");

                var token = _jwtService.GenerateToken(user);

                return new
                {
                    message = "Registration successful",
                    userId = user.UserId,
                    token
                };
            }

            public object Login(string email, string password)
            {
                _log.Log($"Login attempt: {email}");

                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    _log.Log($"Login failed. User not found: {email}");
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    _log.Log($"Login failed. Wrong password for: {email}");
                    return null;
                }

                var token = _jwtService.GenerateToken(user);

                _log.Log($"Login successful. UserId={user.UserId}");

                return token;
            }
        }
    }

