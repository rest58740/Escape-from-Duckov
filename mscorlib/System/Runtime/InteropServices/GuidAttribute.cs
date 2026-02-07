using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006FF RID: 1791
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class GuidAttribute : Attribute
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x000E11BE File Offset: 0x000DF3BE
		public GuidAttribute(string guid)
		{
			this._val = guid;
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600408F RID: 16527 RVA: 0x000E11CD File Offset: 0x000DF3CD
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002AC7 RID: 10951
		internal string _val;
	}
}
