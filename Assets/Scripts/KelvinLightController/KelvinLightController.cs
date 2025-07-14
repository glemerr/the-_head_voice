using UnityEngine;

[ExecuteAlways]
public class KelvinLightController : MonoBehaviour
{
    [Tooltip("Arrastra aquí la luz direccional a controlar")]
    public Light dirLight;

    [Tooltip("Temperatura en Kelvin (1 000–20 000)")]
    [Range(1000f, 20000f)]
    public float kelvin = 6500f;

    void OnValidate()
    {
        if (dirLight == null)
            dirLight = GetComponent<Light>();
        ApplyKelvin();
    }

    void Update()
    {
        ApplyKelvin();
    }

    void ApplyKelvin()
    {
        dirLight.color = KelvinToRGB(kelvin);
    }

    Color KelvinToRGB(float K)
    {
        float temp = K / 100f;
        float r = temp <= 66f
            ? 255f
            : 329.698727446f * Mathf.Pow(temp - 60f, -0.1332047592f);

        float g = temp <= 66f
            ? (temp <= 19f
                ? 0f
                : 99.4708025861f * Mathf.Log(temp) - 161.1195681661f)
            : 288.1221695283f * Mathf.Pow(temp - 60f, -0.0755148492f);

        float b = temp >= 66f
            ? 255f
            : (temp <= 19f
                ? 0f
                : 138.5177312231f * Mathf.Log(temp - 10f) - 305.0447927307f);

        return new Color(
            Mathf.Clamp01(r / 255f),
            Mathf.Clamp01(g / 255f),
            Mathf.Clamp01(b / 255f)
        );
    }
}