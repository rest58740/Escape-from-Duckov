using System;
using UnityEngine;

namespace ECM2.Examples.Glide
{
	// Token: 0x0200008C RID: 140
	public class GlideAbility : MonoBehaviour
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00012609 File Offset: 0x00010809
		public bool glideInputPressed
		{
			get
			{
				return this._glideInputPressed;
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00012611 File Offset: 0x00010811
		public virtual bool IsGliding()
		{
			return this._isGliding;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00012619 File Offset: 0x00010819
		public virtual void Glide()
		{
			this._glideInputPressed = true;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00012622 File Offset: 0x00010822
		public virtual void StopGliding()
		{
			this._glideInputPressed = false;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001262B File Offset: 0x0001082B
		protected virtual bool IsGlideAllowed()
		{
			return this.canEverGlide && this._character.IsFalling();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00012644 File Offset: 0x00010844
		protected virtual bool CanGlide()
		{
			bool flag = this.IsGlideAllowed();
			if (flag)
			{
				Vector3 rhs = -this._character.GetGravityDirection();
				flag = (Vector3.Dot(this._character.GetVelocity(), rhs) < 0f);
			}
			return flag;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00012688 File Offset: 0x00010888
		protected virtual void CheckGlideInput()
		{
			if (!this._isGliding && this._glideInputPressed && this.CanGlide())
			{
				this._isGliding = true;
				this._character.maxFallSpeed = this.maxFallSpeedGliding;
				return;
			}
			if (this._isGliding && (!this._glideInputPressed || !this.CanGlide()))
			{
				this._isGliding = false;
				this._character.maxFallSpeed = 40f;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000126F5 File Offset: 0x000108F5
		private void OnBeforeCharacterSimulationUpdated(float deltaTime)
		{
			this.CheckGlideInput();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000126FD File Offset: 0x000108FD
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001270B File Offset: 0x0001090B
		private void OnEnable()
		{
			this._character.BeforeSimulationUpdated += this.OnBeforeCharacterSimulationUpdated;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00012724 File Offset: 0x00010924
		private void OnDisable()
		{
			this._character.BeforeSimulationUpdated -= this.OnBeforeCharacterSimulationUpdated;
		}

		// Token: 0x040002CC RID: 716
		public bool canEverGlide = true;

		// Token: 0x040002CD RID: 717
		public float maxFallSpeedGliding = 1f;

		// Token: 0x040002CE RID: 718
		private Character _character;

		// Token: 0x040002CF RID: 719
		protected bool _glideInputPressed;

		// Token: 0x040002D0 RID: 720
		protected bool _isGliding;
	}
}
