using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    
    [Header("UI References")]
    public RectTransform notificationPanel;
    public GameObject notificationPrefab;
    public Transform notificationContainer;
    
    [Header("Animation Settings")]
    public float slideInDuration = 0.5f;
    public float slideOutDuration = 0.5f;
    public float displayDuration = 2f;
    public float spacing = 10f;
    
    [Header("Position Settings")]
    public Vector2 hiddenPosition = new Vector2(500, 0);
    public Vector2 visiblePosition = new Vector2(-50, 0);
    
    private Queue<NotificationData> notificationQueue = new Queue<NotificationData>();
    private bool isShowing;
    private List<NotificationItem> activeNotifications = new List<NotificationItem>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        // Debug.Log("NotificationManager initialized");
        }
        else
        {
            Debug.Log("NotificationManager initialized");

            Destroy(gameObject);
        }
        
        // Initialize panel position
        notificationPanel.anchoredPosition = hiddenPosition;
    }
    
    public void ShowNotification(Sprite icon, string title, string description, NotificationType type = NotificationType.Info)
    {
        NotificationData data = new NotificationData(icon, title, description, type);
        notificationQueue.Enqueue(data);
        Debug.Log($"Notification queued: {title}" + isShowing);
        if (!isShowing)
        {
            StartCoroutine(ShowNextNotification());
        }
    }
    
    private IEnumerator ShowNextNotification()
    {
        isShowing = true;
        
        // Slide panel in
        yield return StartCoroutine(SlidePanel(true));
        
        while (notificationQueue.Count > 0)
        {
            // ... [notification creation code] ...

            NotificationData data = notificationQueue.Dequeue();
                // Create notification item
            GameObject notificationObj = Instantiate(notificationPrefab, notificationContainer);
            NotificationItem notification = notificationObj.GetComponent<NotificationItem>();
            notification.Setup(data);
            activeNotifications.Add(notification);
            
            // Position notifications
            UpdateNotificationPositions();
            
            // Wait for display duration
            yield return new WaitForSeconds(displayDuration);
            // Wait for display duration
            yield return new WaitForSeconds(displayDuration);
            
            // Remove oldest notification with animation
            if (activeNotifications.Count > 0)
            {
                NotificationItem toRemove = activeNotifications[0];
                activeNotifications.RemoveAt(0);
                
                // Start exit animation
                StartCoroutine(toRemove.AnimateExit());
                
                // Update positions immediately
                UpdateNotificationPositions();
                
                // Wait for exit animation before next notification
                yield return new WaitForSeconds(0.3f);
            }
        }
        
        // Slide panel out
        yield return StartCoroutine(SlidePanel(false));
        
        isShowing = false;
    }
    
    private void UpdateNotificationPositions()
    {
        float yPosition = 0;
        
        for (int i = activeNotifications.Count - 1; i >= 0; i--)
        {
            RectTransform rt = activeNotifications[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, yPosition);
            yPosition -= (rt.sizeDelta.y + spacing);
        }
    }
    
private IEnumerator SlidePanel(bool slideIn)
{
    Vector2 startPos = notificationPanel.anchoredPosition;
    Vector2 targetPos = slideIn ? visiblePosition : hiddenPosition;
    
    float elapsed = 0f;
    float duration = slideIn ? slideInDuration : slideOutDuration;
    AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    while (elapsed < duration)
    {
        float progress = curve.Evaluate(elapsed / duration);
        notificationPanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, progress);
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    notificationPanel.anchoredPosition = targetPos;
    
    // Add bounce effect for slide in
    if (slideIn)
    {
        elapsed = 0f;
        Vector2 bounceStart = targetPos;
        Vector2 bounceTarget = targetPos + new Vector2(20f, 0f);
        
        while (elapsed < 0.1f)
        {
            notificationPanel.anchoredPosition = Vector2.Lerp(
                bounceStart, 
                bounceTarget, 
                elapsed / 0.1f
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        elapsed = 0f;
        while (elapsed < 0.1f)
        {
            notificationPanel.anchoredPosition = Vector2.Lerp(
                bounceTarget, 
                bounceStart, 
                elapsed / 0.1f
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        notificationPanel.anchoredPosition = bounceStart;
    }
}
    
    // Public methods for specific notification types
    public void ShowItemNotification(string itemName, string description, Sprite icon)
    {
        ShowNotification(icon, $"{itemName} Collected", description, NotificationType.Item);
    }
    
    public void ShowMissionNotification(string missionTitle, string description)
    {
        ShowNotification(null, missionTitle, description, NotificationType.Mission);
    }
    
    public void ShowWarning(string title, string description)
    {
        ShowNotification(null, title, description, NotificationType.Warning);
    }
}

public enum NotificationType
{
    Info,
    Item,
    Mission,
    Warning
}

public class NotificationData
{
    public Sprite icon;
    public string title;
    public string description;
    public NotificationType type;
    
    public NotificationData(Sprite icon, string title, string description, NotificationType type)
    {
        this.icon = icon;
        this.title = title;
        this.description = description;
        this.type = type;
    }
}