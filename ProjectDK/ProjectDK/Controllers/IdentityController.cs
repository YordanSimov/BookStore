using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using System.Security.Claims;
using System.Text;
using ProjectDK.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using ProjectDK.Models.Models.Users;
using ProjectDK.BL.Interfaces;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IConfiguration configuration;
        // private readonly IEmployeeService employeeService;
        private readonly IIdentityService identityService;

        public IdentityController(IConfiguration configuration, IIdentityService identityService)
        {
            this.configuration = configuration;
            this.identityService = identityService;
        }
        [AllowAnonymous]
        [HttpPost(nameof(CreateUser))]

        public async Task<IActionResult> CreateUser([FromBody] UserInfo user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                return BadRequest("Username or password is invalid");

            var result = await identityService.CreateAsync(user);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest != null && !string.IsNullOrEmpty(loginRequest.UserName) && !string.IsNullOrEmpty(loginRequest.Password))
            {
                var user = await identityService.CheckUserAndPassword(loginRequest.UserName, loginRequest.Password);

                if (user != null)
                {
                    var userRoles = await identityService.GetUserRoles(user);
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,configuration.GetSection("Jwt:Subject").Value),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToString()),
                        new Claim("UserId",user.UserId.ToString()),
                        new Claim("DisplayName",user.DisplayName ?? String.Empty),
                        new Claim("Email",user.Email ?? String.Empty),
                        new Claim("UserName",user.UserName ?? String.Empty),
                        new Claim("View","View"),
                        new Claim("Test","Test")
                    };

                    foreach (var role in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                        configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                return BadRequest("Invalid credentials");
            }
            return NotFound("Missing username and/or password");
        }
    }
}
