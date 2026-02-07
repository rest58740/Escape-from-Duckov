using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace SymmetryBreakStudio.TastyGrassShader.Example
{
	// Token: 0x02000005 RID: 5
	public class DemoGUI : MonoBehaviour
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002228 File Offset: 0x00000428
		private void Update()
		{
			this._smoothedFrameTime = Mathf.SmoothDamp(this._smoothedFrameTime, Time.deltaTime, ref this._frameTimeVelocity, 1f);
			if (!this.storedOriginalSettings && TastyGrassShaderGlobalSettings.LastActiveInstance != null)
			{
				this.storedOriginalSettings = true;
				TastyGrassShaderGlobalSettings lastActiveInstance = TastyGrassShaderGlobalSettings.LastActiveInstance;
				this.originalDensity = lastActiveInstance.densityScale;
				this.originalLodScale = lastActiveInstance.lodScale;
				this.originalLodFalloffExponent = lastActiveInstance.lodFalloffExponent;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000229C File Offset: 0x0000049C
		private IEnumerator LoadLightScene()
		{
			if (!SceneManager.GetSceneByName(this.lightSettingsScene).isLoaded)
			{
				yield return SceneManager.LoadSceneAsync(this.lightSettingsScene, LoadSceneMode.Additive);
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(this.lightSettingsScene));
			}
			yield break;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022AB File Offset: 0x000004AB
		private void OnEnable()
		{
			this.tgsTerrain = this.unityTerrain.GetComponents<TgsForUnityTerrain>()[0];
			base.StartCoroutine(this.LoadLightScene());
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022CD File Offset: 0x000004CD
		private void OnDestroy()
		{
			if (this.storedOriginalSettings && TastyGrassShaderGlobalSettings.LastActiveInstance != null)
			{
				TastyGrassShaderGlobalSettings lastActiveInstance = TastyGrassShaderGlobalSettings.LastActiveInstance;
				lastActiveInstance.densityScale = this.originalDensity;
				lastActiveInstance.lodScale = this.originalLodScale;
				lastActiveInstance.lodFalloffExponent = this.originalLodFalloffExponent;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000230C File Offset: 0x0000050C
		private void OnGUI()
		{
			if (TastyGrassShaderGlobalSettings.LastActiveInstance == null)
			{
				GUI.Window(2, new Rect(10f, 10f, 300f, 600f), new GUI.WindowFunction(this.AddRenderFeatureWindow), "Error");
				return;
			}
			GUI.Window(0, new Rect(10f, 10f, 300f, 520f), new GUI.WindowFunction(this.MainGuiWindow), "Settings");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002389 File Offset: 0x00000589
		private void AddRenderFeatureWindow(int id)
		{
			GUILayout.Label("<b> Please add the Tasty Grass Shader Render Feature. </b>", Array.Empty<GUILayoutOption>());
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000239C File Offset: 0x0000059C
		private void MainGuiWindow(int id)
		{
			GUILayout.Label("<b> Tasty Grass Shader - Demo (v. 2.1.3) </b>", Array.Empty<GUILayoutOption>());
			TastyGrassShaderGlobalSettings lastActiveInstance = TastyGrassShaderGlobalSettings.LastActiveInstance;
			GUILayout.Label("<b>General</b>", Array.Empty<GUILayoutOption>());
			GUILayout.Label(string.Format("Screen Resolution: {0}x{1}", Screen.width, Screen.height), Array.Empty<GUILayoutOption>());
			GUILayout.Label(string.Format("FPS: {0:F} ({1:F}MS)", 1f / this._smoothedFrameTime, this._smoothedFrameTime * 1000f), Array.Empty<GUILayoutOption>());
			float num = (float)TgsGlobalStatus.instancesReady / (float)TgsGlobalStatus.instances * 100f;
			GUILayout.Label(string.Format("Baking Status: {0:F}%\n({1} Chunks Active, {2} Chunks Ready)", num, TgsGlobalStatus.instances, TgsGlobalStatus.instancesReady), Array.Empty<GUILayoutOption>());
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
			GUILayout.Space(12f);
			GUILayout.Label("<b>Tasty Grass Shader - Settings</b>", Array.Empty<GUILayoutOption>());
			this.CheckBox("Enable Tasty Grass Shader", ref TgsManager.Enable);
			lastActiveInstance.densityScale = this.SliderFloat("Global Density", lastActiveInstance.densityScale, 0.01f, 4f);
			lastActiveInstance.lodScale = this.SliderFloat("Global Lod Scale", lastActiveInstance.lodScale, 0.01f, 4f);
			lastActiveInstance.lodFalloffExponent = this.SliderFloat("Global LOD Falloff Exponent", lastActiveInstance.lodFalloffExponent, 1f, 10f);
			GUILayout.Space(12f);
			GUILayout.Label("<b>Rendering Settings</b>", Array.Empty<GUILayoutOption>());
			bool flag = universalRenderPipelineAsset.msaaSampleCount == 4;
			this.CheckBox("MSAA 4x", ref flag);
			this.CheckBox("No Alpha To Coverage", ref lastActiveInstance.noAlphaToCoverage);
			universalRenderPipelineAsset.msaaSampleCount = (flag ? 4 : 1);
			if (GUILayout.Button("Setup Benchmarking Setting", Array.Empty<GUILayoutOption>()))
			{
				Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 300);
				universalRenderPipelineAsset.msaaSampleCount = 1;
				lastActiveInstance.noAlphaToCoverage = true;
				lastActiveInstance.densityScale = 0.5f;
				lastActiveInstance.lodScale = 1f;
				lastActiveInstance.lodFalloffExponent = 2.5f;
			}
			GUILayout.Space(12f);
			if (GUILayout.Button("Exit", Array.Empty<GUILayoutOption>()))
			{
				Application.Quit();
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025C8 File Offset: 0x000007C8
		private float SliderFloat(string label, float value, float min, float max)
		{
			GUILayout.Label(label, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("   ", Array.Empty<GUILayoutOption>());
			float num = GUILayout.HorizontalSlider(value, min, max, new GUILayoutOption[]
			{
				GUILayout.Width(220f)
			});
			GUILayout.Label(string.Format("{0:F}", num), Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			return num;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002638 File Offset: 0x00000838
		private void SliderInt(string label, ref int value, int min, int max)
		{
			GUILayout.Label(label, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("   ", Array.Empty<GUILayoutOption>());
			value = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)value, (float)min, (float)max, new GUILayoutOption[]
			{
				GUILayout.Width(220f)
			}));
			GUILayout.Label(string.Format("{0:F}", value), Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000026B0 File Offset: 0x000008B0
		private void CheckBox(string label, ref bool value)
		{
			value = GUILayout.Toggle(value, label, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x04000009 RID: 9
		private const float frameSmoothTime = 1f;

		// Token: 0x0400000A RID: 10
		private const float sliderValueLabelWidth = 220f;

		// Token: 0x0400000B RID: 11
		public GameObject unityTerrain;

		// Token: 0x0400000C RID: 12
		public string lightSettingsScene;

		// Token: 0x0400000D RID: 13
		public TgsWindSettings windSettings;

		// Token: 0x0400000E RID: 14
		private float _frameTimeVelocity;

		// Token: 0x0400000F RID: 15
		private float _smoothedFrameTime;

		// Token: 0x04000010 RID: 16
		private bool originalAlphaClip;

		// Token: 0x04000011 RID: 17
		private float originalDensity;

		// Token: 0x04000012 RID: 18
		private float originalLodScale;

		// Token: 0x04000013 RID: 19
		private float originalLodFalloffExponent;

		// Token: 0x04000014 RID: 20
		private bool storedOriginalSettings;

		// Token: 0x04000015 RID: 21
		private TgsForUnityTerrain tgsTerrain;
	}
}
