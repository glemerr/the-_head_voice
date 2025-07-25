using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectablePickup : MonoBehaviour
{
    [Header("Collectable Settings")]
    [SerializeField] private CollectableObject collectableData;
    [SerializeField] private int quantity = 1;
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private GameObject pickupEffect;
    
    [Header("UI Configuration")]
    [SerializeField] private float yOffset = 2.0f;
    [SerializeField] private float edgeBuffer = 50f;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private GameObject keyUIPrefab;

    private Camera mainCamera;
    private Canvas mainCanvas;
    private GameObject keyItemUI;
    private RectTransform uiRectTransform;
    private Vector3 uiVelocity;
    private bool playerInTrigger;

    void Start()
    {
        // Auto-add sphere collider for detection
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = pickupRadius;

        // Initialize UI
        mainCamera = Camera.main;
        mainCanvas = FindFirstObjectByType<Canvas>();
        InitializeUI();
    }

    private void InitializeUI()
    {
        if (!keyUIPrefab || !mainCanvas) return;
        
        keyItemUI = Instantiate(keyUIPrefab, mainCanvas.transform);
        uiRectTransform = keyItemUI.GetComponent<RectTransform>();
        keyItemUI.SetActive(false);
    }

    private void LateUpdate()
    {
        if (playerInTrigger)
        {
            // Check for E key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptPickup();
            }

            // Update UI position if active
            if (keyItemUI != null && keyItemUI.activeSelf)
                UpdateUIPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInTrigger = true;
        ShowPickupUIMessage();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInTrigger = false;
        HidePickupUIMessage();
    }

    private void ShowPickupUIMessage()
    {
        if (keyItemUI) keyItemUI.SetActive(true);
    }

    private void HidePickupUIMessage()
    {
        if (keyItemUI) keyItemUI.SetActive(false);
    }

    private void UpdateUIPosition()
    {
        Vector3 worldPos = transform.position + Vector3.up * yOffset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        bool isBehind = IsBehindCamera(worldPos);

        Vector3 targetPos;
        Quaternion targetRot = Quaternion.identity;

        if (screenPos.z > 0 && !isBehind)
        {
            targetPos = screenPos;
        }
        else
        {
            CalculateOffscreenPosition(screenPos, isBehind, out targetPos, out targetRot);
        }

        ApplyUIMovement(targetPos, targetRot);
        ApplyUIScaling(worldPos);
    }

    private bool IsBehindCamera(Vector3 worldPos)
    {
        return Vector3.Dot(worldPos - mainCamera.transform.position, 
                        mainCamera.transform.forward) < 0;
    }

    private void CalculateOffscreenPosition(Vector3 screenPos, bool isBehind, 
                        out Vector3 targetPos, out Quaternion targetRot)
    {
        if (isBehind) screenPos *= -1;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 bounds = screenCenter - Vector3.one * edgeBuffer;
        Vector3 dir = (screenPos - screenCenter).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);

        // Edge intersection calculation
        float slope = Mathf.Tan(angle);
        if (Mathf.Abs(slope) > (bounds.y / bounds.x))
        {
            targetPos = new Vector3(
                screenCenter.x + bounds.y / Mathf.Abs(slope) * Mathf.Sign(dir.x),
                screenCenter.y + bounds.y * Mathf.Sign(dir.y),
                0
            );
        }
        else
        {
            targetPos = new Vector3(
                screenCenter.x + bounds.x * Mathf.Sign(dir.x),
                screenCenter.y + bounds.x * slope,
                0
            );
        }

        float iconAngle = angle * Mathf.Rad2Deg;
        targetRot = Quaternion.Euler(0, 0, iconAngle - 90f);
    }

    private void ApplyUIMovement(Vector3 targetPos, Quaternion targetRot)
    {
        keyItemUI.transform.position = Vector3.SmoothDamp(
            keyItemUI.transform.position, 
            targetPos, 
            ref uiVelocity, 
            smoothTime
        );

        keyItemUI.transform.rotation = Quaternion.Slerp(
            keyItemUI.transform.rotation,
            targetRot,
            Time.deltaTime * 10f
        );
    }

    private void ApplyUIScaling(Vector3 worldPos)
    {
        float dist = Vector3.Distance(mainCamera.transform.position, worldPos);
        float scale = Mathf.Clamp(1f - (dist / maxDistance), minScale, maxScale);
        keyItemUI.transform.localScale = Vector3.one * scale;
    }

    public void AttemptPickup()
    {
        if (InventoryManager.Instance.AddCollectable(collectableData, quantity))
        {
            // Successfully picked up
            if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            AudioManager.Instance.PlayPickupSound();
            DestroyKeyUI();
            Destroy(gameObject);
        }
    }

    private void DestroyKeyUI()
    {
        if (keyItemUI != null)
        {
            if (Application.isPlaying)
            {
                Destroy(keyItemUI);
            }
            else
            {
                DestroyImmediate(keyItemUI);
            }
            keyItemUI = null;
        }
    }

    private void OnDestroy()
    {
        DestroyKeyUI();
    }

    private void OnDisable()
    {
        DestroyKeyUI();
    }

    // Visual helper in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}