using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C2 RID: 962
	public static class ItemUIUtilities
	{
		// Token: 0x140000EE RID: 238
		// (add) Token: 0x0600227E RID: 8830 RVA: 0x00078E10 File Offset: 0x00077010
		// (remove) Token: 0x0600227F RID: 8831 RVA: 0x00078E44 File Offset: 0x00077044
		public static event Action OnSelectionChanged;

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x06002280 RID: 8832 RVA: 0x00078E78 File Offset: 0x00077078
		// (remove) Token: 0x06002281 RID: 8833 RVA: 0x00078EAC File Offset: 0x000770AC
		public static event Action<Item> OnOrphanRaised;

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x00078EDF File Offset: 0x000770DF
		public static ItemDisplay SelectedItemDisplayRaw
		{
			get
			{
				return ItemUIUtilities.selectedItemDisplay;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x00078EE6 File Offset: 0x000770E6
		// (set) Token: 0x06002284 RID: 8836 RVA: 0x00078F10 File Offset: 0x00077110
		public static ItemDisplay SelectedItemDisplay
		{
			get
			{
				if (ItemUIUtilities.selectedItemDisplay == null)
				{
					return null;
				}
				if (ItemUIUtilities.selectedItemDisplay.Target == null)
				{
					return null;
				}
				return ItemUIUtilities.selectedItemDisplay;
			}
			private set
			{
				ItemDisplay itemDisplay = ItemUIUtilities.selectedItemDisplay;
				if (itemDisplay != null)
				{
					itemDisplay.NotifyUnselected();
				}
				ItemUIUtilities.selectedItemDisplay = value;
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					ItemUIUtilities.selectedItemTypeID = -1;
				}
				else
				{
					ItemUIUtilities.selectedItemTypeID = selectedItem.TypeID;
					ItemUIUtilities.cachedSelectedItemMeta = ItemAssetsCollection.GetMetaData(ItemUIUtilities.selectedItemTypeID);
					ItemUIUtilities.cacheGunSelected = selectedItem.Tags.Contains("Gun");
				}
				ItemDisplay itemDisplay2 = ItemUIUtilities.selectedItemDisplay;
				if (itemDisplay2 != null)
				{
					itemDisplay2.NotifySelected();
				}
				Action onSelectionChanged = ItemUIUtilities.OnSelectionChanged;
				if (onSelectionChanged == null)
				{
					return;
				}
				onSelectionChanged();
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x00078F98 File Offset: 0x00077198
		public static Item SelectedItem
		{
			get
			{
				if (ItemUIUtilities.SelectedItemDisplay == null)
				{
					return null;
				}
				return ItemUIUtilities.SelectedItemDisplay.Target;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00078FB3 File Offset: 0x000771B3
		public static bool IsGunSelected
		{
			get
			{
				return !(ItemUIUtilities.SelectedItem == null) && ItemUIUtilities.cacheGunSelected;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x00078FC9 File Offset: 0x000771C9
		public static string SelectedItemCaliber
		{
			get
			{
				return ItemUIUtilities.cachedSelectedItemMeta.caliber;
			}
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x06002288 RID: 8840 RVA: 0x00078FD8 File Offset: 0x000771D8
		// (remove) Token: 0x06002289 RID: 8841 RVA: 0x0007900C File Offset: 0x0007720C
		public static event Action<Item, bool> OnPutItem;

		// Token: 0x0600228A RID: 8842 RVA: 0x0007903F File Offset: 0x0007723F
		public static void Select(ItemDisplay itemDisplay)
		{
			ItemUIUtilities.SelectedItemDisplay = itemDisplay;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00079047 File Offset: 0x00077247
		public static void RaiseOrphan(Item orphan)
		{
			if (orphan == null)
			{
				return;
			}
			Action<Item> onOrphanRaised = ItemUIUtilities.OnOrphanRaised;
			if (onOrphanRaised != null)
			{
				onOrphanRaised(orphan);
			}
			Debug.LogWarning(string.Format("游戏中出现了孤儿Item {0}。", orphan));
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00079074 File Offset: 0x00077274
		public static void NotifyPutItem(Item item, bool pickup = false)
		{
			Action<Item, bool> onPutItem = ItemUIUtilities.OnPutItem;
			if (onPutItem == null)
			{
				return;
			}
			onPutItem(item, pickup);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00079088 File Offset: 0x00077288
		public static string GetPropertiesDisplayText(this Item item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (item.Variables != null)
			{
				foreach (CustomData customData in item.Variables)
				{
					if (customData.Display)
					{
						stringBuilder.AppendLine(customData.DisplayName + "\t" + customData.GetValueDisplayString(""));
					}
				}
			}
			if (item.Constants != null)
			{
				foreach (CustomData customData2 in item.Constants)
				{
					if (customData2.Display)
					{
						stringBuilder.AppendLine(customData2.DisplayName + "\t" + customData2.GetValueDisplayString(""));
					}
				}
			}
			if (item.Stats != null)
			{
				foreach (Stat stat in item.Stats)
				{
					if (stat.Display)
					{
						stringBuilder.AppendLine(string.Format("{0}\t{1}", stat.DisplayName, stat.Value));
					}
				}
			}
			if (item.Modifiers != null)
			{
				foreach (ModifierDescription modifierDescription in item.Modifiers)
				{
					if (modifierDescription.Display)
					{
						stringBuilder.AppendLine(modifierDescription.DisplayName + "\t" + modifierDescription.GetDisplayValueString("0.##"));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00079260 File Offset: 0x00077460
		[return: TupleElementNames(new string[]
		{
			"name",
			"value",
			"polarity"
		})]
		public static List<ValueTuple<string, string, Polarity>> GetPropertyValueTextPair(this Item item)
		{
			List<ValueTuple<string, string, Polarity>> list = new List<ValueTuple<string, string, Polarity>>();
			if (item.Variables != null)
			{
				foreach (CustomData customData in item.Variables)
				{
					if (customData.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(customData.DisplayName, customData.GetValueDisplayString(""), 0));
					}
				}
			}
			if (item.Constants != null)
			{
				foreach (CustomData customData2 in item.Constants)
				{
					if (customData2.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(customData2.DisplayName, customData2.GetValueDisplayString(""), 0));
					}
				}
			}
			if (item.Stats != null)
			{
				foreach (Stat stat in item.Stats)
				{
					if (stat.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(stat.DisplayName, stat.Value.ToString(), 0));
					}
				}
			}
			if (item.Modifiers != null)
			{
				foreach (ModifierDescription modifierDescription in item.Modifiers)
				{
					if (modifierDescription.Display)
					{
						Polarity polarity = StatInfoDatabase.GetPolarity(modifierDescription.Key);
						if (modifierDescription.Value < 0f)
						{
							polarity = -polarity;
						}
						list.Add(new ValueTuple<string, string, Polarity>(modifierDescription.DisplayName, modifierDescription.GetDisplayValueString("0.##"), polarity));
					}
				}
			}
			return list;
		}

		// Token: 0x0400176D RID: 5997
		private static ItemDisplay selectedItemDisplay;

		// Token: 0x0400176E RID: 5998
		private static bool cacheGunSelected;

		// Token: 0x0400176F RID: 5999
		private static int selectedItemTypeID;

		// Token: 0x04001770 RID: 6000
		private static ItemMetaData cachedSelectedItemMeta;
	}
}
