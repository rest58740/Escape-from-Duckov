using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007CE RID: 1998
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020402-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeLib
	{
		// Token: 0x06004590 RID: 17808
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x06004591 RID: 17809
		void GetTypeInfo(int index, out ITypeInfo ppTI);

		// Token: 0x06004592 RID: 17810
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x06004593 RID: 17811
		void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

		// Token: 0x06004594 RID: 17812
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x06004595 RID: 17813
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x06004596 RID: 17814
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06004597 RID: 17815
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x06004598 RID: 17816
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x06004599 RID: 17817
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);
	}
}
