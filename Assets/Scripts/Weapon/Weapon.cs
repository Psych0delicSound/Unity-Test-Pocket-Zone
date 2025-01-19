using System;
using UnityEngine;

public abstract class Weapon : Item
{
    [Header("Weapon Stats")]
    public const int damage = 10;
    public float range = 2f, cooldownTime = 1f;
    [NonSerialized] public float cooldown = 0f;

    public abstract void Attack(Character attacker, Character target);

    protected virtual void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    protected bool CanAttack()
    {
        return cooldown <= 0;
    }
}