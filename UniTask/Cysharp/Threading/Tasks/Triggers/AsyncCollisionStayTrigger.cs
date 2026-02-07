using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000093 RID: 147
	[DisallowMultipleComponent]
	public sealed class AsyncCollisionStayTrigger : AsyncTriggerBase<Collision>
	{
		// Token: 0x0600045F RID: 1119 RVA: 0x0000C094 File Offset: 0x0000A294
		private void OnCollisionStay(Collision coll)
		{
			base.RaiseEvent(coll);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000C09D File Offset: 0x0000A29D
		public IAsyncOnCollisionStayHandler GetOnCollisionStayAsyncHandler()
		{
			return new AsyncTriggerHandler<Collision>(this, false);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000C0A6 File Offset: 0x0000A2A6
		public IAsyncOnCollisionStayHandler GetOnCollisionStayAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collision>(this, cancellationToken, false);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
		public UniTask<Collision> OnCollisionStayAsync()
		{
			return ((IAsyncOnCollisionStayHandler)new AsyncTriggerHandler<Collision>(this, true)).OnCollisionStayAsync();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000C0BE File Offset: 0x0000A2BE
		public UniTask<Collision> OnCollisionStayAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCollisionStayHandler)new AsyncTriggerHandler<Collision>(this, cancellationToken, true)).OnCollisionStayAsync();
		}
	}
}
