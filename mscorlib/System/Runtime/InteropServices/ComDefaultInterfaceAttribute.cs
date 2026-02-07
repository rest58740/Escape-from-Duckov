using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E8 RID: 1768
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComDefaultInterfaceAttribute : Attribute
	{
		// Token: 0x06004067 RID: 16487 RVA: 0x000E0F91 File Offset: 0x000DF191
		public ComDefaultInterfaceAttribute(Type defaultInterface)
		{
			this._val = defaultInterface;
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06004068 RID: 16488 RVA: 0x000E0FA0 File Offset: 0x000DF1A0
		public Type Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A34 RID: 10804
		internal Type _val;
	}
}
