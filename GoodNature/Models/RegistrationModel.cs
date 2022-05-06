using System.ComponentModel.DataAnnotations;

namespace GoodNature.Models
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = Constants.ConfirmPasswordDisplayName)]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = Constants.FirstNameDisplayName)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = Constants.LastNameDisplayName)]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = Constants.Address1DisplayName)]
        public string Address1 { get; set; }       
        
        [Display(Name = Constants.Address2DisplayName)]
        public string Address2 { get; set; }
        
        [Required]
        [RegularExpression(Constants.PostcodeRegex, ErrorMessage = Constants.PostcodeErrorMessage)]
        public string Postcode { get; set; }
        
        [Required]
        [RegularExpression(Constants.PhoneNumberRegex, ErrorMessage = Constants.PhoneNumberErrorMessage)]
        [Display(Name = Constants.PhoneNumberDisplayName)]
        public string PhoneNumber { get; set; }
        
        public bool AcceptUserAgreement { get; set; }
        
        public string RegistrationInvalid { get; set; }
        
        public int CategoryId { get; set; }
    }
}
