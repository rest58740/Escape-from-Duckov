using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200070E RID: 1806
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibVersionAttribute : Attribute
	{
		// Token: 0x060040B9 RID: 16569 RVA: 0x000E15C1 File Offset: 0x000DF7C1
		public TypeLibVersionAttribute(int major, int minor)
		{
			this._major = major;
			this._minor = minor;
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x000E15D7 File Offset: 0x000DF7D7
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060040BB RID: 16571 RVA: 0x000E15DF File Offset: 0x000DF7DF
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x04002AE7 RID: 10983
		internal int _major;

		// Token: 0x04002AE8 RID: 10984
		internal int _minor;
	}
}
