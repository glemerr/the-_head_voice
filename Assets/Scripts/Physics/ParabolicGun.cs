using UnityEngine;

public class ParabolicGun : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float gravity = -9.81f;
    public float timeToLive = 5f;
    public float bounciness = 0.6f; // 0.6 = lose 40% energy per bounce
    public int maxBounces = 3;

    public Vector3 initialVelocity;
    private Vector3 position;
    private Vector3 velocity;
    private float elapsedTime = 0f;
    private float lifeTimer = 0f;
    private int bounceCount = 0;



    void Start()
    {

        float radAngle = Mathf.Atan2(initialVelocity.y, initialVelocity.x); // Ensure angle is between 0 and 90 degrees

        Vector3 dir = firePoint.forward;

        velocity = initialVelocity;
        position = firePoint.position;


    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > timeToLive)
        {
            Destroy(gameObject);
            return;
        }

        ParabolicMove();
    }

    void ParabolicMove()
    {
        float deltaTime = Time.deltaTime;
        elapsedTime += deltaTime;

        // Apply gravity
        velocity.y += gravity * deltaTime;

        // Calculate next position
        Vector3 nextPosition = position + velocity * deltaTime;

        // Simple collision with ground (y = 0)
        if (nextPosition.y <= 0f && velocity.y < 0f)
        {
            nextPosition.y = 0f;
            velocity.y = -velocity.y * bounciness;
            velocity.x *= bounciness;
            velocity.z *= bounciness;

            bounceCount++;
            if (bounceCount >= maxBounces || velocity.magnitude < 1f)
            {
                velocity = Vector3.zero;
                return;
            }
        }

        position = nextPosition;
        transform.position = position;
    }


}
