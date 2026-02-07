using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C4 RID: 1476
	[ComVisible(true)]
	public interface ITransportHeaders
	{
		// Token: 0x17000809 RID: 2057
		object this[object key]
		{
			get;
			set;
		}

		// Token: 0x0600389F RID: 14495
		IEnumerator GetEnumerator();
	}
}
