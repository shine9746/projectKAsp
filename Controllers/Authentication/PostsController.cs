using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectK.Data;
using ProjectK.Models;
namespace ProjectK.Controllers.Authentication
{
    [ApiController]
    [Route("api/Posts")]
    public class PostsController : ControllerBase
    {
        private readonly prokjectKDbContext prokjectKDbContext;
        public PostsController(prokjectKDbContext projectDbContext) {
            prokjectKDbContext = projectDbContext;
        }
        [Authorize]
        [HttpPost("CreatePost")]
        public IActionResult CreatePost([FromBody] UserPosts UserPost)
        {
            var response = new {mode = 0,responseMessage = "Something went wrong"};
            if (UserPost != null)
            {
                var user = prokjectKDbContext.Users.FirstOrDefault(details => details.UserId == UserPost.UserId);
                if (user != null)
                {
                UserPost.postCreationDate = DateTime.Now;
                prokjectKDbContext.Posts.Add(UserPost);
                prokjectKDbContext.SaveChanges();
                response = new { mode = 1, responseMessage = "Post Created" };
                }
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {

            var response = new { mode = 0, responseMessage = "Something went wrong",postsList = new List<GetAllUsersPostsModel>() };
            
            var posts = await prokjectKDbContext.GetUserPosts.FromSqlRaw("CALL GetUserPosts()").AsNoTracking().ToListAsync();
            if (posts.Count > 0)
            {
                response = new { mode = 1, responseMessage = "Post fetched",postsList = posts };
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("UserPostInteraction")]
        public async Task<IActionResult> UserPostInteraction([FromBody] UserPostInteractionModel UserPostInteraction)
        {
            var response = new { mode = 0, responseMessage = "Something went wrong" };
            if (UserPostInteraction != null)
            {
                await prokjectKDbContext.Database.ExecuteSqlRawAsync("CALL UpdateUserPostInteraction({0}, {1}, {2});",
                    UserPostInteraction.UserId, UserPostInteraction.PostId, UserPostInteraction.InteractionMode);
                prokjectKDbContext.UserpostInteraction.Add(UserPostInteraction);
                    prokjectKDbContext.SaveChanges();
                    response = new { mode = 1, responseMessage = "" };
            }

            return Ok(response);
        }

    }

}
