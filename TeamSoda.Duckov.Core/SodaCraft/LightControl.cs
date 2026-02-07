using System;
using Umbra;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x0200043D RID: 1085
	[VolumeComponentMenu("SodaCraft/LightControl")]
	[Serializable]
	public class LightControl : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600270C RID: 9996 RVA: 0x00087203 File Offset: 0x00085403
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x00087210 File Offset: 0x00085410
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x00087214 File Offset: 0x00085414
		public override void Override(VolumeComponent state, float interpFactor)
		{
			LightControl lightControl = state as LightControl;
			base.Override(state, interpFactor);
			RenderSettings.ambientSkyColor = lightControl.skyColor.value;
			RenderSettings.ambientEquatorColor = lightControl.equatorColor.value;
			RenderSettings.ambientGroundColor = lightControl.groundColor.value;
			Shader.SetGlobalColor(this.fowColorID, lightControl.fowColor.value);
			Shader.SetGlobalColor(this.SodaPointLight_EnviromentTintID, lightControl.SodaLightTint.value);
			if (!LightControl.light)
			{
				LightControl.light = RenderSettings.sun;
			}
			if (LightControl.light)
			{
				LightControl.light.color = lightControl.sunColor.value;
				LightControl.light.intensity = lightControl.sunIntensity.value;
				LightControl.light.transform.rotation = Quaternion.Euler(lightControl.sunRotation.value);
				if (!LightControl.lightShadows)
				{
					LightControl.lightShadows = LightControl.light.GetComponent<UmbraSoftShadows>();
				}
				if (LightControl.lightShadows)
				{
					float value = lightControl.sunShadowHardness.value;
					LightControl.lightShadows.profile.contactStrength = value;
				}
			}
		}

		// Token: 0x04001AAA RID: 6826
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001AAB RID: 6827
		public ColorParameter skyColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x04001AAC RID: 6828
		public ColorParameter equatorColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x04001AAD RID: 6829
		public ColorParameter groundColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x04001AAE RID: 6830
		public ColorParameter sunColor = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x04001AAF RID: 6831
		public ColorParameter fowColor = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x04001AB0 RID: 6832
		public MinFloatParameter sunIntensity = new MinFloatParameter(1f, 0f, false);

		// Token: 0x04001AB1 RID: 6833
		public ClampedFloatParameter sunShadowHardness = new ClampedFloatParameter(0.96f, 0f, 1f, false);

		// Token: 0x04001AB2 RID: 6834
		public Vector3Parameter sunRotation = new Vector3Parameter(new Vector3(59f, 168f, 0f), false);

		// Token: 0x04001AB3 RID: 6835
		public ColorParameter SodaLightTint = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x04001AB4 RID: 6836
		private int SodaPointLight_EnviromentTintID = Shader.PropertyToID("SodaPointLight_EnviromentTint");

		// Token: 0x04001AB5 RID: 6837
		private int fowColorID = Shader.PropertyToID("_SodaUnknowColor");

		// Token: 0x04001AB6 RID: 6838
		private static Light light;

		// Token: 0x04001AB7 RID: 6839
		private static UmbraSoftShadows lightShadows;
	}
}
