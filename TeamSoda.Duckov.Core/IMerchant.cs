using System;
using ItemStatsSystem;

// Token: 0x02000209 RID: 521
public interface IMerchant
{
	// Token: 0x06000F7D RID: 3965
	int ConvertPrice(Item item, bool selling = false);
}
