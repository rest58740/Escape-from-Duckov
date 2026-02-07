using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E7 RID: 1767
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class InterfaceTypeAttribute : Attribute
	{
		// Token: 0x06004064 RID: 16484 RVA: 0x000E0F7A File Offset: 0x000DF17A
		public InterfaceTypeAttribute(ComInterfaceType interfaceType)
		{
			this._val = interfaceType;
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x000E0F7A File Offset: 0x000DF17A
		public InterfaceTypeAttribute(short interfaceType)
		{
			this._val = (ComInterfaceType)interfaceType;
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x000E0F89 File Offset: 0x000DF189
		public ComInterfaceType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A33 RID: 10803
		internal ComInterfaceType _val;
	}
}
