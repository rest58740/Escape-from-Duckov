using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Rules;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using Saves;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Token: 0x0200010C RID: 268
public class LevelManager : MonoBehaviour
{
	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x000297FD File Offset: 0x000279FD
	public static LevelManager Instance
	{
		get
		{
			if (!LevelManager.instance)
			{
				LevelManager.SetInstance();
			}
			return LevelManager.instance;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x00029818 File Offset: 0x00027A18
	public static Transform LootBoxInventoriesParent
	{
		get
		{
			if (LevelManager.Instance._lootBoxInventoriesParent == null)
			{
				GameObject gameObject = new GameObject("Loot Box Inventories");
				gameObject.transform.SetParent(LevelManager.Instance.transform);
				LevelManager.Instance._lootBoxInventoriesParent = gameObject.transform;
				LevelManager.LootBoxInventories.Clear();
			}
			return LevelManager.Instance._lootBoxInventoriesParent;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x0002987B File Offset: 0x00027A7B
	public static Dictionary<int, Inventory> LootBoxInventories
	{
		get
		{
			if (LevelManager.Instance._lootBoxInventories == null)
			{
				LevelManager.Instance._lootBoxInventories = new Dictionary<int, Inventory>();
			}
			return LevelManager.Instance._lootBoxInventories;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x000298A2 File Offset: 0x00027AA2
	public bool IsRaidMap
	{
		get
		{
			return LevelConfig.IsRaidMap;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x000298A9 File Offset: 0x00027AA9
	public bool IsBaseLevel
	{
		get
		{
			return LevelConfig.IsBaseLevel;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000928 RID: 2344 RVA: 0x000298B0 File Offset: 0x00027AB0
	public InputManager InputManager
	{
		get
		{
			return this.inputManager;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x000298B8 File Offset: 0x00027AB8
	public CharacterCreator CharacterCreator
	{
		get
		{
			return this.characterCreator;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x000298C0 File Offset: 0x00027AC0
	public ExitCreator ExitCreator
	{
		get
		{
			return this.exitCreator;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x000298C8 File Offset: 0x00027AC8
	public ExplosionManager ExplosionManager
	{
		get
		{
			return this.explosionManager;
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600092C RID: 2348 RVA: 0x000298D0 File Offset: 0x00027AD0
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x000298DC File Offset: 0x00027ADC
	public CharacterMainControl MainCharacter
	{
		get
		{
			return this.mainCharacter;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x000298E4 File Offset: 0x00027AE4
	public CharacterMainControl PetCharacter
	{
		get
		{
			return this.petCharacter;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x000298EC File Offset: 0x00027AEC
	public GameCamera GameCamera
	{
		get
		{
			return this.gameCamera;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x000298F4 File Offset: 0x00027AF4
	public FogOfWarManager FogOfWarManager
	{
		get
		{
			return this.fowManager;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000931 RID: 2353 RVA: 0x000298FC File Offset: 0x00027AFC
	public TimeOfDayController TimeOfDayController
	{
		get
		{
			return this.timeOfDayController;
		}
	}

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06000932 RID: 2354 RVA: 0x00029904 File Offset: 0x00027B04
	// (remove) Token: 0x06000933 RID: 2355 RVA: 0x00029938 File Offset: 0x00027B38
	public static event Action OnLevelBeginInitializing;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06000934 RID: 2356 RVA: 0x0002996C File Offset: 0x00027B6C
	// (remove) Token: 0x06000935 RID: 2357 RVA: 0x000299A0 File Offset: 0x00027BA0
	public static event Action OnLevelInitialized;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06000936 RID: 2358 RVA: 0x000299D4 File Offset: 0x00027BD4
	// (remove) Token: 0x06000937 RID: 2359 RVA: 0x00029A08 File Offset: 0x00027C08
	public static event Action OnAfterLevelInitialized;

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000938 RID: 2360 RVA: 0x00029A3B File Offset: 0x00027C3B
	public AIMainBrain AIMainBrain
	{
		get
		{
			return this.aiMainBrain;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x00029A43 File Offset: 0x00027C43
	public static bool LevelInitializing
	{
		get
		{
			return !(LevelManager.Instance == null) && LevelManager.Instance.initingLevel;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x00029A5E File Offset: 0x00027C5E
	public static bool AfterInit
	{
		get
		{
			return !(LevelManager.Instance == null) && LevelManager.Instance.afterInit;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x00029A79 File Offset: 0x00027C79
	public PetProxy PetProxy
	{
		get
		{
			return this.petProxy;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x0600093C RID: 2364 RVA: 0x00029A81 File Offset: 0x00027C81
	public BulletPool BulletPool
	{
		get
		{
			return this.bulletPool;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x00029A89 File Offset: 0x00027C89
	public CustomFaceManager CustomFaceManager
	{
		get
		{
			return this.customFaceManager;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x0600093E RID: 2366 RVA: 0x00029A91 File Offset: 0x00027C91
	// (set) Token: 0x0600093F RID: 2367 RVA: 0x00029AAC File Offset: 0x00027CAC
	public static string LevelInitializingComment
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance._levelInitializingComment;
		}
		set
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			LevelManager.Instance._levelInitializingComment = value;
			Action<string> onLevelInitializingCommentChanged = LevelManager.OnLevelInitializingCommentChanged;
			if (onLevelInitializingCommentChanged != null)
			{
				onLevelInitializingCommentChanged(value);
			}
			Debug.Log("[Level Initialization] " + value);
		}
	}

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06000940 RID: 2368 RVA: 0x00029AE8 File Offset: 0x00027CE8
	// (remove) Token: 0x06000941 RID: 2369 RVA: 0x00029B1C File Offset: 0x00027D1C
	public static event Action<string> OnLevelInitializingCommentChanged;

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000942 RID: 2370 RVA: 0x00029B4F File Offset: 0x00027D4F
	public static bool LevelInited
	{
		get
		{
			return !(LevelManager.instance == null) && LevelManager.instance.levelInited;
		}
	}

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06000943 RID: 2371 RVA: 0x00029B6C File Offset: 0x00027D6C
	// (remove) Token: 0x06000944 RID: 2372 RVA: 0x00029BA0 File Offset: 0x00027DA0
	public static event Action<EvacuationInfo> OnEvacuated;

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06000945 RID: 2373 RVA: 0x00029BD4 File Offset: 0x00027DD4
	// (remove) Token: 0x06000946 RID: 2374 RVA: 0x00029C08 File Offset: 0x00027E08
	public static event Action<DamageInfo> OnMainCharacterDead;

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000947 RID: 2375 RVA: 0x00029C3B File Offset: 0x00027E3B
	public float LevelTime
	{
		get
		{
			return Time.time - this.levelStartTime;
		}
	}

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x06000948 RID: 2376 RVA: 0x00029C4C File Offset: 0x00027E4C
	// (remove) Token: 0x06000949 RID: 2377 RVA: 0x00029C80 File Offset: 0x00027E80
	public static event Action OnNewGameReport;

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600094A RID: 2378 RVA: 0x00029CB3 File Offset: 0x00027EB3
	public static Ruleset Rule
	{
		get
		{
			return LevelManager.rule;
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00029CBA File Offset: 0x00027EBA
	public static void RegisterWaitForInitialization<T>(T toWait) where T : class, IInitializedQueryHandler
	{
		if (toWait == null)
		{
			return;
		}
		if (toWait == null)
		{
			return;
		}
		LevelManager.waitForInitializationList.Add(toWait);
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00029CDE File Offset: 0x00027EDE
	public static bool UnregisterWaitForInitialization<T>(T obj) where T : class
	{
		return LevelManager.waitForInitializationList.Remove(obj);
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00029CF0 File Offset: 0x00027EF0
	private void Start()
	{
		if (!SceneLoader.IsSceneLoading)
		{
			this.StartInit(default(SceneLoadingContext));
		}
		else
		{
			SceneLoader.onFinishedLoadingScene += this.StartInit;
		}
		if (!SavesSystem.Load<bool>("NewGameReported"))
		{
			SavesSystem.Save<bool>("NewGameReported", true);
			Action onNewGameReport = LevelManager.OnNewGameReport;
			if (onNewGameReport != null)
			{
				onNewGameReport();
			}
		}
		if (GameManager.newBoot)
		{
			this.OnNewBoot();
			GameManager.newBoot = false;
		}
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00029D60 File Offset: 0x00027F60
	private void OnDestroy()
	{
		SceneLoader.onFinishedLoadingScene -= this.StartInit;
		CharacterMainControl characterMainControl = this.mainCharacter;
		if (characterMainControl == null)
		{
			return;
		}
		Health health = characterMainControl.Health;
		if (health == null)
		{
			return;
		}
		health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnMainCharacterDie));
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00029D9E File Offset: 0x00027F9E
	private void OnNewBoot()
	{
		Debug.Log("New boot");
		GameClock.Instance.StepTimeTil(new TimeSpan(7, 0, 0));
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00029DBC File Offset: 0x00027FBC
	private void StartInit(SceneLoadingContext context)
	{
		this.InitLevel(context).Forget();
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00029DD8 File Offset: 0x00027FD8
	private UniTaskVoid InitLevel(SceneLoadingContext context)
	{
		LevelManager.<InitLevel>d__116 <InitLevel>d__;
		<InitLevel>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<InitLevel>d__.<>4__this = this;
		<InitLevel>d__.context = context;
		<InitLevel>d__.<>1__state = -1;
		<InitLevel>d__.<>t__builder.Start<LevelManager.<InitLevel>d__116>(ref <InitLevel>d__);
		return <InitLevel>d__.<>t__builder.Task;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x00029E24 File Offset: 0x00028024
	private UniTask CreateMate()
	{
		LevelManager.<CreateMate>d__117 <CreateMate>d__;
		<CreateMate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateMate>d__.<>4__this = this;
		<CreateMate>d__.<>1__state = -1;
		<CreateMate>d__.<>t__builder.Start<LevelManager.<CreateMate>d__117>(ref <CreateMate>d__);
		return <CreateMate>d__.<>t__builder.Task;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00029E68 File Offset: 0x00028068
	private UniTask WaitForOtherInitialization()
	{
		LevelManager.<WaitForOtherInitialization>d__118 <WaitForOtherInitialization>d__;
		<WaitForOtherInitialization>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<WaitForOtherInitialization>d__.<>1__state = -1;
		<WaitForOtherInitialization>d__.<>t__builder.Start<LevelManager.<WaitForOtherInitialization>d__118>(ref <WaitForOtherInitialization>d__);
		return <WaitForOtherInitialization>d__.<>t__builder.Task;
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00029EA4 File Offset: 0x000280A4
	private void HandleRaidInitialization()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		if (this.IsRaidMap)
		{
			if (currentRaid.ended)
			{
				RaidUtilities.NewRaid();
				this.isNewRaidLevel = true;
				return;
			}
		}
		else if (this.IsBaseLevel && !currentRaid.ended)
		{
			RaidUtilities.NotifyEnd();
		}
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x00029EEC File Offset: 0x000280EC
	public void RefreshMainCharacterFace()
	{
		if (this.mainCharacter.characterModel.CustomFace)
		{
			CustomFaceSettingData saveData = this.customFaceManager.LoadMainCharacterSetting();
			this.mainCharacter.characterModel.CustomFace.LoadFromData(saveData);
		}
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00029F34 File Offset: 0x00028134
	private UniTask CreateMainCharacterAsync(Vector3 position, Quaternion rotation)
	{
		LevelManager.<CreateMainCharacterAsync>d__121 <CreateMainCharacterAsync>d__;
		<CreateMainCharacterAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateMainCharacterAsync>d__.<>4__this = this;
		<CreateMainCharacterAsync>d__.position = position;
		<CreateMainCharacterAsync>d__.rotation = rotation;
		<CreateMainCharacterAsync>d__.<>1__state = -1;
		<CreateMainCharacterAsync>d__.<>t__builder.Start<LevelManager.<CreateMainCharacterAsync>d__121>(ref <CreateMainCharacterAsync>d__);
		return <CreateMainCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x00029F88 File Offset: 0x00028188
	private void SetCharacterItemsInspected()
	{
		foreach (Slot slot in this.mainCharacter.CharacterItem.Slots)
		{
			if (slot.Content != null)
			{
				slot.Content.Inspected = true;
			}
		}
		foreach (Item item in this.mainCharacter.CharacterItem.Inventory)
		{
			if (item != null)
			{
				item.Inspected = true;
			}
		}
		foreach (Item item2 in this.petProxy.Inventory)
		{
			if (item2 != null)
			{
				item2.Inspected = true;
			}
		}
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0002A090 File Offset: 0x00028290
	private static void SetInstance()
	{
		if (LevelManager.instance)
		{
			return;
		}
		LevelManager.instance = Object.FindFirstObjectByType<LevelManager>();
		LevelManager.instance;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0002A0B4 File Offset: 0x000282B4
	private UniTask<Item> LoadOrCreateCharacterItemInstance()
	{
		LevelManager.<LoadOrCreateCharacterItemInstance>d__124 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.<>4__this = this;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<LevelManager.<LoadOrCreateCharacterItemInstance>d__124>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0002A0F7 File Offset: 0x000282F7
	public void NotifyEvacuated(EvacuationInfo info)
	{
		this.mainCharacter.Health.SetInvincible(true);
		Action<EvacuationInfo> onEvacuated = LevelManager.OnEvacuated;
		if (onEvacuated != null)
		{
			onEvacuated(info);
		}
		this.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		SavesSystem.SaveFile(true);
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0002A12C File Offset: 0x0002832C
	public void NotifySaveBeforeLoadScene(bool saveToFile)
	{
		this.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		if (saveToFile)
		{
			SavesSystem.SaveFile(true);
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0002A144 File Offset: 0x00028344
	private void OnMainCharacterDie(DamageInfo dmgInfo)
	{
		if (this.dieTask)
		{
			return;
		}
		this.dieTask = true;
		this.CharacterDieTask(dmgInfo).Forget();
		Action<DamageInfo> onMainCharacterDead = LevelManager.OnMainCharacterDead;
		if (onMainCharacterDead == null)
		{
			return;
		}
		onMainCharacterDead(dmgInfo);
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002A180 File Offset: 0x00028380
	private UniTaskVoid CharacterDieTask(DamageInfo dmgInfo)
	{
		LevelManager.<CharacterDieTask>d__129 <CharacterDieTask>d__;
		<CharacterDieTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CharacterDieTask>d__.<>4__this = this;
		<CharacterDieTask>d__.dmgInfo = dmgInfo;
		<CharacterDieTask>d__.<>1__state = -1;
		<CharacterDieTask>d__.<>t__builder.Start<LevelManager.<CharacterDieTask>d__129>(ref <CharacterDieTask>d__);
		return <CharacterDieTask>d__.<>t__builder.Task;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0002A1CB File Offset: 0x000283CB
	internal void SaveMainCharacter()
	{
		if (!LevelConfig.SaveCharacter)
		{
			return;
		}
		this.mainCharacter.CharacterItem.Save("MainCharacterItemData");
		SavesSystem.Save<float>("MainCharacterHealth", this.MainCharacter.Health.CurrentHealth);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0002A204 File Offset: 0x00028404
	[return: TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})]
	private ValueTuple<string, SubSceneEntry.Location> GetPlayerStartLocation()
	{
		List<ValueTuple<string, SubSceneEntry.Location>> list = new List<ValueTuple<string, SubSceneEntry.Location>>();
		string text = "StartPoints";
		if (LevelManager.loadLevelBeaconIndex > 0)
		{
			text = text + "_" + LevelManager.loadLevelBeaconIndex.ToString();
			LevelManager.loadLevelBeaconIndex = 0;
		}
		foreach (SubSceneEntry subSceneEntry in MultiSceneCore.Instance.SubScenes)
		{
			foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
			{
				if (this.IsPathCompatible(location, text))
				{
					list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry.sceneID, location));
				}
			}
		}
		if (list.Count == 0)
		{
			text = "StartPoints";
			foreach (SubSceneEntry subSceneEntry2 in MultiSceneCore.Instance.SubScenes)
			{
				foreach (SubSceneEntry.Location location2 in subSceneEntry2.cachedLocations)
				{
					if (this.IsPathCompatible(location2, text))
					{
						list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry2.sceneID, location2));
					}
				}
			}
		}
		return list.GetRandom<ValueTuple<string, SubSceneEntry.Location>>();
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0002A394 File Offset: 0x00028594
	private void CreateMainCharacterMapElement()
	{
		if (MultiSceneCore.Instance != null)
		{
			SimplePointOfInterest simplePointOfInterest = this.mainCharacter.gameObject.AddComponent<SimplePointOfInterest>();
			simplePointOfInterest.Color = this.characterMapIconColor;
			simplePointOfInterest.ShadowColor = this.characterMapShadowColor;
			simplePointOfInterest.ShadowDistance = 0f;
			simplePointOfInterest.Setup(this.characterMapIcon, "You", true, null);
		}
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0002A3F3 File Offset: 0x000285F3
	private void OnSubSceneLoaded()
	{
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0002A3F8 File Offset: 0x000285F8
	private bool IsPathCompatible(SubSceneEntry.Location location, string keyWord)
	{
		string path = location.path;
		int num = path.IndexOf('/');
		return num != -1 && path.Substring(0, num) == keyWord;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0002A42C File Offset: 0x0002862C
	public void TestTeleport()
	{
		MultiSceneCore.Instance.LoadAndTeleport(this.testTeleportTarget).Forget<bool>();
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0002A444 File Offset: 0x00028644
	private LevelManager.LevelInfo mGetInfo()
	{
		Scene? activeSubScene = MultiSceneCore.ActiveSubScene;
		string activeSubSceneID = (activeSubScene != null) ? activeSubScene.Value.name : "";
		return new LevelManager.LevelInfo
		{
			isBaseLevel = this.IsBaseLevel,
			sceneName = base.gameObject.scene.name,
			activeSubSceneID = activeSubSceneID
		};
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0002A4B0 File Offset: 0x000286B0
	public static LevelManager.LevelInfo GetCurrentLevelInfo()
	{
		if (LevelManager.Instance == null)
		{
			return default(LevelManager.LevelInfo);
		}
		return LevelManager.Instance.mGetInfo();
	}

	// Token: 0x0400085F RID: 2143
	private Transform _lootBoxInventoriesParent;

	// Token: 0x04000860 RID: 2144
	private Dictionary<int, Inventory> _lootBoxInventories;

	// Token: 0x04000861 RID: 2145
	[SerializeField]
	private Transform defaultStartPos;

	// Token: 0x04000862 RID: 2146
	private static LevelManager instance;

	// Token: 0x04000863 RID: 2147
	[SerializeField]
	private InputManager inputManager;

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private CharacterCreator characterCreator;

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private ExitCreator exitCreator;

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private ExplosionManager explosionManager;

	// Token: 0x04000867 RID: 2151
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x04000868 RID: 2152
	private CharacterMainControl mainCharacter;

	// Token: 0x04000869 RID: 2153
	private CharacterMainControl petCharacter;

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private GameCamera gameCamera;

	// Token: 0x0400086B RID: 2155
	[SerializeField]
	private FogOfWarManager fowManager;

	// Token: 0x0400086C RID: 2156
	[SerializeField]
	private TimeOfDayController timeOfDayController;

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private AIMainBrain aiMainBrain;

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	private CharacterRandomPreset matePreset;

	// Token: 0x04000872 RID: 2162
	private bool initingLevel;

	// Token: 0x04000873 RID: 2163
	private bool isNewRaidLevel;

	// Token: 0x04000874 RID: 2164
	private bool afterInit;

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	private CharacterRandomPreset petPreset;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private Sprite characterMapIcon;

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	private Color characterMapIconColor;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private Color characterMapShadowColor;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private MultiSceneLocation testTeleportTarget;

	// Token: 0x0400087A RID: 2170
	[SerializeField]
	public SkillBase defaultSkill;

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private PetProxy petProxy;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private CustomFaceManager customFaceManager;

	// Token: 0x0400087D RID: 2173
	[SerializeField]
	private BulletPool bulletPool;

	// Token: 0x0400087E RID: 2174
	private string _levelInitializingComment = "";

	// Token: 0x0400087F RID: 2175
	public static int loadLevelBeaconIndex = 0;

	// Token: 0x04000881 RID: 2177
	private bool levelInited;

	// Token: 0x04000882 RID: 2178
	public const string MainCharacterItemSaveKey = "MainCharacterItemData";

	// Token: 0x04000883 RID: 2179
	public const string MainCharacterHealthSaveKey = "MainCharacterHealth";

	// Token: 0x04000886 RID: 2182
	private float levelStartTime = -0.1f;

	// Token: 0x04000888 RID: 2184
	private static Ruleset rule;

	// Token: 0x04000889 RID: 2185
	private static List<object> waitForInitializationList = new List<object>();

	// Token: 0x0400088A RID: 2186
	public static float enemySpawnCountFactor = 1f;

	// Token: 0x0400088B RID: 2187
	public static bool forceBossSpawn = false;

	// Token: 0x0400088C RID: 2188
	private bool dieTask;

	// Token: 0x020004A9 RID: 1193
	[Serializable]
	public struct LevelInfo
	{
		// Token: 0x04001C8E RID: 7310
		public bool isBaseLevel;

		// Token: 0x04001C8F RID: 7311
		public string sceneName;

		// Token: 0x04001C90 RID: 7312
		public string activeSubSceneID;
	}
}
