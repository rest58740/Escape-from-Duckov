using System;
using System.Diagnostics;
using Unity;

namespace System.Threading.Tasks
{
	// Token: 0x02000328 RID: 808
	[DebuggerDisplay("ShouldExitCurrentIteration = {ShouldExitCurrentIteration}")]
	public class ParallelLoopState
	{
		// Token: 0x06002237 RID: 8759 RVA: 0x0007B93C File Offset: 0x00079B3C
		internal ParallelLoopState(ParallelLoopStateFlags fbase)
		{
			this._flagsBase = fbase;
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x0007B94B File Offset: 0x00079B4B
		internal virtual bool InternalShouldExitCurrentIteration
		{
			get
			{
				throw new NotSupportedException("This method is not supported.");
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x0007B957 File Offset: 0x00079B57
		public bool ShouldExitCurrentIteration
		{
			get
			{
				return this.InternalShouldExitCurrentIteration;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x0007B95F File Offset: 0x00079B5F
		public bool IsStopped
		{
			get
			{
				return (this._flagsBase.LoopStateFlags & 4) != 0;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x0007B971 File Offset: 0x00079B71
		public bool IsExceptional
		{
			get
			{
				return (this._flagsBase.LoopStateFlags & 1) != 0;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x0007B94B File Offset: 0x00079B4B
		internal virtual long? InternalLowestBreakIteration
		{
			get
			{
				throw new NotSupportedException("This method is not supported.");
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x0007B983 File Offset: 0x00079B83
		public long? LowestBreakIteration
		{
			get
			{
				return this.InternalLowestBreakIteration;
			}
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0007B98B File Offset: 0x00079B8B
		public void Stop()
		{
			this._flagsBase.Stop();
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0007B94B File Offset: 0x00079B4B
		internal virtual void InternalBreak()
		{
			throw new NotSupportedException("This method is not supported.");
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0007B998 File Offset: 0x00079B98
		public void Break()
		{
			this.InternalBreak();
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x0007B9A0 File Offset: 0x00079BA0
		internal static void Break(int iteration, ParallelLoopStateFlags32 pflags)
		{
			int num = 0;
			if (pflags.AtomicLoopStateUpdate(2, 13, ref num))
			{
				int lowestBreakIteration = pflags._lowestBreakIteration;
				if (iteration < lowestBreakIteration)
				{
					SpinWait spinWait = default(SpinWait);
					while (Interlocked.CompareExchange(ref pflags._lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
					{
						spinWait.SpinOnce();
						lowestBreakIteration = pflags._lowestBreakIteration;
						if (iteration > lowestBreakIteration)
						{
							break;
						}
					}
				}
				return;
			}
			if ((num & 4) != 0)
			{
				throw new InvalidOperationException("Break was called after Stop was called.");
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0007BA08 File Offset: 0x00079C08
		internal static void Break(long iteration, ParallelLoopStateFlags64 pflags)
		{
			int num = 0;
			if (pflags.AtomicLoopStateUpdate(2, 13, ref num))
			{
				long lowestBreakIteration = pflags.LowestBreakIteration;
				if (iteration < lowestBreakIteration)
				{
					SpinWait spinWait = default(SpinWait);
					while (Interlocked.CompareExchange(ref pflags._lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
					{
						spinWait.SpinOnce();
						lowestBreakIteration = pflags.LowestBreakIteration;
						if (iteration > lowestBreakIteration)
						{
							break;
						}
					}
				}
				return;
			}
			if ((num & 4) != 0)
			{
				throw new InvalidOperationException("Break was called after Stop was called.");
			}
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000173AD File Offset: 0x000155AD
		internal ParallelLoopState()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001C33 RID: 7219
		private readonly ParallelLoopStateFlags _flagsBase;
	}
}
