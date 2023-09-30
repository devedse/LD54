using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeueurFohlHandlerScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject HostScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainMenu.activeInHierarchy &&  Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            MinigameManager.Instance.SignalR.OnHostLobby();
        }

        if (HostScreen.activeInHierarchy && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            MinigameManager.Instance.StartNextGame();
        }
    }
}
