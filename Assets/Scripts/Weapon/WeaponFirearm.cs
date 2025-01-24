using UnityEngine;
using UnityEngine.Events;

public class WeaponFirearm : Weapon
{
    public int bulletsLoaded = 0, bulletsLoadLimit = 8;
    GameObject projectilePrefab;
    public event UnityAction OnBulletsChanged;

    void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }

    public override void Attack(Character attacker, Vector2 direction)
    {
        if (!CanAttack() || bulletsLoaded < 1) return;

        ShootProjectile(attacker.GetWeaponPosition(), direction);
        cooldown = cooldownTime;
    }

    public void Reload()
    {
        //
    }

    private void ShootProjectile(Vector2 projectileSpawnpoint, Vector2 direction)
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnpoint, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDetails(damage, direction);
            bulletsLoaded--;
            UpdateBulletsNumber(bulletsLoaded);
        }
    }

    public void UpdateBulletsNumber(int newCount)
    {
        bulletsLoaded = newCount;
        OnBulletsChanged?.Invoke();
    }
}