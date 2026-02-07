using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000FB RID: 251
	[DisallowMultipleComponent]
	public sealed class AsyncPointerExitTrigger : AsyncTriggerBase<PointerEventData>, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x0000CE3A File Offset: 0x0000B03A
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000CE43 File Offset: 0x0000B043
		public IAsyncOnPointerExitHandler GetOnPointerExitAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000CE4C File Offset: 0x0000B04C
		public IAsyncOnPointerExitHandler GetOnPointerExitAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000CE56 File Offset: 0x0000B056
		public UniTask<PointerEventData> OnPointerExitAsync()
		{
			return ((IAsyncOnPointerExitHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerExitAsync();
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000CE64 File Offset: 0x0000B064
		public UniTask<PointerEventData> OnPointerExitAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPointerExitHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerExitAsync();
		}
	}
}
