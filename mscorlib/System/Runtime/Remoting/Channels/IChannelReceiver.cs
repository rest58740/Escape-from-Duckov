using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B4 RID: 1460
	[ComVisible(true)]
	public interface IChannelReceiver : IChannel
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06003875 RID: 14453
		object ChannelData { get; }

		// Token: 0x06003876 RID: 14454
		string[] GetUrlsForUri(string objectURI);

		// Token: 0x06003877 RID: 14455
		void StartListening(object data);

		// Token: 0x06003878 RID: 14456
		void StopListening(object data);
	}
}
