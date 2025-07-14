using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class InterestPointManager : MonoBehaviour
{
    [System.Serializable]
    public class InterestPointData
    {
        public Transform target;
        [HideInInspector] public RectTransform icon;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public float visibilityTimer;
    }

    [Header("References")]
    public Transform player;
    public Camera mainCamera;
    public GameObject indicatorPrefab;
    public Canvas canvas;

    [Header("Settings")]
    public List<InterestPointData> interestPoints = new List<InterestPointData>();
    public float edgeBuffer = 50f;
    public float smoothTime = 0.2f;
    public float maxScale = 1.2f;
    public float minScale = 0.6f;
    public float maxDistance = 100f;
    public float showDuration = 3f;
    public float cooldownDuration = 5f;
    public KeyCode toggleKey;

    [Header("Visual Effects")]
    public Gradient colorGradient;
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.2f;

    private float cooldownTimer;
    private bool indicatorsActive;


    private void Start()
    {
        InitializeIndicators();
    }

    protected void InitializeIndicators()
    {
        foreach (var point in interestPoints)
        {
            GameObject iconGO = Instantiate(indicatorPrefab, canvas.transform);
            point.icon = iconGO.GetComponent<RectTransform>();
            point.icon.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        removeDeadInterestPoints();
        UpdateCooldown();
        HandleInput();
        UpdateIndicators();
    }

    private void removeDeadInterestPoints()
    {
        interestPoints.RemoveAll(point => point.target == null || point.target.gameObject == null);
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(toggleKey) && cooldownTimer <= 0)
        {
            ToggleIndicators();
            cooldownTimer = cooldownDuration;
        }
    }

    private void ToggleIndicators()
    {
        indicatorsActive = !indicatorsActive;
        
        foreach (var point in interestPoints)
        {
            if (indicatorsActive)
            {
                point.icon.gameObject.SetActive(true);
                point.visibilityTimer = showDuration;
                point.velocity = Vector3.zero;
            }
            else
            {
                point.icon.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateIndicators()
    {
        foreach (var point in interestPoints)
        {
            if (!point.icon.gameObject.activeSelf) continue;
            
            UpdateVisibilityTimer(point);
            UpdateIndicatorPosition(point);
            ApplyVisualEffects(point);
        }
    }

    private void UpdateVisibilityTimer(InterestPointData point)
    {
        point.visibilityTimer -= Time.deltaTime;
        if (point.visibilityTimer <= 0)
        {
            point.icon.gameObject.SetActive(false);
        }
    }

    private void UpdateIndicatorPosition(InterestPointData point)
    {
        Vector3 worldPos = point.target.position + Vector3.up * 1.5f; // 0.5m above object
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        // Vector3 screenPos = mainCamera.WorldToScreenPoint(point.target.position);
        bool isBehind = Vector3.Dot(point.target.position - player.position, player.forward) < 0;

        // Calculate target position and rotation
        Vector3 targetPosition;
        Quaternion targetRotation;
        
        if (screenPos.z > 0 && !isBehind)
        {
            // On-screen position
            targetPosition = screenPos;
            targetRotation = Quaternion.identity;
        }
        else
        {
            // Off-screen position
            if (isBehind) screenPos *= -1;
            
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 screenBounds = screenCenter - new Vector3(edgeBuffer, edgeBuffer, 0);

            Vector3 direction = (screenPos - screenCenter).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x);
            
            // Calculate edge position
            float slope = Mathf.Tan(angle);
            if (Mathf.Abs(slope) > (screenBounds.y / screenBounds.x))
            {
                targetPosition = new Vector3(
                    screenCenter.x + screenBounds.y / Mathf.Abs(slope) * Mathf.Sign(direction.x),
                    screenCenter.y + screenBounds.y * Mathf.Sign(direction.y),
                    0
                );
            }
            else
            {
                targetPosition = new Vector3(
                    screenCenter.x + screenBounds.x * Mathf.Sign(direction.x),
                    screenCenter.y + screenBounds.x * slope ,
                    0
                );
            }
            
            // Calculate rotation toward target
            float iconAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, 0, iconAngle - 90);
        }

        // Apply smooth movement
        point.icon.position = Vector3.SmoothDamp(
            point.icon.position, 
            targetPosition, 
            ref point.velocity, 
            smoothTime
        );
        
        // Smooth rotation
        point.icon.rotation = Quaternion.Slerp(
            point.icon.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );

        // Distance-based scaling
        float distance = Vector3.Distance(player.position, point.target.position);
        float scaleFactor = Mathf.Clamp(1 - (distance / maxDistance), minScale, maxScale);
        point.icon.localScale = Vector3.one * scaleFactor;
    }

    private void ApplyVisualEffects(InterestPointData point)
    {
        //Image iconImage = point.icon.GetComponentInChildren<Image>();
        Image[] images = point.icon.GetComponentsInChildren<Image>();
        Image iconImage = images[images.Length - 1];
        float timePercent = point.visibilityTimer / showDuration;
        
        // Pulsing effect
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity) + 1f;
        
        // Apply color gradient
        iconImage.color = colorGradient.Evaluate(timePercent);
        
        // Apply pulsing to scale
        point.icon.localScale *= pulse;
    }
}