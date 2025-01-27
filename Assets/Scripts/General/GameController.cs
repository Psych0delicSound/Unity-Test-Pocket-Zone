using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
	public Player player;

	[SerializeField] private ItemDictionary itemDictionary;
	[SerializeField] private EnemyDictionary enemyDictionary;

	[SerializeField] private Tilemap tilemapGround;
	List<Vector3> availablePlaces;
	
	public GameObject GetItemPrefab(int id) => itemDictionary.GetItemPrefab(id);
	public GameObject GetEnemyPrefab(int id) => enemyDictionary.GetEnemyPrefab(id);

	public void Start()
	{
		FillAvailablePlacesList();
		InitialSpawnEnemies(GetEnemyPrefab(0), 3);
	}

	public void Spawn(GameObject prefab, Vector2 vectorPos)
	{
		Instantiate(prefab, vectorPos, Quaternion.identity);
	}

	void InitialSpawnEnemies(GameObject prefab, int count)
	{
		for (int i = 0; i < count; i++)
		{
			int randomPlace = Random.Range(0, availablePlaces.Count);
			Vector2 vector = availablePlaces[randomPlace];
			Spawn(prefab, vector);
		}
	}

	void FillAvailablePlacesList()
	{
        availablePlaces = new List<Vector3>();
		BoundsInt boundsInt = tilemapGround.cellBounds;
		TileBase[] allTiles = tilemapGround.GetTilesBlock(boundsInt);
		Vector3 start = tilemapGround.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];
				if (tile != null)
				{
					Vector3 place = start + new Vector3(x + 0.5f, y + 2f, 0);
					availablePlaces.Add(place);
				}
            }
        }
	}
}