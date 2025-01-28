using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary : MonoBehaviour
{
    public List<Enemy> prefabs;
    Dictionary<int, GameObject> enemyDictionary;

    void Awake()
    {
        enemyDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i] != null)
            {
                prefabs[i].id = i;
            }
        }

        foreach (Enemy i in prefabs)
        {
            enemyDictionary[i.id] = i.gameObject;
        }
    }

    public GameObject GetEnemyPrefab(int id)
    {
        enemyDictionary.TryGetValue(id, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"item with id {id} not found");
        }
        return prefab;
    }
}
