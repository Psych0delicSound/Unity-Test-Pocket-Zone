using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SlotType { Inventory, InHand, Destroyer }

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemDictionary itemDictionary;
    [SerializeField] private int initialSlots = 20;
    [SerializeField] private Transform inventoryT, inventoryContentT, inventoryBinT, attackButtonT, stickRT;
    [SerializeField] private GameObject slotPrefab;
    public GameController gameController;
    public Slot slotInHand;
    private List<Slot> slots = new List<Slot>();
    public List<Item> inventoryData;

    void Start()
    {
        InitializeSlots();
        slotInHand.Initialize(-1, this);
        InventoryTurning();
    }

    public void InventoryTurning()
    {
        inventoryT.gameObject.SetActive(!inventoryT.gameObject.activeSelf);
        inventoryBinT.gameObject.SetActive(inventoryT.gameObject.activeSelf);
        if (slotInHand.GetCurrentItem)
            slotInHand.GetCurrentItem.GetComponent<ItemDrag>().enabled = inventoryT.gameObject.activeSelf;
        attackButtonT.gameObject.SetActive(!inventoryT.gameObject.activeSelf);
        stickRT.gameObject.SetActive(!inventoryT.gameObject.activeSelf);
    }

    private void InitializeSlots()
    {
        if (slotPrefab == null || inventoryContentT == null)
        {
            Debug.LogError("Slot prefab or content transform not assigned!");
            return;
        }

        for (int i = 0; i < initialSlots; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryContentT).GetComponent<Slot>();
            if (slot == null)
            {
                Debug.LogError("Slot prefab is missing Slot component!");
                continue;
            }

            slot.Initialize(i, this);
            slots.Add(slot);
        }
    }

    public void HandleItemMove(int originalIndex, int targetIndex)
    {
        if (originalIndex >= inventoryData.Count || targetIndex >= inventoryData.Count)
        {
            if (originalIndex >= slots.Count || targetIndex >= slots.Count) return;
            inventoryData.Add(null);
            targetIndex = inventoryData.Count -1;
        }
        
        Item temp = inventoryData[originalIndex];
        inventoryData[originalIndex] = inventoryData[targetIndex];
        inventoryData[targetIndex] = temp;

        ClearAndUpdateSlots();
    }

    public void AddItem(Item item)
    {
        if (TryStackItem(item) || TryAddToEmptySlot(item))
        {
            ClearAndUpdateSlots();
        }
        else Debug.Log("Inventory full");
    }

    private bool TryStackItem(Item newItem)
    {
        for (int i = 0; i < inventoryData.Count; i++)
        {
            if (inventoryData.Count - 1 < i) return false;
            Item itemForStack = inventoryData[i];
            if (itemForStack == null || itemForStack.id != newItem.id || itemForStack.inStack >= newItem.stackLimit) continue;

            int remainingSpace = newItem.stackLimit - itemForStack.inStack;
            if (newItem.inStack <= remainingSpace)
            {
                itemForStack.inStack += newItem.inStack;
                return true;
            }

            newItem.inStack -= remainingSpace;
            itemForStack.inStack = newItem.stackLimit;
        }
        return false;
    }

    private bool TryAddToEmptySlot(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetCurrentItem != null) continue;

            if (inventoryData.Count - 1 < i) inventoryData.Add(item);
            else inventoryData[i] = item;

            return true;
        }
        return false;
    }

    public void SetInventoryItems(List<SaveDataItem> savedData)
    {
        foreach (SaveDataItem data in savedData)
        {
            Item item = Instantiate(itemDictionary.GetItemPrefab(data.id)).GetComponent<Item>();
            if (item != null)
            {
                item.inStack = data.inStack;
                if (item is WeaponFirearm) ((WeaponFirearm) item).bulletsLoaded = data.bulletsLoaded;
                AddItem(item);
            }
        }
    }

    public List<SaveDataItem> GetInventoryItems()
    {
        List<SaveDataItem> dataInventory = new List<SaveDataItem>();
        foreach (Item item in inventoryData)
        {
            int bullets = item is WeaponFirearm ? ((WeaponFirearm) item).bulletsLoaded : 0;
            dataInventory.Add(new SaveDataItem
            {
                id = item.id,
                inStack = item.inStack,
                bulletsLoaded = bullets
            });
        }
        return dataInventory;
    }

    public void SetEquippedWeapon(SaveDataItem data)
    {
        if (data == null || data.id == -1) return;

        Weapon weapon = Instantiate(itemDictionary.GetItemPrefab(data.id)).GetComponent<Weapon>();
        if (weapon is WeaponFirearm) ((WeaponFirearm) weapon).bulletsLoaded = data.bulletsLoaded;

        EquipWeapon(weapon);
        ClearAndUpdateSlots();
    }

    public SaveDataItem GetEquippedWeapon()
    {
        Weapon weapon = (Weapon)slotInHand.GetCurrentItem;
        if (weapon == null) return new SaveDataItem
        {
            id = -1,
            inStack = 0,
            bulletsLoaded = 0
        };

        int bullets = weapon is WeaponFirearm ? ((WeaponFirearm) weapon).bulletsLoaded : 0;

        return new SaveDataItem
        {
            id = weapon.id,
            inStack = weapon.inStack,
            bulletsLoaded = bullets
        };
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon == null) return;

        gameController.player.EquipWeapon(weapon);
        slotInHand.SetCurrentItem(weapon);
        inventoryData.Remove(weapon);
    }

    public GameObject GetItemPrefab(int id)
    {
        return itemDictionary.GetItemPrefab(id);
    }

    public void UpdateSlotData(Item item)
    {
        int index = inventoryData.IndexOf(item);
        slots[index].SetCurrentItem(item);
        item.gameObject.transform.SetParent(slots[index].transform);
    }

    void ClearStacks()
    {
        int x = inventoryData.Count < slots.Count ? 1 : 0;
        for (int i = 0; i < inventoryData.Count + x; i++)
        {
            slots[i].ClearStack();
        }
    }

    public void ClearAndUpdateSlots()
    {
        ClearStacks();
        inventoryData = inventoryData.Where(item => item != null).ToList();
        for (int i = 0; i < inventoryData.Count; i++)
        {
            UpdateSlotData(inventoryData[i]);
        }
        EquipWeapon(gameController.player.GetEquippedWeapon);
    }

    public int GetAmmoCount(int ammoId)
    {
        return inventoryData
            .Where(item => item != null && item.id == ammoId)
            .Sum(item => item.inStack);
    }

    public void DecreaseItemStack(int itemID, int amount)
    {
        int remaining = amount;
        List<Item> itemsToCheck = inventoryData.Where(item => item != null && item.id == itemID).ToList();

        foreach (Item item in itemsToCheck)
        {
            if (remaining <= 0) break;
            int deduct = Mathf.Min(item.inStack, remaining);
            item.inStack -= deduct;
            remaining -= deduct;

            if (item.inStack <= 0)
            {
                inventoryData.Remove(item);
                Destroy(item.gameObject);
            }
        }

        ClearAndUpdateSlots();
    }
}