using System;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int SlotIndex => slotIndex;
    public SlotType type;
    private TextMeshProUGUI stackText;
    private int slotIndex;
    [NonSerialized] public InventoryController inventory;
    private Item currentItem;
    public Item GetCurrentItem => currentItem;
    public void SetCurrentItem(Item newItem) => currentItem = newItem;

    public void Initialize(int index, InventoryController controller)
    {
        slotIndex = index;
        inventory = controller;
        stackText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateSlot()
    {
        if (currentItem == null)
        {
            ClearStack();
            return;
        }

        currentItem.transform.SetParent(transform);
        currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        if (type == SlotType.InHand && currentItem is WeaponFirearm)
        {
            UpdateBulletDisplay();
            ((WeaponFirearm)currentItem).OnBulletsChanged += UpdateBulletDisplay;
        }
        else
        {
            stackText.text = currentItem.inStack > 1 ? currentItem.inStack.ToString() : "";
        }
    }

    private void UpdateBulletDisplay()
    {
        stackText.text = ((WeaponFirearm)currentItem).bulletsLoaded.ToString();
    }

    public void ClearStack()
    {
        stackText.text = "";
    }
}