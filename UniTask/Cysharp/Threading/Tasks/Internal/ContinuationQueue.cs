using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000109 RID: 265
	internal sealed class ContinuationQueue
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0000D540 File Offset: 0x0000B740
		public ContinuationQueue(PlayerLoopTiming timing)
		{
			this.timing = timing;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0000D578 File Offset: 0x0000B778
		public void Enqueue(Action continuation)
		{
			bool flag = false;
			try
			{
				this.gate.Enter(ref flag);
				if (this.dequing)
				{
					if (this.waitingList.Length == this.waitingListCount)
					{
						int num = this.waitingListCount * 2;
						if (num > 2146435071)
						{
							num = 2146435071;
						}
						Action[] destinationArray = new Action[num];
						Array.Copy(this.waitingList, destinationArray, this.waitingListCount);
						this.waitingList = destinationArray;
					}
					this.waitingList[this.waitingListCount] = continuation;
					this.waitingListCount++;
				}
				else
				{
					if (this.actionList.Length == this.actionListCount)
					{
						int num2 = this.actionListCount * 2;
						if (num2 > 2146435071)
						{
							num2 = 2146435071;
						}
						Action[] destinationArray2 = new Action[num2];
						Array.Copy(this.actionList, destinationArray2, this.actionListCount);
						this.actionList = destinationArray2;
					}
					this.actionList[this.actionListCount] = continuation;
					this.actionListCount++;
				}
			}
			finally
			{
				if (flag)
				{
					this.gate.Exit(false);
				}
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0000D68C File Offset: 0x0000B88C
		public int Clear()
		{
			int result = this.actionListCount + this.waitingListCount;
			this.actionListCount = 0;
			this.actionList = new Action[16];
			this.waitingListCount = 0;
			this.waitingList = new Action[16];
			return result;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0000D6C3 File Offset: 0x0000B8C3
		public void Run()
		{
			this.RunCore();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0000D6CB File Offset: 0x0000B8CB
		private void Initialization()
		{
			this.RunCore();
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0000D6D3 File Offset: 0x0000B8D3
		private void LastInitialization()
		{
			this.RunCore();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0000D6DB File Offset: 0x0000B8DB
		private void EarlyUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0000D6E3 File Offset: 0x0000B8E3
		private void LastEarlyUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0000D6EB File Offset: 0x0000B8EB
		private void FixedUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0000D6F3 File Offset: 0x0000B8F3
		private void LastFixedUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0000D6FB File Offset: 0x0000B8FB
		private void PreUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000D703 File Offset: 0x0000B903
		private void LastPreUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0000D70B File Offset: 0x0000B90B
		private void Update()
		{
			this.RunCore();
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0000D713 File Offset: 0x0000B913
		private void LastUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0000D71B File Offset: 0x0000B91B
		private void PreLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0000D723 File Offset: 0x0000B923
		private void LastPreLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0000D72B File Offset: 0x0000B92B
		private void PostLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0000D733 File Offset: 0x0000B933
		private void LastPostLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0000D73B File Offset: 0x0000B93B
		private void TimeUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0000D743 File Offset: 0x0000B943
		private void LastTimeUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0000D74C File Offset: 0x0000B94C
		[DebuggerHidden]
		private void RunCore()
		{
			bool flag = false;
			try
			{
				this.gate.Enter(ref flag);
				if (this.actionListCount == 0)
				{
					return;
				}
				this.dequing = true;
			}
			finally
			{
				if (flag)
				{
					this.gate.Exit(false);
				}
			}
			for (int i = 0; i < this.actionListCount; i++)
			{
				Action action = this.actionList[i];
				this.actionList[i] = null;
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			bool flag2 = false;
			try
			{
				this.gate.Enter(ref flag2);
				this.dequing = false;
				Action[] array = this.actionList;
				this.actionListCount = this.waitingListCount;
				this.actionList = this.waitingList;
				this.waitingListCount = 0;
				this.waitingList = array;
			}
			finally
			{
				if (flag2)
				{
					this.gate.Exit(false);
				}
			}
		}

		// Token: 0x04000101 RID: 257
		private const int MaxArrayLength = 2146435071;

		// Token: 0x04000102 RID: 258
		private const int InitialSize = 16;

		// Token: 0x04000103 RID: 259
		private readonly PlayerLoopTiming timing;

		// Token: 0x04000104 RID: 260
		private SpinLock gate = new SpinLock(false);

		// Token: 0x04000105 RID: 261
		private bool dequing;

		// Token: 0x04000106 RID: 262
		private int actionListCount;

		// Token: 0x04000107 RID: 263
		private Action[] actionList = new Action[16];

		// Token: 0x04000108 RID: 264
		private int waitingListCount;

		// Token: 0x04000109 RID: 265
		private Action[] waitingList = new Action[16];
	}
}
