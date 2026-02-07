using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B6 RID: 694
	public class PassivePropDisplay : MonoBehaviour
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x00054CF6 File Offset: 0x00052EF6
		// (set) Token: 0x060016DA RID: 5850 RVA: 0x00054CFE File Offset: 0x00052EFE
		public RectTransform rectTransform { get; private set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00054D07 File Offset: 0x00052F07
		public NavEntry NavEntry
		{
			get
			{
				return this.navEntry;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x00054D0F File Offset: 0x00052F0F
		// (set) Token: 0x060016DD RID: 5853 RVA: 0x00054D17 File Offset: 0x00052F17
		public GoldMinerArtifact Target { get; private set; }

		// Token: 0x060016DE RID: 5854 RVA: 0x00054D20 File Offset: 0x00052F20
		internal void Setup(GoldMinerArtifact target, int amount)
		{
			this.Target = target;
			this.icon.sprite = target.Icon;
			this.rectTransform = (base.transform as RectTransform);
			this.amounText.text = ((amount > 1) ? string.Format("{0}", amount) : "");
		}

		// Token: 0x040010F4 RID: 4340
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x040010F5 RID: 4341
		[SerializeField]
		private Image icon;

		// Token: 0x040010F6 RID: 4342
		[SerializeField]
		private TextMeshProUGUI amounText;
	}
}
