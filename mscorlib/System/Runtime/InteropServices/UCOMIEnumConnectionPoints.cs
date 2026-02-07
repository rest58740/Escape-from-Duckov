using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000758 RID: 1880
	[Guid("b196b285-bab4-101a-b69c-00aa00341d07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete]
	[ComImport]
	public interface UCOMIEnumConnectionPoints
	{
		// Token: 0x0600425A RID: 16986
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIConnectionPoint[] rgelt, out int pceltFetched);

		// Token: 0x0600425B RID: 16987
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600425C RID: 16988
		[PreserveSig]
		int Reset();

		// Token: 0x0600425D RID: 16989
		void Clone(out UCOMIEnumConnectionPoints ppenum);
	}
}
