using Auth.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(UserDbContext context, IConfiguration configuration)
        {
            this._context = context;
            _configuration = configuration;
        }

        [HttpPost("CreateNewUser")]
        public IActionResult CreateUser(User user)
        {
            var userExistWithEmail = _context.users.SingleOrDefault(u => u.emailId == user.emailId);
            if (userExistWithEmail == null)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                user.password = hashedPassword;
                _context.users.Add(user);
                _context.SaveChanges();

                return Created("User Registered Success", new UserDto
                {
                    userId = user.userId,
                    emailId = user.emailId, 
                    createdDate = user.createdDate,
                    fullName = user.fullName,
                    mobileNo = user.mobileNo,
                    role = user.role
                });
            }
            else
            {
                return StatusCode(509, "Email Id already exist");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = _context.users.SingleOrDefault(u => u.emailId == userLogin.emailId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.password, user.password)) 
            {
                return StatusCode(401, "Wrong Credentials!!!");
            }
            else
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.Email, user.emailId),
                    new Claim(ClaimTypes.Role, user.role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                                issuer: _configuration["JwtSettings:Issuer"],
                                audience: _configuration["JwtSettings:Audience"],
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(60),
                                signingCredentials: creds);

                return StatusCode(200, new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user = new UserDto
                            {
                                userId = user.userId,
                                emailId = user.emailId,
                                createdDate = user.createdDate,
                                fullName = user.fullName,
                                mobileNo = user.mobileNo,
                                role = user.role
                            }
            });
            }
        }

        [Authorize(Roles = "user")]
        [HttpGet("getUsersForUser")]
        public IActionResult GetUsersForUser()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var list = _context.users
                .Where(u => u.role == "user").Select(user => new UserDto
            {
                userId = user.userId,
                emailId = user.emailId,
                createdDate = user.createdDate,
                fullName = user.fullName,
                mobileNo = user.mobileNo,
                role = user.role
            }).ToList();
            return Ok(list);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("getUsersForAdmin")]
        public IActionResult GetUsersForAdmin()
        {
            var list = _context.users.Select(user => new UserDto
            {
                userId = user.userId,
                emailId = user.emailId,
                createdDate = user.createdDate,
                fullName = user.fullName,
                mobileNo = user.mobileNo,
                role = user.role
            }).ToList();
            return Ok(list);
        }

    }
}
