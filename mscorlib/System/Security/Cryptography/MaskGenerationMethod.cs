using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200049C RID: 1180
	[ComVisible(true)]
	public abstract class MaskGenerationMethod
	{
		// Token: 0x06002F42 RID: 12098
		[ComVisible(true)]
		public abstract byte[] GenerateMask(byte[] rgbSeed, int cbReturn);
	}
}
