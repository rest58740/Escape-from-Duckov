using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x0200000D RID: 13
	[AddComponentMenu("")]
	public class EffectAbstractBase : MonoBehaviour
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000356D File Offset: 0x0000176D
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003575 File Offset: 0x00001775
		[Obsolete("Use 'restoreIntensityOnDisable' instead")]
		public bool restoreBaseIntensity
		{
			get
			{
				return this.restoreIntensityOnDisable;
			}
			set
			{
				this.restoreIntensityOnDisable = value;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000357E File Offset: 0x0000177E
		public virtual void InitFrom(EffectAbstractBase Source)
		{
			if (Source)
			{
				this.componentsToChange = Source.componentsToChange;
				this.restoreIntensityOnDisable = Source.restoreIntensityOnDisable;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000035A0 File Offset: 0x000017A0
		private void GetIntensity(VolumetricLightBeamSD beam)
		{
			if (beam)
			{
				this.m_BaseIntensityBeamInside = beam.intensityInside;
				this.m_BaseIntensityBeamOutside = beam.intensityOutside;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000035C2 File Offset: 0x000017C2
		private void GetIntensity(VolumetricLightBeamHD beam)
		{
			if (beam)
			{
				this.m_BaseIntensityBeamOutside = beam.intensity;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000035D8 File Offset: 0x000017D8
		private void SetIntensity(VolumetricLightBeamSD beam, float additive)
		{
			if (beam)
			{
				beam.intensityInside = Mathf.Max(0f, this.m_BaseIntensityBeamInside + additive);
				beam.intensityOutside = Mathf.Max(0f, this.m_BaseIntensityBeamOutside + additive);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003612 File Offset: 0x00001812
		private void SetIntensity(VolumetricLightBeamHD beam, float additive)
		{
			if (beam)
			{
				beam.intensity = Mathf.Max(0f, this.m_BaseIntensityBeamOutside + additive);
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003634 File Offset: 0x00001834
		protected void SetAdditiveIntensity(float additive)
		{
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.VolumetricLightBeam) && this.m_Beam)
			{
				this.SetIntensity(this.m_Beam as VolumetricLightBeamSD, additive);
				this.SetIntensity(this.m_Beam as VolumetricLightBeamHD, additive);
			}
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.UnityLight) && this.m_Light)
			{
				this.m_Light.intensity = Mathf.Max(0f, this.m_BaseIntensityLight + additive);
			}
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.VolumetricDustParticles) && this.m_Particles)
			{
				this.m_Particles.alphaAdditionalRuntime = 1f + additive;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003704 File Offset: 0x00001904
		private void Awake()
		{
			this.m_Beam = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.m_Light = base.GetComponent<Light>();
			this.m_Particles = base.GetComponent<VolumetricDustParticles>();
			this.GetIntensity(this.m_Beam as VolumetricLightBeamSD);
			this.GetIntensity(this.m_Beam as VolumetricLightBeamHD);
			this.m_BaseIntensityLight = (this.m_Light ? this.m_Light.intensity : 0f);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000377C File Offset: 0x0000197C
		protected virtual void OnEnable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003784 File Offset: 0x00001984
		private void OnDisable()
		{
			base.StopAllCoroutines();
			if (this.restoreIntensityOnDisable)
			{
				this.SetAdditiveIntensity(0f);
			}
		}

		// Token: 0x04000040 RID: 64
		public const string ClassName = "EffectAbstractBase";

		// Token: 0x04000041 RID: 65
		public EffectAbstractBase.ComponentsToChange componentsToChange = (EffectAbstractBase.ComponentsToChange)2147483647;

		// Token: 0x04000042 RID: 66
		[FormerlySerializedAs("restoreBaseIntensity")]
		public bool restoreIntensityOnDisable = true;

		// Token: 0x04000043 RID: 67
		protected VolumetricLightBeamAbstractBase m_Beam;

		// Token: 0x04000044 RID: 68
		protected Light m_Light;

		// Token: 0x04000045 RID: 69
		protected VolumetricDustParticles m_Particles;

		// Token: 0x04000046 RID: 70
		protected float m_BaseIntensityBeamInside;

		// Token: 0x04000047 RID: 71
		protected float m_BaseIntensityBeamOutside;

		// Token: 0x04000048 RID: 72
		protected float m_BaseIntensityLight;

		// Token: 0x020000A0 RID: 160
		[Flags]
		public enum ComponentsToChange
		{
			// Token: 0x04000373 RID: 883
			UnityLight = 1,
			// Token: 0x04000374 RID: 884
			VolumetricLightBeam = 2,
			// Token: 0x04000375 RID: 885
			VolumetricDustParticles = 4
		}
	}
}
