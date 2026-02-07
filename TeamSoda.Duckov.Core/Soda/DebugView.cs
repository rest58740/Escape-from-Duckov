using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Soda
{
	// Token: 0x02000230 RID: 560
	public class DebugView : MonoBehaviour
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x000434A1 File Offset: 0x000416A1
		public DebugView Instance
		{
			get
			{
				return this.instance;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x000434A9 File Offset: 0x000416A9
		public bool EdgeLightActive
		{
			get
			{
				return this.edgeLightActive;
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x0600113E RID: 4414 RVA: 0x000434B4 File Offset: 0x000416B4
		// (remove) Token: 0x0600113F RID: 4415 RVA: 0x000434E8 File Offset: 0x000416E8
		public static event Action<DebugView> OnDebugViewConfigChanged;

		// Token: 0x06001140 RID: 4416 RVA: 0x0004351B File Offset: 0x0004171B
		private void Awake()
		{
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0004351D File Offset: 0x0004171D
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnlevelInited;
			SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(this.OnSceneLoaded);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00043544 File Offset: 0x00041744
		private void InitFromData()
		{
			if (PlayerPrefs.HasKey("ResMode"))
			{
				this.resMode = (ResModes)PlayerPrefs.GetInt("ResMode");
			}
			else
			{
				this.resMode = ResModes.R720p;
			}
			if (PlayerPrefs.HasKey("TexMode"))
			{
				this.texMode = (TextureModes)PlayerPrefs.GetInt("TexMode");
			}
			else
			{
				this.texMode = TextureModes.High;
			}
			if (PlayerPrefs.HasKey("InputDevice"))
			{
				this.inputDevice = PlayerPrefs.GetInt("InputDevice");
			}
			else
			{
				this.inputDevice = 1;
			}
			if (PlayerPrefs.HasKey("BloomActive"))
			{
				this.bloomActive = (PlayerPrefs.GetInt("BloomActive") != 0);
			}
			else
			{
				this.bloomActive = true;
			}
			if (PlayerPrefs.HasKey("EdgeLightActive"))
			{
				this.edgeLightActive = (PlayerPrefs.GetInt("EdgeLightActive") != 0);
			}
			else
			{
				this.edgeLightActive = true;
			}
			if (PlayerPrefs.HasKey("AOActive"))
			{
				this.aoActive = (PlayerPrefs.GetInt("AOActive") != 0);
			}
			else
			{
				this.aoActive = false;
			}
			if (PlayerPrefs.HasKey("DofActive"))
			{
				this.dofActive = (PlayerPrefs.GetInt("DofActive") != 0);
			}
			else
			{
				this.dofActive = false;
			}
			if (PlayerPrefs.HasKey("ReporterActive"))
			{
				this.reporterActive = (PlayerPrefs.GetInt("ReporterActive") != 0);
				return;
			}
			this.reporterActive = false;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00043688 File Offset: 0x00041888
		private void Update()
		{
			this.deltaTimes[this.frameIndex] = Time.deltaTime;
			this.frameIndex++;
			if (this.frameIndex >= this.frameSampleCount)
			{
				this.frameIndex = 0;
				float num = 0f;
				for (int i = 0; i < this.frameSampleCount; i++)
				{
					num += this.deltaTimes[i];
				}
				int num2 = Mathf.RoundToInt((float)this.frameSampleCount / Mathf.Max(0.0001f, num));
				this.fpsText1.text = num2.ToString();
				this.fpsText2.text = num2.ToString();
			}
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0004372C File Offset: 0x0004192C
		public void SetInputDevice(int type)
		{
			if (!true)
			{
				InputManager.SetInputDevice(InputManager.InputDevices.touch);
				this.inputDeviceText.text = "触摸";
				PlayerPrefs.SetInt("InputDevice", 0);
				return;
			}
			InputManager.SetInputDevice(InputManager.InputDevices.mouseKeyboard);
			this.inputDeviceText.text = "键鼠";
			PlayerPrefs.SetInt("InputDevice", 1);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00043782 File Offset: 0x00041982
		public void SetRes(int resModeIndex)
		{
			this.SetRes((ResModes)resModeIndex);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0004378C File Offset: 0x0004198C
		public void SetRes(ResModes mode)
		{
			this.resMode = mode;
			this.screenRes.x = (float)Display.main.systemWidth;
			this.screenRes.y = (float)Display.main.systemHeight;
			PlayerPrefs.SetInt("ResMode", (int)mode);
			int num = 1;
			int num2 = 1;
			switch (this.resMode)
			{
			case ResModes.Source:
				num = Mathf.RoundToInt(this.screenRes.x);
				num2 = Mathf.RoundToInt(this.screenRes.y);
				break;
			case ResModes.HalfRes:
				num = Mathf.RoundToInt(this.screenRes.x / 2f);
				num2 = Mathf.RoundToInt(this.screenRes.y / 2f);
				break;
			case ResModes.R720p:
				num = Mathf.RoundToInt(this.screenRes.x / this.screenRes.y * 720f);
				num2 = 720;
				break;
			case ResModes.R480p:
				num = Mathf.RoundToInt(this.screenRes.x / this.screenRes.y * 480f);
				num2 = 480;
				break;
			}
			this.resText.text = string.Format("{0}x{1}", num, num2);
			Screen.SetResolution(num, num2, 1);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x000438E1 File Offset: 0x00041AE1
		public void SetTexture(int texModeIndex)
		{
			this.SetTexture((TextureModes)texModeIndex);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000438EC File Offset: 0x00041AEC
		public void SetTexture(TextureModes mode)
		{
			this.texMode = mode;
			QualitySettings.globalTextureMipmapLimit = (int)this.texMode;
			switch (this.texMode)
			{
			case TextureModes.High:
				this.texText.text = "高";
				break;
			case TextureModes.Middle:
				this.texText.text = "中";
				break;
			case TextureModes.Low:
				this.texText.text = "低";
				break;
			case TextureModes.VeryLow:
				this.texText.text = "极低";
				break;
			}
			PlayerPrefs.SetInt("TexMode", (int)this.texMode);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00043990 File Offset: 0x00041B90
		private void OnlevelInited()
		{
			this.SetInvincible(this.invincible);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x000439A0 File Offset: 0x00041BA0
		private void OnSceneLoaded(Scene s1, Scene s2)
		{
			this.SetShadow().Forget();
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x000439BC File Offset: 0x00041BBC
		private UniTaskVoid SetShadow()
		{
			DebugView.<SetShadow>d__49 <SetShadow>d__;
			<SetShadow>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SetShadow>d__.<>4__this = this;
			<SetShadow>d__.<>1__state = -1;
			<SetShadow>d__.<>t__builder.Start<DebugView.<SetShadow>d__49>(ref <SetShadow>d__);
			return <SetShadow>d__.<>t__builder.Task;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000439FF File Offset: 0x00041BFF
		public void ToggleBloom()
		{
			this.bloomActive = !this.bloomActive;
			this.SetBloom(this.bloomActive);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00043A1C File Offset: 0x00041C1C
		private void SetBloom(bool active)
		{
			Bloom bloom;
			bool flag = this.volumeProfile.TryGet<Bloom>(ref bloom);
			this.bloomText.text = (active ? "开" : "关");
			if (flag)
			{
				bloom.active = active;
			}
			this.bloomActive = active;
			PlayerPrefs.SetInt("BloomActive", this.bloomActive ? 1 : 0);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00043A86 File Offset: 0x00041C86
		public void ToggleEdgeLight()
		{
			this.edgeLightActive = !this.edgeLightActive;
			this.SetEdgeLight(this.edgeLightActive);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00043AA4 File Offset: 0x00041CA4
		private void SetEdgeLight(bool active)
		{
			this.edgeLightText.text = (active ? "开" : "关");
			this.edgeLightActive = active;
			PlayerPrefs.SetInt("EdgeLightActive", this.edgeLightActive ? 1 : 0);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.supportsCameraDepthTexture = active;
			}
			this.SetShadow();
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00043B20 File Offset: 0x00041D20
		public void ToggleAO()
		{
			this.aoActive = !this.aoActive;
			this.SetAO(this.aoActive);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00043B3D File Offset: 0x00041D3D
		public void ToggleDof()
		{
			this.dofActive = !this.dofActive;
			this.SetDof(this.dofActive);
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00043B5A File Offset: 0x00041D5A
		public void ToggleInvincible()
		{
			this.invincible = !this.invincible;
			this.SetInvincible(this.invincible);
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00043B77 File Offset: 0x00041D77
		private void SetReporter(bool active)
		{
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00043B79 File Offset: 0x00041D79
		public void ToggleReporter()
		{
			this.SetReporter(!this.reporterActive);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00043B8C File Offset: 0x00041D8C
		private void SetAO(bool active)
		{
			ScriptableRendererFeature scriptableRendererFeature = this.rendererData.rendererFeatures.Find((ScriptableRendererFeature a) => a.name == "ScreenSpaceAmbientOcclusion");
			if (scriptableRendererFeature != null)
			{
				scriptableRendererFeature.SetActive(active);
				this.aoText.text = (active ? "开" : "关");
				PlayerPrefs.SetInt("AOActive", active ? 1 : 0);
			}
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00043C14 File Offset: 0x00041E14
		private void SetDof(bool active)
		{
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00043C16 File Offset: 0x00041E16
		private void SetInvincible(bool active)
		{
			this.invincibleText.text = (active ? "开" : "关");
			this.invincible = active;
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00043C4C File Offset: 0x00041E4C
		public void CreateItem()
		{
			this.CreateItemTask().Forget();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00043C68 File Offset: 0x00041E68
		private UniTaskVoid CreateItemTask()
		{
			DebugView.<CreateItemTask>d__63 <CreateItemTask>d__;
			<CreateItemTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<CreateItemTask>d__.<>4__this = this;
			<CreateItemTask>d__.<>1__state = -1;
			<CreateItemTask>d__.<>t__builder.Start<DebugView.<CreateItemTask>d__63>(ref <CreateItemTask>d__);
			return <CreateItemTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000DA1 RID: 3489
		private DebugView instance;

		// Token: 0x04000DA2 RID: 3490
		private Vector2 screenRes;

		// Token: 0x04000DA3 RID: 3491
		private ResModes resMode;

		// Token: 0x04000DA4 RID: 3492
		private TextureModes texMode;

		// Token: 0x04000DA5 RID: 3493
		public TextMeshProUGUI resText;

		// Token: 0x04000DA6 RID: 3494
		public TextMeshProUGUI texText;

		// Token: 0x04000DA7 RID: 3495
		public TextMeshProUGUI fpsText1;

		// Token: 0x04000DA8 RID: 3496
		public TextMeshProUGUI fpsText2;

		// Token: 0x04000DA9 RID: 3497
		public TextMeshProUGUI inputDeviceText;

		// Token: 0x04000DAA RID: 3498
		public TextMeshProUGUI bloomText;

		// Token: 0x04000DAB RID: 3499
		public TextMeshProUGUI edgeLightText;

		// Token: 0x04000DAC RID: 3500
		public TextMeshProUGUI aoText;

		// Token: 0x04000DAD RID: 3501
		public TextMeshProUGUI dofText;

		// Token: 0x04000DAE RID: 3502
		public TextMeshProUGUI invincibleText;

		// Token: 0x04000DAF RID: 3503
		public TextMeshProUGUI reporterText;

		// Token: 0x04000DB0 RID: 3504
		public UniversalRendererData rendererData;

		// Token: 0x04000DB1 RID: 3505
		private float[] deltaTimes;

		// Token: 0x04000DB2 RID: 3506
		private int frameIndex;

		// Token: 0x04000DB3 RID: 3507
		public int frameSampleCount = 30;

		// Token: 0x04000DB4 RID: 3508
		public GameObject openButton;

		// Token: 0x04000DB5 RID: 3509
		public GameObject panel;

		// Token: 0x04000DB6 RID: 3510
		public VolumeProfile volumeProfile;

		// Token: 0x04000DB7 RID: 3511
		private bool bloomActive;

		// Token: 0x04000DB8 RID: 3512
		private bool edgeLightActive;

		// Token: 0x04000DB9 RID: 3513
		private bool aoActive;

		// Token: 0x04000DBA RID: 3514
		private int inputDevice;

		// Token: 0x04000DBB RID: 3515
		private bool dofActive;

		// Token: 0x04000DBC RID: 3516
		private bool invincible;

		// Token: 0x04000DBD RID: 3517
		private bool reporterActive;

		// Token: 0x04000DBE RID: 3518
		private Light light;

		// Token: 0x04000DBF RID: 3519
		[ItemTypeID]
		public int createItemID;
	}
}
