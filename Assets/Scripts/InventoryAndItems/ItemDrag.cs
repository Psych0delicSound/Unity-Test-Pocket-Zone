using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    Player player;
    GameController gameController;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
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

        if (dropSlot == null || (originalSlot.type == "InHand" && dropSlot.currentItem.tag != "Weapon") || (dropSlot.type == "InHand" && originalSlot.currentItem.tag != "Weapon"))
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null) dropSlot = dropItem.GetComponentInParent<Slot>();
        }

        if (dropSlot != null)
        {
            if (dropSlot.type == "Destroyer")
            {
                Destroy(originalSlot.gameObject);
                Destroy(gameObject);
                return;
            }
            else if (dropSlot.type == "InHand" && originalSlot.currentItem.tag == "Weapon")
            {
                player.EquipWeapon(originalSlot.currentItem.GetComponent<Weapon>());
                transform.SetParent(dropSlot.transform);
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

            if (originalSlot.currentItem != null) originalSlot.UpdateStackNumber(originalSlot.currentItem.GetComponent<Item>().inStack);
            if (dropSlot.currentItem != null) dropSlot.UpdateStackNumber(dropSlot.currentItem.GetComponent<Item>().inStack);
            if (dropSlot.type == "InHand" && dropSlot.currentItem.tag == "Weapon")
            {
                gameController.UpdateBulletsCount();
                Destroy(originalSlot.gameObject);
            }
        }
        else
        {
            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
