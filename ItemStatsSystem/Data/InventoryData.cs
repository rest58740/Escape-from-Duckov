using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

namespace ItemStatsSystem.Data
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class InventoryData
	{
		// Token: 0x06000243 RID: 579 RVA: 0x00008E28 File Offset: 0x00007028
		public static InventoryData FromInventory(Inventory inventory)
		{
			InventoryData inventoryData = new InventoryData();
			inventoryData.capacity = inventory.Capacity;
			int lastItemPosition = inventory.GetLastItemPosition();
			for (int i = 0; i <= lastItemPosition; i++)
			{
				Item itemAt = inventory.GetItemAt(i);
				if (!(itemAt == null))
				{
					InventoryData.Entry entry = new InventoryData.Entry();
					entry.inventoryPosition = i;
					entry.itemTreeData = ItemTreeData.FromItem(itemAt);
					inventoryData.entries.Add(entry);
				}
			}
			inventoryData.lockedIndexes = new List<int>(inventory.lockedIndexes);
			return inventoryData;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008EA8 File Offset: 0x000070A8
		public static UniTask LoadIntoInventory(InventoryData data, Inventory inventoryInstance)
		{
			InventoryData.<LoadIntoInventory>d__5 <LoadIntoInventory>d__;
			<LoadIntoInventory>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadIntoInventory>d__.data = data;
			<LoadIntoInventory>d__.inventoryInstance = inventoryInstance;
			<LoadIntoInventory>d__.<>1__state = -1;
			<LoadIntoInventory>d__.<>t__builder.Start<InventoryData.<LoadIntoInventory>d__5>(ref <LoadIntoInventory>d__);
			return <LoadIntoInventory>d__.<>t__builder.Task;
		}

		// Token: 0x040000CF RID: 207
		public int capacity = 16;

		// Token: 0x040000D0 RID: 208
		public List<InventoryData.Entry> entries = new List<InventoryData.Entry>();

		// Token: 0x040000D1 RID: 209
		public List<int> lockedIndexes = new List<int>();

		// Token: 0x02000053 RID: 83
		[Serializable]
		public class Entry
		{
			// Token: 0x04000144 RID: 324
			public int inventoryPosition;

			// Token: 0x04000145 RID: 325
			public ItemTreeData itemTreeData;
		}
	}
}
