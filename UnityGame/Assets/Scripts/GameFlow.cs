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
        Debug.Log("GameFlow: Start");
        foreach (var p in MinigameManager.Instance.SignalR.Players.Values)
        {
            p.ResetButtonBindings();
        }
        StartTutorial.Invoke();
    }

    public void FinishTutorial()
    {
        Debug.Log("GameFlow: FinishTutorial");
        StartCountdown.Invoke();
        if (MinigameManager.Instance.ScoreCanvas)
            MinigameManager.Instance.ScoreCanvas.SetActive(true);
    }

    public void FinishCountdown()
    {
        Debug.Log("GameFlow: FinishCountdown");
        StartGame.Invoke();
    }

    public void SkipTut()
    {
        SkipTutorial.Invoke();
    }

    public void EndGame()
    {
        Debug.Log("GameFlow: EndGame");
        if (MinigameManager.Instance.ScoreCanvas)
            MinigameManager.Instance.ScoreCanvas.SetActive(false);
        MinigameManager.ShowPodiumBetweenGames();
        //NextGame(); // todo victory etc
    }

    public void ClaimReward()
    {
        Debug.Log("GameFlow: ClaimReward");
        if (MinigameManager.Instance.ScoreCanvas)
            MinigameManager.Instance.ScoreCanvas.SetActive(true);
        MinigameManager.ShowRewardScene();
    }

    public void NextGame()
    {
        Debug.Log("GameFlow: NextGame");
        MinigameManager.Instance.StartNextGame();
    }
}
