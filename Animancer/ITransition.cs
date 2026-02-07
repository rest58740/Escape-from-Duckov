using System;

namespace Animancer
{
	// Token: 0x02000031 RID: 49
	public interface ITransition : IHasKey, IPolymorphic
	{
		// Token: 0x06000354 RID: 852
		AnimancerState CreateState();

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000355 RID: 853
		float FadeDuration { get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000356 RID: 854
		FadeMode FadeMode { get; }

		// Token: 0x06000357 RID: 855
		void Apply(AnimancerState state);
	}
}
