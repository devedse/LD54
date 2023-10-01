using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void RemovePlayer(PC pc)
    {
        var existingPlayerCards = PlayersPanelRoot.GetComponentsInChildren<PlayerCard>().Where(t => t.PC == pc).ToList();
        foreach (var existing in existingPlayerCards)
        {
            Destroy(existing.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MinigameManager.Instance.SignalR.HandleJoinAndLeaveEvents();

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C)) && !MinigameManager.Instance.SignalR.Players.ContainsKey(PC.KeyboardZXCPlayerName))
        {
            var pc = MinigameManager.Instance.SignalR.AddOrConnectClient(PC.KeyboardZXCPlayerName);
            pc.ListenToKeyboardZXC = true;
        }
        if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !MinigameManager.Instance.SignalR.Players.ContainsKey(PC.KeyboardArrowKeysPlayerName))
        {
            var pc = MinigameManager.Instance.SignalR.AddOrConnectClient(PC.KeyboardArrowKeysPlayerName);
            pc.ListenToKeyboardArrowKeys = true;
        }
    }
}
