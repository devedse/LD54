using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PC : MonoBehaviour
{
    public string PlayerName;
    public Sprite PlayerImage;

    public UnityEvent OnButton0Press;
    public UnityEvent OnButton1Press;
    public UnityEvent OnButton2Press;
    public UnityEvent OnButton0Release;
    public UnityEvent OnButton1Release;
    public UnityEvent OnButton2Release;

    public void OnPress(int button, bool pressed)
    {
        switch (button)
        {
            case 0:
                if (pressed)
                    OnButton0Press.Invoke();
                else
                    OnButton0Release.Invoke();
                break;
            case 1:
                if (pressed)
                    OnButton1Press.Invoke();
                else
                    OnButton1Release.Invoke();
                break;
            case 2:
                if (pressed)
                    OnButton2Press.Invoke();
                else
                    OnButton2Release.Invoke();
                break;
            default:
                break;
        }
    }

    public void ResetButtonBindings()
    {
        OnButton0Press.RemoveAllListeners();
        OnButton1Press.RemoveAllListeners();
        OnButton2Press.RemoveAllListeners();
        OnButton0Release.RemoveAllListeners();
        OnButton1Release.RemoveAllListeners();
        OnButton2Release.RemoveAllListeners();
    }
}
