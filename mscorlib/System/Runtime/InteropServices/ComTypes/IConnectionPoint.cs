using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200079C RID: 1948
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPoint
	{
		// Token: 0x060044F4 RID: 17652
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x060044F5 RID: 17653
		void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);

		// Token: 0x060044F6 RID: 17654
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		// Token: 0x060044F7 RID: 17655
		void Unadvise(int dwCookie);

		// Token: 0x060044F8 RID: 17656
		void EnumConnections(out IEnumConnections ppEnum);
	}
}
