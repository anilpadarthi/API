using SIMAPI.Data.Models.Login;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMAPI.Data.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public string TokenHash { get; set; } = null!; // store HASH of token
        public string? JwtId { get; set; }              // optional link to JWT's jti
        public int UserId { get; set; }
        [NotMapped]
        public LoggedInUserDto? User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public string? ReplacedByTokenHash { get; set; } // for rotation tracking
    }
}
