using System;
using UnityEngine;

namespace ECM2.Examples.OrientToGround
{
	// Token: 0x02000086 RID: 134
	public class CharacterOrientToGround : MonoBehaviour, IColliderFilter
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x000115B8 File Offset: 0x0000F7B8
		public bool Filter(Collider otherCollider)
		{
			CharacterMovement characterMovement = this._character.GetCharacterMovement();
			return otherCollider == characterMovement.collider;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000115E4 File Offset: 0x0000F7E4
		private Vector3 ComputeAverageNormal()
		{
			CharacterMovement characterMovement = this._character.GetCharacterMovement();
			Vector3 up = Vector3.up;
			Vector3 a = this._character.GetPosition() + up * (characterMovement.height * 0.5f);
			Vector3 direction = -up;
			float height = characterMovement.height;
			LayerMask mask = this.groundMask;
			Vector3 vector = Vector3.zero;
			float num = -this.rayOffset;
			float num2 = -this.rayOffset;
			int num3 = 0;
			for (int i = 0; i < 3; i++)
			{
				num2 = -this.rayOffset;
				for (int j = 0; j < 3; j++)
				{
					RaycastHit raycastHit;
					if (CollisionDetection.Raycast(a + new Vector3(num, 0f, num2), direction, height, mask, QueryTriggerInteraction.Ignore, out raycastHit, this._hits, this) > 0 && Vector3.Angle(raycastHit.normal, up) < this.maxSlopeAngle)
					{
						vector += raycastHit.normal;
						if (this.drawRays)
						{
							Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.yellow);
						}
						num3++;
					}
					num2 += this.rayOffset;
				}
				num += this.rayOffset;
			}
			if (num3 > 0)
			{
				vector /= (float)num3;
			}
			else
			{
				vector = up;
			}
			if (this.drawRays)
			{
				Debug.DrawRay(this._character.GetPosition(), vector * 2f, Color.green);
			}
			return vector;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00011764 File Offset: 0x0000F964
		private void OnAfterSimulationUpdated(float deltaTime)
		{
			Vector3 toDirection = this._character.IsWalking() ? this.ComputeAverageNormal() : Vector3.up;
			Quaternion quaternion = this._character.GetRotation();
			Quaternion lhs = Quaternion.FromToRotation(quaternion * Vector3.up, toDirection);
			quaternion = Quaternion.Slerp(quaternion, lhs * quaternion, this.alignRate * deltaTime);
			this._character.SetRotation(quaternion);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000117CC File Offset: 0x0000F9CC
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000117DA File Offset: 0x0000F9DA
		private void OnEnable()
		{
			this._character.AfterSimulationUpdated += this.OnAfterSimulationUpdated;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000117F3 File Offset: 0x0000F9F3
		private void OnDisable()
		{
			this._character.AfterSimulationUpdated -= this.OnAfterSimulationUpdated;
		}

		// Token: 0x040002A3 RID: 675
		public float maxSlopeAngle = 30f;

		// Token: 0x040002A4 RID: 676
		public float alignRate = 10f;

		// Token: 0x040002A5 RID: 677
		public float rayOffset = 0.1f;

		// Token: 0x040002A6 RID: 678
		public LayerMask groundMask = 1;

		// Token: 0x040002A7 RID: 679
		[Space(15f)]
		public bool drawRays = true;

		// Token: 0x040002A8 RID: 680
		private readonly RaycastHit[] _hits = new RaycastHit[8];

		// Token: 0x040002A9 RID: 681
		private Character _character;
	}
}
