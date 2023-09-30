using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject HostPanel;
    public GameObject ButtonPanel;
    public InputField RoomInput;

    public HostScreen HostScreen;
    public SignalRTest SignalR;

    internal void ShowHostScreen(string name)
    {
        HideAll();
        HostPanel.SetActive(true);
        HostScreen.RoomText.text = name;
    }

    private void HideAll()
    {
        MainMenuPanel.SetActive(false);
        HostPanel.SetActive(false);
        ButtonPanel.SetActive(false);
    }

    public void OnJoinClick()
    {
        string room = RoomInput.text;
        SignalR.OnJoinLobby(room);
    }

    // Start is called before the first frame update
    void Start()
    {
        HideAll();
        MainMenuPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ShowButtonScreen()
    {
        HideAll();
        ButtonPanel.SetActive(true);
    }
}
