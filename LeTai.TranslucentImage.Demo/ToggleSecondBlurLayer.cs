using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x0200000D RID: 13
	public class ToggleSecondBlurLayer : MonoBehaviour
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002953 File Offset: 0x00000B53
		private void Start()
		{
			base.StartCoroutine(this.DisableSource());
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002962 File Offset: 0x00000B62
		private IEnumerator DisableSource()
		{
			yield return null;
			this.changer.SetUpdateRate(0f);
			yield break;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002971 File Offset: 0x00000B71
		public void Toggle()
		{
			if (Mathf.Approximately(this.changer.GetUpdateRate(), 0f))
			{
				this.changer.SetUpdateRate(this.updateRateInput.value);
				return;
			}
			this.changer.SetUpdateRate(0f);
		}

		// Token: 0x04000029 RID: 41
		public ChangeBlurConfig changer;

		// Token: 0x0400002A RID: 42
		public Slider updateRateInput;
	}
}
