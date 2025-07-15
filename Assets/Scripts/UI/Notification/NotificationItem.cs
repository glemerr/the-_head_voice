using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotificationItem : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image background;

    [Header("Colors")]
    public Color infoColor = new Color(0.2f, 0.6f, 1f, 0.8f);
    public Color itemColor = new Color(0.4f, 0.8f, 0.4f, 0.8f);
    public Color missionColor = new Color(1f, 0.8f, 0.2f, 0.8f);
    public Color warningColor = new Color(1f, 0.3f, 0.3f, 0.8f);

    public void Setup(NotificationData data)
    {
        // Set icon
        if (data.icon != null)
        {
            iconImage.sprite = data.icon;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }

        // Set text
        titleText.text = data.title;
        descriptionText.text = data.description;

        // Set background color based on type
        switch (data.type)
        {
            case NotificationType.Info:
                background.color = infoColor;
                break;
            case NotificationType.Item:
                background.color = itemColor;
                break;
            case NotificationType.Mission:
                background.color = missionColor;
                break;
            case NotificationType.Warning:
                background.color = warningColor;
                break;
        }

        // Start animation
        StartCoroutine(AnimateEntry());
    }

    private IEnumerator AnimateEntry()
    {
        // Get references to components
        RectTransform rt = GetComponent<RectTransform>();
        CanvasGroup cg = GetComponent<CanvasGroup>();

        // Store original scale and position
        Vector3 originalScale = rt.localScale;
        Vector2 originalPosition = rt.anchoredPosition;

        // Set initial animation state
        cg.alpha = 0f;
        rt.localScale = originalScale * 0.8f;
        rt.anchoredPosition = originalPosition + new Vector2(50f, 0f);

        // Animation parameters
        float duration = 0.4f;
        float elapsed = 0f;
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        while (elapsed < duration)
        {
            float progress = curve.Evaluate(elapsed / duration);

            // Fade in
            cg.alpha = Mathf.Lerp(0f, 1f, progress);

            // Scale up
            rt.localScale = Vector3.Lerp(originalScale * 0.8f, originalScale, progress);

            // Slide from right
            rt.anchoredPosition = Vector2.Lerp(
                originalPosition + new Vector2(50f, 0f),
                originalPosition,
                progress
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final state
        cg.alpha = 1f;
        rt.localScale = originalScale;
        rt.anchoredPosition = originalPosition;
    }

public IEnumerator AnimateExit()
{
    RectTransform rt = GetComponent<RectTransform>();
    CanvasGroup cg = GetComponent<CanvasGroup>();
    Vector3 originalScale = rt.localScale;
    
    float duration = 0.3f;
    float elapsed = 0f;
    
    while (elapsed < duration)
    {
        float progress = elapsed / duration;
        
        // Fade out
        cg.alpha = Mathf.Lerp(1f, 0f, progress);
        
        // Scale down slightly
        rt.localScale = Vector3.Lerp(originalScale, originalScale * 0.9f, progress);
        
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    // Destroy after animation completes
    Destroy(gameObject);
}
}