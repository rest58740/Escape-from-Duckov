using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017F RID: 383
	public struct MatchMakingKeyValuePair_t
	{
		// Token: 0x060008CD RID: 2253 RVA: 0x0000CA18 File Offset: 0x0000AC18
		private MatchMakingKeyValuePair_t(string strKey, string strValue)
		{
			this.m_szKey = strKey;
			this.m_szValue = strValue;
		}

		// Token: 0x04000A3C RID: 2620
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szKey;

		// Token: 0x04000A3D RID: 2621
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szValue;
	}
}
