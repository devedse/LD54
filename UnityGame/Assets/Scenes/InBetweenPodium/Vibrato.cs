using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrato : MonoBehaviour
{
    [Header("Vibration Settings")]
    public float frequency = 30.0f; // How fast the vibration is
    public float amplitude = 0.01f; // How big the vibration is

    [Header("Rotation Settings")]
    public float maxRotation = 1f; // Maximum rotation in degrees

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float time;

    private void Start()
    {
        // Save the initial position and rotation
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        time += Time.deltaTime;

        // Compute the oscillation for position (you can use Cos or Sin, depending on the desired effect)
        float yOffset = amplitude * Mathf.Sin(time * frequency);

        // Adjust the position
        transform.localPosition = initialPosition + new Vector3(0, yOffset, 0);

        // Compute the oscillation for rotation
        float rotationOffset = maxRotation * Mathf.Sin(time * 1.1f * frequency);

        // Adjust the rotation
        transform.localRotation = initialRotation * Quaternion.Euler(0, 0, rotationOffset);
    }
}
