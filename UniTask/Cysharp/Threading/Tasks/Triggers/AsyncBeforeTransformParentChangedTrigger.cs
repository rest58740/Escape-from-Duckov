using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000087 RID: 135
	[DisallowMultipleComponent]
	public sealed class AsyncBeforeTransformParentChangedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x0000BF06 File Offset: 0x0000A106
		private void OnBeforeTransformParentChanged()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000BF13 File Offset: 0x0000A113
		public IAsyncOnBeforeTransformParentChangedHandler GetOnBeforeTransformParentChangedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public IAsyncOnBeforeTransformParentChangedHandler GetOnBeforeTransformParentChangedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000BF26 File Offset: 0x0000A126
		public UniTask OnBeforeTransformParentChangedAsync()
		{
			return ((IAsyncOnBeforeTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnBeforeTransformParentChangedAsync();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000BF34 File Offset: 0x0000A134
		public UniTask OnBeforeTransformParentChangedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnBeforeTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnBeforeTransformParentChangedAsync();
		}
	}
}
