using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000760 RID: 1888
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020403-0000-0000-c000-000000000046")]
	[Obsolete]
	[ComImport]
	public interface UCOMITypeComp
	{
		// Token: 0x06004296 RID: 17046
		void Bind([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, short wFlags, out UCOMITypeInfo ppTInfo, out DESCKIND pDescKind, out BINDPTR pBindPtr);

		// Token: 0x06004297 RID: 17047
		void BindType([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, out UCOMITypeInfo ppTInfo, out UCOMITypeComp ppTComp);
	}
}
