using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B0 RID: 944
	public class ItemModifierEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002154 RID: 8532 RVA: 0x00074F5B File Offset: 0x0007315B
		public void NotifyPooled()
		{
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00074F5D File Offset: 0x0007315D
		public void NotifyReleased()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00074F65 File Offset: 0x00073165
		internal void Setup(ModifierDescription target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00074F80 File Offset: 0x00073180
		private void Refresh()
		{
			this.displayName.text = this.target.DisplayName;
			StatInfoDatabase.Entry entry = StatInfoDatabase.Get(this.target.Key);
			this.value.text = this.target.GetDisplayValueString(entry.DisplayFormat);
			Color color = this.color_Neutral;
			Polarity polarity = entry.polarity;
			if (this.target.Value != 0f)
			{
				switch (polarity)
				{
				case -1:
					color = ((this.target.Value < 0f) ? this.color_Positive : this.color_Negative);
					break;
				case 1:
					color = ((this.target.Value > 0f) ? this.color_Positive : this.color_Negative);
					break;
				}
			}
			this.value.color = color;
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00075057 File Offset: 0x00073257
		private void RegisterEvents()
		{
			ModifierDescription modifierDescription = this.target;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x00075060 File Offset: 0x00073260
		private void UnregisterEvents()
		{
			ModifierDescription modifierDescription = this.target;
		}

		// Token: 0x040016BE RID: 5822
		private ModifierDescription target;

		// Token: 0x040016BF RID: 5823
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040016C0 RID: 5824
		[SerializeField]
		private TextMeshProUGUI value;

		// Token: 0x040016C1 RID: 5825
		[SerializeField]
		private Color color_Neutral;

		// Token: 0x040016C2 RID: 5826
		[SerializeField]
		private Color color_Positive;

		// Token: 0x040016C3 RID: 5827
		[SerializeField]
		private Color color_Negative;
	}
}
