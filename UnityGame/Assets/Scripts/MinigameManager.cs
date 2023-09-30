using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                        pc.PlayerName = PC.KeyboardZXCPlayerName;
                        pc.ListenToKeyboardZXC = true;
                    }
                    else if (i == 1)
                    {
                        pc.PlayerName = PC.KeyboardArrowKeysPlayerName;
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
        if (_instance.SignalR.Players.Count < Games.Minigames[GameIndex].MinPlayers || _instance.SignalR.Players.Count > Games.Minigames[GameIndex].MaxPlayers)
        {
            if (Games)
            {
                if (!Games.Minigames.Any(x => _instance.SignalR.Players.Count >= x.MinPlayers && _instance.SignalR.Players.Count <= x.MaxPlayers))
                {
                    throw new System.Exception("No matching games for playercount");
                }
                else
                {
                    StartNextGame();
                }
            }
        }
        else
        {
            SceneManager.LoadScene(Games.Minigames[GameIndex].SceneName);
        }
    }
}
