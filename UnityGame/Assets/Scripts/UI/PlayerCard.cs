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
        var sprite = CurrentImage.ImageIdle;

        // assume "sprite" is your Sprite object
        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        PC.PlayerImageTexture = croppedTexture;
        PlayerImage.sprite = CurrentImage.ImageIdle;
    }

    public void SetReady(bool isReady)
    {
        IsReady = isReady;
        ReadyText.text = isReady ? "Ready" : "";
    }
}
