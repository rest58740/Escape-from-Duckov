using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LeTai.TrueShadow.PluginInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LeTai.TrueShadow
{
	// Token: 0x02000017 RID: 23
	[RequireComponent(typeof(Graphic))]
	[HelpURL("https://leloctai.com/trueshadow/docs/articles/customize.html")]
	[ExecuteAlways]
	public class TrueShadow : UIBehaviour, IMeshModifier, ICanvasElement
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004F02 File Offset: 0x00003102
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00004F09 File Offset: 0x00003109
		public static bool ExternalActive
		{
			get
			{
				return TrueShadow._externalActive;
			}
			set
			{
				TrueShadow._externalActive = value;
				Action<bool> onSetExternalActive = TrueShadow.OnSetExternalActive;
				if (onSetExternalActive == null)
				{
					return;
				}
				onSetExternalActive(value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004F21 File Offset: 0x00003121
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004F29 File Offset: 0x00003129
		public bool IgnoreExternalActive
		{
			get
			{
				return this.ignoreExternalActive;
			}
			set
			{
				this.ignoreExternalActive = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004F32 File Offset: 0x00003132
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00004F3C File Offset: 0x0000313C
		public float Size
		{
			get
			{
				return this.size;
			}
			set
			{
				float b = Mathf.Max(0f, value);
				if (this.modifiedFromInspector || !Mathf.Approximately(this.size, b))
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
					this.SetTextureDirty();
				}
				this.size = b;
				if (this.Inset && this.OffsetDistance > this.Size)
				{
					this.OffsetDistance = this.Size;
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004FA7 File Offset: 0x000031A7
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004FB0 File Offset: 0x000031B0
		public float Spread
		{
			get
			{
				return this.spread;
			}
			set
			{
				float b = Mathf.Clamp01(value);
				if (this.modifiedFromInspector || !Mathf.Approximately(this.spread, b))
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
					this.SetTextureDirty();
				}
				this.spread = b;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004FF4 File Offset: 0x000031F4
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004FFC File Offset: 0x000031FC
		public bool UseGlobalAngle
		{
			get
			{
				return this.useGlobalAngle;
			}
			set
			{
				this.useGlobalAngle = value;
				ProjectSettings.Instance.globalAngleChanged -= this.OnGlobalAngleChanged;
				float globalAngle = ProjectSettings.Instance.GlobalAngle;
				if (this.useGlobalAngle)
				{
					this.offset = Math.AngleDistanceVector(globalAngle, this.offset.magnitude, Vector2.right);
					this.SetLayoutDirty();
					if (this.Cutout)
					{
						this.SetTextureDirty();
					}
					ProjectSettings.Instance.globalAngleChanged += this.OnGlobalAngleChanged;
					return;
				}
				float num = this.offsetAngle;
				this.OffsetAngle = globalAngle;
				this.OffsetAngle = num;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00005095 File Offset: 0x00003295
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000050A0 File Offset: 0x000032A0
		public float OffsetAngle
		{
			get
			{
				return this.offsetAngle;
			}
			set
			{
				if (this.UseGlobalAngle)
				{
					return;
				}
				float b = (value + 360f) % 360f;
				if (this.modifiedFromInspector || !Mathf.Approximately(this.offsetAngle, b))
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
					if (this.Cutout)
					{
						this.SetTextureDirty();
					}
				}
				this.offsetAngle = b;
				this.offset = Math.AngleDistanceVector(this.offsetAngle, this.offset.magnitude, Vector2.right);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000511D File Offset: 0x0000331D
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00005128 File Offset: 0x00003328
		public float OffsetDistance
		{
			get
			{
				return this.offsetDistance;
			}
			set
			{
				float b;
				if (this.Inset)
				{
					b = Mathf.Clamp(value, 0f, this.Size);
				}
				else
				{
					b = Mathf.Max(0f, value);
				}
				if (this.modifiedFromInspector || !Mathf.Approximately(this.offsetDistance, b))
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
					if (this.Cutout)
					{
						this.SetTextureDirty();
					}
				}
				this.offsetDistance = b;
				this.offset = ((this.offset.sqrMagnitude < 1E-06f) ? Math.AngleDistanceVector(this.offsetAngle, this.offsetDistance, Vector2.right) : (this.offset.normalized * this.offsetDistance));
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000051DD File Offset: 0x000033DD
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000051E5 File Offset: 0x000033E5
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (this.modifiedFromInspector || value != this.color)
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
				}
				this.color = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00005211 File Offset: 0x00003411
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00005219 File Offset: 0x00003419
		public bool UseCasterAlpha
		{
			get
			{
				return this.useCasterAlpha;
			}
			set
			{
				if (this.modifiedFromInspector || value != this.useCasterAlpha)
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
				}
				this.useCasterAlpha = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00005240 File Offset: 0x00003440
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00005248 File Offset: 0x00003448
		public bool IgnoreCasterColor
		{
			get
			{
				return this.ignoreCasterColor;
			}
			set
			{
				if (this.modifiedFromInspector || value != this.ignoreCasterColor)
				{
					this.modifiedFromInspector = false;
					this.SetTextureDirty();
				}
				this.ignoreCasterColor = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000526F File Offset: 0x0000346F
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00005278 File Offset: 0x00003478
		public bool Inset
		{
			get
			{
				return this.inset;
			}
			set
			{
				if (this.modifiedFromInspector || value != this.inset)
				{
					this.modifiedFromInspector = false;
					this.SetLayoutDirty();
					this.SetTextureDirty();
				}
				this.inset = value;
				if (this.Inset && this.OffsetDistance > this.Size)
				{
					this.OffsetDistance = this.Size;
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000052D2 File Offset: 0x000034D2
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000052DC File Offset: 0x000034DC
		public BlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				if (!this.Graphic || !this.CanvasRenderer)
				{
					return;
				}
				this.blendMode = value;
				this.shadowRenderer.UpdateMaterial();
				BlendMode blendMode = this.blendMode;
				if (blendMode <= BlendMode.Multiply)
				{
					this.ColorBleedMode = ColorBleedMode.Black;
					return;
				}
				this.ColorBleedMode = ColorBleedMode.Black;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005330 File Offset: 0x00003530
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00005338 File Offset: 0x00003538
		public ColorBleedMode ColorBleedMode
		{
			get
			{
				return this.colorBleedMode;
			}
			set
			{
				if (this.modifiedFromInspector || this.colorBleedMode != value)
				{
					this.modifiedFromInspector = false;
					this.colorBleedMode = value;
					this.SetTextureDirty();
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000535F File Offset: 0x0000355F
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00005367 File Offset: 0x00003567
		public bool DisableFitCompensation
		{
			get
			{
				return this.disableFitCompensation;
			}
			set
			{
				if (this.modifiedFromInspector || this.disableFitCompensation != value)
				{
					this.modifiedFromInspector = false;
					this.disableFitCompensation = value;
					this.SetLayoutDirty();
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005390 File Offset: 0x00003590
		public Color ClearColor
		{
			get
			{
				switch (this.colorBleedMode)
				{
				case ColorBleedMode.ImageColor:
					return this.Graphic.color.WithA(0f);
				case ColorBleedMode.ShadowColor:
					return this.Color.WithA(0f);
				case ColorBleedMode.Black:
					return Color.clear;
				case ColorBleedMode.White:
					return new Color(1f, 1f, 1f, 0f);
				case ColorBleedMode.Plugin:
				{
					ITrueShadowCasterClearColorProvider trueShadowCasterClearColorProvider = this.casterClearColorProvider;
					if (trueShadowCasterClearColorProvider == null)
					{
						return Color.clear;
					}
					return trueShadowCasterClearColorProvider.GetTrueShadowCasterClearColor();
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005422 File Offset: 0x00003622
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000542C File Offset: 0x0000362C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShadowAsSibling
		{
			get
			{
				return this.shadowAsSibling;
			}
			set
			{
				this.shadowAsSibling = value;
				ShadowRenderer.ClearMaskMaterialCache();
				if (this.shadowAsSibling)
				{
					ShadowSorter.Instance.Register(this);
					return;
				}
				ShadowSorter.Instance.UnRegister(this);
				if (this.shadowRenderer)
				{
					Transform transform = this.shadowRenderer.transform;
					transform.SetParent(base.transform, true);
					transform.SetSiblingIndex(0);
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000548F File Offset: 0x0000368F
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00005497 File Offset: 0x00003697
		public int CustomHash
		{
			get
			{
				return this.customHash;
			}
			set
			{
				if (this.customHash != value)
				{
					this.SetTextureDirty();
				}
				this.customHash = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000054AF File Offset: 0x000036AF
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000054B7 File Offset: 0x000036B7
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000054C9 File Offset: 0x000036C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool Cutout
		{
			get
			{
				return !this.shadowAsSibling || this.cutout;
			}
			set
			{
				this.cutout = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000054D2 File Offset: 0x000036D2
		// (set) Token: 0x060000BC RID: 188 RVA: 0x000054DA File Offset: 0x000036DA
		internal Mesh SpriteMesh { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000054E3 File Offset: 0x000036E3
		// (set) Token: 0x060000BE RID: 190 RVA: 0x000054EB File Offset: 0x000036EB
		internal Graphic Graphic { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000054F4 File Offset: 0x000036F4
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x000054FC File Offset: 0x000036FC
		internal CanvasRenderer CanvasRenderer { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005505 File Offset: 0x00003705
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000550D File Offset: 0x0000370D
		internal RectTransform RectTransform { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005518 File Offset: 0x00003718
		internal Texture Content
		{
			get
			{
				Graphic graphic = this.Graphic;
				Image image = graphic as Image;
				if (image == null)
				{
					RawImage rawImage = graphic as RawImage;
					if (rawImage != null)
					{
						return rawImage.texture;
					}
					TextMeshProUGUI textMeshProUGUI = graphic as TextMeshProUGUI;
					if (textMeshProUGUI != null)
					{
						return textMeshProUGUI.materialForRendering.mainTexture;
					}
					TMP_SubMeshUI tmp_SubMeshUI = graphic as TMP_SubMeshUI;
					if (tmp_SubMeshUI == null)
					{
						return this.Graphic.mainTexture;
					}
					return tmp_SubMeshUI.materialForRendering.mainTexture;
				}
				else
				{
					Sprite overrideSprite = image.overrideSprite;
					if (!overrideSprite)
					{
						return null;
					}
					return overrideSprite.texture;
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000055A0 File Offset: 0x000037A0
		internal ShadowContainer ShadowContainer
		{
			get
			{
				return this.shadowContainer;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000055A8 File Offset: 0x000037A8
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000055B0 File Offset: 0x000037B0
		internal bool HierachyDirty { get; private set; }

		// Token: 0x060000C7 RID: 199 RVA: 0x000055B9 File Offset: 0x000037B9
		private void OnGlobalAngleChanged(float angle)
		{
			this.offset = Math.AngleDistanceVector(angle, this.offset.magnitude, Vector2.right);
			this.SetLayoutDirty();
			if (this.Cutout)
			{
				this.SetTextureDirty();
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000055EB File Offset: 0x000037EB
		protected override void Awake()
		{
			if (this.ShadowAsSibling)
			{
				ShadowSorter.Instance.Register(this);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005600 File Offset: 0x00003800
		protected override void OnEnable()
		{
			TrueShadow[] components = base.GetComponents<TrueShadow>();
			int num = 0;
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] == this || components[i].shadowRenderer)
				{
					components[i].shadowIndex = num++;
				}
			}
			this.RectTransform = base.GetComponent<RectTransform>();
			this.Graphic = base.GetComponent<Graphic>();
			this.CanvasRenderer = base.GetComponent<CanvasRenderer>();
			if (!this.SpriteMesh)
			{
				this.SpriteMesh = new Mesh();
			}
			this.InitializePlugins();
			if (this.bakedShadows == null)
			{
				this.bakedShadows = new List<Sprite>(4);
			}
			this.InitInvalidator();
			ShadowRenderer.Initialize(this, ref this.shadowRenderer);
			Canvas.willRenderCanvases += this.OnWillRenderCanvas;
			if (this.UseGlobalAngle)
			{
				ProjectSettings.Instance.globalAngleChanged -= this.OnGlobalAngleChanged;
				ProjectSettings.Instance.globalAngleChanged += this.OnGlobalAngleChanged;
			}
			if (this.Graphic)
			{
				this.Graphic.SetVerticesDirty();
			}
			this.NextFrames(new Action(this.CopyToTMPSubMeshes), 2);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005724 File Offset: 0x00003924
		public void ApplySerializedData()
		{
			this.Size = this.size;
			this.Spread = this.spread;
			this.OffsetAngle = this.offsetAngle;
			this.OffsetDistance = this.offsetDistance;
			this.BlendMode = this.blendMode;
			this.ShadowAsSibling = this.shadowAsSibling;
			this.SetHierachyDirty();
			this.SetLayoutDirty();
			this.SetTextureDirty();
			if (this.shadowRenderer)
			{
				this.shadowRenderer.SetMaterialDirty();
			}
			this.CopyToTMPSubMeshes();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000057AC File Offset: 0x000039AC
		protected override void OnDisable()
		{
			ProjectSettings.Instance.globalAngleChanged -= this.OnGlobalAngleChanged;
			Canvas.willRenderCanvases -= this.OnWillRenderCanvas;
			this.TerminateInvalidator();
			this.TerminatePlugins();
			if (this.shadowRenderer)
			{
				this.shadowRenderer.gameObject.SetActive(false);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000580A File Offset: 0x00003A0A
		protected override void OnDestroy()
		{
			ShadowSorter.Instance.UnRegister(this);
			if (this.shadowRenderer)
			{
				this.shadowRenderer.Dispose();
			}
			ShadowFactory.Instance.ReleaseContainer(ref this.shadowContainer);
			base.StopAllCoroutines();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005848 File Offset: 0x00003A48
		private bool ShouldPerformWorks()
		{
			if (!this.ignoreExternalActive && !TrueShadow.ExternalActive)
			{
				return false;
			}
			bool flag = this.CanvasRenderer && this.CanvasRenderer.cull && this.shadowRenderer.CanvasRenderer && this.shadowRenderer.CanvasRenderer.cull;
			return base.isActiveAndEnabled && !flag;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000058B4 File Offset: 0x00003AB4
		private void LateUpdate()
		{
			if (this.shadowRenderer)
			{
				this.shadowRenderer.gameObject.SetActive(this.Graphic && this.Graphic.isActiveAndEnabled);
			}
			if (!this.ShouldPerformWorks())
			{
				return;
			}
			this.CheckTransformDirtied();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005908 File Offset: 0x00003B08
		public void Rebuild(CanvasUpdate executing)
		{
			if (!this.ShouldPerformWorks())
			{
				return;
			}
			if (executing == CanvasUpdate.PostLayout && this.layoutDirty)
			{
				this.shadowRenderer.ReLayout();
				this.layoutDirty = false;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005934 File Offset: 0x00003B34
		private void OnWillRenderCanvas()
		{
			if (this == null)
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.ShouldPerformWorks())
			{
				return;
			}
			if (this.textureDirty && this.Graphic && this.Graphic.canvas)
			{
				ShadowFactory.Instance.Get(new ShadowSettingSnapshot(this), ref this.shadowContainer);
				ShadowRenderer shadowRenderer = this.shadowRenderer;
				ShadowContainer shadowContainer = this.shadowContainer;
				shadowRenderer.SetTexture((shadowContainer != null) ? shadowContainer.Texture : null);
				this.textureDirty = false;
			}
			if (!this.shadowAsSibling)
			{
				if (this.shadowRenderer.transform.parent != this.RectTransform)
				{
					this.shadowRenderer.transform.SetParent(this.RectTransform, true);
				}
				if (this.shadowRenderer.transform.GetSiblingIndex() != this.shadowIndex)
				{
					this.shadowRenderer.transform.SetSiblingIndex(this.shadowIndex);
				}
				this.UnSetHierachyDirty();
				if (this.layoutDirty)
				{
					this.shadowRenderer.ReLayout();
					this.layoutDirty = false;
				}
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005A4B File Offset: 0x00003C4B
		public void LayoutComplete()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005A4D File Offset: 0x00003C4D
		public void GraphicUpdateComplete()
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005A4F File Offset: 0x00003C4F
		public void SetTextureDirty()
		{
			this.textureDirty = true;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005A58 File Offset: 0x00003C58
		public void SetLayoutDirty()
		{
			this.layoutDirty = true;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005A61 File Offset: 0x00003C61
		public void SetHierachyDirty()
		{
			this.HierachyDirty = true;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005A6A File Offset: 0x00003C6A
		internal void UnSetHierachyDirty()
		{
			this.HierachyDirty = false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005A74 File Offset: 0x00003C74
		public void CopyTo(TrueShadow other)
		{
			other.Inset = this.Inset;
			other.Size = this.Size;
			other.Spread = this.Spread;
			other.UseGlobalAngle = this.UseGlobalAngle;
			other.OffsetAngle = this.OffsetAngle;
			other.OffsetDistance = this.OffsetDistance;
			other.Color = this.Color;
			other.BlendMode = this.BlendMode;
			other.UseCasterAlpha = this.UseCasterAlpha;
			other.IgnoreCasterColor = this.IgnoreCasterColor;
			other.ColorBleedMode = this.ColorBleedMode;
			other.DisableFitCompensation = this.DisableFitCompensation;
			other.SetLayoutTextureDirty();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005B18 File Offset: 0x00003D18
		public void CopyTo(GameObject other)
		{
			TrueShadow component = other.GetComponent<TrueShadow>();
			if (component)
			{
				this.CopyTo(component);
				return;
			}
			TrueShadow other2 = other.AddComponent<TrueShadow>();
			this.CopyTo(other2);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005B4C File Offset: 0x00003D4C
		public void CopyToTMPSubMeshes()
		{
			if (!(this.Graphic is TextMeshProUGUI))
			{
				return;
			}
			TMP_SubMeshUI[] componentsInChildren = base.GetComponentsInChildren<TMP_SubMeshUI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.CopyTo(componentsInChildren[i].gameObject);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005B8C File Offset: 0x00003D8C
		private void InitInvalidator()
		{
			this.checkHierarchyDirtiedDelegate = new Action(this.CheckHierarchyDirtied);
			this.hierachyTrackers = new ChangeTracker<int>[]
			{
				new ChangeTracker<int>(() => this.RectTransform.GetSiblingIndex(), delegate(int newValue)
				{
					this.SetHierachyDirty();
					return newValue;
				}, null),
				new ChangeTracker<int>(delegate()
				{
					if (this.shadowRenderer)
					{
						return this.shadowRenderer.transform.GetSiblingIndex();
					}
					return -1;
				}, delegate(int newValue)
				{
					this.SetHierachyDirty();
					return newValue;
				}, null)
			};
			IChangeTracker[] array = new IChangeTracker[2];
			array[0] = new ChangeTracker<Vector3>(() => this.RectTransform.position, delegate(Vector3 newValue)
			{
				this.SetLayoutDirty();
				return newValue;
			}, (Vector3 prev, Vector3 curr) => prev == curr);
			array[1] = new ChangeTracker<Quaternion>(() => this.RectTransform.rotation, delegate(Quaternion newValue)
			{
				this.SetLayoutDirty();
				if (this.Cutout)
				{
					this.SetTextureDirty();
				}
				return newValue;
			}, (Quaternion prev, Quaternion curr) => prev == curr);
			this.transformTrackers = array;
			if (this.Graphic is TextMeshProUGUI || this.Graphic is TMP_SubMeshUI)
			{
				IChangeTracker[] array2 = this.transformTrackers;
				this.transformTrackers = new IChangeTracker[array2.Length + 1];
				Array.Copy(array2, this.transformTrackers, array2.Length);
				this.transformTrackers[array2.Length] = new ChangeTracker<Vector3>(() => this.RectTransform.lossyScale, delegate(Vector3 newValue)
				{
					this.SetLayoutTextureDirty();
					return newValue;
				}, delegate(Vector3 prev, Vector3 curr)
				{
					if (prev == curr)
					{
						return true;
					}
					if (prev.x * prev.y * prev.z < 1E-09f && curr.x * curr.y * curr.z > 1E-09f)
					{
						return false;
					}
					Vector3 vector = curr - prev;
					return Mathf.Abs(vector.x / prev.x) < 0.25f && Mathf.Abs(vector.y / prev.y) < 0.25f && Mathf.Abs(vector.z / prev.z) < 0.25f;
				});
			}
			this.Graphic.RegisterDirtyLayoutCallback(new UnityAction(this.SetLayoutTextureDirty));
			this.Graphic.RegisterDirtyVerticesCallback(new UnityAction(this.SetLayoutTextureDirty));
			this.Graphic.RegisterDirtyMaterialCallback(new UnityAction(this.OnGraphicMaterialDirty));
			this.CheckHierarchyDirtied();
			this.CheckTransformDirtied();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005D5C File Offset: 0x00003F5C
		private void TerminateInvalidator()
		{
			if (this.Graphic)
			{
				this.Graphic.UnregisterDirtyLayoutCallback(new UnityAction(this.SetLayoutTextureDirty));
				this.Graphic.UnregisterDirtyVerticesCallback(new UnityAction(this.SetLayoutTextureDirty));
				this.Graphic.UnregisterDirtyMaterialCallback(new UnityAction(this.OnGraphicMaterialDirty));
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005DBB File Offset: 0x00003FBB
		private void OnGraphicMaterialDirty()
		{
			this.SetLayoutTextureDirty();
			this.shadowRenderer.UpdateMaterial();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005DD0 File Offset: 0x00003FD0
		internal void CheckTransformDirtied()
		{
			if (this.transformTrackers != null)
			{
				for (int i = 0; i < this.transformTrackers.Length; i++)
				{
					this.transformTrackers[i].Check();
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005E08 File Offset: 0x00004008
		internal void CheckHierarchyDirtied()
		{
			if (this.ShadowAsSibling && this.hierachyTrackers != null)
			{
				for (int i = 0; i < this.hierachyTrackers.Length; i++)
				{
					this.hierachyTrackers[i].Check();
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005E48 File Offset: 0x00004048
		internal void ForgetSiblingIndexChanges()
		{
			for (int i = 0; i < this.hierachyTrackers.Length; i++)
			{
				this.hierachyTrackers[i].Forget();
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005E75 File Offset: 0x00004075
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.SetHierachyDirty();
			this.NextFrames(this.checkHierarchyDirtiedDelegate, 1);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005E99 File Offset: 0x00004099
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.SetLayoutTextureDirty();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005EB0 File Offset: 0x000040B0
		protected override void OnDidApplyAnimationProperties()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.SetLayoutTextureDirty();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005EC1 File Offset: 0x000040C1
		public void ModifyMesh(Mesh mesh)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.SpriteMesh)
			{
				Utility.SafeDestroy(this.SpriteMesh);
			}
			this.SpriteMesh = UnityEngine.Object.Instantiate<Mesh>(mesh);
			this.SetLayoutTextureDirty();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005EF6 File Offset: 0x000040F6
		public void ModifyMesh(VertexHelper verts)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.SpriteMesh)
			{
				this.SpriteMesh = new Mesh();
			}
			verts.FillMesh(this.SpriteMesh);
			this.SetLayoutTextureDirty();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005F2C File Offset: 0x0000412C
		private void SetLayoutTextureDirty()
		{
			TextMeshProUGUI textMeshProUGUI = this.Graphic as TextMeshProUGUI;
			if (textMeshProUGUI != null)
			{
				this.SpriteMesh = (string.IsNullOrEmpty(textMeshProUGUI.text) ? null : textMeshProUGUI.mesh);
			}
			else
			{
				TMP_SubMeshUI tmp_SubMeshUI = this.Graphic as TMP_SubMeshUI;
				if (tmp_SubMeshUI != null)
				{
					this.SpriteMesh = (string.IsNullOrEmpty(tmp_SubMeshUI.textComponent.text) ? null : tmp_SubMeshUI.mesh);
				}
			}
			this.SetLayoutDirty();
			this.SetTextureDirty();
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005FA2 File Offset: 0x000041A2
		public bool UsingRendererMaterialProvider
		{
			get
			{
				return this.rendererMaterialProvider != null;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005FB0 File Offset: 0x000041B0
		private void InitializePlugins()
		{
			this.casterMaterialProvider = base.GetComponent<ITrueShadowCasterMaterialProvider>();
			this.casterMaterialPropertiesModifier = base.GetComponent<ITrueShadowCasterMaterialPropertiesModifier>();
			this.casterMeshModifier = base.GetComponent<ITrueShadowCasterMeshModifier>();
			this.casterClearColorProvider = base.GetComponent<ITrueShadowCasterClearColorProvider>();
			if (this.casterClearColorProvider != null)
			{
				this.ColorBleedMode = ColorBleedMode.Plugin;
			}
			this.rendererMaterialProvider = base.GetComponent<ITrueShadowRendererMaterialProvider>();
			this.rendererMaterialModifier = base.GetComponent<ITrueShadowRendererMaterialModifier>();
			this.rendererMeshModifier = base.GetComponent<ITrueShadowRendererMeshModifier>();
			if (this.casterMaterialProvider != null)
			{
				this.casterMaterialProvider.materialReplaced += this.HandleCasterMaterialReplaced;
				this.casterMaterialProvider.materialModified += this.HandleCasterMaterialModified;
			}
			if (this.rendererMaterialProvider != null)
			{
				this.rendererMaterialProvider.materialReplaced += this.HandleRendererMaterialReplaced;
				this.rendererMaterialProvider.materialModified += this.HandleRendererMaterialModified;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000608C File Offset: 0x0000428C
		private void TerminatePlugins()
		{
			if (this.casterMaterialProvider != null)
			{
				this.casterMaterialProvider.materialReplaced -= this.HandleCasterMaterialReplaced;
				this.casterMaterialProvider.materialModified -= this.HandleCasterMaterialModified;
			}
			if (this.rendererMaterialProvider != null)
			{
				this.rendererMaterialProvider.materialReplaced -= this.HandleRendererMaterialReplaced;
				this.rendererMaterialProvider.materialModified -= this.HandleRendererMaterialModified;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006105 File Offset: 0x00004305
		public void RefreshPlugins()
		{
			this.TerminatePlugins();
			this.InitializePlugins();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006113 File Offset: 0x00004313
		private void HandleCasterMaterialReplaced()
		{
			this.SetTextureDirty();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000611B File Offset: 0x0000431B
		private void HandleRendererMaterialReplaced()
		{
			if (this.shadowRenderer)
			{
				this.shadowRenderer.UpdateMaterial();
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006135 File Offset: 0x00004335
		private void HandleCasterMaterialModified()
		{
			this.SetTextureDirty();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000613D File Offset: 0x0000433D
		private void HandleRendererMaterialModified()
		{
			if (this.shadowRenderer)
			{
				this.shadowRenderer.SetMaterialDirty();
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006158 File Offset: 0x00004358
		public virtual Material GetShadowCastingMaterial()
		{
			Material material = null;
			if (this.casterMaterialProvider != null)
			{
				material = this.casterMaterialProvider.GetTrueShadowCasterMaterial();
			}
			else if (this.Graphic is TextMeshProUGUI || this.Graphic is TMP_SubMeshUI)
			{
				material = this.Graphic.materialForRendering;
			}
			if (!(material != null))
			{
				return this.Graphic.material;
			}
			return material;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000061B9 File Offset: 0x000043B9
		public virtual void ModifyShadowCastingMaterialProperties(MaterialPropertyBlock propertyBlock)
		{
			ITrueShadowCasterMaterialPropertiesModifier trueShadowCasterMaterialPropertiesModifier = this.casterMaterialPropertiesModifier;
			if (trueShadowCasterMaterialPropertiesModifier == null)
			{
				return;
			}
			trueShadowCasterMaterialPropertiesModifier.ModifyTrueShadowCasterMaterialProperties(propertyBlock);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000061CC File Offset: 0x000043CC
		public virtual void ModifyShadowCastingMesh(Mesh mesh)
		{
			ITrueShadowCasterMeshModifier trueShadowCasterMeshModifier = this.casterMeshModifier;
			if (trueShadowCasterMeshModifier != null)
			{
				trueShadowCasterMeshModifier.ModifyTrueShadowCasterMesh(mesh);
			}
			this.MakeOpaque(mesh);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000061E8 File Offset: 0x000043E8
		private void MakeOpaque(Mesh mesh)
		{
			if (this.shadowAsSibling)
			{
				return;
			}
			mesh.GetColors(this.meshColors);
			int count = this.meshColors.Count;
			if (count < 1)
			{
				return;
			}
			if (this.meshColorsOpaque.Count == count)
			{
				if (this.meshColors[0].a == this.meshColorsOpaque[0].a)
				{
					return;
				}
			}
			else
			{
				this.meshColorsOpaque.Clear();
				this.meshColorsOpaque.AddRange(Enumerable.Repeat<Color32>(new Color32(0, 0, 0, 0), count));
			}
			for (int i = 0; i < count; i++)
			{
				Color32 value = this.meshColors[i];
				value.a = byte.MaxValue;
				this.meshColorsOpaque[i] = value;
			}
			mesh.SetColors(this.meshColorsOpaque);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000062B4 File Offset: 0x000044B4
		public virtual Material GetShadowRenderingMaterial()
		{
			ITrueShadowRendererMaterialProvider trueShadowRendererMaterialProvider = this.rendererMaterialProvider;
			Material material = (trueShadowRendererMaterialProvider != null) ? trueShadowRendererMaterialProvider.GetTrueShadowRendererMaterial() : null;
			if (!(material != null))
			{
				return this.BlendMode.GetMaterial();
			}
			return material;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000062EA File Offset: 0x000044EA
		public virtual void ModifyShadowRendererMaterial(Material baseMaterial)
		{
			ITrueShadowRendererMaterialModifier trueShadowRendererMaterialModifier = this.rendererMaterialModifier;
			if (trueShadowRendererMaterialModifier == null)
			{
				return;
			}
			trueShadowRendererMaterialModifier.ModifyTrueShadowRendererMaterial(baseMaterial);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000062FD File Offset: 0x000044FD
		public virtual void ModifyShadowRendererMesh(VertexHelper vertexHelper)
		{
			ITrueShadowRendererMeshModifier trueShadowRendererMeshModifier = this.rendererMeshModifier;
			if (trueShadowRendererMeshModifier == null)
			{
				return;
			}
			trueShadowRendererMeshModifier.ModifyTrueShadowRendererMesh(vertexHelper);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000063A6 File Offset: 0x000045A6
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x0400007E RID: 126
		private static bool _externalActive = true;

		// Token: 0x0400007F RID: 127
		public static Action<bool> OnSetExternalActive;

		// Token: 0x04000080 RID: 128
		private static readonly Color DEFAULT_COLOR = new Color(0f, 0f, 0f, 0.6f);

		// Token: 0x04000081 RID: 129
		[SerializeField]
		private bool ignoreExternalActive;

		// Token: 0x04000082 RID: 130
		[Tooltip("Size of the shadow")]
		[SerializeField]
		private float size = 32f;

		// Token: 0x04000083 RID: 131
		[Tooltip("Spread of the shadow")]
		[SpreadSlider]
		[SerializeField]
		private float spread;

		// Token: 0x04000084 RID: 132
		[SerializeField]
		private bool useGlobalAngle;

		// Token: 0x04000085 RID: 133
		[Tooltip("Direction to offset the shadow toward")]
		[SerializeField]
		private float offsetAngle = 90f;

		// Token: 0x04000086 RID: 134
		[Tooltip("How far to offset the shadow")]
		[SerializeField]
		private float offsetDistance = 12f;

		// Token: 0x04000087 RID: 135
		[SerializeField]
		private Vector2 offset = Vector2.zero;

		// Token: 0x04000088 RID: 136
		[Tooltip("Tint the shadow")]
		[SerializeField]
		private Color color = TrueShadow.DEFAULT_COLOR;

		// Token: 0x04000089 RID: 137
		[Tooltip("Inset shadow")]
		[InsetToggle]
		[SerializeField]
		private bool inset;

		// Token: 0x0400008A RID: 138
		[Tooltip("Blend mode of the shadow")]
		[SerializeField]
		private BlendMode blendMode;

		// Token: 0x0400008B RID: 139
		[FormerlySerializedAs("multiplyCasterAlpha")]
		[Tooltip("Allow shadow to cross-fade with caster")]
		[SerializeField]
		private bool useCasterAlpha = true;

		// Token: 0x0400008C RID: 140
		[Tooltip("Ignore the shadow caster's color, so you can choose specific color for your shadow")]
		[SerializeField]
		private bool ignoreCasterColor;

		// Token: 0x0400008D RID: 141
		[Tooltip("How to obtain the color of the area outside of the source image. Automatically set based on Blend Mode. You should only change this setting if you are using some very custom UI that require it")]
		[SerializeField]
		private ColorBleedMode colorBleedMode;

		// Token: 0x0400008E RID: 142
		[Tooltip("Improve shadow fit on some sprites")]
		[SerializeField]
		private bool disableFitCompensation;

		// Token: 0x0400008F RID: 143
		[Tooltip("Position the shadow GameObject as previous sibling of the UI element")]
		[SerializeField]
		private bool shadowAsSibling;

		// Token: 0x04000090 RID: 144
		[Tooltip("Cut the source image from the shadow to avoid shadow showing behind semi-transparent UI")]
		[SerializeField]
		private bool cutout;

		// Token: 0x04000091 RID: 145
		[Tooltip("Bake the shadow to a sprite to reduce CPU and GPU usage at runtime, at the cost of storage, memory and flexibility")]
		[SerializeField]
		private bool baked;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		private bool modifiedFromInspector;

		// Token: 0x04000093 RID: 147
		[SerializeField]
		private List<Sprite> bakedShadows;

		// Token: 0x04000094 RID: 148
		internal ShadowRenderer shadowRenderer;

		// Token: 0x04000099 RID: 153
		private ShadowContainer shadowContainer;

		// Token: 0x0400009A RID: 154
		private int customHash;

		// Token: 0x0400009B RID: 155
		private bool textureDirty;

		// Token: 0x0400009C RID: 156
		private bool layoutDirty;

		// Token: 0x0400009D RID: 157
		private int shadowIndex = -1;

		// Token: 0x0400009F RID: 159
		private Action checkHierarchyDirtiedDelegate;

		// Token: 0x040000A0 RID: 160
		private IChangeTracker[] transformTrackers;

		// Token: 0x040000A1 RID: 161
		private ChangeTracker<int>[] hierachyTrackers;

		// Token: 0x040000A2 RID: 162
		private ITrueShadowCasterMaterialProvider casterMaterialProvider;

		// Token: 0x040000A3 RID: 163
		private ITrueShadowCasterMaterialPropertiesModifier casterMaterialPropertiesModifier;

		// Token: 0x040000A4 RID: 164
		private ITrueShadowCasterMeshModifier casterMeshModifier;

		// Token: 0x040000A5 RID: 165
		private ITrueShadowCasterClearColorProvider casterClearColorProvider;

		// Token: 0x040000A6 RID: 166
		private ITrueShadowRendererMaterialProvider rendererMaterialProvider;

		// Token: 0x040000A7 RID: 167
		private ITrueShadowRendererMaterialModifier rendererMaterialModifier;

		// Token: 0x040000A8 RID: 168
		private ITrueShadowRendererMeshModifier rendererMeshModifier;

		// Token: 0x040000A9 RID: 169
		private readonly List<Color32> meshColors = new List<Color32>(4);

		// Token: 0x040000AA RID: 170
		private readonly List<Color32> meshColorsOpaque = new List<Color32>(4);
	}
}
