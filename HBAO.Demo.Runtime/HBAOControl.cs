using System;
using UnityEngine;
using UnityEngine.UI;

namespace HorizonBasedAmbientOcclusion
{
	// Token: 0x02000003 RID: 3
	public class HBAOControl : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BB File Offset: 0x000002BB
		public void Start()
		{
			this.hbao.SetDebugMode(HBAO.DebugMode.Disabled);
			this.hbao.SetAoRadius(this.aoRadiusSlider.value);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020DF File Offset: 0x000002DF
		public void ToggleShowAO()
		{
			if (this.hbao.generalSettings.debugMode != HBAO.DebugMode.Disabled)
			{
				this.hbao.SetDebugMode(HBAO.DebugMode.Disabled);
				return;
			}
			this.hbao.SetDebugMode(HBAO.DebugMode.AOOnly);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000210C File Offset: 0x0000030C
		public void UpdateAoRadius()
		{
			this.hbao.SetAoRadius(this.aoRadiusSlider.value);
		}

		// Token: 0x04000001 RID: 1
		public HBAO hbao;

		// Token: 0x04000002 RID: 2
		public Slider aoRadiusSlider;
	}
}
