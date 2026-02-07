using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020009BB RID: 2491
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DebuggerBrowsableAttribute : Attribute
	{
		// Token: 0x060059A2 RID: 22946 RVA: 0x00133004 File Offset: 0x00131204
		public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
		{
			if (state < DebuggerBrowsableState.Never || state > DebuggerBrowsableState.RootHidden)
			{
				throw new ArgumentOutOfRangeException("state");
			}
			this.state = state;
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x00133026 File Offset: 0x00131226
		public DebuggerBrowsableState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0400377F RID: 14207
		private DebuggerBrowsableState state;
	}
}
