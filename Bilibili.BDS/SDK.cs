using System;
using UnityEngine;

namespace Bilibili.BDS
{
	// Token: 0x02000003 RID: 3
	public static class SDK
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020B8 File Offset: 0x000002B8
		public static int SDKInitBDS(SDK.ENV env = SDK.ENV.cb, string channelID = "None")
		{
			string strENV = env.ToString();
			int result = SDK.InitBDS("14457", channelID, strENV);
			Debug.Log("Initialize Bilibili BDS: " + result.ToString());
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000002F8
		public static int SDKInitBDS(string channelID, SDK.ENV env)
		{
			string strENV = env.ToString();
			return SDK.InitBDS("14457", channelID, strENV);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000211F File Offset: 0x0000031F
		private static int InitBDS(string strGameId, string strChannelId, string strENV)
		{
			return 0;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002122 File Offset: 0x00000322
		public static int ReportCustomEvent(string strEventName, string strPlayerInfo, string strExtension, string strCpParam)
		{
			return 0;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002125 File Offset: 0x00000325
		public static int ReportCustomEventSync(string strEventName, string strPlayerInfo, string strExtension, string strCpParam)
		{
			return 0;
		}

		// Token: 0x04000001 RID: 1
		public const string GameID = "14457";

		// Token: 0x02000006 RID: 6
		public enum ENV
		{
			// Token: 0x0400000A RID: 10
			cb,
			// Token: 0x0400000B RID: 11
			beta,
			// Token: 0x0400000C RID: 12
			preview,
			// Token: 0x0400000D RID: 13
			ob
		}
	}
}
