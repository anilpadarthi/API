namespace SIMAPI.Data.Models.Login
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public int UserId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Expires { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
