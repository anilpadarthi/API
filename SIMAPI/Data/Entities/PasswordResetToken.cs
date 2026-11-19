namespace SIMAPI.Data.Entities
{
    public class PasswordResetToken
    {
        public long PasswordResetTokenId { get; set; }

        public int UserId { get; set; }

        public string TokenHash { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }

        public DateTime? UsedDate { get; set; }
    }
}
