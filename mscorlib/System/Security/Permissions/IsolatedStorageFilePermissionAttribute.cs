using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000442 RID: 1090
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class IsolatedStorageFilePermissionAttribute : IsolatedStoragePermissionAttribute
	{
		// Token: 0x06002C3A RID: 11322 RVA: 0x0009F394 File Offset: 0x0009D594
		public IsolatedStorageFilePermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x0009F3A0 File Offset: 0x0009D5A0
		public override IPermission CreatePermission()
		{
			IsolatedStorageFilePermission isolatedStorageFilePermission;
			if (base.Unrestricted)
			{
				isolatedStorageFilePermission = new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			}
			else
			{
				isolatedStorageFilePermission = new IsolatedStorageFilePermission(PermissionState.None);
				isolatedStorageFilePermission.UsageAllowed = base.UsageAllowed;
				isolatedStorageFilePermission.UserQuota = base.UserQuota;
			}
			return isolatedStorageFilePermission;
		}
	}
}
