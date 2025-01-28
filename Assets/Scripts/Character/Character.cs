using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] int maxHealth,
                        currentHealth,
                        strength;
	[SerializeField] protected float movementSpeed;
    Rigidbody2D rb;
    [NonSerialized] public bool isDead = false;
    public Transform weaponPosition;
    protected Weapon equippedWeapon;
    public virtual Weapon GetEquippedWeapon => equippedWeapon;
    protected Vector2 lookingAngle;
    [SerializeField] float cooldownTime = 1f;
    float cooldown = 0f;
    public HPBar hpBar;
    [NonSerialized] public GameController gameController;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
		rb = GetComponent<Rigidbody2D>();

        gameController = FindAnyObjectByType<GameController>();
    }

    protected virtual void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

	protected virtual void Move(Vector2 direction)
	{
		rb.velocity = direction * movementSpeed;
	}

    public virtual void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        hpBar.ChangeValue((float) currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected bool CanAttack => cooldown <= 0;

    virtual public void Attack()
    {
        if (isDead || !CanAttack) return;

        if (equippedWeapon != null) equippedWeapon.Attack(this, lookingAngle);
        //else if () AttackUnarmed();
    }

    virtual public void AttackUnarmed(Character targetCharacter)
    {
        if (isDead || !CanAttack) return;

        targetCharacter.TakeDamage(strength);
        cooldown = cooldownTime;
    }

    public void Reload()
    {
        if (equippedWeapon is WeaponFirearm)
        {
            ((WeaponFirearm)equippedWeapon).Reload();
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} has died");
    }
}