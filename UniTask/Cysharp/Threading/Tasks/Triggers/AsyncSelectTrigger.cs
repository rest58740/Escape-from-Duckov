using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000101 RID: 257
	[DisallowMultipleComponent]
	public sealed class AsyncSelectTrigger : AsyncTriggerBase<BaseEventData>, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x0000CEFD File Offset: 0x0000B0FD
		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000CF06 File Offset: 0x0000B106
		public IAsyncOnSelectHandler GetOnSelectAsyncHandler()
		{
			return new AsyncTriggerHandler<BaseEventData>(this, false);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0000CF0F File Offset: 0x0000B10F
		public IAsyncOnSelectHandler GetOnSelectAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0000CF19 File Offset: 0x0000B119
		public UniTask<BaseEventData> OnSelectAsync()
		{
			return ((IAsyncOnSelectHandler)new AsyncTriggerHandler<BaseEventData>(this, true)).OnSelectAsync();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0000CF27 File Offset: 0x0000B127
		public UniTask<BaseEventData> OnSelectAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnSelectHandler)new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, true)).OnSelectAsync();
		}
	}
}
