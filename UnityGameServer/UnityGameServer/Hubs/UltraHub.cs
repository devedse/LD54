using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace UnityGameServer.Hubs
{
    public class UltraHub : Hub
    {
        private static readonly ConcurrentBag<string> roomNames = new ConcurrentBag<string>();
        private static readonly ConcurrentDictionary<string, string> connectionToRoomMap = new ConcurrentDictionary<string, string>();

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
            await Clients.Caller.SendAsync("Server_ReceiveRoomName", roomName);

            Console.WriteLine($"Created room {roomName} for {Context.ConnectionId}");
        }

        public async Task Client_JoinRoom(string roomName, string clientName)
        {
            Console.WriteLine($"Received join room {roomName} from {Context.ConnectionId}");

            connectionToRoomMap[Context.ConnectionId] = clientName;
            await Clients.Group(roomName).SendAsync("Server_ReceiveJoinRoom", clientName);
        }

        public Task Client_SendButtonPress(int button)
        {
            Console.WriteLine($"Received button press {button} from {Context.ConnectionId}");

            //Get the room name associated with the current connection
            if (connectionToRoomMap.TryGetValue(Context.ConnectionId, out string roomName))
            {
                //Send to clients in the room (group)
                return Clients.Group(roomName).SendAsync("Server_ReceiveButtonPress", button);
            }
            return Task.CompletedTask;
        }
    }
}
