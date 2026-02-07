using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006ED RID: 1773
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class LCIDConversionAttribute : Attribute
	{
		// Token: 0x06004070 RID: 16496 RVA: 0x000E0FF2 File Offset: 0x000DF1F2
		public LCIDConversionAttribute(int lcid)
		{
			this._val = lcid;
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x000E1001 File Offset: 0x000DF201
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A3C RID: 10812
		internal int _val;
	}
}
