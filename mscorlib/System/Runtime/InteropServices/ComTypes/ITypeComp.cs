using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B2 RID: 1970
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020403-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeComp
	{
		// Token: 0x06004559 RID: 17753
		void Bind([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, short wFlags, out ITypeInfo ppTInfo, out DESCKIND pDescKind, out BINDPTR pBindPtr);

		// Token: 0x0600455A RID: 17754
		void BindType([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, out ITypeInfo ppTInfo, out ITypeComp ppTComp);
	}
}
