using System;
using ECM2;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002E7 RID: 743
	public class FPSMovement : Character
	{
		// Token: 0x060017A8 RID: 6056 RVA: 0x000570F8 File Offset: 0x000552F8
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00057100 File Offset: 0x00055300
		protected override void Start()
		{
			base.Start();
			if (this.game == null)
			{
				this.game.GetComponentInParent<MiniGame>();
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00057122 File Offset: 0x00055322
		public void SetGame(MiniGame game)
		{
			this.game = game;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0005712B File Offset: 0x0005532B
		private void Update()
		{
			this.UpdateRotation();
			this.UpdateMovement();
			if (this.game.GetButtonDown(MiniGame.Button.B))
			{
				this.Jump();
				return;
			}
			if (this.game.GetButtonUp(MiniGame.Button.B))
			{
				this.StopJumping();
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00057164 File Offset: 0x00055364
		private void UpdateMovement()
		{
			Vector2 axis = this.game.GetAxis(0);
			Vector3 vector = Vector3.zero;
			vector += Vector3.right * axis.x;
			vector += Vector3.forward * axis.y;
			if (base.camera)
			{
				vector = Extensions.relativeTo(vector, base.cameraTransform, true);
			}
			base.SetMovementDirection(vector);
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x000571D4 File Offset: 0x000553D4
		private void UpdateRotation()
		{
			Vector2 axis = this.game.GetAxis(1);
			this.AddYawInput(axis.x * this.lookSensitivity.x);
			if (axis.y == 0f)
			{
				return;
			}
			float num = MathLib.ClampAngle(-base.cameraTransform.localRotation.eulerAngles.x + axis.y * this.lookSensitivity.y, -80f, 80f);
			base.cameraTransform.localRotation = Quaternion.Euler(-num, 0f, 0f);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0005726C File Offset: 0x0005546C
		public void AddControlYawInput(float value)
		{
			this.AddYawInput(value);
		}

		// Token: 0x04001155 RID: 4437
		[SerializeField]
		private MiniGame game;

		// Token: 0x04001156 RID: 4438
		[SerializeField]
		private Vector2 lookSensitivity;
	}
}
