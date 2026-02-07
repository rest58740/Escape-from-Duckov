using System;

namespace Animancer.FSM
{
	// Token: 0x0200000F RID: 15
	public interface IKeyedStateMachine<TKey>
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000056 RID: 86
		TKey CurrentKey { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000057 RID: 87
		TKey PreviousKey { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000058 RID: 88
		TKey NextKey { get; }

		// Token: 0x06000059 RID: 89
		object TrySetState(TKey key);

		// Token: 0x0600005A RID: 90
		object TryResetState(TKey key);

		// Token: 0x0600005B RID: 91
		object ForceSetState(TKey key);
	}
}
