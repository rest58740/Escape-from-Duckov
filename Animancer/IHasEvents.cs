using System;

namespace Animancer
{
	// Token: 0x0200002C RID: 44
	public interface IHasEvents
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000343 RID: 835
		AnimancerEvent.Sequence Events { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000344 RID: 836
		ref AnimancerEvent.Sequence.Serializable SerializedEvents { get; }
	}
}
