using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000043 RID: 67
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamAbstractBase))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-triggerzone/")]
	[AddComponentMenu("VLB/Common/Trigger Zone")]
	public class TriggerZone : MonoBehaviour
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00009D51 File Offset: 0x00007F51
		private TriggerZone.TriggerZoneUpdateRate updateRate
		{
			get
			{
				if (UtilsBeamProps.GetDimensions(this.m_Beam) == Dimensions.Dim3D)
				{
					return TriggerZone.TriggerZoneUpdateRate.OnEnable;
				}
				if (!(this.m_DynamicOcclusionRaycasting != null))
				{
					return TriggerZone.TriggerZoneUpdateRate.OnEnable;
				}
				return TriggerZone.TriggerZoneUpdateRate.OnOcclusionChange;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009D74 File Offset: 0x00007F74
		private void OnEnable()
		{
			this.m_Beam = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.m_DynamicOcclusionRaycasting = base.GetComponent<DynamicOcclusionRaycasting>();
			TriggerZone.TriggerZoneUpdateRate updateRate = this.updateRate;
			if (updateRate == TriggerZone.TriggerZoneUpdateRate.OnEnable)
			{
				this.ComputeZone();
				base.enabled = false;
				return;
			}
			if (updateRate != TriggerZone.TriggerZoneUpdateRate.OnOcclusionChange)
			{
				return;
			}
			if (this.m_DynamicOcclusionRaycasting)
			{
				this.m_DynamicOcclusionRaycasting.onOcclusionProcessed += this.OnOcclusionProcessed;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009DDA File Offset: 0x00007FDA
		private void OnOcclusionProcessed()
		{
			this.ComputeZone();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009DE4 File Offset: 0x00007FE4
		private void ComputeZone()
		{
			if (this.m_Beam)
			{
				float coneRadiusStart = UtilsBeamProps.GetConeRadiusStart(this.m_Beam);
				float num = UtilsBeamProps.GetFallOffEnd(this.m_Beam) * this.rangeMultiplier;
				float num2 = Mathf.LerpUnclamped(coneRadiusStart, UtilsBeamProps.GetConeRadiusEnd(this.m_Beam), this.rangeMultiplier);
				if (UtilsBeamProps.GetDimensions(this.m_Beam) == Dimensions.Dim3D)
				{
					MeshCollider orAddComponent = base.gameObject.GetOrAddComponent<MeshCollider>();
					Mathf.Min(UtilsBeamProps.GetGeomSides(this.m_Beam), 8);
					Mesh mesh = MeshGenerator.GenerateConeZ_Radii_DoubleCaps(num, coneRadiusStart, num2, 8, false);
					mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
					orAddComponent.sharedMesh = mesh;
					orAddComponent.convex = this.setIsTrigger;
					orAddComponent.isTrigger = this.setIsTrigger;
					return;
				}
				if (this.m_PolygonCollider2D == null)
				{
					this.m_PolygonCollider2D = base.gameObject.GetOrAddComponent<PolygonCollider2D>();
				}
				Vector2[] array = new Vector2[]
				{
					new Vector2(0f, -coneRadiusStart),
					new Vector2(num, -num2),
					new Vector2(num, num2),
					new Vector2(0f, coneRadiusStart)
				};
				if (this.m_DynamicOcclusionRaycasting && this.m_DynamicOcclusionRaycasting.planeEquationWS.IsValid())
				{
					Plane planeEquationWS = this.m_DynamicOcclusionRaycasting.planeEquationWS;
					if (Utils.IsAlmostZero(planeEquationWS.normal.z))
					{
						Vector3 vector = planeEquationWS.ClosestPointOnPlaneCustom(Vector3.zero);
						Vector3 vector2 = planeEquationWS.ClosestPointOnPlaneCustom(Vector3.up);
						if (Utils.IsAlmostZero(Vector3.SqrMagnitude(vector - vector2)))
						{
							vector = planeEquationWS.ClosestPointOnPlaneCustom(Vector3.right);
						}
						vector = base.transform.InverseTransformPoint(vector);
						vector2 = base.transform.InverseTransformPoint(vector2);
						PolygonHelper.Plane2D plane2D = PolygonHelper.Plane2D.FromPoints(vector, vector2);
						if (plane2D.normal.x > 0f)
						{
							plane2D.Flip();
						}
						array = plane2D.CutConvex(array);
					}
				}
				this.m_PolygonCollider2D.points = array;
				this.m_PolygonCollider2D.isTrigger = this.setIsTrigger;
			}
		}

		// Token: 0x04000194 RID: 404
		public const string ClassName = "TriggerZone";

		// Token: 0x04000195 RID: 405
		public bool setIsTrigger = true;

		// Token: 0x04000196 RID: 406
		public float rangeMultiplier = 1f;

		// Token: 0x04000197 RID: 407
		private const int kMeshColliderNumSides = 8;

		// Token: 0x04000198 RID: 408
		private VolumetricLightBeamAbstractBase m_Beam;

		// Token: 0x04000199 RID: 409
		private DynamicOcclusionRaycasting m_DynamicOcclusionRaycasting;

		// Token: 0x0400019A RID: 410
		private PolygonCollider2D m_PolygonCollider2D;

		// Token: 0x020000C6 RID: 198
		private enum TriggerZoneUpdateRate
		{
			// Token: 0x0400040D RID: 1037
			OnEnable,
			// Token: 0x0400040E RID: 1038
			OnOcclusionChange
		}
	}
}
