using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int damage;
	[SerializeField] float movementSpeed;
    Rigidbody2D rb;

    [NonSerialized] public bool isDead = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
		rb = GetComponent<Rigidbody2D>();
    }

	protected virtual void Move(Vector2 direction)
	{
		rb.velocity = direction * movementSpeed;
	}

    public virtual void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Attack(Character target)
    {
        if (isDead) return;

        Debug.Log($"{gameObject.name} is attacking {target.gameObject.name}");
        target.TakeDamage(damage);
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} has died");
    }
}