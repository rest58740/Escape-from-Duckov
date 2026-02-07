using System;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x0200001E RID: 30
	public abstract class GlobalConfig<T> : ScriptableObject, IGlobalConfigEvents where T : GlobalConfig<T>, new()
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000B3E4 File Offset: 0x000095E4
		public static GlobalConfigAttribute ConfigAttribute
		{
			get
			{
				if (GlobalConfig<T>.configAttribute == null)
				{
					GlobalConfig<T>.configAttribute = typeof(T).GetCustomAttribute<GlobalConfigAttribute>();
					if (GlobalConfig<T>.configAttribute == null)
					{
						GlobalConfig<T>.configAttribute = new GlobalConfigAttribute(typeof(T).GetNiceName());
					}
				}
				return GlobalConfig<T>.configAttribute;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000B431 File Offset: 0x00009631
		public static bool HasInstanceLoaded
		{
			get
			{
				return GlobalConfigUtility<T>.HasInstanceLoaded;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000B438 File Offset: 0x00009638
		public static T Instance
		{
			get
			{
				return GlobalConfigUtility<T>.GetInstance(GlobalConfig<T>.ConfigAttribute.AssetPath, null);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000B44A File Offset: 0x0000964A
		public static void LoadInstanceIfAssetExists()
		{
			GlobalConfigUtility<T>.LoadInstanceIfAssetExists(GlobalConfig<T>.ConfigAttribute.AssetPath, null);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000B45C File Offset: 0x0000965C
		public void OpenInEditor()
		{
			Debug.Log("Downloading, installing and launching the Unity Editor so we can open this config window in the editor, please stand by until pigs can fly and hell has frozen over...");
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000B468 File Offset: 0x00009668
		protected virtual void OnConfigInstanceFirstAccessed()
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000B468 File Offset: 0x00009668
		protected virtual void OnConfigAutoCreated()
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000B46A File Offset: 0x0000966A
		void IGlobalConfigEvents.OnConfigAutoCreated()
		{
			this.OnConfigAutoCreated();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000B472 File Offset: 0x00009672
		void IGlobalConfigEvents.OnConfigInstanceFirstAccessed()
		{
			this.OnConfigInstanceFirstAccessed();
		}

		// Token: 0x0400004C RID: 76
		private static GlobalConfigAttribute configAttribute;

		// Token: 0x0400004D RID: 77
		private static T instance;
	}
}
