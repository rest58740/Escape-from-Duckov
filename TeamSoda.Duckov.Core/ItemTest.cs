using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class ItemTest : MonoBehaviour
{
	// Token: 0x06000D32 RID: 3378 RVA: 0x00037C99 File Offset: 0x00035E99
	public void DoInstantiate()
	{
		this.characterInstance = this.characterTemplate.CreateInstance();
		this.swordInstance = this.swordTemplate.CreateInstance();
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00037CC0 File Offset: 0x00035EC0
	public void EquipSword()
	{
		Item item;
		this.characterInstance.Slots["Weapon"].Plug(this.swordInstance, ref item);
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00037CF0 File Offset: 0x00035EF0
	public void UequipSword()
	{
		this.characterInstance.Slots["Weapon"].Unplug();
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x00037D0D File Offset: 0x00035F0D
	public void DestroyInstances()
	{
		if (this.characterInstance)
		{
			ItemTreeExtensions.DestroyTreeImmediate(this.characterInstance);
		}
		if (this.swordInstance)
		{
			ItemTreeExtensions.DestroyTreeImmediate(this.swordInstance);
		}
	}

	// Token: 0x04000B7A RID: 2938
	public Item characterTemplate;

	// Token: 0x04000B7B RID: 2939
	public Item swordTemplate;

	// Token: 0x04000B7C RID: 2940
	public Item characterInstance;

	// Token: 0x04000B7D RID: 2941
	public Item swordInstance;
}
