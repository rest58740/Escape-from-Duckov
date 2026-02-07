using System;
using System.Security;

namespace System.Runtime
{
	// Token: 0x0200054F RID: 1359
	public static class ProfileOptimization
	{
		// Token: 0x060035AF RID: 13743 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal static void InternalSetProfileRoot(string directoryPath)
		{
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal static void InternalStartProfile(string profile, IntPtr ptrNativeAssemblyLoadContext)
		{
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000C1FF0 File Offset: 0x000C01F0
		[SecurityCritical]
		public static void SetProfileRoot(string directoryPath)
		{
			ProfileOptimization.InternalSetProfileRoot(directoryPath);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000C1FF8 File Offset: 0x000C01F8
		[SecurityCritical]
		public static void StartProfile(string profile)
		{
			ProfileOptimization.InternalStartProfile(profile, IntPtr.Zero);
		}
	}
}
