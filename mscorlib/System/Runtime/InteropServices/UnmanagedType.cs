using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006FD RID: 1789
	[ComVisible(true)]
	[Serializable]
	public enum UnmanagedType
	{
		// Token: 0x04002AA1 RID: 10913
		Bool = 2,
		// Token: 0x04002AA2 RID: 10914
		I1,
		// Token: 0x04002AA3 RID: 10915
		U1,
		// Token: 0x04002AA4 RID: 10916
		I2,
		// Token: 0x04002AA5 RID: 10917
		U2,
		// Token: 0x04002AA6 RID: 10918
		I4,
		// Token: 0x04002AA7 RID: 10919
		U4,
		// Token: 0x04002AA8 RID: 10920
		I8,
		// Token: 0x04002AA9 RID: 10921
		U8,
		// Token: 0x04002AAA RID: 10922
		R4,
		// Token: 0x04002AAB RID: 10923
		R8,
		// Token: 0x04002AAC RID: 10924
		Currency = 15,
		// Token: 0x04002AAD RID: 10925
		BStr = 19,
		// Token: 0x04002AAE RID: 10926
		LPStr,
		// Token: 0x04002AAF RID: 10927
		LPWStr,
		// Token: 0x04002AB0 RID: 10928
		LPTStr,
		// Token: 0x04002AB1 RID: 10929
		ByValTStr,
		// Token: 0x04002AB2 RID: 10930
		IUnknown = 25,
		// Token: 0x04002AB3 RID: 10931
		IDispatch,
		// Token: 0x04002AB4 RID: 10932
		Struct,
		// Token: 0x04002AB5 RID: 10933
		Interface,
		// Token: 0x04002AB6 RID: 10934
		SafeArray,
		// Token: 0x04002AB7 RID: 10935
		ByValArray,
		// Token: 0x04002AB8 RID: 10936
		SysInt,
		// Token: 0x04002AB9 RID: 10937
		SysUInt,
		// Token: 0x04002ABA RID: 10938
		VBByRefStr = 34,
		// Token: 0x04002ABB RID: 10939
		AnsiBStr,
		// Token: 0x04002ABC RID: 10940
		TBStr,
		// Token: 0x04002ABD RID: 10941
		VariantBool,
		// Token: 0x04002ABE RID: 10942
		FunctionPtr,
		// Token: 0x04002ABF RID: 10943
		AsAny = 40,
		// Token: 0x04002AC0 RID: 10944
		LPArray = 42,
		// Token: 0x04002AC1 RID: 10945
		LPStruct,
		// Token: 0x04002AC2 RID: 10946
		CustomMarshaler,
		// Token: 0x04002AC3 RID: 10947
		Error,
		// Token: 0x04002AC4 RID: 10948
		[ComVisible(false)]
		IInspectable,
		// Token: 0x04002AC5 RID: 10949
		[ComVisible(false)]
		HString,
		// Token: 0x04002AC6 RID: 10950
		[ComVisible(false)]
		LPUTF8Str
	}
}
