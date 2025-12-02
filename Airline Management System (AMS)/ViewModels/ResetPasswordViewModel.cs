namespace Airline_Management_System__AMS_.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        public string VerificationCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
