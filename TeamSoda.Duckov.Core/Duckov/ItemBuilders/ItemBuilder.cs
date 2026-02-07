using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.ItemBuilders
{
	// Token: 0x020002F3 RID: 755
	public class ItemBuilder
	{
		// Token: 0x0600187C RID: 6268 RVA: 0x0005A329 File Offset: 0x00058529
		public static ItemBuilder New()
		{
			return new ItemBuilder();
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0005A330 File Offset: 0x00058530
		public Item Instantiate()
		{
			Item item = new GameObject(string.Format("CustomItem_{0}", this.typeID)).AddComponent<Item>();
			item.SetTypeID(this.typeID);
			item.Icon = this.icon;
			item.MaxStackCount = this.maxStackCount;
			if (this.maxStackCount > 1)
			{
				item.StackCount = this.stackCount;
			}
			item.CreateSlotsComponent();
			item.CreateStatsComponent();
			item.CreateInventoryComponent();
			item.CreateModifiersComponent();
			foreach (SlotDesc slotDesc in this.slots.Values)
			{
				Slot slot = new Slot(slotDesc.key);
				slot.requireTags.AddRange(slotDesc.requireTags);
				slot.excludeTags.AddRange(slotDesc.excludeTags);
				item.Slots.Add(slot);
			}
			foreach (StatDesc statDesc in this.stats.Values)
			{
				Stat stat = new Stat(statDesc.key, statDesc.value, statDesc.display);
				item.Stats.Add(stat);
			}
			foreach (CustomData copyFrom in this.constants)
			{
				item.Constants.Add(new CustomData(copyFrom));
			}
			foreach (CustomData copyFrom2 in this.variables)
			{
				item.Variables.Add(new CustomData(copyFrom2));
			}
			foreach (ModifierDescription modifierDescription in this.modifiers)
			{
				item.Modifiers.Add(modifierDescription);
			}
			item.Modifiers.ReapplyModifiers();
			return item;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0005A588 File Offset: 0x00058788
		public ItemBuilder Icon(Sprite sprite)
		{
			this.icon = sprite;
			return this;
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0005A592 File Offset: 0x00058792
		public ItemBuilder TypeID(int id)
		{
			this.typeID = id;
			return this;
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0005A59C File Offset: 0x0005879C
		public ItemBuilder EnableStacking(int maxStackCount, int stackCount)
		{
			this.maxStackCount = maxStackCount;
			this.stackCount = stackCount;
			return this;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0005A5AD File Offset: 0x000587AD
		public ItemBuilder DisableStacking()
		{
			this.maxStackCount = 1;
			this.stackCount = 1;
			return this;
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0005A5C0 File Offset: 0x000587C0
		public ItemBuilder Slot(string key, List<Tag> requireTags, List<Tag> excludeTags)
		{
			List<Tag> list = new List<Tag>();
			List<Tag> list2 = new List<Tag>();
			if (requireTags != null)
			{
				list.AddRange(requireTags);
			}
			if (excludeTags != null)
			{
				list2.AddRange(excludeTags);
			}
			this.slots[key] = new SlotDesc
			{
				key = key,
				requireTags = list,
				excludeTags = list2
			};
			return this;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0005A61C File Offset: 0x0005881C
		public ItemBuilder Slot(string key, Tag requireTag, Tag excludeTag = null)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				Debug.LogError("[Item Builder] Key of slot cannot be empty");
				return this;
			}
			List<Tag> list = new List<Tag>();
			List<Tag> list2 = new List<Tag>();
			if (requireTag != null)
			{
				list.Add(requireTag);
			}
			if (excludeTag != null)
			{
				list2.Add(excludeTag);
			}
			this.slots[key] = new SlotDesc
			{
				key = key,
				requireTags = list,
				excludeTags = list2
			};
			return this;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0005A698 File Offset: 0x00058898
		public ItemBuilder Stat(string key, float value, bool display = false)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				Debug.LogError("[Item Builder] Key of stat cannot be empty");
				return this;
			}
			this.stats[key] = new StatDesc
			{
				key = key,
				value = value,
				display = display
			};
			return this;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0005A6E7 File Offset: 0x000588E7
		public ItemBuilder SetVariable(string key, float value, bool display = false)
		{
			this.variables.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0005A705 File Offset: 0x00058905
		public ItemBuilder SetVariable(string key, int value, bool display = false)
		{
			this.variables.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0005A723 File Offset: 0x00058923
		public ItemBuilder SetVariable(string key, bool value, bool display = false)
		{
			this.variables.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0005A741 File Offset: 0x00058941
		public ItemBuilder SetVariable(string key, string value, bool display = false)
		{
			this.variables.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0005A75F File Offset: 0x0005895F
		public ItemBuilder SetConstant(string key, float value, bool display = false)
		{
			this.constants.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0005A77D File Offset: 0x0005897D
		public ItemBuilder SetConstant(string key, int value, bool display = false)
		{
			this.constants.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0005A79B File Offset: 0x0005899B
		public ItemBuilder SetConstant(string key, bool value, bool display = false)
		{
			this.constants.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0005A7B9 File Offset: 0x000589B9
		public ItemBuilder SetConstant(string key, string value, bool display = false)
		{
			this.constants.Set(key, value, true);
			this.variables.SetDisplay(key, display);
			return this;
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0005A7D7 File Offset: 0x000589D7
		public ItemBuilder Modifier(ModifierDescription description)
		{
			this.modifiers.Add(description);
			return this;
		}

		// Token: 0x040011DD RID: 4573
		public int typeID;

		// Token: 0x040011DE RID: 4574
		public Sprite icon;

		// Token: 0x040011DF RID: 4575
		public int maxStackCount = 1;

		// Token: 0x040011E0 RID: 4576
		public int stackCount = 1;

		// Token: 0x040011E1 RID: 4577
		public Dictionary<string, SlotDesc> slots = new Dictionary<string, SlotDesc>();

		// Token: 0x040011E2 RID: 4578
		public Dictionary<string, StatDesc> stats = new Dictionary<string, StatDesc>();

		// Token: 0x040011E3 RID: 4579
		public CustomDataCollection constants = new CustomDataCollection();

		// Token: 0x040011E4 RID: 4580
		public CustomDataCollection variables = new CustomDataCollection();

		// Token: 0x040011E5 RID: 4581
		public List<ModifierDescription> modifiers = new List<ModifierDescription>();
	}
}
