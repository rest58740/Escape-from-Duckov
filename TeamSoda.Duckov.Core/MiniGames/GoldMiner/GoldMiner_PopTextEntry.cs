using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B2 RID: 690
	public class GoldMiner_PopTextEntry : MonoBehaviour
	{
		// Token: 0x060016B7 RID: 5815 RVA: 0x00054750 File Offset: 0x00052950
		public void Setup(Vector3 pos, string text, Action<GoldMiner_PopTextEntry> releaseAction)
		{
			this.initialized = true;
			this.tmp.text = text;
			this.life = 0f;
			base.transform.position = pos;
			this.releaseAction = releaseAction;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00054784 File Offset: 0x00052984
		private void Update()
		{
			if (!this.initialized)
			{
				return;
			}
			this.life += Time.deltaTime;
			base.transform.position += Vector3.up * this.moveSpeed * Time.deltaTime;
			if (this.life >= this.lifeTime)
			{
				this.Release();
			}
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000547F0 File Offset: 0x000529F0
		private void Release()
		{
			if (this.releaseAction != null)
			{
				this.releaseAction(this);
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x040010D3 RID: 4307
		public TextMeshProUGUI tmp;

		// Token: 0x040010D4 RID: 4308
		public float lifeTime;

		// Token: 0x040010D5 RID: 4309
		public float moveSpeed = 1f;

		// Token: 0x040010D6 RID: 4310
		private bool initialized;

		// Token: 0x040010D7 RID: 4311
		private float life;

		// Token: 0x040010D8 RID: 4312
		private Action<GoldMiner_PopTextEntry> releaseAction;
	}
}
