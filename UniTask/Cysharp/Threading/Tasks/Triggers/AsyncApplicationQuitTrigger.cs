using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200007F RID: 127
	[DisallowMultipleComponent]
	public sealed class AsyncApplicationQuitTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		private void OnApplicationQuit()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000BDFD File Offset: 0x00009FFD
		public IAsyncOnApplicationQuitHandler GetOnApplicationQuitAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000BE06 File Offset: 0x0000A006
		public IAsyncOnApplicationQuitHandler GetOnApplicationQuitAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000BE10 File Offset: 0x0000A010
		public UniTask OnApplicationQuitAsync()
		{
			return ((IAsyncOnApplicationQuitHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnApplicationQuitAsync();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000BE1E File Offset: 0x0000A01E
		public UniTask OnApplicationQuitAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnApplicationQuitHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnApplicationQuitAsync();
		}
	}
}
