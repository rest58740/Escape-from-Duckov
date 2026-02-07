using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x0200000C RID: 12
	[RequireComponent(typeof(Slider))]
	public class SliderChangeOnStart : MonoBehaviour
	{
		// Token: 0x06000025 RID: 37 RVA: 0x0000290F File Offset: 0x00000B0F
		private void Start()
		{
			this.slider = base.GetComponent<Slider>();
			this.slider.value -= 1f;
			this.slider.value += 1f;
		}

		// Token: 0x04000028 RID: 40
		private Slider slider;
	}
}
