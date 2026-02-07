using System;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000718 RID: 1816
	[ComVisible(false)]
	public interface ICustomQueryInterface
	{
		// Token: 0x060040D7 RID: 16599
		[SecurityCritical]
		CustomQueryInterfaceResult GetInterface([In] ref Guid iid, out IntPtr ppv);
	}
}
