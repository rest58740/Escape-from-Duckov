using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200061B RID: 1563
	[ComVisible(true)]
	public interface IMessage
	{
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06003AF8 RID: 15096
		IDictionary Properties { get; }
	}
}
