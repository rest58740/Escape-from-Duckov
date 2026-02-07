using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class ItemStarterkit : InteractableBase
{
	// Token: 0x06000748 RID: 1864 RVA: 0x00020FBF File Offset: 0x0001F1BF
	protected override bool IsInteractable()
	{
		return !this.picked && this.cached;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00020FD8 File Offset: 0x0001F1D8
	private UniTask CacheItems()
	{
		ItemStarterkit.<CacheItems>d__10 <CacheItems>d__;
		<CacheItems>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CacheItems>d__.<>4__this = this;
		<CacheItems>d__.<>1__state = -1;
		<CacheItems>d__.<>t__builder.Start<ItemStarterkit.<CacheItems>d__10>(ref <CacheItems>d__);
		return <CacheItems>d__.<>t__builder.Task;
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0002101B File Offset: 0x0001F21B
	protected override void Awake()
	{
		base.Awake();
		SavesSystem.OnCollectSaveData += this.Save;
		SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00021045 File Offset: 0x0001F245
	protected override void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
		SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
		base.OnDestroy();
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0002106F File Offset: 0x0001F26F
	private void OnStartedLoadingScene(SceneLoadingContext context)
	{
		this.picked = false;
		this.Save();
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0002107E File Offset: 0x0001F27E
	private void Save()
	{
		SavesSystem.Save<bool>(this.saveKey, this.picked);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00021094 File Offset: 0x0001F294
	private void Load()
	{
		this.picked = SavesSystem.Load<bool>(this.saveKey);
		base.MarkerActive = !this.picked;
		if (this.notPickedItem)
		{
			GameObject gameObject = this.notPickedItem;
			if (gameObject != null)
			{
				gameObject.SetActive(!this.picked);
			}
		}
		if (this.pickedItem)
		{
			this.pickedItem.SetActive(this.picked);
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00021106 File Offset: 0x0001F306
	protected override void Start()
	{
		base.Start();
		this.Load();
		if (!this.picked)
		{
			this.CacheItems().Forget();
		}
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00021128 File Offset: 0x0001F328
	protected override void OnInteractFinished()
	{
		foreach (Item item in this.itemsCache)
		{
			ItemUtilities.SendToPlayerCharacter(item, false);
		}
		this.picked = true;
		base.MarkerActive = !this.picked;
		this.itemsCache.Clear();
		this.OnPicked();
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x000211A4 File Offset: 0x0001F3A4
	private void OnPicked()
	{
		if (this.notPickedItem)
		{
			this.notPickedItem.SetActive(false);
		}
		if (this.pickedItem)
		{
			this.pickedItem.SetActive(true);
		}
		if (this.pickFX)
		{
			this.pickFX.SetActive(true);
		}
		NotificationText.Push(LocalizationManager.ToPlainText(this.notificationTextKey));
	}

	// Token: 0x040006F8 RID: 1784
	[ItemTypeID]
	[SerializeField]
	private List<int> items;

	// Token: 0x040006F9 RID: 1785
	[SerializeField]
	private GameObject notPickedItem;

	// Token: 0x040006FA RID: 1786
	[SerializeField]
	private GameObject pickedItem;

	// Token: 0x040006FB RID: 1787
	[SerializeField]
	private GameObject pickFX;

	// Token: 0x040006FC RID: 1788
	private List<Item> itemsCache;

	// Token: 0x040006FD RID: 1789
	[SerializeField]
	private string notificationTextKey;

	// Token: 0x040006FE RID: 1790
	private bool caching;

	// Token: 0x040006FF RID: 1791
	private bool cached;

	// Token: 0x04000700 RID: 1792
	private bool picked;

	// Token: 0x04000701 RID: 1793
	private string saveKey = "StarterKit_Picked";
}
