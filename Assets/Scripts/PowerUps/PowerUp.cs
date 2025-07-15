using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public float duration = 5f;
    public string powerUpName;
    public string description;
    public Sprite icon;
    public float effectStrength = 1f;
    public float extraEffectStrength = 0f;
    public float cooldownTime = 10f;


    // This method will be called when the power-up is picked up

    public abstract void Activate(GameObject FirstPersonController);
}
