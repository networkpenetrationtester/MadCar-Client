using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class HAX : MonoBehaviour
{
	// Token: 0x0600184F RID: 6223 RVA: 0x000AE028 File Offset: 0x000AC228
	public HAX()
	{
		this.layerMask = -262145;
		this._targeterRect = new Rect(0f, 50f, 200f, 1000f);
		this._miscRect = new Rect(420f, 50f, 200f, 1000f);
		this._snakeRect = new Rect(210f, 50f, 200f, 1000f);
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000AE0A4 File Offset: 0x000AC2A4
	public void Update()
	{
		if (Input.GetKey(KeyCode.Print))
		{
			Debug.LogError("CLIP THAT!!!");
		}
		if (Input.GetMouseButton(2))
		{
			this.GUIoffset += new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * 10f;
		}
		try
		{
			if (HAX_STORAGE.safeMode && this.rigid.velocity.sqrMagnitude > 25000f)
			{
				this.rigid.drag += 1f;
				this.rigid.angularDrag += 0.5f;
			}
			else
			{
				this.rigid.drag = 0f;
				this.rigid.angularDrag = 0f;
			}
			if (Input.GetKey(KeyCode.LeftControl) && HAX_STORAGE.TARGETER.targetingMode != HAX_STORAGE.TargetingMode.None && HAX_STORAGE.targeterMode)
			{
				this.MasterCar.transform.LookAt(HAX_STORAGE.TARGETER.TargetPosition);
				this.rigid.AddRelativeForce(Vector3.forward * HAX_STORAGE.TARGETER.targetingSpeed, HAX_STORAGE.TARGETER.forceMode);
				this.carController.transform.position = this.rigid.position;
			}
			if (HAX_STORAGE.funnyMode)
			{
				this.rigid.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * HAX_STORAGE.TARGETER.targetingSpeed, HAX_STORAGE.TARGETER.forceMode);
				this.rigid.AddRelativeTorque(Vector3.up * Input.GetAxis("Horizontal") * HAX_STORAGE.TARGETER.targetingSpeed, HAX_STORAGE.TARGETER.forceMode);
				if (Input.GetKey(KeyCode.LeftAlt))
				{
					this.rigid.AddRelativeForce(Vector3.up * HAX_STORAGE.TARGETER.targetingSpeed, HAX_STORAGE.TARGETER.forceMode);
				}
			}
		}
		catch
		{
			base.Invoke("Init", 0.1f);
		}
		if (Input.GetMouseButton(1))
		{
			HAX_STORAGE.TARGETER.targetingMode = HAX_STORAGE.TargetingMode.Cursor;
			try
			{
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out HAX_STORAGE.laserPointerInfo, float.MaxValue);
			}
			catch
			{
			}
		}
		if (Input.GetKey(KeyCode.Keypad4))
		{
			Time.timeScale = 0f;
		}
		if (!Input.GetKey(KeyCode.Keypad4))
		{
			Time.timeScale = 1f;
		}
		if (Input.GetKeyUp(KeyCode.Keypad3))
		{
			HAX_STORAGE.neoMode = !HAX_STORAGE.neoMode;
		}
		if (Input.GetKey(KeyCode.Keypad2))
		{
			Debug.LogError("AppVersion: " + PhotonNetwork.AppVersion);
			Debug.LogError("AuthValues: " + PhotonNetwork.AuthValues.ToString());
			Debug.LogError("AutomaticallySyncScene: " + PhotonNetwork.AutomaticallySyncScene.ToString());
			Debug.LogError("BestRegionSummaryInPreferences: " + PhotonNetwork.BestRegionSummaryInPreferences);
			Debug.LogError("CloudRegion: " + PhotonNetwork.CloudRegion);
			Debug.LogError("ConnectMethod: " + PhotonNetwork.ConnectMethod.ToString());
			Debug.LogError("CrcCheckEnabled: " + PhotonNetwork.CrcCheckEnabled.ToString());
			Debug.LogError("CurrentLobby: " + PhotonNetwork.CurrentLobby.ToString());
			Debug.LogError("CurrentRoom: " + PhotonNetwork.CurrentRoom.ToString());
			Debug.LogError("CurrentCluster: " + PhotonNetwork.CurrentCluster);
			Debug.LogError("GameVersion: " + PhotonNetwork.GameVersion);
			Debug.LogError("Server: " + PhotonNetwork.Server.ToString());
			Debug.LogError("ServerAddress: " + PhotonNetwork.ServerAddress);
			PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(new string[]
			{
				"aaa",
				"aaa"
			});
		}
		if (Input.GetKey(KeyCode.Keypad1))
		{
			string text = HAX_STORAGE.laserPointerInfo.collider.name + ": " + HAX_STORAGE.laserPointerInfo.collider.gameObject.layer.ToString();
			if (this.laserInsight != text)
			{
				Debug.LogError(text);
				this.laserInsight = text;
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad6))
		{
			HAX_STORAGE.SeizeRoom();
			Hashtable hashtable = new Hashtable();
			hashtable["test"] = "dunce hat";
			HAX_STORAGE.TARGETER.TargetGameObject.GetPhotonView().Controller.SetCustomProperties(hashtable, null, null);
		}
		if (Input.GetKey(KeyCode.Keypad7))
		{
			Debug.LogError(HAX_STORAGE.TARGETER.TargetGameObject.GetPhotonView().Controller.CustomProperties["test"]);
		}
		if (Input.GetKeyUp(KeyCode.Keypad5))
		{
			HAX_STORAGE.Punish(HAX_STORAGE.TARGETER.TargetGameObject.GetPhotonView(), HAX_STORAGE.PunishmentType.FRY);
		}
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000AE588 File Offset: 0x000AC788
	public void OnGUI()
	{
		GUILayout.BeginArea(new Rect(100f + this.GUIoffset.x, 500f + this.GUIoffset.y, (float)Screen.width, (float)Screen.height));
		GUILayout.Space(10f);
		this.toggleGUI = GUILayout.Toggle(this.toggleGUI, "Enable GUI", Array.Empty<GUILayoutOption>());
		if (this.toggleGUI)
		{
			GUILayout.BeginArea(this._targeterRect);
			GUILayout.Label("TARGETER", Array.Empty<GUILayoutOption>());
			GUILayout.Label("Targeters: " + HAX_STORAGE.TARGETER.cars.size.ToString(), Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Spawn Targeter", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.CreateCar(HAX_STORAGE.BotType.Targeter);
			}
			if (GUILayout.Button("Destroy One", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.DestroyCar(HAX_STORAGE.BotType.Targeter, false);
			}
			if (GUILayout.Button("Destroy All", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.DestroyCar(HAX_STORAGE.BotType.Targeter, true);
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Speed: ", Array.Empty<GUILayoutOption>());
			try
			{
				HAX_STORAGE.TARGETER.targetingSpeed = Mathf.Max(float.MinValue, Mathf.Min(float.MaxValue, float.Parse(GUILayout.TextField(HAX_STORAGE.TARGETER.targetingSpeed.ToString(), 100, Array.Empty<GUILayoutOption>()))));
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("Force Mode: " + HAX_STORAGE.TARGETER.forceMode.ToString(), Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Acceleration", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.forceMode = ForceMode.Acceleration;
			}
			if (GUILayout.Button("Force", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.forceMode = ForceMode.Force;
			}
			if (GUILayout.Button("Impulse", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.forceMode = ForceMode.Impulse;
			}
			if (GUILayout.Button("VelocityChange", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.forceMode = ForceMode.VelocityChange;
			}
			GUILayout.Label("Targeting Mode: " + HAX_STORAGE.TARGETER.targetingMode.ToString(), Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Player", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.targetingMode = HAX_STORAGE.TargetingMode.Player;
			}
			if (GUILayout.Button("Cursor", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.targetingMode = HAX_STORAGE.TargetingMode.Cursor;
			}
			if (GUILayout.Button("Reset", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TARGETER.targetingMode = HAX_STORAGE.TargetingMode.None;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			try
			{
				GUILayout.Label("Stepping Rate: ", Array.Empty<GUILayoutOption>());
				HAX_STORAGE.TARGETER.steppingRate = Mathf.Max(float.MinValue, Mathf.Min(float.MaxValue, float.Parse(GUILayout.TextField(HAX_STORAGE.TARGETER.steppingRate.ToString(), 100, Array.Empty<GUILayoutOption>()))));
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			HAX_STORAGE.TARGETER.useStepping = GUILayout.Toggle(HAX_STORAGE.TARGETER.useStepping, "Use Stepping", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			try
			{
				GUILayout.Label("Explosion Strength: ", Array.Empty<GUILayoutOption>());
				HAX_STORAGE.TARGETER.explosionStrength = Mathf.Max(float.MinValue, Mathf.Min(float.MaxValue, float.Parse(GUILayout.TextField(HAX_STORAGE.TARGETER.explosionStrength.ToString(), 100, Array.Empty<GUILayoutOption>()))));
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			HAX_STORAGE.TARGETER.useExplosionMode = GUILayout.Toggle(HAX_STORAGE.TARGETER.useExplosionMode, "Use Explosion Mode", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.TARGETER.useGravity = GUILayout.Toggle(HAX_STORAGE.TARGETER.useGravity, "Use Gravity", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.TARGETER.useNoClip = GUILayout.Toggle(HAX_STORAGE.TARGETER.useNoClip, "Use Noclip", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.TARGETER.reuseMode = GUILayout.Toggle(HAX_STORAGE.TARGETER.reuseMode, "Reuse Targeters Mode", Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
			GUILayout.BeginArea(this._snakeRect);
			GUILayout.Label("SNAKE", Array.Empty<GUILayoutOption>());
			GUILayout.Label("Snakes: " + HAX_STORAGE.SNAKE.cars.size.ToString(), Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Spawn Snake", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.CreateCar(HAX_STORAGE.BotType.Snake);
			}
			if (GUILayout.Button("Destroy One", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.DestroyCar(HAX_STORAGE.BotType.Snake, false);
			}
			if (GUILayout.Button("Destroy All", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.DestroyCar(HAX_STORAGE.BotType.Snake, true);
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(this._miscRect);
			GUILayout.Label("MISCELLANEOUS", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Master: " + PhotonNetwork.IsMasterClient.ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.Label("ping: " + PhotonNetwork.GetPing().ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Seize Room", Array.Empty<GUILayoutOption>()) && !PhotonNetwork.IsMasterClient)
			{
				HAX_STORAGE.SeizeRoom();
			}
			if (GUILayout.Button((PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.red) ? "rebel" : "align", Array.Empty<GUILayoutOption>()))
			{
				if (PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.red)
				{
					PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.none);
					Debug.LogError("MAD mode enabled.");
				}
				else
				{
					PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.red);
					Debug.LogError("MAD mode disabled.");
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Set MaxPlayers", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.SeizeRoom();
				PhotonNetwork.CurrentRoom.MaxPlayers = (byte)HAX_STORAGE.maxPlayers;
				Debug.LogError("MaxPlayers set to " + HAX_STORAGE.maxPlayers.ToString() + ".");
			}
			try
			{
				HAX_STORAGE.maxPlayers = Math.Max(0, Math.Min(int.Parse(GUILayout.TextField(HAX_STORAGE.maxPlayers.ToString(), 10, Array.Empty<GUILayoutOption>())), 255));
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button(">:3", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.SeizeRoom();
				PhotonNetwork.AutomaticallySyncScene = true;
				PhotonNetwork.LoadLevel(GameProperties.CarShowroom_Scene_Name);
				MCM_SpawnPlayerCar mcm_SpawnPlayerCar = UnityEngine.Object.FindObjectOfType<MCM_SpawnPlayerCar>();
				mcm_SpawnPlayerCar._playerListEntries.Clear();
				mcm_SpawnPlayerCar.UpdatePlayerListEntries();
			}
			if (GUILayout.Button("Map 1", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.SeizeRoom();
				PhotonNetwork.AutomaticallySyncScene = true;
				PhotonNetwork.LoadLevel(GameProperties.Scene1_Name);
				MCM_SpawnPlayerCar mcm_SpawnPlayerCar2 = UnityEngine.Object.FindObjectOfType<MCM_SpawnPlayerCar>();
				mcm_SpawnPlayerCar2._playerListEntries.Clear();
				mcm_SpawnPlayerCar2.UpdatePlayerListEntries();
			}
			if (GUILayout.Button("Map 2", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.SeizeRoom();
				PhotonNetwork.AutomaticallySyncScene = true;
				PhotonNetwork.LoadLevel(GameProperties.Scene2_Name);
				MCM_SpawnPlayerCar mcm_SpawnPlayerCar3 = UnityEngine.Object.FindObjectOfType<MCM_SpawnPlayerCar>();
				mcm_SpawnPlayerCar3._playerListEntries.Clear();
				mcm_SpawnPlayerCar3.UpdatePlayerListEntries();
			}
			if (GUILayout.Button("Map 3", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.SeizeRoom();
				PhotonNetwork.AutomaticallySyncScene = true;
				PhotonNetwork.LoadLevel(GameProperties.Scene3_Name);
				MCM_SpawnPlayerCar mcm_SpawnPlayerCar4 = UnityEngine.Object.FindObjectOfType<MCM_SpawnPlayerCar>();
				mcm_SpawnPlayerCar4._playerListEntries.Clear();
				mcm_SpawnPlayerCar4.UpdatePlayerListEntries();
			}
			GUILayout.EndHorizontal();
			HAX_STORAGE.targeterMode = GUILayout.Toggle(HAX_STORAGE.targeterMode, "Targeter Mode", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.funnyMode = GUILayout.Toggle(HAX_STORAGE.funnyMode, "Funny Mode", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.safeMode = GUILayout.Toggle(HAX_STORAGE.safeMode, "Safe Mode", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.gayMode = GUILayout.Toggle(HAX_STORAGE.gayMode, "Gay Mode", Array.Empty<GUILayoutOption>());
			HAX_STORAGE.playerNoclip = GUILayout.Toggle(HAX_STORAGE.playerNoclip, "You Noclip", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Hack Person", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.TakeOwnership(HAX_STORAGE.TARGETER.TargetGameObject.GetPhotonView(), false);
			}
			if (GUILayout.Button("Un-hack Person", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.ReturnOwnership(HAX_STORAGE.TARGETER.TargetGameObject.GetPhotonView(), false);
			}
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Add Spring :3", Array.Empty<GUILayoutOption>()))
			{
				SpringJoint springJoint = HAX_STORAGE.TARGETER.TargetGameObject.AddComponent<SpringJoint>();
				springJoint.transform.SetParent(HAX_STORAGE.TARGETER.TargetGameObject.transform);
				springJoint.connectedBody = this.rigid;
			}
			if (GUILayout.Button("TP to Person", Array.Empty<GUILayoutOption>()))
			{
				this.rigid.position = HAX_STORAGE.TARGETER.TargetGameObject.transform.position;
				this.carController.transform.position = HAX_STORAGE.TARGETER.TargetGameObject.transform.position;
			}
			GUILayout.Label("Collision Punishment: " + HAX_STORAGE.collisionPunishmentType.ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Min. Pun. Speed: ", Array.Empty<GUILayoutOption>());
			try
			{
				HAX_STORAGE.collisionPunishmentSpeedMin = Mathf.Max(0f, Mathf.Min(float.Parse(GUILayout.TextField(HAX_STORAGE.collisionPunishmentSpeedMin.ToString(), 100, Array.Empty<GUILayoutOption>())), float.MaxValue));
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Banish Height: ", Array.Empty<GUILayoutOption>());
			try
			{
				HAX_STORAGE.banishHeight = Mathf.Max(0f, Mathf.Min(float.Parse(GUILayout.TextField(HAX_STORAGE.banishHeight.ToString(), 100, Array.Empty<GUILayoutOption>())), float.MaxValue));
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("-", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.NONE;
			}
			if (GUILayout.Button("B", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.BANISH;
			}
			if (GUILayout.Button("D", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.DISABLE;
			}
			if (GUILayout.Button("K", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.KICK;
			}
			if (GUILayout.Button("C", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.CRASH;
			}
			if (GUILayout.Button("!", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.KILL;
			}
			if (GUILayout.Button("F", Array.Empty<GUILayoutOption>()))
			{
				HAX_STORAGE.collisionPunishmentType = HAX_STORAGE.PunishmentType.FRY;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		GUILayout.EndArea();
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000AEF20 File Offset: 0x000AD120
	public void NeoNick()
	{
		if (HAX_STORAGE.neoMode && this.init)
		{
			PhotonNetwork.NickName = "Ne0x" + UnityEngine.Random.Range(0, 16777215).ToString("X") + "@localhost:~$ _";
			this.MasterCar.GetComponent<Nametag>().ownerNameMap.text = PhotonNetwork.NickName;
		}
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x000AEF84 File Offset: 0x000AD184
	public void Init()
	{
		if (this.init)
		{
			base.CancelInvoke("Init");
			return;
		}
		try
		{
			this.MasterCar = HAX_STORAGE.MasterCar;
			this.photonView = this.MasterCar.GetPhotonView();
			this.rigid = this.MasterCar.GetComponent<Rigidbody>();
			this.carController = this.MasterCar.GetComponent<RCC_CarControllerV3>();
			this.colliders = this.MasterCar.GetComponentsInChildren<Collider>();
			this.carColorManager = this.MasterCar.GetComponent<CarsBodyColor_Game>();
			HAX_STORAGE.hax = this;
		}
		catch
		{
			base.Invoke("Init", 0.1f);
		}
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000AF034 File Offset: 0x000AD234
	public Vector3 HsvToVector(float H, float S, float V)
	{
		float num;
		for (num = H; num < 0f; num += 360f)
		{
		}
		while (num >= 360f)
		{
			num -= 360f;
		}
		float i3;
		float i2;
		float i;
		if ((double)V <= 0.0)
		{
			i = (i2 = (i3 = 0f));
		}
		else if ((double)S <= 0.0)
		{
			i3 = V;
			i = V;
			i2 = V;
		}
		else
		{
			float num2 = num / 60f;
			int num3 = (int)Mathf.Floor(num2);
			float num4 = num2 - (float)num3;
			float num5 = V * (1f - S);
			float num6 = V * (1f - S * num4);
			float num7 = V * (1f - S * (1f - num4));
			switch (num3)
			{
			case -1:
				i2 = V;
				i = num5;
				i3 = num6;
				break;
			case 0:
				i2 = V;
				i = num7;
				i3 = num5;
				break;
			case 1:
				i2 = num6;
				i = V;
				i3 = num5;
				break;
			case 2:
				i2 = num5;
				i = V;
				i3 = num7;
				break;
			case 3:
				i2 = num5;
				i = num6;
				i3 = V;
				break;
			case 4:
				i2 = num7;
				i = num5;
				i3 = V;
				break;
			case 5:
				i2 = V;
				i = num5;
				i3 = num6;
				break;
			case 6:
				i2 = V;
				i = num7;
				i3 = num5;
				break;
			default:
				i3 = V;
				i = V;
				i2 = V;
				break;
			}
		}
		return new Vector3(this.Clamp(i2), this.Clamp(i), this.Clamp(i3));
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x00022D68 File Offset: 0x00020F68
	public float Clamp(float i)
	{
		if (i < 0f)
		{
			return 0f;
		}
		if (i > 1f)
		{
			return 1f;
		}
		return i;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000AF17C File Offset: 0x000AD37C
	public void Gay()
	{
		if (!HAX_STORAGE.gayMode)
		{
			return;
		}
		try
		{
			this.carColorManager.ChangeCarColor(this.HsvToVector(Mathf.Cos(this.hueShifter) * 360f, 1f, 1f), 0f, 1f);
			this.carColorManager.ChangeCarBodyColor();
			this.hueShifter += 0.1f;
		}
		catch
		{
		}
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x000AF1FC File Offset: 0x000AD3FC
	public void Awake()
	{
		base.InvokeRepeating("Gay", 0f, 0.1f);
		base.InvokeRepeating("NeoNick", 0f, 0.1f);
		PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.red);
		base.Invoke("Init", 0f);
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x000AF250 File Offset: 0x000AD450
	public void OnCollisionEnter(Collision collision)
	{
		PhotonView componentInParent = collision.collider.GetComponentInParent<PhotonView>();
		if (this.photonView.OwnerActorNr != componentInParent.OwnerActorNr && collision.relativeVelocity.sqrMagnitude >= HAX_STORAGE.collisionPunishmentSpeedMin)
		{
			HAX_STORAGE.Punish(componentInParent, HAX_STORAGE.collisionPunishmentType);
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06001859 RID: 6233 RVA: 0x000AF29C File Offset: 0x000AD49C
	public bool init
	{
		get
		{
			return null != this.rigid && null != this.carController && null != this.carColorManager && null != this.MasterCar && null != this.photonView && this.colliders != null;
		}
	}

	// Token: 0x040015DB RID: 5595
	public bool toggleGUI;

	// Token: 0x040015DC RID: 5596
	public int layerMask;

	// Token: 0x040015DD RID: 5597
	public Rect _targeterRect;

	// Token: 0x040015DE RID: 5598
	public Rect _miscRect;

	// Token: 0x040015DF RID: 5599
	public Rect _snakeRect;

	// Token: 0x040015E0 RID: 5600
	public Rigidbody rigid;

	// Token: 0x040015E1 RID: 5601
	public RCC_CarControllerV3 carController;

	// Token: 0x040015E2 RID: 5602
	public GameObject MasterCar;

	// Token: 0x040015E3 RID: 5603
	public string laserInsight;

	// Token: 0x040015E4 RID: 5604
	public GameObject tmp;

	// Token: 0x040015E5 RID: 5605
	public CarsBodyColor_Game carColorManager;

	// Token: 0x040015E6 RID: 5606
	[Range(0f, 6.2831855f)]
	public float hueShifter;

	// Token: 0x040015E7 RID: 5607
	public PhotonView photonView;

	// Token: 0x040015E8 RID: 5608
	public Collider[] colliders;

	// Token: 0x040015E9 RID: 5609
	public Vector2 GUIoffset;

	// Token: 0x020003BB RID: 955
	public class Targeter : MonoBehaviourPun
	{
		// Token: 0x0600185A RID: 6234 RVA: 0x000AF2FC File Offset: 0x000AD4FC
		public void Update()
		{
			if (HAX_STORAGE.safeMode && this.rigid.velocity.sqrMagnitude > 25000f)
			{
				this.rigid.drag += 1f;
				this.rigid.angularDrag += 0.5f;
			}
			else
			{
				this.rigid.drag = 0f;
				this.rigid.angularDrag = 0f;
			}
			this.rigid.useGravity = HAX_STORAGE.TARGETER.useGravity;
			if (Input.GetKey(KeyCode.LeftControl) && HAX_STORAGE.TARGETER.targetingMode != HAX_STORAGE.TargetingMode.None)
			{
				this.carController.transform.position = this.rigid.position;
				base.transform.LookAt(HAX_STORAGE.TARGETER.TargetPosition);
				this.rigid.AddRelativeForce(Vector3.forward * HAX_STORAGE.TARGETER.targetingSpeed, HAX_STORAGE.TARGETER.forceMode);
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000AF3E8 File Offset: 0x000AD5E8
		public void Awake()
		{
			this.rigid = base.GetComponentInParent<Rigidbody>();
			this.colliders = base.GetComponentsInParent<Collider>();
			this.wheelColliders = base.GetComponentsInParent<WheelCollider>();
			this.carController = base.GetComponentInParent<RCC_CarControllerV3>();
			base.InvokeRepeating("Stepping", 0.01f, HAX_STORAGE.TARGETER.steppingRate);
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00022D87 File Offset: 0x00020F87
		public void Stepping()
		{
			if (HAX_STORAGE.TARGETER.useStepping)
			{
				this.rigid.velocity = Vector3.zero;
				this.rigid.angularVelocity = Vector3.zero;
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x000AF43C File Offset: 0x000AD63C
		public void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.Equals(HAX_STORAGE.TARGETER.TargetGameObject))
			{
				if (HAX_STORAGE.TARGETER.useExplosionMode)
				{
					this.rigid.AddExplosionForce(HAX_STORAGE.TARGETER.explosionStrength, this.rigid.position, 100f, 0f, HAX_STORAGE.TARGETER.forceMode);
				}
				if (HAX_STORAGE.TARGETER.reuseMode)
				{
					base.Invoke("Reuse", 0.5f);
				}
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00022DB8 File Offset: 0x00020FB8
		public void Reuse()
		{
			base.transform.position = HAX_STORAGE.MasterCar.transform.position + Vector3.up * 10f;
		}

		// Token: 0x040015EA RID: 5610
		public Rigidbody rigid;

		// Token: 0x040015EB RID: 5611
		public PhotonView view;

		// Token: 0x040015EC RID: 5612
		public Collider[] colliders;

		// Token: 0x040015ED RID: 5613
		public RCC_CarControllerV3 carController;

		// Token: 0x040015EE RID: 5614
		public WheelCollider[] wheelColliders;
	}

	// Token: 0x020003BC RID: 956
	public class Snake : MonoBehaviour
	{
		// Token: 0x06001860 RID: 6240 RVA: 0x000AF4A4 File Offset: 0x000AD6A4
		public void Awake()
		{
			this.rigid = base.gameObject.GetComponent<Rigidbody>();
			this.charJoint = base.gameObject.AddComponent<ConfigurableJoint>();
			if (HAX_STORAGE.SNAKE.cars.size > 0)
			{
				Transform transform = HAX_STORAGE.SNAKE.LastCar.transform;
				base.gameObject.transform.position = transform.position - transform.forward * 6.9f;
				base.gameObject.transform.rotation = transform.rotation;
			}
			try
			{
				this.charJoint.connectedBody = HAX_STORAGE.SNAKE.LastCar.GetComponentInParent<Rigidbody>();
			}
			catch
			{
				this.charJoint.connectedBody = HAX_STORAGE.MasterCar.GetComponentInParent<Rigidbody>();
			}
			this.rigid.isKinematic = false;
			this.charJoint.anchor = Vector3.forward * 6f;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0001222E File Offset: 0x0001042E
		public void Update()
		{
		}

		// Token: 0x040015EF RID: 5615
		public ConfigurableJoint hingeJoint;

		// Token: 0x040015F0 RID: 5616
		public Rigidbody rigid;

		// Token: 0x040015F1 RID: 5617
		public int MyIndex = -1;

		// Token: 0x040015F2 RID: 5618
		public GameObject Parent;

		// Token: 0x040015F3 RID: 5619
		public GameObject Child;

		// Token: 0x040015F4 RID: 5620
		public ConfigurableJoint charJoint;
	}
}
