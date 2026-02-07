using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class EnemyCreator : MonoBehaviour
{
	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0002CF38 File Offset: 0x0002B138
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0002CF44 File Offset: 0x0002B144
	private void Start()
	{
		Debug.LogError("This scripts shouldn't exist!", this);
		if (LevelManager.LevelInited)
		{
			this.StartCreate();
			return;
		}
		LevelManager.OnLevelInitialized += this.StartCreate;
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0002CF70 File Offset: 0x0002B170
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.StartCreate;
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0002CF84 File Offset: 0x0002B184
	private void StartCreate()
	{
		int creatorID = this.GetCreatorID();
		if (MultiSceneCore.Instance != null)
		{
			if (MultiSceneCore.Instance.usedCreatorIds.Contains(creatorID))
			{
				return;
			}
			MultiSceneCore.Instance.usedCreatorIds.Add(creatorID);
		}
		this.CreateCharacterAsync();
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0002CFD0 File Offset: 0x0002B1D0
	private UniTaskVoid CreateCharacterAsync()
	{
		EnemyCreator.<CreateCharacterAsync>d__11 <CreateCharacterAsync>d__;
		<CreateCharacterAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateCharacterAsync>d__.<>4__this = this;
		<CreateCharacterAsync>d__.<>1__state = -1;
		<CreateCharacterAsync>d__.<>t__builder.Start<EnemyCreator.<CreateCharacterAsync>d__11>(ref <CreateCharacterAsync>d__);
		return <CreateCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0002D014 File Offset: 0x0002B214
	private void PlugAccessories()
	{
		Slot slot = this.character.PrimWeaponSlot();
		Item item = (slot != null) ? slot.Content : null;
		if (item == null)
		{
			return;
		}
		CharacterMainControl characterMainControl = this.character;
		Inventory inventory;
		if (characterMainControl == null)
		{
			inventory = null;
		}
		else
		{
			Item characterItem = characterMainControl.CharacterItem;
			inventory = ((characterItem != null) ? characterItem.Inventory : null);
		}
		Inventory inventory2 = inventory;
		if (inventory2 == null)
		{
			return;
		}
		foreach (Item item2 in inventory2)
		{
			if (!(item2 == null))
			{
				item.TryPlug(item2, true, null, 0);
			}
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0002D0B4 File Offset: 0x0002B2B4
	private UniTask AddBullet()
	{
		EnemyCreator.<AddBullet>d__13 <AddBullet>d__;
		<AddBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<AddBullet>d__.<>4__this = this;
		<AddBullet>d__.<>1__state = -1;
		<AddBullet>d__.<>t__builder.Start<EnemyCreator.<AddBullet>d__13>(ref <AddBullet>d__);
		return <AddBullet>d__.<>t__builder.Task;
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0002D0F8 File Offset: 0x0002B2F8
	private UniTask<List<Item>> GenerateItems()
	{
		EnemyCreator.<GenerateItems>d__14 <GenerateItems>d__;
		<GenerateItems>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<GenerateItems>d__.<>4__this = this;
		<GenerateItems>d__.<>1__state = -1;
		<GenerateItems>d__.<>t__builder.Start<EnemyCreator.<GenerateItems>d__14>(ref <GenerateItems>d__);
		return <GenerateItems>d__.<>t__builder.Task;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0002D13C File Offset: 0x0002B33C
	private UniTask<Item> LoadOrCreateCharacterItemInstance()
	{
		EnemyCreator.<LoadOrCreateCharacterItemInstance>d__15 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.<>4__this = this;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<EnemyCreator.<LoadOrCreateCharacterItemInstance>d__15>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0002D180 File Offset: 0x0002B380
	private int GetCreatorID()
	{
		Transform parent = base.transform.parent;
		string text = base.transform.GetSiblingIndex().ToString();
		while (parent != null)
		{
			text = string.Format("{0}/{1}", parent.GetSiblingIndex(), text);
			parent = parent.parent;
		}
		text = string.Format("{0}/{1}", base.gameObject.scene.buildIndex, text);
		return text.GetHashCode();
	}

	// Token: 0x0400092C RID: 2348
	private CharacterMainControl character;

	// Token: 0x0400092D RID: 2349
	[SerializeField]
	private List<RandomItemGenerateDescription> itemsToGenerate;

	// Token: 0x0400092E RID: 2350
	[SerializeField]
	private ItemFilter bulletFilter;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	private AudioManager.VoiceType voiceType;

	// Token: 0x04000930 RID: 2352
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x04000931 RID: 2353
	[SerializeField]
	private AICharacterController aiController;
}
