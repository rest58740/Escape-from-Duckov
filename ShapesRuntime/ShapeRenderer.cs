using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200000D RID: 13
	[DisallowMultipleComponent]
	public abstract class ShapeRenderer : MonoBehaviour
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00005738 File Offset: 0x00003938
		private MaterialPropertyBlock Mpb
		{
			get
			{
				MaterialPropertyBlock result;
				if ((result = this.mpb) == null)
				{
					result = (this.mpb = new MaterialPropertyBlock());
				}
				return result;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000575D File Offset: 0x0000395D
		internal bool IsUsingUniqueMaterials
		{
			get
			{
				return !this.IsInstanced;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00005768 File Offset: 0x00003968
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00005775 File Offset: 0x00003975
		public Mesh Mesh
		{
			get
			{
				return this.mf.sharedMesh;
			}
			private set
			{
				this.mf.sharedMesh = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005784 File Offset: 0x00003984
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000057A4 File Offset: 0x000039A4
		public int SortingLayerID
		{
			get
			{
				bool flag;
				return this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingLayerID;
			}
			set
			{
				bool flag;
				this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingLayerID = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000057C8 File Offset: 0x000039C8
		// (set) Token: 0x060001AA RID: 426 RVA: 0x000057E8 File Offset: 0x000039E8
		public int SortingOrder
		{
			get
			{
				bool flag;
				return this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingOrder;
			}
			set
			{
				bool flag;
				this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingOrder = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00005809 File Offset: 0x00003A09
		public string SortingLayerName
		{
			get
			{
				return SortingLayer.IDToName(this.SortingLayerID);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00005816 File Offset: 0x00003A16
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000581E File Offset: 0x00003A1E
		public ShapesBlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
				this.UpdateMaterial();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000582D File Offset: 0x00003A2D
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00005838 File Offset: 0x00003A38
		public ScaleMode ScaleMode
		{
			get
			{
				return this.scaleMode;
			}
			set
			{
				int propScaleMode = ShapesMaterialUtils.propScaleMode;
				this.scaleMode = value;
				this.SetIntNow(propScaleMode, (int)value);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000585A File Offset: 0x00003A5A
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00005864 File Offset: 0x00003A64
		public virtual Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				this.SetColorNow(propColor, value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00005886 File Offset: 0x00003A86
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000588E File Offset: 0x00003A8E
		public virtual DetailLevel DetailLevel
		{
			get
			{
				return this.detailLevel;
			}
			set
			{
				this.detailLevel = value;
				this.UpdateMesh(true);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000589E File Offset: 0x00003A9E
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x000058A6 File Offset: 0x00003AA6
		public ShapeCulling Culling
		{
			get
			{
				return this.culling;
			}
			set
			{
				this.culling = value;
				this.UpdateBounds();
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x000058B5 File Offset: 0x00003AB5
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x000058BD File Offset: 0x00003ABD
		public float BoundsPadding
		{
			get
			{
				return this.boundsPadding;
			}
			set
			{
				this.boundsPadding = value;
				this.UpdateBounds();
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000058CC File Offset: 0x00003ACC
		private bool IsInstanced
		{
			get
			{
				return this.UsingDefaultZTests && this.UsingDefaultMasking && this.UsingDefaultRenderQueue;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000058E6 File Offset: 0x00003AE6
		private bool UsingDefaultRenderQueue
		{
			get
			{
				return this.renderQueue == -1;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000058F1 File Offset: 0x00003AF1
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000058FC File Offset: 0x00003AFC
		public int RenderQueue
		{
			get
			{
				return this.renderQueue;
			}
			set
			{
				this.renderQueue = value;
				if (this.IsUsingUniqueMaterials)
				{
					this.UpdateMaterial();
					Material[] array = this.instancedMaterials;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].renderQueue = this.renderQueue;
					}
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00005941 File Offset: 0x00003B41
		private bool UsingDefaultZTests
		{
			get
			{
				return this.zTest == CompareFunction.LessEqual && this.zOffsetFactor == 0f && this.zOffsetUnits == 0;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00005964 File Offset: 0x00003B64
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000596C File Offset: 0x00003B6C
		public CompareFunction ZTest
		{
			get
			{
				return this.zTest;
			}
			set
			{
				int propZTest = ShapesMaterialUtils.propZTest;
				this.zTest = value;
				this.SetIntOnAllInstancedMaterials(propZTest, (int)value);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000598E File Offset: 0x00003B8E
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00005998 File Offset: 0x00003B98
		public float ZOffsetFactor
		{
			get
			{
				return this.zOffsetFactor;
			}
			set
			{
				int propZOffsetFactor = ShapesMaterialUtils.propZOffsetFactor;
				this.zOffsetFactor = value;
				this.SetFloatOnAllInstancedMaterials(propZOffsetFactor, value);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000059BA File Offset: 0x00003BBA
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000059C4 File Offset: 0x00003BC4
		public int ZOffsetUnits
		{
			get
			{
				return this.zOffsetUnits;
			}
			set
			{
				int propZOffsetUnits = ShapesMaterialUtils.propZOffsetUnits;
				this.zOffsetUnits = value;
				this.SetIntOnAllInstancedMaterials(propZOffsetUnits, value);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000059E6 File Offset: 0x00003BE6
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x000059F0 File Offset: 0x00003BF0
		public ColorWriteMask ColorMask
		{
			get
			{
				return this.colorMask;
			}
			set
			{
				int propColorMask = ShapesMaterialUtils.propColorMask;
				this.colorMask = value;
				this.SetIntOnAllInstancedMaterials(propColorMask, (int)value);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00005A14 File Offset: 0x00003C14
		private bool UsingDefaultMasking
		{
			get
			{
				return this.stencilComp == CompareFunction.Always && this.stencilOpPass == StencilOp.Keep && this.stencilRefID == 0 && this.stencilReadMask == byte.MaxValue && this.stencilWriteMask == byte.MaxValue && this.colorMask == ColorWriteMask.All;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00005A60 File Offset: 0x00003C60
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00005A68 File Offset: 0x00003C68
		public CompareFunction StencilComp
		{
			get
			{
				return this.stencilComp;
			}
			set
			{
				int propStencilComp = ShapesMaterialUtils.propStencilComp;
				this.stencilComp = value;
				this.SetIntOnAllInstancedMaterials(propStencilComp, (int)value);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00005A8A File Offset: 0x00003C8A
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00005A94 File Offset: 0x00003C94
		public StencilOp StencilOpPass
		{
			get
			{
				return this.stencilOpPass;
			}
			set
			{
				int propStencilOpPass = ShapesMaterialUtils.propStencilOpPass;
				this.stencilOpPass = value;
				this.SetIntOnAllInstancedMaterials(propStencilOpPass, (int)value);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00005AB6 File Offset: 0x00003CB6
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public byte StencilRefID
		{
			get
			{
				return this.stencilRefID;
			}
			set
			{
				int propStencilID = ShapesMaterialUtils.propStencilID;
				this.stencilRefID = value;
				this.SetIntOnAllInstancedMaterials(propStencilID, (int)value);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00005AE2 File Offset: 0x00003CE2
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00005AEC File Offset: 0x00003CEC
		public byte StencilReadMask
		{
			get
			{
				return this.stencilReadMask;
			}
			set
			{
				int propStencilReadMask = ShapesMaterialUtils.propStencilReadMask;
				this.stencilReadMask = value;
				this.SetIntOnAllInstancedMaterials(propStencilReadMask, (int)value);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00005B0E File Offset: 0x00003D0E
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00005B18 File Offset: 0x00003D18
		public byte StencilWriteMask
		{
			get
			{
				return this.stencilWriteMask;
			}
			set
			{
				int propStencilWriteMask = ShapesMaterialUtils.propStencilWriteMask;
				this.stencilWriteMask = value;
				this.SetIntOnAllInstancedMaterials(propStencilWriteMask, (int)value);
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00005B3C File Offset: 0x00003D3C
		private T MakeSureComponentExists<T>(ref T field, out bool created) where T : Component
		{
			if (field == null)
			{
				field = base.GetComponent<T>();
				if (field == null)
				{
					field = base.gameObject.AddComponent<T>();
					created = true;
				}
				field.hideFlags = HideFlags.HideInInspector;
			}
			created = false;
			return field;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00005BA8 File Offset: 0x00003DA8
		private void VerifyComponents()
		{
			if (!this.initializedComponents)
			{
				this.initializedComponents = true;
				bool flag;
				this.MakeSureComponentExists<MeshFilter>(ref this.mf, out flag);
				bool flag2;
				this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag2);
			}
			if (this.rnd.receiveShadows)
			{
				this.rnd.receiveShadows = false;
			}
			if (this.rnd.shadowCastingMode != ShadowCastingMode.Off)
			{
				this.rnd.shadowCastingMode = ShadowCastingMode.Off;
			}
			if (this.rnd.lightProbeUsage != LightProbeUsage.Off)
			{
				this.rnd.lightProbeUsage = LightProbeUsage.Off;
			}
			if (this.rnd.reflectionProbeUsage != ReflectionProbeUsage.Off)
			{
				this.rnd.reflectionProbeUsage = ReflectionProbeUsage.Off;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00005C46 File Offset: 0x00003E46
		public virtual void Awake()
		{
			this.VerifyComponents();
			this.UpdateMaterial();
			this.UpdateMesh(false);
			this.UpdateAllMaterialProperties();
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00005C61 File Offset: 0x00003E61
		private bool HasGeneratedOrCopyOfMesh
		{
			get
			{
				return this.MeshUpdateMode == MeshUpdateMode.SelfGenerated || this.MeshUpdateMode == MeshUpdateMode.UseAssetCopy;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00005C77 File Offset: 0x00003E77
		public virtual void OnEnable()
		{
			this.UpdateMesh(false);
			this.rnd.enabled = true;
			if (this.UseCamOnPreCull)
			{
				this.SubscribeCamPreCull();
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00005C9A File Offset: 0x00003E9A
		private void OnDisable()
		{
			if (this.rnd != null)
			{
				this.rnd.enabled = false;
			}
			if (this.UseCamOnPreCull)
			{
				this.UnsubscribeCamPreCull();
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00005CC4 File Offset: 0x00003EC4
		private void OnPreCamCullWithCam(Camera cam)
		{
			this.CamOnPreCull();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00005CCC File Offset: 0x00003ECC
		private void OnPreCamCullWithCam(ScriptableRenderContext ctx, Camera cam)
		{
			this.CamOnPreCull();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00005CD4 File Offset: 0x00003ED4
		private void SubscribeCamPreCull()
		{
			if (UnityInfo.UsingSRP)
			{
				RenderPipelineManager.beginCameraRendering += new Action<ScriptableRenderContext, Camera>(this.OnPreCamCullWithCam);
				return;
			}
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCamCullWithCam));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00005D0F File Offset: 0x00003F0F
		private void UnsubscribeCamPreCull()
		{
			if (UnityInfo.UsingSRP)
			{
				RenderPipelineManager.beginCameraRendering -= new Action<ScriptableRenderContext, Camera>(this.OnPreCamCullWithCam);
				return;
			}
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCamCullWithCam));
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00005D4A File Offset: 0x00003F4A
		private void Reset()
		{
			this.UpdateAllMaterialProperties();
			this.UpdateMesh(true);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00005D5C File Offset: 0x00003F5C
		private void OnDestroy()
		{
			if (this.HasGeneratedOrCopyOfMesh && this.Mesh != null)
			{
				Object.DestroyImmediate(this.Mesh);
			}
			this.TryDestroyInOnDestroy(this.rnd);
			this.TryDestroyInOnDestroy(this.mf);
			this.TryDestroyInstancedMaterials(true);
		}

		// Token: 0x060001DC RID: 476
		private protected abstract Bounds GetUnpaddedLocalBounds_Internal();

		// Token: 0x060001DD RID: 477
		private protected abstract void SetAllMaterialProperties();

		// Token: 0x060001DE RID: 478 RVA: 0x00005DA9 File Offset: 0x00003FA9
		private protected virtual void ShapeClampRanges()
		{
		}

		// Token: 0x060001DF RID: 479
		private protected abstract void GetMaterials(Material[] mats);

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00005DAB File Offset: 0x00003FAB
		private protected virtual int MaterialCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00005DAE File Offset: 0x00003FAE
		private protected virtual void GenerateMesh()
		{
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00005DB0 File Offset: 0x00003FB0
		private protected virtual Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.QuadMesh[this.HasDetailLevels ? 2 : 0];
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00005DC4 File Offset: 0x00003FC4
		private protected virtual MeshUpdateMode MeshUpdateMode
		{
			get
			{
				return MeshUpdateMode.UseAsset;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00005DC7 File Offset: 0x00003FC7
		internal virtual bool HasScaleModes
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00005DCA File Offset: 0x00003FCA
		internal virtual bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00005DCD File Offset: 0x00003FCD
		private protected virtual bool UseCamOnPreCull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00005DD0 File Offset: 0x00003FD0
		internal virtual void CamOnPreCull()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00005DD4 File Offset: 0x00003FD4
		private void UpdateBounds()
		{
			Bounds bounds = this.GetBounds();
			MeshUpdateMode meshUpdateMode = this.MeshUpdateMode;
			if ((meshUpdateMode == MeshUpdateMode.UseAssetCopy || meshUpdateMode == MeshUpdateMode.SelfGenerated) && this.Mesh != null)
			{
				this.Mesh.bounds = bounds;
				this.rnd.ResetLocalBounds();
				return;
			}
			if (this.Culling == ShapeCulling.CalculatedLocal)
			{
				this.rnd.localBounds = bounds;
				return;
			}
			if (this.Culling == ShapeCulling.SimpleGlobal)
			{
				this.rnd.ResetLocalBounds();
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00005E48 File Offset: 0x00004048
		private void TryDestroyInstancedMaterials(bool inOnDestroy = false)
		{
			if (this.instancedMaterials != null)
			{
				for (int i = 0; i < this.instancedMaterials.Length; i++)
				{
					if (this.instancedMaterials[i] != null)
					{
						if (inOnDestroy)
						{
							this.TryDestroyInOnDestroy(this.instancedMaterials[i]);
						}
						else
						{
							this.instancedMaterials[i].DestroyBranched();
						}
					}
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00005EA0 File Offset: 0x000040A0
		private void MakeSureMaterialInstancesAreGood(Material[] sourceMats)
		{
			ShapeRenderer.<>c__DisplayClass135_0 CS$<>8__locals1;
			CS$<>8__locals1.sourceMats = sourceMats;
			CS$<>8__locals1.<>4__this = this;
			if (this.instancedMaterials == null)
			{
				this.<MakeSureMaterialInstancesAreGood>g__PopulateAll|135_1(ref CS$<>8__locals1);
				return;
			}
			if (this.instancedMaterials.Length != CS$<>8__locals1.sourceMats.Length)
			{
				this.TryDestroyInstancedMaterials(false);
				this.<MakeSureMaterialInstancesAreGood>g__PopulateAll|135_1(ref CS$<>8__locals1);
				return;
			}
			for (int i = 0; i < CS$<>8__locals1.sourceMats.Length; i++)
			{
				if (this.instancedMaterials[i] == null)
				{
					this.instancedMaterials[i] = this.<MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|135_0(i, ref CS$<>8__locals1);
				}
				else if (this.instancedMaterials[i].shader != CS$<>8__locals1.sourceMats[i].shader)
				{
					this.instancedMaterials[i].DestroyBranched();
					this.instancedMaterials[i] = this.<MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|135_0(i, ref CS$<>8__locals1);
				}
				else
				{
					this.instancedMaterials[i].shaderKeywords = CS$<>8__locals1.sourceMats[i].shaderKeywords;
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00005F88 File Offset: 0x00004188
		private protected void UpdateMaterial()
		{
			if (this.mats == null || this.mats.Length != this.MaterialCount)
			{
				this.mats = new Material[this.MaterialCount];
			}
			this.GetMaterials(this.mats);
			if (this.IsUsingUniqueMaterials)
			{
				this.MakeSureMaterialInstancesAreGood(this.mats);
				this.mats = this.instancedMaterials;
			}
			this.VerifyComponents();
			this.rnd.sharedMaterials = this.mats;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006004 File Offset: 0x00004204
		public void UpdateMesh(bool force = false)
		{
			MeshUpdateMode meshUpdateMode = this.MeshUpdateMode;
			if (meshUpdateMode == MeshUpdateMode.UseAsset && (this.Mesh == null || this.Mesh != this.GetInitialMeshAsset()))
			{
				this.Mesh = this.GetInitialMeshAsset();
				return;
			}
			int instanceID = base.gameObject.GetInstanceID();
			if (this.Mesh == null || this.meshOwnerID != instanceID)
			{
				this.meshOwnerID = instanceID;
				if (meshUpdateMode == MeshUpdateMode.UseAssetCopy)
				{
					this.Mesh = Object.Instantiate<Mesh>(this.GetInitialMeshAsset());
					this.Mesh.hideFlags = HideFlags.HideAndDontSave;
					this.Mesh.MarkDynamic();
				}
				else if (meshUpdateMode == MeshUpdateMode.SelfGenerated)
				{
					this.Mesh = new Mesh
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					this.Mesh.MarkDynamic();
					this.GenerateMesh();
				}
			}
			else if (force && meshUpdateMode == MeshUpdateMode.SelfGenerated)
			{
				this.GenerateMesh();
			}
			this.UpdateBounds();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000060E8 File Offset: 0x000042E8
		public Bounds GetBounds()
		{
			Bounds unpaddedLocalBounds_Internal = this.GetUnpaddedLocalBounds_Internal();
			unpaddedLocalBounds_Internal.Expand(this.boundsPadding);
			return unpaddedLocalBounds_Internal;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000610C File Offset: 0x0000430C
		public Bounds GetWorldBounds()
		{
			Bounds bounds = this.GetBounds();
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			Transform transform = base.transform;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					for (int k = -1; k <= 1; k += 2)
					{
						Vector3 rhs = transform.TransformPoint(bounds.center + Vector3.Scale(bounds.extents, new Vector3((float)i, (float)j, (float)k)));
						vector = Vector3.Min(vector, rhs);
						vector2 = Vector3.Max(vector2, rhs);
					}
				}
			}
			return new Bounds((vector2 + vector) / 2f, ShapesMath.Abs(vector2 - vector));
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000061D9 File Offset: 0x000043D9
		private void OnDidApplyAnimationProperties()
		{
			this.UpdateAllMaterialProperties();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000061E4 File Offset: 0x000043E4
		private void SetIntOnAllInstancedMaterials(int property, int value)
		{
			if (this.IsUsingUniqueMaterials)
			{
				this.UpdateMaterial();
				Material[] array = this.instancedMaterials;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetInt_Shapes(property, value);
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006220 File Offset: 0x00004420
		private void SetFloatOnAllInstancedMaterials(int property, float value)
		{
			if (this.IsUsingUniqueMaterials)
			{
				this.UpdateMaterial();
				Material[] array = this.instancedMaterials;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetFloat(property, value);
				}
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000625C File Offset: 0x0000445C
		internal void UpdateAllMaterialProperties()
		{
			if (!base.gameObject.scene.IsValid())
			{
				return;
			}
			this.UpdateMaterial();
			if (this.IsUsingUniqueMaterials)
			{
				foreach (Material material in this.instancedMaterials)
				{
					material.SetInt_Shapes(ShapesMaterialUtils.propZTest, (int)this.zTest);
					material.SetFloat(ShapesMaterialUtils.propZOffsetFactor, this.zOffsetFactor);
					material.SetInt_Shapes(ShapesMaterialUtils.propZOffsetUnits, this.zOffsetUnits);
					material.SetInt_Shapes(ShapesMaterialUtils.propColorMask, (int)this.colorMask);
					material.SetInt_Shapes(ShapesMaterialUtils.propStencilComp, (int)this.stencilComp);
					material.SetInt_Shapes(ShapesMaterialUtils.propStencilOpPass, (int)this.stencilOpPass);
					material.SetInt_Shapes(ShapesMaterialUtils.propStencilID, (int)this.stencilRefID);
					material.SetInt_Shapes(ShapesMaterialUtils.propStencilReadMask, (int)this.stencilReadMask);
					material.SetInt_Shapes(ShapesMaterialUtils.propStencilWriteMask, (int)this.stencilWriteMask);
					material.renderQueue = this.renderQueue;
				}
			}
			this.SetColor(ShapesMaterialUtils.propColor, this.color);
			if (this.HasScaleModes)
			{
				this.SetInt(ShapesMaterialUtils.propScaleMode, (int)this.scaleMode);
			}
			this.SetAllMaterialProperties();
			this.ApplyProperties();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00006388 File Offset: 0x00004588
		private protected void ApplyProperties()
		{
			this.VerifyComponents();
			this.rnd.SetPropertyBlock(this.Mpb);
			this.UpdateBounds();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000063A8 File Offset: 0x000045A8
		private protected void SetAllDashValues(DashStyle style, bool dashed, bool matchSpacingToSize, float thickness, bool setType, bool now)
		{
			float netAbsoluteSize = style.GetNetAbsoluteSize(dashed, thickness);
			if (dashed)
			{
				this.SetFloat(ShapesMaterialUtils.propDashSpacing, this.GetNetDashSpacing(style, true, matchSpacingToSize, thickness));
				this.SetFloat(ShapesMaterialUtils.propDashOffset, style.offset);
				this.SetInt(ShapesMaterialUtils.propDashSpace, (int)style.space);
				this.SetInt(ShapesMaterialUtils.propDashSnap, (int)style.snap);
				if (setType)
				{
					this.SetInt(ShapesMaterialUtils.propDashType, (int)style.type);
					if (style.type.HasModifier())
					{
						this.SetFloat(ShapesMaterialUtils.propDashShapeModifier, style.shapeModifier);
					}
				}
			}
			if (now)
			{
				this.SetFloatNow(ShapesMaterialUtils.propDashSize, netAbsoluteSize);
				return;
			}
			this.SetFloat(ShapesMaterialUtils.propDashSize, netAbsoluteSize);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000645C File Offset: 0x0000465C
		private protected float GetNetDashSpacing(DashStyle style, bool dashed, bool matchSpacingToSize, float thickness)
		{
			if (matchSpacingToSize && style.space == DashSpace.FixedCount)
			{
				return 0.5f;
			}
			if (!matchSpacingToSize)
			{
				return style.GetNetAbsoluteSpacing(dashed, thickness);
			}
			return style.GetNetAbsoluteSize(dashed, thickness);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000648C File Offset: 0x0000468C
		private protected void SetColor(int prop, Color value)
		{
			if (ShapeGroup.shapeGroupsInScene > 0)
			{
				ShapeGroup[] componentsInParent = base.GetComponentsInParent<ShapeGroup>();
				if (componentsInParent != null)
				{
					foreach (ShapeGroup shapeGroup in from g in componentsInParent
					where g.IsEnabled
					select g)
					{
						value *= shapeGroup.Color;
					}
				}
			}
			this.Mpb.SetColor(prop, value);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00006520 File Offset: 0x00004720
		private protected void SetFloat(int prop, float value)
		{
			this.Mpb.SetFloat(prop, value);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000652F File Offset: 0x0000472F
		private protected void SetInt(int prop, int value)
		{
			this.Mpb.SetInt_Shapes(prop, value);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000653E File Offset: 0x0000473E
		private protected void SetVector3(int prop, Vector3 value)
		{
			this.Mpb.SetVector(prop, value);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00006552 File Offset: 0x00004752
		private protected void SetVector4(int prop, Vector4 value)
		{
			this.Mpb.SetVector(prop, value);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00006561 File Offset: 0x00004761
		private protected void SetColorNow(int prop, Color value)
		{
			this.SetColor(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00006571 File Offset: 0x00004771
		private protected void SetFloatNow(int prop, float value)
		{
			this.Mpb.SetFloat(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00006586 File Offset: 0x00004786
		private protected void SetIntNow(int prop, int value)
		{
			this.Mpb.SetInt_Shapes(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000659B File Offset: 0x0000479B
		private protected void SetVector3Now(int prop, Vector3 value)
		{
			this.Mpb.SetVector(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000065B5 File Offset: 0x000047B5
		private protected void SetVector4Now(int prop, Vector4 value)
		{
			this.Mpb.SetVector(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00006632 File Offset: 0x00004832
		[CompilerGenerated]
		private Material <MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|135_0(int index, ref ShapeRenderer.<>c__DisplayClass135_0 A_2)
		{
			return new Material(A_2.sourceMats[index])
			{
				name = A_2.sourceMats[index].name + " (instance)"
			};
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00006660 File Offset: 0x00004860
		[CompilerGenerated]
		private void <MakeSureMaterialInstancesAreGood>g__PopulateAll|135_1(ref ShapeRenderer.<>c__DisplayClass135_0 A_1)
		{
			this.instancedMaterials = new Material[A_1.sourceMats.Length];
			for (int i = 0; i < A_1.sourceMats.Length; i++)
			{
				this.instancedMaterials[i] = this.<MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|135_0(i, ref A_1);
			}
		}

		// Token: 0x04000055 RID: 85
		private bool initializedComponents;

		// Token: 0x04000056 RID: 86
		private MeshRenderer rnd;

		// Token: 0x04000057 RID: 87
		private MeshFilter mf;

		// Token: 0x04000058 RID: 88
		private int meshOwnerID;

		// Token: 0x04000059 RID: 89
		private MaterialPropertyBlock mpb;

		// Token: 0x0400005A RID: 90
		private Material[] instancedMaterials;

		// Token: 0x0400005B RID: 91
		[NonSerialized]
		public bool meshOutOfDate = true;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		private ShapesBlendMode blendMode = ShapesBlendMode.Transparent;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private ScaleMode scaleMode;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		[ShapesColorField(true)]
		private protected Color color = Color.white;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		private protected DetailLevel detailLevel = DetailLevel.Medium;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private protected ShapeCulling culling;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private protected float boundsPadding;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		private int renderQueue = -1;

		// Token: 0x04000063 RID: 99
		public const int DEFAULT_RENDER_QUEUE_AUTO = -1;

		// Token: 0x04000064 RID: 100
		public const CompareFunction DEFAULT_ZTEST = CompareFunction.LessEqual;

		// Token: 0x04000065 RID: 101
		public const float DEFAULT_ZOFS_FACTOR = 0f;

		// Token: 0x04000066 RID: 102
		public const int DEFAULT_ZOFS_UNITS = 0;

		// Token: 0x04000067 RID: 103
		public const ColorWriteMask DEFAULT_COLOR_MASK = ColorWriteMask.All;

		// Token: 0x04000068 RID: 104
		[SerializeField]
		private CompareFunction zTest = CompareFunction.LessEqual;

		// Token: 0x04000069 RID: 105
		[SerializeField]
		private float zOffsetFactor;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private int zOffsetUnits;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		private ColorWriteMask colorMask = ColorWriteMask.All;

		// Token: 0x0400006C RID: 108
		public const CompareFunction DEFAULT_STENCIL_COMP = CompareFunction.Always;

		// Token: 0x0400006D RID: 109
		public const StencilOp DEFAULT_STENCIL_OP = StencilOp.Keep;

		// Token: 0x0400006E RID: 110
		public const byte DEFAULT_STENCIL_REF_ID = 0;

		// Token: 0x0400006F RID: 111
		public const byte DEFAULT_STENCIL_MASK = 255;

		// Token: 0x04000070 RID: 112
		[SerializeField]
		private CompareFunction stencilComp = CompareFunction.Always;

		// Token: 0x04000071 RID: 113
		[SerializeField]
		private StencilOp stencilOpPass;

		// Token: 0x04000072 RID: 114
		[SerializeField]
		private byte stencilRefID;

		// Token: 0x04000073 RID: 115
		[SerializeField]
		private byte stencilReadMask = byte.MaxValue;

		// Token: 0x04000074 RID: 116
		[SerializeField]
		private byte stencilWriteMask = byte.MaxValue;

		// Token: 0x04000075 RID: 117
		private Material[] mats;
	}
}
