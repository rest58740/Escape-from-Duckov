using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200019C RID: 412
public class TimeOfDayVolumeControl : MonoBehaviour
{
	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00034EC7 File Offset: 0x000330C7
	public VolumeProfile CurrentProfile
	{
		get
		{
			return this.currentProfile;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00034ECF File Offset: 0x000330CF
	public VolumeProfile BufferTargetProfile
	{
		get
		{
			return this.bufferTargetProfile;
		}
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00034ED8 File Offset: 0x000330D8
	private void Update()
	{
		if (!this.blending && this.bufferTargetProfile != null)
		{
			this.StartBlendToBufferdTarget();
		}
		if (this.blending)
		{
			this.UpdateBlending(Time.deltaTime);
		}
		if (!this.blending && this.fromVolume.gameObject.activeSelf)
		{
			this.fromVolume.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00034F40 File Offset: 0x00033140
	private void UpdateBlending(float deltaTime)
	{
		this.blendTimer += deltaTime;
		float num = this.blendTimer / this.blendTime;
		if (num > 1f)
		{
			num = 1f;
			this.blending = false;
		}
		this.toVolume.weight = this.blendCurve.Evaluate(num);
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00034F95 File Offset: 0x00033195
	public void SetTargetProfile(VolumeProfile profile)
	{
		this.bufferTargetProfile = profile;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00034FA0 File Offset: 0x000331A0
	private void StartBlendToBufferdTarget()
	{
		this.blending = true;
		this.blendingTargetProfile = this.bufferTargetProfile;
		this.bufferTargetProfile = null;
		this.currentProfile = this.blendingTargetProfile;
		this.fromVolume.gameObject.SetActive(true);
		this.fromVolume.profile = this.toVolume.profile;
		this.fromVolume.weight = 1f;
		this.toVolume.profile = this.blendingTargetProfile;
		this.toVolume.weight = 0f;
		this.blendTimer = 0f;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00035036 File Offset: 0x00033236
	public void ForceSetProfile(VolumeProfile profile)
	{
		this.bufferTargetProfile = profile;
		this.StartBlendToBufferdTarget();
		this.UpdateBlending(999f);
	}

	// Token: 0x04000AC7 RID: 2759
	private VolumeProfile currentProfile;

	// Token: 0x04000AC8 RID: 2760
	private VolumeProfile blendingTargetProfile;

	// Token: 0x04000AC9 RID: 2761
	private VolumeProfile bufferTargetProfile;

	// Token: 0x04000ACA RID: 2762
	public Volume fromVolume;

	// Token: 0x04000ACB RID: 2763
	public Volume toVolume;

	// Token: 0x04000ACC RID: 2764
	private bool blending;

	// Token: 0x04000ACD RID: 2765
	private float blendTimer;

	// Token: 0x04000ACE RID: 2766
	public float blendTime = 2f;

	// Token: 0x04000ACF RID: 2767
	public AnimationCurve blendCurve;
}
