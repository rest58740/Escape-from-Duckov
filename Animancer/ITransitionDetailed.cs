using System;

namespace Animancer
{
	// Token: 0x02000033 RID: 51
	public interface ITransitionDetailed : ITransition, IHasKey, IPolymorphic
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600035A RID: 858
		bool IsValid { get; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600035B RID: 859
		bool IsLooping { get; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600035C RID: 860
		// (set) Token: 0x0600035D RID: 861
		float NormalizedStartTime { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600035E RID: 862
		float MaximumDuration { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600035F RID: 863
		// (set) Token: 0x06000360 RID: 864
		float Speed { get; set; }
	}
}
