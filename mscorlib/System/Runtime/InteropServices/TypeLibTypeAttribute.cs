using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F9 RID: 1785
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibTypeAttribute : Attribute
	{
		// Token: 0x06004082 RID: 16514 RVA: 0x000E1151 File Offset: 0x000DF351
		public TypeLibTypeAttribute(TypeLibTypeFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x000E1151 File Offset: 0x000DF351
		public TypeLibTypeAttribute(short flags)
		{
			this._val = (TypeLibTypeFlags)flags;
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x000E1160 File Offset: 0x000DF360
		public TypeLibTypeFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A70 RID: 10864
		internal TypeLibTypeFlags _val;
	}
}
