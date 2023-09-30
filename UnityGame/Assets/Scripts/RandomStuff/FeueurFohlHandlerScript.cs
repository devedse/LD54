using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FeueurFohlHandlerScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject HostScreen;

    public TextMeshProUGUI TextMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI.text = GetAllPressedKeys();

        if (MainMenu.activeInHierarchy && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
        {
            MinigameManager.Instance.SignalR.OnHostLobby();
        }

        if (HostScreen.activeInHierarchy && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
        {
            MinigameManager.Instance.StartNextGame();
        }
    }

    private string GetAllPressedKeys()
    {
        var keyString = string.Join(", ", System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => Input.GetKey(k)).ToArray());

        string result = $"Keys: {keyString}";

        return result;
    }
}
