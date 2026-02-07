using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000720 RID: 1824
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumConnections instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIEnumConnections
	{
		// Token: 0x060040FD RID: 16637
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] CONNECTDATA[] rgelt, out int pceltFetched);

		// Token: 0x060040FE RID: 16638
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060040FF RID: 16639
		[PreserveSig]
		void Reset();

		// Token: 0x06004100 RID: 16640
		void Clone(out UCOMIEnumConnections ppenum);
	}
}
