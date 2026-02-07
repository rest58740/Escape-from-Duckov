using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000081 RID: 129
	[TupleElementNames(new string[]
	{
		"data",
		"channels"
	})]
	[DisallowMultipleComponent]
	public sealed class AsyncAudioFilterReadTrigger : AsyncTriggerBase<ValueTuple<float[], int>>
	{
		// Token: 0x06000420 RID: 1056 RVA: 0x0000BE35 File Offset: 0x0000A035
		private void OnAudioFilterRead(float[] data, int channels)
		{
			base.RaiseEvent(new ValueTuple<float[], int>(data, channels));
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000BE44 File Offset: 0x0000A044
		public IAsyncOnAudioFilterReadHandler GetOnAudioFilterReadAsyncHandler()
		{
			return new AsyncTriggerHandler<ValueTuple<float[], int>>(this, false);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000BE4D File Offset: 0x0000A04D
		public IAsyncOnAudioFilterReadHandler GetOnAudioFilterReadAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<ValueTuple<float[], int>>(this, cancellationToken, false);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000BE57 File Offset: 0x0000A057
		[return: TupleElementNames(new string[]
		{
			"data",
			"channels"
		})]
		public UniTask<ValueTuple<float[], int>> OnAudioFilterReadAsync()
		{
			return ((IAsyncOnAudioFilterReadHandler)new AsyncTriggerHandler<ValueTuple<float[], int>>(this, true)).OnAudioFilterReadAsync();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000BE65 File Offset: 0x0000A065
		[return: TupleElementNames(new string[]
		{
			"data",
			"channels"
		})]
		public UniTask<ValueTuple<float[], int>> OnAudioFilterReadAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnAudioFilterReadHandler)new AsyncTriggerHandler<ValueTuple<float[], int>>(this, cancellationToken, true)).OnAudioFilterReadAsync();
		}
	}
}
