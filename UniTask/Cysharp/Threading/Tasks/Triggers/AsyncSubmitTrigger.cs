using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000103 RID: 259
	[DisallowMultipleComponent]
	public sealed class AsyncSubmitTrigger : AsyncTriggerBase<BaseEventData>, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x0000CF3E File Offset: 0x0000B13E
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			base.RaiseEvent(eventData);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0000CF47 File Offset: 0x0000B147
		public IAsyncOnSubmitHandler GetOnSubmitAsyncHandler()
		{
			return new AsyncTriggerHandler<BaseEventData>(this, false);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000CF50 File Offset: 0x0000B150
		public IAsyncOnSubmitHandler GetOnSubmitAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, false);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0000CF5A File Offset: 0x0000B15A
		public UniTask<BaseEventData> OnSubmitAsync()
		{
			return ((IAsyncOnSubmitHandler)new AsyncTriggerHandler<BaseEventData>(this, true)).OnSubmitAsync();
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0000CF68 File Offset: 0x0000B168
		public UniTask<BaseEventData> OnSubmitAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnSubmitHandler)new AsyncTriggerHandler<BaseEventData>(this, cancellationToken, true)).OnSubmitAsync();
		}
	}
}
