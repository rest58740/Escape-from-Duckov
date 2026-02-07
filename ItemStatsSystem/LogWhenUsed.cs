using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000024 RID: 36
	public class LogWhenUsed : UsageBehavior
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00008009 File Offset: 0x00006209
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000800C File Offset: 0x0000620C
		protected override void OnUse(Item item, object user)
		{
			Debug.Log(item.name);
		}
	}
}
