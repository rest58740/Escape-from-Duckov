using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AC RID: 684
	public class GoldMinerMoneyDisplay : MonoBehaviour
	{
		// Token: 0x06001697 RID: 5783 RVA: 0x000541DC File Offset: 0x000523DC
		private void Update()
		{
			this.text.text = this.master.Money.ToString(this.format);
		}

		// Token: 0x040010B4 RID: 4276
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010B5 RID: 4277
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040010B6 RID: 4278
		[SerializeField]
		private string format = "$0";
	}
}
