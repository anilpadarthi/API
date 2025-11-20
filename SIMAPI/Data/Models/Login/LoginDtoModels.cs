using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models.Login
{

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
    public class RefreshRequestDto { public string RefreshToken { get; set; } }
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public LoggedInUserDto UserDetails { get; set; }
    }


    public class LoggedInUserDto
    {
        public int userId { get; set; }
        public int userRoleId { get; set; }
        public string userName { get; set; }
        public string? email { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? mobile { get; set; }
        public string? designation { get; set; }
        public string? userImage { get; set; }
        public string? doj { get; set; }
        public UserRole userRole { get; set; }
    }
}
