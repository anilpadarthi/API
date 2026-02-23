namespace SIMAPI.Repository.Interfaces
{
    public interface IErrorLogRepository
    {
        Task LogErrorAsync(Exception ex, string optional = "");
    }
}
