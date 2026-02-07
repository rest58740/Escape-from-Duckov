using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200049D RID: 1181
	[ComVisible(true)]
	public abstract class MD5 : HashAlgorithm
	{
		// Token: 0x06002F44 RID: 12100 RVA: 0x0000F94D File Offset: 0x0000DB4D
		protected MD5()
		{
			this.HashSizeValue = 128;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000A8620 File Offset: 0x000A6820
		public new static MD5 Create()
		{
			return MD5.Create("System.Security.Cryptography.MD5");
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000A862C File Offset: 0x000A682C
		public new static MD5 Create(string algName)
		{
			return (MD5)CryptoConfig.CreateFromName(algName);
		}
	}
}
