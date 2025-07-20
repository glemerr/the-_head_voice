using UnityEngine;

public class PortalRotator : MonoBehaviour
{
    private float rotationSpeed;
    private bool clockwise;

    public void Initialize(float speed, bool isClockwise)
    {
        rotationSpeed = speed;
        clockwise = isClockwise;
    }

    void Update()
    {
        float direction = clockwise ? 1f : -1f;
        transform.Rotate(0, rotationSpeed * direction * Time.deltaTime, 0);
    }
}