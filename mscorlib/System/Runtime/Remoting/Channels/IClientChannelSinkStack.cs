using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005BA RID: 1466
	[ComVisible(true)]
	public interface IClientChannelSinkStack : IClientResponseChannelSinkStack
	{
		// Token: 0x06003887 RID: 14471
		object Pop(IClientChannelSink sink);

		// Token: 0x06003888 RID: 14472
		void Push(IClientChannelSink sink, object state);
	}
}
