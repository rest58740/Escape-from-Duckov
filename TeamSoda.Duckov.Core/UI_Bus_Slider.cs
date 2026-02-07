using System;
using Duckov;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000207 RID: 519
public class UI_Bus_Slider : MonoBehaviour
{
	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0003DE20 File Offset: 0x0003C020
	private AudioManager.Bus BusRef
	{
		get
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (this.busRef == null)
			{
				this.busRef = AudioManager.GetBus(this.busName);
				if (this.busRef == null)
				{
					Debug.LogError("Bus not found:" + this.busName);
				}
			}
			return this.busRef;
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0003DE74 File Offset: 0x0003C074
	private void Initialize()
	{
		if (this.BusRef == null)
		{
			return;
		}
		this.slider.SetValueWithoutNotify(this.BusRef.Volume);
		this.volumeNumberText.text = (this.BusRef.Volume * 100f).ToString("0");
		this.initialized = true;
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0003DED0 File Offset: 0x0003C0D0
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0003DEEE File Offset: 0x0003C0EE
	private void Start()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x0003DEFE File Offset: 0x0003C0FE
	private void OnEnable()
	{
		this.Initialize();
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0003DF08 File Offset: 0x0003C108
	private void OnValueChanged(float value)
	{
		if (this.BusRef == null)
		{
			return;
		}
		this.BusRef.Volume = value;
		this.BusRef.Mute = (value == 0f);
		this.volumeNumberText.text = (this.BusRef.Volume * 100f).ToString("0");
	}

	// Token: 0x04000CC2 RID: 3266
	private AudioManager.Bus busRef;

	// Token: 0x04000CC3 RID: 3267
	[SerializeField]
	private string busName;

	// Token: 0x04000CC4 RID: 3268
	[SerializeField]
	private TextMeshProUGUI volumeNumberText;

	// Token: 0x04000CC5 RID: 3269
	[SerializeField]
	private Slider slider;

	// Token: 0x04000CC6 RID: 3270
	private bool initialized;
}
