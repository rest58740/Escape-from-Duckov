using System;

namespace Animancer
{
	// Token: 0x02000032 RID: 50
	public interface ITransition<TState> : ITransition, IHasKey, IPolymorphic where TState : AnimancerState
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000358 RID: 856
		TState State { get; }

		// Token: 0x06000359 RID: 857
		TState CreateState();
	}
}
