using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200070A RID: 1802
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class AutomationProxyAttribute : Attribute
	{
		// Token: 0x060040AF RID: 16559 RVA: 0x000E1547 File Offset: 0x000DF747
		public AutomationProxyAttribute(bool val)
		{
			this._val = val;
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x000E1556 File Offset: 0x000DF756
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002AE1 RID: 10977
		internal bool _val;
	}
}
