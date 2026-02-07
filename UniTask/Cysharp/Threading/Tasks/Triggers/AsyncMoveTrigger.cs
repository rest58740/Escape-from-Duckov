using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F3 RID: 243
	[DisallowMultipleComponent]
	public sealed class AsyncMoveTrigger : AsyncTriggerBase<AxisEventData>, IMoveHandler, IEventSystemHandler
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x0000CD36 File Offset: 0x0000AF36
		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000CD3F File Offset: 0x0000AF3F
		public IAsyncOnMoveHandler GetOnMoveAsyncHandler()
		{
			return new AsyncTriggerHandler<AxisEventData>(this, false);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000CD48 File Offset: 0x0000AF48
		public IAsyncOnMoveHandler GetOnMoveAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AxisEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0000CD52 File Offset: 0x0000AF52
		public UniTask<AxisEventData> OnMoveAsync()
		{
			return ((IAsyncOnMoveHandler)new AsyncTriggerHandler<AxisEventData>(this, true)).OnMoveAsync();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0000CD60 File Offset: 0x0000AF60
		public UniTask<AxisEventData> OnMoveAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMoveHandler)new AsyncTriggerHandler<AxisEventData>(this, cancellationToken, true)).OnMoveAsync();
		}
	}
}
