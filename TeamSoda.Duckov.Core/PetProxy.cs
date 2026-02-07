using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class PetProxy : MonoBehaviour
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002A536 File Offset: 0x00028736
	public static PetProxy Instance
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.PetProxy;
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002A551 File Offset: 0x00028751
	public static Inventory PetInventory
	{
		get
		{
			if (PetProxy.Instance == null)
			{
				return null;
			}
			return PetProxy.Instance.Inventory;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x0600096C RID: 2412 RVA: 0x0002A56C File Offset: 0x0002876C
	public static bool InventoryEmpty
	{
		get
		{
			return !(PetProxy.Instance == null) && !(PetProxy.Instance.Inventory == null) && PetProxy.Instance.Inventory.GetItemCount() <= 0;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002A5A6 File Offset: 0x000287A6
	public Inventory Inventory
	{
		get
		{
			return this.inventory;
		}
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0002A5AE File Offset: 0x000287AE
	private void Start()
	{
		SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		if (LevelConfig.SavePet)
		{
			ItemSavesUtilities.LoadInventory("Inventory_Safe", this.inventory).Forget();
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0002A5DD File Offset: 0x000287DD
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0002A5F0 File Offset: 0x000287F0
	private void OnCollectSaveData()
	{
		if (LevelConfig.SavePet)
		{
			this.inventory.Save("Inventory_Safe");
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0002A60C File Offset: 0x0002880C
	public void DestroyItemInBase()
	{
		if (!this.Inventory)
		{
			return;
		}
		List<Item> list = new List<Item>();
		foreach (Item item in this.Inventory)
		{
			list.Add(item);
		}
		foreach (Item item2 in list)
		{
			if (item2.Tags.Contains("DestroyInBase"))
			{
				ItemTreeExtensions.DestroyTree(item2);
			}
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0002A6C0 File Offset: 0x000288C0
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (LevelManager.Instance.PetCharacter == null)
		{
			return;
		}
		base.transform.position = LevelManager.Instance.PetCharacter.transform.position;
		if (this.checkTimer > 0f)
		{
			this.checkTimer -= Time.unscaledDeltaTime;
			return;
		}
		if (CharacterMainControl.Main.PetCapcity != this.inventory.Capacity)
		{
			this.inventory.SetCapacity(CharacterMainControl.Main.PetCapcity);
		}
		this.checkTimer = 1f;
	}

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04000890 RID: 2192
	private float checkTimer = 0.02f;
}
