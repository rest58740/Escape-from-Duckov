using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000B4 RID: 180
	public static class UnitySerializationInitializer
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x000216A0 File Offset: 0x0001F8A0
		public static bool Initialized
		{
			get
			{
				return UnitySerializationInitializer.initialized;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000216A7 File Offset: 0x0001F8A7
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x000216AE File Offset: 0x0001F8AE
		public static RuntimePlatform CurrentPlatform { get; private set; }

		// Token: 0x060004F1 RID: 1265 RVA: 0x000216B8 File Offset: 0x0001F8B8
		public static void Initialize()
		{
			if (!UnitySerializationInitializer.initialized)
			{
				object @lock = UnitySerializationInitializer.LOCK;
				lock (@lock)
				{
					if (!UnitySerializationInitializer.initialized)
					{
						try
						{
							GlobalConfig<GlobalSerializationConfig>.LoadInstanceIfAssetExists();
							UnitySerializationInitializer.CurrentPlatform = Application.platform;
							if (!Application.isEditor)
							{
								ArchitectureInfo.SetRuntimePlatform(UnitySerializationInitializer.CurrentPlatform);
							}
						}
						finally
						{
							UnitySerializationInitializer.initialized = true;
						}
					}
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00021738 File Offset: 0x0001F938
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeRuntime()
		{
			UnitySerializationInitializer.Initialize();
		}

		// Token: 0x040001C5 RID: 453
		private static readonly object LOCK = new object();

		// Token: 0x040001C6 RID: 454
		private static bool initialized = false;
	}
}
