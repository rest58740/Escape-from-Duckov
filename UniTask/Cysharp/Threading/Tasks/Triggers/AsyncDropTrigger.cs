using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000ED RID: 237
	[DisallowMultipleComponent]
	public sealed class AsyncDropTrigger : AsyncTriggerBase<PointerEventData>, IDropHandler, IEventSystemHandler
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0000CC73 File Offset: 0x0000AE73
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000CC7C File Offset: 0x0000AE7C
		public IAsyncOnDropHandler GetOnDropAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000CC85 File Offset: 0x0000AE85
		public IAsyncOnDropHandler GetOnDropAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0000CC8F File Offset: 0x0000AE8F
		public UniTask<PointerEventData> OnDropAsync()
		{
			return ((IAsyncOnDropHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnDropAsync();
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0000CC9D File Offset: 0x0000AE9D
		public UniTask<PointerEventData> OnDropAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnDropHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnDropAsync();
		}
	}
}
