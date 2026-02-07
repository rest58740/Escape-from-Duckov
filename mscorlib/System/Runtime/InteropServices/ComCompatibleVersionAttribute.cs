using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200070F RID: 1807
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComCompatibleVersionAttribute : Attribute
	{
		// Token: 0x060040BC RID: 16572 RVA: 0x000E15E7 File Offset: 0x000DF7E7
		public ComCompatibleVersionAttribute(int major, int minor, int build, int revision)
		{
			this._major = major;
			this._minor = minor;
			this._build = build;
			this._revision = revision;
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060040BD RID: 16573 RVA: 0x000E160C File Offset: 0x000DF80C
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x000E1614 File Offset: 0x000DF814
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060040BF RID: 16575 RVA: 0x000E161C File Offset: 0x000DF81C
		public int BuildNumber
		{
			get
			{
				return this._build;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x000E1624 File Offset: 0x000DF824
		public int RevisionNumber
		{
			get
			{
				return this._revision;
			}
		}

		// Token: 0x04002AE9 RID: 10985
		internal int _major;

		// Token: 0x04002AEA RID: 10986
		internal int _minor;

		// Token: 0x04002AEB RID: 10987
		internal int _build;

		// Token: 0x04002AEC RID: 10988
		internal int _revision;
	}
}
