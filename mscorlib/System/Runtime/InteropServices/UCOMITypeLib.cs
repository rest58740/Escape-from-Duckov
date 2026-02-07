using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000761 RID: 1889
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020402-0000-0000-c000-000000000046")]
	[Obsolete]
	[ComImport]
	public interface UCOMITypeLib
	{
		// Token: 0x06004298 RID: 17048
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x06004299 RID: 17049
		void GetTypeInfo(int index, out UCOMITypeInfo ppTI);

		// Token: 0x0600429A RID: 17050
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x0600429B RID: 17051
		void GetTypeInfoOfGuid(ref Guid guid, out UCOMITypeInfo ppTInfo);

		// Token: 0x0600429C RID: 17052
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x0600429D RID: 17053
		void GetTypeComp(out UCOMITypeComp ppTComp);

		// Token: 0x0600429E RID: 17054
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x0600429F RID: 17055
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x060042A0 RID: 17056
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UCOMITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x060042A1 RID: 17057
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);
	}
}
