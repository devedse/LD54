using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    public SignalRTest SignalR;
    public MinigamesScriptableObject Games;
    public int GameIndex = -1;

    public void Start()
    {
        Instance = this;
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
