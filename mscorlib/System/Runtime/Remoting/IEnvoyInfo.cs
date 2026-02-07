using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x0200055D RID: 1373
	[ComVisible(true)]
	public interface IEnvoyInfo
	{
		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060035E2 RID: 13794
		// (set) Token: 0x060035E3 RID: 13795
		IMessageSink EnvoySinks { get; set; }
	}
}
