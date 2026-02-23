namespace SIMAPI.Business.Interfaces
{
    public interface IErrorLogService
    {
        Task LogErrorAsync(Exception ex, string optional = "");
    }
}
