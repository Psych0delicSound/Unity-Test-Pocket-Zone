using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
	private Vector2 movementInput;
    [SerializeField] private Transform handWithGunT;
    public InventoryController inventoryController;

    protected override void Start()
    {
        base.Start();
        lookingAngle = new Vector2(0, -1);
    }

    protected override void Update()
    {
        base.Move(movementInput);
    }

	public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); 
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != Vector2.zero) lookingAngle = context.ReadValue<Vector2>().normalized;
        
        handWithGunT.rotation = Quaternion.LookRotation(Vector3.forward, -lookingAngle);
    }


}