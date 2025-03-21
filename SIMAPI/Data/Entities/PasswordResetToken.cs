namespace SIMAPI.Data.Entities
{
    public class PasswordResetToken
    {
        public int PasswordResetTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
