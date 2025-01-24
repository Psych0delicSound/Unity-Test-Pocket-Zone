using UnityEngine;
using UnityEngine.Events;

public class WeaponFirearm : Weapon
{
    public int bulletsLoaded = 0, bulletsLoadLimit = 8;
    GameObject projectilePrefab;
    public event UnityAction OnBulletsChanged;

    void Start()
    {
        projectilePrefab = Resources.Load("Prefabs/General/Projectile") as GameObject;
    }

    public override void Attack(Character attacker, Vector2 direction)
    {
        if (!CanAttack()) return;
        if (bulletsLoaded < 1)
        {
            Reload();
            return;
        }

        ShootProjectile(attacker.weaponPosition, direction);
        cooldown = cooldownTime;
    }

    public void Reload()
    {
        Debug.Log("R");
    }

    private void ShootProjectile(Transform projectileSpawnpoint, Vector2 direction)
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.identity);

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