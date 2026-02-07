using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class ItemSetting_MeleeWeapon : ItemSettingBase
{
	// Token: 0x06000849 RID: 2121 RVA: 0x00025863 File Offset: 0x00023A63
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0002586B File Offset: 0x00023A6B
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsMeleeWeapon", true, true);
	}

	// Token: 0x040007C4 RID: 1988
	public bool dealExplosionDamage;

	// Token: 0x040007C5 RID: 1989
	public ElementTypes element;

	// Token: 0x040007C6 RID: 1990
	[Range(0f, 1f)]
	public float buffChance;

	// Token: 0x040007C7 RID: 1991
	public Buff buff;
}
