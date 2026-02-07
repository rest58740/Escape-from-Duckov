using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004A9 RID: 1193
	[ComVisible(true)]
	[Serializable]
	public struct RSAParameters
	{
		// Token: 0x040021B8 RID: 8632
		public byte[] Exponent;

		// Token: 0x040021B9 RID: 8633
		public byte[] Modulus;

		// Token: 0x040021BA RID: 8634
		[NonSerialized]
		public byte[] P;

		// Token: 0x040021BB RID: 8635
		[NonSerialized]
		public byte[] Q;

		// Token: 0x040021BC RID: 8636
		[NonSerialized]
		public byte[] DP;

		// Token: 0x040021BD RID: 8637
		[NonSerialized]
		public byte[] DQ;

		// Token: 0x040021BE RID: 8638
		[NonSerialized]
		public byte[] InverseQ;

		// Token: 0x040021BF RID: 8639
		[NonSerialized]
		public byte[] D;
	}
}
