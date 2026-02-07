using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007CF RID: 1999
	[Guid("00020411-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ITypeLib2 : ITypeLib
	{
		// Token: 0x0600459A RID: 17818
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x0600459B RID: 17819
		void GetTypeInfo(int index, out ITypeInfo ppTI);

		// Token: 0x0600459C RID: 17820
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x0600459D RID: 17821
		void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

		// Token: 0x0600459E RID: 17822
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x0600459F RID: 17823
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x060045A0 RID: 17824
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x060045A1 RID: 17825
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x060045A2 RID: 17826
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x060045A3 RID: 17827
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);

		// Token: 0x060045A4 RID: 17828
		void GetCustData(ref Guid guid, out object pVarVal);

		// Token: 0x060045A5 RID: 17829
		[LCIDConversion(1)]
		void GetDocumentation2(int index, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

		// Token: 0x060045A6 RID: 17830
		void GetLibStatistics(IntPtr pcUniqueNames, out int pcchUniqueNames);

		// Token: 0x060045A7 RID: 17831
		void GetAllCustData(IntPtr pCustData);
	}
}
