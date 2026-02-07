using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000480 RID: 1152
	[ComVisible(true)]
	[Serializable]
	public enum CipherMode
	{
		// Token: 0x04002134 RID: 8500
		CBC = 1,
		// Token: 0x04002135 RID: 8501
		ECB,
		// Token: 0x04002136 RID: 8502
		OFB,
		// Token: 0x04002137 RID: 8503
		CFB,
		// Token: 0x04002138 RID: 8504
		CTS
	}
}
