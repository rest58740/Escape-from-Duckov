using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000048 RID: 72
	public abstract class VolumetricLightBeamAbstractBase : MonoBehaviour
	{
		// Token: 0x060002C3 RID: 707
		public abstract BeamGeometryAbstractBase GetBeamGeometry();

		// Token: 0x060002C4 RID: 708
		protected abstract void SetBeamGeometryNull();

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000B59C File Offset: 0x0000979C
		public bool hasGeometry
		{
			get
			{
				return this.GetBeamGeometry() != null;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000B5AA File Offset: 0x000097AA
		public Bounds bounds
		{
			get
			{
				if (!(this.GetBeamGeometry() != null))
				{
					return new Bounds(Vector3.zero, Vector3.zero);
				}
				return this.GetBeamGeometry().meshRenderer.bounds;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002C7 RID: 711 RVA: 0x0000B5DC File Offset: 0x000097DC
		// (remove) Token: 0x060002C8 RID: 712 RVA: 0x0000B614 File Offset: 0x00009814
		private event VolumetricLightBeamAbstractBase.BeamGeometryGeneratedHandler BeamGeometryGeneratedEvent;

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B649 File Offset: 0x00009849
		public void RegisterBeamGeometryGeneratedCallback(VolumetricLightBeamAbstractBase.BeamGeometryGeneratedHandler callback)
		{
			if (this.hasGeometry)
			{
				callback(this);
				return;
			}
			this.BeamGeometryGeneratedEvent += callback;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000B662 File Offset: 0x00009862
		public virtual void GenerateGeometry()
		{
			if (this.BeamGeometryGeneratedEvent != null)
			{
				this.BeamGeometryGeneratedEvent(this);
				this.BeamGeometryGeneratedEvent = null;
			}
		}

		// Token: 0x060002CB RID: 715
		public abstract bool IsScalable();

		// Token: 0x060002CC RID: 716
		public abstract Vector3 GetLossyScale();

		// Token: 0x060002CD RID: 717 RVA: 0x0000B680 File Offset: 0x00009880
		public virtual void CopyPropsFrom(VolumetricLightBeamAbstractBase beamSrc, BeamProps beamProps)
		{
			if (beamProps.HasFlag(BeamProps.Transform))
			{
				base.transform.position = beamSrc.transform.position;
				base.transform.rotation = beamSrc.transform.rotation;
				base.transform.localScale = beamSrc.transform.localScale;
			}
			if (beamProps.HasFlag(BeamProps.SideSoftness))
			{
				UtilsBeamProps.SetThickness(this, UtilsBeamProps.GetThickness(beamSrc));
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000B702 File Offset: 0x00009902
		public int _INTERNAL_pluginVersion
		{
			get
			{
				return this.pluginVersion;
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000B70C File Offset: 0x0000990C
		public Light GetLightSpotAttachedSlow(out VolumetricLightBeamAbstractBase.AttachedLightType lightType)
		{
			Light component = base.GetComponent<Light>();
			if (!component)
			{
				lightType = VolumetricLightBeamAbstractBase.AttachedLightType.NoLight;
				return null;
			}
			if (component.type == LightType.Spot)
			{
				lightType = VolumetricLightBeamAbstractBase.AttachedLightType.SpotLight;
				return component;
			}
			lightType = VolumetricLightBeamAbstractBase.AttachedLightType.OtherLight;
			return null;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000B73E File Offset: 0x0000993E
		public Light lightSpotAttached
		{
			get
			{
				return this.m_CachedLightSpot;
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B748 File Offset: 0x00009948
		protected void InitLightSpotAttachedCached()
		{
			VolumetricLightBeamAbstractBase.AttachedLightType attachedLightType;
			this.m_CachedLightSpot = this.GetLightSpotAttachedSlow(out attachedLightType);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B763 File Offset: 0x00009963
		private void OnDestroy()
		{
			this.DestroyBeam();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000B76B File Offset: 0x0000996B
		protected void DestroyBeam()
		{
			if (Application.isPlaying)
			{
				BeamGeometryAbstractBase.DestroyBeamGeometryGameObject(this.GetBeamGeometry());
			}
			this.SetBeamGeometryNull();
		}

		// Token: 0x040001B3 RID: 435
		public const string ClassName = "VolumetricLightBeamAbstractBase";

		// Token: 0x040001B5 RID: 437
		[SerializeField]
		protected int pluginVersion = -1;

		// Token: 0x040001B6 RID: 438
		protected Light m_CachedLightSpot;

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x060004FB RID: 1275
		public delegate void BeamGeometryGeneratedHandler(VolumetricLightBeamAbstractBase beam);

		// Token: 0x020000CA RID: 202
		public enum AttachedLightType
		{
			// Token: 0x04000416 RID: 1046
			NoLight,
			// Token: 0x04000417 RID: 1047
			OtherLight,
			// Token: 0x04000418 RID: 1048
			SpotLight
		}
	}
}
