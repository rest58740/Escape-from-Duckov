using System;
using Duckov;
using Duckov.Utilities;
using FMOD.Studio;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class ItemAgent_Kazoo : DuckovItemAgent
{
	// Token: 0x060007E5 RID: 2021 RVA: 0x00023C6C File Offset: 0x00021E6C
	private void Update()
	{
		if (!base.Holder)
		{
			return;
		}
		if (!this.camera)
		{
			if (GameCamera.Instance)
			{
				this.camera = GameCamera.Instance.renderCamera;
			}
			if (!this.camera)
			{
				return;
			}
		}
		if (!this.holderInited)
		{
			base.Holder.OnTriggerInputUpdateEvent += this.OnTriggerUpdate;
			this.uiInstance = Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.KazooUi, base.Holder.transform.position, quaternion.identity);
			this.uiInstance.transform.localScale = Vector3.one * 2f * this.maxScale;
			this.SyncUi(base.Holder.transform);
			this.holderInited = true;
		}
		if (this.targetMakingSound != this.currentMakingSound)
		{
			this.makeAiSoundCoolTimer = this.makeAiSoundCoolTime;
			this.currentMakingSound = this.targetMakingSound;
			if (this.currentMakingSound)
			{
				if (this.currentEvent != null)
				{
					this.currentEvent.Value.stop(0);
				}
				this.currentEvent = AudioManager.Post(this.audioEvent, base.gameObject);
				if (this.particle)
				{
					this.particle.Emit(1);
				}
			}
			else if (this.currentEvent != null)
			{
				this.currentEvent.Value.stop(0);
			}
		}
		if (this.currentMakingSound)
		{
			Vector3 right = this.camera.transform.right;
			right.y = 0f;
			right.Normalize();
			Vector3 position = base.Holder.transform.position;
			Vector3 vector = base.Holder.GetCurrentAimPoint() - position;
			vector.y = 0f;
			float value = Vector3.Dot(right, vector) * 24f / this.maxScale;
			AudioManager.SetRTPC("Kazoo/Pitch", value, base.gameObject);
			AudioManager.SetRTPC("Kazoo/Intensity", 1f, base.gameObject);
			this.makeAiSoundCoolTimer -= Time.deltaTime;
			if (this.makeAiSoundCoolTimer <= 0f)
			{
				this.makeAiSoundCoolTimer = this.makeAiSoundCoolTime;
				AIMainBrain.MakeSound(new AISound
				{
					fromCharacter = base.Holder,
					fromObject = base.gameObject,
					pos = base.transform.position,
					fromTeam = base.Holder.Team,
					soundType = SoundTypes.unknowNoise,
					radius = this.maxSoundRange
				});
			}
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00023F20 File Offset: 0x00022120
	private void LateUpdate()
	{
		if (base.Holder)
		{
			this.SyncUi(base.Holder.transform);
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00023F40 File Offset: 0x00022140
	public void OnTriggerUpdate(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		this.targetMakingSound = trigger;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00023F49 File Offset: 0x00022149
	protected override void OnInitialize()
	{
		base.OnInitialize();
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00023F54 File Offset: 0x00022154
	private void SyncUi(Transform parent)
	{
		if (!this.uiInstance || !parent)
		{
			return;
		}
		Vector3 forward = this.camera.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		this.uiInstance.transform.position = parent.position - forward * this.zOffset;
		this.uiInstance.transform.rotation = quaternion.LookRotation(forward, Vector3.up);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00023FEC File Offset: 0x000221EC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.uiInstance)
		{
			Object.Destroy(this.uiInstance.gameObject);
		}
		if (base.Holder)
		{
			base.Holder.OnTriggerInputUpdateEvent -= this.OnTriggerUpdate;
		}
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00024040 File Offset: 0x00022240
	private void OnDisable()
	{
		if (this.currentEvent != null)
		{
			this.currentEvent.Value.stop(0);
		}
	}

	// Token: 0x0400077D RID: 1917
	private string audioEvent = "SFX/Special/Kazoo";

	// Token: 0x0400077E RID: 1918
	private EventInstance? currentEvent;

	// Token: 0x0400077F RID: 1919
	private bool currentMakingSound;

	// Token: 0x04000780 RID: 1920
	private bool targetMakingSound;

	// Token: 0x04000781 RID: 1921
	private float makeAiSoundCoolTimer = 0.15f;

	// Token: 0x04000782 RID: 1922
	private float makeAiSoundCoolTime = 0.15f;

	// Token: 0x04000783 RID: 1923
	public float maxScale = 15f;

	// Token: 0x04000784 RID: 1924
	public float maxSoundRange = 18f;

	// Token: 0x04000785 RID: 1925
	private Camera camera;

	// Token: 0x04000786 RID: 1926
	private bool holderInited;

	// Token: 0x04000787 RID: 1927
	private GameObject uiInstance;

	// Token: 0x04000788 RID: 1928
	private float zOffset = 6f;

	// Token: 0x04000789 RID: 1929
	[SerializeField]
	private ParticleSystem particle;
}
