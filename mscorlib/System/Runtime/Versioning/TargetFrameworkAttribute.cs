using System;

namespace System.Runtime.Versioning
{
	// Token: 0x0200063B RID: 1595
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class TargetFrameworkAttribute : Attribute
	{
		// Token: 0x06003C22 RID: 15394 RVA: 0x000D10B0 File Offset: 0x000CF2B0
		public TargetFrameworkAttribute(string frameworkName)
		{
			if (frameworkName == null)
			{
				throw new ArgumentNullException("frameworkName");
			}
			this._frameworkName = frameworkName;
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003C23 RID: 15395 RVA: 0x000D10CD File Offset: 0x000CF2CD
		public string FrameworkName
		{
			get
			{
				return this._frameworkName;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x000D10D5 File Offset: 0x000CF2D5
		// (set) Token: 0x06003C25 RID: 15397 RVA: 0x000D10DD File Offset: 0x000CF2DD
		public string FrameworkDisplayName
		{
			get
			{
				return this._frameworkDisplayName;
			}
			set
			{
				this._frameworkDisplayName = value;
			}
		}

		// Token: 0x040026EE RID: 9966
		private string _frameworkName;

		// Token: 0x040026EF RID: 9967
		private string _frameworkDisplayName;
	}
}
