using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D7 RID: 1751
	public interface ICustomAdapter
	{
		// Token: 0x06004032 RID: 16434
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetUnderlyingObject();
	}
}
