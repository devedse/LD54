using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyCard : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Players;
    public Image Trophy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ShowPlayers(List<PC> podiums, float amount, string title, Color color)
    {
        Title.text = $"{title}: {amount} win{(amount != 1 ? "s" : "")}";
        Players.text = String.Join('\n', podiums.Select(x => x.PlayerName).ToArray());
        Trophy.color = color;
    }
}
