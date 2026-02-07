using System;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x0200001F RID: 31
	public static class GlobalConfigUtility<T> where T : ScriptableObject
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000B482 File Offset: 0x00009682
		public static bool HasInstanceLoaded
		{
			get
			{
				return GlobalConfigUtility<T>.instance != null;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000B494 File Offset: 0x00009694
		public static T GetInstance(string defaultAssetFolderPath, string defaultFileNameWithoutExtension = null)
		{
			if (GlobalConfigUtility<T>.instance == null)
			{
				GlobalConfigUtility<T>.LoadInstanceIfAssetExists(defaultAssetFolderPath, defaultFileNameWithoutExtension);
				T t = GlobalConfigUtility<T>.instance;
				if (t == null)
				{
					t = ScriptableObject.CreateInstance<T>();
				}
				GlobalConfigUtility<T>.instance = t;
				IGlobalConfigEvents globalConfigEvents = GlobalConfigUtility<T>.instance as IGlobalConfigEvents;
				if (globalConfigEvents != null)
				{
					globalConfigEvents.OnConfigInstanceFirstAccessed();
				}
			}
			return GlobalConfigUtility<T>.instance;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000B4F8 File Offset: 0x000096F8
		internal static void LoadInstanceIfAssetExists(string assetPath, string defaultFileNameWithoutExtension = null)
		{
			string text = defaultFileNameWithoutExtension ?? typeof(T).GetNiceName();
			if (assetPath.Contains("/resources/", 5))
			{
				string text2 = assetPath;
				int num = text2.LastIndexOf("/resources/", 5);
				if (num >= 0)
				{
					text2 = text2.Substring(num + "/resources/".Length);
				}
				string text3 = text;
				GlobalConfigUtility<T>.instance = Resources.Load<T>(text2 + text3);
			}
		}

		// Token: 0x0400004E RID: 78
		private static T instance;
	}
}
