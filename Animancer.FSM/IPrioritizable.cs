using System;

namespace Animancer.FSM
{
	// Token: 0x0200000D RID: 13
	public interface IPrioritizable : IState
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000052 RID: 82
		float Priority { get; }
	}
}
