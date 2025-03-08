using MagicOnion;

namespace MyApp.Shared.Services
{
    /// <summary>
    /// Client -> Server API
    /// </summary>
    public interface IChatService : IService<IChatService>
    {
        UnaryResult GenerateException(string message);
        UnaryResult SendReportAsync(string message);
    }
}