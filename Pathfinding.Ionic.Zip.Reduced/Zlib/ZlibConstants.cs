using System;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000068 RID: 104
	public static class ZlibConstants
	{
		// Token: 0x0400038C RID: 908
		public const int WindowBitsMax = 15;

		// Token: 0x0400038D RID: 909
		public const int WindowBitsDefault = 15;

		// Token: 0x0400038E RID: 910
		public const int Z_OK = 0;

		// Token: 0x0400038F RID: 911
		public const int Z_STREAM_END = 1;

		// Token: 0x04000390 RID: 912
		public const int Z_NEED_DICT = 2;

		// Token: 0x04000391 RID: 913
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x04000392 RID: 914
		public const int Z_DATA_ERROR = -3;

		// Token: 0x04000393 RID: 915
		public const int Z_BUF_ERROR = -5;

		// Token: 0x04000394 RID: 916
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x04000395 RID: 917
		public const int WorkingBufferSizeMin = 1024;
	}
}
