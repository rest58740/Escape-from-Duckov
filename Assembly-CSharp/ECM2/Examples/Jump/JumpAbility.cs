using System;
using UnityEngine;

namespace ECM2.Examples.Jump
{
	// Token: 0x0200008B RID: 139
	public class JumpAbility : MonoBehaviour
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001215F File Offset: 0x0001035F
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x00012167 File Offset: 0x00010367
		public bool canEverJump
		{
			get
			{
				return this._canEverJump;
			}
			set
			{
				this._canEverJump = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00012170 File Offset: 0x00010370
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00012178 File Offset: 0x00010378
		public bool jumpWhileCrouching
		{
			get
			{
				return this._jumpWhileCrouching;
			}
			set
			{
				this._jumpWhileCrouching = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00012181 File Offset: 0x00010381
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x00012189 File Offset: 0x00010389
		public int jumpMaxCount
		{
			get
			{
				return this._jumpMaxCount;
			}
			set
			{
				this._jumpMaxCount = Mathf.Max(1, value);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00012198 File Offset: 0x00010398
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x000121A0 File Offset: 0x000103A0
		public float jumpImpulse
		{
			get
			{
				return this._jumpImpulse;
			}
			set
			{
				this._jumpImpulse = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x000121B3 File Offset: 0x000103B3
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x000121BB File Offset: 0x000103BB
		public float jumpMaxHoldTime
		{
			get
			{
				return this._jumpMaxHoldTime;
			}
			set
			{
				this._jumpMaxHoldTime = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x000121CE File Offset: 0x000103CE
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x000121D6 File Offset: 0x000103D6
		public float jumpPreGroundedTime
		{
			get
			{
				return this._jumpPreGroundedTime;
			}
			set
			{
				this._jumpPreGroundedTime = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x000121E9 File Offset: 0x000103E9
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x000121F1 File Offset: 0x000103F1
		public float jumpPostGroundedTime
		{
			get
			{
				return this._jumpPostGroundedTime;
			}
			set
			{
				this._jumpPostGroundedTime = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00012204 File Offset: 0x00010404
		public bool jumpButtonPressed
		{
			get
			{
				return this._jumpButtonPressed;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0001220C File Offset: 0x0001040C
		public float jumpButtonHeldDownTime
		{
			get
			{
				return this._jumpButtonHeldDownTime;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00012214 File Offset: 0x00010414
		public int jumpCount
		{
			get
			{
				return this._jumpCount;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001221C File Offset: 0x0001041C
		public float jumpHoldTime
		{
			get
			{
				return this._jumpHoldTime;
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00012224 File Offset: 0x00010424
		public virtual bool IsJumping()
		{
			return this._isJumping;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001222C File Offset: 0x0001042C
		public void Jump()
		{
			this._jumpButtonPressed = true;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00012235 File Offset: 0x00010435
		public void StopJumping()
		{
			this._jumpButtonPressed = false;
			this._jumpButtonHeldDownTime = 0f;
			this._isJumping = false;
			this._jumpHoldTime = 0f;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001225B File Offset: 0x0001045B
		public virtual int GetJumpCount()
		{
			return this._jumpCount;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00012264 File Offset: 0x00010464
		public virtual bool CanJump()
		{
			if (!this.canEverJump)
			{
				return false;
			}
			if (this._character.IsCrouched() && !this.jumpWhileCrouching)
			{
				return false;
			}
			if (this.jumpMaxCount == 0 || this._jumpCount >= this.jumpMaxCount)
			{
				return false;
			}
			if (this._jumpCount == 0)
			{
				bool flag = this._character.IsWalking() || (this._character.IsFalling() && this.jumpPostGroundedTime > 0f && this._character.fallingTime < this.jumpPostGroundedTime);
				if (this._character.IsFalling() && !flag)
				{
					flag = (this.jumpMaxCount > 1);
					if (flag)
					{
						this._jumpCount++;
					}
				}
				return flag;
			}
			return this._character.IsFalling();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001232C File Offset: 0x0001052C
		protected virtual Vector3 CalcJumpImpulse()
		{
			Vector3 vector = -this._character.GetGravityDirection();
			float d = Mathf.Max(Vector3.Dot(this._character.GetVelocity(), vector), this.jumpImpulse);
			return vector * d;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00012370 File Offset: 0x00010570
		protected virtual void DoJump(float deltaTime)
		{
			if (this._jumpButtonPressed)
			{
				this._jumpButtonHeldDownTime += deltaTime;
			}
			if (this._jumpButtonPressed && !this.IsJumping())
			{
				if (this.jumpPreGroundedTime > 0f && this._jumpButtonHeldDownTime > this.jumpPreGroundedTime)
				{
					return;
				}
				if (this.CanJump())
				{
					this._character.SetMovementMode(Character.MovementMode.Falling, 0);
					this._character.PauseGroundConstraint(0.1f);
					this._character.LaunchCharacter(this.CalcJumpImpulse(), true, false);
					this._jumpCount++;
					this._isJumping = true;
				}
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00012414 File Offset: 0x00010614
		protected virtual void Jumping(float deltaTime)
		{
			if (!this.canEverJump)
			{
				if (this.IsJumping())
				{
					this.StopJumping();
				}
				return;
			}
			this.DoJump(deltaTime);
			if (this.IsJumping() && this._jumpButtonPressed && this.jumpMaxHoldTime > 0f && this._jumpHoldTime < this.jumpMaxHoldTime)
			{
				Vector3 gravityVector = this._character.GetGravityVector();
				float magnitude = gravityVector.magnitude;
				Vector3 a = (magnitude > 0f) ? (gravityVector / magnitude) : Vector3.zero;
				float t = Mathf.InverseLerp(0f, this.jumpMaxHoldTime, this._jumpHoldTime);
				float d = Mathf.LerpUnclamped(magnitude, 0f, t);
				Vector3 force = -a * d;
				this._character.AddForce(force, ForceMode.Force);
				this._jumpHoldTime += deltaTime;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000124EA File Offset: 0x000106EA
		protected virtual void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMode)
		{
			if (this._character.IsWalking())
			{
				this._jumpCount = 0;
				return;
			}
			if (this._character.IsFlying() || this._character.IsSwimming())
			{
				this.StopJumping();
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00012521 File Offset: 0x00010721
		protected virtual void Reset()
		{
			this._canEverJump = true;
			this._jumpWhileCrouching = true;
			this._jumpMaxCount = 1;
			this._jumpImpulse = 5f;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00012543 File Offset: 0x00010743
		protected virtual void OnValidate()
		{
			this.jumpMaxCount = this._jumpMaxCount;
			this.jumpImpulse = this._jumpImpulse;
			this.jumpMaxHoldTime = this._jumpMaxHoldTime;
			this.jumpPreGroundedTime = this._jumpPreGroundedTime;
			this.jumpPostGroundedTime = this._jumpPostGroundedTime;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00012581 File Offset: 0x00010781
		protected virtual void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001258F File Offset: 0x0001078F
		protected virtual void OnEnable()
		{
			this._character.MovementModeChanged += this.OnMovementModeChanged;
			this._character.BeforeSimulationUpdated += this.Jumping;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000125C1 File Offset: 0x000107C1
		protected virtual void OnDisable()
		{
			this._character.BeforeSimulationUpdated -= this.Jumping;
			this._character.MovementModeChanged -= this.OnMovementModeChanged;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000125F3 File Offset: 0x000107F3
		protected virtual void Start()
		{
			this._character.canEverJump = false;
		}

		// Token: 0x040002BF RID: 703
		[Space(15f)]
		[Tooltip("Is the character able to jump ?")]
		[SerializeField]
		private bool _canEverJump;

		// Token: 0x040002C0 RID: 704
		[Tooltip("Can jump while crouching ?")]
		[SerializeField]
		private bool _jumpWhileCrouching;

		// Token: 0x040002C1 RID: 705
		[Tooltip("The max number of jumps the Character can perform.")]
		[SerializeField]
		private int _jumpMaxCount;

		// Token: 0x040002C2 RID: 706
		[Tooltip("Initial velocity (instantaneous vertical velocity) when jumping.")]
		[SerializeField]
		private float _jumpImpulse;

		// Token: 0x040002C3 RID: 707
		[Tooltip("The maximum time (in seconds) to hold the jump. eg: Variable height jump.")]
		[SerializeField]
		private float _jumpMaxHoldTime;

		// Token: 0x040002C4 RID: 708
		[Tooltip("How early before hitting the ground you can trigger a jump (in seconds).")]
		[SerializeField]
		private float _jumpPreGroundedTime;

		// Token: 0x040002C5 RID: 709
		[Tooltip("How long after leaving the ground you can trigger a jump (in seconds).")]
		[SerializeField]
		private float _jumpPostGroundedTime;

		// Token: 0x040002C6 RID: 710
		private Character _character;

		// Token: 0x040002C7 RID: 711
		protected bool _jumpButtonPressed;

		// Token: 0x040002C8 RID: 712
		protected float _jumpButtonHeldDownTime;

		// Token: 0x040002C9 RID: 713
		protected float _jumpHoldTime;

		// Token: 0x040002CA RID: 714
		protected int _jumpCount;

		// Token: 0x040002CB RID: 715
		protected bool _isJumping;
	}
}
