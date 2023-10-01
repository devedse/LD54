using UnityEngine;
using UnityEngine.UI;

public class InitialMenuController : MonoBehaviour
{
    public Image FromLeft;
    public Image FromRight;

    public Image GameTitle;

    private float startTime;

    private float elapsedTime = 0f;
    private const float totalTimeBounce = 2f;
    private const float totalTimeButton = 2f;
    private float initialBounceMagnitude;

    void Start()
    {
        initialBounceMagnitude = Screen.width / 2;
        startTime = Time.time;
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;

        if (elapsedTime < totalTimeButton)
        {
            GameTitle.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, 400), new Vector2(0, -30), Mathf.Min(elapsedTime / totalTimeButton, 1));
        }
        else
        {
            GameTitle.rectTransform.anchoredPosition = new Vector2(0, -30);
        }

        if (elapsedTime < totalTimeBounce)
        {
            float normalizedTime = elapsedTime / totalTimeBounce;
            float bounceMagnitude = initialBounceMagnitude * (1 - normalizedTime);
            float bounceOffset = bounceMagnitude * Mathf.Abs(Mathf.Cos(elapsedTime * Mathf.PI));


            FromLeft.rectTransform.anchoredPosition = new Vector2(-bounceOffset, FromLeft.rectTransform.anchoredPosition.y);
            FromRight.rectTransform.anchoredPosition = new Vector2(bounceOffset, FromRight.rectTransform.anchoredPosition.y);

        }
        else
        {
            // After 3 seconds, ensure images are exactly at target position
            FromLeft.rectTransform.anchoredPosition = new Vector2(0, FromLeft.rectTransform.anchoredPosition.y);
            FromRight.rectTransform.anchoredPosition = new Vector2(0, FromRight.rectTransform.anchoredPosition.y);
            this.enabled = false;  // Disable the script
        }
    }
}
