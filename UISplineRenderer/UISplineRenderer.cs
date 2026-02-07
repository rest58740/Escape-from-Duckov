using System;
using System.Collections.Generic;
using System.Linq;
using LeTai;
using LeTai.TrueShadow;
using LeTai.TrueShadow.PluginInterfaces;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace UI_Spline_Renderer
{
	// Token: 0x02000011 RID: 17
	[RequireComponent(typeof(CanvasRenderer), typeof(SplineContainer))]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class UISplineRenderer : MaskableGraphic, ITrueShadowCustomHashProvider
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000348C File Offset: 0x0000168C
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000034C4 File Offset: 0x000016C4
		public LineTexturePreset lineTexturePreset
		{
			get
			{
				if (this.m_Texture == UISplineRendererSettings.Instance.defaultLineTexture)
				{
					return LineTexturePreset.Default;
				}
				if (this.m_Texture == UISplineRendererSettings.Instance.uvTestLineTexture)
				{
					return LineTexturePreset.UVTest;
				}
				return LineTexturePreset.Custom;
			}
			set
			{
				switch (value)
				{
				case LineTexturePreset.Default:
					this.texture = UISplineRendererSettings.Instance.defaultLineTexture;
					return;
				case LineTexturePreset.UVTest:
					this.texture = UISplineRendererSettings.Instance.uvTestLineTexture;
					return;
				case LineTexturePreset.Custom:
					Debug.LogWarning("[UI Spline Renderer] If you want to change the line texture, just set value to the \"texture\" property. Then It will be automatically changed to LineTexturePreset.Custom");
					return;
				default:
					throw new ArgumentOutOfRangeException("value", value, null);
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003523 File Offset: 0x00001723
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003530 File Offset: 0x00001730
		public StartEndImagePreset startImagePreset
		{
			get
			{
				return InternalUtility.GetCurrentStartImagePreset(this.startImageSprite);
			}
			set
			{
				switch (value)
				{
				case StartEndImagePreset.None:
					this.startImageSprite = null;
					return;
				case StartEndImagePreset.Triangle:
					this.startImageSprite = UISplineRendererSettings.Instance.triangleHead;
					return;
				case StartEndImagePreset.Arrow:
					this.startImageSprite = UISplineRendererSettings.Instance.arrowHead;
					return;
				case StartEndImagePreset.EmptyCircle:
					this.startImageSprite = UISplineRendererSettings.Instance.emptyCircleHead;
					return;
				case StartEndImagePreset.FilledCircle:
					this.startImageSprite = UISplineRendererSettings.Instance.filledCircleHead;
					return;
				case StartEndImagePreset.Custom:
					Debug.LogWarning("[UI Spline Renderer] If you want to change the start image, just set value to the \"startImageSprite\" property. Then It will be automatically changed to StartEndImagePreset.Custom");
					return;
				default:
					throw new ArgumentOutOfRangeException("value", value, null);
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000035C5 File Offset: 0x000017C5
		// (set) Token: 0x06000043 RID: 67 RVA: 0x000035D4 File Offset: 0x000017D4
		public StartEndImagePreset endImagePreset
		{
			get
			{
				return InternalUtility.GetCurrentStartImagePreset(this.endImageSprite);
			}
			set
			{
				switch (value)
				{
				case StartEndImagePreset.None:
					this.endImageSprite = null;
					return;
				case StartEndImagePreset.Triangle:
					this.endImageSprite = UISplineRendererSettings.Instance.triangleHead;
					return;
				case StartEndImagePreset.Arrow:
					this.endImageSprite = UISplineRendererSettings.Instance.arrowHead;
					return;
				case StartEndImagePreset.EmptyCircle:
					this.endImageSprite = UISplineRendererSettings.Instance.emptyCircleHead;
					return;
				case StartEndImagePreset.FilledCircle:
					this.endImageSprite = UISplineRendererSettings.Instance.filledCircleHead;
					return;
				case StartEndImagePreset.Custom:
					Debug.LogWarning("[UI Spline Renderer] If you want to change the end image, just set value to the \"endImageSprite\" property. Then It will be automatically changed to StartEndImagePreset.Custom");
					return;
				default:
					throw new ArgumentOutOfRangeException("value", value, null);
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003669 File Offset: 0x00001869
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003671 File Offset: 0x00001871
		public override Color color
		{
			get
			{
				return base.color;
			}
			set
			{
				base.color = value;
				if (this.recursiveColor)
				{
					this.UpdateGraphicColors();
					this.UpdateTrueShadowCustomHash();
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000368E File Offset: 0x0000188E
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003696 File Offset: 0x00001896
		public bool recursiveColor
		{
			get
			{
				return this._recursiveColor;
			}
			set
			{
				this._recursiveColor = value;
				this.UpdateGraphicColors();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000036AB File Offset: 0x000018AB
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000036B4 File Offset: 0x000018B4
		public int resolution
		{
			get
			{
				return this._resolution;
			}
			set
			{
				value = Mathf.Clamp(value, 1, 10);
				float segmentLength;
				if (value > 1)
				{
					switch (value)
					{
					case 2:
						segmentLength = 0.02f;
						break;
					case 3:
						segmentLength = 0.05f;
						break;
					case 4:
						segmentLength = 0.08f;
						break;
					case 5:
						segmentLength = 0.12f;
						break;
					case 6:
						segmentLength = 0.18f;
						break;
					case 7:
						segmentLength = 0.24f;
						break;
					case 8:
						segmentLength = 0.32f;
						break;
					case 9:
						segmentLength = 0.4f;
						break;
					default:
						segmentLength = 0.5f;
						break;
					}
				}
				else
				{
					segmentLength = 0.01f;
				}
				this._segmentLength = segmentLength;
				this._resolution = value;
				this._needToResample = true;
				this.SetVerticesDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003769 File Offset: 0x00001969
		public override Texture mainTexture
		{
			get
			{
				if (!(this.m_Texture == null))
				{
					return this.m_Texture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003785 File Offset: 0x00001985
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000378D File Offset: 0x0000198D
		public Texture texture
		{
			get
			{
				return this.m_Texture;
			}
			set
			{
				if (value != null && this.m_Texture == value)
				{
					return;
				}
				this.m_Texture = value;
				this.SetMaterialDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000037BA File Offset: 0x000019BA
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000037C2 File Offset: 0x000019C2
		public Sprite startImageSprite
		{
			get
			{
				return this._startImageSprite;
			}
			set
			{
				this._startImageSprite = value;
				this.UpdateStartEndImages(true);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000037D2 File Offset: 0x000019D2
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000037DA File Offset: 0x000019DA
		public Sprite endImageSprite
		{
			get
			{
				return this._endImageSprite;
			}
			set
			{
				this._endImageSprite = value;
				this.UpdateStartEndImages(false);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000037EC File Offset: 0x000019EC
		public int vertexCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.splineContainer.Splines.Count; i++)
				{
					num += this.splineContainer[i].CalcVertexCount(this._segmentLength, this.clipRange);
				}
				return num;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003837 File Offset: 0x00001A37
		// (set) Token: 0x06000053 RID: 83 RVA: 0x0000383F File Offset: 0x00001A3F
		public float startImageSize
		{
			get
			{
				return this._startImageSize;
			}
			set
			{
				if (Math.Abs(this._startImageSize - value) > 0.0001f)
				{
					this._startImageSize = value;
					this.UpdateStartEndImages(true);
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003865 File Offset: 0x00001A65
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000386D File Offset: 0x00001A6D
		public OffsetMode startImageOffsetMode
		{
			get
			{
				return this._startImageOffsetMode;
			}
			set
			{
				if (this._startImageOffsetMode != value)
				{
					this._startImageOffsetMode = value;
					this.UpdateStartEndImages(true);
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000056 RID: 86 RVA: 0x0000388B File Offset: 0x00001A8B
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003893 File Offset: 0x00001A93
		public float startImageOffset
		{
			get
			{
				return this._startImageOffset;
			}
			set
			{
				if (Math.Abs(this._startImageOffset - value) > 0.0001f)
				{
					this._startImageOffset = value;
					this.UpdateStartEndImages(true);
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000038B9 File Offset: 0x00001AB9
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000038C1 File Offset: 0x00001AC1
		public float normalizedStartImageOffset
		{
			get
			{
				return this._normalizedStartImageOffset;
			}
			set
			{
				if (Math.Abs(this._normalizedStartImageOffset - value) > 0.0001f)
				{
					this._normalizedStartImageOffset = value;
					this.UpdateStartEndImages(true);
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000038E7 File Offset: 0x00001AE7
		// (set) Token: 0x0600005B RID: 91 RVA: 0x000038EF File Offset: 0x00001AEF
		public float endImageSize
		{
			get
			{
				return this._endImageSize;
			}
			set
			{
				if (Math.Abs(this._endImageSize - value) > 0.0001f)
				{
					this._endImageSize = value;
					this.UpdateStartEndImages(false);
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003915 File Offset: 0x00001B15
		// (set) Token: 0x0600005D RID: 93 RVA: 0x0000391D File Offset: 0x00001B1D
		public OffsetMode endImageOffsetMode
		{
			get
			{
				return this._endImageOffsetMode;
			}
			set
			{
				if (this._endImageOffsetMode != value)
				{
					this._endImageOffsetMode = value;
					this.UpdateStartEndImages(false);
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000393B File Offset: 0x00001B3B
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003943 File Offset: 0x00001B43
		public float endImageOffset
		{
			get
			{
				return this._endImageOffset;
			}
			set
			{
				if (Math.Abs(this._endImageOffset - value) > 0.0001f)
				{
					this._endImageOffset = value;
					this.UpdateStartEndImages(false);
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003969 File Offset: 0x00001B69
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003971 File Offset: 0x00001B71
		public float normalizedEndImageOffset
		{
			get
			{
				return this._normalizedEndImageOffset;
			}
			set
			{
				if (Math.Abs(this._normalizedEndImageOffset - value) > 0.0001f)
				{
					this._normalizedEndImageOffset = value;
					this.UpdateStartEndImages(false);
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003997 File Offset: 0x00001B97
		// (set) Token: 0x06000063 RID: 99 RVA: 0x0000399F File Offset: 0x00001B9F
		public bool recursiveMaterial
		{
			get
			{
				return this._recursiveMaterial;
			}
			set
			{
				this._recursiveMaterial = value;
				if (value)
				{
					this.ManipulateOtherGraphics(delegate(MaskableGraphic x)
					{
						x.material = this.material;
					});
					this.UpdateTrueShadowCustomHash();
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000039C3 File Offset: 0x00001BC3
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000039CC File Offset: 0x00001BCC
		public override Material material
		{
			get
			{
				return base.material;
			}
			set
			{
				base.material = value;
				if (this.recursiveMaterial)
				{
					this.ManipulateOtherGraphics(delegate(MaskableGraphic x)
					{
						x.material = value;
					});
					this.UpdateTrueShadowCustomHash();
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003A12 File Offset: 0x00001C12
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003A1A File Offset: 0x00001C1A
		public bool keepZeroZ
		{
			get
			{
				return this._keepZeroZ;
			}
			set
			{
				this._keepZeroZ = value;
				this._needToResample = true;
				this.SetVerticesDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003A36 File Offset: 0x00001C36
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003A3E File Offset: 0x00001C3E
		public bool keepBillboard
		{
			get
			{
				return this._keepBillboard;
			}
			set
			{
				this._keepBillboard = value;
				this._needToResample = true;
				this.SetVerticesDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003A5A File Offset: 0x00001C5A
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003A62 File Offset: 0x00001C62
		public float width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
				this._needToResample = true;
				this.SetVerticesDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003A7E File Offset: 0x00001C7E
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00003A86 File Offset: 0x00001C86
		public UVMode uvMode
		{
			get
			{
				return this._uvMode;
			}
			set
			{
				this._uvMode = value;
				this.SetMaterialDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003A9B File Offset: 0x00001C9B
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003AA3 File Offset: 0x00001CA3
		public Vector2 uvMultiplier
		{
			get
			{
				return this._uvMultiplier;
			}
			set
			{
				this._uvMultiplier = value;
				this.SetMaterialDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003AB8 File Offset: 0x00001CB8
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public Vector2 uvOffset
		{
			get
			{
				return this._uvOffset;
			}
			set
			{
				this._uvOffset = value;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003ADB File Offset: 0x00001CDB
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003AE3 File Offset: 0x00001CE3
		public Vector2 clipRange
		{
			get
			{
				return this._clipRange;
			}
			set
			{
				this._clipRange = value;
				this._needToResample = true;
				this.SetVerticesDirty();
				this.UpdateTrueShadowCustomHash();
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003B00 File Offset: 0x00001D00
		protected override void OnEnable()
		{
			base.OnEnable();
			Spline.Changed += this.OnSplineChanged;
			SplineContainer.SplineAdded += this.OnSplineAddedOrRemoved;
			SplineContainer.SplineRemoved += this.OnSplineAddedOrRemoved;
			if (!this._defaultTextureInitialized && this.m_Texture == null)
			{
				this.m_Texture = UISplineRendererSettings.Instance.defaultLineTexture;
			}
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			this.UpdateRaycastTargetRect();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002609 File Offset: 0x00000809
		protected override void Start()
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003B80 File Offset: 0x00001D80
		protected override void OnDisable()
		{
			base.OnDisable();
			Spline.Changed -= this.OnSplineChanged;
			SplineContainer.SplineAdded -= this.OnSplineAddedOrRemoved;
			SplineContainer.SplineRemoved -= this.OnSplineAddedOrRemoved;
			this.SetVerticesDirty();
			this.SetMaterialDirty();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003BD4 File Offset: 0x00001DD4
		protected override void OnDestroy()
		{
			base.OnDestroy();
			int count = this.startImages.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.startImages[i] != null)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(this.startImages[i].gameObject);
					}
					else
					{
						Object.DestroyImmediate(this.startImages[i].gameObject);
					}
				}
				if (this.endImages[i] != null)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(this.endImages[i].gameObject);
					}
					else
					{
						Object.DestroyImmediate(this.endImages[i].gameObject);
					}
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003C97 File Offset: 0x00001E97
		public override void SetVerticesDirty()
		{
			base.SetVerticesDirty();
			this.DoExtrudeSplineJobAll();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003CA5 File Offset: 0x00001EA5
		public override void SetMaterialDirty()
		{
			base.SetMaterialDirty();
			this.ManipulateOtherGraphics(delegate(MaskableGraphic x)
			{
				x.maskable = base.maskable;
			});
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003CBF File Offset: 0x00001EBF
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this._vh == null)
			{
				this._vh = vh;
			}
			vh.Clear();
			this.Draw();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003CDC File Offset: 0x00001EDC
		public override bool Raycast(Vector2 sp, Camera eventCamera)
		{
			return base.Raycast(sp, eventCamera) && this.SplineRaycast(sp, eventCamera);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003CF4 File Offset: 0x00001EF4
		private bool SplineRaycast(Vector2 sp, Camera eventCamera)
		{
			Vector3 vector = default(Vector3);
			if (base.canvas.renderMode == null)
			{
				Vector3 vector2;
				vector2..ctor(sp.x, sp.y, base.transform.position.z);
				vector = base.transform.InverseTransformPoint(vector2);
			}
			else
			{
				Vector3 vector3 = eventCamera.ScreenToWorldPoint(new Vector3(sp.x, sp.y, base.transform.position.z - eventCamera.transform.position.z));
				vector = base.transform.InverseTransformPoint(vector3);
				vector.z = 0f;
			}
			using (IEnumerator<Spline> enumerator = this.splineContainer.Splines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float3 @float;
					float t;
					if (SplineUtility.GetNearestPoint<Spline>(enumerator.Current, vector, ref @float, ref t, 4, 2) <= this.GetWidthAt(t))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003DFC File Offset: 0x00001FFC
		public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			base.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			this.ManipulateOtherGraphics(delegate(MaskableGraphic x)
			{
				x.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			});
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003E50 File Offset: 0x00002050
		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			this.ManipulateOtherGraphics(delegate(MaskableGraphic x)
			{
				x.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			});
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003EB0 File Offset: 0x000020B0
		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
		{
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
			this.ManipulateOtherGraphics(delegate(MaskableGraphic x)
			{
				x.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
			});
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003F20 File Offset: 0x00002120
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
		{
			bool flag = false;
			int i = 0;
			while (i < this.splineContainer.Splines.Count)
			{
				if (this.splineContainer.Splines[i] == spline)
				{
					flag = true;
					if (modification == 2)
					{
						BezierKnot bezierKnot = spline[knotIndex];
						float3 position = bezierKnot.Position;
						if (this.keepZeroZ)
						{
							bezierKnot.Position = new float3(position.x, position.y, 0f);
						}
						spline.SetKnotNoNotify(knotIndex, bezierKnot, 1);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				return;
			}
			if (this.keepZeroZ)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
			}
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			this.UpdateRaycastTargetRect();
			this.UpdateTrueShadowCustomHash();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003FFC File Offset: 0x000021FC
		private void OnSplineAddedOrRemoved(SplineContainer container, int i)
		{
			if (container != this.splineContainer)
			{
				return;
			}
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			this.UpdateRaycastTargetRect();
			this.UpdateTrueShadowCustomHash();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004028 File Offset: 0x00002228
		private void UpdateRaycastTargetRect()
		{
			if (this.splineContainer.Splines.Count == 0)
			{
				return;
			}
			if (this.splineContainer.Splines.Count == 1 && this.splineContainer.Spline.Count < 2)
			{
				return;
			}
			float3 @float = this.splineContainer.EvaluatePosition(0f);
			float x = base.transform.lossyScale.x;
			Bounds bounds;
			bounds..ctor(@float, Vector3.one * (this.GetWidthAt(0f) * x));
			for (int i = 0; i < this.splineContainer.Splines.Count; i++)
			{
				Spline spline = this.splineContainer[i];
				int num = spline.CalcVertexCount(this._segmentLength, this.clipRange) / 2;
				for (int j = 0; j < num; j++)
				{
					float num2 = (float)j / (float)(num - 1);
					float3 float2 = this.splineContainer.EvaluatePosition<Spline>(spline, num2);
					Bounds bounds2;
					bounds2..ctor(float2, Vector3.one * (this.GetWidthAt(num2) * x));
					bounds.Encapsulate(bounds2);
				}
			}
			if (this.splineContainer.Splines.Count == 1)
			{
				float3 float3 = this.splineContainer.EvaluatePosition(1f);
				Bounds bounds3;
				bounds3..ctor(float3, Vector3.one * (this.GetWidthAt(1f) * x));
				bounds.Encapsulate(bounds3);
			}
			if (float.IsNaN(bounds.center.x) || float.IsNaN(bounds.center.y) || float.IsNaN(bounds.center.z) || float.IsNaN(bounds.size.x) || float.IsNaN(bounds.size.y) || float.IsNaN(bounds.size.z))
			{
				return;
			}
			Vector3 position = base.transform.position;
			bounds.center = new Vector3(bounds.center.x, bounds.center.y, position.z);
			Vector3 size = bounds.size;
			Vector3 lossyScale = base.canvas.transform.lossyScale;
			bounds.size = new Vector3(size.x / lossyScale.x, size.y / lossyScale.y, 1f);
			RectTransform rectTransform = base.rectTransform.parent as RectTransform;
			base.rectTransform.sizeDelta = bounds.size;
			base.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			base.rectTransform.position = bounds.center;
			base.rectTransform.SetPivotWithoutRect(position);
			Rect rect = rectTransform.rect;
			this.UpdateStartEndImages(true);
			this.UpdateStartEndImages(false);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004314 File Offset: 0x00002514
		public void UpdateTrueShadowCustomHash()
		{
			if (this._trueShadow == null)
			{
				this._trueShadow = base.GetComponent<TrueShadow>();
			}
			if (this._trueShadow == null)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.splineContainer.Splines.Count; i++)
			{
				Spline spline = this.splineContainer.Splines[i];
				num += spline.Count;
				for (int j = 0; j < spline.Count; j++)
				{
					BezierKnot bezierKnot = spline[j];
					num2 += bezierKnot.Position.GetHashCode();
					num2 += bezierKnot.Rotation.GetHashCode();
					num2 += bezierKnot.TangentIn.GetHashCode();
					num2 += bezierKnot.TangentOut.GetHashCode();
				}
			}
			this._trueShadow.CustomHash = HashUtils.CombineHashCodes((this.texture == null) ? 0 : this.texture.GetHashCode(), (this.material == null) ? 0 : this.material.GetHashCode(), this.color.GetHashCode(), this._colorGradient.GetHashCode(), this.uvOffset.GetHashCode(), this.uvMultiplier.GetHashCode(), num, num2);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004494 File Offset: 0x00002694
		private void Draw()
		{
			if (this.splineContainer.Splines.Count <= 0)
			{
				return;
			}
			if (this.width == 0f)
			{
				return;
			}
			for (int i = 0; i < this._vertices.Count; i++)
			{
				UIVertex uivertex = this._vertices[i];
				this._vh.AddVert(uivertex);
			}
			for (int j = 0; j < this._triangles.Count; j++)
			{
				int3 @int = this._triangles[j];
				this._vh.AddTriangle(@int.x, @int.y, @int.z);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004534 File Offset: 0x00002734
		private void DoExtrudeSplineJobAll()
		{
			GarbageCollector.GCMode = 0;
			if (this.splineContainer == null)
			{
				this.splineContainer = base.GetComponent<SplineContainer>();
			}
			int num = 0;
			this._vertices.Clear();
			this._triangles.Clear();
			for (int i = 0; i < this.splineContainer.Splines.Count; i++)
			{
				Spline spline = this.splineContainer[i];
				this.DoExtrudeSplineJob(spline, num);
				num += spline.CalcVertexCount(this._segmentLength, this.clipRange);
			}
			this._needToResample = false;
			GarbageCollector.GCMode = 1;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000045CC File Offset: 0x000027CC
		private void DoExtrudeSplineJob(Spline spline, int startIdx)
		{
			NativeSpline spline2;
			spline2..ctor(spline, 3);
			NativeColorGradient colorGradient = this._colorGradient.ToNative(3);
			int num = spline.CalcVertexCount(this._segmentLength, this.clipRange) / 2;
			NativeCurve widthCurve = new NativeCurve(this._widthCurve, 3);
			NativeArray<float3> evaluatedPos = new NativeArray<float3>(num, 3, 1);
			NativeArray<float3> evaluatedTan = new NativeArray<float3>(num, 3, 1);
			NativeArray<float3> evaluatedNor = new NativeArray<float3>(num, 3, 1);
			NativeList<UIVertex> vertices = new NativeList<UIVertex>(3);
			NativeList<int3> triangles = new NativeList<int3>(3);
			SplineExtrudeJob splineExtrudeJob = new SplineExtrudeJob
			{
				spline = spline2,
				widthCurve = widthCurve,
				startIdx = startIdx,
				keepBillboard = this.keepBillboard,
				keepZeroZ = this.keepZeroZ,
				clipRange = this.clipRange,
				uvMultiplier = this.uvMultiplier,
				uvOffset = this.uvOffset,
				color = this.color,
				colorGradient = colorGradient,
				uvMode = this.uvMode,
				width = this.width,
				triangles = triangles,
				vertices = vertices,
				edgeCount = num,
				evaluatedPos = evaluatedPos,
				evaluatedTan = evaluatedTan,
				evaluatedNor = evaluatedNor
			};
			this._jobHandle = IJobExtensions.Schedule<SplineExtrudeJob>(splineExtrudeJob, default(JobHandle));
			this._jobHandle.Complete();
			for (int i = 0; i < vertices.Length; i++)
			{
				this._vertices.Add(vertices[i]);
			}
			for (int j = 0; j < triangles.Length; j++)
			{
				this._triangles.Add(triangles[j]);
			}
			triangles.Dispose();
			vertices.Dispose();
			widthCurve.Dispose();
			spline2.Dispose();
			colorGradient.Dispose();
			evaluatedPos.Dispose();
			evaluatedTan.Dispose();
			evaluatedNor.Dispose();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000047D0 File Offset: 0x000029D0
		private void UpdateGraphicColors()
		{
			if (!this._recursiveColor)
			{
				return;
			}
			float t = this._startImageOffset / this.splineContainer.CalculateLength();
			for (int i = 0; i < this.startImages.Count; i++)
			{
				this.startImages[i].color = this.GetColorAt(t);
			}
			float t2 = 1f - this._endImageOffset / this.splineContainer.CalculateLength();
			for (int j = 0; j < this.endImages.Count; j++)
			{
				this.endImages[j].color = this.GetColorAt(t2);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004870 File Offset: 0x00002A70
		private void ManipulateOtherGraphics(Action<MaskableGraphic> graphic)
		{
			for (int i = 0; i < this.startImages.Count; i++)
			{
				graphic(this.startImages[i]);
			}
			for (int j = 0; j < this.endImages.Count; j++)
			{
				graphic(this.endImages[j]);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000048D0 File Offset: 0x00002AD0
		internal void UpdateStartEndImages(bool isStartImage)
		{
			Sprite sprite = isStartImage ? this._startImageSprite : this._endImageSprite;
			List<Image> list = isStartImage ? this.startImages : this.endImages;
			float num = isStartImage ? this._startImageOffset : this._endImageOffset;
			float num2 = isStartImage ? this._startImageSize : this._endImageSize;
			OffsetMode offsetMode = isStartImage ? this._startImageOffsetMode : this._endImageOffsetMode;
			float num3 = isStartImage ? this._normalizedStartImageOffset : this._normalizedEndImageOffset;
			if (sprite == null)
			{
				while (list.Count > 0)
				{
					if (Application.isPlaying)
					{
						List<Image> list2 = list;
						Object.Destroy(list2[list2.Count - 1].gameObject);
					}
					else
					{
						List<Image> list3 = list;
						Object.DestroyImmediate(list3[list3.Count - 1].gameObject);
					}
					list.RemoveAt(list.Count - 1);
				}
				return;
			}
			int num4 = this.splineContainer.Splines.Count((Spline x) => x.Count > 1);
			if (list.Count > num4)
			{
				int num5 = list.Count - num4;
				for (int i = 0; i < num5; i++)
				{
					if (Application.isPlaying)
					{
						List<Image> list4 = list;
						Object.Destroy(list4[list4.Count - 1].gameObject);
					}
					else
					{
						List<Image> list5 = list;
						Object.DestroyImmediate(list5[list5.Count - 1].gameObject);
					}
					list.RemoveAt(list.Count - 1);
				}
			}
			else if (list.Count < num4)
			{
				int num6 = num4 - list.Count;
				for (int j = 0; j < num6; j++)
				{
					GameObject gameObject = new GameObject(string.Format("Spline UI Renderer - {0}[{1}]", isStartImage ? "StartImage" : "EndImage", j));
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localScale = Vector3.one;
					gameObject.layer = LayerMask.NameToLayer("UI");
					Image image = gameObject.AddComponent<Image>();
					image.SetNativeSize();
					list.Add(image);
				}
			}
			for (int k = 0; k < this.splineContainer.Splines.Count; k++)
			{
				Spline spline = this.splineContainer[k];
				if (spline.Count >= 2)
				{
					Image image2 = list[k];
					image2.rectTransform.sizeDelta = Vector2.one * num2;
					image2.sprite = sprite;
					float length = spline.GetLength();
					float num7;
					if (offsetMode == OffsetMode.Distance)
					{
						num7 = (isStartImage ? (num / length) : (1f + num / length));
					}
					else
					{
						num7 = num3;
					}
					float3 @float;
					quaternion quaternion;
					if (num7 < 0f || num7 > 1f)
					{
						float num8 = isStartImage ? 0f : 1f;
						if (isStartImage && num7 > 1f)
						{
							num8 = 1f;
						}
						else if (!isStartImage && num7 < 0f)
						{
							num8 = 0f;
						}
						if (offsetMode == OffsetMode.Normalized)
						{
							num8 = num7;
						}
						float3 float2;
						float3 float3;
						this.splineContainer.Evaluate<Spline>(spline, num8, ref @float, ref float2, ref float3);
						if (float2 == Vector3.zero)
						{
							float3 float4 = this.splineContainer.EvaluatePosition<Spline>(spline, isStartImage ? 0.01f : 0.99f);
							float2 = (isStartImage ? (float4 - @float) : (@float - float4));
						}
						if (this.keepBillboard)
						{
							quaternion = quaternion.LookRotation(new float3(0f, 0f, 1f), float2);
						}
						else
						{
							quaternion = quaternion.LookRotation(float3, float2);
						}
						float num9 = num;
						if (offsetMode == OffsetMode.Distance)
						{
							if (isStartImage && num7 > 1f)
							{
								num9 -= length;
							}
							else if (!isStartImage && num7 < 0f)
							{
								num9 += length;
							}
						}
						else
						{
							num9 = length * ((num7 > 1f) ? (num7 - 1f) : num7);
						}
						@float += quaternion * (Vector3.up * num9);
						if (this.keepZeroZ)
						{
							@float.z = base.transform.position.z;
						}
						if (this.recursiveColor)
						{
							image2.color = this.GetColorAt((float)(isStartImage ? 0 : 1));
						}
					}
					else
					{
						float3 float5;
						float3 float6;
						this.splineContainer.Evaluate<Spline>(spline, num7, ref @float, ref float5, ref float6);
						if (float5 == Vector3.zero)
						{
							float3 float7 = SplineUtility.EvaluateAcceleration<Spline>(spline, num7);
							float5 = (isStartImage ? (float7 - @float) : (@float - float7));
						}
						if (this.keepZeroZ)
						{
							@float.z = base.transform.position.z;
						}
						if (this.keepBillboard)
						{
							quaternion = quaternion.LookRotation(new float3(0f, 0f, 1f), float5);
						}
						else
						{
							quaternion = quaternion.LookRotation(float6, float5);
						}
						if (this.recursiveColor)
						{
							image2.color = this.GetColorAt(num7);
						}
					}
					list[k].transform.SetPositionAndRotation(@float, quaternion);
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004DEE File Offset: 0x00002FEE
		public Color GetColorAt(float t)
		{
			t = Mathf.Clamp01(t);
			return this.color * this._colorGradient.Evaluate(t);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004E0F File Offset: 0x0000300F
		public float GetWidthAt(float t)
		{
			return this._widthCurve.Evaluate(t) * this.width;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004E24 File Offset: 0x00003024
		public void SetWidthCurve(AnimationCurve curve)
		{
			this._widthCurve = curve;
			this.SetVerticesDirty();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004E33 File Offset: 0x00003033
		public void ChangeWidthCurveKey(int index, Keyframe key)
		{
			this._widthCurve.MoveKey(index, key);
			this.SetVerticesDirty();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004E49 File Offset: 0x00003049
		public void SetColorGradient(Gradient gradient)
		{
			this._colorGradient = gradient;
			this.SetVerticesDirty();
			this.UpdateGraphicColors();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004E5E File Offset: 0x0000305E
		public void ChangeColorGradientAlphaKey(int index, GradientAlphaKey key)
		{
			this._colorGradient.alphaKeys[index] = key;
			this.SetVerticesDirty();
			this.UpdateGraphicColors();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004E7E File Offset: 0x0000307E
		public void ChangeColorGradientAlphaKey(int index, GradientColorKey key)
		{
			this._colorGradient.colorKeys[index] = key;
			this.SetVerticesDirty();
			this.UpdateGraphicColors();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004E9E File Offset: 0x0000309E
		public void ForceUpdate()
		{
			this.DoExtrudeSplineJobAll();
			this.Rebuild(1);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004EAD File Offset: 0x000030AD
		[ContextMenu("ReorientKnots")]
		public void ReorientKnots()
		{
			this.splineContainer.ReorientKnots(false);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004EBC File Offset: 0x000030BC
		public static UISplineRenderer Create(IEnumerable<Vector3> positions, RectTransform parent, bool isLocal = false, LineTexturePreset lineTexture = LineTexturePreset.Default, StartEndImagePreset startImage = StartEndImagePreset.None, StartEndImagePreset endImage = StartEndImagePreset.None)
		{
			RectTransform rectTransform = new GameObject("New UI Spline Renderer").AddComponent<RectTransform>();
			rectTransform.SetParent(parent, false);
			SplineContainer splineContainer = rectTransform.gameObject.AddComponent<SplineContainer>();
			foreach (Vector3 vector in positions)
			{
				Vector3 vector2 = isLocal ? vector : splineContainer.transform.InverseTransformPoint(vector);
				BezierKnot bezierKnot;
				bezierKnot..ctor(vector2);
				splineContainer.Spline.Add(bezierKnot);
			}
			splineContainer.ReorientKnotsAndSmooth();
			UISplineRenderer uisplineRenderer = splineContainer.gameObject.AddComponent<UISplineRenderer>();
			uisplineRenderer.lineTexturePreset = lineTexture;
			uisplineRenderer.startImagePreset = startImage;
			uisplineRenderer.endImagePreset = endImage;
			return uisplineRenderer;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004F74 File Offset: 0x00003174
		public static UISplineRenderer Create(IEnumerable<Vector3> positions, RectTransform parent, bool isLocal, Texture lineTexture, Sprite startImage, Sprite endImage)
		{
			RectTransform rectTransform = new GameObject("New UI Spline Renderer").AddComponent<RectTransform>();
			rectTransform.SetParent(parent, false);
			SplineContainer splineContainer = rectTransform.gameObject.AddComponent<SplineContainer>();
			foreach (Vector3 vector in positions)
			{
				Vector3 vector2 = isLocal ? vector : splineContainer.transform.InverseTransformPoint(vector);
				BezierKnot bezierKnot;
				bezierKnot..ctor(vector2);
				splineContainer.Spline.Add(bezierKnot);
			}
			splineContainer.ReorientKnotsAndSmooth();
			UISplineRenderer uisplineRenderer = splineContainer.gameObject.AddComponent<UISplineRenderer>();
			uisplineRenderer.texture = lineTexture;
			uisplineRenderer.startImageSprite = startImage;
			uisplineRenderer.endImageSprite = endImage;
			return uisplineRenderer;
		}

		// Token: 0x04000035 RID: 53
		public SplineContainer splineContainer;

		// Token: 0x04000036 RID: 54
		public List<Image> startImages = new List<Image>();

		// Token: 0x04000037 RID: 55
		public List<Image> endImages = new List<Image>();

		// Token: 0x04000038 RID: 56
		[SerializeField]
		private Gradient _colorGradient = new Gradient();

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private AnimationCurve _widthCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400003A RID: 58
		[SerializeField]
		private bool _keepZeroZ = true;

		// Token: 0x0400003B RID: 59
		[SerializeField]
		private bool _keepBillboard = true;

		// Token: 0x0400003C RID: 60
		[SerializeField]
		private float _width = 10f;

		// Token: 0x0400003D RID: 61
		[SerializeField]
		private UVMode _uvMode;

		// Token: 0x0400003E RID: 62
		[SerializeField]
		private Vector2 _uvMultiplier = new Vector2(1f, 1f);

		// Token: 0x0400003F RID: 63
		[SerializeField]
		private Vector2 _uvOffset;

		// Token: 0x04000040 RID: 64
		[SerializeField]
		private Vector2 _clipRange = new Vector2(0f, 1f);

		// Token: 0x04000041 RID: 65
		[SerializeField]
		private bool _recursiveMaterial = true;

		// Token: 0x04000042 RID: 66
		[SerializeField]
		private bool _recursiveColor = true;

		// Token: 0x04000043 RID: 67
		[SerializeField]
		private int _resolution = 5;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private float _segmentLength = 0.12f;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		private Texture m_Texture;

		// Token: 0x04000046 RID: 70
		[SerializeField]
		private Sprite _startImageSprite;

		// Token: 0x04000047 RID: 71
		[SerializeField]
		private Sprite _endImageSprite;

		// Token: 0x04000048 RID: 72
		[SerializeField]
		private float _startImageSize = 32f;

		// Token: 0x04000049 RID: 73
		[SerializeField]
		private OffsetMode _startImageOffsetMode;

		// Token: 0x0400004A RID: 74
		[SerializeField]
		private float _startImageOffset;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		private float _normalizedStartImageOffset;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		private float _endImageSize = 32f;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		private OffsetMode _endImageOffsetMode;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		private float _endImageOffset;

		// Token: 0x0400004F RID: 79
		[SerializeField]
		private float _normalizedEndImageOffset = 1f;

		// Token: 0x04000050 RID: 80
		[SerializeField]
		private bool _defaultTextureInitialized;

		// Token: 0x04000051 RID: 81
		private bool _needToResample;

		// Token: 0x04000052 RID: 82
		private List<UIVertex> _vertices = new List<UIVertex>();

		// Token: 0x04000053 RID: 83
		private List<int3> _triangles = new List<int3>();

		// Token: 0x04000054 RID: 84
		private JobHandle _jobHandle;

		// Token: 0x04000055 RID: 85
		private VertexHelper _vh;

		// Token: 0x04000056 RID: 86
		private TrueShadow _trueShadow;
	}
}
