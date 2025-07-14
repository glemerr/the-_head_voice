using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoneUIManager : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup zoneUI;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI progressText;
    public Image timerBar;
    public Image timerBackground;

    public GameObject PanelUI;

    [Header("Timing Settings")]
    public float statusDuration = 2f;
    public float titleDuration = 2f;
    public float fadeDuration = 0.5f;

    private Coroutine currentSequence;
    private float maxTime;
    private float currentTime;
    public TMP_Text enemyCountText;
    public TMP_Text timerText;
    void Start()
    {
        // Initialize with all UI hidden
        zoneUI.alpha = 0;
        statusText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
        timerBar.gameObject.SetActive(false);
        timerBackground.gameObject.SetActive(false);
    }

    public void StartZoneSequence(Zone zoneConfig)
    {
        // Stop any existing sequence
        if (currentSequence != null)
        {
            StopCoroutine(currentSequence);
        }

        // Start new sequence
        currentSequence = StartCoroutine(ZoneSequence(zoneConfig));
    }

    private IEnumerator ZoneSequence(Zone zoneConfig)
    {
        maxTime = zoneConfig.maxTime;
        currentTime = maxTime;

        // Fade in UI
        yield return StartCoroutine(FadeUI(0f, 1f, fadeDuration));

        // Phase 1: Show status message
        statusText.text = zoneConfig.startMessages;
        statusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(statusDuration);

        // Phase 2: Show title
        statusText.gameObject.SetActive(false);
        titleText.text = zoneConfig.zoneName;
        titleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(titleDuration);

        // Phase 3: Show progress and timer
        titleText.gameObject.SetActive(false);
        progressText.gameObject.SetActive(true);
        timerBar.gameObject.SetActive(true);
        timerBackground.gameObject.SetActive(true);
        //yield return new WaitForSeconds(titleDuration);
        // Update progress and timer continuously
        StartCoroutine(DeactivatePanelAfterDelay(2f));

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerBar();
            UpdateProgressText(zoneConfig);
            yield return null;
            //UpdateTimerBar();

        }
        //PanelUI.SetActive(true);

        // Fade out UI at end
        //PanelUI.SetActive(false);
        yield return StartCoroutine(FadeUI(1f, 0f, fadeDuration));

        yield return new WaitForSeconds(titleDuration);
            //UpdateProgressText(zoneConfig);
        PanelUI.SetActive(false);
        // Hide all elements
        statusText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
        timerBar.gameObject.SetActive(false);
        timerBackground.gameObject.SetActive(false);
        //yield return new WaitForSeconds(titleDuration);
    }

    private void UpdateTimerBar()
    {
        float fillAmount = Mathf.Clamp01(currentTime / maxTime);
        timerBar.fillAmount = fillAmount;
        //Debug.Log($"Timer Bar Fill Amount: {fillAmount}");
        // Visual feedback - change color based on time remaining
        timerBar.color = Color.Lerp(Color.red, Color.blue, fillAmount);
    }

    private void UpdateProgressText(Zone zoneConfig)
    {
        progressText.text = string.Format(
            zoneConfig.progessMessages,
            ZoneManager.Instance.EnemiesRemaining
        );
    }

    public void EndZoneSequence(bool success, Zone zoneConfig)
    {
        if (currentSequence != null)
        {
            StopCoroutine(currentSequence);
        }

        StartCoroutine(EndSequence(success, zoneConfig));
    }

    private IEnumerator EndSequence(bool success, Zone zoneConfig)
    {
        // Show final message
        PanelUI.SetActive(true);
        statusText.text = success ? zoneConfig.completeMessage : zoneConfig.failMessages;
        statusText.gameObject.SetActive(true);
        progressText.gameObject.SetActive(false);
        timerBar.gameObject.SetActive(false);
        timerBackground.gameObject.SetActive(false);

        // Wait for message to be read
        yield return new WaitForSeconds(3f);

        // Fade out UI
        yield return StartCoroutine(FadeUI(1f, 0f, fadeDuration));

        // Hide all elements
        statusText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
    }

    private IEnumerator FadeUI(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        //PanelUI.SetActive(true);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            zoneUI.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        zoneUI.alpha = endAlpha;
    }

    public void UpdateEnemyCount(int defeated, int total)
    {
        enemyCountText.text = $"{defeated}/{total}";

        // Change color based on progress
        float progress = (float)defeated / total;
        enemyCountText.color = Color.Lerp(Color.red, Color.green, progress);
    }
        public void UpdateTimer(float timeRemaining)
    {
        // Format time as minutes:seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    private IEnumerator DeactivatePanelAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    PanelUI.SetActive(false);
}

}