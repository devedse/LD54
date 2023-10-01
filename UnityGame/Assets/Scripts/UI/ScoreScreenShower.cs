using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreenShower : MonoBehaviour
{
    public GameObject Top;
    public GameObject Bottom;
    public GameObject Left;
    public GameObject Right;
    public GameObject LeftRight;
    public GameObject TopBottom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(ScoreScreenOptions o)
    {
        HideAll();
        switch (o)
        {
            case ScoreScreenOptions.Hide: 
                break;
            case ScoreScreenOptions.Top: Top.SetActive(true);
                break;
            case ScoreScreenOptions.Bottom: Bottom.SetActive(true);
                break;
            case ScoreScreenOptions.Left: Left.SetActive(true);
                break;
            case ScoreScreenOptions.Right: Right.SetActive(true);
                break;
            case ScoreScreenOptions.LeftRight: LeftRight.SetActive(true);
                break;
            case ScoreScreenOptions.TopBottom: TopBottom.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void HideAll()
    {
        Top.SetActive(false);
        Bottom.SetActive(false);
        Left.SetActive(false);
        Right.SetActive(false);
        LeftRight.SetActive(false);
        TopBottom.SetActive(false);
    }
}

public enum ScoreScreenOptions
{
    Hide,
    Top,
    Bottom,
    Left,
    Right,
    LeftRight,
    TopBottom,
}