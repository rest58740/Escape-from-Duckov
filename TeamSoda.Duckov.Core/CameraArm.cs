using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200004A RID: 74
public class CameraArm : MonoBehaviour
{
	// Token: 0x060001CB RID: 459 RVA: 0x00008AA0 File Offset: 0x00006CA0
	private void Update()
	{
		if (this.volumeInfluence)
		{
			this.pitch = Mathf.Lerp(this.pitch, CameraArm.globalPitch, 5f * Time.deltaTime);
			this.yaw = Mathf.Lerp(this.yaw, CameraArm.globalYaw, 2f * Time.deltaTime);
			this.distance = Mathf.Lerp(this.distance, CameraArm.globalDistance, 2f * Time.deltaTime);
		}
		this.UpdateArm();
		if (CameraArm.topDownView != this.topDownViewVolume.enabled)
		{
			this.topDownViewVolume.enabled = CameraArm.topDownView;
		}
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00008B40 File Offset: 0x00006D40
	public static void ToggleView()
	{
		CameraArm.topDownView = !CameraArm.topDownView;
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00008B50 File Offset: 0x00006D50
	private void UpdateArm()
	{
		this.pitchRoot.transform.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
		this.virtualCamera.transform.localPosition = new Vector3(0f, 0f, -this.distance);
		this.virtualCamera.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00008BBE File Offset: 0x00006DBE
	private void OnValidate()
	{
		this.UpdateArm();
	}

	// Token: 0x04000173 RID: 371
	public bool volumeInfluence;

	// Token: 0x04000174 RID: 372
	public float pitch;

	// Token: 0x04000175 RID: 373
	public float yaw;

	// Token: 0x04000176 RID: 374
	public float distance;

	// Token: 0x04000177 RID: 375
	public static float globalPitch = 55f;

	// Token: 0x04000178 RID: 376
	public static float globalYaw = -30f;

	// Token: 0x04000179 RID: 377
	public static float globalDistance = -45f;

	// Token: 0x0400017A RID: 378
	public Transform pitchRoot;

	// Token: 0x0400017B RID: 379
	public Transform yawRoot;

	// Token: 0x0400017C RID: 380
	public CinemachineVirtualCamera virtualCamera;

	// Token: 0x0400017D RID: 381
	public GameCamera gameCamera;

	// Token: 0x0400017E RID: 382
	private static bool topDownView = false;

	// Token: 0x0400017F RID: 383
	public Volume topDownViewVolume;
}
