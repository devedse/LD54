using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int countdownTime = 6;
    public float timePerSecond = 0.33f;

    private float startTime;

    public Transform RewardParent;
    public Module Reward;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartCountdown()
    {
        var rewardInstance = Instantiate(MinigameManager.Instance.NextModuleReward, RewardParent);
        rewardInstance.transform.localPosition = Vector3.zero;
        Reward = rewardInstance.GetComponent<Module>();
        rewardInstance.AddComponent<RotatorScript>().RotatoSpeed = new Vector3(0, 90, 0);
        RewardParent.position = Camera.main.transform.position + (Camera.main.transform.forward * 1.5f);
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
        text.text = countdownTime <= 0 ? "GOO!!!" : countdownTime > 3 ? $"\n\n\n\nReward: {Reward.DisplayName}" : countdownTime.ToString();

        if (countdownTime == 0)
        {
            FindFirstObjectByType<GameFlow>().FinishCountdown();
            gameObject.SetActive(false);
            RewardParent.gameObject.SetActive(false);
        }
    }
}
