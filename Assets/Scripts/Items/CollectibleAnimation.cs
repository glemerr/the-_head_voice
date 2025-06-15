using UnityEngine;

public class CollectibleAnimation : MonoBehaviour
{
    [Header("Float Animation Settings")]
    [Tooltip("Amplitude of the up and down movement.")]
    public float floatAmplitude = 0.5f;
    [Tooltip("Speed of the floating movement.")]
    public float floatFrequency = 1f;
    [Tooltip("Optional offset for the starting position.")]
    public Vector3 floatOffset = Vector3.zero;

    [Header("Light Pulse Settings")]
    [Tooltip("Reference to the Light component (can be set in the Inspector).")]
    public Light itemLight;
    [Tooltip("Base intensity of the light.")]
    public float baseIntensity = 1f;
    [Tooltip("Variation range for the light intensity.")]
    public float intensityVariation = 0.5f;
    [Tooltip("Speed at which the light pulses.")]
    public float intensityFrequency = 2f;

    private Vector3 startPos;

    private void Awake()
    {
        // Store the starting position of the collectible.
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        startPos = transform.position;

        // If the light isn't assigned in the Inspector, try finding one in children.
        if (itemLight == null)
        {
            itemLight = GetComponentInChildren<Light>();
        }
    }

    private void Update()
    {
        // Animate the vertical movement with a sine wave.
        float newY = startPos.y + floatAmplitude * Mathf.Sin(Time.time * floatFrequency * Mathf.PI * 2);
        transform.position = new Vector3(startPos.x, newY, startPos.z) + floatOffset;

        // Animate the light pulsing effect using a sine wave modulation.
        if (itemLight != null)
        {
            itemLight.intensity = baseIntensity + intensityVariation * Mathf.Sin(Time.time * intensityFrequency * Mathf.PI * 2);
        }
    }
}