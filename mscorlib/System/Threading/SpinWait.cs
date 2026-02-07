using System;
using Internal.Runtime.Augments;

namespace System.Threading
{
	// Token: 0x020002A4 RID: 676
	public struct SpinWait
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x0006F48C File Offset: 0x0006D68C
		// (set) Token: 0x06001DFC RID: 7676 RVA: 0x0006F494 File Offset: 0x0006D694
		public int Count
		{
			get
			{
				return this._count;
			}
			internal set
			{
				this._count = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001DFD RID: 7677 RVA: 0x0006F49D File Offset: 0x0006D69D
		public bool NextSpinWillYield
		{
			get
			{
				return this._count >= 10 || PlatformHelper.IsSingleProcessor;
			}
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0006F4B0 File Offset: 0x0006D6B0
		public void SpinOnce()
		{
			this.SpinOnceCore(20);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0006F4BA File Offset: 0x0006D6BA
		public void SpinOnce(int sleep1Threshold)
		{
			if (sleep1Threshold < -1)
			{
				throw new ArgumentOutOfRangeException("sleep1Threshold", sleep1Threshold, "Number must be either non-negative and less than or equal to Int32.MaxValue or -1.");
			}
			if (sleep1Threshold >= 0 && sleep1Threshold < 10)
			{
				sleep1Threshold = 10;
			}
			this.SpinOnceCore(sleep1Threshold);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0006F4EC File Offset: 0x0006D6EC
		private void SpinOnceCore(int sleep1Threshold)
		{
			if ((this._count >= 10 && ((this._count >= sleep1Threshold && sleep1Threshold >= 0) || (this._count - 10) % 2 == 0)) || PlatformHelper.IsSingleProcessor)
			{
				if (this._count >= sleep1Threshold && sleep1Threshold >= 0)
				{
					RuntimeThread.Sleep(1);
				}
				else if (((this._count >= 10) ? ((this._count - 10) / 2) : this._count) % 5 == 4)
				{
					RuntimeThread.Sleep(0);
				}
				else
				{
					RuntimeThread.Yield();
				}
			}
			else
			{
				int num = RuntimeThread.OptimalMaxSpinWaitsPerSpinIteration;
				if (this._count <= 30 && 1 << this._count < num)
				{
					num = 1 << this._count;
				}
				RuntimeThread.SpinWait(num);
			}
			this._count = ((this._count == int.MaxValue) ? 10 : (this._count + 1));
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0006F5BB File Offset: 0x0006D7BB
		public void Reset()
		{
			this._count = 0;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0006F5C4 File Offset: 0x0006D7C4
		public static void SpinUntil(Func<bool> condition)
		{
			SpinWait.SpinUntil(condition, -1);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0006F5D0 File Offset: 0x0006D7D0
		public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.");
			}
			return SpinWait.SpinUntil(condition, (int)num);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0006F614 File Offset: 0x0006D814
		public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.");
			}
			if (condition == null)
			{
				throw new ArgumentNullException("condition", "The condition argument is null.");
			}
			uint num = 0U;
			if (millisecondsTimeout != 0 && millisecondsTimeout != -1)
			{
				num = TimeoutHelper.GetTime();
			}
			SpinWait spinWait = default(SpinWait);
			while (!condition())
			{
				if (millisecondsTimeout == 0)
				{
					return false;
				}
				spinWait.SpinOnce();
				if (millisecondsTimeout != -1 && spinWait.NextSpinWillYield && (long)millisecondsTimeout <= (long)((ulong)(TimeoutHelper.GetTime() - num)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001A6D RID: 6765
		internal const int YieldThreshold = 10;

		// Token: 0x04001A6E RID: 6766
		private const int Sleep0EveryHowManyYields = 5;

		// Token: 0x04001A6F RID: 6767
		internal const int DefaultSleep1Threshold = 20;

		// Token: 0x04001A70 RID: 6768
		internal static readonly int SpinCountforSpinBeforeWait = PlatformHelper.IsSingleProcessor ? 1 : 35;

		// Token: 0x04001A71 RID: 6769
		internal const int Sleep1ThresholdForLongSpinBeforeWait = 40;

		// Token: 0x04001A72 RID: 6770
		private int _count;
	}
}
