using System;
using UnityEngine;

namespace Duckov.CustomOptions
{
	// Token: 0x0200031D RID: 797
	public class CustomOptionsPanel : MonoBehaviour
	{
		// Token: 0x06001A21 RID: 6689 RVA: 0x0005F738 File Offset: 0x0005D938
		private void OnEnable()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = (base.transform as RectTransform);
				if (this.rectTransform == null)
				{
					Debug.LogError("[Custom Options] Failed to convert to rect transform.");
					return;
				}
			}
			Action<RectTransform> onPanelEnabled = CustomOptionsPanel.OnPanelEnabled;
			if (onPanelEnabled == null)
			{
				return;
			}
			onPanelEnabled(this.rectTransform);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0005F792 File Offset: 0x0005D992
		private void OnDisable()
		{
		}

		// Token: 0x040012F3 RID: 4851
		private RectTransform rectTransform;

		// Token: 0x040012F4 RID: 4852
		public static Action<RectTransform> OnPanelEnabled;
	}
}
