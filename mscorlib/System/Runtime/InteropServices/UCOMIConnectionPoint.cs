using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000756 RID: 1878
	[Obsolete]
	[Guid("b196b286-bab4-101a-b69c-00aa00341d07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIConnectionPoint
	{
		// Token: 0x06004253 RID: 16979
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x06004254 RID: 16980
		void GetConnectionPointContainer(out UCOMIConnectionPointContainer ppCPC);

		// Token: 0x06004255 RID: 16981
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		// Token: 0x06004256 RID: 16982
		void Unadvise(int dwCookie);

		// Token: 0x06004257 RID: 16983
		void EnumConnections(out UCOMIEnumConnections ppEnum);
	}
}
