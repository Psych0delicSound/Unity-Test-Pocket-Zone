using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Slot originalSlot;
    private RectTransform rectTransform;
    private Item item;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        item = GetComponent<Item>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSlot = originalParent.GetComponent<Slot>();

        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Slot targetSlot = GetDropSlot(eventData);
        HandleDropLogic(targetSlot);
    }

    private Slot GetDropSlot(PointerEventData eventData)
    {
        GameObject dropObject = eventData.pointerEnter;
        return dropObject?.GetComponent<Slot>() ?? dropObject?.GetComponentInParent<Slot>();
    }

    private void HandleDropLogic(Slot targetSlot)
{
    if (!ValidateDrop(targetSlot))
    {
        ResetPosition();
        return;
    }

    if (targetSlot.type == SlotType.Destroyer)
    {
        HandleItemDestroy();
        return;
    }

    // Handle dragging to InHand slot
    if (targetSlot.type == SlotType.InHand)
    {
        Item draggedItem = originalSlot.GetCurrentItem;
        Item equippedItem = targetSlot.GetCurrentItem;

        if (equippedItem != null)
        {
            // Swap equipped item back to original inventory slot
            originalSlot.SetCurrentItem(equippedItem);
            equippedItem.transform.SetParent(originalSlot.transform);
            if (originalSlot.type == SlotType.Inventory)
                originalSlot.inventory.inventoryData[originalSlot.SlotIndex] = equippedItem;
        }
        else if (originalSlot.type == SlotType.Inventory)
        {
            // Clear original inventory slot
            originalSlot.inventory.inventoryData[originalSlot.SlotIndex] = null;
        }

        // Equip the dragged item
        originalSlot.inventory.EquipWeapon((Weapon)draggedItem);
        return;
    }

    // Handle dragging from InHand to Inventory
    if (originalSlot.type == SlotType.InHand && targetSlot.type == SlotType.Inventory)
    {
        Item equippedItem = originalSlot.GetCurrentItem;
        Item targetItem = targetSlot.GetCurrentItem;

        if (targetItem != null)
        {
            if (!(targetItem is Weapon))
            {
                ResetPosition();
                return;
            }
            // Swap: equip target item and move equipped item to inventory
            originalSlot.inventory.EquipWeapon((Weapon)targetItem);
            targetSlot.SetCurrentItem(equippedItem);
            equippedItem.transform.SetParent(targetSlot.transform);
            originalSlot.inventory.inventoryData[targetSlot.SlotIndex] = equippedItem;
        }
        else
        {
            // Move equipped weapon to inventory
            targetSlot.SetCurrentItem(equippedItem);
            equippedItem.transform.SetParent(targetSlot.transform);
            originalSlot.inventory.inventoryData[targetSlot.SlotIndex] = equippedItem;
            originalSlot.SetCurrentItem(null);
            originalSlot.inventory.gameController.player.EquipWeapon(null);
        }
        originalSlot.inventory.ClearAndUpdateSlots();
        return;
    }

    // Default inventory-to-inventory move
    originalSlot.inventory.HandleItemMove(originalSlot.SlotIndex, targetSlot.SlotIndex);
}

    private bool ValidateDrop(Slot targetSlot)
    {
        if (originalSlot == null ||
            targetSlot == null ||
            originalSlot.inventory == null ||
            targetSlot.type == SlotType.InHand && !(item is Weapon))
                return false;

        return true;
    }

    private void ResetPosition()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    void HandleItemDestroy()
    {
        
        if (originalSlot.type == SlotType.InHand)
        {
            originalSlot.inventory.gameController.player.EquipWeapon(null);
            originalSlot.SetCurrentItem(null);
        }
        else originalSlot.inventory.inventoryData[originalSlot.SlotIndex] = null;
        originalSlot.inventory.ClearAndUpdateSlots();
        Destroy(gameObject);
    }
}