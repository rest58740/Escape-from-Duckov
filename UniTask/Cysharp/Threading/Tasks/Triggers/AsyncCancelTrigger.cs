using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E7 RID: 231
	[DisallowMultipleComponent]
	public sealed class AsyncCancelTrigger : AsyncTriggerBase<BaseEventData>, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000CBB9 File Offset: 0x0000ADB9
		public IAsyncOnCancelHandler GetOnCancelAsyncHandler()
		{
			return new AsyncTriggerHandler<BaseEventData>(this, false);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000CBC2 File Offset: 0x0000ADC2
		public IAsyncOnCancelHandler GetOnCancelAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, false);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000CBCC File Offset: 0x0000ADCC
		public UniTask<BaseEventData> OnCancelAsync()
		{
			return ((IAsyncOnCancelHandler)new AsyncTriggerHandler<BaseEventData>(this, true)).OnCancelAsync();
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000CBDA File Offset: 0x0000ADDA
		public UniTask<BaseEventData> OnCancelAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCancelHandler)new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, true)).OnCancelAsync();
		}
	}
}
