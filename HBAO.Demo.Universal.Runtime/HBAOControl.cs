using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace HorizonBasedAmbientOcclusion.Universal
{
	// Token: 0x02000003 RID: 3
	public class HBAOControl : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public void Start()
		{
			HBAO hbao;
			this.postProcessProfile.TryGet<HBAO>(out hbao);
			if (hbao != null)
			{
				hbao.EnableHBAO(true);
				hbao.SetDebugMode(HBAO.DebugMode.Disabled);
				hbao.SetAoRadius(this.aoRadiusSlider.value);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
		public void ToggleHBAO()
		{
			HBAO hbao;
			this.postProcessProfile.TryGet<HBAO>(out hbao);
			if (hbao != null)
			{
				this.m_HbaoDisplayed = !this.m_HbaoDisplayed;
				hbao.EnableHBAO(this.m_HbaoDisplayed);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002140 File Offset: 0x00000340
		public void ToggleShowAO()
		{
			HBAO hbao;
			this.postProcessProfile.TryGet<HBAO>(out hbao);
			if (hbao != null)
			{
				if (hbao.GetDebugMode() != HBAO.DebugMode.Disabled)
				{
					hbao.SetDebugMode(HBAO.DebugMode.Disabled);
					return;
				}
				hbao.SetDebugMode(HBAO.DebugMode.AOOnly);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000217C File Offset: 0x0000037C
		public void UpdateAoRadius()
		{
			HBAO hbao;
			this.postProcessProfile.TryGet<HBAO>(out hbao);
			if (hbao != null)
			{
				hbao.SetAoRadius(this.aoRadiusSlider.value);
			}
		}

		// Token: 0x04000001 RID: 1
		public VolumeProfile postProcessProfile;

		// Token: 0x04000002 RID: 2
		public Slider aoRadiusSlider;

		// Token: 0x04000003 RID: 3
		private bool m_HbaoDisplayed = true;
	}
}
