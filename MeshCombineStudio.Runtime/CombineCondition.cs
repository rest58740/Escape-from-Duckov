using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshCombineStudio
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public struct CombineCondition
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000E074 File Offset: 0x0000C274
		public static CombineCondition Default
		{
			get
			{
				return new CombineCondition
				{
					matInstanceId = -1,
					lightmapIndex = -1,
					shadowCastingMode = ShadowCastingMode.On,
					receiveShadows = true,
					lightmapScale = 1f,
					lightProbeUsage = LightProbeUsage.BlendProbes,
					reflectionProbeUsage = ReflectionProbeUsage.BlendProbes,
					probeAnchor = null,
					motionVectorGenerationMode = MotionVectorGenerationMode.Camera,
					layer = 0,
					rootInstanceId = -1
				};
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
		public static void MakeFoundReport(FoundCombineConditions fcc)
		{
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition.matInstanceId);
			}
			fcc.matCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition2 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition2.lightmapIndex);
			}
			fcc.lightmapIndexCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition3 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition3.shadowCastingMode);
			}
			fcc.shadowCastingCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition4 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition4.receiveShadows);
			}
			fcc.receiveShadowsCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition5 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition5.lightmapScale);
			}
			fcc.lightmapScale = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition6 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition6.lightProbeUsage);
			}
			fcc.lightProbeUsageCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition7 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition7.reflectionProbeUsage);
			}
			fcc.reflectionProbeUsageCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition8 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition8.probeAnchor);
			}
			fcc.probeAnchorCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition9 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition9.motionVectorGenerationMode);
			}
			fcc.motionVectorGenerationModeCount = CombineCondition.countSet.Count;
			CombineCondition.countSet.Clear();
			foreach (CombineCondition combineCondition10 in fcc.combineConditions)
			{
				CombineCondition.countSet.Add(combineCondition10.layer);
			}
			fcc.layerCount = CombineCondition.countSet.Count;
			fcc.combineConditionsCount = fcc.combineConditions.Count;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000E544 File Offset: 0x0000C744
		public void ReadFromGameObject(int rootInstanceId, CombineConditionSettings combineConditions, bool copyBakedLighting, GameObject go, Transform t, MeshRenderer mr, Material mat)
		{
			this.matInstanceId = (combineConditions.sameMaterial ? mat.GetInstanceID() : combineConditions.combineCondition.matInstanceId);
			this.lightmapIndex = (copyBakedLighting ? mr.lightmapIndex : (this.lightmapIndex = -1));
			this.shadowCastingMode = (combineConditions.sameShadowCastingMode ? mr.shadowCastingMode : combineConditions.combineCondition.shadowCastingMode);
			this.receiveShadows = (combineConditions.sameReceiveShadows ? mr.receiveShadows : combineConditions.combineCondition.receiveShadows);
			this.lightmapScale = (combineConditions.sameLightmapScale ? this.GetLightmapScale(mr) : combineConditions.combineCondition.lightmapScale);
			this.lightProbeUsage = (combineConditions.sameLightProbeUsage ? mr.lightProbeUsage : combineConditions.combineCondition.lightProbeUsage);
			this.reflectionProbeUsage = (combineConditions.sameReflectionProbeUsage ? mr.reflectionProbeUsage : combineConditions.combineCondition.reflectionProbeUsage);
			this.probeAnchor = (combineConditions.sameProbeAnchor ? mr.probeAnchor : combineConditions.combineCondition.probeAnchor);
			this.motionVectorGenerationMode = (combineConditions.sameMotionVectorGenerationMode ? mr.motionVectorGenerationMode : combineConditions.combineCondition.motionVectorGenerationMode);
			this.layer = (combineConditions.sameLayer ? go.layer : combineConditions.combineCondition.layer);
			this.rootInstanceId = rootInstanceId;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000E6A7 File Offset: 0x0000C8A7
		private float GetLightmapScale(MeshRenderer mr)
		{
			return 1f;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000E6AE File Offset: 0x0000C8AE
		private void SetLightmapScale(MeshRenderer mr, float lightmapScale)
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000E6B0 File Offset: 0x0000C8B0
		public void WriteToGameObject(GameObject go, MeshRenderer mr)
		{
			mr.lightmapIndex = this.lightmapIndex;
			mr.shadowCastingMode = this.shadowCastingMode;
			mr.receiveShadows = this.receiveShadows;
			if (this.lightmapScale != 1f)
			{
				this.SetLightmapScale(mr, this.lightmapScale);
			}
			mr.lightProbeUsage = this.lightProbeUsage;
			mr.reflectionProbeUsage = this.reflectionProbeUsage;
			mr.motionVectorGenerationMode = this.motionVectorGenerationMode;
			mr.probeAnchor = this.probeAnchor;
			go.layer = this.layer;
		}

		// Token: 0x040001D7 RID: 471
		public static HashSet<object> countSet = new HashSet<object>();

		// Token: 0x040001D8 RID: 472
		public int matInstanceId;

		// Token: 0x040001D9 RID: 473
		public int lightmapIndex;

		// Token: 0x040001DA RID: 474
		public ShadowCastingMode shadowCastingMode;

		// Token: 0x040001DB RID: 475
		public bool receiveShadows;

		// Token: 0x040001DC RID: 476
		public float lightmapScale;

		// Token: 0x040001DD RID: 477
		public LightProbeUsage lightProbeUsage;

		// Token: 0x040001DE RID: 478
		public ReflectionProbeUsage reflectionProbeUsage;

		// Token: 0x040001DF RID: 479
		public Transform probeAnchor;

		// Token: 0x040001E0 RID: 480
		public MotionVectorGenerationMode motionVectorGenerationMode;

		// Token: 0x040001E1 RID: 481
		public int layer;

		// Token: 0x040001E2 RID: 482
		public int rootInstanceId;
	}
}
