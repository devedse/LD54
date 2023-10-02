using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingPlayerTag : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Text;

    public void SetPlayer(PC pc)
    {
        Image.color = pc.PlayerColor;
        Text.text = pc.PlayerName;
        Text.color = pc.PlayerColor * new Color(.5f, .5f, .5f);
    }
}
