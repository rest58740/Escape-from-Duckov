using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x0200055C RID: 1372
	[ComVisible(true)]
	public interface IChannelInfo
	{
		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060035E0 RID: 13792
		// (set) Token: 0x060035E1 RID: 13793
		object[] ChannelData { get; set; }
	}
}
