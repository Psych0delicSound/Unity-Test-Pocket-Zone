using System;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [NonSerialized] public GameObject currentItem;
    TextMeshProUGUI textItemStackCount;

    public void UpdateStackNumber(int inStack)
    {
        textItemStackCount = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (inStack > 1) textItemStackCount.text = $"{inStack}";
        else textItemStackCount.text = "";
    }
}
