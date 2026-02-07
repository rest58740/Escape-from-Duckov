using System;
using System.Diagnostics;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200010D RID: 269
	internal sealed class PlayerLoopRunner
	{
		// Token: 0x06000630 RID: 1584 RVA: 0x0000E254 File Offset: 0x0000C454
		public PlayerLoopRunner(PlayerLoopTiming timing)
		{
			this.unhandledExceptionCallback = delegate(Exception ex)
			{
				Debug.LogException(ex);
			};
			this.timing = timing;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
		public void AddAction(IPlayerLoopItem item)
		{
			object obj = this.runningAndQueueLock;
			lock (obj)
			{
				if (this.running)
				{
					this.waitQueue.Enqueue(item);
					return;
				}
			}
			obj = this.arrayLock;
			lock (obj)
			{
				if (this.loopItems.Length == this.tail)
				{
					Array.Resize<IPlayerLoopItem>(ref this.loopItems, checked(this.tail * 2));
				}
				IPlayerLoopItem[] array = this.loopItems;
				int num = this.tail;
				this.tail = num + 1;
				array[num] = item;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0000E37C File Offset: 0x0000C57C
		public int Clear()
		{
			object obj = this.arrayLock;
			int result;
			lock (obj)
			{
				int num = 0;
				for (int i = 0; i < this.loopItems.Length; i++)
				{
					if (this.loopItems[i] != null)
					{
						num++;
					}
					this.loopItems[i] = null;
				}
				this.tail = 0;
				result = num;
			}
			return result;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000E3F0 File Offset: 0x0000C5F0
		public void Run()
		{
			this.RunCore();
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
		private void Initialization()
		{
			this.RunCore();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0000E400 File Offset: 0x0000C600
		private void LastInitialization()
		{
			this.RunCore();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0000E408 File Offset: 0x0000C608
		private void EarlyUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0000E410 File Offset: 0x0000C610
		private void LastEarlyUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0000E418 File Offset: 0x0000C618
		private void FixedUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0000E420 File Offset: 0x0000C620
		private void LastFixedUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0000E428 File Offset: 0x0000C628
		private void PreUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0000E430 File Offset: 0x0000C630
		private void LastPreUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0000E438 File Offset: 0x0000C638
		private void Update()
		{
			this.RunCore();
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0000E440 File Offset: 0x0000C640
		private void LastUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0000E448 File Offset: 0x0000C648
		private void PreLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0000E450 File Offset: 0x0000C650
		private void LastPreLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0000E458 File Offset: 0x0000C658
		private void PostLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0000E460 File Offset: 0x0000C660
		private void LastPostLateUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0000E468 File Offset: 0x0000C668
		private void TimeUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0000E470 File Offset: 0x0000C670
		private void LastTimeUpdate()
		{
			this.RunCore();
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0000E478 File Offset: 0x0000C678
		[DebuggerHidden]
		private void RunCore()
		{
			object obj = this.runningAndQueueLock;
			lock (obj)
			{
				this.running = true;
			}
			obj = this.arrayLock;
			lock (obj)
			{
				int num = this.tail - 1;
				int i = 0;
				while (i < this.loopItems.Length)
				{
					IPlayerLoopItem playerLoopItem = this.loopItems[i];
					if (playerLoopItem != null)
					{
						try
						{
							if (!playerLoopItem.MoveNext())
							{
								this.loopItems[i] = null;
								goto IL_F9;
							}
							goto IL_106;
						}
						catch (Exception obj2)
						{
							this.loopItems[i] = null;
							try
							{
								this.unhandledExceptionCallback(obj2);
							}
							catch
							{
							}
							goto IL_F9;
						}
						goto IL_93;
					}
					goto IL_F9;
					IL_106:
					i++;
					continue;
					IL_93:
					IPlayerLoopItem playerLoopItem2 = this.loopItems[num];
					if (playerLoopItem2 != null)
					{
						try
						{
							if (!playerLoopItem2.MoveNext())
							{
								this.loopItems[num] = null;
								num--;
								goto IL_F9;
							}
							this.loopItems[i] = playerLoopItem2;
							this.loopItems[num] = null;
							num--;
							goto IL_106;
						}
						catch (Exception obj3)
						{
							this.loopItems[num] = null;
							num--;
							try
							{
								this.unhandledExceptionCallback(obj3);
							}
							catch
							{
							}
							goto IL_F9;
						}
					}
					num--;
					IL_F9:
					if (i >= num)
					{
						this.tail = i;
						break;
					}
					goto IL_93;
				}
				object obj4 = this.runningAndQueueLock;
				lock (obj4)
				{
					this.running = false;
					while (this.waitQueue.Count != 0)
					{
						if (this.loopItems.Length == this.tail)
						{
							Array.Resize<IPlayerLoopItem>(ref this.loopItems, checked(this.tail * 2));
						}
						IPlayerLoopItem[] array = this.loopItems;
						int num2 = this.tail;
						this.tail = num2 + 1;
						array[num2] = this.waitQueue.Dequeue();
					}
				}
			}
		}

		// Token: 0x04000113 RID: 275
		private const int InitialSize = 16;

		// Token: 0x04000114 RID: 276
		private readonly PlayerLoopTiming timing;

		// Token: 0x04000115 RID: 277
		private readonly object runningAndQueueLock = new object();

		// Token: 0x04000116 RID: 278
		private readonly object arrayLock = new object();

		// Token: 0x04000117 RID: 279
		private readonly Action<Exception> unhandledExceptionCallback;

		// Token: 0x04000118 RID: 280
		private int tail;

		// Token: 0x04000119 RID: 281
		private bool running;

		// Token: 0x0400011A RID: 282
		private IPlayerLoopItem[] loopItems = new IPlayerLoopItem[16];

		// Token: 0x0400011B RID: 283
		private MinimumQueue<IPlayerLoopItem> waitQueue = new MinimumQueue<IPlayerLoopItem>(16);
	}
}
