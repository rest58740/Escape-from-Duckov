using System;

namespace ItemStatsSystem
{
	// Token: 0x02000019 RID: 25
	[MenuPath("General/Update")]
	public class UpdateTrigger : EffectTrigger
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000040B4 File Offset: 0x000022B4
		public override string DisplayName
		{
			get
			{
				return "Update";
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000040BB File Offset: 0x000022BB
		private void Update()
		{
			base.Trigger(true);
		}
	}
}
