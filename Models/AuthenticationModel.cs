using System.ComponentModel.DataAnnotations;

namespace ProjectK.Models
{
    public class AuthenticationModel
    {
        [Required(ErrorMessage = "Mail Required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string? Password { get; set; }    
    }
}
