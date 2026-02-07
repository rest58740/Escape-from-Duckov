using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006FB RID: 1787
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibVarAttribute : Attribute
	{
		// Token: 0x06004088 RID: 16520 RVA: 0x000E117F File Offset: 0x000DF37F
		public TypeLibVarAttribute(TypeLibVarFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x000E117F File Offset: 0x000DF37F
		public TypeLibVarAttribute(short flags)
		{
			this._val = (TypeLibVarFlags)flags;
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600408A RID: 16522 RVA: 0x000E118E File Offset: 0x000DF38E
		public TypeLibVarFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A72 RID: 10866
		internal TypeLibVarFlags _val;
	}
}
