using UnityEngine;

[ExecuteAlways]
public class GUICenterDot : MonoBehaviour
{
    [Header("Sizing Options")]
    [Tooltip("If true, uses Size Ratio to compute dot size. If false, uses Fixed Size (pixels).")]
    public bool useRatio = true;

    [Tooltip("Relative size as fraction of the screen’s shorter dimension.")]
    [Range(0f, 0.1f)]
    public float sizeRatio = 0.005f;

    [Tooltip("Fixed size in pixels (used when Use Ratio is false).")]
    public float fixedSize = 8f;

    [Header("Dot Appearance")]
    public Color color = Color.white;
    public Texture2D dotTexture;  

    // fallback white texture
    private Texture2D fallbackTex;

    void OnEnable()
    {
        fallbackTex = Texture2D.whiteTexture;
    }

    void OnGUI()
    {
        // Determine dot size
        float shortest = Mathf.Min(Screen.width, Screen.height);
        float dotSize = useRatio
            ? shortest * sizeRatio
            : fixedSize;
        dotSize = Mathf.Clamp(dotSize, 2f, 64f);

        // Set GUI color
        GUI.color = color;

        // Choose texture
        Texture2D tex = dotTexture != null ? dotTexture : fallbackTex;

        // Centered rectangle
        Rect rect = new Rect(
            (Screen.width  - dotSize) * 0.5f,
            (Screen.height - dotSize) * 0.5f,
            dotSize,
            dotSize
        );

        GUI.DrawTexture(rect, tex);

        // Reset GUI color
        GUI.color = Color.white;
    }
}