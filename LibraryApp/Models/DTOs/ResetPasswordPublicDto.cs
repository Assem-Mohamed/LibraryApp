namespace LibraryApp.Models.DTOs
{
    public class ResetPasswordPublicDto
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
