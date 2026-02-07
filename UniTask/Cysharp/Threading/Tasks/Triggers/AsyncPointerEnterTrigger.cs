using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F9 RID: 249
	[DisallowMultipleComponent]
	public sealed class AsyncPointerEnterTrigger : AsyncTriggerBase<PointerEventData>, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x0000CDF9 File Offset: 0x0000AFF9
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000CE02 File Offset: 0x0000B002
		public IAsyncOnPointerEnterHandler GetOnPointerEnterAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000CE0B File Offset: 0x0000B00B
		public IAsyncOnPointerEnterHandler GetOnPointerEnterAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000CE15 File Offset: 0x0000B015
		public UniTask<PointerEventData> OnPointerEnterAsync()
		{
			return ((IAsyncOnPointerEnterHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerEnterAsync();
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0000CE23 File Offset: 0x0000B023
		public UniTask<PointerEventData> OnPointerEnterAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPointerEnterHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerEnterAsync();
		}
	}
}
