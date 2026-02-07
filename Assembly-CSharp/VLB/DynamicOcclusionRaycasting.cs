using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x0200003A RID: 58
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-raycasting/")]
	[AddComponentMenu("VLB/SD/Dynamic Occlusion (Raycasting)")]
	public class DynamicOcclusionRaycasting : DynamicOcclusionAbstractBase
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00008355 File Offset: 0x00006555
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000835D File Offset: 0x0000655D
		[Obsolete("Use 'fadeDistanceToSurface' instead")]
		public float fadeDistanceToPlane
		{
			get
			{
				return this.fadeDistanceToSurface;
			}
			set
			{
				this.fadeDistanceToSurface = value;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008366 File Offset: 0x00006566
		public bool IsColliderHiddenByDynamicOccluder(Collider collider)
		{
			return this.planeEquationWS.IsValid() && !GeometryUtility.TestPlanesAABB(new Plane[]
			{
				this.planeEquationWS
			}, collider.bounds);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008398 File Offset: 0x00006598
		protected override string GetShaderKeyword()
		{
			return "VLB_OCCLUSION_CLIPPING_PLANE";
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000839F File Offset: 0x0000659F
		protected override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
		{
			return MaterialManager.SD.DynamicOcclusion.ClippingPlane;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000083A2 File Offset: 0x000065A2
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x000083AA File Offset: 0x000065AA
		public Plane planeEquationWS { get; private set; }

		// Token: 0x060001D7 RID: 471 RVA: 0x000083B3 File Offset: 0x000065B3
		protected override void OnValidateProperties()
		{
			base.OnValidateProperties();
			this.minOccluderArea = Mathf.Max(this.minOccluderArea, 0f);
			this.fadeDistanceToSurface = Mathf.Max(this.fadeDistanceToSurface, 0f);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000083E7 File Offset: 0x000065E7
		protected override void OnEnablePostValidate()
		{
			this.m_CurrentHit.SetNull();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000083F4 File Offset: 0x000065F4
		protected override void OnDisable()
		{
			base.OnDisable();
			this.SetHitNull();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008404 File Offset: 0x00006604
		private void Start()
		{
			if (Application.isPlaying)
			{
				TriggerZone component = base.GetComponent<TriggerZone>();
				if (component)
				{
					this.m_RangeMultiplier = Mathf.Max(1f, component.rangeMultiplier);
				}
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008440 File Offset: 0x00006640
		private Vector3 GetRandomVectorAround(Vector3 direction, float angleDiff)
		{
			float num = angleDiff * 0.5f;
			return Quaternion.Euler(UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num)) * direction;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00008478 File Offset: 0x00006678
		private QueryTriggerInteraction queryTriggerInteraction
		{
			get
			{
				if (!this.considerTriggers)
				{
					return QueryTriggerInteraction.Ignore;
				}
				return QueryTriggerInteraction.Collide;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00008485 File Offset: 0x00006685
		private float raycastMaxDistance
		{
			get
			{
				return this.m_Master.raycastDistance * this.m_RangeMultiplier * this.m_Master.GetLossyScale().z;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000084AA File Offset: 0x000066AA
		private DynamicOcclusionRaycasting.HitResult GetBestHit(Vector3 rayPos, Vector3 rayDir)
		{
			if (this.dimensions != Dimensions.Dim2D)
			{
				return this.GetBestHit3D(rayPos, rayDir);
			}
			return this.GetBestHit2D(rayPos, rayDir);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000084C8 File Offset: 0x000066C8
		private DynamicOcclusionRaycasting.HitResult GetBestHit3D(Vector3 rayPos, Vector3 rayDir)
		{
			RaycastHit[] array = Physics.RaycastAll(rayPos, rayDir, this.raycastMaxDistance, this.layerMask.value, this.queryTriggerInteraction);
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].collider.gameObject != this.m_Master.gameObject && array[i].collider.bounds.GetMaxArea2D() >= this.minOccluderArea && array[i].distance < num2)
				{
					num2 = array[i].distance;
					num = i;
				}
			}
			if (num != -1)
			{
				return new DynamicOcclusionRaycasting.HitResult(ref array[num]);
			}
			return default(DynamicOcclusionRaycasting.HitResult);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008584 File Offset: 0x00006784
		private DynamicOcclusionRaycasting.HitResult GetBestHit2D(Vector3 rayPos, Vector3 rayDir)
		{
			RaycastHit2D[] array = Physics2D.RaycastAll(new Vector2(rayPos.x, rayPos.y), new Vector2(rayDir.x, rayDir.y), this.raycastMaxDistance, this.layerMask.value);
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if ((this.considerTriggers || !array[i].collider.isTrigger) && array[i].collider.gameObject != this.m_Master.gameObject && array[i].collider.bounds.GetMaxArea2D() >= this.minOccluderArea && array[i].distance < num2)
				{
					num2 = array[i].distance;
					num = i;
				}
			}
			if (num != -1)
			{
				return new DynamicOcclusionRaycasting.HitResult(ref array[num]);
			}
			return default(DynamicOcclusionRaycasting.HitResult);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008678 File Offset: 0x00006878
		private uint GetDirectionCount()
		{
			if (this.dimensions != Dimensions.Dim2D)
			{
				return 4U;
			}
			return 2U;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008688 File Offset: 0x00006888
		private Vector3 GetDirection(uint dirInt)
		{
			dirInt %= this.GetDirectionCount();
			switch (dirInt)
			{
			case 0U:
				return this.m_Master.raycastGlobalUp;
			case 1U:
				return -this.m_Master.raycastGlobalUp;
			case 2U:
				return -this.m_Master.raycastGlobalRight;
			case 3U:
				return this.m_Master.raycastGlobalRight;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000086F6 File Offset: 0x000068F6
		private bool IsHitValid(ref DynamicOcclusionRaycasting.HitResult hit, Vector3 forwardVec)
		{
			return hit.hasCollider && Vector3.Dot(hit.normal, -forwardVec) >= this.maxSurfaceDot;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008720 File Offset: 0x00006920
		protected override bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			Vector3 raycastGlobalForward = this.m_Master.raycastGlobalForward;
			DynamicOcclusionRaycasting.HitResult hitResult = this.GetBestHit(base.transform.position, raycastGlobalForward);
			if (this.IsHitValid(ref hitResult, raycastGlobalForward))
			{
				if (this.minSurfaceRatio > 0.5f)
				{
					float raycastDistance = this.m_Master.raycastDistance;
					for (uint num = 0U; num < this.GetDirectionCount(); num += 1U)
					{
						Vector3 a = this.GetDirection(num + this.m_PrevNonSubHitDirectionId) * (this.minSurfaceRatio * 2f - 1f);
						a.Scale(base.transform.localScale);
						Vector3 vector = base.transform.position + a * this.m_Master.coneRadiusStart;
						Vector3 a2 = base.transform.position + a * this.m_Master.coneRadiusEnd + raycastGlobalForward * raycastDistance;
						DynamicOcclusionRaycasting.HitResult bestHit = this.GetBestHit(vector, (a2 - vector).normalized);
						if (!this.IsHitValid(ref bestHit, raycastGlobalForward))
						{
							this.m_PrevNonSubHitDirectionId = num;
							hitResult.SetNull();
							break;
						}
						if (bestHit.distance > hitResult.distance)
						{
							hitResult = bestHit;
						}
					}
				}
			}
			else
			{
				hitResult.SetNull();
			}
			this.SetHit(ref hitResult);
			return hitResult.hasCollider;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008880 File Offset: 0x00006A80
		private void SetHit(ref DynamicOcclusionRaycasting.HitResult hit)
		{
			if (!hit.hasCollider)
			{
				this.SetHitNull();
				return;
			}
			PlaneAlignment planeAlignment = this.planeAlignment;
			if (planeAlignment != PlaneAlignment.Surface && planeAlignment == PlaneAlignment.Beam)
			{
				this.SetClippingPlane(new Plane(-this.m_Master.raycastGlobalForward, hit.point));
			}
			else
			{
				this.SetClippingPlane(new Plane(hit.normal, hit.point));
			}
			this.m_CurrentHit = hit;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000088F0 File Offset: 0x00006AF0
		private void SetHitNull()
		{
			this.SetClippingPlaneOff();
			this.m_CurrentHit.SetNull();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00008904 File Offset: 0x00006B04
		protected override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
		{
			Plane planeEquationWS = this.planeEquationWS;
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionClippingPlaneWS, new Vector4(planeEquationWS.normal.x, planeEquationWS.normal.y, planeEquationWS.normal.z, planeEquationWS.distance));
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionClippingPlaneProps, this.fadeDistanceToSurface);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008964 File Offset: 0x00006B64
		private void SetClippingPlane(Plane planeWS)
		{
			planeWS = planeWS.TranslateCustom(planeWS.normal * this.planeOffset);
			this.SetPlaneWS(planeWS);
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), this.m_MaterialModifierCallbackCached);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000089A0 File Offset: 0x00006BA0
		private void SetClippingPlaneOff()
		{
			this.SetPlaneWS(default(Plane));
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), null);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000089CE File Offset: 0x00006BCE
		private void SetPlaneWS(Plane planeWS)
		{
			this.planeEquationWS = planeWS;
		}

		// Token: 0x04000132 RID: 306
		public new const string ClassName = "DynamicOcclusionRaycasting";

		// Token: 0x04000133 RID: 307
		public Dimensions dimensions;

		// Token: 0x04000134 RID: 308
		public LayerMask layerMask = Consts.DynOcclusion.LayerMaskDefault;

		// Token: 0x04000135 RID: 309
		public bool considerTriggers;

		// Token: 0x04000136 RID: 310
		public float minOccluderArea;

		// Token: 0x04000137 RID: 311
		public float minSurfaceRatio = 0.5f;

		// Token: 0x04000138 RID: 312
		public float maxSurfaceDot = 0.25f;

		// Token: 0x04000139 RID: 313
		public PlaneAlignment planeAlignment;

		// Token: 0x0400013A RID: 314
		public float planeOffset = 0.1f;

		// Token: 0x0400013B RID: 315
		[FormerlySerializedAs("fadeDistanceToPlane")]
		public float fadeDistanceToSurface = 0.25f;

		// Token: 0x0400013C RID: 316
		private DynamicOcclusionRaycasting.HitResult m_CurrentHit;

		// Token: 0x0400013D RID: 317
		private float m_RangeMultiplier = 1f;

		// Token: 0x0400013F RID: 319
		private uint m_PrevNonSubHitDirectionId;

		// Token: 0x020000BB RID: 187
		public struct HitResult
		{
			// Token: 0x060004D9 RID: 1241 RVA: 0x00013C2E File Offset: 0x00011E2E
			public HitResult(ref RaycastHit hit3D)
			{
				this.point = hit3D.point;
				this.normal = hit3D.normal;
				this.distance = hit3D.distance;
				this.collider3D = hit3D.collider;
				this.collider2D = null;
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x00013C68 File Offset: 0x00011E68
			public HitResult(ref RaycastHit2D hit2D)
			{
				this.point = hit2D.point;
				this.normal = hit2D.normal;
				this.distance = hit2D.distance;
				this.collider2D = hit2D.collider;
				this.collider3D = null;
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x060004DB RID: 1243 RVA: 0x00013CB6 File Offset: 0x00011EB6
			public bool hasCollider
			{
				get
				{
					return this.collider2D || this.collider3D;
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x060004DC RID: 1244 RVA: 0x00013CD2 File Offset: 0x00011ED2
			public string name
			{
				get
				{
					if (this.collider3D)
					{
						return this.collider3D.name;
					}
					if (this.collider2D)
					{
						return this.collider2D.name;
					}
					return "null collider";
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x060004DD RID: 1245 RVA: 0x00013D0C File Offset: 0x00011F0C
			public Bounds bounds
			{
				get
				{
					if (this.collider3D)
					{
						return this.collider3D.bounds;
					}
					if (this.collider2D)
					{
						return this.collider2D.bounds;
					}
					return default(Bounds);
				}
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x00013D54 File Offset: 0x00011F54
			public void SetNull()
			{
				this.collider2D = null;
				this.collider3D = null;
			}

			// Token: 0x040003CC RID: 972
			public Vector3 point;

			// Token: 0x040003CD RID: 973
			public Vector3 normal;

			// Token: 0x040003CE RID: 974
			public float distance;

			// Token: 0x040003CF RID: 975
			private Collider2D collider2D;

			// Token: 0x040003D0 RID: 976
			private Collider collider3D;
		}

		// Token: 0x020000BC RID: 188
		private enum Direction
		{
			// Token: 0x040003D2 RID: 978
			Up,
			// Token: 0x040003D3 RID: 979
			Down,
			// Token: 0x040003D4 RID: 980
			Left,
			// Token: 0x040003D5 RID: 981
			Right,
			// Token: 0x040003D6 RID: 982
			Max2D = 1,
			// Token: 0x040003D7 RID: 983
			Max3D = 3
		}
	}
}
