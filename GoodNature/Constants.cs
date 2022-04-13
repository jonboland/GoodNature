namespace GoodNature
{
    public class Constants
    {
        // Various        
        public const string TitleLengthErrorMessage = "The title's length must be between two and 200 characters.";

        public const string ThumbnailImagePathDisplayName = "Thumbnail Path";
        public const string MediaTypeDisplayName = "Media Type";

        // CategoryItem Entity
        public const string MissingMediaTypeErrorMessage = "Please select an option from the {0} dropdown list.";

        public const string DateTimeItemReleasedDisplayName = "Release Date";

        // Content Entity
        public const string HTMLContentDisplayName = "HTML Content";
        public const string VideoLinkDisplayName = "Video Link";

        // LoginModel
        public const string RememberMeDisplayName = "Remember Me";

        // RegistrationModel
        public const string ConfirmPasswordDisplayName = "Confirm Password";
        public const string FirstNameDisplayName = "First Name";
        public const string LastNameDisplayName = "Last Name";
        public const string Address1DisplayName = "Address Line 1";
        public const string Address2DisplayName = "Address Line 2";
        public const string PhoneNumberDisplayName = "Phone Number";

        public const string PostcodeRegex = "^[a-zA-Z]{1,2}[0-9][0-9A-Za-z]{0,1} {0,1}[0-9][A-Za-z]{2}$";
        public const string PhoneNumberRegex = @"(\s*\(?0\d{4}\)?\s*\d{6}\s*)|(\s*\(?0\d{3}\)?\s*\d{3}\s*\d{4}\s*)";

        // UserAuthController
        public const string LoginFailedErrorMessage = "Oops! Login details are incorrect";
    }
}
