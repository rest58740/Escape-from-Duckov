using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007C9 RID: 1993
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020401-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeInfo
	{
		// Token: 0x0600455B RID: 17755
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x0600455C RID: 17756
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x0600455D RID: 17757
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x0600455E RID: 17758
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x0600455F RID: 17759
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x06004560 RID: 17760
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x06004561 RID: 17761
		void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);

		// Token: 0x06004562 RID: 17762
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x06004563 RID: 17763
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);

		// Token: 0x06004564 RID: 17764
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06004565 RID: 17765
		void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);

		// Token: 0x06004566 RID: 17766
		void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);

		// Token: 0x06004567 RID: 17767
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x06004568 RID: 17768
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x06004569 RID: 17769
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x0600456A RID: 17770
		void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);

		// Token: 0x0600456B RID: 17771
		[PreserveSig]
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x0600456C RID: 17772
		[PreserveSig]
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x0600456D RID: 17773
		[PreserveSig]
		void ReleaseVarDesc(IntPtr pVarDesc);
	}
}
