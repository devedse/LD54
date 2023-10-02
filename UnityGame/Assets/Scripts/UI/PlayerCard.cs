using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    public TextMeshProUGUI ReadyText;
    public TextMeshProUGUI PlayerName;
    public Image PlayerImage;
    public PC PC;
    public PlayerImagesScriptableObject Images;
    public PlayerImageScriptableObject CurrentImage;
    public Image NameBackgroundImage;
    public bool IsReady;

    public void SetPC(PC pc)
    {
        this.PC = pc;
        PlayerName.text = pc.PlayerName;
        ChangeImage(Images.Images[0]);
        PlayerImage.sprite = CurrentImage.ImageIdle;
        pc.ResetButtonBindings();
        pc.OnButton0Press.AddListener(() => { CycleImages(false); });
        pc.OnButton1Press.AddListener(() => { SetReady(!IsReady); });
        pc.OnButton2Press.AddListener(() => { CycleImages(true); });
        NameBackgroundImage.color = pc.PlayerColor;
        GetComponent<Image>().color = pc.PlayerColor * new Color(1, 1, 1, .7f);
        PlayerName.color = pc.PlayerColor * new Color(.5f, .5f, .5f);
        ReadyText.color = pc.PlayerColor * new Color(.5f, .5f, .5f);
    }

    private void CycleImages(bool up)
    {
        var index = Images.Images.IndexOf(CurrentImage);
        if (up)
            index++;
        else
            index--;
        if (index >= Images.Images.Count)
            index = 0;
        if (index < 0)
            index = Images.Images.Count - 1;

        ChangeImage(Images.Images[index]);
    }

    public void ChangeImage(PlayerImageScriptableObject imageSO)
    {
        CurrentImage = imageSO;
        PC.PlayerImage = CurrentImage.ImageIdle;
        PC.PlayerHappy = CurrentImage.ImageWin;
        PC.PlayerMad = CurrentImage.ImageSad;
        PlayerImage.sprite = CurrentImage.ImageIdle;
    }

    public void SetReady(bool isReady)
    {
        IsReady = isReady;
        ReadyText.text = isReady ? "Ready" : "";
    }
}
