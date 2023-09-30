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
        
    }
}
