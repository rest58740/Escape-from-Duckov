using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F1 RID: 241
	[DisallowMultipleComponent]
	public sealed class AsyncInitializePotentialDragTrigger : AsyncTriggerBase<PointerEventData>, IInitializePotentialDragHandler, IEventSystemHandler
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0000CCF5 File Offset: 0x0000AEF5
		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000CCFE File Offset: 0x0000AEFE
		public IAsyncOnInitializePotentialDragHandler GetOnInitializePotentialDragAsyncHandler()
		{
			return new AsyncTriggerHandler<PointerEventData>(this, false);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0000CD07 File Offset: 0x0000AF07
		public IAsyncOnInitializePotentialDragHandler GetOnInitializePotentialDragAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000CD11 File Offset: 0x0000AF11
		public UniTask<PointerEventData> OnInitializePotentialDragAsync()
		{
			return ((IAsyncOnInitializePotentialDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnInitializePotentialDragAsync();
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000CD1F File Offset: 0x0000AF1F
		public UniTask<PointerEventData> OnInitializePotentialDragAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnInitializePotentialDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnInitializePotentialDragAsync();
		}
	}
}
