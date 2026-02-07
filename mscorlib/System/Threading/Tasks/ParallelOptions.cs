using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200031C RID: 796
	public class ParallelOptions
	{
		// Token: 0x060021DD RID: 8669 RVA: 0x0007937A File Offset: 0x0007757A
		public ParallelOptions()
		{
			this._scheduler = TaskScheduler.Default;
			this._maxDegreeOfParallelism = -1;
			this._cancellationToken = CancellationToken.None;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x0007939F File Offset: 0x0007759F
		// (set) Token: 0x060021DF RID: 8671 RVA: 0x000793A7 File Offset: 0x000775A7
		public TaskScheduler TaskScheduler
		{
			get
			{
				return this._scheduler;
			}
			set
			{
				this._scheduler = value;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x000793B0 File Offset: 0x000775B0
		internal TaskScheduler EffectiveTaskScheduler
		{
			get
			{
				if (this._scheduler == null)
				{
					return TaskScheduler.Current;
				}
				return this._scheduler;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000793C6 File Offset: 0x000775C6
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x000793CE File Offset: 0x000775CE
		public int MaxDegreeOfParallelism
		{
			get
			{
				return this._maxDegreeOfParallelism;
			}
			set
			{
				if (value == 0 || value < -1)
				{
					throw new ArgumentOutOfRangeException("MaxDegreeOfParallelism");
				}
				this._maxDegreeOfParallelism = value;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000793E9 File Offset: 0x000775E9
		// (set) Token: 0x060021E4 RID: 8676 RVA: 0x000793F1 File Offset: 0x000775F1
		public CancellationToken CancellationToken
		{
			get
			{
				return this._cancellationToken;
			}
			set
			{
				this._cancellationToken = value;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x000793FC File Offset: 0x000775FC
		internal int EffectiveMaxConcurrencyLevel
		{
			get
			{
				int num = this.MaxDegreeOfParallelism;
				int maximumConcurrencyLevel = this.EffectiveTaskScheduler.MaximumConcurrencyLevel;
				if (maximumConcurrencyLevel > 0 && maximumConcurrencyLevel != 2147483647)
				{
					num = ((num == -1) ? maximumConcurrencyLevel : Math.Min(maximumConcurrencyLevel, num));
				}
				return num;
			}
		}

		// Token: 0x04001BE9 RID: 7145
		private TaskScheduler _scheduler;

		// Token: 0x04001BEA RID: 7146
		private int _maxDegreeOfParallelism;

		// Token: 0x04001BEB RID: 7147
		private CancellationToken _cancellationToken;
	}
}
