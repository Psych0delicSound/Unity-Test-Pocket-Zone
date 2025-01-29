using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private InventoryController inventory;

    void Start() => inventory = FindObjectOfType<InventoryController>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item") || col.CompareTag("Weapon"))
        {
            Item item = col.GetComponent<Item>();
            if (item != null)
            {
                inventory.AddItem(item);
            }
        }
    }
}
