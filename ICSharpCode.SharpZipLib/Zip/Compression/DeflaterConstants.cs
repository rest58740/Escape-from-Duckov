using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x0200000E RID: 14
	public class DeflaterConstants
	{
		// Token: 0x04000064 RID: 100
		public const bool DEBUGGING = false;

		// Token: 0x04000065 RID: 101
		public const int STORED_BLOCK = 0;

		// Token: 0x04000066 RID: 102
		public const int STATIC_TREES = 1;

		// Token: 0x04000067 RID: 103
		public const int DYN_TREES = 2;

		// Token: 0x04000068 RID: 104
		public const int PRESET_DICT = 32;

		// Token: 0x04000069 RID: 105
		public const int DEFAULT_MEM_LEVEL = 8;

		// Token: 0x0400006A RID: 106
		public const int MAX_MATCH = 258;

		// Token: 0x0400006B RID: 107
		public const int MIN_MATCH = 3;

		// Token: 0x0400006C RID: 108
		public const int MAX_WBITS = 15;

		// Token: 0x0400006D RID: 109
		public const int WSIZE = 32768;

		// Token: 0x0400006E RID: 110
		public const int WMASK = 32767;

		// Token: 0x0400006F RID: 111
		public const int HASH_BITS = 15;

		// Token: 0x04000070 RID: 112
		public const int HASH_SIZE = 32768;

		// Token: 0x04000071 RID: 113
		public const int HASH_MASK = 32767;

		// Token: 0x04000072 RID: 114
		public const int HASH_SHIFT = 5;

		// Token: 0x04000073 RID: 115
		public const int MIN_LOOKAHEAD = 262;

		// Token: 0x04000074 RID: 116
		public const int MAX_DIST = 32506;

		// Token: 0x04000075 RID: 117
		public const int PENDING_BUF_SIZE = 65536;

		// Token: 0x04000076 RID: 118
		public const int DEFLATE_STORED = 0;

		// Token: 0x04000077 RID: 119
		public const int DEFLATE_FAST = 1;

		// Token: 0x04000078 RID: 120
		public const int DEFLATE_SLOW = 2;

		// Token: 0x04000079 RID: 121
		public static int MAX_BLOCK_SIZE = Math.Min(65535, 65531);

		// Token: 0x0400007A RID: 122
		public static int[] GOOD_LENGTH = new int[]
		{
			0,
			4,
			4,
			4,
			4,
			8,
			8,
			8,
			32,
			32
		};

		// Token: 0x0400007B RID: 123
		public static int[] MAX_LAZY = new int[]
		{
			0,
			4,
			5,
			6,
			4,
			16,
			16,
			32,
			128,
			258
		};

		// Token: 0x0400007C RID: 124
		public static int[] NICE_LENGTH = new int[]
		{
			0,
			8,
			16,
			32,
			16,
			32,
			128,
			128,
			258,
			258
		};

		// Token: 0x0400007D RID: 125
		public static int[] MAX_CHAIN = new int[]
		{
			0,
			4,
			8,
			32,
			16,
			32,
			128,
			256,
			1024,
			4096
		};

		// Token: 0x0400007E RID: 126
		public static int[] COMPR_FUNC = new int[]
		{
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			2
		};
	}
}
