using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SodaCraft.Localizations
{
	// Token: 0x0200000C RID: 12
	public class TextLocalizor : MonoBehaviour
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002BBF File Offset: 0x00000DBF
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002BC7 File Offset: 0x00000DC7
		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
				this.Refresh();
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002BD6 File Offset: 0x00000DD6
		private void Awake()
		{
			LocalizationManager.OnSetLanguage += new Action<SystemLanguage>(this.OnSetLanguage);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002BE9 File Offset: 0x00000DE9
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BF1 File Offset: 0x00000DF1
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= new Action<SystemLanguage>(this.OnSetLanguage);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C04 File Offset: 0x00000E04
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C0C File Offset: 0x00000E0C
		private void Refresh()
		{
			this.RefreshReferences();
			this.RefreshTexts();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C1A File Offset: 0x00000E1A
		private void RefreshDataModel()
		{
			LocalizationManager.SetLanguage();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C24 File Offset: 0x00000E24
		private void RefreshReferences()
		{
			bool flag = string.IsNullOrEmpty(this.key);
			if (this.text == null)
			{
				this.text = base.GetComponent<Text>();
				if (flag && this.text)
				{
					this.key = this.text.text;
				}
			}
			if (this.tmpText == null)
			{
				this.tmpText = base.GetComponent<TMP_Text>();
				if (flag && this.tmpText)
				{
					this.key = this.tmpText.text;
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002CB4 File Offset: 0x00000EB4
		private void RefreshTexts()
		{
			string text = this.key.ToPlainText();
			if (this.text != null)
			{
				this.text.text = text;
			}
			if (this.tmpText != null)
			{
				this.tmpText.text = text;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D01 File Offset: 0x00000F01
		private void OnValidate()
		{
			this.Refresh();
		}

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private Text text;

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private TMP_Text tmpText;

		// Token: 0x04000015 RID: 21
		[LocalizationKey("UIText")]
		[SerializeField]
		private string key;
	}
}
