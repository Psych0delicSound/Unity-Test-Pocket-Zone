using UnityEngine;

public class WeaponFirearm : Weapon
{
    public int bulletsLoaded = 0, bulletsLoadLimit = 8;
    public GameObject projectilePrefab;

    public override void Attack(Character attacker, Character target)
    {
        if (!CanAttack()) return;

        Debug.Log($"{attacker.gameObject.name}  shot at  {target.gameObject.name} with a firearm");
        ShootProjectile(target);
        cooldown = 1f;
    }

    private void ShootProjectile(Character target)
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity); //
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(target.transform, damage);
        }
    }
}