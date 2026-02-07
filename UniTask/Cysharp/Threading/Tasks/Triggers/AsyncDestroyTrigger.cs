using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200006D RID: 109
	[DisallowMultipleComponent]
	public sealed class AsyncDestroyTrigger : MonoBehaviour
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000AEA7 File Offset: 0x000090A7
		public CancellationToken CancellationToken
		{
			get
			{
				if (this.cancellationTokenSource == null)
				{
					this.cancellationTokenSource = new CancellationTokenSource();
					if (!this.awakeCalled)
					{
						PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, new AsyncDestroyTrigger.AwakeMonitor(this));
					}
				}
				return this.cancellationTokenSource.Token;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000AEDB File Offset: 0x000090DB
		private void Awake()
		{
			this.awakeCalled = true;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000AEE4 File Offset: 0x000090E4
		private void OnDestroy()
		{
			this.called = true;
			CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			CancellationTokenSource cancellationTokenSource2 = this.cancellationTokenSource;
			if (cancellationTokenSource2 == null)
			{
				return;
			}
			cancellationTokenSource2.Dispose();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000AF10 File Offset: 0x00009110
		public UniTask OnDestroyAsync()
		{
			if (this.called)
			{
				return UniTask.CompletedTask;
			}
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			this.CancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
			{
				((UniTaskCompletionSource)state).TrySetResult();
			}, uniTaskCompletionSource);
			return uniTaskCompletionSource.Task;
		}

		// Token: 0x040000EC RID: 236
		private bool awakeCalled;

		// Token: 0x040000ED RID: 237
		private bool called;

		// Token: 0x040000EE RID: 238
		private CancellationTokenSource cancellationTokenSource;

		// Token: 0x02000205 RID: 517
		private class AwakeMonitor : IPlayerLoopItem
		{
			// Token: 0x06000BAF RID: 2991 RVA: 0x0002A766 File Offset: 0x00028966
			public AwakeMonitor(AsyncDestroyTrigger trigger)
			{
				this.trigger = trigger;
			}

			// Token: 0x06000BB0 RID: 2992 RVA: 0x0002A775 File Offset: 0x00028975
			public bool MoveNext()
			{
				if (this.trigger.called || this.trigger.awakeCalled)
				{
					return false;
				}
				if (this.trigger == null)
				{
					this.trigger.OnDestroy();
					return false;
				}
				return true;
			}

			// Token: 0x04000536 RID: 1334
			private readonly AsyncDestroyTrigger trigger;
		}
	}
}
