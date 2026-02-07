using System;

namespace ItemStatsSystem
{
	// Token: 0x02000015 RID: 21
	public class ItemInCharacterSlotFilter : EffectFilter
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00003EAA File Offset: 0x000020AA
		protected override bool OnEvaluate(EffectTriggerEventContext context)
		{
			return !(base.Master == null) && !(base.Master.Item == null) && base.Master.Item.IsInCharacterSlot();
		}
	}
}
