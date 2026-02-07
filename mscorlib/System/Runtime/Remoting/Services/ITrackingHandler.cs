using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x0200057C RID: 1404
	[ComVisible(true)]
	public interface ITrackingHandler
	{
		// Token: 0x0600370B RID: 14091
		void DisconnectedObject(object obj);

		// Token: 0x0600370C RID: 14092
		void MarshaledObject(object obj, ObjRef or);

		// Token: 0x0600370D RID: 14093
		void UnmarshaledObject(object obj, ObjRef or);
	}
}
