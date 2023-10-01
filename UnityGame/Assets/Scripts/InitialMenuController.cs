using UnityEngine;
using UnityEngine.UI;

public class InitialMenuController : MonoBehaviour
{
    public Image FromLeft;
    public Image FromRight;

    public Button GameTxt;

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

        var rectGame = (RectTransform)GameTxt.transform;
        if (elapsedTime < totalTimeButton)
        {
            rectGame.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0, -291.325f), Mathf.Min(elapsedTime / totalTimeButton, 1));
        }
        else
        {
            rectGame.anchoredPosition = new Vector2(0, -291.325f);
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
