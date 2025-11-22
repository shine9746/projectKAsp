using Microsoft.AspNetCore.Mvc;
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

    }
}
