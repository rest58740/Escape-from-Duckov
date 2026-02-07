using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000736 RID: 1846
	[Guid("00020401-0000-0000-C000-000000000046")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ITypeInfo instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMITypeInfo
	{
		// Token: 0x06004101 RID: 16641
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x06004102 RID: 16642
		void GetTypeComp(out UCOMITypeComp ppTComp);

		// Token: 0x06004103 RID: 16643
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x06004104 RID: 16644
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x06004105 RID: 16645
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x06004106 RID: 16646
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x06004107 RID: 16647
		void GetImplTypeFlags(int index, out int pImplTypeFlags);

		// Token: 0x06004108 RID: 16648
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x06004109 RID: 16649
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, out object pVarResult, out EXCEPINFO pExcepInfo, out int puArgErr);

		// Token: 0x0600410A RID: 16650
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x0600410B RID: 16651
		void GetDllEntry(int memid, INVOKEKIND invKind, out string pBstrDllName, out string pBstrName, out short pwOrdinal);

		// Token: 0x0600410C RID: 16652
		void GetRefTypeInfo(int hRef, out UCOMITypeInfo ppTI);

		// Token: 0x0600410D RID: 16653
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x0600410E RID: 16654
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x0600410F RID: 16655
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x06004110 RID: 16656
		void GetContainingTypeLib(out UCOMITypeLib ppTLB, out int pIndex);

		// Token: 0x06004111 RID: 16657
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x06004112 RID: 16658
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x06004113 RID: 16659
		void ReleaseVarDesc(IntPtr pVarDesc);
	}
}
