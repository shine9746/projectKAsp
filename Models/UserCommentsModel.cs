using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace ProjectK.Models
{
    public class UserCommentsModel
    {
        [Key]
        public int? CommentId { get; set; }
        public int UserId { get; set; }

        public int PostId { get; set; }
        public string CommentContent { get; set; } = "";

        public DateTime commentCreationDate { get; set; }

    }

    public class GetComments
    {
        public int PostId { get; set; }

    }

    public class GetCommentsModel
    {
        public int? CommentId { get; set; }
        public int PostId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        public string UserImage { get; set; } = "";

        public string CommentContent { get; set; } = "";
        public DateTime CommentCreationDate { get; set; }

    }

    public class CommentsResponse
    {
        public int Mode { get; set; }
        public string ResponseMessage { get; set; } = "";
        public List<GetCommentsModel>? Comments { get; set; }
    }

    public class CommentDetailsModal
    {
        public int CommentId { get; set; }
    }

    [Keyless]
    public class DeleteResultDto
    {
        public int AffectedRows { get; set; }
    }
}
