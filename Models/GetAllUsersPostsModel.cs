using System.ComponentModel.DataAnnotations;

namespace ProjectK.Models
{
    public class GetAllUsersPostsModel
    {
        [Key]
        public int PostId { get; set; }
        public string PostTitle { get; set; } = "";
        public string PostContent { get; set; } = "";
        public int PostOwnerId { get; set; }
        public DateTime postCreationDate { get; set; }
        public string PostOwnerName { get; set; } = "";
        public string PostOwnerImage { get; set; } = "";

        public int? InteractionMode { get; set; }

        public int PostLikes { get; set; } = 0;

        public int PostDisLikes { get; set; } = 0;

        public int CommentsCount { get; set; } = 0;
    }
}
