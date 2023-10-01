using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SignalRTest : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Dictionary<string, PC> Players = new Dictionary<string, PC>();

    public List<PC> PlayersOrderedByScore => Players.Values.OrderByDescending(t => t.Score).ToList();

    public SignalR SignalR;
    public MainMenu MainMenu;
    public HostScreen HostScreen;

    private string DeveURL = "https://LD54_Server.Devedse.DuckDNS.org/UltraHub";

    public PC GetPlayerByNumber(int playerNumber)
    {
        return Players.Values.FirstOrDefault(t => t.PlayerIndex == playerNumber);
    }

    public void ReceiveRoomName(string name)
    {
        MainMenu.ShowHostScreen(name);
    }

    public void OnHostLobby()
    {
        DontDestroyOnLoad(gameObject);// Connection callbacks

        SetupSignalR();
        SignalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            // Log the connected ID
            Debug.Log($"Connected: {e.ConnectionId}");

            // Send payload A to hub as JSON
            var message = "Hallo Devedse ik ben een scherm :)))";
            SignalR.Invoke("Server_CreateRoom", message);
        };
        SignalR.Connect();
    }

    public void SetupSignalR()
    {
        SignalR = new SignalR();
#if UNITY_EDITOR
        //        DeveURL = "http://10.88.10.1:5281/UltraHub";
#endif
        SignalR.Init(DeveURL);

        // Handler callbacks
        SignalR.On("Server_ReceiveButtonPress", (string button, string pressed, string playername) =>
        {
            // Deserialize payload A from JSON
            Debug.Log($"Server_ReceiveButtonPress: {playername} {button} {pressed}");
            Players[playername].OnPress(int.Parse(button), pressed == "true");
        });
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
            AddClient(payload);
            Debug.Log($"Server_ReceiveJoinRoom: {payload}");
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
        });

        SignalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
        {
            // Log the disconnected ID
            Debug.Log($"Disconnected: {e.ConnectionId}");
        };
    }

    public PC AddClient(string name)
    {
        var player = Instantiate(PlayerPrefab, transform);
        var pc = player.GetComponent<PC>();
        pc.PlayerName = name;
        pc.PlayerIndex = Players.Count;
        pc.PlayerColor = MinigameManager.Instance.GetPlayerColor(pc.PlayerIndex);
        Players.Add(name, pc);

        HostScreen.AddPlayer(pc);

        return pc;
    }

    public void OnJoinLobby(string lobbyCode)
    {
        SetupSignalR();
        SignalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            // Log the connected ID
            Debug.Log($"Connected: {e.ConnectionId}");

            JoinRoom(lobbyCode, Guid.NewGuid().ToString());
        };
        Debug.Log("Connecting");
        SignalR.Connect();
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
