using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Buffs;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class ItemSetting_Gun : ItemSettingBase
{
	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600082B RID: 2091 RVA: 0x00024F3D File Offset: 0x0002313D
	public int TargetBulletID
	{
		get
		{
			return this.targetBulletID;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600082C RID: 2092 RVA: 0x00024F48 File Offset: 0x00023148
	public string CurrentBulletName
	{
		get
		{
			if (this.TargetBulletID < 0)
			{
				return LocalizationManager.ToPlainText("UI_Bullet_NotAssigned");
			}
			return ItemAssetsCollection.GetMetaData(this.TargetBulletID).DisplayName;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x0600082D RID: 2093 RVA: 0x00024F7C File Offset: 0x0002317C
	public int BulletCount
	{
		get
		{
			if (this.loadingBullets)
			{
				return -1;
			}
			if (this.bulletCount < 0)
			{
				this.bulletCount = this.GetBulletCount();
			}
			return this.bulletCount;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600082F RID: 2095 RVA: 0x00024FC8 File Offset: 0x000231C8
	// (set) Token: 0x0600082E RID: 2094 RVA: 0x00024FA3 File Offset: 0x000231A3
	private int bulletCount
	{
		get
		{
			return this._bulletCountCache;
		}
		set
		{
			this._bulletCountCache = value;
			base.Item.Variables.SetInt(this.bulletCountHash, this._bulletCountCache);
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000830 RID: 2096 RVA: 0x00024FD0 File Offset: 0x000231D0
	public int Capacity
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemSetting_Gun.CapacityHash));
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x00024FE7 File Offset: 0x000231E7
	public bool LoadingBullets
	{
		get
		{
			return this.loadingBullets;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000832 RID: 2098 RVA: 0x00024FEF File Offset: 0x000231EF
	public bool LoadBulletsSuccess
	{
		get
		{
			return this.loadBulletsSuccess;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000833 RID: 2099 RVA: 0x00024FF7 File Offset: 0x000231F7
	public int OverrideTriggerMode
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemSetting_Gun.OverrideTriggerModeHash));
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000834 RID: 2100 RVA: 0x00025010 File Offset: 0x00023210
	public ItemSetting_Gun.TriggerModes currentTriggerMode
	{
		get
		{
			int overrideTriggerMode = this.OverrideTriggerMode;
			if (overrideTriggerMode > 0)
			{
				switch (overrideTriggerMode)
				{
				case 1:
					return ItemSetting_Gun.TriggerModes.bolt;
				case 2:
					return ItemSetting_Gun.TriggerModes.semi;
				case 3:
					return ItemSetting_Gun.TriggerModes.auto;
				}
			}
			return this.triggerMode;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000836 RID: 2102 RVA: 0x00025053 File Offset: 0x00023253
	// (set) Token: 0x06000835 RID: 2101 RVA: 0x0002504A File Offset: 0x0002324A
	public Item PreferdBulletsToLoad
	{
		get
		{
			return this.preferedBulletsToLoad;
		}
		set
		{
			this.preferedBulletsToLoad = value;
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0002505B File Offset: 0x0002325B
	public void SetTargetBulletType(Item bulletItem)
	{
		if (bulletItem != null)
		{
			this.SetTargetBulletType(bulletItem.TypeID);
			return;
		}
		this.SetTargetBulletType(-1);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0002507C File Offset: 0x0002327C
	public void SetTargetBulletType(int typeID)
	{
		bool flag = false;
		if (this.TargetBulletID != typeID && this.TargetBulletID != -1)
		{
			flag = true;
		}
		this.targetBulletID = typeID;
		if (flag)
		{
			this.TakeOutAllBullets();
		}
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x000250B0 File Offset: 0x000232B0
	public override void Start()
	{
		base.Start();
		this.AutoSetTypeInInventory(null);
		CustomData entry = base.Item.Variables.GetEntry(this.bulletCountHash);
		if (entry != null)
		{
			entry.Display = true;
		}
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x000250EC File Offset: 0x000232EC
	public void UseABullet()
	{
		if (LevelManager.Instance.IsBaseLevel)
		{
			return;
		}
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null) && item.StackCount >= 1)
			{
				item.StackCount--;
				break;
			}
		}
		this.bulletCount--;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00025174 File Offset: 0x00023374
	public bool IsFull()
	{
		return this.bulletCount >= this.Capacity;
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00025188 File Offset: 0x00023388
	public bool IsValidBullet(Item newBulletItem)
	{
		if (newBulletItem == null)
		{
			return false;
		}
		if (!newBulletItem.Tags.Contains(GameplayDataSettings.Tags.Bullet))
		{
			return false;
		}
		Item currentLoadedBullet = this.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null && currentLoadedBullet.TypeID == newBulletItem.TypeID && this.bulletCount >= this.Capacity)
		{
			return false;
		}
		string @string = newBulletItem.Constants.GetString(this.caliberHash, null);
		string string2 = base.Item.Constants.GetString(this.caliberHash, null);
		return !(@string != string2);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0002521C File Offset: 0x0002341C
	public bool LoadSpecificBullet(Item newBulletItem)
	{
		Debug.Log("尝试安装指定弹药");
		if (!this.IsValidBullet(newBulletItem))
		{
			return false;
		}
		Debug.Log("指定弹药判定通过");
		ItemAgent_Gun itemAgent_Gun = base.Item.ActiveAgent as ItemAgent_Gun;
		if (!(itemAgent_Gun != null))
		{
			Inventory inventory = base.Item.InInventory;
			if (inventory != null && inventory != CharacterMainControl.Main.CharacterItem.Inventory)
			{
				inventory = null;
			}
			this.preferedBulletsToLoad = newBulletItem;
			this.LoadBulletsFromInventory(inventory).Forget();
			return true;
		}
		if (itemAgent_Gun.Holder != null)
		{
			bool flag = itemAgent_Gun.CharacterReload(newBulletItem);
			Debug.Log(string.Format("角色reload:{0}", flag));
			return true;
		}
		return false;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x000252D8 File Offset: 0x000234D8
	public UniTaskVoid LoadBulletsFromInventory(Inventory inventory)
	{
		ItemSetting_Gun.<LoadBulletsFromInventory>d__50 <LoadBulletsFromInventory>d__;
		<LoadBulletsFromInventory>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<LoadBulletsFromInventory>d__.<>4__this = this;
		<LoadBulletsFromInventory>d__.inventory = inventory;
		<LoadBulletsFromInventory>d__.<>1__state = -1;
		<LoadBulletsFromInventory>d__.<>t__builder.Start<ItemSetting_Gun.<LoadBulletsFromInventory>d__50>(ref <LoadBulletsFromInventory>d__);
		return <LoadBulletsFromInventory>d__.<>t__builder.Task;
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00025324 File Offset: 0x00023524
	public bool AutoSetTypeInInventory(Inventory inventory)
	{
		string @string = base.Item.Constants.GetString(this.caliberHash, null);
		Item currentLoadedBullet = this.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null)
		{
			this.SetTargetBulletType(currentLoadedBullet);
			return false;
		}
		if (inventory == null)
		{
			return false;
		}
		foreach (Item item in inventory)
		{
			if (item.GetBool("IsBullet", false) && !(item.Constants.GetString(this.caliberHash, null) != @string))
			{
				this.SetTargetBulletType(item);
				break;
			}
		}
		return this.targetBulletID != -1;
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000253E0 File Offset: 0x000235E0
	public int GetBulletCount()
	{
		int num = 0;
		if (base.Item == null)
		{
			return 0;
		}
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00025450 File Offset: 0x00023650
	public Item GetCurrentLoadedBullet()
	{
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				return item;
			}
		}
		return null;
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x000254AC File Offset: 0x000236AC
	public int GetBulletCountofTypeInInventory(int bulletItemTypeID, Inventory inventory)
	{
		if (this.targetBulletID == -1)
		{
			return 0;
		}
		int num = 0;
		foreach (Item item in inventory)
		{
			if (!(item == null) && item.TypeID == bulletItemTypeID)
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00025518 File Offset: 0x00023718
	public void TakeOutAllBullets()
	{
		if (base.Item == null)
		{
			return;
		}
		if (LevelManager.Instance == null)
		{
			return;
		}
		List<Item> list = new List<Item>();
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				list.Add(item);
			}
		}
		CharacterMainControl characterMainControl = base.Item.GetCharacterMainControl();
		if (base.Item.InInventory && base.Item.InInventory == LevelManager.Instance.PetProxy.Inventory)
		{
			characterMainControl = LevelManager.Instance.MainCharacter;
		}
		else if (base.Item.PluggedIntoSlot != null && base.Item.PluggedIntoSlot.Master != null && base.Item.PluggedIntoSlot.Master.InInventory && base.Item.PluggedIntoSlot.Master.InInventory == LevelManager.Instance.PetProxy.Inventory)
		{
			characterMainControl = LevelManager.Instance.MainCharacter;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Item item2 = list[i];
			if (!(item2 == null))
			{
				if (characterMainControl)
				{
					item2.Drop(characterMainControl, true);
					characterMainControl.PickupItem(item2);
				}
				else
				{
					bool flag = false;
					Inventory inInventory = base.Item.InInventory;
					if (inInventory)
					{
						flag = inInventory.AddAndMerge(item2, 0);
					}
					if (!flag)
					{
						item2.Detach();
						ItemTreeExtensions.DestroyTree(item2);
					}
				}
			}
		}
		this.bulletCount = 0;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x000256DC File Offset: 0x000238DC
	public Dictionary<int, BulletTypeInfo> GetBulletTypesInInventory(Inventory inventory)
	{
		Dictionary<int, BulletTypeInfo> dictionary = new Dictionary<int, BulletTypeInfo>();
		string @string = base.Item.Constants.GetString(this.caliberHash, null);
		foreach (Item item in inventory)
		{
			if (!(item == null) && item.GetBool("IsBullet", false) && !(item.Constants.GetString(this.caliberHash, null) != @string))
			{
				if (!dictionary.ContainsKey(item.TypeID))
				{
					BulletTypeInfo bulletTypeInfo = new BulletTypeInfo();
					bulletTypeInfo.bulletTypeID = item.TypeID;
					bulletTypeInfo.count = item.StackCount;
					dictionary.Add(bulletTypeInfo.bulletTypeID, bulletTypeInfo);
				}
				else
				{
					dictionary[item.TypeID].count += item.StackCount;
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x000257D4 File Offset: 0x000239D4
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsGun", true, true);
	}

	// Token: 0x040007AF RID: 1967
	private int targetBulletID = -1;

	// Token: 0x040007B0 RID: 1968
	public ADSAimMarker adsAimMarker;

	// Token: 0x040007B1 RID: 1969
	public GameObject muzzleFxPfb;

	// Token: 0x040007B2 RID: 1970
	public Projectile bulletPfb;

	// Token: 0x040007B3 RID: 1971
	public string shootKey = "Default";

	// Token: 0x040007B4 RID: 1972
	public string reloadKey = "Default";

	// Token: 0x040007B5 RID: 1973
	private int bulletCountHash = "BulletCount".GetHashCode();

	// Token: 0x040007B6 RID: 1974
	private int _bulletCountCache = -1;

	// Token: 0x040007B7 RID: 1975
	private static int CapacityHash = "Capacity".GetHashCode();

	// Token: 0x040007B8 RID: 1976
	private bool loadingBullets;

	// Token: 0x040007B9 RID: 1977
	private bool loadBulletsSuccess;

	// Token: 0x040007BA RID: 1978
	private int caliberHash = "Caliber".GetHashCode();

	// Token: 0x040007BB RID: 1979
	private static int OverrideTriggerModeHash = "OverrideTriggerMode".GetHashCode();

	// Token: 0x040007BC RID: 1980
	[SerializeField]
	public ItemSetting_Gun.TriggerModes triggerMode;

	// Token: 0x040007BD RID: 1981
	public ItemSetting_Gun.ReloadModes reloadMode;

	// Token: 0x040007BE RID: 1982
	public bool autoReload;

	// Token: 0x040007BF RID: 1983
	public ElementTypes element;

	// Token: 0x040007C0 RID: 1984
	public Buff buff;

	// Token: 0x040007C1 RID: 1985
	private Item preferedBulletsToLoad;

	// Token: 0x0200048A RID: 1162
	public enum TriggerModes
	{
		// Token: 0x04001C29 RID: 7209
		auto,
		// Token: 0x04001C2A RID: 7210
		semi,
		// Token: 0x04001C2B RID: 7211
		bolt
	}

	// Token: 0x0200048B RID: 1163
	public enum ReloadModes
	{
		// Token: 0x04001C2D RID: 7213
		fullMag,
		// Token: 0x04001C2E RID: 7214
		singleBullet
	}
}
