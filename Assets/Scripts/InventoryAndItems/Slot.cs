using System;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public SlotType type;
    private TextMeshProUGUI stackText;
    private int slotIndex;
    [NonSerialized] public InventoryController inventory;
    private Item currentItem;
    public int SlotIndex => slotIndex;

    public Item GetCurrentItem() => currentItem;

    public void Initialize(int index, InventoryController controller)
    {
        slotIndex = index;
        inventory = controller;
        stackText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateSlot()
    {
        currentItem = inventory.inventoryData[SlotIndex];

        if (currentItem == null)
        {
            stackText.text = "";
            return;
        }

        currentItem.transform.SetParent(transform);
        currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        if (type == SlotType.InHand)
        {
            Debug.Log("InHand");
            if (currentItem is Weapon)
            {
                Debug.Log("Weapon");
                inventory.gameController.player.EquipWeapon((Weapon)currentItem);
            }
            if (currentItem is WeaponFirearm)
            {
                Debug.Log("WeaponFirearm");
                UpdateBulletDisplay();
                ((WeaponFirearm)currentItem).OnBulletsChanged += UpdateBulletDisplay;
                stackText.text = ((WeaponFirearm)currentItem).bulletsLoaded.ToString();
            }
        }
        else
        {
            stackText.text = currentItem.inStack > 1 ? currentItem.inStack.ToString() : "";
        }
    }

    private void UpdateBulletDisplay()
    {
        Item item = inventory.GetSlotItem(slotIndex);
        stackText.text = ((WeaponFirearm)item).bulletsLoaded.ToString();
    }
}