using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C6 RID: 198
public class InteractSelectionHUD : MonoBehaviour
{
	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001CE36 File Offset: 0x0001B036
	public InteractableBase InteractTarget
	{
		get
		{
			return this.interactable;
		}
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0001CE3E File Offset: 0x0001B03E
	public void SetInteractable(InteractableBase _interactable, bool _hasUpDown)
	{
		this.interactable = _interactable;
		this.text.text = this.interactable.GetInteractName();
		this.UpdateRequireItem(this.interactable);
		this.selectionPoint.SetActive(_hasUpDown);
		this.hasUpDown = _hasUpDown;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0001CE7C File Offset: 0x0001B07C
	private void UpdateRequireItem(InteractableBase interactable)
	{
		if (!interactable || !interactable.requireItem)
		{
			this.requireCanvasGroup.alpha = 0f;
			return;
		}
		this.requireCanvasGroup.alpha = 1f;
		CharacterMainControl mainCharacter = LevelManager.Instance.MainCharacter;
		bool flag = interactable.whenToUseRequireItem > InteractableBase.WhenToUseRequireItemTypes.None;
		string str = flag ? LocalizationManager.ToPlainText(this.requirUseItemTextKey) : LocalizationManager.ToPlainText(this.requirItemTextKey);
		this.requireText.text = str + " " + interactable.GetRequiredItemName();
		if (flag)
		{
			TextMeshProUGUI textMeshProUGUI = this.requireText;
			textMeshProUGUI.text += " x1";
		}
		this.requirementIcon.sprite = interactable.GetRequireditemIcon();
		if (interactable.TryGetRequiredItem(mainCharacter).Item1)
		{
			this.requireItemBackgroundImage.color = this.hasRequireItemColor;
			return;
		}
		this.requireItemBackgroundImage.color = this.noRequireItemColor;
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0001CF68 File Offset: 0x0001B168
	public void SetSelection(bool _select)
	{
		this.selecting = _select;
		this.selectIndicator.SetActive(this.selecting);
		this.upDownIndicator.SetActive(this.selecting && this.hasUpDown);
		this.selectionPoint.SetActive(!this.selecting && this.hasUpDown);
		if (_select)
		{
			UnityEvent onSelectedEvent = this.OnSelectedEvent;
			if (onSelectedEvent != null)
			{
				onSelectedEvent.Invoke();
			}
			this.background.color = this.selectedColor;
			return;
		}
		this.background.color = this.unselectedColor;
	}

	// Token: 0x0400061C RID: 1564
	private InteractableBase interactable;

	// Token: 0x0400061D RID: 1565
	public GameObject selectIndicator;

	// Token: 0x0400061E RID: 1566
	public TextMeshProUGUI text;

	// Token: 0x0400061F RID: 1567
	public ProceduralImage background;

	// Token: 0x04000620 RID: 1568
	public Color selectedColor;

	// Token: 0x04000621 RID: 1569
	public Color unselectedColor;

	// Token: 0x04000622 RID: 1570
	public CanvasGroup requireCanvasGroup;

	// Token: 0x04000623 RID: 1571
	public ProceduralImage requireItemBackgroundImage;

	// Token: 0x04000624 RID: 1572
	public TextMeshProUGUI requireText;

	// Token: 0x04000625 RID: 1573
	[LocalizationKey("UI")]
	public string requirItemTextKey = "UI_RequireItem";

	// Token: 0x04000626 RID: 1574
	[LocalizationKey("UI")]
	public string requirUseItemTextKey = "UI_RequireUseItem";

	// Token: 0x04000627 RID: 1575
	public Image requirementIcon;

	// Token: 0x04000628 RID: 1576
	public Color hasRequireItemColor;

	// Token: 0x04000629 RID: 1577
	public Color noRequireItemColor;

	// Token: 0x0400062A RID: 1578
	private bool selecting;

	// Token: 0x0400062B RID: 1579
	public UnityEvent OnSelectedEvent;

	// Token: 0x0400062C RID: 1580
	public GameObject selectionPoint;

	// Token: 0x0400062D RID: 1581
	public GameObject upDownIndicator;

	// Token: 0x0400062E RID: 1582
	private bool hasUpDown;
}
