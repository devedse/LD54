using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalRTest : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Dictionary<string, PC> Players = new Dictionary<string, PC>();

    public List<PC> PlayersOrderedByScore => Players.Values.OrderByDescending(t => t.Score).ToList();

    public MinigameManager MinigameManagerInstance;
    public SignalR SignalR;
    public MainMenu MainMenu;
    public HostScreen HostScreen;

    private string DeveURL = "https://LD54_Server.Devedse.DuckDNS.org/UltraHub";

    public bool LobbyHasStartedSoBlockNewPlayerJoins { get; set; } = false;

    public PC GetPlayerByNumber(int playerNumber)
    {
        return Players.Values.FirstOrDefault(t => t.PlayerIndex == playerNumber);
    }

    public void ReceiveRoomName(string name)
    {
        MainMenu.GoTo_HostMenuPanel(name);
    }

    public void OnHostLobby()
    {
        SetupSignalR();
        SignalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            // Log the connected ID
            Debug.Log($"Connected: {e.ConnectionId}");

            // Send payload A to hub as JSON
            var message = "Hallo Devedse ik ben een scherm :)))";
            SignalR.Invoke("Server_CreateRoom", message);
        };
        SignalR.Connect(error => MinigameManagerInstance.CompletelyRestartGameAndShit(error));
    }

    public void SetupSignalR()
    {
        SignalR = new SignalR();
#if UNITY_EDITOR
        //DeveURL = "http://10.88.10.1:5281/UltraHub";
#endif
        SignalR.Init(DeveURL);

        // Handler callbacks
        SignalR.On("Server_ReceiveRoomName", (string payload) =>
        {
            // Deserialize payload B from JSON
            //var json = JsonUtility.FromJson<JsonPayload>(payload);
            //Debug.Log($"Server_ReceiveRoomName: {json.message}");
            Debug.Log($"Server_ReceiveRoomName: {payload}");
            ReceiveRoomName(payload);
        });
        SignalR.On("Server_ReceiveJoinRoom", (string payload) =>
        {
            // Deserialize payload B from JSON
            //var json = JsonUtility.FromJson<JsonPayload>(payload);
            //Debug.Log($"Server_ReceiveRoomName: {json.message}");
            AddOrConnectClient(payload);
            Debug.Log($"Server_ReceiveJoinRoom: {payload}");
        });
        SignalR.On("Server_ReceiveClientDisconnected", (string playerName) =>
        {
            SetClientDisconnectedToRemoveLater(playerName);
            Debug.Log($"Server_ReceiveClientDisconnected: {playerName}");
        });


        SignalR.On("Client_ReceiveButtonPress", (string button, string pressed, string playername) =>
        {
            // Deserialize payload A from JSON
            Debug.Log($"Client_ReceiveButtonPress: {playername} {button} {pressed}");
            Players[playername].OnPress(int.Parse(button), pressed == "true");
        });
        SignalR.On("Client_JoinRoomResult", (string payload) =>
        {
            // Deserialize payload B from JSON
            //var json = JsonUtility.FromJson<JsonPayload>(payload);
            //Debug.Log($"Server_ReceiveRoomName: {json.message}");
            Debug.Log($"Client_JoinRoomResult: {payload}");
            if (payload == "true")
            {
                MainMenu.ShowButtonScreen();
            }
            else if (payload == "false_ClientNameAlreadyInUse")
            {
                //TODO show error popup
                Debug.Log("ClientNameAlreadyInUse");

                MinigameManagerInstance.CompletelyRestartGameAndShit("Client name already in use");
            }
            else if (payload == "false_RoomDoesNotExist")
            {
                //TODO show error popup
                Debug.Log("RoomDoesNotExist");

                MinigameManagerInstance.CompletelyRestartGameAndShit("Room does not exist");
            }
        });
        SignalR.On("Client_ReceiveServerDisconnected", (string roomName) =>
        {
            // Log the disconnected ID
            Debug.Log($"Client_ReceiveServerDisconnected from RoomName: {roomName}");

            MinigameManagerInstance.CompletelyRestartGameAndShit("Host Lobby disconnected");
        });

        SignalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
        {
            // Log the disconnected ID
            Debug.Log($"Disconnected: {e.ConnectionId}");

            MinigameManagerInstance.CompletelyRestartGameAndShit($"Connection Disconnected: {e.ConnectionId}");
        };
    }

    private void SetClientDisconnectedToRemoveLater(string playerName)
    {
        if (Players.TryGetValue(playerName, out var existingPc))
        {
            existingPc.IsConnected = false;
        }
    }

    public PC AddOrConnectClient(string name)
    {
        if (Players.TryGetValue(name, out var existingPc))
        {
            existingPc.IsConnected = true;
            return existingPc;
        }
        else if (!LobbyHasStartedSoBlockNewPlayerJoins)
        {
            var player = Instantiate(PlayerPrefab, transform);
            var pc = player.GetComponent<PC>();
            pc.PlayerName = name;
            pc.PlayerIndex = Players.Count;
            pc.PlayerColor = MinigameManagerInstance.GetPlayerColor(pc.PlayerIndex);
            Players.Add(name, pc);

            HostScreen.AddPlayer(pc);

            return pc;
        }
        else
        {
            return null;
        }
    }

    public void HandleLeaveEvents()
    {
        var toRemove = Players.Values.Where(t => t.IsConnected == false).ToList();

        foreach (var playerToRemove in toRemove)
        {
            Players.Remove(playerToRemove.PlayerName);
            HostScreen.RemovePlayer(playerToRemove);
            Destroy(playerToRemove.gameObject);
        }
    }

    public void OnJoinLobby(string playerName, string lobbyCode)
    {
        SetupSignalR();
        SignalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            // Log the connected ID
            Debug.Log($"Connected: {e.ConnectionId}");

            JoinRoom(lobbyCode, playerName);
        };
        Debug.Log("Connecting");
        SignalR.Connect(error => MinigameManagerInstance.CompletelyRestartGameAndShit(error));
    }

    void Start()
    {

    }

    public void JoinRoom(string roomId, string clientName)
    {
        Debug.Log("Joining room");
        SignalR.Invoke("Client_JoinRoom", roomId, clientName);
    }
    public void SendButtonPress(string button, string pressed)
    {
        SignalR.Invoke("Client_SendButtonPress", button, pressed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
