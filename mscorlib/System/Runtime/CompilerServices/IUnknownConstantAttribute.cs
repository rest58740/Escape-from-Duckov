using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000842 RID: 2114
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class IUnknownConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060046BE RID: 18110 RVA: 0x000E7169 File Offset: 0x000E5369
		public override object Value
		{
			get
			{
				return new UnknownWrapper(null);
			}
		}
	}
}
