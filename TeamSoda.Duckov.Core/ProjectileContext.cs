using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000072 RID: 114
public struct ProjectileContext
{
	// Token: 0x04000387 RID: 903
	public Vector3 direction;

	// Token: 0x04000388 RID: 904
	public CharacterMainControl traceTarget;

	// Token: 0x04000389 RID: 905
	public float traceAbility;

	// Token: 0x0400038A RID: 906
	public bool firstFrameCheck;

	// Token: 0x0400038B RID: 907
	public Vector3 firstFrameCheckStartPoint;

	// Token: 0x0400038C RID: 908
	public float halfDamageDistance;

	// Token: 0x0400038D RID: 909
	public float distance;

	// Token: 0x0400038E RID: 910
	public float speed;

	// Token: 0x0400038F RID: 911
	public Teams team;

	// Token: 0x04000390 RID: 912
	public int penetrate;

	// Token: 0x04000391 RID: 913
	public float damage;

	// Token: 0x04000392 RID: 914
	public float critDamageFactor;

	// Token: 0x04000393 RID: 915
	public float critRate;

	// Token: 0x04000394 RID: 916
	public float armorPiercing;

	// Token: 0x04000395 RID: 917
	public float armorBreak;

	// Token: 0x04000396 RID: 918
	public float element_Physics;

	// Token: 0x04000397 RID: 919
	public float element_Fire;

	// Token: 0x04000398 RID: 920
	public float element_Poison;

	// Token: 0x04000399 RID: 921
	public float element_Electricity;

	// Token: 0x0400039A RID: 922
	public float element_Space;

	// Token: 0x0400039B RID: 923
	public float element_Ghost;

	// Token: 0x0400039C RID: 924
	public float element_Ice;

	// Token: 0x0400039D RID: 925
	public CharacterMainControl fromCharacter;

	// Token: 0x0400039E RID: 926
	public float gravity;

	// Token: 0x0400039F RID: 927
	public float explosionRange;

	// Token: 0x040003A0 RID: 928
	public float explosionDamage;

	// Token: 0x040003A1 RID: 929
	public float buffChance;

	// Token: 0x040003A2 RID: 930
	public Buff buff;

	// Token: 0x040003A3 RID: 931
	public float bleedChance;

	// Token: 0x040003A4 RID: 932
	public bool ignoreHalfObsticle;

	// Token: 0x040003A5 RID: 933
	[ItemTypeID]
	public int fromWeaponItemID;
}
