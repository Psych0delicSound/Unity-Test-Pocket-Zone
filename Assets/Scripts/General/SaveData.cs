using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SaveData
{
	public Vector3 playerPosition;
	public List<SaveDataItem> saveDataInventory;
	public SaveDataItem equippedWeapon;
}