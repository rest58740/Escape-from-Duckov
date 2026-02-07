using System;
using Duckov.Buffs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039F RID: 927
	public class ExtraBuffViewHoverPanel : MonoBehaviour
	{
		// Token: 0x0600203A RID: 8250 RVA: 0x000715FE File Offset: 0x0006F7FE
		internal void NotifyEnter(ExtraBuffViewEntry extraBuffViewEntry)
		{
			this.target = extraBuffViewEntry;
			if (this.target == null || this.target.target == null)
			{
				return;
			}
			this.Refresh();
			this.SkipShow();
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x00071635 File Offset: 0x0006F835
		private void FixedUpdate()
		{
			if (this.target == null || this.target.target == null)
			{
				this.SkipHide();
				return;
			}
			this.Refresh();
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00071668 File Offset: 0x0006F868
		private void Refresh()
		{
			if (this.target == null || this.target.target == null)
			{
				return;
			}
			Buff buff = this.target.target;
			this.displayName.text = buff.DisplayName;
			this.layer.text = ((buff.CurrentLayers > 1) ? string.Format("{0}", buff.CurrentLayers) : "");
			this.description.text = buff.Description;
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000716F5 File Offset: 0x0006F8F5
		internal void NotifyExit(ExtraBuffViewEntry extraBuffViewEntry)
		{
			if (this.target == null)
			{
				this.SkipHide();
			}
			if (this.target != extraBuffViewEntry)
			{
				return;
			}
			this.SkipHide();
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00071720 File Offset: 0x0006F920
		public void SkipShow()
		{
			this.fadeGroup.SkipShow();
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0007172D File Offset: 0x0006F92D
		public void SkipHide()
		{
			this.target = null;
			this.fadeGroup.SkipHide();
		}

		// Token: 0x04001607 RID: 5639
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001608 RID: 5640
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001609 RID: 5641
		[SerializeField]
		private TextMeshProUGUI layer;

		// Token: 0x0400160A RID: 5642
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x0400160B RID: 5643
		private ExtraBuffViewEntry target;
	}
}
