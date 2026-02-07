using System;
using System.Collections.Generic;

namespace FMODUnity
{
	// Token: 0x02000129 RID: 297
	internal static class Legacy
	{
		// Token: 0x0600079B RID: 1947 RVA: 0x0000B140 File Offset: 0x00009340
		public static void CopySetting<T, U>(List<T> list, Legacy.Platform fromPlatform, Legacy.Platform toPlatform) where T : Legacy.PlatformSetting<U>, new()
		{
			T t = list.Find((T x) => x.Platform == fromPlatform);
			T t2 = list.Find((T x) => x.Platform == toPlatform);
			if (t != null)
			{
				if (t2 == null)
				{
					T t3 = Activator.CreateInstance<T>();
					t3.Platform = toPlatform;
					t2 = t3;
					list.Add(t2);
				}
				t2.Value = t.Value;
				return;
			}
			if (t2 != null)
			{
				list.Remove(t2);
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000B1DC File Offset: 0x000093DC
		public static void CopySetting(List<Legacy.PlatformBoolSetting> list, Legacy.Platform fromPlatform, Legacy.Platform toPlatform)
		{
			Legacy.CopySetting<Legacy.PlatformBoolSetting, TriStateBool>(list, fromPlatform, toPlatform);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0000B1E6 File Offset: 0x000093E6
		public static void CopySetting(List<Legacy.PlatformIntSetting> list, Legacy.Platform fromPlatform, Legacy.Platform toPlatform)
		{
			Legacy.CopySetting<Legacy.PlatformIntSetting, int>(list, fromPlatform, toPlatform);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0000B1F0 File Offset: 0x000093F0
		public static string DisplayName(Legacy.Platform platform)
		{
			switch (platform)
			{
			case Legacy.Platform.Desktop:
				return "Desktop";
			case Legacy.Platform.Mobile:
				return "Mobile";
			case Legacy.Platform.MobileHigh:
				return "High-End Mobile";
			case Legacy.Platform.MobileLow:
				return "Low-End Mobile";
			case Legacy.Platform.Console:
				return "Console";
			case Legacy.Platform.Windows:
				return "Windows";
			case Legacy.Platform.Mac:
				return "OSX";
			case Legacy.Platform.Linux:
				return "Linux";
			case Legacy.Platform.iOS:
				return "iOS";
			case Legacy.Platform.Android:
				return "Android";
			case Legacy.Platform.XboxOne:
				return "XBox One";
			case Legacy.Platform.PS4:
				return "PS4";
			case Legacy.Platform.AppleTV:
				return "Apple TV";
			case Legacy.Platform.UWP:
				return "UWP";
			case Legacy.Platform.Switch:
				return "Switch";
			case Legacy.Platform.WebGL:
				return "WebGL";
			}
			return "Unknown";
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0000B2B8 File Offset: 0x000094B8
		public static float SortOrder(Legacy.Platform legacyPlatform)
		{
			switch (legacyPlatform)
			{
			case Legacy.Platform.Desktop:
				return 1f;
			case Legacy.Platform.Mobile:
				return 2f;
			case Legacy.Platform.MobileHigh:
				return 2.1f;
			case Legacy.Platform.MobileLow:
				return 2.2f;
			case Legacy.Platform.Console:
				return 3f;
			case Legacy.Platform.Windows:
				return 1.1f;
			case Legacy.Platform.Mac:
				return 1.2f;
			case Legacy.Platform.Linux:
				return 1.3f;
			case Legacy.Platform.XboxOne:
				return 3.1f;
			case Legacy.Platform.PS4:
				return 3.2f;
			case Legacy.Platform.AppleTV:
				return 2.3f;
			case Legacy.Platform.Switch:
				return 3.3f;
			}
			return 0f;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000B364 File Offset: 0x00009564
		public static Legacy.Platform Parent(Legacy.Platform platform)
		{
			switch (platform)
			{
			case Legacy.Platform.Desktop:
			case Legacy.Platform.Mobile:
			case Legacy.Platform.Console:
				return Legacy.Platform.Default;
			case Legacy.Platform.MobileHigh:
			case Legacy.Platform.MobileLow:
			case Legacy.Platform.iOS:
			case Legacy.Platform.Android:
			case Legacy.Platform.AppleTV:
				return Legacy.Platform.Mobile;
			case Legacy.Platform.Windows:
			case Legacy.Platform.Mac:
			case Legacy.Platform.Linux:
			case Legacy.Platform.UWP:
			case Legacy.Platform.WebGL:
				return Legacy.Platform.Desktop;
			case Legacy.Platform.XboxOne:
			case Legacy.Platform.PS4:
			case Legacy.Platform.Switch:
			case Legacy.Platform.Reserved_1:
			case Legacy.Platform.Reserved_2:
			case Legacy.Platform.Reserved_3:
				return Legacy.Platform.Console;
			}
			return Legacy.Platform.None;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public static bool IsGroup(Legacy.Platform platform)
		{
			return platform - Legacy.Platform.Desktop <= 1 || platform == Legacy.Platform.Console;
		}

		// Token: 0x0200014E RID: 334
		[Serializable]
		public enum Platform
		{
			// Token: 0x040006D2 RID: 1746
			None,
			// Token: 0x040006D3 RID: 1747
			PlayInEditor,
			// Token: 0x040006D4 RID: 1748
			Default,
			// Token: 0x040006D5 RID: 1749
			Desktop,
			// Token: 0x040006D6 RID: 1750
			Mobile,
			// Token: 0x040006D7 RID: 1751
			MobileHigh,
			// Token: 0x040006D8 RID: 1752
			MobileLow,
			// Token: 0x040006D9 RID: 1753
			Console,
			// Token: 0x040006DA RID: 1754
			Windows,
			// Token: 0x040006DB RID: 1755
			Mac,
			// Token: 0x040006DC RID: 1756
			Linux,
			// Token: 0x040006DD RID: 1757
			iOS,
			// Token: 0x040006DE RID: 1758
			Android,
			// Token: 0x040006DF RID: 1759
			Deprecated_1,
			// Token: 0x040006E0 RID: 1760
			XboxOne,
			// Token: 0x040006E1 RID: 1761
			PS4,
			// Token: 0x040006E2 RID: 1762
			Deprecated_2,
			// Token: 0x040006E3 RID: 1763
			Deprecated_3,
			// Token: 0x040006E4 RID: 1764
			AppleTV,
			// Token: 0x040006E5 RID: 1765
			UWP,
			// Token: 0x040006E6 RID: 1766
			Switch,
			// Token: 0x040006E7 RID: 1767
			WebGL,
			// Token: 0x040006E8 RID: 1768
			Deprecated_4,
			// Token: 0x040006E9 RID: 1769
			Reserved_1,
			// Token: 0x040006EA RID: 1770
			Reserved_2,
			// Token: 0x040006EB RID: 1771
			Reserved_3,
			// Token: 0x040006EC RID: 1772
			Count
		}

		// Token: 0x0200014F RID: 335
		public class PlatformSettingBase
		{
			// Token: 0x040006ED RID: 1773
			public Legacy.Platform Platform;
		}

		// Token: 0x02000150 RID: 336
		public class PlatformSetting<T> : Legacy.PlatformSettingBase
		{
			// Token: 0x040006EE RID: 1774
			public T Value;
		}

		// Token: 0x02000151 RID: 337
		[Serializable]
		public class PlatformIntSetting : Legacy.PlatformSetting<int>
		{
		}

		// Token: 0x02000152 RID: 338
		[Serializable]
		public class PlatformStringSetting : Legacy.PlatformSetting<string>
		{
		}

		// Token: 0x02000153 RID: 339
		[Serializable]
		public class PlatformBoolSetting : Legacy.PlatformSetting<TriStateBool>
		{
		}
	}
}
