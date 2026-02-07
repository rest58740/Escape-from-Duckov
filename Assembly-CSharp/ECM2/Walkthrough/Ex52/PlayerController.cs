using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex52
{
	// Token: 0x0200006E RID: 110
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x06000381 RID: 897 RVA: 0x0000F595 File Offset: 0x0000D795
		protected void OnCollided(ref CollisionResult collisionResult)
		{
			Debug.Log("Collided with " + collisionResult.collider.name);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000F5B1 File Offset: 0x0000D7B1
		protected void OnFoundGround(ref FindGroundResult foundGround)
		{
			Debug.Log("Found " + foundGround.collider.name + " ground");
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000F5D2 File Offset: 0x0000D7D2
		protected void OnLanded(Vector3 landingVelocity)
		{
			Debug.Log(string.Format("Landed with {0:F4} landing velocity.", landingVelocity));
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F5E9 File Offset: 0x0000D7E9
		protected void OnCrouched()
		{
			Debug.Log("Crouched");
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000F5F5 File Offset: 0x0000D7F5
		protected void OnUnCrouched()
		{
			Debug.Log("UnCrouched");
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F601 File Offset: 0x0000D801
		protected void OnJumped()
		{
			Debug.Log("Jumped!");
			this._character.notifyJumpApex = true;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F619 File Offset: 0x0000D819
		protected void OnReachedJumpApex()
		{
			Debug.Log(string.Format("Apex reached {0:F4}", this._character.GetVelocity()));
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F63A File Offset: 0x0000D83A
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F648 File Offset: 0x0000D848
		private void OnEnable()
		{
			this._character.Collided += this.OnCollided;
			this._character.FoundGround += this.OnFoundGround;
			this._character.Landed += this.OnLanded;
			this._character.Crouched += this.OnCrouched;
			this._character.UnCrouched += this.OnUnCrouched;
			this._character.Jumped += this.OnJumped;
			this._character.ReachedJumpApex += this.OnReachedJumpApex;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		private void OnDisable()
		{
			this._character.Collided -= this.OnCollided;
			this._character.FoundGround -= this.OnFoundGround;
			this._character.Landed -= this.OnLanded;
			this._character.Crouched -= this.OnCrouched;
			this._character.UnCrouched -= this.OnUnCrouched;
			this._character.Jumped -= this.OnJumped;
			this._character.ReachedJumpApex -= this.OnReachedJumpApex;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
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
			if (this._character.camera)
			{
				vector2 = vector2.relativeTo(this._character.cameraTransform, true);
			}
			this._character.SetMovementDirection(vector2);
			if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
			{
				this._character.Crouch();
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
			{
				this._character.UnCrouch();
			}
			if (Input.GetButtonDown("Jump"))
			{
				this._character.Jump();
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this._character.StopJumping();
			}
		}

		// Token: 0x0400026C RID: 620
		private Character _character;
	}
}
