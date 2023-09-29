using Microsoft.AspNetCore.SignalR;

namespace UnityGameServer.Hubs
{
    public class UltraHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"Received a message from: {user} with the message: {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
