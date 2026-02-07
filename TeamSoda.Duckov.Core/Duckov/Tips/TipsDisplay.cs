using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Duckov.Tips
{
	// Token: 0x02000253 RID: 595
	public class TipsDisplay : MonoBehaviour
	{
		// Token: 0x060012C5 RID: 4805 RVA: 0x0004802C File Offset: 0x0004622C
		public void DisplayRandom()
		{
			if (this.entries.Length == 0)
			{
				return;
			}
			TipEntry tipEntry = this.entries[Random.Range(0, this.entries.Length)];
			this.text.text = tipEntry.Description;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00048070 File Offset: 0x00046270
		public void Display(string tipID)
		{
			TipEntry tipEntry = this.entries.FirstOrDefault((TipEntry e) => e.TipID == tipID);
			if (tipEntry.TipID != tipID)
			{
				return;
			}
			this.text.text = tipEntry.Description;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x000480C9 File Offset: 0x000462C9
		private void OnEnable()
		{
			this.canvasGroup.alpha = (SceneLoader.HideTips ? 0f : 1f);
			this.DisplayRandom();
		}

		// Token: 0x04000E6C RID: 3692
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000E6D RID: 3693
		[SerializeField]
		private TipEntry[] entries;

		// Token: 0x04000E6E RID: 3694
		[SerializeField]
		private CanvasGroup canvasGroup;
	}
}
