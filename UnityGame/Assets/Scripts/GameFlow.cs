using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameFlow : MonoBehaviour
{
    public UnityEvent StartTutorial;
    public UnityEvent StartCountdown;
    public UnityEvent StartGame;

    // Start is called before the first frame update
    void Start()
    {
        StartTutorial.Invoke();
    }

    public void FinishTutorial()
    {
        StartCountdown.Invoke();
    }

    public void FinishCountdown()
    {
        StartGame.Invoke();
    }

    public void EndGame()
    {
        NextGame(); // todo victory etc
    }

    public void NextGame()
    {
        MinigameManager.Instance.StartNextGame();
    }
}
