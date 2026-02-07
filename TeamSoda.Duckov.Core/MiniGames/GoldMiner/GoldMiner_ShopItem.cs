using System;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A4 RID: 676
	public class GoldMiner_ShopItem : MonoBehaviour
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00053C0A File Offset: 0x00051E0A
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x00053C12 File Offset: 0x00051E12
		public string DisplayNameKey
		{
			get
			{
				return this.displayNameKey;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x00053C1A File Offset: 0x00051E1A
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayNameKey);
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x00053C27 File Offset: 0x00051E27
		public int BasePrice
		{
			get
			{
				return this.basePrice;
			}
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00053C2F File Offset: 0x00051E2F
		public void OnBought(GoldMiner target)
		{
			UnityEvent<GoldMiner> unityEvent = this.onBought;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(target);
		}

		// Token: 0x0400109D RID: 4253
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400109E RID: 4254
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayNameKey;

		// Token: 0x0400109F RID: 4255
		[SerializeField]
		private int basePrice;

		// Token: 0x040010A0 RID: 4256
		public UnityEvent<GoldMiner> onBought;
	}
}
