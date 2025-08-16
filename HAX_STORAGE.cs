using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020003BE RID: 958
public static class HAX_STORAGE
{
	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06001866 RID: 6246 RVA: 0x00022E15 File Offset: 0x00021015
	public static GameObject MasterCar
	{
		get
		{
			return UnityEngine.Object.FindObjectOfType<RCC_Camera>()._playerCar.gameObject;
		}
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000AF594 File Offset: 0x000AD794
	public static GameObject CreateCar(HAX_STORAGE.BotType botType)
	{
		Transform transform = HAX_STORAGE.MasterCar.transform;
		int @int = PlayerPrefs.GetInt(GamePrefs.Selected_Car_Pref);
		GameObject gameObject = PhotonNetwork.Instantiate(GameProperties.PlayerCars[@int], transform.position - transform.forward * 7f, transform.rotation, 0, null);
		RCC_CarControllerV3 component = gameObject.GetComponent<RCC_CarControllerV3>();
		switch (botType)
		{
		case HAX_STORAGE.BotType.Clone:
			HAX_STORAGE.CLONE.cars.Add(gameObject);
			return gameObject;
		case HAX_STORAGE.BotType.Targeter:
			gameObject.AddComponent<HAX.Targeter>();
			HAX_STORAGE.TARGETER.cars.Add(gameObject);
			component.canControl = false;
			return gameObject;
		case HAX_STORAGE.BotType.Snake:
		{
			Rigidbody rigid = HAX_STORAGE.hax.rigid;
			rigid.velocity = Vector3.zero;
			rigid.angularVelocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.AddComponent<HAX.Snake>();
			HAX_STORAGE.SNAKE.cars.Add(gameObject);
			return gameObject;
		}
		}
		return gameObject;
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06001868 RID: 6248 RVA: 0x00022E26 File Offset: 0x00021026
	public static BetterList<GameObject>[] CarLists
	{
		get
		{
			return new BetterList<GameObject>[]
			{
				HAX_STORAGE.CLONE.cars,
				HAX_STORAGE.TARGETER.cars,
				HAX_STORAGE.DUMMY.cars,
				HAX_STORAGE.SNAKE.cars
			};
		}
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x000AF674 File Offset: 0x000AD874
	public static void DestroyCar(HAX_STORAGE.BotType botType, bool all = false)
	{
		BetterList<GameObject> betterList = HAX_STORAGE.CarLists[(int)botType];
		if (betterList.size > 0)
		{
			PhotonNetwork.Destroy(betterList.Pop());
			if (all)
			{
				HAX_STORAGE.DestroyCar(botType, true);
			}
		}
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x000AF88C File Offset: 0x000ADA8C
	public static void Punish(PhotonView photonViewOther, HAX_STORAGE.PunishmentType punishmentType)
	{
		if (punishmentType == HAX_STORAGE.PunishmentType.NONE)
		{
			return;
		}
		bool flag = photonViewOther.ControllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber;
		if (!flag && photonViewOther.Controller.GetTeam() == PunTeams.Team.red && PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.red)
		{
			return;
		}
		if (!flag)
		{
			HAX_STORAGE.TakeOwnership(photonViewOther, true);
		}
		Player owner = photonViewOther.Owner;
		PhotonView photonView = HAX_STORAGE.hax.photonView;
		for (int i = 0; i < 10; i++)
		{
			HAX_STORAGE.SeizeRoom();
		}
		switch (punishmentType)
		{
		case HAX_STORAGE.PunishmentType.DISABLE:
			PhotonNetwork.DestroyPlayerObjects(owner);
			Debug.LogError("Disabled " + owner.NickName);
			return;
		case HAX_STORAGE.PunishmentType.KICK:
			for (int j = 0; j < 1000; j++)
			{
				photonView.RPC("kicked :3", owner, new object[1000]);
			}
			Debug.LogError("Kicked " + owner.NickName);
			return;
		case HAX_STORAGE.PunishmentType.CRASH:
			for (int k = 0; k < 2; k++)
			{
				photonView.RPC("crashed >:3", owner, new object[100000]);
			}
			Debug.LogError("Crashed " + owner.NickName);
			return;
		case HAX_STORAGE.PunishmentType.KILL:
			for (int l = 0; l < 1000; l++)
			{
				photonView.RPC("DIE >:D", owner, new object[100000]);
			}
			Debug.LogError("KILLED " + owner.NickName);
			return;
		case HAX_STORAGE.PunishmentType.NONE:
			return;
		case HAX_STORAGE.PunishmentType.BANISH:
			photonViewOther.GetComponent<Rigidbody>().AddExplosionForce(HAX_STORAGE.banishHeight, photonViewOther.transform.position, 1f, 1000f, ForceMode.VelocityChange);
			Task.Delay(1000).ContinueWith(delegate(Task t)
			{
				if (!flag)
				{
					HAX_STORAGE.ReturnOwnership(photonViewOther, true);
				}
			});
			Debug.LogError("B A N I S H E D " + owner.NickName);
			return;
		case HAX_STORAGE.PunishmentType.FRY:
			photonViewOther.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * float.MaxValue, ForceMode.VelocityChange);
			Task.Delay(300).ContinueWith(delegate(Task t)
			{
				for (int m = 0; m < 100; m++)
				{
					photonView.RPC("You're fried. >:(", owner, new object[100000]);
				}
				PhotonNetwork.DestroyPlayerObjects(photonViewOther.Owner);
			});
			Debug.LogError("F̴̧̲̹̽̓Ȑ̸̥̎̊Ǐ̸̢̦͝E̷̗͈͒͠D̸͔̙͆͝ " + owner.NickName);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x00022E4E File Offset: 0x0002104E
	public static void TakeOwnership(PhotonView other, bool quiet)
	{
		other.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
		if (!quiet)
		{
			Debug.LogError("Took ownership of " + other.Owner.NickName);
		}
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x00022E7D File Offset: 0x0002107D
	public static void ReturnOwnership(PhotonView other, bool quiet)
	{
		other.ControllerActorNr = other.OwnerActorNr;
		if (!quiet)
		{
			Debug.LogError("Returned ownership to " + other.Owner.NickName);
		}
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x00022EA8 File Offset: 0x000210A8
	public static void SeizeRoom()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
		}
	}

	// Token: 0x040015F7 RID: 5623
	public static RaycastHit laserPointerInfo;

	// Token: 0x040015F8 RID: 5624
	public static string[] PlayerCarNames = new string[]
	{
		"00 Car 43",
		"01 Car 22",
		"02 Car 27",
		"03 Car 23",
		"04 Car 86",
		"05 Car 50",
		"06 Car 17",
		"07 Classic Car",
		"08 Car 45",
		"09 Car 42",
		"10 Car 29",
		"11 Car 1",
		"12 Car 57",
		"13 Car 3",
		"14 Car 60",
		"15 Car 21",
		"16 Police Car 1",
		"17 Police Car 2",
		"18 Car 5",
		"19 Car 10",
		"20 Car 11",
		"21 Car 26",
		"22 Car 35",
		"23 Car 38",
		"24 Car 40",
		"25 Car 48",
		"26 Car 4",
		"27 Car 6",
		"28 Car 8",
		"29 Car 13",
		"30 Car 53",
		"31 Car 49",
		"32 Car 39",
		"33 Car 19",
		"34 Police Car 3",
		"35 Car 20",
		"36 Car 9",
		"37 Car 46",
		"38 Car 33",
		"39 Car 14",
		"40 Car 31",
		"41 Car 36",
		"42 Car 51",
		"43 Car 37",
		"44 Car 58"
	};

	// Token: 0x040015F9 RID: 5625
	public static bool playerNoclip = false;

	// Token: 0x040015FA RID: 5626
	public static bool funnyMode = false;

	// Token: 0x040015FB RID: 5627
	public static int maxPlayers = 10;

	// Token: 0x040015FC RID: 5628
	public static bool safeMode = true;

	// Token: 0x040015FD RID: 5629
	public static bool targeterMode;

	// Token: 0x040015FE RID: 5630
	public static bool gayMode = false;

	// Token: 0x040015FF RID: 5631
	public static bool neoMode = false;

	// Token: 0x04001600 RID: 5632
	public static HAX_STORAGE.PunishmentType collisionPunishmentType = HAX_STORAGE.PunishmentType.NONE;

	// Token: 0x04001601 RID: 5633
	public static float collisionPunishmentSpeedMin = 100f;

	// Token: 0x04001602 RID: 5634
	public static GameObject tmp;

	// Token: 0x04001603 RID: 5635
	public static Dictionary<PhotonView, int> ControlledMap;

	// Token: 0x04001604 RID: 5636
	public static HAX hax;

	// Token: 0x04001605 RID: 5637
	public static float banishHeight = 10f;

	// Token: 0x020003BF RID: 959
	public enum TargetingMode
	{
		// Token: 0x04001607 RID: 5639
		Cursor,
		// Token: 0x04001608 RID: 5640
		Player,
		// Token: 0x04001609 RID: 5641
		None
	}

	// Token: 0x020003C0 RID: 960
	public static class CLONE
	{
		// Token: 0x0400160A RID: 5642
		public static BetterList<GameObject> cars = new BetterList<GameObject>();
	}

	// Token: 0x020003C1 RID: 961
	public static class DUMMY
	{
		// Token: 0x0400160B RID: 5643
		public static BetterList<GameObject> cars = new BetterList<GameObject>();
	}

	// Token: 0x020003C2 RID: 962
	public static class SNAKE
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x00022ED4 File Offset: 0x000210D4
		public static GameObject LastCar
		{
			get
			{
				return HAX_STORAGE.SNAKE.cars[HAX_STORAGE.SNAKE.cars.size - 1];
			}
		}

		// Token: 0x0400160C RID: 5644
		public static BetterList<GameObject> cars = new BetterList<GameObject>();
	}

	// Token: 0x020003C3 RID: 963
	public static class TARGETER
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x000AFB20 File Offset: 0x000ADD20
		public static Vector3 TargetPosition
		{
			get
			{
				if (HAX_STORAGE.TARGETER.targetingMode == HAX_STORAGE.TargetingMode.Cursor)
				{
					return HAX_STORAGE.laserPointerInfo.point;
				}
				if (HAX_STORAGE.TARGETER.targetingMode == HAX_STORAGE.TargetingMode.Player)
				{
					foreach (PhotonView photonView in PhotonNetwork.PhotonViewCollection)
					{
						if (photonView.OwnerActorNr == HAX_STORAGE.TARGETER.targetPhotonViewID)
						{
							return photonView.transform.position;
						}
					}
				}
				return Vector3.zero;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x000AFBAC File Offset: 0x000ADDAC
		public static GameObject TargetGameObject
		{
			get
			{
				foreach (PhotonView photonView in PhotonNetwork.PhotonViewCollection)
				{
					if (photonView.OwnerActorNr == HAX_STORAGE.TARGETER.targetPhotonViewID)
					{
						return photonView.gameObject;
					}
				}
				return null;
			}
		}

		// Token: 0x0400160D RID: 5645
		public static BetterList<GameObject> cars = new BetterList<GameObject>();

		// Token: 0x0400160E RID: 5646
		public static bool useStepping = false;

		// Token: 0x0400160F RID: 5647
		public static bool useGravity = true;

		// Token: 0x04001610 RID: 5648
		public static float steppingRate = 0.01f;

		// Token: 0x04001611 RID: 5649
		public static HAX_STORAGE.TargetingMode targetingMode;

		// Token: 0x04001612 RID: 5650
		public static ForceMode forceMode = ForceMode.Acceleration;

		// Token: 0x04001613 RID: 5651
		public static float targetingSpeed = 100f;

		// Token: 0x04001614 RID: 5652
		public static int targetPhotonViewID = -1;

		// Token: 0x04001615 RID: 5653
		public static bool useNoClip = false;

		// Token: 0x04001616 RID: 5654
		public static bool useExplosionMode;

		// Token: 0x04001617 RID: 5655
		public static bool reuseMode = false;

		// Token: 0x04001618 RID: 5656
		public static float explosionStrength = 1f;
	}

	// Token: 0x020003C4 RID: 964
	public enum BotType
	{
		// Token: 0x0400161A RID: 5658
		Clone,
		// Token: 0x0400161B RID: 5659
		Targeter,
		// Token: 0x0400161C RID: 5660
		Dummy,
		// Token: 0x0400161D RID: 5661
		Snake
	}

	// Token: 0x020003C5 RID: 965
	public enum PunishmentType
	{
		// Token: 0x0400161F RID: 5663
		DISABLE,
		// Token: 0x04001620 RID: 5664
		KICK,
		// Token: 0x04001621 RID: 5665
		CRASH,
		// Token: 0x04001622 RID: 5666
		KILL,
		// Token: 0x04001623 RID: 5667
		NONE,
		// Token: 0x04001624 RID: 5668
		BANISH,
		// Token: 0x04001625 RID: 5669
		FRY
	}
}
