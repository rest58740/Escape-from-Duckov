using System;

namespace System.Security.Permissions
{
	// Token: 0x0200042E RID: 1070
	[Flags]
	public enum ReflectionPermissionFlag
	{
		// Token: 0x04001FEF RID: 8175
		[Obsolete("This permission has been deprecated. Use PermissionState.Unrestricted to get full access.")]
		AllFlags = 7,
		// Token: 0x04001FF0 RID: 8176
		MemberAccess = 2,
		// Token: 0x04001FF1 RID: 8177
		NoFlags = 0,
		// Token: 0x04001FF2 RID: 8178
		[Obsolete("This permission is no longer used by the CLR.")]
		ReflectionEmit = 4,
		// Token: 0x04001FF3 RID: 8179
		RestrictedMemberAccess = 8,
		// Token: 0x04001FF4 RID: 8180
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		TypeInformation = 1
	}
}
