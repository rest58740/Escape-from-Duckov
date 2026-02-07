using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200074C RID: 1868
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class MarshalAsAttribute : Attribute
	{
		// Token: 0x06004234 RID: 16948 RVA: 0x000E3A7A File Offset: 0x000E1C7A
		public MarshalAsAttribute(short unmanagedType)
		{
			this.utype = (UnmanagedType)unmanagedType;
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x000E3A7A File Offset: 0x000E1C7A
		public MarshalAsAttribute(UnmanagedType unmanagedType)
		{
			this.utype = unmanagedType;
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x000E3A89 File Offset: 0x000E1C89
		public UnmanagedType Value
		{
			get
			{
				return this.utype;
			}
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x000E3A91 File Offset: 0x000E1C91
		internal MarshalAsAttribute Copy()
		{
			return (MarshalAsAttribute)base.MemberwiseClone();
		}

		// Token: 0x04002BD4 RID: 11220
		public string MarshalCookie;

		// Token: 0x04002BD5 RID: 11221
		[ComVisible(true)]
		public string MarshalType;

		// Token: 0x04002BD6 RID: 11222
		[ComVisible(true)]
		[PreserveDependency("GetCustomMarshalerInstance", "System.Runtime.InteropServices.Marshal")]
		public Type MarshalTypeRef;

		// Token: 0x04002BD7 RID: 11223
		public Type SafeArrayUserDefinedSubType;

		// Token: 0x04002BD8 RID: 11224
		private UnmanagedType utype;

		// Token: 0x04002BD9 RID: 11225
		public UnmanagedType ArraySubType;

		// Token: 0x04002BDA RID: 11226
		public VarEnum SafeArraySubType;

		// Token: 0x04002BDB RID: 11227
		public int SizeConst;

		// Token: 0x04002BDC RID: 11228
		public int IidParameterIndex;

		// Token: 0x04002BDD RID: 11229
		public short SizeParamIndex;
	}
}
