using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E9 RID: 233
	[DisallowMultipleComponent]
	public sealed class AsyncDeselectTrigger : AsyncTriggerBase<BaseEventData>, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x0000CBF1 File Offset: 0x0000ADF1
		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000CBFA File Offset: 0x0000ADFA
		public IAsyncOnDeselectHandler GetOnDeselectAsyncHandler()
		{
			return new AsyncTriggerHandler<BaseEventData>(this, false);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000CC03 File Offset: 0x0000AE03
		public IAsyncOnDeselectHandler GetOnDeselectAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, false);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000CC0D File Offset: 0x0000AE0D
		public UniTask<BaseEventData> OnDeselectAsync()
		{
			return ((IAsyncOnDeselectHandler)new AsyncTriggerHandler<BaseEventData>(this, true)).OnDeselectAsync();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0000CC1B File Offset: 0x0000AE1B
		public UniTask<BaseEventData> OnDeselectAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnDeselectHandler)new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, true)).OnDeselectAsync();
		}
	}
}
