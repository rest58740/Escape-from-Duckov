using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000479 RID: 1145
	[ComVisible(true)]
	public abstract class AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002E63 RID: 11875
		// (set) Token: 0x06002E64 RID: 11876
		public abstract string Parameters { get; set; }

		// Token: 0x06002E65 RID: 11877
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06002E66 RID: 11878
		public abstract byte[] DecryptKeyExchange(byte[] rgb);
	}
}
