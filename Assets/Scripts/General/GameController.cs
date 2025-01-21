using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Slot inHand;
	public Player player;
	WeaponFirearm wf;
	

	public void UpdateBulletsCount()
	{
		if (player.GetWeapon() is WeaponFirearm)
		{
			wf = player.GetWeapon() as WeaponFirearm;
			inHand.UpdateBulletsNumber(wf.bulletsLoaded);
		}
	}
}