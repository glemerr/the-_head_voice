using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItemManager : MonoBehaviour
{
    [System.Serializable]
    public struct ItemPrefabEntry
    {
        public KeySpot.KeyItemType type;
        public GameObject prefab;
    }

    [Header("Configuration")]
    [SerializeField] private float pressTime = 1.0f;
    [SerializeField] private ItemPrefabEntry[] prefabEntries;

    [Header("Dependencies")]
    [SerializeField] private List<KeySpot> spots;
    [SerializeField] private GunManager gunManager;

    private Dictionary<KeySpot.KeyItemType, GameObject> itemPrefabs;
    private KeySpot currentSpot;
    private float holdTime;
    private bool isPressing;

    private void Awake()
    {
        InitializeItemPrefabs();
        ResolveDependencies();
    }

    private void InitializeItemPrefabs()
    {
        itemPrefabs = new Dictionary<KeySpot.KeyItemType, GameObject>();
        foreach (var entry in prefabEntries)
        {
            if (entry.prefab && !itemPrefabs.ContainsKey(entry.type))
                itemPrefabs.Add(entry.type, entry.prefab);
        }
    }

    private void ResolveDependencies()
    {
        if (!gunManager) gunManager = FindFirstObjectByType<GunManager>();
        
        foreach (var spot in spots)
        {
            if (spot) spot.Initialize(this, gunManager);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Update()
    {
        if (currentSpot == null) return;
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            HandleKeyPress();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            ResetKeyPress();
        }
    }

    private void HandleKeyPress()
    {
        if (!isPressing)
        {
            isPressing = true;
            holdTime = 0f;
        }

        holdTime += Time.deltaTime;

        if (holdTime >= pressTime)
        {
            CompleteInteraction();
        }
    }

    private void ResetKeyPress()
    {
        isPressing = false;
        holdTime = 0f;
    }

    private void CompleteInteraction()
    {
        if (currentSpot == null) return;
        
        currentSpot.SpawnItem();
        currentSpot = null;
        ResetKeyPress();
    }

    public void ActivateSpot(KeySpot spot)
    {
        currentSpot = spot;
    }
    
    public void DeactivateSpot()
    {
        currentSpot = null;
    }

    public GameObject GetItemPrefab(KeySpot.KeyItemType type)
    {
        return itemPrefabs.TryGetValue(type, out var prefab) ? prefab : null;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        CleanupAllKeyUIs();
    }

    private void CleanupAllKeyUIs()
    {
        foreach (var spot in spots)
        {
            if (spot != null)
            {
                spot.DestroyKeyUI();
            }
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (spots.Count == 0)
        {
            var foundSpots = FindObjectsByType<KeySpot>(FindObjectsSortMode.None);
            spots = new List<KeySpot>(foundSpots);
        }
    }
    #endif
}