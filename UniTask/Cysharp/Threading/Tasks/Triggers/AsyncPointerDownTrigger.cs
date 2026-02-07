using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F7 RID: 247
	[DisallowMultipleComponent]
	public sealed class AsyncPointerDownTrigger : AsyncTriggerBase<PointerEventData>, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000CDC1 File Offset: 0x0000AFC1
		public IAsyncOnPointerDownHandler GetOnPointerDownAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0000CDCA File Offset: 0x0000AFCA
		public IAsyncOnPointerDownHandler GetOnPointerDownAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
		public UniTask<PointerEventData> OnPointerDownAsync()
		{
			return ((IAsyncOnPointerDownHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerDownAsync();
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000CDE2 File Offset: 0x0000AFE2
		public UniTask<PointerEventData> OnPointerDownAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPointerDownHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerDownAsync();
		}
	}
}
