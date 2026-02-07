using System;
using ECM2;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class Movement : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000402 RID: 1026 RVA: 0x00011DA3 File Offset: 0x0000FFA3
	public float walkSpeed
	{
		get
		{
			return this.characterController.CharacterWalkSpeed * (this.characterController.IsInAdsInput ? this.characterController.AdsWalkSpeedMultiplier : 1f);
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x00011DD0 File Offset: 0x0000FFD0
	public float originWalkSpeed
	{
		get
		{
			return this.characterController.CharacterOriginWalkSpeed;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000404 RID: 1028 RVA: 0x00011DDD File Offset: 0x0000FFDD
	public float runSpeed
	{
		get
		{
			return this.characterController.CharacterRunSpeed;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x00011DEA File Offset: 0x0000FFEA
	public float originRunSpeed
	{
		get
		{
			return this.characterController.CharacterOriginRunSpeed;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x00011DF7 File Offset: 0x0000FFF7
	public float walkAcc
	{
		get
		{
			return this.characterController.CharacterWalkAcc;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00011E04 File Offset: 0x00010004
	public float runAcc
	{
		get
		{
			return this.characterController.CharacterRunAcc;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x00011E11 File Offset: 0x00010011
	public float turnSpeed
	{
		get
		{
			return this.characterController.CharacterTurnSpeed;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x00011E1E File Offset: 0x0001001E
	public float aimTurnSpeed
	{
		get
		{
			return this.characterController.CharacterAimTurnSpeed;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600040A RID: 1034 RVA: 0x00011E2B File Offset: 0x0001002B
	public Vector3 MoveInput
	{
		get
		{
			return this.moveInput;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x00011E33 File Offset: 0x00010033
	public bool Running
	{
		get
		{
			return this.running;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x00011E3B File Offset: 0x0001003B
	public bool Moving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x00011E43 File Offset: 0x00010043
	public bool IsOnGround
	{
		get
		{
			return this.characterMovement.isOnGround;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x00011E50 File Offset: 0x00010050
	public bool StandStill
	{
		get
		{
			return !this.moving && this.characterMovement.velocity.magnitude < 0.1f;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x00011E73 File Offset: 0x00010073
	private bool checkCanMove
	{
		get
		{
			return this.characterController.CanMove();
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x00011E80 File Offset: 0x00010080
	private bool checkCanRun
	{
		get
		{
			return this.characterController.CanRun();
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x00011E8D File Offset: 0x0001008D
	public Vector3 CurrentMoveDirectionXZ
	{
		get
		{
			return this.currentMoveDirectionXZ;
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x00011E95 File Offset: 0x00010095
	public Transform rotationRoot
	{
		get
		{
			return this.characterController.modelRoot;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000413 RID: 1043 RVA: 0x00011EA2 File Offset: 0x000100A2
	public unsafe Vector3 Velocity
	{
		get
		{
			return *this.characterMovement.velocity;
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00011EB4 File Offset: 0x000100B4
	private void Awake()
	{
		this.characterMovement.constrainToGround = true;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00011EC2 File Offset: 0x000100C2
	public void SetMoveInput(Vector3 _moveInput)
	{
		_moveInput.y = 0f;
		this.moveInput = _moveInput;
		this.moving = false;
		if (this.checkCanMove && this.moveInput.magnitude > 0.02f)
		{
			this.moving = true;
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00011EFF File Offset: 0x000100FF
	public void SetForceMoveVelocity(Vector3 _forceMoveVelocity)
	{
		this.forceMove = true;
		this.forceMoveVelocity = _forceMoveVelocity;
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00011F0F File Offset: 0x0001010F
	public void SetAimDirection(Vector3 _aimDirection)
	{
		this.targetAimDirection = _aimDirection;
		this.targetAimDirection.y = 0f;
		this.targetAimDirection.Normalize();
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00011F34 File Offset: 0x00010134
	public void SetAimDirectionToTarget(Vector3 targetPoint, Transform aimHandler)
	{
		Vector3 position = base.transform.position;
		position.y = 0f;
		Vector3 position2 = aimHandler.position;
		position2.y = 0f;
		targetPoint.y = 0f;
		float num = Vector3.Distance(position, targetPoint);
		float num2 = Vector3.Distance(position, position2);
		if (num < num2 + 0.25f)
		{
			return;
		}
		float num3 = Mathf.Asin(num2 / num) * 57.29578f;
		this.targetAimDirection = Quaternion.Euler(0f, -num3, 0f) * (targetPoint - position).normalized;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00011FD0 File Offset: 0x000101D0
	public void SetPushCharacter(bool push)
	{
		this.characterMovement.AllowPushCharacters = push;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00011FE0 File Offset: 0x000101E0
	private void UpdateAiming()
	{
		Vector3 currentAimPoint = this.characterController.GetCurrentAimPoint();
		currentAimPoint.y = base.transform.position.y;
		if (Vector3.Distance(currentAimPoint, base.transform.position) > 0.6f && this.characterController.IsAiming() && this.characterController.CanControlAim())
		{
			this.SetAimDirectionToTarget(currentAimPoint, this.characterController.CurrentUsingAimSocket);
			return;
		}
		if (this.Moving)
		{
			this.SetAimDirection(this.CurrentMoveDirectionXZ);
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001206C File Offset: 0x0001026C
	public unsafe void UpdateMovement()
	{
		bool checkCanRun = this.checkCanRun;
		bool checkCanMove = this.checkCanMove;
		if (this.moveInput.magnitude <= 0.02f || !checkCanMove)
		{
			this.moving = false;
			this.running = false;
		}
		else
		{
			this.moving = true;
		}
		if (!checkCanRun)
		{
			this.running = false;
		}
		if (this.moving && checkCanRun)
		{
			this.running = true;
		}
		if (!this.forceMove)
		{
			this.UpdateNormalMove();
		}
		else
		{
			this.UpdateForceMove();
			this.forceMove = false;
		}
		this.UpdateAiming();
		this.UpdateRotation(Time.deltaTime);
		*this.characterMovement.velocity += Physics.gravity * Time.deltaTime;
		this.characterMovement.Move(*this.characterMovement.velocity, Time.deltaTime);
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00012147 File Offset: 0x00010347
	private void Update()
	{
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00012149 File Offset: 0x00010349
	public unsafe void ForceSetPosition(Vector3 Pos)
	{
		this.characterMovement.PauseGroundConstraint(1f);
		this.characterMovement.SetPosition(Pos, false);
		*this.characterMovement.velocity = Vector3.zero;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00012180 File Offset: 0x00010380
	private unsafe void UpdateNormalMove()
	{
		Vector3 vector = *this.characterMovement.velocity;
		Vector3 vector2 = Vector3.zero;
		float num = this.walkAcc;
		if (this.moving)
		{
			vector2 = this.moveInput * (this.running ? this.runSpeed : this.walkSpeed);
			num = (this.running ? this.runAcc : this.walkAcc);
		}
		vector2.y = vector.y;
		vector = Vector3.MoveTowards(vector, vector2, num * Time.deltaTime);
		Vector3 vector3 = vector;
		vector3.y = 0f;
		if (vector3.magnitude > 0.02f)
		{
			this.currentMoveDirectionXZ = vector3.normalized;
		}
		*this.characterMovement.velocity = vector;
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00012244 File Offset: 0x00010444
	private unsafe void UpdateForceMove()
	{
		Vector3 vector = *this.characterMovement.velocity;
		Vector3 vector2 = this.forceMoveVelocity;
		float walkAcc = this.walkAcc;
		vector2.y = vector.y;
		vector = vector2;
		Vector3 vector3 = vector;
		vector3.y = 0f;
		if (vector3.magnitude > 0.02f)
		{
			this.currentMoveDirectionXZ = vector3.normalized;
		}
		*this.characterMovement.velocity = vector;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x000122BC File Offset: 0x000104BC
	public void ForceTurnTo(Vector3 direction)
	{
		this.targetAimDirection = direction.normalized;
		Quaternion rotation = Quaternion.Euler(0f, Quaternion.LookRotation(this.targetAimDirection, Vector3.up).eulerAngles.y, 0f);
		this.rotationRoot.rotation = rotation;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00012310 File Offset: 0x00010510
	private void UpdateRotation(float deltaTime)
	{
		if (this.targetAimDirection.magnitude < 0.1f)
		{
			this.targetAimDirection = this.rotationRoot.forward;
		}
		float num = this.turnSpeed;
		if (this.characterController.IsAiming() && this.characterController.IsMainCharacter)
		{
			num = this.aimTurnSpeed;
		}
		if (this.targetAimDirection.magnitude > 0.1f)
		{
			Quaternion quaternion = Quaternion.Euler(0f, Quaternion.LookRotation(this.targetAimDirection, Vector3.up).eulerAngles.y, 0f);
			this.rotationRoot.rotation = Quaternion.RotateTowards(this.rotationRoot.rotation, quaternion, num * deltaTime);
		}
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000123C6 File Offset: 0x000105C6
	public void ForceSetAimDirectionToAimPoint()
	{
		this.UpdateRotation(99999f);
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000123D4 File Offset: 0x000105D4
	public float GetMoveAnimationValue()
	{
		float magnitude = this.characterMovement.velocity.magnitude;
		float num;
		if (this.moving && this.running)
		{
			num = Mathf.InverseLerp(this.walkSpeed, this.runSpeed, magnitude) + 1f;
			num *= this.walkSpeed / this.originWalkSpeed;
		}
		else
		{
			num = Mathf.Clamp01(magnitude / this.walkSpeed);
			num *= this.walkSpeed / this.originWalkSpeed;
		}
		if (this.walkSpeed <= 0f)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00012468 File Offset: 0x00010668
	public Vector2 GetLocalMoveDirectionAnimationValue()
	{
		Vector2 up = Vector2.up;
		if (!this.StandStill)
		{
			Vector3 vector = this.currentMoveDirectionXZ;
			Vector3 vector2 = this.rotationRoot.InverseTransformDirection(vector);
			up.x = vector2.x;
			up.y = vector2.z;
		}
		return up;
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000124B2 File Offset: 0x000106B2
	private void FixedUpdate()
	{
	}

	// Token: 0x04000312 RID: 786
	public CharacterMainControl characterController;

	// Token: 0x04000313 RID: 787
	[SerializeField]
	private CharacterMovement characterMovement;

	// Token: 0x04000314 RID: 788
	public Vector3 targetAimDirection;

	// Token: 0x04000315 RID: 789
	private Vector3 moveInput;

	// Token: 0x04000316 RID: 790
	private bool running;

	// Token: 0x04000317 RID: 791
	private bool moving;

	// Token: 0x04000318 RID: 792
	private Vector3 currentMoveDirectionXZ;

	// Token: 0x04000319 RID: 793
	public bool forceMove;

	// Token: 0x0400031A RID: 794
	public Vector3 forceMoveVelocity;

	// Token: 0x0400031B RID: 795
	private const float movingInputThreshold = 0.02f;
}
