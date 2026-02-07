using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000030 RID: 48
	public static class Progress
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00003C0E File Offset: 0x00001E0E
		public static IProgress<T> Create<T>(Action<T> handler)
		{
			if (handler == null)
			{
				return Progress.NullProgress<T>.Instance;
			}
			return new Progress.AnonymousProgress<T>(handler);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003C1F File Offset: 0x00001E1F
		public static IProgress<T> CreateOnlyValueChanged<T>(Action<T> handler, IEqualityComparer<T> comparer = null)
		{
			if (handler == null)
			{
				return Progress.NullProgress<T>.Instance;
			}
			return new Progress.OnlyValueChangedProgress<T>(handler, comparer ?? UnityEqualityComparer.GetDefault<T>());
		}

		// Token: 0x02000160 RID: 352
		private sealed class NullProgress<T> : IProgress<T>
		{
			// Token: 0x06000771 RID: 1905 RVA: 0x00011480 File Offset: 0x0000F680
			private NullProgress()
			{
			}

			// Token: 0x06000772 RID: 1906 RVA: 0x00011488 File Offset: 0x0000F688
			public void Report(T value)
			{
			}

			// Token: 0x040001CC RID: 460
			public static readonly IProgress<T> Instance = new Progress.NullProgress<T>();
		}

		// Token: 0x02000161 RID: 353
		private sealed class AnonymousProgress<T> : IProgress<T>
		{
			// Token: 0x06000774 RID: 1908 RVA: 0x00011496 File Offset: 0x0000F696
			public AnonymousProgress(Action<T> action)
			{
				this.action = action;
			}

			// Token: 0x06000775 RID: 1909 RVA: 0x000114A5 File Offset: 0x0000F6A5
			public void Report(T value)
			{
				this.action(value);
			}

			// Token: 0x040001CD RID: 461
			private readonly Action<T> action;
		}

		// Token: 0x02000162 RID: 354
		private sealed class OnlyValueChangedProgress<T> : IProgress<T>
		{
			// Token: 0x06000776 RID: 1910 RVA: 0x000114B3 File Offset: 0x0000F6B3
			public OnlyValueChangedProgress(Action<T> action, IEqualityComparer<T> comparer)
			{
				this.action = action;
				this.comparer = comparer;
				this.isFirstCall = true;
			}

			// Token: 0x06000777 RID: 1911 RVA: 0x000114D0 File Offset: 0x0000F6D0
			public void Report(T value)
			{
				if (this.isFirstCall)
				{
					this.isFirstCall = false;
				}
				else if (this.comparer.Equals(value, this.latestValue))
				{
					return;
				}
				this.latestValue = value;
				this.action(value);
			}

			// Token: 0x040001CE RID: 462
			private readonly Action<T> action;

			// Token: 0x040001CF RID: 463
			private readonly IEqualityComparer<T> comparer;

			// Token: 0x040001D0 RID: 464
			private bool isFirstCall;

			// Token: 0x040001D1 RID: 465
			private T latestValue;
		}
	}
}
