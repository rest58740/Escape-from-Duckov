using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000EF RID: 239
	[DisallowMultipleComponent]
	public sealed class AsyncEndDragTrigger : AsyncTriggerBase<PointerEventData>, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x060005A1 RID: 1441 RVA: 0x0000CCB4 File Offset: 0x0000AEB4
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0000CCBD File Offset: 0x0000AEBD
		public IAsyncOnEndDragHandler GetOnEndDragAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0000CCC6 File Offset: 0x0000AEC6
		public IAsyncOnEndDragHandler GetOnEndDragAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		public UniTask<PointerEventData> OnEndDragAsync()
		{
			return ((IAsyncOnEndDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnEndDragAsync();
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0000CCDE File Offset: 0x0000AEDE
		public UniTask<PointerEventData> OnEndDragAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnEndDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnEndDragAsync();
		}
	}
}
