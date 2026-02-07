using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x0200067D RID: 1661
	[SecurityCritical]
	[ComVisible(true)]
	public sealed class InternalRM
	{
		// Token: 0x06003DD6 RID: 15830 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("_LOGGING")]
		public static void InfoSoap(params object[] messages)
		{
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x000D59BC File Offset: 0x000D3BBC
		public static bool SoapCheckEnabled()
		{
			return BCLDebug.CheckEnabled("SOAP");
		}
	}
}
