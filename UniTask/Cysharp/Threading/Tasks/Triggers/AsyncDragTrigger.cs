using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000EB RID: 235
	[DisallowMultipleComponent]
	public sealed class AsyncDragTrigger : AsyncTriggerBase<PointerEventData>, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x0000CC32 File Offset: 0x0000AE32
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0000CC3B File Offset: 0x0000AE3B
		public IAsyncOnDragHandler GetOnDragAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0000CC44 File Offset: 0x0000AE44
		public IAsyncOnDragHandler GetOnDragAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0000CC4E File Offset: 0x0000AE4E
		public UniTask<PointerEventData> OnDragAsync()
		{
			return ((IAsyncOnDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnDragAsync();
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0000CC5C File Offset: 0x0000AE5C
		public UniTask<PointerEventData> OnDragAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnDragAsync();
		}
	}
}
