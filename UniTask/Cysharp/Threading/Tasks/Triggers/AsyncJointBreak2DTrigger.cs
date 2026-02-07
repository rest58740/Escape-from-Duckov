using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000A5 RID: 165
	[DisallowMultipleComponent]
	public sealed class AsyncJointBreak2DTrigger : AsyncTriggerBase<Joint2D>
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x0000C2F1 File Offset: 0x0000A4F1
		private void OnJointBreak2D(Joint2D brokenJoint)
		{
			base.RaiseEvent(brokenJoint);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000C2FA File Offset: 0x0000A4FA
		public IAsyncOnJointBreak2DHandler GetOnJointBreak2DAsyncHandler()
		{
			return new AsyncTriggerHandler<Joint2D>(this, false);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000C303 File Offset: 0x0000A503
		public IAsyncOnJointBreak2DHandler GetOnJointBreak2DAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Joint2D>(this, cancellationToken, false);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000C30D File Offset: 0x0000A50D
		public UniTask<Joint2D> OnJointBreak2DAsync()
		{
			return ((IAsyncOnJointBreak2DHandler)new AsyncTriggerHandler<Joint2D>(this, true)).OnJointBreak2DAsync();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000C31B File Offset: 0x0000A51B
		public UniTask<Joint2D> OnJointBreak2DAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnJointBreak2DHandler)new AsyncTriggerHandler<Joint2D>(this, cancellationToken, true)).OnJointBreak2DAsync();
		}
	}
}
