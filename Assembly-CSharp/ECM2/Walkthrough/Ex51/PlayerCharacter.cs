using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex51
{
	// Token: 0x0200006F RID: 111
	public class PlayerCharacter : Character
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0000F8BF File Offset: 0x0000DABF
		protected override void OnCollided(ref CollisionResult collisionResult)
		{
			base.OnCollided(ref collisionResult);
			Debug.Log("Collided with " + collisionResult.collider.name);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000F8E2 File Offset: 0x0000DAE2
		protected override void OnFoundGround(ref FindGroundResult foundGround)
		{
			base.OnFoundGround(ref foundGround);
			Debug.Log("Found " + foundGround.collider.name + " ground");
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000F90A File Offset: 0x0000DB0A
		protected override void OnLanded(Vector3 landingVelocity)
		{
			base.OnLanded(landingVelocity);
			Debug.Log(string.Format("Landed with {0:F4} landing velocity.", landingVelocity));
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000F928 File Offset: 0x0000DB28
		protected override void OnCrouched()
		{
			base.OnCrouched();
			Debug.Log("Crouched");
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000F93A File Offset: 0x0000DB3A
		protected override void OnUnCrouched()
		{
			base.OnUnCrouched();
			Debug.Log("UnCrouched");
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000F94C File Offset: 0x0000DB4C
		protected override void OnJumped()
		{
			base.OnJumped();
			Debug.Log("Jumped!");
			base.notifyJumpApex = true;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000F965 File Offset: 0x0000DB65
		protected override void OnReachedJumpApex()
		{
			base.OnReachedJumpApex();
			Debug.Log(string.Format("Apex reached {0:F4}", base.GetVelocity()));
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000F988 File Offset: 0x0000DB88
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			vector2 += Vector3.right * vector.x;
			vector2 += Vector3.forward * vector.y;
			if (base.camera)
			{
				vector2 = vector2.relativeTo(base.cameraTransform, true);
			}
			base.SetMovementDirection(vector2);
			if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
			{
				this.Crouch();
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
			{
				this.UnCrouch();
			}
			if (Input.GetButtonDown("Jump"))
			{
				this.Jump();
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this.StopJumping();
			}
		}
	}
}
