using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PC : MonoBehaviour
{
    public const string KeyboardZXCPlayerName = "PC Player ZXC";
    public const string KeyboardArrowKeysPlayerName = "PC Player ArrowKeys";

    public int PlayerIndex; // never higher than playercount - 1
    public string PlayerName;
    public Sprite PlayerImage;
    public Color PlayerColor;
    public Sprite PlayerHappy;
    public Sprite PlayerMad;

    internal void ResetScore()
    {
        ScoreFromCurrentMinigame = 0;
    }

    public UnityEvent OnButton0Press = new UnityEvent();
    public UnityEvent OnButton1Press = new UnityEvent();
    public UnityEvent OnButton2Press = new UnityEvent();
    public UnityEvent OnButton0Release = new UnityEvent();
    public UnityEvent OnButton1Release = new UnityEvent();
    public UnityEvent OnButton2Release = new UnityEvent();

    public bool ListenToKeyboardZXC = false;
    public bool ListenToKeyboardArrowKeys = false;

    private int ScoreFromCurrentMinigame;
    public int Score { get { return ScoreFromCurrentMinigame; } }
    public List<IngameScoreScreenCard> Cards = new List<IngameScoreScreenCard>();


    public Dictionary<int, ShipModuleType> SlotsAndModules = new Dictionary<int, ShipModuleType>();

    public ShipModuleType GetModuleForSlot(int slot)
    {
        if (slot < 0 || slot > 2)
        {
            throw new Exception("Slot must be between 0 and 2");
        }

        if (SlotsAndModules.ContainsKey(slot))
        {
            return SlotsAndModules[slot];
        }
        else
        {
            return ShipModuleType.None;
        }
    }

    public void SetModuleForSlot(int slot, ShipModuleType module)
    {
        if (slot < 0 || slot > 2)
        {
            throw new Exception("Slot must be between 0 and 2");
        }

        SlotsAndModules[slot] = module;
    }

    public void ChangeScore(int amount)
    {
        ScoreFromCurrentMinigame += amount;
        foreach (var card in Cards)
        {
            card.UpdateScore(ScoreFromCurrentMinigame, amount);
        }
    }

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



    private void Update()
    {
        if (ListenToKeyboardZXC)
        {
            // Check for ZXC buttons pressed
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnButton0Press.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                OnButton1Press.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnButton2Press.Invoke();
            }

            // Check for ZXC buttons released
            if (Input.GetKeyUp(KeyCode.Z))
            {
                OnButton0Release.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.X))
            {
                OnButton1Release.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                OnButton2Release.Invoke();
            }
        }

        if (ListenToKeyboardArrowKeys)
        {
            // Check for arrow key buttons pressed
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnButton0Press.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnButton1Press.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnButton2Press.Invoke();
            }

            // Check for arrow key buttons released
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                OnButton0Release.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                OnButton1Release.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                OnButton2Release.Invoke();
            }
        }
    }
}

public enum ShipModuleType
{
    None,
    Chainsaw,
    Booster,
    Parasolding,
    Turbine,
    Squid,
    Adelaar,
}