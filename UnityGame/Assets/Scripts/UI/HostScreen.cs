using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HostScreen : MonoBehaviour
{
    public TextMeshProUGUI RoomText;
    public Transform PlayersPanelRoot;
    public GameObject PlayerUIPrefab;

    public void AddPlayer(PC pc)
    {
        var card = Instantiate(PlayerUIPrefab, PlayersPanelRoot).GetComponent<PlayerCard>();
        card.SetPC(pc);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C)) && !MinigameManager.Instance.SignalR.Players.ContainsKey(PC.KeyboardZXCPlayerName))
        {
            var pc = MinigameManager.Instance.SignalR.AddClient(PC.KeyboardZXCPlayerName);
            pc.ListenToKeyboardZXC = true;
        }
        if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !MinigameManager.Instance.SignalR.Players.ContainsKey(PC.KeyboardArrowKeysPlayerName))
        {
            var pc = MinigameManager.Instance.SignalR.AddClient(PC.KeyboardArrowKeysPlayerName);
            pc.ListenToKeyboardArrowKeys = true;
        }
    }
}
