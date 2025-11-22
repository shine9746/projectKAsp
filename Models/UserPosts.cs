using System.ComponentModel.DataAnnotations;

namespace ProjectK.Models
{
    public class UserPosts
    {
        [Key]
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostTitle { get; set; }= "";
        public string PostContent { get; set; } = "";
        public int PostLikes { get; set; } = 0;
        public int PostDisLikes { get; set; } = 0;
        public DateTime postCreationDate { get; set; }
    }
}
