using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006FA RID: 1786
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibFuncAttribute : Attribute
	{
		// Token: 0x06004085 RID: 16517 RVA: 0x000E1168 File Offset: 0x000DF368
		public TypeLibFuncAttribute(TypeLibFuncFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x000E1168 File Offset: 0x000DF368
		public TypeLibFuncAttribute(short flags)
		{
			this._val = (TypeLibFuncFlags)flags;
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06004087 RID: 16519 RVA: 0x000E1177 File Offset: 0x000DF377
		public TypeLibFuncFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A71 RID: 10865
		internal TypeLibFuncFlags _val;
	}
}
