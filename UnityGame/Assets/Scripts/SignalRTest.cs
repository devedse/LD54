using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalRTest : MonoBehaviour
{
    public SignalR SignalR;
    public MainMenu MainMenu;

    private string DeveURL = "https://LD54_Server.Devedse.DuckDNS.org/UltraHub";

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
        //DeveURL = "http://10.88.10.1:5281/UltraHub";
#endif
        SignalR.Init(DeveURL);

        // Handler callbacks
        SignalR.On("Server_ReceiveButtonPress", (string payload) =>
        {
            // Deserialize payload A from JSON
            Debug.Log($"Server_ReceiveButtonPress: {payload}");
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
            Debug.Log($"Server_ReceiveJoinRoom: {payload}");
        });
        SignalR.On("Client_JoinRoomResult", (bool payload) =>
        {
            // Deserialize payload B from JSON
            //var json = JsonUtility.FromJson<JsonPayload>(payload);
            //Debug.Log($"Server_ReceiveRoomName: {json.message}");
            Debug.Log($"Client_JoinRoomResult: {payload}");
            if (payload)
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

    public void OnJoinLobby(string lobbyCode)
    {
        SetupSignalR();
        SignalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            // Log the connected ID
            Debug.Log($"Connected: {e.ConnectionId}");

            JoinRoom(lobbyCode, "nietdevedse");
        };
        SignalR.Connect();
    }

    void Start()
    {
        
    }

    public void JoinRoom(string roomId, string clientName)
    {
        SignalR.Invoke("Client_JoinRoom", roomId, clientName);
    }
    public void SendButtonPress(int button, bool pressed)
    {
        SignalR.Invoke("Client_SendButtonPress", button, pressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
