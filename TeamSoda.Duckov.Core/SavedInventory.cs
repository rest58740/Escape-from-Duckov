using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class SavedInventory : MonoBehaviour
{
	// Token: 0x06000803 RID: 2051 RVA: 0x0002466F File Offset: 0x0002286F
	private void Awake()
	{
		if (this.inventory == null)
		{
			this.inventory = base.GetComponent<Inventory>();
		}
		this.Register();
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00024691 File Offset: 0x00022891
	private void Start()
	{
		if (this.registered)
		{
			this.Load();
		}
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x000246A1 File Offset: 0x000228A1
	private void OnDestroy()
	{
		this.Unregsister();
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x000246AC File Offset: 0x000228AC
	private void Register()
	{
		SavedInventory savedInventory;
		if (SavedInventory.activeInventories.TryGetValue(this.key, out savedInventory))
		{
			Debug.LogError("存在多个带有相同Key的Saved Inventory: " + this.key, base.gameObject);
			return;
		}
		SavesSystem.OnCollectSaveData += this.Save;
		this.registered = true;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00024701 File Offset: 0x00022901
	private void Unregsister()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00024714 File Offset: 0x00022914
	private void Save()
	{
		this.inventory.Save(this.key);
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00024727 File Offset: 0x00022927
	private void Load()
	{
		if (this.inventory.Loading)
		{
			Debug.LogError("Inventory is already loading.", base.gameObject);
			return;
		}
		ItemSavesUtilities.LoadInventory(this.key, this.inventory).Forget();
	}

	// Token: 0x0400079A RID: 1946
	[SerializeField]
	private Inventory inventory;

	// Token: 0x0400079B RID: 1947
	[SerializeField]
	private string key = "DefaultSavedInventory";

	// Token: 0x0400079C RID: 1948
	private static Dictionary<string, SavedInventory> activeInventories = new Dictionary<string, SavedInventory>();

	// Token: 0x0400079D RID: 1949
	private bool registered;
}
