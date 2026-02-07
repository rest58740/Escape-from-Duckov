using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C8 RID: 1480
	[ComVisible(true)]
	[Serializable]
	public enum ServerProcessing
	{
		// Token: 0x040025EB RID: 9707
		Complete,
		// Token: 0x040025EC RID: 9708
		OneWay,
		// Token: 0x040025ED RID: 9709
		Async
	}
}
