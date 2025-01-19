using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
	Vector2 movementInput;
    Weapon equippedWeapon;

    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        base.Move(movementInput);
    }

	protected override void Attack(Character target)
	{
        if ( equippedWeapon == null) return; //target == null ||

        base.Attack(target);
	}

	protected void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); 
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
        Debug.Log("Equipped: " + newWeapon.GetType().Name);
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
    }
}