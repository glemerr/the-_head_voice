using UnityEngine;

[CreateAssetMenu(fileName = "Gun1", menuName = "FPS/Guns/Gun1", order = 1)]
public class Gun1 : GunBase
{
    public override void Fire(Transform firePoint, Camera playerCamera, LineRenderer lineRenderer)
    {
        // Instantiate the projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Grab (or add) a Rigidbody on the projectile
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        Bullet bullet = proj.GetComponent<Bullet>();
        if (rb == null)
            rb = proj.AddComponent<Rigidbody>();

        // Ensure gravity is enabled
        //rb.useGravity = true;
        rb.mass = 1f; // tweak if you like

        // Compute initial velocity vector
        Vector3 dir = playerCamera.transform.forward;
        Vector3 initVel = dir * projectileSpeed;
        bullet.direction = dir.normalized; // store direction for bullet logic
        bullet.speed = projectileSpeed; // set speed for bullet logic
        bullet.damage = damage; // set damage for bullet logic
        // Assign velocity to the rigidbody
        rb.linearVelocity = initVel;

        //Debug.Log("Fired"+ initVel);
        // Optionally adjust linear damping to simulate air resistance
        rb.linearDamping = 0f;
        
        
        // Clean up after timeToLive seconds
        Destroy(proj, timeToLive);

    // Optional: draw predicted trajectory using physics params
    DrawTrajectory( firePoint.position,
                    initVel,
                    Physics.gravity,
                    timeToLive,
                    lineRenderer );
}

private void DrawTrajectory(
    Vector3 start,
    Vector3 initialVelocity,
    Vector3 gravity,
    float totalTime,
    LineRenderer lr,
    int resolution = 30
)
{
    lr.positionCount = resolution + 1;
    for (int i = 0; i <= resolution; i++)
    {
        float t = totalTime * i / resolution;
        // s = s0 + v0*t + ½·g·t²
        Vector3 pos = start
                    + initialVelocity * t
                    + 0.5f * gravity * t * t;
        lr.SetPosition(i, pos);
    }
}
}
