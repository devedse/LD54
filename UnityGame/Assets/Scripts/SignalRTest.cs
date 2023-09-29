using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalRTest : MonoBehaviour
{
    void Start()
    {
        // Initialize SignalR
        var signalR = new SignalR();
        signalR.Init("http://10.88.10.1:5281/UltraHub");

        // Handler callbacks
        signalR.On("Server_ReceiveButtonPress", (string payload) =>
        {
            // Deserialize payload A from JSON
            var json = JsonUtility.FromJson<JsonPayload>(payload);
            Debug.Log($"Server_ReceiveButtonPress: {payload}");
        });
        signalR.On("Server_ReceiveRoomName", (string payload) =>
        {
            // Deserialize payload B from JSON
            //var json = JsonUtility.FromJson<JsonPayload>(payload);
            //Debug.Log($"Server_ReceiveRoomName: {json.message}");
            Debug.Log($"Server_ReceiveRoomName: {payload}");
        });

        // Connection callbacks
        signalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            // Log the connected ID
            Debug.Log($"Connected: {e.ConnectionId}");

            // Send payload A to hub as JSON
            var json1 = new JsonPayload
            {
                message = "Hallo Devedse :)))"
            };
            signalR.Invoke("Server_CreateRoom", JsonUtility.ToJson(json1));
        };
        signalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
        {
            // Log the disconnected ID
            Debug.Log($"Disconnected: {e.ConnectionId}");
        };

        signalR.Connect();
        Debug.Log("hallo");
    }

    [Serializable]
    public class JsonPayload
    {
        public string message;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
