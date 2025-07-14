using UnityEngine;

public class FireSpan : Bullet
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float fireTime = 10f;
    public float fireOff = 10f;
    public GameObject fire;
    public float SpawTimeFire = 0;
    private bool FireOnFire = true;

    public float burnDuration = 3f;
    public float burnTickRate = 1f;
    public float burnDamagePerTick = 2f;
    private Camera playerCamera;
    protected override void Start()
    {
        fire.SetActive(FireOnFire);
        playerCamera = Camera.main;
        base.Start();
        //Debug.Log(speed);
    }

    // Update is called once per frame
    void Update()
    {
        SpawTimeFire += Time.deltaTime;
        //print(SpawTimeFire);
        if (SpawTimeFire > fireTime)
        {
            if (FireOnFire)
            {
                FireOnFire = false;
                fire.SetActive(FireOnFire);
                SpawTimeFire = 0;
            }
            else
            {
                FireOnFire = true;
                fire.SetActive(FireOnFire);
                SpawTimeFire = 0;
            }
        }
            transform.position = playerCamera.transform.position - playerCamera.transform.forward*2- new Vector3(0, 0.5f, 0);

    // Look and rotate in the direction of the camera
            transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);

    }

    protected override void OnHit(Collider hit)
    {
        var enemy = hit.GetComponent<LifeSystem>();
        if (enemy != null)
        {
            // initial hit
            enemy.TakeDamage(damage);
            // then apply DOT
            enemy.StartCoroutine(Ignite(enemy));
        }

    }
    
        private System.Collections.IEnumerator Ignite(LifeSystem target)
    {
        float elapsed = 0f;
        while (elapsed < burnDuration)
        {
            yield return new WaitForSeconds(burnTickRate);
            target.TakeDamage(burnDamagePerTick);
            elapsed += burnTickRate;
        }
    }
}
