using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static string ErrorToShow = "";

    //Main
    public GameObject SpaceShipBackgroundDingetjes;

    public GameObject Background;
    public GameObject InitialMenuPanel;
    public GameObject MainMenuPanel;
    public GameObject LoadingMenuPanel;

    //Server
    public GameObject HostMenuPanel;

    //Client
    public GameObject ClientNameMenuPanel;
    public GameObject ClientRoomCodeMenuPanel;
    public GameObject ButtonScreenPanel;
    //ClientInputs
    public InputField PlayerNameInput;
    public InputField RoomCodeInput;

    //Other
    public HostScreen HostScreen;
    public SignalRTest SignalR;
    public TextMeshProUGUI ErrorText;

    private void HideAll()
    {
        SpaceShipBackgroundDingetjes.SetActive(false);

        Background.SetActive(true);
        ErrorText.text = string.Empty;
        InitialMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        LoadingMenuPanel.SetActive(false);

        HostMenuPanel.SetActive(false);

        ClientNameMenuPanel.SetActive(false);
        ClientRoomCodeMenuPanel.SetActive(false);
        ButtonScreenPanel.SetActive(false);
    }

    public void GoTo_MainMenu()
    {
        HideAll();
        MainMenuPanel.SetActive(true);

        if (!string.IsNullOrWhiteSpace(ErrorToShow))
        {
            ErrorText.text = ErrorToShow;
            ErrorToShow = null;
        }
    }

    private void GoTo_InitialMenu()
    {
        HideAll();
        Background.SetActive(false);
        InitialMenuPanel.SetActive(true);
        SpaceShipBackgroundDingetjes.SetActive(true);
    }

    //Server stuff
    public void OnHostClick()
    {
        GoTo_LoadingScreen();
        SignalR.OnHostLobby();
    }

    public void GoTo_HostMenuPanel(string roomCode)
    {
        HideAll();
        HostMenuPanel.SetActive(true);
        HostScreen.RoomText.text = $"RoomCode: {roomCode}";
    }




    public void GoTo_ClientNameMenuPanel()
    {
        PlayerNameInput.text = string.Empty;
        HideAll();
        ClientNameMenuPanel.SetActive(true);
    }

    public void GoTo_ClientRoomCodeMenuPanel()
    {
        RoomCodeInput.text = string.Empty;
        HideAll();
        ClientRoomCodeMenuPanel.SetActive(true);
    }

    public void GoTo_LoadingScreen()
    {
        HideAll();
        LoadingMenuPanel.SetActive(true);
    }

    public void OnJoinClick()
    {
        string playerName = PlayerNameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = RandomNameGenerator.GeneratePlayerName();
        }

        string room = RoomCodeInput.text;
        SignalR.OnJoinLobby(playerName, room);
        GoTo_LoadingScreen();
    }



    public void OnClickToCompletelyRestartAndKillEverything()
    {
        MinigameManager.Instance.CompletelyRestartGameAndShit(string.Empty);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FirstRun)
        {
            GoTo_InitialMenu();
            FirstRun = false;
        }
        else
        {
            GoTo_MainMenu();
        }

    }

    public static bool FirstRun = true;

    // Update is called once per frame
    void Update()
    {

    }

    internal void ShowButtonScreen()
    {
        HideAll();
        ButtonScreenPanel.SetActive(true);
    }
}
