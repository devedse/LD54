using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public MinigamesScriptableObject Games;
    public int GameIndex = -1;

    public void StartNextGame()
    {
        GameIndex++;
        SceneManager.LoadScene(Games.Minigames[GameIndex].SceneName);
    }
}
