using System;
using System.Threading;
using Pathfinding.Collections;

namespace Pathfinding.Sync
{
	// Token: 0x02000226 RID: 550
	internal class BlockableChannel<T> where T : class
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x000534D0 File Offset: 0x000516D0
		// (set) Token: 0x06000D1F RID: 3359 RVA: 0x000534D8 File Offset: 0x000516D8
		public int numReceivers { get; private set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x000534E1 File Offset: 0x000516E1
		// (set) Token: 0x06000D21 RID: 3361 RVA: 0x000534E9 File Offset: 0x000516E9
		public bool isClosed { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x000534F4 File Offset: 0x000516F4
		public bool isEmpty
		{
			get
			{
				object obj = this.lockObj;
				bool result;
				lock (obj)
				{
					result = (this.queue.Length == 0);
				}
				return result;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00053540 File Offset: 0x00051740
		public bool allReceiversBlocked
		{
			get
			{
				return this.blocked && this.waitingReceivers == this.numReceivers;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0005355E File Offset: 0x0005175E
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x00053568 File Offset: 0x00051768
		public bool isBlocked
		{
			get
			{
				return this.blocked;
			}
			set
			{
				object obj = this.lockObj;
				lock (obj)
				{
					this.blocked = value;
					if (!this.isClosed)
					{
						this.isStarving = (value || this.queue.Length == 0);
					}
				}
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000535D0 File Offset: 0x000517D0
		public void Close()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.isClosed = true;
				this.isStarving = false;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00053618 File Offset: 0x00051818
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x00053629 File Offset: 0x00051829
		private bool isStarving
		{
			get
			{
				return !this.starving.WaitOne(0);
			}
			set
			{
				if (value)
				{
					this.starving.Reset();
					return;
				}
				this.starving.Set();
			}
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00053648 File Offset: 0x00051848
		public void Reopen()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.numReceivers != 0)
				{
					throw new InvalidOperationException("Can only reopen a channel after Close has been called on all receivers");
				}
				this.isClosed = false;
				this.isBlocked = false;
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000536A4 File Offset: 0x000518A4
		public BlockableChannel<T>.Receiver AddReceiver()
		{
			object obj = this.lockObj;
			BlockableChannel<T>.Receiver result;
			lock (obj)
			{
				if (this.isClosed)
				{
					throw new InvalidOperationException("Channel is closed");
				}
				int numReceivers = this.numReceivers;
				this.numReceivers = numReceivers + 1;
				result = new BlockableChannel<T>.Receiver(this);
			}
			return result;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0005370C File Offset: 0x0005190C
		public void PushFront(T path)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.isClosed)
				{
					throw new InvalidOperationException("Channel is closed");
				}
				this.queue.PushStart(path);
				if (!this.blocked)
				{
					this.isStarving = false;
				}
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00053778 File Offset: 0x00051978
		public void Push(T path)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.isClosed)
				{
					throw new InvalidOperationException("Channel is closed");
				}
				this.queue.PushEnd(path);
				if (!this.blocked)
				{
					this.isStarving = false;
				}
			}
		}

		// Token: 0x04000A2D RID: 2605
		private readonly object lockObj = new object();

		// Token: 0x04000A2E RID: 2606
		private CircularBuffer<T> queue = new CircularBuffer<T>(16);

		// Token: 0x04000A30 RID: 2608
		private volatile int waitingReceivers;

		// Token: 0x04000A31 RID: 2609
		private ManualResetEvent starving = new ManualResetEvent(false);

		// Token: 0x04000A32 RID: 2610
		private volatile bool blocked;

		// Token: 0x02000227 RID: 551
		public enum PopState
		{
			// Token: 0x04000A35 RID: 2613
			Ok,
			// Token: 0x04000A36 RID: 2614
			Wait,
			// Token: 0x04000A37 RID: 2615
			Closed
		}

		// Token: 0x02000228 RID: 552
		public struct Receiver
		{
			// Token: 0x06000D2E RID: 3374 RVA: 0x00053810 File Offset: 0x00051A10
			public Receiver(BlockableChannel<T> channel)
			{
				this.channel = channel;
			}

			// Token: 0x06000D2F RID: 3375 RVA: 0x0005381C File Offset: 0x00051A1C
			public void Close()
			{
				object lockObj = this.channel.lockObj;
				lock (lockObj)
				{
					BlockableChannel<T> blockableChannel = this.channel;
					int numReceivers = blockableChannel.numReceivers;
					blockableChannel.numReceivers = numReceivers - 1;
				}
				this.channel = null;
			}

			// Token: 0x06000D30 RID: 3376 RVA: 0x00053878 File Offset: 0x00051A78
			public BlockableChannel<T>.PopState Receive(out T item)
			{
				Interlocked.Increment(ref this.channel.waitingReceivers);
				BlockableChannel<T>.PopState result;
				for (;;)
				{
					this.channel.starving.WaitOne();
					object lockObj = this.channel.lockObj;
					lock (lockObj)
					{
						if (this.channel.isClosed)
						{
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							item = default(T);
							result = BlockableChannel<T>.PopState.Closed;
						}
						else
						{
							if (this.channel.queue.Length == 0)
							{
								this.channel.isStarving = true;
							}
							if (this.channel.isStarving)
							{
								continue;
							}
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							item = this.channel.queue.PopStart();
							result = BlockableChannel<T>.PopState.Ok;
						}
					}
					break;
				}
				return result;
			}

			// Token: 0x06000D31 RID: 3377 RVA: 0x0005395C File Offset: 0x00051B5C
			public BlockableChannel<T>.PopState ReceiveNoBlock(bool blockedBefore, out T item)
			{
				item = default(T);
				if (!blockedBefore)
				{
					Interlocked.Increment(ref this.channel.waitingReceivers);
				}
				while (!this.channel.isStarving)
				{
					object lockObj = this.channel.lockObj;
					BlockableChannel<T>.PopState result;
					lock (lockObj)
					{
						if (this.channel.isClosed)
						{
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							result = BlockableChannel<T>.PopState.Closed;
						}
						else
						{
							if (this.channel.queue.Length == 0)
							{
								this.channel.isStarving = true;
							}
							if (this.channel.isStarving)
							{
								continue;
							}
							Interlocked.Decrement(ref this.channel.waitingReceivers);
							item = this.channel.queue.PopStart();
							result = BlockableChannel<T>.PopState.Ok;
						}
					}
					return result;
				}
				return BlockableChannel<T>.PopState.Wait;
			}

			// Token: 0x04000A38 RID: 2616
			private BlockableChannel<T> channel;
		}
	}
}
