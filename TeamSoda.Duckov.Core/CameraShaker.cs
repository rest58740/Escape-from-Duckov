using System;
using Cinemachine;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class CameraShaker : MonoBehaviour
{
	// Token: 0x06000BD1 RID: 3025 RVA: 0x00032896 File Offset: 0x00030A96
	private void Awake()
	{
		CameraShaker._instance = this;
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000328A0 File Offset: 0x00030AA0
	public static void Shake(Vector3 velocity, CameraShaker.CameraShakeTypes shakeType)
	{
		if (CameraShaker._instance == null)
		{
			return;
		}
		switch (shakeType)
		{
		case CameraShaker.CameraShakeTypes.recoil:
			CameraShaker._instance.recoilSource.GenerateImpulseWithVelocity(velocity);
			return;
		case CameraShaker.CameraShakeTypes.explosion:
			CameraShaker._instance.explosionSource.GenerateImpulseWithVelocity(velocity);
			return;
		case CameraShaker.CameraShakeTypes.meleeAttackHit:
			CameraShaker._instance.meleeAttackSource.GenerateImpulseWithVelocity(velocity);
			return;
		default:
			return;
		}
	}

	// Token: 0x04000A1F RID: 2591
	private static CameraShaker _instance;

	// Token: 0x04000A20 RID: 2592
	public CinemachineImpulseSource recoilSource;

	// Token: 0x04000A21 RID: 2593
	public CinemachineImpulseSource meleeAttackSource;

	// Token: 0x04000A22 RID: 2594
	public CinemachineImpulseSource explosionSource;

	// Token: 0x020004D7 RID: 1239
	public enum CameraShakeTypes
	{
		// Token: 0x04001D63 RID: 7523
		recoil,
		// Token: 0x04001D64 RID: 7524
		explosion,
		// Token: 0x04001D65 RID: 7525
		meleeAttackHit
	}
}
