using UnityEngine;

public class WeaponMelee : Weapon
{
    public override void Attack(Character attacker, Character target)
    {
        if (!CanAttack()) return;

        float distance = Vector3.Distance(attacker.transform.position, target.transform.position);
        if (distance <= range)
        {
            Debug.Log(attacker.gameObject.name + " attacked " + target.gameObject.name + " with a melee weapon.");
            target.TakeDamage(damage);
            cooldown = 1f;
        }
        else
        {
            Debug.Log(target.gameObject.name + " is out of range.");
        }
    }
}