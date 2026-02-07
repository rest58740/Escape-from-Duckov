using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class CharacterCreator : MonoBehaviour
{
	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060008BC RID: 2236 RVA: 0x00027714 File Offset: 0x00025914
	public CharacterMainControl characterPfb
	{
		get
		{
			return GameplayDataSettings.Prefabs.CharacterPrefab;
		}
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00027720 File Offset: 0x00025920
	public UniTask<CharacterMainControl> CreateCharacter(Item itemInstance, CharacterModel modelPrefab, Vector3 pos, Quaternion rotation)
	{
		CharacterCreator.<CreateCharacter>d__2 <CreateCharacter>d__;
		<CreateCharacter>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateCharacter>d__.<>4__this = this;
		<CreateCharacter>d__.itemInstance = itemInstance;
		<CreateCharacter>d__.modelPrefab = modelPrefab;
		<CreateCharacter>d__.pos = pos;
		<CreateCharacter>d__.rotation = rotation;
		<CreateCharacter>d__.<>1__state = -1;
		<CreateCharacter>d__.<>t__builder.Start<CharacterCreator.<CreateCharacter>d__2>(ref <CreateCharacter>d__);
		return <CreateCharacter>d__.<>t__builder.Task;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00027784 File Offset: 0x00025984
	public UniTask<Item> LoadOrCreateCharacterItemInstance(int itemTypeID)
	{
		CharacterCreator.<LoadOrCreateCharacterItemInstance>d__3 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.itemTypeID = itemTypeID;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<CharacterCreator.<LoadOrCreateCharacterItemInstance>d__3>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}
}
