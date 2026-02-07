using System;
using Duckov.PerkTrees;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200037E RID: 894
	public class RequirePerkUnlocked : Condition
	{
		// Token: 0x06001F4B RID: 8011 RVA: 0x0006EBCE File Offset: 0x0006CDCE
		public override bool Evaluate()
		{
			return this.GetUnlocked();
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0006EBD8 File Offset: 0x0006CDD8
		private bool GetUnlocked()
		{
			if (this.perk)
			{
				return this.perk.Unlocked;
			}
			PerkTree perkTree = PerkTreeManager.GetPerkTree(this.perkTreeID);
			if (perkTree)
			{
				foreach (Perk perk in perkTree.perks)
				{
					if (perk.gameObject.name == this.perkObjectName)
					{
						this.perk = perk;
						return this.perk.Unlocked;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0400155A RID: 5466
		[SerializeField]
		private string perkTreeID;

		// Token: 0x0400155B RID: 5467
		[SerializeField]
		private string perkObjectName;

		// Token: 0x0400155C RID: 5468
		private Perk perk;
	}
}
