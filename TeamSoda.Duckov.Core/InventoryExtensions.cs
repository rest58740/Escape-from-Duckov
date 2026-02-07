using System;
using ItemStatsSystem;

// Token: 0x020000EE RID: 238
public static class InventoryExtensions
{
	// Token: 0x0600080C RID: 2060 RVA: 0x0002477C File Offset: 0x0002297C
	private static void Sort(this Inventory inventory, Comparison<Item> comparison)
	{
		inventory.Content.Sort(comparison);
	}
}
