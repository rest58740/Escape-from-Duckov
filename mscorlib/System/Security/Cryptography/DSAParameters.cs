using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200048A RID: 1162
	[ComVisible(true)]
	[Serializable]
	public struct DSAParameters
	{
		// Token: 0x04002159 RID: 8537
		public byte[] P;

		// Token: 0x0400215A RID: 8538
		public byte[] Q;

		// Token: 0x0400215B RID: 8539
		public byte[] G;

		// Token: 0x0400215C RID: 8540
		public byte[] Y;

		// Token: 0x0400215D RID: 8541
		public byte[] J;

		// Token: 0x0400215E RID: 8542
		[NonSerialized]
		public byte[] X;

		// Token: 0x0400215F RID: 8543
		public byte[] Seed;

		// Token: 0x04002160 RID: 8544
		public int Counter;
	}
}
