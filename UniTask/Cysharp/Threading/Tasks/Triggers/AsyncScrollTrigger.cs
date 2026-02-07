using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000FF RID: 255
	[DisallowMultipleComponent]
	public sealed class AsyncScrollTrigger : AsyncTriggerBase<PointerEventData>, IScrollHandler, IEventSystemHandler
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0000CEC5 File Offset: 0x0000B0C5
		public IAsyncOnScrollHandler GetOnScrollAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0000CECE File Offset: 0x0000B0CE
		public IAsyncOnScrollHandler GetOnScrollAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0000CED8 File Offset: 0x0000B0D8
		public UniTask<PointerEventData> OnScrollAsync()
		{
			return ((IAsyncOnScrollHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnScrollAsync();
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0000CEE6 File Offset: 0x0000B0E6
		public UniTask<PointerEventData> OnScrollAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnScrollHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnScrollAsync();
		}
	}
}
