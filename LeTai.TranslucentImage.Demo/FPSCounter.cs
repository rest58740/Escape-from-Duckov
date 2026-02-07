using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000007 RID: 7
	[RequireComponent(typeof(Text))]
	public class FPSCounter : MonoBehaviour
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002475 File Offset: 0x00000675
		private void Start()
		{
			this.m_FpsNextPeriod = Time.realtimeSinceStartup + 0.5f;
			this.m_Text = base.GetComponent<Text>();
			this.display = this.m_Text.text;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024A8 File Offset: 0x000006A8
		private void Update()
		{
			this.m_FpsAccumulator++;
			if (Time.realtimeSinceStartup > this.m_FpsNextPeriod)
			{
				this.m_CurrentFps = (int)((float)this.m_FpsAccumulator / 0.5f);
				this.m_FpsAccumulator = 0;
				this.m_FpsNextPeriod += 0.5f;
				this.m_Text.text = string.Format(this.display, this.m_CurrentFps);
			}
		}

		// Token: 0x04000013 RID: 19
		private const float fpsMeasurePeriod = 0.5f;

		// Token: 0x04000014 RID: 20
		private int m_FpsAccumulator;

		// Token: 0x04000015 RID: 21
		private float m_FpsNextPeriod;

		// Token: 0x04000016 RID: 22
		private int m_CurrentFps;

		// Token: 0x04000017 RID: 23
		private string display = "{0} FPS";

		// Token: 0x04000018 RID: 24
		private Text m_Text;
	}
}
