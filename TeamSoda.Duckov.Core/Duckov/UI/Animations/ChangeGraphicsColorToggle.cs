using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003FD RID: 1021
	public class ChangeGraphicsColorToggle : ToggleComponent
	{
		// Token: 0x0600251C RID: 9500 RVA: 0x00081D6B File Offset: 0x0007FF6B
		protected override void OnSetToggle(ToggleAnimation master, bool value)
		{
			ShortcutExtensions.DOKill(this.image, false);
			DOTweenModuleUI.DOColor(this.image, value ? this.trueColor : this.falseColor, this.duration);
		}

		// Token: 0x0400192F RID: 6447
		[SerializeField]
		private Image image;

		// Token: 0x04001930 RID: 6448
		[SerializeField]
		private Color trueColor;

		// Token: 0x04001931 RID: 6449
		[SerializeField]
		private Color falseColor;

		// Token: 0x04001932 RID: 6450
		[SerializeField]
		private float duration = 0.1f;
	}
}
