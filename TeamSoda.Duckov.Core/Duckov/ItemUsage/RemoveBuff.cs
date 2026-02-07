using System;
using ItemStatsSystem;
using UnityEngine.Serialization;

namespace Duckov.ItemUsage
{
	// Token: 0x02000385 RID: 901
	public class RemoveBuff : UsageBehavior
	{
		// Token: 0x06001F68 RID: 8040 RVA: 0x0006F300 File Offset: 0x0006D500
		public override bool CanBeUsed(Item item, object user)
		{
			if (!item)
			{
				return false;
			}
			if (this.useDurability && item.Durability < (float)this.durabilityUsage)
			{
				return false;
			}
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			return !(characterMainControl == null) && characterMainControl.HasBuff(this.buffID);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0006F350 File Offset: 0x0006D550
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			if (!this.litmitRemoveLayerCount)
			{
				characterMainControl.RemoveBuff(this.buffID, false);
			}
			for (int i = 0; i < this.removeLayerCount; i++)
			{
				characterMainControl.RemoveBuff(this.buffID, this.litmitRemoveLayerCount);
			}
			if (this.useDurability && item.Durability > 0f)
			{
				item.Durability -= (float)this.durabilityUsage;
			}
		}

		// Token: 0x04001571 RID: 5489
		public int buffID;

		// Token: 0x04001572 RID: 5490
		[FormerlySerializedAs("removeOneLayer")]
		public bool litmitRemoveLayerCount;

		// Token: 0x04001573 RID: 5491
		public int removeLayerCount = 2;

		// Token: 0x04001574 RID: 5492
		public bool useDurability;

		// Token: 0x04001575 RID: 5493
		public int durabilityUsage = 1;
	}
}
