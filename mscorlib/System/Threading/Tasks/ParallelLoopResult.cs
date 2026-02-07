using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200032E RID: 814
	public struct ParallelLoopResult
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x0007BD43 File Offset: 0x00079F43
		public bool IsCompleted
		{
			get
			{
				return this._completed;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x0007BD4B File Offset: 0x00079F4B
		public long? LowestBreakIteration
		{
			get
			{
				return this._lowestBreakIteration;
			}
		}

		// Token: 0x04001C40 RID: 7232
		internal bool _completed;

		// Token: 0x04001C41 RID: 7233
		internal long? _lowestBreakIteration;
	}
}
