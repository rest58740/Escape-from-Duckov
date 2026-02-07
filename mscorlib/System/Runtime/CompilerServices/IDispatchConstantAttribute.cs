using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000835 RID: 2101
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class IDispatchConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x000E7132 File Offset: 0x000E5332
		public override object Value
		{
			get
			{
				return new DispatchWrapper(null);
			}
		}
	}
}
