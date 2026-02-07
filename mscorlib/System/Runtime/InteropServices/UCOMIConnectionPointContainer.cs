using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000757 RID: 1879
	[Obsolete]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b196b284-bab4-101a-b69c-00aa00341d07")]
	[ComImport]
	public interface UCOMIConnectionPointContainer
	{
		// Token: 0x06004258 RID: 16984
		void EnumConnectionPoints(out UCOMIEnumConnectionPoints ppEnum);

		// Token: 0x06004259 RID: 16985
		void FindConnectionPoint(ref Guid riid, out UCOMIConnectionPoint ppCP);
	}
}
