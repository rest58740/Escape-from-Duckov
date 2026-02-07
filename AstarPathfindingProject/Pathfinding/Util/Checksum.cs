using System;

namespace Pathfinding.Util
{
	// Token: 0x0200028D RID: 653
	internal class Checksum
	{
		// Token: 0x06000FA5 RID: 4005 RVA: 0x0005FF5C File Offset: 0x0005E15C
		public static uint GetChecksum(byte[] arr, uint hash = 0U)
		{
			hash ^= 2166136261U;
			if (arr == null)
			{
				return hash - 1U;
			}
			for (int i = 0; i < arr.Length; i++)
			{
				hash = (hash ^ (uint)arr[i]) * 16777619U;
			}
			return hash;
		}
	}
}
