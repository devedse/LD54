using System.Collections;
using UnityEngine;

public class BeatBonker : MonoBehaviour
{
    [Header("Beat Configuration")]
    public float bpm = 130f; // beats per minute
    public float initialDelay = 0.5f; // delay before the first beat

    [Header("Size Increase Configuration")]
    public float scaleFactor = 1.3f; // how much to scale the ship on the beat
    public float duration = 0.2f; // duration of the scale effect

    public float chanceToTrigger = 1;

    public bool RotateOnBeat = false;
    public bool BonkOnBeat = true;

    private void Start()
    {
        StartBonking();
    }

    public void StartBonking()
    {
        StopAllCoroutines();

        InvokeRepeating("TriggerBeat", initialDelay, 60f / bpm);
    }

    void TriggerBeat()
    {
        if (gameObject.activeInHierarchy)
        {
            if (Random.Range(0.0f, 1.0f) <= chanceToTrigger)
            {
                if (RotateOnBeat && BonkOnBeat)
                {
                    if (Random.Range(0.0f, 1f) <= 0.5f)
                    {
                        StartCoroutine(RotOnBeat());
                    }
                    else
                    {
                        StartCoroutine(ScaleOnBeat());
                    }
                }
                else if (RotateOnBeat)
                {
                    StartCoroutine(RotOnBeat());
                }
                else if (BonkOnBeat)
                {
                    StartCoroutine(ScaleOnBeat());
                }
            }
        }
    }

    IEnumerator ScaleOnBeat()
    {
        float elapsed = 0f;

        var initialScale = transform.localScale;

        while (elapsed < duration)
        {
            float currentScaleFactor = Mathf.Sin((elapsed / duration) * Mathf.PI) * (scaleFactor - 1) + 1;
            transform.localScale = initialScale * currentScaleFactor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;
    }

    IEnumerator RotOnBeat()
    {
        float elapsed = 0f;

        var fromScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        var targetScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(fromScale, targetScale, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }



        transform.localScale = targetScale;
    }
}
