using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x020005DB RID: 1499
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum SoapOption
	{
		// Token: 0x0400260C RID: 9740
		None = 0,
		// Token: 0x0400260D RID: 9741
		AlwaysIncludeTypes = 1,
		// Token: 0x0400260E RID: 9742
		XsdString = 2,
		// Token: 0x0400260F RID: 9743
		EmbedAll = 4,
		// Token: 0x04002610 RID: 9744
		Option1 = 8,
		// Token: 0x04002611 RID: 9745
		Option2 = 16
	}
}
