using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectK.Data;
using ProjectK.Models;

namespace ProjectK.Controllers.Authentication
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly prokjectKDbContext prokjectKDbContext;
        public UserController(prokjectKDbContext projectDbContext)
        {
            prokjectKDbContext = projectDbContext;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var response = new { mode = 0, message = "No users found", usersList = new List<UserModel>() };
            var users = prokjectKDbContext.Users.ToList();
            if (users.Count > 0)
            {
                response = new { mode = 1, message = "No users found", usersList = users };
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel UserDetails)
        {
            var response = new { mode = 0, message = "Something went wrong."};
            var user = await prokjectKDbContext.Users.FindAsync(UserDetails?.UserId);
            if (UserDetails != null || user != null)
            {
                var existingUser = prokjectKDbContext.Users.FirstOrDefault(details => (details.Email == UserDetails!.Email || details.PhoneNumber ==
                UserDetails.PhoneNumber) &&  details.UserId != UserDetails!.UserId);
                if (existingUser != null)
                {
                    response = new { mode = 0, message = "User exist with same email or phone number" };
                }
                else
                {
                    user!.UserName = UserDetails!.UserName; 
                    user.Email = UserDetails.Email; 
                    user.PhoneNumber = UserDetails.PhoneNumber;
                    user.Address = UserDetails.Address;
                    user.Gender = UserDetails.Gender;

                    if (!string.IsNullOrEmpty(UserDetails.File))
                    {
                        byte[] fileBytes = Convert.FromBase64String(UserDetails.File);
                        string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images", "UserFiles");
                        if (!Directory.Exists(uploadFolder))
                            Directory.CreateDirectory(uploadFolder);
                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string filePath = Path.Combine(uploadFolder, fileName);

                        await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                        string relativePath = $"/Images/UserFiles/{fileName}";
                        user.FilePath = relativePath;
                    }
                    UserDetails.File = "";
                    prokjectKDbContext.SaveChanges();
                    response = new { mode = 1, message = "User updated." };

                }
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(DeleteUserModel UserDetails)
        {
            var response = new { mode = 0, message = "Something went wrong." };
            var user = await prokjectKDbContext.Users.FindAsync(UserDetails.UserId);
            if (user != null)
            {
                if(user.Password != UserDetails.Password)
                {
                    response = new { mode = 0, message = "Wrong password!" };
                }
                else
                {
                    prokjectKDbContext.Users.Remove(user);
                    await prokjectKDbContext.SaveChangesAsync();
                    response = new { mode = 1, message = "User deleted" };
                }
            }
            return Ok(response);
        }

    }
}
