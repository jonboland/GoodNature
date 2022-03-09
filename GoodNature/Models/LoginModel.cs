using System.ComponentModel.DataAnnotations;

namespace GoodNature.Models
{
    public class LoginModel
    {
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = Constants.RememberMeDisplayName)]
        public bool RememberMe { get; set; }
        public string LoginInvalid { get; set; }
    }
}
