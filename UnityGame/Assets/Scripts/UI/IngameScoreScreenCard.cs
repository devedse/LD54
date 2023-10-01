using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameScoreScreenCard : MonoBehaviour
{
    public TextMeshProUGUI ScoreTxt;
    public TextMeshProUGUI NameTxt;
    public Image PlayerImage;
    public Image NameBackgroundImage;
    public PC pc;
    private float LastTimeChanged;
    private bool notNeutral;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(PC pc)
    {
        this.pc = pc;
        NameTxt.text = pc.PlayerName;
        ScoreTxt.text = "0";
        PlayerImage.sprite = pc.PlayerImage;
        NameBackgroundImage.color = pc.PlayerColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (notNeutral)
        {
            if (Time.time > LastTimeChanged + 1)
            {
                notNeutral = false;
                PlayerImage.sprite = pc.PlayerImage;
            }
        }
    }

    internal void UpdateScore(int absoluteScore, int relativeChange)
    {
        ScoreTxt.text = absoluteScore.ToString();
        if (relativeChange > 0)
            PlayerImage.sprite = pc.PlayerHappy;
        else
            PlayerImage.sprite = pc.PlayerMad;
        LastTimeChanged = Time.time;
        notNeutral = true;
    }
}
