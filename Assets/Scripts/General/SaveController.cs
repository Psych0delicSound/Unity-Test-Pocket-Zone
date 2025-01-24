using UnityEngine;
using System.IO;

public class SaveController : MonoBehaviour
{
	string saveLocation;
	InventoryController inventoryController;

	Transform playerT;

	void Start()
	{
		saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
		inventoryController = FindObjectOfType<InventoryController>();
		playerT = GameObject.FindGameObjectWithTag("Player").transform;

		LoadGame();
	}

	void OnApplicationQuit()
	{
		SaveGame();
	}

	public void SaveGame()
	{
		SaveData saveData = new SaveData
		{
			playerPosition = playerT.position,
			saveDataInventory = inventoryController.inventoryData
		};

		File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
	}

	public void LoadGame()
	{
		if (File.Exists(saveLocation))
		{
			SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
			playerT.position = saveData.playerPosition;
			inventoryController.SetInventoryItems(saveData.saveDataInventory);
		}
		else
		{
			SaveGame();
		}
	}
}