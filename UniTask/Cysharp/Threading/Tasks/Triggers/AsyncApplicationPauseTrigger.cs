using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200007D RID: 125
	[DisallowMultipleComponent]
	public sealed class AsyncApplicationPauseTrigger : AsyncTriggerBase<bool>
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x0000BDAF File Offset: 0x00009FAF
		private void OnApplicationPause(bool pauseStatus)
		{
			base.RaiseEvent(pauseStatus);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public IAsyncOnApplicationPauseHandler GetOnApplicationPauseAsyncHandler()
		{
			return new AsyncTriggerHandler<bool>(this, false);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000BDC1 File Offset: 0x00009FC1
		public IAsyncOnApplicationPauseHandler GetOnApplicationPauseAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<bool>(this, cancellationToken, false);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000BDCB File Offset: 0x00009FCB
		public UniTask<bool> OnApplicationPauseAsync()
		{
			return ((IAsyncOnApplicationPauseHandler)new AsyncTriggerHandler<bool>(this, true)).OnApplicationPauseAsync();
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000BDD9 File Offset: 0x00009FD9
		public UniTask<bool> OnApplicationPauseAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnApplicationPauseHandler)new AsyncTriggerHandler<bool>(this, cancellationToken, true)).OnApplicationPauseAsync();
		}
	}
}
