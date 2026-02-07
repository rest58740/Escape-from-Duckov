using System;
using System.Collections.Generic;
using LeTai.TrueShadow.PluginInterfaces;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x0200000A RID: 10
	[HelpURL("https://leloctai.com/asset/translucentimage/docs/articles/customize.html#translucent-image")]
	public class TranslucentImage : Image, IMeshModifier, ITrueShadowCustomHashProvider
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000024F4 File Offset: 0x000006F4
		protected override void Start()
		{
			base.Start();
			this.isBirp = !GraphicsSettings.currentRenderPipeline;
			this.oldVibrancy = this.vibrancy;
			this.oldBrightness = this.brightness;
			this.oldFlatten = this.flatten;
			this.AutoAcquireSource();
			if (this.material)
			{
				if (Application.isPlaying && this.material.shader.name != "UI/TranslucentImage")
				{
					Debug.LogWarning("Translucent Image requires a material using the \"UI/TranslucentImage\" shader");
				}
				else if (this.source)
				{
					this.material.SetTexture(TranslucentImage._blurTexPropId, this.source.BlurredScreen);
				}
			}
			if (base.canvas)
			{
				base.canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025C8 File Offset: 0x000007C8
		private bool IsInPrefabMode()
		{
			return false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000025CC File Offset: 0x000007CC
		private void AutoAcquireSource()
		{
			if (this.IsInPrefabMode() && !Application.isPlaying)
			{
				return;
			}
			if (this.sourceAcquiredOnStart)
			{
				return;
			}
			this.source = (this.source ? this.source : Shims.FindObjectOfType<TranslucentImageSource>(false, true));
			this.sourceAcquiredOnStart = true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000261B File Offset: 0x0000081B
		private bool Validate()
		{
			return this.IsActive() && this.material && this.source && this.source.BlurredScreen;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002658 File Offset: 0x00000858
		private void LateUpdate()
		{
			if (!this.shouldRun)
			{
				return;
			}
			if (!this.materialForRenderingCached)
			{
				this.materialForRenderingCached = this.materialForRendering;
			}
			this.materialForRenderingCached.SetTexture(TranslucentImage._blurTexPropId, this.source.BlurredScreen);
			if (this.isBirp || base.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.materialForRenderingCached.SetVector(TranslucentImage._cropRegionPropId, this.source.BlurRegionNormalizedScreenSpace.ToMinMaxVector());
			}
			else
			{
				this.materialForRenderingCached.SetVector(TranslucentImage._cropRegionPropId, this.source.BlurRegion.ToMinMaxVector());
			}
			this.source.Request();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002704 File Offset: 0x00000904
		private void Update()
		{
			this.shouldRun = this.Validate();
			if (!this.shouldRun)
			{
				return;
			}
			if (TranslucentImage._vibrancyPropId == 0 || TranslucentImage._brightnessPropId == 0 || TranslucentImage._flattenPropId == 0)
			{
				return;
			}
			this.materialForRenderingCached = this.materialForRendering;
			this.SyncMaterialProperty(TranslucentImage._vibrancyPropId, ref this.vibrancy, ref this.oldVibrancy);
			this.SyncMaterialProperty(TranslucentImage._brightnessPropId, ref this.brightness, ref this.oldBrightness);
			this.SyncMaterialProperty(TranslucentImage._flattenPropId, ref this.flatten, ref this.oldFlatten);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002790 File Offset: 0x00000990
		private void SyncMaterialProperty(int propId, ref float value, ref float oldValue)
		{
			float @float = this.materialForRendering.GetFloat(propId);
			if ((double)Mathf.Abs(@float - value) > 0.0001)
			{
				if ((double)Mathf.Abs(value - oldValue) > 0.0001)
				{
					if (this.materialForRenderingCached)
					{
						this.materialForRenderingCached.SetFloat(propId, value);
					}
					this.material.SetFloat(propId, value);
					this.SetMaterialDirty();
				}
				else
				{
					value = @float;
				}
			}
			oldValue = value;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000280D File Offset: 0x00000A0D
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002815 File Offset: 0x00000A15
		public float spriteBlending
		{
			get
			{
				return this.m_spriteBlending;
			}
			set
			{
				this.m_spriteBlending = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002824 File Offset: 0x00000A24
		public virtual void ModifyMesh(VertexHelper vh)
		{
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			for (int i = 0; i < list.Count; i++)
			{
				UIVertex value = list[i];
				value.uv1 = new Vector2(this.spriteBlending, 0f);
				list[i] = value;
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002888 File Offset: 0x00000A88
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetVerticesDirty();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002896 File Offset: 0x00000A96
		protected override void OnDisable()
		{
			this.SetVerticesDirty();
			base.OnDisable();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028A4 File Offset: 0x00000AA4
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetVerticesDirty();
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000028B4 File Offset: 0x00000AB4
		public virtual void ModifyMesh(Mesh mesh)
		{
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				this.ModifyMesh(vertexHelper);
				vertexHelper.FillMesh(mesh);
			}
		}

		// Token: 0x04000011 RID: 17
		public TranslucentImageSource source;

		// Token: 0x04000012 RID: 18
		[Tooltip("(De)Saturate the image, 1 is normal, 0 is black and white, below zero make the image negative")]
		[Range(-1f, 3f)]
		public float vibrancy = 1f;

		// Token: 0x04000013 RID: 19
		[Tooltip("Brighten/darken the image")]
		[Range(-1f, 1f)]
		public float brightness;

		// Token: 0x04000014 RID: 20
		[Tooltip("Flatten the color behind to help keep contrast on varying background")]
		[Range(0f, 1f)]
		public float flatten = 0.1f;

		// Token: 0x04000015 RID: 21
		private static readonly int _vibrancyPropId = Shader.PropertyToID("_Vibrancy");

		// Token: 0x04000016 RID: 22
		private static readonly int _brightnessPropId = Shader.PropertyToID("_Brightness");

		// Token: 0x04000017 RID: 23
		private static readonly int _flattenPropId = Shader.PropertyToID("_Flatten");

		// Token: 0x04000018 RID: 24
		private static readonly int _blurTexPropId = Shader.PropertyToID("_BlurTex");

		// Token: 0x04000019 RID: 25
		private static readonly int _cropRegionPropId = Shader.PropertyToID("_CropRegion");

		// Token: 0x0400001A RID: 26
		private Material materialForRenderingCached;

		// Token: 0x0400001B RID: 27
		private bool shouldRun;

		// Token: 0x0400001C RID: 28
		private bool isBirp;

		// Token: 0x0400001D RID: 29
		private bool sourceAcquiredOnStart;

		// Token: 0x0400001E RID: 30
		private float oldVibrancy;

		// Token: 0x0400001F RID: 31
		private float oldBrightness;

		// Token: 0x04000020 RID: 32
		private float oldFlatten;

		// Token: 0x04000021 RID: 33
		[Tooltip("Blend between the sprite and background blur")]
		[Range(0f, 1f)]
		[FormerlySerializedAs("spriteBlending")]
		public float m_spriteBlending = 0.65f;
	}
}
