using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200043C RID: 1084
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class GacIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002C0B RID: 11275 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public GacIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x0009A092 File Offset: 0x00098292
		public override IPermission CreatePermission()
		{
			return new GacIdentityPermission();
		}
	}
}
