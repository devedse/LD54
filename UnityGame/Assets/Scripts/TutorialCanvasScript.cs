using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasScript : MonoBehaviour
{
    public float duration = 5f;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > duration)
        {
            KillTheTutorial();
        }
    }

    public void StartTutorial()
    {
        gameObject.SetActive(true);
    }

    public void KillTheTutorial()
    {
        FindFirstObjectByType<GameFlow>().FinishTutorial();
        gameObject.SetActive(false);
    }
}
