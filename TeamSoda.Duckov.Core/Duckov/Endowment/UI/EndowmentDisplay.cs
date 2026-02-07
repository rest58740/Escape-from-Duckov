using System;
using TMPro;
using UnityEngine;

namespace Duckov.Endowment.UI
{
	// Token: 0x02000308 RID: 776
	public class EndowmentDisplay : MonoBehaviour
	{
		// Token: 0x06001979 RID: 6521 RVA: 0x0005D630 File Offset: 0x0005B830
		private void Refresh()
		{
			EndowmentEntry endowmentEntry = EndowmentManager.Current;
			if (endowmentEntry == null)
			{
				this.displayNameText.text = "?";
				this.descriptionsText.text = "?";
				return;
			}
			this.displayNameText.text = endowmentEntry.DisplayName;
			this.descriptionsText.text = endowmentEntry.DescriptionAndEffects;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0005D68F File Offset: 0x0005B88F
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x04001290 RID: 4752
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001291 RID: 4753
		[SerializeField]
		private TextMeshProUGUI descriptionsText;
	}
}
