using System;

namespace System.Security.Cryptography
{
	// Token: 0x02000467 RID: 1127
	public interface ICspAsymmetricAlgorithm
	{
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002DCF RID: 11727
		CspKeyContainerInfo CspKeyContainerInfo { get; }

		// Token: 0x06002DD0 RID: 11728
		byte[] ExportCspBlob(bool includePrivateParameters);

		// Token: 0x06002DD1 RID: 11729
		void ImportCspBlob(byte[] rawData);
	}
}
