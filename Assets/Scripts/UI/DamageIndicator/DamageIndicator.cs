// DamageIndicator.cs
using UnityEngine;
using System.Collections;

public class DamageIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxDuration = 8f;
    [SerializeField] private float fadeInSpeed = 4f;
    [SerializeField] private float fadeOutSpeed = 2f;
    
    private float currentTimer;
    private Transform target;
    private Transform player;
    private Camera mainCamera;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;
    private System.Action unRegister;
    
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup.alpha = 0f;
    }

    public void Register(Transform target, Transform player, Camera cam, System.Action unRegister)
    {
        this.target = target;
        this.player = player;
        this.mainCamera = cam;
        this.unRegister = unRegister;
        
        StartCoroutine(RotateToTarget());
        RestartTimer();
    }

    public void RestartTimer()
    {
        currentTimer = maxDuration;
        canvasGroup.alpha = 1f;
        StopAllCoroutines();
        StartCoroutine(Countdown());
        StartCoroutine(RotateToTarget());
    }

    IEnumerator RotateToTarget()
    {
        while (enabled && target && player && mainCamera)
        {
            // Get direction from player to target
            if (target)
            {
                tRot = target.rotation;
                tPos = target.position;
            }
            tRot = Quaternion.LookRotation(player.position - tPos, Vector3.up);
            tRot.z = -tRot.y;
            tRot.y = 0f;
            tRot.x = 0f;
            Vector3 north = new Vector3(0f, 0f, player.eulerAngles.y);
            rectTransform.localRotation = Quaternion.Euler(north) * tRot;
            yield return null;
        }
    }

    IEnumerator Countdown()
    {
        // Fade in
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += fadeInSpeed * Time.deltaTime;
            yield return null;
        }

        // Main countdown
        while (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            yield return null;
        }

        // Fade out
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }

        // Cleanup
        unRegister?.Invoke();
        Destroy(gameObject);
    }
}