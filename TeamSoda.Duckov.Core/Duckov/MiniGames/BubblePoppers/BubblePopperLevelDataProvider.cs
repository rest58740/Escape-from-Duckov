using System;
using UnityEngine;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002EB RID: 747
	public class BubblePopperLevelDataProvider : MonoBehaviour
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x00059478 File Offset: 0x00057678
		public int TotalLevels
		{
			get
			{
				return this.totalLevels;
			}
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00059480 File Offset: 0x00057680
		internal int[] GetData(int levelIndex)
		{
			int num = this.seed + levelIndex;
			int[] array = new int[60 + 10 * (levelIndex / 2)];
			Random random = new Random(num);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = random.Next(0, this.master.AvaliableColorCount);
			}
			return array;
		}

		// Token: 0x040011AF RID: 4527
		[SerializeField]
		private BubblePopper master;

		// Token: 0x040011B0 RID: 4528
		[SerializeField]
		private int totalLevels = 10;

		// Token: 0x040011B1 RID: 4529
		[SerializeField]
		public int seed;
	}
}
