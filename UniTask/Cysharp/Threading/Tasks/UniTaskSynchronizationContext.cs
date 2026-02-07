using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000058 RID: 88
	public class UniTaskSynchronizationContext : SynchronizationContext
	{
		// Token: 0x06000237 RID: 567 RVA: 0x00009457 File Offset: 0x00007657
		public override void Send(SendOrPostCallback d, object state)
		{
			d(state);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00009460 File Offset: 0x00007660
		public override void Post(SendOrPostCallback d, object state)
		{
			bool flag = false;
			try
			{
				UniTaskSynchronizationContext.gate.Enter(ref flag);
				if (UniTaskSynchronizationContext.dequing)
				{
					if (UniTaskSynchronizationContext.waitingList.Length == UniTaskSynchronizationContext.waitingListCount)
					{
						int num = UniTaskSynchronizationContext.waitingListCount * 2;
						if (num > 2146435071)
						{
							num = 2146435071;
						}
						UniTaskSynchronizationContext.Callback[] destinationArray = new UniTaskSynchronizationContext.Callback[num];
						Array.Copy(UniTaskSynchronizationContext.waitingList, destinationArray, UniTaskSynchronizationContext.waitingListCount);
						UniTaskSynchronizationContext.waitingList = destinationArray;
					}
					UniTaskSynchronizationContext.waitingList[UniTaskSynchronizationContext.waitingListCount] = new UniTaskSynchronizationContext.Callback(d, state);
					UniTaskSynchronizationContext.waitingListCount++;
				}
				else
				{
					if (UniTaskSynchronizationContext.actionList.Length == UniTaskSynchronizationContext.actionListCount)
					{
						int num2 = UniTaskSynchronizationContext.actionListCount * 2;
						if (num2 > 2146435071)
						{
							num2 = 2146435071;
						}
						UniTaskSynchronizationContext.Callback[] destinationArray2 = new UniTaskSynchronizationContext.Callback[num2];
						Array.Copy(UniTaskSynchronizationContext.actionList, destinationArray2, UniTaskSynchronizationContext.actionListCount);
						UniTaskSynchronizationContext.actionList = destinationArray2;
					}
					UniTaskSynchronizationContext.actionList[UniTaskSynchronizationContext.actionListCount] = new UniTaskSynchronizationContext.Callback(d, state);
					UniTaskSynchronizationContext.actionListCount++;
				}
			}
			finally
			{
				if (flag)
				{
					UniTaskSynchronizationContext.gate.Exit(false);
				}
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009570 File Offset: 0x00007770
		public override void OperationStarted()
		{
			Interlocked.Increment(ref UniTaskSynchronizationContext.opCount);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000957D File Offset: 0x0000777D
		public override void OperationCompleted()
		{
			Interlocked.Decrement(ref UniTaskSynchronizationContext.opCount);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000958A File Offset: 0x0000778A
		public override SynchronizationContext CreateCopy()
		{
			return this;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009590 File Offset: 0x00007790
		internal static void Run()
		{
			bool flag = false;
			try
			{
				UniTaskSynchronizationContext.gate.Enter(ref flag);
				if (UniTaskSynchronizationContext.actionListCount == 0)
				{
					return;
				}
				UniTaskSynchronizationContext.dequing = true;
			}
			finally
			{
				if (flag)
				{
					UniTaskSynchronizationContext.gate.Exit(false);
				}
			}
			for (int i = 0; i < UniTaskSynchronizationContext.actionListCount; i++)
			{
				UniTaskSynchronizationContext.Callback callback = UniTaskSynchronizationContext.actionList[i];
				UniTaskSynchronizationContext.actionList[i] = default(UniTaskSynchronizationContext.Callback);
				callback.Invoke();
			}
			bool flag2 = false;
			try
			{
				UniTaskSynchronizationContext.gate.Enter(ref flag2);
				UniTaskSynchronizationContext.dequing = false;
				UniTaskSynchronizationContext.Callback[] array = UniTaskSynchronizationContext.actionList;
				UniTaskSynchronizationContext.actionListCount = UniTaskSynchronizationContext.waitingListCount;
				UniTaskSynchronizationContext.actionList = UniTaskSynchronizationContext.waitingList;
				UniTaskSynchronizationContext.waitingListCount = 0;
				UniTaskSynchronizationContext.waitingList = array;
			}
			finally
			{
				if (flag2)
				{
					UniTaskSynchronizationContext.gate.Exit(false);
				}
			}
		}

		// Token: 0x040000C6 RID: 198
		private const int MaxArrayLength = 2146435071;

		// Token: 0x040000C7 RID: 199
		private const int InitialSize = 16;

		// Token: 0x040000C8 RID: 200
		private static SpinLock gate = new SpinLock(false);

		// Token: 0x040000C9 RID: 201
		private static bool dequing = false;

		// Token: 0x040000CA RID: 202
		private static int actionListCount = 0;

		// Token: 0x040000CB RID: 203
		private static UniTaskSynchronizationContext.Callback[] actionList = new UniTaskSynchronizationContext.Callback[16];

		// Token: 0x040000CC RID: 204
		private static int waitingListCount = 0;

		// Token: 0x040000CD RID: 205
		private static UniTaskSynchronizationContext.Callback[] waitingList = new UniTaskSynchronizationContext.Callback[16];

		// Token: 0x040000CE RID: 206
		private static int opCount;

		// Token: 0x020001ED RID: 493
		[StructLayout(LayoutKind.Auto)]
		private readonly struct Callback
		{
			// Token: 0x06000B02 RID: 2818 RVA: 0x00027BB2 File Offset: 0x00025DB2
			public Callback(SendOrPostCallback callback, object state)
			{
				this.callback = callback;
				this.state = state;
			}

			// Token: 0x06000B03 RID: 2819 RVA: 0x00027BC4 File Offset: 0x00025DC4
			public void Invoke()
			{
				try
				{
					this.callback(this.state);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}

			// Token: 0x04000485 RID: 1157
			private readonly SendOrPostCallback callback;

			// Token: 0x04000486 RID: 1158
			private readonly object state;
		}
	}
}
