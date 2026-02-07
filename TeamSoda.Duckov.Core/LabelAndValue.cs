using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class LabelAndValue : MonoBehaviour
{
	// Token: 0x06000AEB RID: 2795 RVA: 0x0003016C File Offset: 0x0002E36C
	public void Setup(string label, string value, Polarity valuePolarity)
	{
		this.labelText.text = label;
		this.valueText.text = value;
		Color color = this.colorNeutral;
		switch (valuePolarity)
		{
		case -1:
			color = this.colorNegative;
			break;
		case 0:
			color = this.colorNeutral;
			break;
		case 1:
			color = this.colorPositive;
			break;
		}
		this.valueText.color = color;
	}

	// Token: 0x04000993 RID: 2451
	[SerializeField]
	private TextMeshProUGUI labelText;

	// Token: 0x04000994 RID: 2452
	[SerializeField]
	private TextMeshProUGUI valueText;

	// Token: 0x04000995 RID: 2453
	[SerializeField]
	private Color colorNeutral;

	// Token: 0x04000996 RID: 2454
	[SerializeField]
	private Color colorPositive;

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	private Color colorNegative;
}
