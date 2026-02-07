using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000245 RID: 581
	public static class PlatformInfo
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x000471BC File Offset: 0x000453BC
		// (set) Token: 0x06001253 RID: 4691 RVA: 0x000471D1 File Offset: 0x000453D1
		public static Platform Platform
		{
			get
			{
				if (Application.isEditor)
				{
					return Platform.UnityEditor;
				}
				return GameMetaData.Instance.Platform;
			}
			set
			{
				GameMetaData.Instance.Platform = value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x000471DE File Offset: 0x000453DE
		// (set) Token: 0x06001255 RID: 4693 RVA: 0x000471E5 File Offset: 0x000453E5
		public static Func<string> GetIDFunc
		{
			get
			{
				return PlatformInfo._getIDFunc;
			}
			set
			{
				PlatformInfo._getIDFunc = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x000471ED File Offset: 0x000453ED
		// (set) Token: 0x06001257 RID: 4695 RVA: 0x000471F4 File Offset: 0x000453F4
		public static Func<string> GetDisplayNameFunc
		{
			get
			{
				return PlatformInfo._getDisplayNameFunc;
			}
			set
			{
				PlatformInfo._getDisplayNameFunc = value;
			}
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000471FC File Offset: 0x000453FC
		public static string GetID()
		{
			string text = null;
			if (PlatformInfo.GetIDFunc != null)
			{
				text = PlatformInfo.GetIDFunc();
			}
			if (text == null)
			{
				text = Environment.MachineName;
			}
			return text;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00047227 File Offset: 0x00045427
		public static string GetDisplayName()
		{
			if (PlatformInfo.GetDisplayNameFunc != null)
			{
				return PlatformInfo.GetDisplayNameFunc();
			}
			return "UNKOWN";
		}

		// Token: 0x04000E20 RID: 3616
		private static Func<string> _getIDFunc;

		// Token: 0x04000E21 RID: 3617
		private static Func<string> _getDisplayNameFunc;
	}
}
