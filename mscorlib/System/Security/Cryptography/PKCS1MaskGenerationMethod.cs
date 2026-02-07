using System;
using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography
{
	// Token: 0x0200049F RID: 1183
	[ComVisible(true)]
	public class PKCS1MaskGenerationMethod : MaskGenerationMethod
	{
		// Token: 0x06002F5C RID: 12124 RVA: 0x000A8C57 File Offset: 0x000A6E57
		public PKCS1MaskGenerationMethod()
		{
			this.HashNameValue = "SHA1";
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000A8C6A File Offset: 0x000A6E6A
		// (set) Token: 0x06002F5E RID: 12126 RVA: 0x000A8C72 File Offset: 0x000A6E72
		public string HashName
		{
			get
			{
				return this.HashNameValue;
			}
			set
			{
				this.HashNameValue = value;
				if (this.HashNameValue == null)
				{
					this.HashNameValue = "SHA1";
				}
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000A8C8E File Offset: 0x000A6E8E
		public override byte[] GenerateMask(byte[] rgbSeed, int cbReturn)
		{
			return PKCS1.MGF1(HashAlgorithm.Create(this.HashNameValue), rgbSeed, cbReturn);
		}

		// Token: 0x04002191 RID: 8593
		private string HashNameValue;
	}
}
