using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        var highScore = MinigameManager.Instance.SignalR.Players.Values.Select(x => x.Score).Max();
        var winners = MinigameManager.Instance.SignalR.Players.Values.Where(x => x.Score == highScore).ToList();
        if (winners.Count == 1)
        {

            winners.First().Stats.MinigameWins++;
            var ordered = MinigameManager.Instance.SignalR.PlayersOrderedByScore;
            var top3 = ordered.Take(3).ToList();
            var lastPodiumScore = top3.Last().Score;
            foreach (var p in ordered.Where(x => x.Score >= lastPodiumScore))
                p.Stats.AmountOfTimesOnPodium++;
            var last = ordered.Last().Score;
            foreach (var p in ordered.Where(x => x.Score <= last))
                p.Stats.LastPaces++;

            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (MinigameManager.Instance.Games.TiebreakerMinigames.Any(x => x.SceneName == currentScene.name))
                ordered[0].Stats.TiebreakerWins++;
            if (currentScene.name == "FinalSpaceGame")
            {
                ordered[0].Stats.BigfiteWins++;
                MinigameManager.Instance.GoToStatsNext = true;
            }

            MinigameManager.ShowPodiumBetweenGames();
        }
        else
        {
            MinigameManager.Instance.DoTiebreaker();
        }
        //NextGame(); // todo victory etc
    }

    public void ClaimReward()
    {
        if (MinigameManager.Instance.GoToStatsNext)
        {
            MinigameManager.ShowStatsScene();
            return;
        }

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
