using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000083 RID: 131
	[DisallowMultipleComponent]
	public sealed class AsyncBecameInvisibleTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000427 RID: 1063 RVA: 0x0000BE7C File Offset: 0x0000A07C
		private void OnBecameInvisible()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000BE89 File Offset: 0x0000A089
		public IAsyncOnBecameInvisibleHandler GetOnBecameInvisibleAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000BE92 File Offset: 0x0000A092
		public IAsyncOnBecameInvisibleHandler GetOnBecameInvisibleAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000BE9C File Offset: 0x0000A09C
		public UniTask OnBecameInvisibleAsync()
		{
			return ((IAsyncOnBecameInvisibleHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnBecameInvisibleAsync();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000BEAA File Offset: 0x0000A0AA
		public UniTask OnBecameInvisibleAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnBecameInvisibleHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnBecameInvisibleAsync();
		}
	}
}
