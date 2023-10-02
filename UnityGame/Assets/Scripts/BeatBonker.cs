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

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;

        InvokeRepeating("TriggerBeat", initialDelay, 60f / bpm);
    }

    void TriggerBeat()
    {
        if (Random.Range(0.0f, 1.0f) <= chanceToTrigger)
        {
            StartCoroutine(ScaleOnBeat());
        }
    }

    IEnumerator ScaleOnBeat()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float currentScaleFactor = Mathf.Sin((elapsed / duration) * Mathf.PI) * (scaleFactor - 1) + 1;
            transform.localScale = initialScale * currentScaleFactor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;
    }
}
