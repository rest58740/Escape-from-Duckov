using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200002E RID: 46
	[ExecuteInEditMode]
	[RequireComponent(typeof(LODGroup))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lodbeamgroup/")]
	public class LODBeamGroup : MonoBehaviour
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00005D5C File Offset: 0x00003F5C
		private void Awake()
		{
			this.m_LODGroup = base.GetComponent<LODGroup>();
			this.SetupLodGroupData();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005D70 File Offset: 0x00003F70
		private void Start()
		{
			this.UnifyBeamsProperties();
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005D78 File Offset: 0x00003F78
		public LOD[] GetLODsFromLODGroup()
		{
			return this.m_LODGroup.GetLODs();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005D85 File Offset: 0x00003F85
		private void SetLODRenderer(int lodIdx, Renderer renderer)
		{
			object renderers;
			if (!renderer)
			{
				renderers = null;
			}
			else
			{
				(renderers = new Renderer[1])[0] = renderer;
			}
			this.SetLODRenderers(lodIdx, renderers);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005DA4 File Offset: 0x00003FA4
		private void SetLODRenderers(int lodIdx, Renderer[] renderers)
		{
			LOD[] lods = this.m_LODGroup.GetLODs();
			lods[lodIdx].renderers = renderers;
			this.m_LODGroup.SetLODs(lods);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005DD8 File Offset: 0x00003FD8
		private void SetLOD(int lodIdx)
		{
			LOD[] lods = this.m_LODGroup.GetLODs();
			if (lods.IsValidIndex(lodIdx))
			{
				BeamGeometryAbstractBase beamGeometry = this.m_LODBeams[lodIdx].GetBeamGeometry();
				if (beamGeometry)
				{
					MeshRenderer meshRenderer = beamGeometry.meshRenderer;
					if (meshRenderer)
					{
						if (this.m_CullVolumetricDustParticles)
						{
							VolumetricDustParticles component = this.m_LODBeams[lodIdx].GetComponent<VolumetricDustParticles>();
							if (component)
							{
								ParticleSystemRenderer particleSystemRenderer = component.FindRenderer();
								if (particleSystemRenderer)
								{
									this.SetLODRenderers(lodIdx, new Renderer[]
									{
										meshRenderer,
										particleSystemRenderer
									});
									return;
								}
							}
						}
						if (lods[lodIdx].renderers == null || lods[lodIdx].renderers.Length != 1 || lods[lodIdx].renderers[0] != meshRenderer)
						{
							this.SetLODRenderer(lodIdx, meshRenderer);
						}
					}
				}
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005EAC File Offset: 0x000040AC
		private void OnBeamGeometryGenerated(VolumetricLightBeamAbstractBase beam)
		{
			if (this.GetLODsFromLODGroup() == null || this.m_LODBeams == null)
			{
				return;
			}
			for (int i = 0; i < this.m_LODBeams.Length; i++)
			{
				if (this.m_LODBeams[i] == beam)
				{
					this.SetLOD(i);
					return;
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005EF8 File Offset: 0x000040F8
		private void SetupLodGroupData()
		{
			if (this.m_LODGroup == null)
			{
				return;
			}
			LOD[] lodsFromLODGroup = this.GetLODsFromLODGroup();
			if (lodsFromLODGroup == null)
			{
				return;
			}
			if (this.m_LODBeams == null || this.m_LODBeams.Length < lodsFromLODGroup.Length)
			{
				Utils.ResizeArray<VolumetricLightBeamAbstractBase>(ref this.m_LODBeams, lodsFromLODGroup.Length);
			}
			for (int i = 0; i < this.m_LODBeams.Length; i++)
			{
				if (this.m_LODBeams[i] == null)
				{
					if (i < lodsFromLODGroup.Length)
					{
						this.SetLODRenderer(i, null);
					}
				}
				else
				{
					this.m_LODBeams[i].RegisterBeamGeometryGeneratedCallback(new VolumetricLightBeamAbstractBase.BeamGeometryGeneratedHandler(this.OnBeamGeometryGenerated));
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005F90 File Offset: 0x00004190
		private void UnifyBeamsProperties()
		{
			if (this.m_LODBeams == null)
			{
				return;
			}
			if (this.m_ResetAllLODsLocalTransform)
			{
				foreach (VolumetricLightBeamAbstractBase volumetricLightBeamAbstractBase in this.m_LODBeams)
				{
					if (volumetricLightBeamAbstractBase)
					{
						volumetricLightBeamAbstractBase.transform.localPosition = Vector3.zero;
						volumetricLightBeamAbstractBase.transform.localRotation = Quaternion.identity;
						volumetricLightBeamAbstractBase.transform.localScale = Vector3.one;
					}
				}
			}
			if (this.m_LOD0PropsToCopy == (BeamProps)0 || this.m_LODBeams.Length <= 1)
			{
				return;
			}
			VolumetricLightBeamAbstractBase volumetricLightBeamAbstractBase2 = this.m_LODBeams[0];
			if (volumetricLightBeamAbstractBase2 == null)
			{
				return;
			}
			for (int j = 1; j < this.m_LODBeams.Length; j++)
			{
				VolumetricLightBeamAbstractBase volumetricLightBeamAbstractBase3 = this.m_LODBeams[j];
				if (volumetricLightBeamAbstractBase3)
				{
					volumetricLightBeamAbstractBase3.CopyPropsFrom(volumetricLightBeamAbstractBase2, this.m_LOD0PropsToCopy);
					UtilsBeamProps.SetColorFromLight(volumetricLightBeamAbstractBase3, false);
					UtilsBeamProps.SetFallOffEndFromLight(volumetricLightBeamAbstractBase3, false);
					UtilsBeamProps.SetIntensityFromLight(volumetricLightBeamAbstractBase3, false);
					UtilsBeamProps.SetSpotAngleFromLight(volumetricLightBeamAbstractBase3, false);
				}
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000607F File Offset: 0x0000427F
		private void Update()
		{
			if (this.m_CopyLOD0PropsEachFrame)
			{
				this.UnifyBeamsProperties();
			}
		}

		// Token: 0x04000108 RID: 264
		[SerializeField]
		private VolumetricLightBeamAbstractBase[] m_LODBeams;

		// Token: 0x04000109 RID: 265
		[SerializeField]
		private bool m_ResetAllLODsLocalTransform;

		// Token: 0x0400010A RID: 266
		[SerializeField]
		private BeamProps m_LOD0PropsToCopy = (BeamProps)(-1);

		// Token: 0x0400010B RID: 267
		[SerializeField]
		private bool m_CopyLOD0PropsEachFrame;

		// Token: 0x0400010C RID: 268
		[SerializeField]
		private bool m_CullVolumetricDustParticles = true;

		// Token: 0x0400010D RID: 269
		private LODGroup m_LODGroup;
	}
}
