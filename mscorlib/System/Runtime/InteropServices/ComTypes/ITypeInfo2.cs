using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007CA RID: 1994
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020412-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeInfo2 : ITypeInfo
	{
		// Token: 0x0600456E RID: 17774
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x0600456F RID: 17775
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x06004570 RID: 17776
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x06004571 RID: 17777
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x06004572 RID: 17778
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x06004573 RID: 17779
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x06004574 RID: 17780
		void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);

		// Token: 0x06004575 RID: 17781
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x06004576 RID: 17782
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);

		// Token: 0x06004577 RID: 17783
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06004578 RID: 17784
		void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);

		// Token: 0x06004579 RID: 17785
		void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);

		// Token: 0x0600457A RID: 17786
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x0600457B RID: 17787
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x0600457C RID: 17788
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x0600457D RID: 17789
		void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);

		// Token: 0x0600457E RID: 17790
		[PreserveSig]
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x0600457F RID: 17791
		[PreserveSig]
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x06004580 RID: 17792
		[PreserveSig]
		void ReleaseVarDesc(IntPtr pVarDesc);

		// Token: 0x06004581 RID: 17793
		void GetTypeKind(out TYPEKIND pTypeKind);

		// Token: 0x06004582 RID: 17794
		void GetTypeFlags(out int pTypeFlags);

		// Token: 0x06004583 RID: 17795
		void GetFuncIndexOfMemId(int memid, INVOKEKIND invKind, out int pFuncIndex);

		// Token: 0x06004584 RID: 17796
		void GetVarIndexOfMemId(int memid, out int pVarIndex);

		// Token: 0x06004585 RID: 17797
		void GetCustData(ref Guid guid, out object pVarVal);

		// Token: 0x06004586 RID: 17798
		void GetFuncCustData(int index, ref Guid guid, out object pVarVal);

		// Token: 0x06004587 RID: 17799
		void GetParamCustData(int indexFunc, int indexParam, ref Guid guid, out object pVarVal);

		// Token: 0x06004588 RID: 17800
		void GetVarCustData(int index, ref Guid guid, out object pVarVal);

		// Token: 0x06004589 RID: 17801
		void GetImplTypeCustData(int index, ref Guid guid, out object pVarVal);

		// Token: 0x0600458A RID: 17802
		[LCIDConversion(1)]
		void GetDocumentation2(int memid, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

		// Token: 0x0600458B RID: 17803
		void GetAllCustData(IntPtr pCustData);

		// Token: 0x0600458C RID: 17804
		void GetAllFuncCustData(int index, IntPtr pCustData);

		// Token: 0x0600458D RID: 17805
		void GetAllParamCustData(int indexFunc, int indexParam, IntPtr pCustData);

		// Token: 0x0600458E RID: 17806
		void GetAllVarCustData(int index, IntPtr pCustData);

		// Token: 0x0600458F RID: 17807
		void GetAllImplTypeCustData(int index, IntPtr pCustData);
	}
}
