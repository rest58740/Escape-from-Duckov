using System;

namespace ItemStatsSystem
{
	// Token: 0x02000014 RID: 20
	[MenuPath("Debug/Bool")]
	public class BoolFilter : EffectFilter
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003E93 File Offset: 0x00002093
		public override string DisplayName
		{
			get
			{
				return "根据 Bool 值";
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003E9A File Offset: 0x0000209A
		protected override bool OnEvaluate(EffectTriggerEventContext context)
		{
			return this.value;
		}

		// Token: 0x0400003A RID: 58
		public bool value;
	}
}
