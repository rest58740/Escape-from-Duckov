using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000003 RID: 3
	public enum TweenCancelBehaviour
	{
		// Token: 0x04000002 RID: 2
		Kill,
		// Token: 0x04000003 RID: 3
		KillWithCompleteCallback,
		// Token: 0x04000004 RID: 4
		Complete,
		// Token: 0x04000005 RID: 5
		CompleteWithSequenceCallback,
		// Token: 0x04000006 RID: 6
		CancelAwait,
		// Token: 0x04000007 RID: 7
		KillAndCancelAwait,
		// Token: 0x04000008 RID: 8
		KillWithCompleteCallbackAndCancelAwait,
		// Token: 0x04000009 RID: 9
		CompleteAndCancelAwait,
		// Token: 0x0400000A RID: 10
		CompleteWithSequenceCallbackAndCancelAwait
	}
}
