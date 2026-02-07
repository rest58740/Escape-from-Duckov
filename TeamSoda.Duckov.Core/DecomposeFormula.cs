using System;
using Duckov.Economy;
using ItemStatsSystem;

// Token: 0x020001B1 RID: 433
[Serializable]
public struct DecomposeFormula
{
	// Token: 0x04000B52 RID: 2898
	[ItemTypeID]
	public int item;

	// Token: 0x04000B53 RID: 2899
	public bool valid;

	// Token: 0x04000B54 RID: 2900
	public Cost result;
}
