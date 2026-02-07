using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020004D7 RID: 1239
	public enum X509ContentType
	{
		// Token: 0x04002288 RID: 8840
		Unknown,
		// Token: 0x04002289 RID: 8841
		Cert,
		// Token: 0x0400228A RID: 8842
		SerializedCert,
		// Token: 0x0400228B RID: 8843
		Pfx,
		// Token: 0x0400228C RID: 8844
		Pkcs12 = 3,
		// Token: 0x0400228D RID: 8845
		SerializedStore,
		// Token: 0x0400228E RID: 8846
		Pkcs7,
		// Token: 0x0400228F RID: 8847
		Authenticode
	}
}
