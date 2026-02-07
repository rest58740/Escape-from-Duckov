using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000026 RID: 38
	public class UsageUtilities : ItemComponent
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000804C File Offset: 0x0000624C
		public float UseTime
		{
			get
			{
				return this.useTime;
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000204 RID: 516 RVA: 0x00008054 File Offset: 0x00006254
		// (remove) Token: 0x06000205 RID: 517 RVA: 0x00008088 File Offset: 0x00006288
		public static event Action<Item> OnItemUsedStaticEvent;

		// Token: 0x06000206 RID: 518 RVA: 0x000080BC File Offset: 0x000062BC
		public bool IsUsable(Item item, object user)
		{
			if (!item)
			{
				return false;
			}
			if (this.useDurability && item.Durability < (float)this.durabilityUsage)
			{
				return false;
			}
			foreach (UsageBehavior usageBehavior in this.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.CanBeUsed(item, user))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008148 File Offset: 0x00006348
		public void Use(Item item, object user)
		{
			foreach (UsageBehavior usageBehavior in this.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.CanBeUsed(item, user))
				{
					usageBehavior.Use(item, user);
				}
			}
			if (this.useDurability && item.Durability > 0f)
			{
				item.Durability -= (float)this.durabilityUsage;
			}
			Action<Item> onItemUsedStaticEvent = UsageUtilities.OnItemUsedStaticEvent;
			if (onItemUsedStaticEvent == null)
			{
				return;
			}
			onItemUsedStaticEvent(item);
		}

		// Token: 0x040000AE RID: 174
		[SerializeField]
		private float useTime;

		// Token: 0x040000AF RID: 175
		public List<UsageBehavior> behaviors = new List<UsageBehavior>();

		// Token: 0x040000B0 RID: 176
		public bool hasSound;

		// Token: 0x040000B1 RID: 177
		public string actionSound;

		// Token: 0x040000B2 RID: 178
		public string useSound;

		// Token: 0x040000B3 RID: 179
		public bool useDurability;

		// Token: 0x040000B4 RID: 180
		public int durabilityUsage = 1;
	}
}
