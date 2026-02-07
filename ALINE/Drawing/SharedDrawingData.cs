using System;
using Unity.Burst;

namespace Drawing
{
	// Token: 0x02000028 RID: 40
	public static class SharedDrawingData
	{
		// Token: 0x0400007C RID: 124
		public static readonly SharedStatic<float> BurstTime = SharedStatic<float>.GetOrCreateUnsafe(4U, 527447541831459905L, -5918529866343830416L);

		// Token: 0x02000029 RID: 41
		private class BurstTimeKey
		{
		}
	}
}
