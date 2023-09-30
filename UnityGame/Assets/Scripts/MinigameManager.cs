using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    private static MinigameManager _instance;

    public static MinigameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //So that shit works in Editor as well
                _instance = new MinigameManager();
                _instance.SignalR = new SignalRTest();

                for (int i = 0; i < 2; i++)
                {
                    var keyboardPlayerControllerGA = new GameObject();
                    keyboardPlayerControllerGA.name = $"KeboardPlayerController{i}";
                    var pc = keyboardPlayerControllerGA.AddComponent<PC>();
                    pc.PlayerIndex = i;

                    if (i == 0)
                    {
                        pc.PlayerName = "PC Player ZXC";
                        pc.ListenToKeyboardZXC = true;
                    }
                    else if (i == 1)
                    {
                        pc.PlayerName = "PC Player ArrowKeys";
                        pc.ListenToKeyboardArrowKeys = true;
                    }

                    _instance.SignalR.Players.Add(i.ToString(), pc);
                }
            }
            return _instance;
        }
    }

    public SignalRTest SignalR;
    public MinigamesScriptableObject Games;
    public int GameIndex = -1;

    public void Start()
    {
        _instance = this;
    }

    public void StartNextGame()
    {
        GameIndex++;
        foreach (var p in SignalR.Players.Values)
        {

        }
        SceneManager.LoadScene(Games.Minigames[GameIndex].SceneName);
    }
}