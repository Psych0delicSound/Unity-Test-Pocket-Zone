using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    ItemDictionary itemDictionary;
    public Transform inventoryT, inventoryContent;
    public GameObject slotPrefab;

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
    }

    public void InventoryTurning()
    {
        inventoryT.gameObject.SetActive(!inventoryT.gameObject.activeSelf);
    }

    public void AddItem(GameObject itemGO)
    {
        Item itemNew = itemGO.GetComponent<Item>();

        SameItemStackFilling(itemNew);
        if (itemNew.inStack == 0) return;

        Slot slot = Instantiate(slotPrefab, inventoryContent).GetComponent<Slot>();
        slot.currentItem = Instantiate(itemGO, slot.transform);
        slot.UpdateStackNumber(itemNew.inStack);
    }

    void SameItemStackFilling(Item itemNew)
    {
        for (int i = 1; i < inventoryContent.childCount; i++)
        {
            Slot slot = inventoryContent.GetChild(i).GetComponent<Slot>();
            Item itemInSlot = slot.currentItem.GetComponent<Item>();

            if (itemNew.id == itemInSlot.id && itemInSlot.inStack < itemInSlot.stackLimit)
            {
                if (itemInSlot.inStack + itemNew.inStack <= itemNew.stackLimit)
                {
                    itemInSlot.inStack += itemNew.inStack;
                    itemNew.inStack = 0;
                    slot.UpdateStackNumber(itemInSlot.inStack);
                    return;
                }
                else
                {
                    itemNew.inStack -= itemInSlot.stackLimit - itemInSlot.inStack;
                    itemInSlot.inStack = itemInSlot.stackLimit;
                }
            }
        }
    }

    public List<SaveDataInventory> GetInventoryItems()
    {
        List<SaveDataInventory> dataInventory = new List<SaveDataInventory>();
        for (int i = 1; i < inventoryContent.childCount; i++)
        {
            Slot slot = inventoryContent.GetChild(i).GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                dataInventory.Add(new SaveDataInventory
                {
                    itemId = item.id,
                    inStack = item.inStack
                });
            }
        }

        return dataInventory;
    }

    public void SetInventoryItems(List<SaveDataInventory> dataInventory)
    {
        foreach (SaveDataInventory data in dataInventory)
        {
            GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemId);
            if (itemPrefab != null)
            {
                itemPrefab.GetComponent<Item>().inStack = data.inStack;
                AddItem(itemPrefab);
            }
        }
    }
}
