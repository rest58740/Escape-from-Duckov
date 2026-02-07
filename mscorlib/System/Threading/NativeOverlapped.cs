using System;

namespace System.Threading
{
	// Token: 0x020002A3 RID: 675
	public struct NativeOverlapped
	{
		// Token: 0x04001A68 RID: 6760
		public IntPtr InternalLow;

		// Token: 0x04001A69 RID: 6761
		public IntPtr InternalHigh;

		// Token: 0x04001A6A RID: 6762
		public int OffsetLow;

		// Token: 0x04001A6B RID: 6763
		public int OffsetHigh;

		// Token: 0x04001A6C RID: 6764
		public IntPtr EventHandle;
	}
}
