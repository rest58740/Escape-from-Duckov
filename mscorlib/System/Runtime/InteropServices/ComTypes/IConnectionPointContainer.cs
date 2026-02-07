using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200079D RID: 1949
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPointContainer
	{
		// Token: 0x060044F9 RID: 17657
		void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);

		// Token: 0x060044FA RID: 17658
		void FindConnectionPoint([In] ref Guid riid, out IConnectionPoint ppCP);
	}
}
