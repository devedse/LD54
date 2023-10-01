using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWiggle : MonoBehaviour
{
    [Header("Wiggle Settings")]
    public float wiggleSpeed = 12f;
    public float wiggleAmount = 4f; // The amount by which the object wiggles
    public Vector3 wiggleDirection = Vector3.up; // Default is up

    [Header("Squish Settings")]
    public float squishSpeed = 10.0f;
    public float squishAmount = 0.05f; // The amount by which the object squishes

    private Vector3 initialScale;
    private float time;

    private Vector3 initialPosition;

    private void Start()
    {
        // Save the initial scale of the object
        initialScale = transform.localScale;

        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        time += Time.deltaTime;

        // Compute wiggle
        Vector3 wiggleOffset = wiggleDirection.normalized * Mathf.Sin(time * wiggleSpeed) * wiggleAmount / 100f;

        // Compute squish
        Vector3 squishScale = initialScale + new Vector3(Mathf.Sin(time * squishSpeed) * squishAmount, -Mathf.Sin(time * squishSpeed) * squishAmount, Mathf.Sin(time * squishSpeed) * squishAmount);

        // Apply wiggle and squish
        transform.localPosition = initialPosition + wiggleOffset;
        transform.localScale = squishScale;
    }
}
