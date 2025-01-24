using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SlotType { Inventory, InHand, Destroyer }

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemDictionary itemDictionary;
    [SerializeField] private int initialSlots = 20;
    [SerializeField] private Transform inventoryT, inventoryContentT, inventoryActionsT;
    [SerializeField] private GameObject slotPrefab;
    public GameController gameController;
    public Slot slotInHand;
    private List<Slot> slots = new List<Slot>();
    public List<Item> inventoryData;

    void Start()
    {
        InitializeSlots();
        inventoryData = new List<Item>(initialSlots); 
    }

    public void InventoryTurning()
    {
        inventoryT.gameObject.SetActive(!inventoryT.gameObject.activeSelf);
        inventoryActionsT.gameObject.SetActive(inventoryT.gameObject.activeSelf);
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

    public Item GetSlotItem(int slotIndex)
    {
        return inventoryData[slotIndex];
    }

    public void UpdateSlotData(int slotIndex)
    {
        slots[slotIndex].UpdateSlot();
    }

    public void HandleItemMove(int sourceIndex, int targetIndex)
    {
        if (sourceIndex >= inventoryData.Count || targetIndex >= inventoryData.Count)
        {
            if (sourceIndex >= slots.Count || targetIndex >= slots.Count) return;
            inventoryData.Add(null);
            targetIndex = inventoryData.Count -1;
        }
        
        Item temp = inventoryData[sourceIndex];
        inventoryData[sourceIndex] = inventoryData[targetIndex];
        inventoryData[targetIndex] = temp;

        ClearAndUpdateSlots();
    }

    public void AddItem(Item item)
    {
        if (TryStackItem(item)) return;
        if (TryAddToEmptySlot(item)) return;
        
        Debug.Log("Inventory full");
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
                UpdateSlotData(i);
                return true;
            }

            newItem.inStack -= remainingSpace;
            itemForStack.inStack = newItem.stackLimit;
            UpdateSlotData(i);
        }
        return false;
    }

    private bool TryAddToEmptySlot(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetCurrentItem() != null) continue;

            if (inventoryData.Count - 1 < i) inventoryData.Add(item);
            else inventoryData[i] = item;

            ClearAndUpdateSlots();
            return true;
        }
        return false;
    }

    public void SetInventoryItems(List<Item> savedData)
    {
        for (int i = 0; i < savedData.Count; i++)
        {
            AddItem(savedData[i]);
        }
    }
    public GameObject GetItemPrefab(int id)
    {
        return itemDictionary.GetItemPrefab(id);
    }

    public void ClearAndUpdateSlots()
    {
        inventoryData = inventoryData.Where(item => item != null).ToList();
        for (int i = 0; i < inventoryData.Count; i++)
        {
            UpdateSlotData(i);
        }
    }
}