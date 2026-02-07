using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E5 RID: 229
	[DisallowMultipleComponent]
	public sealed class AsyncBeginDragTrigger : AsyncTriggerBase<PointerEventData>, IBeginDragHandler, IEventSystemHandler
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x0000CB6F File Offset: 0x0000AD6F
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000CB78 File Offset: 0x0000AD78
		public IAsyncOnBeginDragHandler GetOnBeginDragAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000CB81 File Offset: 0x0000AD81
		public IAsyncOnBeginDragHandler GetOnBeginDragAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000CB8B File Offset: 0x0000AD8B
		public UniTask<PointerEventData> OnBeginDragAsync()
		{
			return ((IAsyncOnBeginDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnBeginDragAsync();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000CB99 File Offset: 0x0000AD99
		public UniTask<PointerEventData> OnBeginDragAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnBeginDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnBeginDragAsync();
		}
	}
}
