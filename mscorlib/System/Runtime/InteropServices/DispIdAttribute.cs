using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E5 RID: 1765
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited = false)]
	[ComVisible(true)]
	public sealed class DispIdAttribute : Attribute
	{
		// Token: 0x06004062 RID: 16482 RVA: 0x000E0F63 File Offset: 0x000DF163
		public DispIdAttribute(int dispId)
		{
			this._val = dispId;
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06004063 RID: 16483 RVA: 0x000E0F72 File Offset: 0x000DF172
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A2D RID: 10797
		internal int _val;
	}
}
