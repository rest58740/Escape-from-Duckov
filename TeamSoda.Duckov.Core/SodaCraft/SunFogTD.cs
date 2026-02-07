using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x0200043A RID: 1082
	[VolumeComponentMenu("SodaCraft/SunFogTD")]
	[Serializable]
	public class SunFogTD : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06002701 RID: 9985 RVA: 0x00086D11 File Offset: 0x00084F11
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x00086D1E File Offset: 0x00084F1E
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x00086D24 File Offset: 0x00084F24
		public override void Override(VolumeComponent state, float interpFactor)
		{
			SunFogTD sunFogTD = state as SunFogTD;
			base.Override(state, interpFactor);
			Shader.SetGlobalColor(this.fogColorHash, sunFogTD.fogColor.value);
			Shader.SetGlobalColor(this.sunColorHash, sunFogTD.sunColor.value);
			Shader.SetGlobalFloat(this.nearDistanceHash, sunFogTD.clipPlanes.value.x);
			Shader.SetGlobalFloat(this.farDistanceHash, sunFogTD.clipPlanes.value.y);
			Shader.SetGlobalFloat(this.sunSizeHash, sunFogTD.sunSize.value);
			Shader.SetGlobalFloat(this.sunPowerHash, sunFogTD.sunPower.value);
			Shader.SetGlobalVector(this.sunPointHash, sunFogTD.sunPoint.value);
			Shader.SetGlobalFloat(this.sunAlphaGainHash, sunFogTD.sunAlphaGain.value);
		}

		// Token: 0x04001A89 RID: 6793
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A8A RID: 6794
		public ColorParameter fogColor = new ColorParameter(new Color(0.68718916f, 1.070217f, 1.3615336f, 0f), true, true, false, false);

		// Token: 0x04001A8B RID: 6795
		public ColorParameter sunColor = new ColorParameter(new Color(4.061477f, 2.5092788f, 1.7816858f, 0f), true, true, false, false);

		// Token: 0x04001A8C RID: 6796
		public FloatRangeParameter clipPlanes = new FloatRangeParameter(new Vector2(41f, 72f), 0.3f, 1000f, false);

		// Token: 0x04001A8D RID: 6797
		public Vector2Parameter sunPoint = new Vector2Parameter(new Vector2(-2.63f, 1.23f), false);

		// Token: 0x04001A8E RID: 6798
		public FloatParameter sunSize = new ClampedFloatParameter(1.85f, 0f, 10f, false);

		// Token: 0x04001A8F RID: 6799
		public ClampedFloatParameter sunPower = new ClampedFloatParameter(1f, 0.1f, 10f, false);

		// Token: 0x04001A90 RID: 6800
		public ClampedFloatParameter sunAlphaGain = new ClampedFloatParameter(0.001f, 0f, 0.25f, false);

		// Token: 0x04001A91 RID: 6801
		private int fogColorHash = Shader.PropertyToID("SunFogColor");

		// Token: 0x04001A92 RID: 6802
		private int sunColorHash = Shader.PropertyToID("SunFogSunColor");

		// Token: 0x04001A93 RID: 6803
		private int nearDistanceHash = Shader.PropertyToID("SunFogNearDistance");

		// Token: 0x04001A94 RID: 6804
		private int farDistanceHash = Shader.PropertyToID("SunFogFarDistance");

		// Token: 0x04001A95 RID: 6805
		private int sunPointHash = Shader.PropertyToID("SunFogSunPoint");

		// Token: 0x04001A96 RID: 6806
		private int sunSizeHash = Shader.PropertyToID("SunFogSunSize");

		// Token: 0x04001A97 RID: 6807
		private int sunPowerHash = Shader.PropertyToID("SunFogSunPower");

		// Token: 0x04001A98 RID: 6808
		private int sunAlphaGainHash = Shader.PropertyToID("SunFogSunAplhaGain");
	}
}
