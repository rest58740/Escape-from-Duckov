using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.Examples.HelloWorld
{
	// Token: 0x020002DE RID: 734
	public class Move : MiniGameBehaviour
	{
		// Token: 0x06001772 RID: 6002 RVA: 0x000566C3 File Offset: 0x000548C3
		private void Awake()
		{
			if (this.rigidbody == null)
			{
				this.rigidbody = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x000566E0 File Offset: 0x000548E0
		protected override void OnUpdate(float deltaTime)
		{
			bool flag = this.CanJump();
			Vector2 vector = base.Game.GetAxis(0) * this.speed;
			float y = this.rigidbody.velocity.y;
			if (base.Game.GetButtonDown(MiniGame.Button.A) && flag)
			{
				y = this.jumpSpeed;
			}
			this.rigidbody.velocity = new Vector3(vector.x, y, vector.y);
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00056751 File Offset: 0x00054951
		private bool CanJump()
		{
			return this.touchingColliders.Count > 0;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00056764 File Offset: 0x00054964
		private void OnCollisionEnter(Collision collision)
		{
			this.touchingColliders.Add(collision.collider);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00056777 File Offset: 0x00054977
		private void OnCollisionExit(Collision collision)
		{
			this.touchingColliders.Remove(collision.collider);
		}

		// Token: 0x04001124 RID: 4388
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x04001125 RID: 4389
		[SerializeField]
		private float speed = 10f;

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private float jumpSpeed = 5f;

		// Token: 0x04001127 RID: 4391
		private List<Collider> touchingColliders = new List<Collider>();
	}
}
