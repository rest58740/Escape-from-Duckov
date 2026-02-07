using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200047A RID: 1146
	[ComVisible(true)]
	public abstract class AsymmetricKeyExchangeFormatter
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002E68 RID: 11880
		public abstract string Parameters { get; }

		// Token: 0x06002E69 RID: 11881
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06002E6A RID: 11882
		public abstract byte[] CreateKeyExchange(byte[] data);

		// Token: 0x06002E6B RID: 11883
		public abstract byte[] CreateKeyExchange(byte[] data, Type symAlgType);
	}
}
