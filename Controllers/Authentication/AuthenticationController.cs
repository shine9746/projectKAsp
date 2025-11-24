using Microsoft.AspNetCore.Mvc;
using ProjectK.Data;
using ProjectK.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ProjectK.Controllers.Authentication
{
        [ApiController]
        [Route("api/Authentication")]
        public class AuthenticationController : ControllerBase
        {
        private readonly prokjectKDbContext prokjectKDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationController(prokjectKDbContext projectDbContext, IConfiguration configuration)
        {
            prokjectKDbContext = projectDbContext;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Authenticate([FromBody] AuthenticationModel AuthDetails)
        {
            string jwtToken = "";
            var responseMessage = new { mode = 0, responseMessage = "Something went wrong", jwtToken = "" };
            if(AuthDetails == null)
            {
               responseMessage = new { mode = 0, responseMessage = "Please contact technical team", jwtToken = jwtToken };
            }
            else
            {
                var users = prokjectKDbContext?.Users?.ToList();
                if(users?.Count > 0)
                {
                int index = users.FindIndex(userDetails => userDetails.Email == AuthDetails.Email && userDetails.Password == AuthDetails.Password);
                    if(index != -1)
                    {
                        jwtToken = GenerateJwtToken(users[index]);
                        responseMessage = new { mode = 1, responseMessage = "Login Successfully", jwtToken = jwtToken };
                    }
                    else
                    {
                        responseMessage = new { mode = 0, responseMessage = "Email and password not matching", jwtToken = jwtToken };
                    }
                }
            }
             return Ok(responseMessage);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] UserModel UserDetails)
        {
            var response = new { mode = 0, responseMessage = "Something went wrong" };
            if (UserDetails == null)
            {
                response = new { mode = 0, responseMessage = "Details required"};
            }
            else if (!ModelState.IsValid)
            {
                string[] fields = { "UserName", "PhoneNumber", "Gender", "Email", "Password", "Address" };
                var errors = fields.Where((field) => ModelState.ContainsKey(field))
                  .SelectMany(field => ModelState[field]!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                if (errors.Count > 0)
                {
                for (int i = 0; i < errors.Count; i++)
                {
                    response = new { mode = 0, responseMessage = errors[i] };
                    break;
                }
                }
            }
            else
            {
                var user = prokjectKDbContext.Users.FirstOrDefault(details => details.Email == UserDetails.Email || details.PhoneNumber == UserDetails.PhoneNumber);
                if (user != null)
                {
                    response = new { mode = 0, responseMessage = "User already exists with same email or password"};
                }
                else
                {
                    if (!string.IsNullOrEmpty(UserDetails.File))
                    {
                        byte[] fileBytes = Convert.FromBase64String(UserDetails.File);
                        string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(),"Images", "UserFiles");
                        if (!Directory.Exists(uploadFolder))
                            Directory.CreateDirectory(uploadFolder);
                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string filePath = Path.Combine(uploadFolder, fileName);

                        await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                        string relativePath = $"/Images/UserFiles/{fileName}";
                        UserDetails.FilePath = relativePath;
                    }
                    UserDetails.File = "";
                    prokjectKDbContext.Users.Add(UserDetails);
                    prokjectKDbContext.SaveChanges();
                    response = new { mode = 1, responseMessage = "User Registered" };

                }          
            }
            return Ok(response);
        }

        private string GenerateJwtToken(UserModel userDetails)
        {
            var jwtSettings = _configuration.GetSection("Jwt") ?? null;
            if (jwtSettings == null)
                throw new Exception("JWT configuration missing!");

            var keyString = jwtSettings["Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            string userJson = JsonSerializer.Serialize(userDetails);
            string id = userDetails?.UserId?.ToString() ?? "";
            var claims = new[]
 {
    new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim("userId", id.ToString()),
    new Claim("userName", userDetails?.UserName ?? ""),
    new Claim("email", userDetails?.Email ?? ""),
    new Claim("phoneNumber", userDetails?.PhoneNumber ?? ""),
    new Claim("gender", userDetails!.Gender.ToString(), ClaimValueTypes.Integer),
    new Claim("filePath", userDetails?.FilePath ?? ""),
     new Claim("address", userDetails?.Address ?? "")
};
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "")),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
