using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x0200043C RID: 1084
	[VolumeComponentMenu("SodaCraft/EdgeLight")]
	[Serializable]
	public class EdgeLight : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06002708 RID: 9992 RVA: 0x0008702D File Offset: 0x0008522D
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x0008703A File Offset: 0x0008523A
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x00087040 File Offset: 0x00085240
		public override void Override(VolumeComponent state, float interpFactor)
		{
			EdgeLight edgeLight = state as EdgeLight;
			base.Override(state, interpFactor);
			Shader.SetGlobalVector(this.edgeLightDirectionHash, edgeLight.direction.value);
			Shader.SetGlobalFloat(this.widthHash, edgeLight.edgeLightWidth.value);
			Shader.SetGlobalFloat(this.fixHash, edgeLight.edgeLightFix.value);
			Shader.SetGlobalFloat(this.clampDistanceHash, edgeLight.EdgeLightClampDistance.value);
			Shader.SetGlobalColor(this.colorHash, edgeLight.edgeLightColor.value);
			Shader.SetGlobalFloat(this.edgeLightBlendScreenColorHash, edgeLight.blendScreenColor.value);
		}

		// Token: 0x04001A9D RID: 6813
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A9E RID: 6814
		public Vector2Parameter direction = new Vector2Parameter(new Vector2(-1f, 1f), false);

		// Token: 0x04001A9F RID: 6815
		public ClampedFloatParameter edgeLightWidth = new ClampedFloatParameter(0.001f, 0f, 0.05f, false);

		// Token: 0x04001AA0 RID: 6816
		public ClampedFloatParameter edgeLightFix = new ClampedFloatParameter(0.001f, 0f, 0.05f, false);

		// Token: 0x04001AA1 RID: 6817
		public FloatParameter EdgeLightClampDistance = new ClampedFloatParameter(0.001f, 0.001f, 1f, false);

		// Token: 0x04001AA2 RID: 6818
		public ColorParameter edgeLightColor = new ColorParameter(Color.white, true, false, false, false);

		// Token: 0x04001AA3 RID: 6819
		public FloatParameter blendScreenColor = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x04001AA4 RID: 6820
		private int edgeLightDirectionHash = Shader.PropertyToID("_EdgeLightDirection");

		// Token: 0x04001AA5 RID: 6821
		private int widthHash = Shader.PropertyToID("_EdgeLightWidth");

		// Token: 0x04001AA6 RID: 6822
		private int colorHash = Shader.PropertyToID("_EdgeLightColor");

		// Token: 0x04001AA7 RID: 6823
		private int fixHash = Shader.PropertyToID("_EdgeLightFix");

		// Token: 0x04001AA8 RID: 6824
		private int clampDistanceHash = Shader.PropertyToID("_EdgeLightClampDistance");

		// Token: 0x04001AA9 RID: 6825
		private int edgeLightBlendScreenColorHash = Shader.PropertyToID("_EdgeLightBlendScreenColor");
	}
}
