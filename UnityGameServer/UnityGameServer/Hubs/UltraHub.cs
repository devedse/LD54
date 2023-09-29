using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace UnityGameServer.Hubs
{
    public class UltraHub : Hub
    {
        private static readonly ConcurrentBag<string> roomNames = new ConcurrentBag<string>();

        public string CreateUniqueRoomName()
        {
            string roomName;
            do
            {
                roomName = CreateRandomRoomName();
            } while (roomNames.Contains(roomName));

            roomNames.Add(roomName);
            return roomName;
        }

        private string CreateRandomRoomName()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task Server_CreateRoom(dynamic a)
        {
            string roomName = CreateUniqueRoomName();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            Console.WriteLine($"Created room {roomName} for {Context.ConnectionId}");
            await Clients.Caller.SendAsync("Server_ReceiveRoomName", roomName);
        }

        public Task Client_JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task Client_SendButtonPress(int button)
        {
            return Clients.Others.SendAsync("Server_ReceiveButtonPress", button);
        }
    }
}
