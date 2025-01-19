using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    InventoryController inventoryController;
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            Item item = col.GetComponent<Item>();
            if (item != null)
            {
                inventoryController.AddItem(col.gameObject);
                Destroy(col.gameObject);
            }
        }
    }
}
