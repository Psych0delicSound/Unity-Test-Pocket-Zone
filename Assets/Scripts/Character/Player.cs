using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
	private Vector2 movementInput, directionInput;
    private Weapon equippedWeapon;
    public Weapon GetEquippedWeapon => equippedWeapon;
    [SerializeField] private Transform handWithGunT;

    protected override void Start()
    {
        base.Start();
        directionInput = new Vector2(0, -1);
    }

    protected void Update()
    {
        base.Move(movementInput);
    }

	override public void Attack()
	{
        if ( equippedWeapon == null) return;

        equippedWeapon.Attack(this, directionInput);
	}

	public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); 
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != Vector2.zero) directionInput = context.ReadValue<Vector2>().normalized;
        
        handWithGunT.rotation = Quaternion.LookRotation(Vector3.forward, -directionInput);
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
    }

    public Weapon GetWeapon()
    {
        return equippedWeapon;
    }
}