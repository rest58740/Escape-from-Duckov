using System;
using Duckov;
using Duckov.Scenes;
using Duckov.Weathers;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200019E RID: 414
public class WeatherFxControl : MonoBehaviour
{
	// Token: 0x06000C63 RID: 3171 RVA: 0x0003506D File Offset: 0x0003326D
	private void Start()
	{
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00035070 File Offset: 0x00033270
	private void Init()
	{
		this.inited = true;
		this.rainingParticleRate = new float[this.rainyFxParticles.Length];
		for (int i = 0; i < this.rainyFxParticles.Length; i++)
		{
			ParticleSystem.EmissionModule emission = this.rainyFxParticles[i].emission;
			this.rainingParticleRate[i] = emission.rateOverTime.constant;
		}
		this.SetFxActive(false);
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x000350D6 File Offset: 0x000332D6
	private void OnSubSceneChanged()
	{
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x000350D8 File Offset: 0x000332D8
	private void Update()
	{
		if (!this.inited)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			this.Init();
			this.SetFxActive(false);
			return;
		}
		else
		{
			if (!TimeOfDayController.Instance)
			{
				return;
			}
			if (!MultiSceneCore.Instance)
			{
				return;
			}
			bool flag = TimeOfDayController.Instance.CurrentWeather == this.targetWeather;
			SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
			if (this.onlyOutDoor && subSceneInfo.IsInDoor)
			{
				flag = false;
				this.lerpValue = 0f;
			}
			if (flag)
			{
				this.overTimer = this.deactiveDelay;
				if (!this.fxActive)
				{
					this.SetFxActive(true);
				}
			}
			else if (this.lerpValue <= 0.01f)
			{
				this.overTimer -= Time.deltaTime;
				if (this.overTimer <= 0f)
				{
					this.SetFxActive(false);
				}
			}
			if (!this.fxActive)
			{
				return;
			}
			this.lerpValue = Mathf.MoveTowards(this.lerpValue, flag ? 1f : 0f, Time.deltaTime / this.lerpTime);
			for (int i = 0; i < this.rainyFxParticles.Length; i++)
			{
				ParticleSystem.EmissionModule emission = this.rainyFxParticles[i].emission;
				float num = this.rainingParticleRate[i];
				emission.rateOverTime = Mathf.Lerp(0f, num, this.lerpValue);
			}
			if (flag != this.audioPlaying)
			{
				this.audioPlaying = flag;
				if (flag)
				{
					this.weatherSoundInstace = AudioManager.Post(this.rainSoundKey, base.gameObject);
					return;
				}
				if (this.weatherSoundInstace != null)
				{
					this.weatherSoundInstace.Value.stop(0);
				}
			}
			return;
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00035284 File Offset: 0x00033484
	private void SetFxActive(bool active)
	{
		foreach (ParticleSystem particleSystem in this.rainyFxParticles)
		{
			if (!(particleSystem == null))
			{
				particleSystem.gameObject.SetActive(active);
			}
		}
		this.fxActive = active;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x000352C8 File Offset: 0x000334C8
	private void OnDestroy()
	{
		if (this.weatherSoundInstace != null)
		{
			this.weatherSoundInstace.Value.stop(0);
		}
	}

	// Token: 0x04000AD3 RID: 2771
	public ParticleSystem[] rainyFxParticles;

	// Token: 0x04000AD4 RID: 2772
	[HideInInspector]
	public float[] rainingParticleRate;

	// Token: 0x04000AD5 RID: 2773
	public Weather targetWeather;

	// Token: 0x04000AD6 RID: 2774
	private float targetParticleRate;

	// Token: 0x04000AD7 RID: 2775
	private float lerpValue;

	// Token: 0x04000AD8 RID: 2776
	public float lerpTime = 5f;

	// Token: 0x04000AD9 RID: 2777
	public float deactiveDelay = 10f;

	// Token: 0x04000ADA RID: 2778
	private float overTimer;

	// Token: 0x04000ADB RID: 2779
	private bool fxActive;

	// Token: 0x04000ADC RID: 2780
	private bool inited;

	// Token: 0x04000ADD RID: 2781
	private EventInstance? weatherSoundInstace;

	// Token: 0x04000ADE RID: 2782
	public string rainSoundKey = "Amb/amb_rain";

	// Token: 0x04000ADF RID: 2783
	private bool audioPlaying;

	// Token: 0x04000AE0 RID: 2784
	[FormerlySerializedAs("onlyInDoor")]
	public bool onlyOutDoor = true;
}
