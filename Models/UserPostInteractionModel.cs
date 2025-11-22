using System.ComponentModel.DataAnnotations;
namespace ProjectK.Models
{
    public class UserPostInteractionModel
    {
        [Key]
        public int UserInteractionId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int InteractionMode { get; set; }
    }
}
