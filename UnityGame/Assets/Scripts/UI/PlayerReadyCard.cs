using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyCard : MonoBehaviour
{
    public PlayerReadyScreen Screen;
    public bool IsReady;
    public TextMeshProUGUI PlayerName;

    public GameObject PlayerImage;
    public GameObject ReadyDiamond;

    public Image PlayerImageI;
    public Image PlayerMaskI;


    // Start is called before the first frame update
    void Start()
    {
        SetNotReady();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNotReady()
    {
        IsReady = false;
        ReadyDiamond.SetActive(!IsReady);
        PlayerImage.SetActive(IsReady);
    }

    public void ToggleReady()
    {
        IsReady = !IsReady;
        ReadyDiamond.SetActive(!IsReady);
        PlayerImage.SetActive(IsReady);
        Screen.RefreshReadyCheck();

        if (IsReady)
        {
            SoundManager.PlaySound(SoundManager.Instance.Sounds.CheesePlayerFinishedStack);
        }
    }
}
