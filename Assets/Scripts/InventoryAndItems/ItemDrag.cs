using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    Player player;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
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
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null) dropSlot = dropItem.GetComponentInParent<Slot>();
            transform.SetParent(originalParent);
        }
        else if (dropSlot != null)
        {
            if (dropSlot.transform.name == "Destroyer")
            {
                Destroy(originalSlot.gameObject);
                Destroy(gameObject);
                return;
            }
            else if (dropSlot.transform.name == "InHand")
            {
                try
                {
                    player.EquipWeapon(originalSlot.currentItem.GetComponent<Weapon>());
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);

                    transform.SetParent(originalParent);
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    originalSlot.UpdateStackNumber(originalSlot.GetComponentInChildren<Item>().inStack);

                    return;
                }
            }
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        originalSlot.UpdateStackNumber(originalSlot.GetComponentInChildren<Item>().inStack);
        dropSlot.UpdateStackNumber(dropSlot.GetComponentInChildren<Item>().inStack);


    }
}
