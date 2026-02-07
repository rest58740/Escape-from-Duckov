using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x0200000B RID: 11
	[ExecuteAlways]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Tai Le Assets/Translucent Image Source")]
	[HelpURL("https://leloctai.com/asset/translucentimage/docs/articles/customize.html#translucent-image-source")]
	public class TranslucentImageSource : MonoBehaviour
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002978 File Offset: 0x00000B78
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002980 File Offset: 0x00000B80
		public BlurConfig BlurConfig
		{
			get
			{
				return this.blurConfig;
			}
			set
			{
				this.blurConfig = value;
				this.InitializeBlurAlgorithm();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000298F File Offset: 0x00000B8F
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00002997 File Offset: 0x00000B97
		public int Downsample
		{
			get
			{
				return this.downsample;
			}
			set
			{
				this.downsample = Mathf.Max(0, value);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029A6 File Offset: 0x00000BA6
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000029B0 File Offset: 0x00000BB0
		public Rect BlurRegion
		{
			get
			{
				return this.blurRegion;
			}
			set
			{
				Vector2 vector = new Vector2(1f / (float)this.Cam.pixelWidth, 1f / (float)this.Cam.pixelHeight);
				this.blurRegion.x = Mathf.Clamp(value.x, 0f, 1f - vector.x);
				this.blurRegion.y = Mathf.Clamp(value.y, 0f, 1f - vector.y);
				this.blurRegion.width = Mathf.Clamp(value.width, vector.x, 1f - this.blurRegion.x);
				this.blurRegion.height = Mathf.Clamp(value.height, vector.y, 1f - this.blurRegion.y);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002A94 File Offset: 0x00000C94
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002A9C File Offset: 0x00000C9C
		public float MaxUpdateRate
		{
			get
			{
				return this.maxUpdateRate;
			}
			set
			{
				this.maxUpdateRate = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002AA5 File Offset: 0x00000CA5
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002AAD File Offset: 0x00000CAD
		public BackgroundFill BackgroundFill
		{
			get
			{
				return this.backgroundFill;
			}
			set
			{
				this.backgroundFill = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002AB6 File Offset: 0x00000CB6
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002ABE File Offset: 0x00000CBE
		public bool Preview
		{
			get
			{
				return this.preview;
			}
			set
			{
				this.preview = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002AC7 File Offset: 0x00000CC7
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002ACF File Offset: 0x00000CCF
		public RenderTexture BlurredScreen
		{
			get
			{
				return this.blurredScreen;
			}
			set
			{
				this.blurredScreen = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002AD8 File Offset: 0x00000CD8
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public Rect CamRectOverride { get; set; } = Rect.zero;

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002AEC File Offset: 0x00000CEC
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002B98 File Offset: 0x00000D98
		public Rect BlurRegionNormalizedScreenSpace
		{
			get
			{
				Rect rect = (this.CamRectOverride.width == 0f) ? this.Cam.rect : this.CamRectOverride;
				rect.min = Vector2.Max(Vector2.zero, rect.min);
				rect.max = Vector2.Min(Vector2.one, rect.max);
				return new Rect(rect.position + this.BlurRegion.position * rect.size, rect.size * this.BlurRegion.size);
			}
			set
			{
				Rect rect = (this.CamRectOverride.width == 0f) ? this.Cam.rect : this.CamRectOverride;
				rect.min = Vector2.Max(Vector2.zero, rect.min);
				rect.max = Vector2.Min(Vector2.one, rect.max);
				this.BlurRegion = new Rect((value.position - rect.position) / rect.size, value.size / rect.size);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002C3C File Offset: 0x00000E3C
		internal Camera Cam
		{
			get
			{
				if (!this.camera)
				{
					return this.camera = base.GetComponent<Camera>();
				}
				return this.camera;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002C6C File Offset: 0x00000E6C
		private float MinUpdateCycle
		{
			get
			{
				if (this.MaxUpdateRate <= 0f)
				{
					return float.PositiveInfinity;
				}
				return 1f / this.MaxUpdateRate;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002C90 File Offset: 0x00000E90
		protected virtual void Start()
		{
			this.previewMaterial = new Material(Shader.Find("Hidden/FillCrop"));
			this.InitializeBlurAlgorithm();
			this.ReallocateBlurTexIfNeeded(Vector2Int.RoundToInt(this.Cam.pixelRect.size));
			this.lastDownsample = this.Downsample;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002CE2 File Offset: 0x00000EE2
		private void OnDestroy()
		{
			if (this.BlurredScreen)
			{
				this.BlurredScreen.Release();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002CFC File Offset: 0x00000EFC
		private void InitializeBlurAlgorithm()
		{
			if (this.blurConfig is ScalableBlurConfig)
			{
				this.blurAlgorithm = new ScalableBlur();
				return;
			}
			this.blurAlgorithm = new ScalableBlur();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D24 File Offset: 0x00000F24
		protected virtual void CreateNewBlurredScreen(Vector2Int camPixelSize)
		{
			if (this.BlurredScreen)
			{
				this.BlurredScreen.Release();
			}
			if (XRSettings.enabled)
			{
				this.BlurredScreen = new RenderTexture(XRSettings.eyeTextureDesc);
				this.BlurredScreen.width = Mathf.RoundToInt((float)this.BlurredScreen.width * this.BlurRegion.width) >> this.Downsample;
				this.BlurredScreen.height = Mathf.RoundToInt((float)this.BlurredScreen.height * this.BlurRegion.height) >> this.Downsample;
				this.BlurredScreen.depth = 0;
			}
			else
			{
				this.BlurredScreen = new RenderTexture(Mathf.RoundToInt((float)camPixelSize.x * this.BlurRegion.width) >> this.Downsample, Mathf.RoundToInt((float)camPixelSize.y * this.BlurRegion.height) >> this.Downsample, 0);
			}
			this.BlurredScreen.antiAliasing = 1;
			this.BlurredScreen.useMipMap = false;
			this.BlurredScreen.name = base.gameObject.name + " Translucent Image Source";
			this.BlurredScreen.filterMode = FilterMode.Bilinear;
			this.BlurredScreen.Create();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002E84 File Offset: 0x00001084
		public void ReallocateBlurTexIfNeeded(Vector2Int camPixelSize)
		{
			if (this.BlurredScreen == null || !this.BlurredScreen.IsCreated() || this.Downsample != this.lastDownsample || !this.BlurRegion.Approximately(this.lastBlurRegion) || camPixelSize != this.lastCamPixelSize || XRSettings.deviceEyeTextureDimension != this.lastEyeTexDim)
			{
				this.CreateNewBlurredScreen(camPixelSize);
				this.lastDownsample = this.Downsample;
				this.lastBlurRegion = this.BlurRegion;
				this.lastCamPixelSize = camPixelSize;
				this.lastEyeTexDim = XRSettings.deviceEyeTextureDimension;
			}
			this.lastUpdate = TranslucentImageSource.GetTrueCurrentTime();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002F24 File Offset: 0x00001124
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.cmd == null)
			{
				this.cmd = new CommandBuffer();
				this.cmd.name = "Translucent Image Source";
			}
			if (this.blurAlgorithm != null && this.BlurConfig != null)
			{
				if (this.ShouldUpdateBlur())
				{
					this.cmd.Clear();
					this.ReallocateBlurTexIfNeeded(Vector2Int.RoundToInt(this.Cam.pixelRect.size));
					this.blurAlgorithm.Init(this.BlurConfig, true);
					BlurExecutor.BlurExecutionData blurExecutionData = new BlurExecutor.BlurExecutionData(source, this, this.blurAlgorithm);
					BlurExecutor.ExecuteBlurWithTempTextures(this.cmd, ref blurExecutionData);
					Graphics.ExecuteCommandBuffer(this.cmd);
				}
				if (this.Preview)
				{
					this.previewMaterial.SetVector(ShaderId.CROP_REGION, this.BlurRegion.ToMinMaxVector());
					Graphics.Blit(this.BlurredScreen, destination, this.previewMaterial);
				}
				else
				{
					Graphics.Blit(source, destination);
				}
			}
			else
			{
				Graphics.Blit(source, destination);
			}
			this.isRequested = false;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000302D File Offset: 0x0000122D
		public void Request()
		{
			this.isRequested = true;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003036 File Offset: 0x00001236
		public bool ShouldUpdateBlur()
		{
			return base.enabled && (this.Preview || this.isRequested) && TranslucentImageSource.GetTrueCurrentTime() - this.lastUpdate >= this.MinUpdateCycle;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000306B File Offset: 0x0000126B
		private static float GetTrueCurrentTime()
		{
			return Time.unscaledTime;
		}

		// Token: 0x04000022 RID: 34
		[SerializeField]
		private BlurConfig blurConfig;

		// Token: 0x04000023 RID: 35
		[SerializeField]
		[Range(0f, 3f)]
		[Tooltip("Reduce the size of the screen before processing. Increase will improve performance but create more artifact")]
		private int downsample;

		// Token: 0x04000024 RID: 36
		[SerializeField]
		[Tooltip("Choose which part of the screen to blur. Smaller region is faster")]
		private Rect blurRegion = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04000025 RID: 37
		[SerializeField]
		[Tooltip("How many time to blur per second. Reduce to increase performance and save battery for slow moving background")]
		private float maxUpdateRate = float.PositiveInfinity;

		// Token: 0x04000026 RID: 38
		[SerializeField]
		[Tooltip("Preview the effect fullscreen. Not recommended for runtime use")]
		private bool preview;

		// Token: 0x04000027 RID: 39
		[SerializeField]
		[Tooltip("Fill the background where the frame buffer alpha is 0. Useful for VR Underlay and Passthrough, where these areas would otherwise be black")]
		private BackgroundFill backgroundFill = new BackgroundFill();

		// Token: 0x04000028 RID: 40
		private int lastDownsample;

		// Token: 0x04000029 RID: 41
		private Rect lastBlurRegion = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400002A RID: 42
		private Vector2Int lastCamPixelSize = Vector2Int.zero;

		// Token: 0x0400002B RID: 43
		private float lastUpdate;

		// Token: 0x0400002C RID: 44
		private IBlurAlgorithm blurAlgorithm;

		// Token: 0x0400002D RID: 45
		private Camera camera;

		// Token: 0x0400002E RID: 46
		private Material previewMaterial;

		// Token: 0x0400002F RID: 47
		private RenderTexture blurredScreen;

		// Token: 0x04000030 RID: 48
		private CommandBuffer cmd;

		// Token: 0x04000031 RID: 49
		private bool isRequested;

		// Token: 0x04000032 RID: 50
		private bool isForOverlayCanvas;

		// Token: 0x04000034 RID: 52
		private TextureDimension lastEyeTexDim;
	}
}
