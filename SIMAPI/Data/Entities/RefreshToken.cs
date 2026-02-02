namespace SIMAPI.Data.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public string Token { get; set; }
        public string Type { get; set; }
        public DateTime Expires { get; set; }

        public bool IsExpired { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }
    }
}
