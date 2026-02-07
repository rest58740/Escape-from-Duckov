using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.Inventories
{
	// Token: 0x020003E7 RID: 999
	public class PagesControl_Entry : MonoBehaviour
	{
		// Token: 0x06002471 RID: 9329 RVA: 0x0008012D File Offset: 0x0007E32D
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0008014B File Offset: 0x0007E34B
		private void OnButtonClicked()
		{
			this.master.NotifySelect(this.index);
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00080160 File Offset: 0x0007E360
		internal void Setup(PagesControl master, int i, bool selected)
		{
			this.master = master;
			this.index = i;
			this.selected = selected;
			this.text.text = string.Format("{0}", this.index);
			this.selectedIndicator.SetActive(this.selected);
		}

		// Token: 0x040018C9 RID: 6345
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018CA RID: 6346
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x040018CB RID: 6347
		[SerializeField]
		private Button button;

		// Token: 0x040018CC RID: 6348
		private PagesControl master;

		// Token: 0x040018CD RID: 6349
		private int index;

		// Token: 0x040018CE RID: 6350
		private bool selected;
	}
}
