using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

// Token: 0x0200018B RID: 395
public class NightVisionVisual : MonoBehaviour
{
	// Token: 0x06000C0A RID: 3082 RVA: 0x0003369C File Offset: 0x0003189C
	public void Awake()
	{
		this.CollectRendererData();
		this.Refresh();
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x000336AA File Offset: 0x000318AA
	private void OnDestroy()
	{
		this.nightVisionType = 0;
		this.Refresh();
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x000336BC File Offset: 0x000318BC
	private void CollectRendererData()
	{
		if (this.rendererData == null)
		{
			return;
		}
		for (int i = 0; i < this.rendererData.rendererFeatures.Count; i++)
		{
			if (this.rendererData.rendererFeatures[i].name == this.thermalCharacterRednerFeatureKey)
			{
				this.thermalCharacterRednerFeature = this.rendererData.rendererFeatures[i];
			}
			else if (this.rendererData.rendererFeatures[i].name == this.thermalBackgroundRednerFeatureKey)
			{
				this.thermalBackgroundRednerFeature = this.rendererData.rendererFeatures[i];
			}
		}
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x0003376C File Offset: 0x0003196C
	private void Update()
	{
		bool flag = false;
		int num = this.CheckNightVisionType();
		if (num >= this.nightVisionTypes.Length)
		{
			num = 1;
		}
		if (this.nightVisionType != num)
		{
			this.nightVisionType = num;
			flag = true;
		}
		if (LevelManager.LevelInited != this.levelInited)
		{
			this.levelInited = LevelManager.LevelInited;
			flag = true;
		}
		if (flag)
		{
			this.Refresh();
		}
		if (this.character && this.nightVisionLight.gameObject.activeInHierarchy)
		{
			this.nightVisionLight.transform.position = this.character.transform.position + Vector3.up * 2f;
		}
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00033817 File Offset: 0x00031A17
	private int CheckNightVisionType()
	{
		if (!this.character)
		{
			if (LevelManager.LevelInited)
			{
				this.character = CharacterMainControl.Main;
			}
			return 0;
		}
		return Mathf.RoundToInt(this.character.NightVisionType);
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x0003384C File Offset: 0x00031A4C
	public void Refresh()
	{
		bool flag = this.nightVisionType > 0;
		this.thermalVolume.gameObject.SetActive(flag);
		this.nightVisionLight.gameObject.SetActive(flag);
		NightVisionVisual.NightVisionType nightVisionType = this.nightVisionTypes[this.nightVisionType];
		bool flag2 = nightVisionType.thermalOn && flag;
		bool active = nightVisionType.thermalBackground && flag;
		this.thermalVolume.profile = nightVisionType.profile;
		this.thermalCharacterRednerFeature.SetActive(flag2);
		this.thermalBackgroundRednerFeature.SetActive(active);
		Shader.SetGlobalFloat("ThermalOn", flag2 ? 1f : 0f);
		if (LevelManager.LevelInited)
		{
			if (flag2)
			{
				LevelManager.Instance.FogOfWarManager.mainVis.ObstacleMask = GameplayDataSettings.Layers.fowBlockLayersWithThermal;
				return;
			}
			LevelManager.Instance.FogOfWarManager.mainVis.ObstacleMask = GameplayDataSettings.Layers.fowBlockLayers;
		}
	}

	// Token: 0x04000A54 RID: 2644
	private int nightVisionType;

	// Token: 0x04000A55 RID: 2645
	public Volume thermalVolume;

	// Token: 0x04000A56 RID: 2646
	public NightVisionVisual.NightVisionType[] nightVisionTypes;

	// Token: 0x04000A57 RID: 2647
	private CharacterMainControl character;

	// Token: 0x04000A58 RID: 2648
	public ScriptableRendererData rendererData;

	// Token: 0x04000A59 RID: 2649
	public List<string> renderFeatureNames;

	// Token: 0x04000A5A RID: 2650
	private ScriptableRendererFeature thermalCharacterRednerFeature;

	// Token: 0x04000A5B RID: 2651
	private ScriptableRendererFeature thermalBackgroundRednerFeature;

	// Token: 0x04000A5C RID: 2652
	public Transform nightVisionLight;

	// Token: 0x04000A5D RID: 2653
	public string thermalCharacterRednerFeatureKey = "ThermalCharacter";

	// Token: 0x04000A5E RID: 2654
	public string thermalBackgroundRednerFeatureKey = "ThermalBackground";

	// Token: 0x04000A5F RID: 2655
	private bool levelInited;

	// Token: 0x020004DA RID: 1242
	[Serializable]
	public struct NightVisionType
	{
		// Token: 0x04001D6D RID: 7533
		public string intro;

		// Token: 0x04001D6E RID: 7534
		public VolumeProfile profile;

		// Token: 0x04001D6F RID: 7535
		[FormerlySerializedAs("thermalCharacter")]
		public bool thermalOn;

		// Token: 0x04001D70 RID: 7536
		public bool thermalBackground;
	}
}
