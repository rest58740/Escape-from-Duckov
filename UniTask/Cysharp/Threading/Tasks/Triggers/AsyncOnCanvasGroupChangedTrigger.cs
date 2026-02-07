using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000089 RID: 137
	[DisallowMultipleComponent]
	public sealed class AsyncOnCanvasGroupChangedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x0000BF4B File Offset: 0x0000A14B
		private void OnCanvasGroupChanged()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000BF58 File Offset: 0x0000A158
		public IAsyncOnCanvasGroupChangedHandler GetOnCanvasGroupChangedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000BF61 File Offset: 0x0000A161
		public IAsyncOnCanvasGroupChangedHandler GetOnCanvasGroupChangedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000BF6B File Offset: 0x0000A16B
		public UniTask OnCanvasGroupChangedAsync()
		{
			return ((IAsyncOnCanvasGroupChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnCanvasGroupChangedAsync();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000BF79 File Offset: 0x0000A179
		public UniTask OnCanvasGroupChangedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCanvasGroupChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnCanvasGroupChangedAsync();
		}
	}
}
