using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000BD RID: 189
	[DisallowMultipleComponent]
	public sealed class AsyncPostRenderTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x0000C621 File Offset: 0x0000A821
		private void OnPostRender()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000C62E File Offset: 0x0000A82E
		public IAsyncOnPostRenderHandler GetOnPostRenderAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000C637 File Offset: 0x0000A837
		public IAsyncOnPostRenderHandler GetOnPostRenderAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000C641 File Offset: 0x0000A841
		public UniTask OnPostRenderAsync()
		{
			return ((IAsyncOnPostRenderHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnPostRenderAsync();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000C64F File Offset: 0x0000A84F
		public UniTask OnPostRenderAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPostRenderHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnPostRenderAsync();
		}
	}
}
