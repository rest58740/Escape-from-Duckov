using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI.PlayerStats
{
	// Token: 0x020003E8 RID: 1000
	public class MainCharacterStatValueDisplay : MonoBehaviour
	{
		// Token: 0x06002475 RID: 9333 RVA: 0x000801BC File Offset: 0x0007E3BC
		private void OnEnable()
		{
			if (this.target == null)
			{
				CharacterMainControl main = CharacterMainControl.Main;
				Stat stat;
				if (main == null)
				{
					stat = null;
				}
				else
				{
					Item characterItem = main.CharacterItem;
					stat = ((characterItem != null) ? characterItem.GetStat(this.statKey.GetHashCode()) : null);
				}
				this.target = stat;
			}
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0008020B File Offset: 0x0007E40B
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00080213 File Offset: 0x0007E413
		private void AutoRename()
		{
			base.gameObject.name = "StatDisplay_" + this.statKey;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00080230 File Offset: 0x0007E430
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty += this.OnTargetDirty;
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x00080252 File Offset: 0x0007E452
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty -= this.OnTargetDirty;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00080274 File Offset: 0x0007E474
		private void OnTargetDirty(Stat stat)
		{
			this.Refresh();
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0008027C File Offset: 0x0007E47C
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.displayNameText.text = this.target.DisplayName;
			float value = this.target.Value;
			this.valueText.text = string.Format(this.format, value);
		}

		// Token: 0x040018CF RID: 6351
		[SerializeField]
		private string statKey;

		// Token: 0x040018D0 RID: 6352
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040018D1 RID: 6353
		[SerializeField]
		private TextMeshProUGUI valueText;

		// Token: 0x040018D2 RID: 6354
		[SerializeField]
		private string format = "{0:0.0}";

		// Token: 0x040018D3 RID: 6355
		private Stat target;
	}
}
