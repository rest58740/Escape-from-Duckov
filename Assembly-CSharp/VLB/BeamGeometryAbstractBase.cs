using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000009 RID: 9
	public abstract class BeamGeometryAbstractBase : MonoBehaviour
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002DAD File Offset: 0x00000FAD
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002DB5 File Offset: 0x00000FB5
		public MeshRenderer meshRenderer { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002DBE File Offset: 0x00000FBE
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002DC6 File Offset: 0x00000FC6
		public MeshFilter meshFilter { get; protected set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002DCF File Offset: 0x00000FCF
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002DD7 File Offset: 0x00000FD7
		public Mesh coneMesh { get; protected set; }

		// Token: 0x06000027 RID: 39
		protected abstract VolumetricLightBeamAbstractBase GetMaster();

		// Token: 0x06000028 RID: 40 RVA: 0x00002DE0 File Offset: 0x00000FE0
		private void Start()
		{
			this.DestroyOrphanBeamGeom();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DE8 File Offset: 0x00000FE8
		private void OnDestroy()
		{
			if (this.m_CustomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_CustomMaterial);
				this.m_CustomMaterial = null;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E0C File Offset: 0x0000100C
		private void DestroyOrphanBeamGeom()
		{
			VolumetricLightBeamAbstractBase master = this.GetMaster();
			if (master && master.GetBeamGeometry() == this)
			{
				return;
			}
			BeamGeometryAbstractBase.DestroyBeamGeometryGameObject(this);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E3D File Offset: 0x0000103D
		public static void DestroyBeamGeometryGameObject(BeamGeometryAbstractBase beamGeom)
		{
			if (beamGeom)
			{
				UnityEngine.Object.DestroyImmediate(beamGeom.gameObject);
			}
		}

		// Token: 0x04000015 RID: 21
		protected Matrix4x4 m_ColorGradientMatrix;

		// Token: 0x04000016 RID: 22
		protected Material m_CustomMaterial;
	}
}
