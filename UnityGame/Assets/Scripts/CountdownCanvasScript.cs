using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int countdownTime = 3;
    public float timePerSecond = 0.33f;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartCountdown()
    {
        startTime = Time.time;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + timePerSecond <= Time.time)
        {
            startTime += timePerSecond;
            countdownTime--;
        }
        text.text = countdownTime <= 0 ? "GOO!!!" : countdownTime.ToString();

        if (countdownTime == 0)
        {
            FindFirstObjectByType<GameFlow>().FinishCountdown();
            gameObject.SetActive(false);
        }
    }
}
