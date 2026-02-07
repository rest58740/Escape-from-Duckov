using System;

namespace Animancer
{
	// Token: 0x0200001B RID: 27
	public class DefaultFadeValueAttribute : DefaultValueAttribute
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00008EB8 File Offset: 0x000070B8
		public override object Primary
		{
			get
			{
				return AnimancerPlayable.DefaultFadeDuration;
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008EC4 File Offset: 0x000070C4
		public DefaultFadeValueAttribute()
		{
			this.Secondary = 0f;
		}
	}
}
