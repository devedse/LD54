using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameFlow : MonoBehaviour
{
    public UnityEvent StartTutorial;
    public UnityEvent SkipTutorial;
    public UnityEvent StartCountdown;
    public UnityEvent StartGame;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var p in MinigameManager.Instance.SignalR.Players.Values)
        {
            p.ResetButtonBindings();
        }
        StartTutorial.Invoke();
    }

    public void FinishTutorial()
    {
        StartCountdown.Invoke();
        if (MinigameManager.Instance.ScoreCanvas)
            MinigameManager.Instance.ScoreCanvas.SetActive(true);
    }

    public void FinishCountdown()
    {
        StartGame.Invoke();
    }

    public void SkipTut()
    {
        SkipTutorial.Invoke();
    }

    public void EndGame()
    {
        if (MinigameManager.Instance.ScoreCanvas)
            MinigameManager.Instance.ScoreCanvas.SetActive(false);
        MinigameManager.ShowPodiumBetweenGames();
        //NextGame(); // todo victory etc
    }

    public void ClaimReward()
    {
        if (MinigameManager.Instance.ScoreCanvas)
            MinigameManager.Instance.ScoreCanvas.SetActive(true);
        MinigameManager.ShowRewardScene();
    }

    public void NextGame()
    {
        MinigameManager.Instance.StartNextGame();
    }
}
