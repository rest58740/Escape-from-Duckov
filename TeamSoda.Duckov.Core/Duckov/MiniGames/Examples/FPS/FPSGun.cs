using System;
using DG.Tweening;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002E4 RID: 740
	public class FPSGun : MiniGameBehaviour
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x000569C8 File Offset: 0x00054BC8
		public float ScatterAngle
		{
			get
			{
				return Mathf.Lerp(this.minScatterAngle, this.maxScatterAngle, this.scatterStatus);
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000569E4 File Offset: 0x00054BE4
		private void Fire()
		{
			this.coolDown = 1f / this.fireRate;
			this.DoCast();
			this.muzzleFlash.Play();
			this.DoFireAnimation();
			this.scatterStatus = Mathf.MoveTowards(this.scatterStatus, 1f, this.scatterIncrementPerShot);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00056A38 File Offset: 0x00054C38
		private void DoFireAnimation()
		{
			ShortcutExtensions.DOKill(this.graphicsTransform, true);
			this.graphicsTransform.localPosition = Vector3.zero;
			this.graphicsTransform.localRotation = Quaternion.identity;
			ShortcutExtensions.DOPunchPosition(this.graphicsTransform, Vector3.back * 0.2f, 0.2f, 10, 1f, false);
			ShortcutExtensions.DOShakeRotation(this.graphicsTransform, 0.5f, -Vector3.right * 10f, 10, 90f, true);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00056AC8 File Offset: 0x00054CC8
		private void DoCast()
		{
			Ray ray = this.mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
			Vector2 vector = Random.insideUnitCircle * this.ScatterAngle / 2f;
			Vector3 vector2 = Quaternion.Euler(vector.y, vector.x, 0f) * Vector3.forward;
			Vector3 direction = this.mainCamera.transform.localToWorldMatrix.MultiplyVector(vector2);
			ray.direction = direction;
			RaycastHit castInfo;
			Physics.Raycast(ray, ref castInfo, 100f, this.castLayers);
			this.HandleBulletTracer(castInfo);
			if (castInfo.collider == null)
			{
				return;
			}
			FPSDamageInfo fpsdamageInfo = new FPSDamageInfo
			{
				source = this,
				amount = 1f,
				point = castInfo.point,
				normal = castInfo.normal
			};
			FPSDamageReceiver component = castInfo.collider.GetComponent<FPSDamageReceiver>();
			if (component)
			{
				component.CastDamage(fpsdamageInfo);
				return;
			}
			this.HandleNormalHit(fpsdamageInfo);
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00056BE8 File Offset: 0x00054DE8
		private void HandleBulletTracer(RaycastHit castInfo)
		{
			if (this.bulletTracer == null)
			{
				return;
			}
			if (!true)
			{
				return;
			}
			Vector3 position = this.muzzle.transform.position;
			Vector3 vector = this.muzzle.transform.forward;
			if (castInfo.collider != null)
			{
				vector = castInfo.point - position;
				if ((castInfo.point - position).magnitude < 5f)
				{
					this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -vector);
					this.bulletTracer.transform.position = castInfo.point;
				}
				else
				{
					this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector);
					this.bulletTracer.transform.position = this.muzzle.position;
				}
			}
			else
			{
				this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector);
				this.bulletTracer.transform.position = this.muzzle.position;
			}
			this.bulletTracer.Emit(1);
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00056D19 File Offset: 0x00054F19
		private void HandleNormalHit(FPSDamageInfo info)
		{
			FXPool.Play(this.normalHitFXPrefab, info.point, Quaternion.FromToRotation(Vector3.forward, info.normal));
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00056D3D File Offset: 0x00054F3D
		internal void SetTrigger(bool value)
		{
			this.trigger = value;
			if (value)
			{
				this.justPressedTrigger = true;
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00056D50 File Offset: 0x00054F50
		internal void Setup(Camera mainCamera, Transform gunParent)
		{
			base.transform.SetParent(gunParent, false);
			base.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			this.mainCamera = mainCamera;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00056D7C File Offset: 0x00054F7C
		protected override void OnUpdate(float deltaTime)
		{
			if (this.coolDown > 0f)
			{
				this.coolDown -= deltaTime;
				this.coolDown = Mathf.Max(0f, this.coolDown);
			}
			if (this.coolDown <= 0f && this.trigger && (this.auto || this.justPressedTrigger))
			{
				this.Fire();
			}
			this.justPressedTrigger = false;
			this.scatterStatus = Mathf.MoveTowards(this.scatterStatus, 0f, this.scatterDecayRate * deltaTime);
			this.UpdateGunPhysicsStatus(deltaTime);
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x00056E11 File Offset: 0x00055011
		private void UpdateGunPhysicsStatus(float deltaTime)
		{
		}

		// Token: 0x04001136 RID: 4406
		[SerializeField]
		private float fireRate = 1f;

		// Token: 0x04001137 RID: 4407
		[SerializeField]
		private bool auto;

		// Token: 0x04001138 RID: 4408
		[SerializeField]
		private Transform muzzle;

		// Token: 0x04001139 RID: 4409
		[SerializeField]
		private ParticleSystem muzzleFlash;

		// Token: 0x0400113A RID: 4410
		[SerializeField]
		private ParticleSystem bulletTracer;

		// Token: 0x0400113B RID: 4411
		[SerializeField]
		private LayerMask castLayers = -1;

		// Token: 0x0400113C RID: 4412
		[SerializeField]
		private ParticleSystem normalHitFXPrefab;

		// Token: 0x0400113D RID: 4413
		[SerializeField]
		private float minScatterAngle;

		// Token: 0x0400113E RID: 4414
		[SerializeField]
		private float maxScatterAngle;

		// Token: 0x0400113F RID: 4415
		[SerializeField]
		private float scatterIncrementPerShot;

		// Token: 0x04001140 RID: 4416
		[SerializeField]
		private float scatterDecayRate;

		// Token: 0x04001141 RID: 4417
		[SerializeField]
		private Transform graphicsTransform;

		// Token: 0x04001142 RID: 4418
		[SerializeField]
		private FPSGun.Pose idlePose;

		// Token: 0x04001143 RID: 4419
		[SerializeField]
		private FPSGun.Pose recoilPose;

		// Token: 0x04001144 RID: 4420
		private float scatterStatus;

		// Token: 0x04001145 RID: 4421
		private float coolDown;

		// Token: 0x04001146 RID: 4422
		private Camera mainCamera;

		// Token: 0x04001147 RID: 4423
		private bool trigger;

		// Token: 0x04001148 RID: 4424
		private bool justPressedTrigger;

		// Token: 0x02000595 RID: 1429
		[Serializable]
		public struct Pose
		{
			// Token: 0x0600298A RID: 10634 RVA: 0x0009A18C File Offset: 0x0009838C
			public static FPSGun.Pose Extraterpolate(FPSGun.Pose poseA, FPSGun.Pose poseB, float t)
			{
				return new FPSGun.Pose
				{
					localPosition = Vector3.LerpUnclamped(poseA.localPosition, poseB.localPosition, t),
					localRotation = Quaternion.LerpUnclamped(poseA.localRotation, poseB.localRotation, t)
				};
			}

			// Token: 0x0600298B RID: 10635 RVA: 0x0009A1D4 File Offset: 0x000983D4
			public Pose(Transform fromTransform)
			{
				this.localPosition = fromTransform.localPosition;
				this.localRotation = fromTransform.localRotation;
			}

			// Token: 0x04002069 RID: 8297
			[SerializeField]
			private Vector3 localPosition;

			// Token: 0x0400206A RID: 8298
			[SerializeField]
			private Quaternion localRotation;
		}
	}
}
