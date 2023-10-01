using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace UnityGameServer.Hubs
{
    public class Room
    {
        public required string ConnectionIdServer { get; set; }

        /// <summary>
        /// ConnectionId, ClientName
        /// </summary>
        public ConcurrentDictionary<string, string> ConnectionIdsClients { get; set; } = new ConcurrentDictionary<string, string>();
    }

    public class UltraHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> ConnectionIdToPlayerNameMapping = new ConcurrentDictionary<string, string>();
        public static readonly ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();
        private static readonly ConcurrentDictionary<string, string> ConnectionToRoomMap = new ConcurrentDictionary<string, string>();

        public static object _clientNameDuplicationLockject = new object();

        public UltraHub()
        {
            Rooms.TryAdd("BLAH", new Room() { ConnectionIdServer = "BLAH" });
        }

        public string CreateUniqueRoomName()
        {
            string roomName;
            do
            {
                roomName = CreateRandomRoomName();
            } while (!Rooms.TryAdd(roomName, new Room() { ConnectionIdServer = Context.ConnectionId }));

            ConnectionToRoomMap[Context.ConnectionId] = roomName;

            return roomName;
        }

        private string CreateRandomRoomName()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task LeaveRoomsForConnection(string connectionId)
        {
            if (ConnectionToRoomMap.TryRemove(connectionId, out string roomName))
            {
                if (Rooms.TryGetValue(roomName, out Room room))
                {
                    if (room.ConnectionIdsClients.TryRemove(connectionId, out string playerName))
                    {
                        Console.WriteLine($"Removed client {connectionId} from room {roomName}. Count in room: {room.ConnectionIdsClients.Count}");
                        await Clients.Client(room.ConnectionIdServer).SendAsync("Server_ReceiveClientDisconnected", playerName);
                    }
                    if (room.ConnectionIdServer == connectionId)
                    {
                        await Clients.Clients(room.ConnectionIdsClients.Keys.ToList()).SendAsync("Client_ReceiveServerDisconnected", roomName);
                        foreach (var client in room.ConnectionIdsClients.Keys.ToList())
                        {
                            ConnectionToRoomMap.TryRemove(client, out string _);
                            ConnectionIdToPlayerNameMapping.TryRemove(client, out string _);
                        }

                        Rooms.TryRemove(roomName, out Room _);
                        Console.WriteLine($"Removed server {connectionId} from room {roomName}. Room completely removed. Total rooms: {Rooms.Count}");
                    }
                }
            }
        }

        public async Task Server_CreateRoom(dynamic a)
        {
            Console.WriteLine($"Received create room from {Context.ConnectionId}");
            await LeaveRoomsForConnection(Context.ConnectionId);

            string roomName = CreateUniqueRoomName();
            await Clients.Caller.SendAsync("Server_ReceiveRoomName", roomName);

            Console.WriteLine($"Created room {roomName} for {Context.ConnectionId}. Total rooms: {Rooms.Count}");
        }

        public async Task Client_JoinRoom(string roomName, string clientName)
        {
            Console.WriteLine($"Received join room {roomName} from {Context.ConnectionId} with client name: {clientName}");
            roomName = roomName.ToUpperInvariant();

            bool allowContinue = false;
            lock (_clientNameDuplicationLockject)
            {
                var whatConnectionIdsBelongToThisPlayer = ConnectionIdToPlayerNameMapping.Where(x => x.Value == clientName).Select(x => x.Key).ToList();

                if (whatConnectionIdsBelongToThisPlayer.Count > 1)
                {
                    Console.WriteLine($"SHOULD NEVER HAPPEN: ClientNameAlreadyInUse for join room {roomName} from {Context.ConnectionId} with client name: {clientName}");
                }

                if (whatConnectionIdsBelongToThisPlayer.Count == 0)
                {
                    Console.WriteLine($"Player with name {clientName} does not exist yet.");
                    //No player with the name clientName exists yet, so we just make a new one.
                    allowContinue = true;                    
                }
                else
                {
                    //Count == 1
                    var firstConnectionId = whatConnectionIdsBelongToThisPlayer.First();

                    Console.WriteLine($"Found existing connectionId {firstConnectionId} for player with name {clientName}");

                    //Check if this connectionId is active in a room
                    if (ConnectionToRoomMap.ContainsKey(firstConnectionId))
                    {
                        Console.WriteLine($"ConnectionToRoomMap contains the key: {firstConnectionId}. {string.Join(",", ConnectionToRoomMap.Keys.ToList())}");

                        //This player is currently active in another room with an active signalr connection
                        //it's not allowed to pick this username
                        allowContinue = false;
                    }
                    else
                    {
                        Console.WriteLine($"ConnectionToRoomMap DOES NOT contains the key: {firstConnectionId}. {string.Join(",", ConnectionToRoomMap.Keys.ToList())}");

                        //Apparently this player is not active in any room so we can take over the name and send a join request to the server
                        allowContinue = true;
                    }
                }
            }

            if (!allowContinue)
            {
                Console.WriteLine($"ClientNameAlreadyInUse for join room {roomName} from {Context.ConnectionId} with client name: {clientName}");
                await Clients.Caller.SendAsync("Client_JoinRoomResult", "false_ClientNameAlreadyInUse");
                return;
            }


            ConnectionIdToPlayerNameMapping[Context.ConnectionId] = clientName;

            await LeaveRoomsForConnection(Context.ConnectionId);


            if (Rooms.TryGetValue(roomName, out Room room))
            {
                room.ConnectionIdsClients.TryAdd(Context.ConnectionId, clientName);
                ConnectionToRoomMap.TryAdd(Context.ConnectionId, roomName);
                await Clients.Caller.SendAsync("Client_JoinRoomResult", "true");
                await Clients.Client(room.ConnectionIdServer).SendAsync("Server_ReceiveJoinRoom", clientName);
                Console.WriteLine($"Client {Context.ConnectionId} joined room {roomName}. Count in room: {room.ConnectionIdsClients.Count}");
                return;
            }
            else
            {
                Console.WriteLine($"Room {roomName} does not exist. Client {Context.ConnectionId} did not join");
                await Clients.Caller.SendAsync("Client_JoinRoomResult", "false_RoomDoesNotExist");
            }
        }

        public async Task Client_SendButtonPress(string button, string pressed)
        {
            Console.WriteLine($"Received button press {button} from {Context.ConnectionId} {pressed}");

            if (ConnectionToRoomMap.TryGetValue(Context.ConnectionId, out string roomName))
            {
                if (Rooms.TryGetValue(roomName, out Room room))
                {
                    var playerName = "???";
                    if (ConnectionIdToPlayerNameMapping.TryGetValue(Context.ConnectionId, out var obtainedPlayerName))
                    {
                        playerName = obtainedPlayerName;
                    }
                    await Clients.Client(room.ConnectionIdServer).SendAsync("Client_ReceiveButtonPress", button, pressed, playerName);
                }
                else
                {
                    Console.WriteLine($"Room {roomName} does not exist. Client {Context.ConnectionId} did not join");
                }
            }
            else
            {
                Console.WriteLine("Could not find room name for connection id: " + Context.ConnectionId);
            }
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Received disconnect from {Context.ConnectionId}, Exception: {exception?.Message}");
            await LeaveRoomsForConnection(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
