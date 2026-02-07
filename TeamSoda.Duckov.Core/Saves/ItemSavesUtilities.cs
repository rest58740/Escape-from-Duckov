using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Data;

namespace Saves
{
	// Token: 0x0200022E RID: 558
	public static class ItemSavesUtilities
	{
		// Token: 0x060010F4 RID: 4340 RVA: 0x0004255C File Offset: 0x0004075C
		public static void SaveAsLastDeadCharacter(Item item)
		{
			uint num = SavesSystem.Load<uint>("DeadCharacterToken");
			uint num2 = num;
			do
			{
				num2 += 1U;
			}
			while (num2 == num);
			SavesSystem.Save<uint>("DeadCharacterToken", num2);
			item.Save("LastDeadCharacter");
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00042594 File Offset: 0x00040794
		public static UniTask<Item> LoadLastDeadCharacterItem()
		{
			ItemSavesUtilities.<LoadLastDeadCharacterItem>d__3 <LoadLastDeadCharacterItem>d__;
			<LoadLastDeadCharacterItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<LoadLastDeadCharacterItem>d__.<>1__state = -1;
			<LoadLastDeadCharacterItem>d__.<>t__builder.Start<ItemSavesUtilities.<LoadLastDeadCharacterItem>d__3>(ref <LoadLastDeadCharacterItem>d__);
			return <LoadLastDeadCharacterItem>d__.<>t__builder.Task;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x000425D0 File Offset: 0x000407D0
		public static void Save(this Item item, string key)
		{
			ItemTreeData value = ItemTreeData.FromItem(item);
			SavesSystem.Save<ItemTreeData>("Item/", key, value);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x000425F0 File Offset: 0x000407F0
		public static void Save(this Inventory inventory, string key)
		{
			InventoryData value = InventoryData.FromInventory(inventory);
			SavesSystem.Save<InventoryData>("Inventory/", key, value);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00042610 File Offset: 0x00040810
		public static UniTask<Item> LoadItem(string key)
		{
			ItemSavesUtilities.<LoadItem>d__6 <LoadItem>d__;
			<LoadItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<LoadItem>d__.key = key;
			<LoadItem>d__.<>1__state = -1;
			<LoadItem>d__.<>t__builder.Start<ItemSavesUtilities.<LoadItem>d__6>(ref <LoadItem>d__);
			return <LoadItem>d__.<>t__builder.Task;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00042654 File Offset: 0x00040854
		public static UniTask LoadInventory(string key, Inventory inventoryInstance)
		{
			ItemSavesUtilities.<LoadInventory>d__7 <LoadInventory>d__;
			<LoadInventory>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadInventory>d__.key = key;
			<LoadInventory>d__.inventoryInstance = inventoryInstance;
			<LoadInventory>d__.<>1__state = -1;
			<LoadInventory>d__.<>t__builder.Start<ItemSavesUtilities.<LoadInventory>d__7>(ref <LoadInventory>d__);
			return <LoadInventory>d__.<>t__builder.Task;
		}

		// Token: 0x04000D91 RID: 3473
		private const string InventoryPrefix = "Inventory/";

		// Token: 0x04000D92 RID: 3474
		private const string ItemPrefix = "Item/";
	}
}
