using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006EB RID: 1771
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	public sealed class ComVisibleAttribute : Attribute
	{
		// Token: 0x0600406C RID: 16492 RVA: 0x000E0FBF File Offset: 0x000DF1BF
		public ComVisibleAttribute(bool visibility)
		{
			this._val = visibility;
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600406D RID: 16493 RVA: 0x000E0FCE File Offset: 0x000DF1CE
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A3A RID: 10810
		internal bool _val;
	}
}
