using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A5 RID: 421
public class DigitInputPanel : MonoBehaviour
{
	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06000CA7 RID: 3239 RVA: 0x00035E6C File Offset: 0x0003406C
	// (remove) Token: 0x06000CA8 RID: 3240 RVA: 0x00035EA4 File Offset: 0x000340A4
	public event Action<string> onInputFieldValueChanged;

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00035EDC File Offset: 0x000340DC
	public long Value
	{
		get
		{
			string text = this.inputField.text;
			if (string.IsNullOrEmpty(text))
			{
				return 0L;
			}
			long result;
			if (!long.TryParse(text, out result))
			{
				return 0L;
			}
			return result;
		}
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x00035F10 File Offset: 0x00034110
	private void Awake()
	{
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnInputFieldValueChanged));
		for (int i = 0; i < this.numKeys.Length; i++)
		{
			int v = i;
			this.numKeys[i].onClick.AddListener(delegate()
			{
				this.OnNumKeyClicked((long)v);
			});
		}
		this.clearButton.onClick.AddListener(new UnityAction(this.OnClearButtonClicked));
		this.backspaceButton.onClick.AddListener(new UnityAction(this.OnBackspaceButtonClicked));
		this.maximumButton.onClick.AddListener(new UnityAction(this.Max));
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00035FD4 File Offset: 0x000341D4
	private void OnBackspaceButtonClicked()
	{
		if (string.IsNullOrEmpty(this.inputField.text))
		{
			return;
		}
		this.inputField.text = this.inputField.text.Substring(0, this.inputField.text.Length - 1);
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x00036022 File Offset: 0x00034222
	private void OnClearButtonClicked()
	{
		this.inputField.text = string.Empty;
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x00036034 File Offset: 0x00034234
	private void OnNumKeyClicked(long v)
	{
		this.inputField.text = string.Format("{0}{1}", this.inputField.text, v);
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0003605C File Offset: 0x0003425C
	private void OnInputFieldValueChanged(string value)
	{
		long num;
		if (long.TryParse(value, out num) && num == 0L)
		{
			this.inputField.SetTextWithoutNotify(string.Empty);
		}
		Action<string> action = this.onInputFieldValueChanged;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x00036097 File Offset: 0x00034297
	public void Setup(long value, Func<long> maxFunc = null)
	{
		this.maxFunction = maxFunc;
		this.inputField.text = string.Format("{0}", value);
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x000360BC File Offset: 0x000342BC
	public void Max()
	{
		if (this.maxFunction == null)
		{
			return;
		}
		long num = this.maxFunction();
		this.inputField.text = string.Format("{0}", num);
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x000360F9 File Offset: 0x000342F9
	internal void Clear()
	{
		this.inputField.text = string.Empty;
	}

	// Token: 0x04000B08 RID: 2824
	[SerializeField]
	private TMP_InputField inputField;

	// Token: 0x04000B09 RID: 2825
	[SerializeField]
	private Button clearButton;

	// Token: 0x04000B0A RID: 2826
	[SerializeField]
	private Button backspaceButton;

	// Token: 0x04000B0B RID: 2827
	[SerializeField]
	private Button maximumButton;

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private Button[] numKeys;

	// Token: 0x04000B0D RID: 2829
	public Func<long> maxFunction;
}
