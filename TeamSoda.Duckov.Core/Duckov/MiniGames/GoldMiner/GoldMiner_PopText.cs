using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B1 RID: 689
	public class GoldMiner_PopText : MiniGameBehaviour
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x000546E0 File Offset: 0x000528E0
		private PrefabPool<GoldMiner_PopTextEntry> TextPool
		{
			get
			{
				if (this._textPool == null)
				{
					this._textPool = new PrefabPool<GoldMiner_PopTextEntry>(this.textTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._textPool;
			}
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00054719 File Offset: 0x00052919
		public void Pop(string content, Vector3 position)
		{
			this.TextPool.Get(null).Setup(position, content, new Action<GoldMiner_PopTextEntry>(this.ReleaseEntry));
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0005473A File Offset: 0x0005293A
		private void ReleaseEntry(GoldMiner_PopTextEntry entry)
		{
			this.TextPool.Release(entry);
		}

		// Token: 0x040010D1 RID: 4305
		[SerializeField]
		private GoldMiner_PopTextEntry textTemplate;

		// Token: 0x040010D2 RID: 4306
		private PrefabPool<GoldMiner_PopTextEntry> _textPool;
	}
}
