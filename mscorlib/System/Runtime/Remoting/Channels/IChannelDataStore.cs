using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B3 RID: 1459
	[ComVisible(true)]
	public interface IChannelDataStore
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06003872 RID: 14450
		string[] ChannelUris { get; }

		// Token: 0x170007FE RID: 2046
		object this[object key]
		{
			get;
			set;
		}
	}
}
