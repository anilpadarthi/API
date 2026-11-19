namespace SIMAPI.Data.Models.ResetPassword
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
        public string Mode { get; set; }
    }

    public class ResetPasswordRequestDto
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class VerifyResetTokenDto
    {
        public string Token { get; set; }
    }
}
