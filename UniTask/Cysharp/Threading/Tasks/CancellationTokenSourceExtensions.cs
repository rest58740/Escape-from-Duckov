using System;
using System.Threading;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200000F RID: 15
	public static class CancellationTokenSourceExtensions
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002A21 File Offset: 0x00000C21
		private static void CancelCancellationTokenSourceState(object state)
		{
			((CancellationTokenSource)state).Cancel();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A2E File Offset: 0x00000C2E
		public static IDisposable CancelAfterSlim(this CancellationTokenSource cts, int millisecondsDelay, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update)
		{
			return cts.CancelAfterSlim(TimeSpan.FromMilliseconds((double)millisecondsDelay), delayType, delayTiming);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002A3F File Offset: 0x00000C3F
		public static IDisposable CancelAfterSlim(this CancellationTokenSource cts, TimeSpan delayTimeSpan, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update)
		{
			return PlayerLoopTimer.StartNew(delayTimeSpan, false, delayType, delayTiming, cts.Token, CancellationTokenSourceExtensions.CancelCancellationTokenSourceStateDelegate, cts);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002A56 File Offset: 0x00000C56
		public static void RegisterRaiseCancelOnDestroy(this CancellationTokenSource cts, Component component)
		{
			cts.RegisterRaiseCancelOnDestroy(component.gameObject);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002A64 File Offset: 0x00000C64
		public static void RegisterRaiseCancelOnDestroy(this CancellationTokenSource cts, GameObject gameObject)
		{
			gameObject.GetAsyncDestroyTrigger().CancellationToken.RegisterWithoutCaptureExecutionContext(CancellationTokenSourceExtensions.CancelCancellationTokenSourceStateDelegate, cts);
		}

		// Token: 0x0400001A RID: 26
		private static readonly Action<object> CancelCancellationTokenSourceStateDelegate = new Action<object>(CancellationTokenSourceExtensions.CancelCancellationTokenSourceState);
	}
}
