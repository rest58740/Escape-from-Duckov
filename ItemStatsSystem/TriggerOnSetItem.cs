using System;

namespace ItemStatsSystem
{
	// Token: 0x02000018 RID: 24
	public class TriggerOnSetItem : EffectTrigger
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x000040A3 File Offset: 0x000022A3
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			base.Trigger(true);
		}
	}
}
