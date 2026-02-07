using System;
using Cysharp.Threading.Tasks;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020001E8 RID: 488
public class UIKeybindingEntry : MonoBehaviour
{
	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0003BF00 File Offset: 0x0003A100
	// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0003BF4F File Offset: 0x0003A14F
	[LocalizationKey("UIText")]
	private string displayNameKey
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overrideDisplayNameKey))
			{
				return this.overrideDisplayNameKey;
			}
			if (this.actionRef == null)
			{
				return "?";
			}
			return "Input_" + this.actionRef.action.name;
		}
		set
		{
		}
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0003BF54 File Offset: 0x0003A154
	private void Awake()
	{
		this.rebindButton.onClick.AddListener(new UnityAction(this.OnButtonClick));
		this.clearButton.onClick.AddListener(new UnityAction(this.OnClearButtonClick));
		this.Setup();
		LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0003BFB0 File Offset: 0x0003A1B0
	private void OnClearButtonClick()
	{
		InputRebinder.ClearRebind(this.actionRef.action.name);
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0003BFC7 File Offset: 0x0003A1C7
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0003BFDA File Offset: 0x0003A1DA
	private void OnLanguageChanged(SystemLanguage language)
	{
		this.label.text = LocalizationManager.ToPlainText(this.displayNameKey);
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0003BFF2 File Offset: 0x0003A1F2
	private void OnButtonClick()
	{
		InputRebinder.RebindAsync(this.actionRef.action.name, this.index, this.excludes, true).Forget<bool>();
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0003C01B File Offset: 0x0003A21B
	private void OnValidate()
	{
		this.Setup();
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0003C023 File Offset: 0x0003A223
	private void Setup()
	{
		this.indicator.Setup(this.actionRef, this.index);
		this.label.text = LocalizationManager.ToPlainText(this.displayNameKey);
	}

	// Token: 0x04000C3A RID: 3130
	[SerializeField]
	private InputActionReference actionRef;

	// Token: 0x04000C3B RID: 3131
	[SerializeField]
	private int index;

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private string overrideDisplayNameKey;

	// Token: 0x04000C3D RID: 3133
	private string[] excludes = new string[]
	{
		"<Mouse>/leftButton",
		"<Mouse>/rightButton",
		"<Pointer>/position",
		"<Pointer>/delta",
		"<Pointer>/press",
		"<Mouse>/scroll"
	};

	// Token: 0x04000C3E RID: 3134
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04000C3F RID: 3135
	[SerializeField]
	private Button rebindButton;

	// Token: 0x04000C40 RID: 3136
	[SerializeField]
	private Button clearButton;

	// Token: 0x04000C41 RID: 3137
	[SerializeField]
	private InputIndicator indicator;
}
