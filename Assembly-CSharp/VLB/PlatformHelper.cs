using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000035 RID: 53
	public class PlatformHelper
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00006E07 File Offset: 0x00005007
		public static string GetCurrentPlatformSuffix()
		{
			return PlatformHelper.GetPlatformSuffix(Application.platform);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006E13 File Offset: 0x00005013
		private static string GetPlatformSuffix(RuntimePlatform platform)
		{
			return platform.ToString();
		}
	}
}
