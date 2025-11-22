
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectK.Models
{
    public class UserModel
    {
        [Key]
        public int? UserId { get; set; }
        [Required(ErrorMessage = "User Name Required")]
        public string UserName { get; set; } = "";
        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; } = "";
        [Required(ErrorMessage = "Phone Number Required")]
        public string PhoneNumber { get; set; } = "";
        [Required(ErrorMessage =  "Gender Required")]
        public int Gender { get; set; }
        [Required(ErrorMessage ="Email Required")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage ="Password Required")]
        public string Password { get; set; } = "";
        [NotMapped]
        public string File { get; set; } = "";
        public string FilePath { get; set; } = "";


    }
}
