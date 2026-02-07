using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000004 RID: 4
	public static class DefaultLoggers
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
		public static ILogger DefaultLogger
		{
			get
			{
				return DefaultLoggers.UnityLogger;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020D4 File Offset: 0x000002D4
		public static ILogger UnityLogger
		{
			get
			{
				if (DefaultLoggers.unityLogger == null)
				{
					object @lock = DefaultLoggers.LOCK;
					lock (@lock)
					{
						if (DefaultLoggers.unityLogger == null)
						{
							DefaultLoggers.unityLogger = new CustomLogger(new Action<string>(Debug.LogWarning), new Action<string>(Debug.LogError), new Action<Exception>(Debug.LogException));
						}
					}
				}
				return DefaultLoggers.unityLogger;
			}
		}

		// Token: 0x04000008 RID: 8
		private static readonly object LOCK = new object();

		// Token: 0x04000009 RID: 9
		private static volatile ILogger unityLogger;
	}
}
