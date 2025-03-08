using System.Threading.Tasks;
using MyApp.Shared.MessagePackObjects;

namespace MyApp.Shared.Hubs
{
    /// <summary>
    /// Server -> Client API
    /// </summary>
    public interface IChatHubReceiver
    {
        void OnJoin(string name);

        void OnLeave(string name);

        void OnSendMessage(MessageResponse message);


        Task<string> HelloAsync(string name, int age);
    }
}