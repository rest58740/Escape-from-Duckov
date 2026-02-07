using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000444 RID: 1092
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class IsolatedStoragePermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002C45 RID: 11333 RVA: 0x0009DD00 File Offset: 0x0009BF00
		protected IsolatedStoragePermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002C46 RID: 11334 RVA: 0x0009F5DC File Offset: 0x0009D7DC
		// (set) Token: 0x06002C47 RID: 11335 RVA: 0x0009F5E4 File Offset: 0x0009D7E4
		public IsolatedStorageContainment UsageAllowed
		{
			get
			{
				return this.usage_allowed;
			}
			set
			{
				this.usage_allowed = value;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002C48 RID: 11336 RVA: 0x0009F5ED File Offset: 0x0009D7ED
		// (set) Token: 0x06002C49 RID: 11337 RVA: 0x0009F5F5 File Offset: 0x0009D7F5
		public long UserQuota
		{
			get
			{
				return this.user_quota;
			}
			set
			{
				this.user_quota = value;
			}
		}

		// Token: 0x04002048 RID: 8264
		private IsolatedStorageContainment usage_allowed;

		// Token: 0x04002049 RID: 8265
		private long user_quota;
	}
}
