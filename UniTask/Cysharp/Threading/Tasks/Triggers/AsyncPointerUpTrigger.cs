using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000FD RID: 253
	[DisallowMultipleComponent]
	public sealed class AsyncPointerUpTrigger : AsyncTriggerBase<PointerEventData>, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0000CE7B File Offset: 0x0000B07B
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000CE84 File Offset: 0x0000B084
		public IAsyncOnPointerUpHandler GetOnPointerUpAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0000CE8D File Offset: 0x0000B08D
		public IAsyncOnPointerUpHandler GetOnPointerUpAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000CE97 File Offset: 0x0000B097
		public UniTask<PointerEventData> OnPointerUpAsync()
		{
			return ((IAsyncOnPointerUpHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerUpAsync();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0000CEA5 File Offset: 0x0000B0A5
		public UniTask<PointerEventData> OnPointerUpAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPointerUpHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerUpAsync();
		}
	}
}
