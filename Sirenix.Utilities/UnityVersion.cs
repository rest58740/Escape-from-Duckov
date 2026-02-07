using System;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000037 RID: 55
	public static class UnityVersion
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000D450 File Offset: 0x0000B650
		static UnityVersion()
		{
			string[] array = Application.unityVersion.Split(new char[]
			{
				'.'
			});
			if (array.Length < 2)
			{
				Debug.LogError("Could not parse current Unity version '" + Application.unityVersion + "'; not enough version elements.");
				return;
			}
			if (!int.TryParse(array[0], ref UnityVersion.Major))
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Could not parse major part '",
					array[0],
					"' of Unity version '",
					Application.unityVersion,
					"'."
				}));
			}
			if (!int.TryParse(array[1], ref UnityVersion.Minor))
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Could not parse minor part '",
					array[1],
					"' of Unity version '",
					Application.unityVersion,
					"'."
				}));
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000B468 File Offset: 0x00009668
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void EnsureLoaded()
		{
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D51D File Offset: 0x0000B71D
		public static bool IsVersionOrGreater(int major, int minor)
		{
			return UnityVersion.Major > major || (UnityVersion.Major == major && UnityVersion.Minor >= minor);
		}

		// Token: 0x04000077 RID: 119
		public static readonly int Major;

		// Token: 0x04000078 RID: 120
		public static readonly int Minor;
	}
}
