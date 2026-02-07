using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200070B RID: 1803
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
	[ComVisible(true)]
	public sealed class PrimaryInteropAssemblyAttribute : Attribute
	{
		// Token: 0x060040B1 RID: 16561 RVA: 0x000E155E File Offset: 0x000DF75E
		public PrimaryInteropAssemblyAttribute(int major, int minor)
		{
			this._major = major;
			this._minor = minor;
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x000E1574 File Offset: 0x000DF774
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x060040B3 RID: 16563 RVA: 0x000E157C File Offset: 0x000DF77C
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x04002AE2 RID: 10978
		internal int _major;

		// Token: 0x04002AE3 RID: 10979
		internal int _minor;
	}
}
