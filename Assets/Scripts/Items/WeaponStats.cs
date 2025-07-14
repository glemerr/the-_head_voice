[System.Serializable]
public class WeaponStats
{
    public float damage;
    public float fireRate;
    public float projectileSpeed;
    public int clipSize;
    public float reloadTime;

    public WeaponStats(float dmg, float rate, float speed, int size, float reload)
    {
        damage = dmg;
        fireRate = rate;
        projectileSpeed = speed;
        reloadTime = reload;
        clipSize = size;

    }

    public WeaponStats Clone()
    {
        return new WeaponStats(damage, fireRate, projectileSpeed, clipSize, reloadTime);
    }
}