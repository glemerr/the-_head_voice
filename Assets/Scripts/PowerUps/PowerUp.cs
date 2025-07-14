using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public float duration = 5f;

    public abstract void Activate(GameObject FirstPersonController);
}
