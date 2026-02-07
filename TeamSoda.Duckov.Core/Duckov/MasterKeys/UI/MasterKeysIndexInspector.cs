using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002EF RID: 751
	public class MasterKeysIndexInspector : MonoBehaviour
	{
		// Token: 0x0600184C RID: 6220 RVA: 0x00059AED File Offset: 0x00057CED
		internal void Setup(MasterKeysIndexEntry target)
		{
			if (target == null)
			{
				this.SetupEmpty();
				return;
			}
			this.SetupNormal(target);
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00059B08 File Offset: 0x00057D08
		private void SetupNormal(MasterKeysIndexEntry target)
		{
			this.targetItemID = target.ItemID;
			this.placeHolder.SetActive(false);
			this.content.SetActive(true);
			this.nameText.text = target.DisplayName;
			this.descriptionText.text = target.Description;
			this.icon.sprite = target.Icon;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00059B6C File Offset: 0x00057D6C
		private void SetupEmpty()
		{
			this.content.gameObject.SetActive(false);
			this.placeHolder.SetActive(true);
		}

		// Token: 0x040011C2 RID: 4546
		[SerializeField]
		private int targetItemID;

		// Token: 0x040011C3 RID: 4547
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040011C4 RID: 4548
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x040011C5 RID: 4549
		[SerializeField]
		private Image icon;

		// Token: 0x040011C6 RID: 4550
		[SerializeField]
		private GameObject content;

		// Token: 0x040011C7 RID: 4551
		[SerializeField]
		private GameObject placeHolder;
	}
}
