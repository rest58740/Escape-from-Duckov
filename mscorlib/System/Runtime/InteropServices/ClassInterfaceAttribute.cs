using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006EA RID: 1770
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ClassInterfaceAttribute : Attribute
	{
		// Token: 0x06004069 RID: 16489 RVA: 0x000E0FA8 File Offset: 0x000DF1A8
		public ClassInterfaceAttribute(ClassInterfaceType classInterfaceType)
		{
			this._val = classInterfaceType;
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x000E0FA8 File Offset: 0x000DF1A8
		public ClassInterfaceAttribute(short classInterfaceType)
		{
			this._val = (ClassInterfaceType)classInterfaceType;
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x000E0FB7 File Offset: 0x000DF1B7
		public ClassInterfaceType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A39 RID: 10809
		internal ClassInterfaceType _val;
	}
}
