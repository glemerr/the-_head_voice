// TestDamageIndicator.cs
using UnityEngine;

public class TestDamageIndicator : MonoBehaviour
{
    [SerializeField][Range(0.1f, 10f)] private float destroyTimer = 1f;

    void Start()
    {
        RegisterDamageIndicator();
    }

    void RegisterDamageIndicator()
    {
        if (!DI_system.Instance.IsTargetVisible(transform))
        {
            DI_system.Instance.CreateIndicator(transform);
        }
        Destroy(gameObject, destroyTimer);
    }
}