using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class AISpecialAttachment_SpawnItemOnCritKill : AISpecialAttachmentBase
{
	// Token: 0x06000507 RID: 1287 RVA: 0x00016BE4 File Offset: 0x00014DE4
	protected override void OnInited()
	{
		this.character.BeforeCharacterSpawnLootOnDead += this.BeforeCharacterSpawnLootOnDead;
		this.SpawnItem().Forget();
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00016C18 File Offset: 0x00014E18
	private UniTaskVoid SpawnItem()
	{
		AISpecialAttachment_SpawnItemOnCritKill.<SpawnItem>d__5 <SpawnItem>d__;
		<SpawnItem>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnItem>d__.<>4__this = this;
		<SpawnItem>d__.<>1__state = -1;
		<SpawnItem>d__.<>t__builder.Start<AISpecialAttachment_SpawnItemOnCritKill.<SpawnItem>d__5>(ref <SpawnItem>d__);
		return <SpawnItem>d__.<>t__builder.Task;
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00016C5B File Offset: 0x00014E5B
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.BeforeCharacterSpawnLootOnDead -= this.BeforeCharacterSpawnLootOnDead;
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00016C84 File Offset: 0x00014E84
	private void BeforeCharacterSpawnLootOnDead(DamageInfo dmgInfo)
	{
		this.hasDead = true;
		Debug.Log(string.Format("Die crit:{0}", dmgInfo.crit));
		bool flag = dmgInfo.crit > 0;
		if (this.inverse == flag || this.character == null)
		{
			if (this.itemInstance != null)
			{
				Object.Destroy(this.itemInstance.gameObject);
			}
			return;
		}
		Debug.Log("pick up on crit");
		if (this.itemInstance != null)
		{
			this.character.CharacterItem.Inventory.AddAndMerge(this.itemInstance, 0);
		}
	}

	// Token: 0x0400043C RID: 1084
	[ItemTypeID]
	public int itemToSpawn;

	// Token: 0x0400043D RID: 1085
	private Item itemInstance;

	// Token: 0x0400043E RID: 1086
	private bool hasDead;

	// Token: 0x0400043F RID: 1087
	public bool inverse;
}
