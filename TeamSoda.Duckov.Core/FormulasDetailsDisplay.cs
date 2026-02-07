using System;
using Duckov.Economy;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BD RID: 445
public class FormulasDetailsDisplay : MonoBehaviour
{
	// Token: 0x06000D61 RID: 3425 RVA: 0x0003839B File Offset: 0x0003659B
	private void SetupEmpty()
	{
		this.contentFadeGroup.Hide();
		this.placeHolderFadeGroup.Show();
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x000383B3 File Offset: 0x000365B3
	private void SetupFormula(CraftingFormula formula)
	{
		this.formula = formula;
		this.RefreshContent();
		this.contentFadeGroup.Show();
		this.placeHolderFadeGroup.Hide();
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x000383D8 File Offset: 0x000365D8
	private void RefreshContent()
	{
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.formula.result.id);
		this.nameText.text = metaData.DisplayName;
		this.descriptionText.text = metaData.Description;
		this.image.sprite = metaData.icon;
		this.costDisplay.Setup(this.formula.cost, 1);
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00038447 File Offset: 0x00036647
	public void Setup(CraftingFormula? formula)
	{
		if (formula == null)
		{
			this.SetupEmpty();
			return;
		}
		if (!CraftingManager.IsFormulaUnlocked(formula.Value.id))
		{
			this.SetupUnknown();
			return;
		}
		this.SetupFormula(formula.Value);
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x00038480 File Offset: 0x00036680
	private void SetupUnknown()
	{
		this.nameText.text = "???";
		this.descriptionText.text = "???";
		this.image.sprite = this.unknownImage;
		this.contentFadeGroup.Show();
		this.placeHolderFadeGroup.Hide();
		this.costDisplay.Setup(default(Cost), 1);
	}

	// Token: 0x04000B95 RID: 2965
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000B96 RID: 2966
	[SerializeField]
	private Image image;

	// Token: 0x04000B97 RID: 2967
	[SerializeField]
	private TextMeshProUGUI descriptionText;

	// Token: 0x04000B98 RID: 2968
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000B99 RID: 2969
	[SerializeField]
	private FadeGroup contentFadeGroup;

	// Token: 0x04000B9A RID: 2970
	[SerializeField]
	private FadeGroup placeHolderFadeGroup;

	// Token: 0x04000B9B RID: 2971
	[SerializeField]
	private Sprite unknownImage;

	// Token: 0x04000B9C RID: 2972
	private CraftingFormula formula;
}
