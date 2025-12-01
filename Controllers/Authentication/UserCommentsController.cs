using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using ProjectK.Data;
using ProjectK.Models;

namespace ProjectK.Controllers.Authentication
{
    [ApiController]
    [Route("api/Comments")]
    public class UserCommentsController : ControllerBase
    {
        private readonly prokjectKDbContext prokjectKDbContext;
        public UserCommentsController(prokjectKDbContext projectDbContext)
        {
            prokjectKDbContext = projectDbContext;
        }

        private int GetUserIdFromJwt()
        {
            return int.Parse(User.FindFirst("userId")!.Value);
        }

        [Authorize]
        [HttpPost("UserComments")]
        public IActionResult UserComments([FromBody] UserCommentsModel UserComments)
        {
            var response = new { mode = 0, responseMessage = "Something went wrong" };
            if (UserComments != null)
            {
                UserComments.commentCreationDate = DateTime.Now;
                prokjectKDbContext.UserComments.Add(UserComments);
                prokjectKDbContext.SaveChanges();
                response = new { mode = 1, responseMessage = "Comment posted" };
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("GetPostComments")]
        public async Task<IActionResult> GetPostComments([FromBody] GetComments Post)
        {
            var response = new { mode = 0, responseMessage = "Something went wrong", comments = new List<GetCommentsModel>() };
            if (Post != null)
            {
                var comments = await prokjectKDbContext.GetComments.FromSqlRaw("CALL GetCommentsByPost({0})", Post.PostId).ToListAsync();
                if (comments != null)
                {
                    response = new { mode = 1, responseMessage = "Something went wrong", comments = comments };
                }
                else
                {
                    response = new { mode = 0, responseMessage = "", comments = new List<GetCommentsModel>() };
                }

            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("DeleteComments")]
        public async Task<IActionResult> DeleteComments([FromBody] int? commentId)
        {
            var response = new { mode = 0, responseMessage = "Something went wrong" };
            if (commentId != null)
            {

                try { 
                int userId = GetUserIdFromJwt();
                    await prokjectKDbContext.Database.ExecuteSqlRawAsync("CALL DeleteUserComments(@commentId, @userId)", 
                        new MySqlParameter("@commentId", commentId), new MySqlParameter("@userId", userId));
                    response = new { mode = 1, responseMessage = "Comment Deleted Successfully" };
                }
                catch (MySqlException)
                {
                    response = new { mode = 0, responseMessage = "Please contact technical support" };
                }
                


  
                    
            
            }
            return Ok(response);

        }



    }
}
