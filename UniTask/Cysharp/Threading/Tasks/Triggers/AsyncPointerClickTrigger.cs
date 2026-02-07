using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F5 RID: 245
	[DisallowMultipleComponent]
	public sealed class AsyncPointerClickTrigger : AsyncTriggerBase<PointerEventData>, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x0000CD77 File Offset: 0x0000AF77
		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000CD80 File Offset: 0x0000AF80
		public IAsyncOnPointerClickHandler GetOnPointerClickAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000CD89 File Offset: 0x0000AF89
		public IAsyncOnPointerClickHandler GetOnPointerClickAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000CD93 File Offset: 0x0000AF93
		public UniTask<PointerEventData> OnPointerClickAsync()
		{
			return ((IAsyncOnPointerClickHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerClickAsync();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000CDA1 File Offset: 0x0000AFA1
		public UniTask<PointerEventData> OnPointerClickAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPointerClickHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerClickAsync();
		}
	}
}
