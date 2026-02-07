using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x02000038 RID: 56
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamSD))]
	public abstract class DynamicOcclusionAbstractBase : MonoBehaviour
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00007C47 File Offset: 0x00005E47
		public void ProcessOcclusionManually()
		{
			this.ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource.User);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001AD RID: 429 RVA: 0x00007C50 File Offset: 0x00005E50
		// (remove) Token: 0x060001AE RID: 430 RVA: 0x00007C88 File Offset: 0x00005E88
		public event Action onOcclusionProcessed;

		// Token: 0x060001AF RID: 431 RVA: 0x00007CC0 File Offset: 0x00005EC0
		protected void ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			if (!Config.Instance.featureEnabledDynamicOcclusion)
			{
				return;
			}
			if (this.m_LastFrameRendered == Time.frameCount && Application.isPlaying && source == DynamicOcclusionAbstractBase.ProcessOcclusionSource.OnEnable)
			{
				return;
			}
			bool flag = this.OnProcessOcclusion(source);
			if (this.onOcclusionProcessed != null)
			{
				this.onOcclusionProcessed();
			}
			if (this.m_Master)
			{
				this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), flag ? this.m_MaterialModifierCallbackCached : null);
			}
			if (this.updateRate.HasFlag(DynamicOcclusionUpdateRate.OnBeamMove))
			{
				this.m_TransformPacked = base.transform.GetWorldPacked();
			}
			bool flag2 = this.m_LastFrameRendered < 0;
			this.m_LastFrameRendered = Time.frameCount;
			if (flag2 && DynamicOcclusionAbstractBase._INTERNAL_ApplyRandomFrameOffset)
			{
				this.m_LastFrameRendered += UnityEngine.Random.Range(0, this.waitXFrames);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00007D97 File Offset: 0x00005F97
		public int _INTERNAL_LastFrameRendered
		{
			get
			{
				return this.m_LastFrameRendered;
			}
		}

		// Token: 0x060001B1 RID: 433
		protected abstract string GetShaderKeyword();

		// Token: 0x060001B2 RID: 434
		protected abstract MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode();

		// Token: 0x060001B3 RID: 435
		protected abstract bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source);

		// Token: 0x060001B4 RID: 436
		protected abstract void OnModifyMaterialCallback(MaterialModifier.Interface owner);

		// Token: 0x060001B5 RID: 437
		protected abstract void OnEnablePostValidate();

		// Token: 0x060001B6 RID: 438 RVA: 0x00007D9F File Offset: 0x00005F9F
		protected virtual void OnValidateProperties()
		{
			this.waitXFrames = Mathf.Clamp(this.waitXFrames, 1, 60);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007DB5 File Offset: 0x00005FB5
		protected virtual void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamSD>();
			this.m_Master._INTERNAL_DynamicOcclusionMode = this.GetDynamicOcclusionMode();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007DD4 File Offset: 0x00005FD4
		protected virtual void OnDestroy()
		{
			this.m_Master._INTERNAL_DynamicOcclusionMode = MaterialManager.SD.DynamicOcclusion.Off;
			this.DisableOcclusion();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00007DE8 File Offset: 0x00005FE8
		protected virtual void OnEnable()
		{
			this.m_MaterialModifierCallbackCached = new MaterialModifier.Callback(this.OnModifyMaterialCallback);
			this.OnValidateProperties();
			this.OnEnablePostValidate();
			this.m_Master.onWillCameraRenderThisBeam += this.OnWillCameraRender;
			if (!this.updateRate.HasFlag(DynamicOcclusionUpdateRate.Never))
			{
				this.m_Master.RegisterOnBeamGeometryInitializedCallback(delegate
				{
					this.ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource.OnEnable);
				});
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007E5A File Offset: 0x0000605A
		protected virtual void OnDisable()
		{
			this.m_Master.onWillCameraRenderThisBeam -= this.OnWillCameraRender;
			this.DisableOcclusion();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007E7C File Offset: 0x0000607C
		private void OnWillCameraRender(Camera cam)
		{
			if (cam != null && cam.enabled && Time.frameCount != this.m_LastFrameRendered)
			{
				bool flag = false;
				if (!flag && this.updateRate.HasFlag(DynamicOcclusionUpdateRate.OnBeamMove) && !this.m_TransformPacked.IsSame(base.transform))
				{
					flag = true;
				}
				if (!flag && this.updateRate.HasFlag(DynamicOcclusionUpdateRate.EveryXFrames) && Time.frameCount >= this.m_LastFrameRendered + this.waitXFrames)
				{
					flag = true;
				}
				if (flag)
				{
					this.ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource.RenderLoop);
				}
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007F17 File Offset: 0x00006117
		private void DisableOcclusion()
		{
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), null);
		}

		// Token: 0x04000122 RID: 290
		public const string ClassName = "DynamicOcclusionAbstractBase";

		// Token: 0x04000123 RID: 291
		public DynamicOcclusionUpdateRate updateRate = DynamicOcclusionUpdateRate.EveryXFrames;

		// Token: 0x04000124 RID: 292
		[FormerlySerializedAs("waitFrameCount")]
		public int waitXFrames = 3;

		// Token: 0x04000126 RID: 294
		public static bool _INTERNAL_ApplyRandomFrameOffset = true;

		// Token: 0x04000127 RID: 295
		private TransformUtils.Packed m_TransformPacked;

		// Token: 0x04000128 RID: 296
		private int m_LastFrameRendered = int.MinValue;

		// Token: 0x04000129 RID: 297
		protected VolumetricLightBeamSD m_Master;

		// Token: 0x0400012A RID: 298
		protected MaterialModifier.Callback m_MaterialModifierCallbackCached;

		// Token: 0x020000B9 RID: 185
		protected enum ProcessOcclusionSource
		{
			// Token: 0x040003C6 RID: 966
			RenderLoop,
			// Token: 0x040003C7 RID: 967
			OnEnable,
			// Token: 0x040003C8 RID: 968
			EditorUpdate,
			// Token: 0x040003C9 RID: 969
			User
		}
	}
}
