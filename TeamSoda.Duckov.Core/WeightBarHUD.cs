using System;
using ItemStatsSystem;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000D5 RID: 213
public class WeightBarHUD : MonoBehaviour
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001E4B3 File Offset: 0x0001C6B3
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0001E4C0 File Offset: 0x0001C6C0
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
		}
		float totalWeight = this.characterMainControl.CharacterItem.TotalWeight;
		float num = this.characterMainControl.MaxWeight;
		if (!Mathf.Approximately(totalWeight, this.weight) || !Mathf.Approximately(num, this.maxWeight))
		{
			this.weight = totalWeight;
			this.maxWeight = num;
			this.percent = this.weight / this.maxWeight;
			this.weightText.text = string.Format(this.weightTextFormat, this.weight, this.maxWeight);
			this.fillImage.fillAmount = this.percent;
			this.SetColor();
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x0001E598 File Offset: 0x0001C798
	private void SetColor()
	{
		Color color;
		if (this.percent < 0.25f)
		{
			color = this.lightColor;
		}
		else if (this.percent < 0.75f)
		{
			color = this.normalColor;
		}
		else if (this.percent < 1f)
		{
			color = this.heavyColor;
		}
		else
		{
			color = this.overWeightColor;
		}
		float num;
		float num2;
		float num3;
		Color.RGBToHSV(color, ref num, ref num2, ref num3);
		Color color2 = color;
		if (num2 > 0.4f)
		{
			num2 = 0.4f;
			num3 = 1f;
			color2 = Color.HSVToRGB(num, num2, num3);
		}
		this.glow.Color = color;
		this.fillImage.color = color2;
		this.weightText.color = color;
	}

	// Token: 0x0400067F RID: 1663
	private CharacterMainControl characterMainControl;

	// Token: 0x04000680 RID: 1664
	private float percent;

	// Token: 0x04000681 RID: 1665
	private float weight;

	// Token: 0x04000682 RID: 1666
	private float maxWeight;

	// Token: 0x04000683 RID: 1667
	public ProceduralImage fillImage;

	// Token: 0x04000684 RID: 1668
	public TrueShadow glow;

	// Token: 0x04000685 RID: 1669
	public Color lightColor;

	// Token: 0x04000686 RID: 1670
	public Color normalColor;

	// Token: 0x04000687 RID: 1671
	public Color heavyColor;

	// Token: 0x04000688 RID: 1672
	public Color overWeightColor;

	// Token: 0x04000689 RID: 1673
	public TextMeshProUGUI weightText;

	// Token: 0x0400068A RID: 1674
	public string weightTextFormat = "{0:0.#}/{1:0.#}kg";
}
