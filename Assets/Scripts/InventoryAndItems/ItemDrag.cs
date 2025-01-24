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

        if (targetSlot.type == SlotType.InHand)
        {
            originalSlot.inventory.EquipWeapon(targetSlot.SlotIndex, (Weapon)originalSlot.GetCurrentItem);
            return;
        }

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
        Destroy(gameObject);
        originalSlot.inventory.inventoryData[originalSlot.SlotIndex] = null;
        originalSlot.inventory.ClearAndUpdateSlots();
    }
}