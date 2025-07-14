using System.Collections.Generic;
using UnityEngine;

public class DI_system : MonoBehaviour
{
    public static DI_system Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private DamageIndicator indicatorPrefab;
    [SerializeField] private RectTransform holder;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player;

    private Dictionary<Transform, DamageIndicator> indicators = new();
    
    void Awake()
    {
        Instance = this;
        if (!mainCamera) mainCamera = Camera.main;
    }

    public bool IsTargetVisible(Transform target)
    {
        if (!mainCamera || !target) return false;
        
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(target.position);
        return screenPoint.z > 0 && 
               screenPoint.x > 0 && screenPoint.x < 1 &&
               screenPoint.y > 0 && screenPoint.y < 1;
    }

    public void CreateIndicator(Transform target)
    {
        if (!target) return;
        Debug.Log($"Creating indicator for {target.name}");
        if (indicators.TryGetValue(target, out var indicator))
        {
            indicator.RestartTimer();
            return;
        }
        
        var newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, mainCamera, () => {
            if (indicators.ContainsKey(target))
                indicators.Remove(target);
        });
        
        indicators.Add(target, newIndicator);
    }

    void Update()
    {
        // Cleanup destroyed targets
        //Debug.Log($"Current indicators count: {indicators.Count}");
        List<Transform> toRemove = new();
        foreach (var kvp in indicators)
        {
            if (kvp.Key == null) toRemove.Add(kvp.Key);
        }
        foreach (var key in toRemove)
        {
            if (indicators.TryGetValue(key, out var indicator))
                Destroy(indicator.gameObject);
            indicators.Remove(key);
        }
    }
}