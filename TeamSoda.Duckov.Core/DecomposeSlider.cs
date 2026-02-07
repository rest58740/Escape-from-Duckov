using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200020A RID: 522
public class DecomposeSlider : MonoBehaviour
{
	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06000F7E RID: 3966 RVA: 0x0003E04C File Offset: 0x0003C24C
	// (remove) Token: 0x06000F7F RID: 3967 RVA: 0x0003E084 File Offset: 0x0003C284
	public event Action<float> OnValueChangedEvent;

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0003E0B9 File Offset: 0x0003C2B9
	// (set) Token: 0x06000F81 RID: 3969 RVA: 0x0003E0CB File Offset: 0x0003C2CB
	public int Value
	{
		get
		{
			return Mathf.RoundToInt(this.slider.value);
		}
		set
		{
			this.slider.value = (float)value;
			this.valueText.text = value.ToString();
		}
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x0003E0EC File Offset: 0x0003C2EC
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x0003E10A File Offset: 0x0003C30A
	private void OnDestroy()
	{
		this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0003E128 File Offset: 0x0003C328
	private void OnValueChanged(float value)
	{
		this.OnValueChangedEvent(value);
		this.valueText.text = value.ToString();
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x0003E148 File Offset: 0x0003C348
	public void SetMinMax(int min, int max)
	{
		this.slider.minValue = (float)min;
		this.slider.maxValue = (float)max;
		this.minText.text = min.ToString();
		this.maxText.text = max.ToString();
	}

	// Token: 0x04000CC9 RID: 3273
	[SerializeField]
	private Slider slider;

	// Token: 0x04000CCA RID: 3274
	public TextMeshProUGUI minText;

	// Token: 0x04000CCB RID: 3275
	public TextMeshProUGUI maxText;

	// Token: 0x04000CCC RID: 3276
	public TextMeshProUGUI valueText;
}
