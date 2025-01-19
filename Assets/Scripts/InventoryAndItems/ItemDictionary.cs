using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    Dictionary<int, GameObject> itemDictionary;

    void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].id = i;
            }
        }

        foreach (Item i in itemPrefabs)
        {
            itemDictionary[i.id] = i.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemId)
    {
        itemDictionary.TryGetValue(itemId, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"item with id {itemId} not found");
        }
        return prefab;
    }
}
