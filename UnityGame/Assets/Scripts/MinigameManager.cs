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
        SceneManager.LoadScene(Games.Minigames[GameIndex].SceneName);
    }
}
