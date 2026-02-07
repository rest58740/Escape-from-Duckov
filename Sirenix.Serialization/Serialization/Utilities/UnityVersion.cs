using System;
using UnityEngine;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D7 RID: 215
	internal static class UnityVersion
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x0002A668 File Offset: 0x00028868
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

		// Token: 0x06000659 RID: 1625 RVA: 0x000021B8 File Offset: 0x000003B8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void EnsureLoaded()
		{
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002A735 File Offset: 0x00028935
		public static bool IsVersionOrGreater(int major, int minor)
		{
			return UnityVersion.Major > major || (UnityVersion.Major == major && UnityVersion.Minor >= minor);
		}

		// Token: 0x0400022C RID: 556
		public static readonly int Major;

		// Token: 0x0400022D RID: 557
		public static readonly int Minor;
	}
}
