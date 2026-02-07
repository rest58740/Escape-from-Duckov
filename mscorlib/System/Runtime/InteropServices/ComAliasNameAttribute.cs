using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000709 RID: 1801
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	public sealed class ComAliasNameAttribute : Attribute
	{
		// Token: 0x060040AD RID: 16557 RVA: 0x000E1530 File Offset: 0x000DF730
		public ComAliasNameAttribute(string alias)
		{
			this._val = alias;
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060040AE RID: 16558 RVA: 0x000E153F File Offset: 0x000DF73F
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002AE0 RID: 10976
		internal string _val;
	}
}
