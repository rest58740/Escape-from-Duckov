using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000008 RID: 8
	public class PlatformPreset : MonoBehaviour
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002534 File Offset: 0x00000734
		private void Start()
		{
			Slider component = GameObject.Find("Size Slider").GetComponent<Slider>();
			Slider component2 = GameObject.Find("Iteration Slider").GetComponent<Slider>();
			Slider component3 = GameObject.Find("Downsample Slider").GetComponent<Slider>();
			Slider component4 = GameObject.Find("Max update rate Slider").GetComponent<Slider>();
			foreach (Preset preset in this.presets)
			{
				if (preset.platform == Application.platform)
				{
					component.value = preset.size;
					component2.value = (float)preset.iteration;
					component3.value = (float)preset.downsample;
					component4.value = preset.maxUpdateRate;
				}
			}
		}

		// Token: 0x04000019 RID: 25
		public Preset[] presets;
	}
}
