using System;
using UnityEngine;

public abstract class Weapon : Item
{
    public const int damage = 10;
    public float range = 2f, cooldownTime = 1f;
    [NonSerialized] public float cooldown = 0f;
    protected GameController gameController;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public abstract void Attack(Character attacker, Vector2 direction);

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