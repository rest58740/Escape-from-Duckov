using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200007B RID: 123
	[DisallowMultipleComponent]
	public sealed class AsyncApplicationFocusTrigger : AsyncTriggerBase<bool>
	{
		// Token: 0x0600040B RID: 1035 RVA: 0x0000BD6E File Offset: 0x00009F6E
		private void OnApplicationFocus(bool hasFocus)
		{
			base.RaiseEvent(hasFocus);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000BD77 File Offset: 0x00009F77
		public IAsyncOnApplicationFocusHandler GetOnApplicationFocusAsyncHandler()
		{
			return new AsyncTriggerHandler<bool>(this, false);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000BD80 File Offset: 0x00009F80
		public IAsyncOnApplicationFocusHandler GetOnApplicationFocusAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<bool>(this, cancellationToken, false);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000BD8A File Offset: 0x00009F8A
		public UniTask<bool> OnApplicationFocusAsync()
		{
			return ((IAsyncOnApplicationFocusHandler)new AsyncTriggerHandler<bool>(this, true)).OnApplicationFocusAsync();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000BD98 File Offset: 0x00009F98
		public UniTask<bool> OnApplicationFocusAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnApplicationFocusHandler)new AsyncTriggerHandler<bool>(this, cancellationToken, true)).OnApplicationFocusAsync();
		}
	}
}
