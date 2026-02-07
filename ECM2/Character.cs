using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000003 RID: 3
	[RequireComponent(typeof(CharacterMovement))]
	public class Character : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		public Camera camera
		{
			get
			{
				return this._camera;
			}
			set
			{
				this._camera = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020D1 File Offset: 0x000002D1
		public Transform cameraTransform
		{
			get
			{
				if (this._camera != null)
				{
					this._cameraTransform = this._camera.transform;
				}
				return this._cameraTransform;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020F8 File Offset: 0x000002F8
		public new Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002100 File Offset: 0x00000300
		public CharacterMovement characterMovement
		{
			get
			{
				return this._characterMovement;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002108 File Offset: 0x00000308
		public Animator animator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002110 File Offset: 0x00000310
		public RootMotionController rootMotionController
		{
			get
			{
				return this._rootMotionController;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002118 File Offset: 0x00000318
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002120 File Offset: 0x00000320
		public float rotationRate
		{
			get
			{
				return this._rotationRate;
			}
			set
			{
				this._rotationRate = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002129 File Offset: 0x00000329
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002131 File Offset: 0x00000331
		public Character.RotationMode rotationMode
		{
			get
			{
				return this._rotationMode;
			}
			set
			{
				this._rotationMode = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000213A File Offset: 0x0000033A
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002142 File Offset: 0x00000342
		public float maxWalkSpeed
		{
			get
			{
				return this._maxWalkSpeed;
			}
			set
			{
				this._maxWalkSpeed = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002155 File Offset: 0x00000355
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000215D File Offset: 0x0000035D
		public float minAnalogWalkSpeed
		{
			get
			{
				return this._minAnalogWalkSpeed;
			}
			set
			{
				this._minAnalogWalkSpeed = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002170 File Offset: 0x00000370
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002178 File Offset: 0x00000378
		public float maxAcceleration
		{
			get
			{
				return this._maxAcceleration;
			}
			set
			{
				this._maxAcceleration = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000218B File Offset: 0x0000038B
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002193 File Offset: 0x00000393
		public float brakingDecelerationWalking
		{
			get
			{
				return this._brakingDecelerationWalking;
			}
			set
			{
				this._brakingDecelerationWalking = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000021A6 File Offset: 0x000003A6
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000021AE File Offset: 0x000003AE
		public float groundFriction
		{
			get
			{
				return this._groundFriction;
			}
			set
			{
				this._groundFriction = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000021C1 File Offset: 0x000003C1
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000021C9 File Offset: 0x000003C9
		public bool canEverCrouch
		{
			get
			{
				return this._canEverCrouch;
			}
			set
			{
				this._canEverCrouch = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000021D2 File Offset: 0x000003D2
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000021DA File Offset: 0x000003DA
		public float crouchedHeight
		{
			get
			{
				return this._crouchedHeight;
			}
			set
			{
				this._crouchedHeight = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000021ED File Offset: 0x000003ED
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000021F5 File Offset: 0x000003F5
		public float unCrouchedHeight
		{
			get
			{
				return this._unCrouchedHeight;
			}
			set
			{
				this._unCrouchedHeight = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002208 File Offset: 0x00000408
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002210 File Offset: 0x00000410
		public float maxWalkSpeedCrouched
		{
			get
			{
				return this._maxWalkSpeedCrouched;
			}
			set
			{
				this._maxWalkSpeedCrouched = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002223 File Offset: 0x00000423
		// (set) Token: 0x06000021 RID: 33 RVA: 0x0000222B File Offset: 0x0000042B
		public bool crouchInputPressed { get; protected set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002234 File Offset: 0x00000434
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000223C File Offset: 0x0000043C
		public float maxFallSpeed
		{
			get
			{
				return this._maxFallSpeed;
			}
			set
			{
				this._maxFallSpeed = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000224F File Offset: 0x0000044F
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002257 File Offset: 0x00000457
		public float brakingDecelerationFalling
		{
			get
			{
				return this._brakingDecelerationFalling;
			}
			set
			{
				this._brakingDecelerationFalling = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000226A File Offset: 0x0000046A
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002272 File Offset: 0x00000472
		public float fallingLateralFriction
		{
			get
			{
				return this._fallingLateralFriction;
			}
			set
			{
				this._fallingLateralFriction = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002285 File Offset: 0x00000485
		public float fallingTime
		{
			get
			{
				return this._fallingTime;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000228D File Offset: 0x0000048D
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002295 File Offset: 0x00000495
		public float airControl
		{
			get
			{
				return this._airControl;
			}
			set
			{
				this._airControl = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000022A3 File Offset: 0x000004A3
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000022AB File Offset: 0x000004AB
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

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000022B4 File Offset: 0x000004B4
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000022BC File Offset: 0x000004BC
		public bool canJumpWhileCrouching
		{
			get
			{
				return this._canJumpWhileCrouching;
			}
			set
			{
				this._canJumpWhileCrouching = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000022C5 File Offset: 0x000004C5
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000022CD File Offset: 0x000004CD
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000022DC File Offset: 0x000004DC
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000022E4 File Offset: 0x000004E4
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000022F7 File Offset: 0x000004F7
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000022FF File Offset: 0x000004FF
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002312 File Offset: 0x00000512
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000231A File Offset: 0x0000051A
		public float jumpMaxPreGroundedTime
		{
			get
			{
				return this._jumpMaxPreGroundedTime;
			}
			set
			{
				this._jumpMaxPreGroundedTime = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000232D File Offset: 0x0000052D
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002335 File Offset: 0x00000535
		public float jumpMaxPostGroundedTime
		{
			get
			{
				return this._jumpMaxPostGroundedTime;
			}
			set
			{
				this._jumpMaxPostGroundedTime = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002348 File Offset: 0x00000548
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002350 File Offset: 0x00000550
		public float jumpInputHoldTime
		{
			get
			{
				return this._jumpInputHoldTime;
			}
			protected set
			{
				this._jumpInputHoldTime = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002363 File Offset: 0x00000563
		// (set) Token: 0x0600003C RID: 60 RVA: 0x0000236B File Offset: 0x0000056B
		public float jumpForceTimeRemaining
		{
			get
			{
				return this._jumpForceTimeRemaining;
			}
			protected set
			{
				this._jumpForceTimeRemaining = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000237E File Offset: 0x0000057E
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002386 File Offset: 0x00000586
		public int jumpCurrentCount
		{
			get
			{
				return this._jumpCurrentCount;
			}
			protected set
			{
				this._jumpCurrentCount = Mathf.Max(0, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002395 File Offset: 0x00000595
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0000239D File Offset: 0x0000059D
		public bool notifyJumpApex { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000023A6 File Offset: 0x000005A6
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000023AE File Offset: 0x000005AE
		public bool jumpInputPressed { get; protected set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000023B7 File Offset: 0x000005B7
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000023BF File Offset: 0x000005BF
		public float maxFlySpeed
		{
			get
			{
				return this._maxFlySpeed;
			}
			set
			{
				this._maxFlySpeed = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000023D2 File Offset: 0x000005D2
		// (set) Token: 0x06000046 RID: 70 RVA: 0x000023DA File Offset: 0x000005DA
		public float brakingDecelerationFlying
		{
			get
			{
				return this._brakingDecelerationFlying;
			}
			set
			{
				this._brakingDecelerationFlying = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000023ED File Offset: 0x000005ED
		// (set) Token: 0x06000048 RID: 72 RVA: 0x000023F5 File Offset: 0x000005F5
		public float flyingFriction
		{
			get
			{
				return this._flyingFriction;
			}
			set
			{
				this._flyingFriction = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002408 File Offset: 0x00000608
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002410 File Offset: 0x00000610
		public float maxSwimSpeed
		{
			get
			{
				return this._maxSwimSpeed;
			}
			set
			{
				this._maxSwimSpeed = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002423 File Offset: 0x00000623
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000242B File Offset: 0x0000062B
		public float brakingDecelerationSwimming
		{
			get
			{
				return this._brakingDecelerationSwimming;
			}
			set
			{
				this._brakingDecelerationSwimming = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000243E File Offset: 0x0000063E
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002446 File Offset: 0x00000646
		public float swimmingFriction
		{
			get
			{
				return this._swimmingFriction;
			}
			set
			{
				this._swimmingFriction = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002459 File Offset: 0x00000659
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002461 File Offset: 0x00000661
		public float buoyancy
		{
			get
			{
				return this._buoyancy;
			}
			set
			{
				this._buoyancy = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002474 File Offset: 0x00000674
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000247C File Offset: 0x0000067C
		public bool useSeparateBrakingFriction
		{
			get
			{
				return this._useSeparateBrakingFriction;
			}
			set
			{
				this._useSeparateBrakingFriction = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002485 File Offset: 0x00000685
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000248D File Offset: 0x0000068D
		public float brakingFriction
		{
			get
			{
				return this._brakingFriction;
			}
			set
			{
				this._brakingFriction = Mathf.Max(0f, value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000024A0 File Offset: 0x000006A0
		// (set) Token: 0x06000056 RID: 86 RVA: 0x000024A8 File Offset: 0x000006A8
		public bool useSeparateBrakingDeceleration
		{
			get
			{
				return this._useSeparateBrakingDeceleration;
			}
			set
			{
				this._useSeparateBrakingDeceleration = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000024B1 File Offset: 0x000006B1
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000024B9 File Offset: 0x000006B9
		public float brakingDeceleration
		{
			get
			{
				return this._brakingDeceleration;
			}
			set
			{
				this._brakingDeceleration = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000024C2 File Offset: 0x000006C2
		// (set) Token: 0x0600005A RID: 90 RVA: 0x000024D5 File Offset: 0x000006D5
		public Vector3 gravity
		{
			get
			{
				return this._gravity * this._gravityScale;
			}
			set
			{
				this._gravity = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000024DE File Offset: 0x000006DE
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000024E6 File Offset: 0x000006E6
		public float gravityScale
		{
			get
			{
				return this._gravityScale;
			}
			set
			{
				this._gravityScale = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000024EF File Offset: 0x000006EF
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000024F7 File Offset: 0x000006F7
		public bool useRootMotion
		{
			get
			{
				return this._useRootMotion;
			}
			set
			{
				this._useRootMotion = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002500 File Offset: 0x00000700
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002508 File Offset: 0x00000708
		public bool enablePhysicsInteraction
		{
			get
			{
				return this._enablePhysicsInteraction;
			}
			set
			{
				this._enablePhysicsInteraction = value;
				if (this._characterMovement)
				{
					this._characterMovement.enablePhysicsInteraction = this._enablePhysicsInteraction;
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000252F File Offset: 0x0000072F
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002537 File Offset: 0x00000737
		public bool applyPushForceToCharacters
		{
			get
			{
				return this._applyPushForceToCharacters;
			}
			set
			{
				this._applyPushForceToCharacters = value;
				if (this._characterMovement)
				{
					this._characterMovement.physicsInteractionAffectsCharacters = this._applyPushForceToCharacters;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000255E File Offset: 0x0000075E
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002566 File Offset: 0x00000766
		public bool applyStandingDownwardForce
		{
			get
			{
				return this._applyStandingDownwardForce;
			}
			set
			{
				this._applyStandingDownwardForce = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000256F File Offset: 0x0000076F
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00002578 File Offset: 0x00000778
		public float mass
		{
			get
			{
				return this._mass;
			}
			set
			{
				this._mass = Mathf.Max(1E-07f, value);
				if (this._characterMovement && this._characterMovement.rigidbody)
				{
					this._characterMovement.rigidbody.mass = this._mass;
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000025CB File Offset: 0x000007CB
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000025D3 File Offset: 0x000007D3
		public float pushForceScale
		{
			get
			{
				return this._pushForceScale;
			}
			set
			{
				this._pushForceScale = Mathf.Max(0f, value);
				if (this._characterMovement)
				{
					this._characterMovement.pushForceScale = this._pushForceScale;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002604 File Offset: 0x00000804
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000260C File Offset: 0x0000080C
		public float standingDownwardForceScale
		{
			get
			{
				return this._standingDownwardForceScale;
			}
			set
			{
				this._standingDownwardForceScale = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000261F File Offset: 0x0000081F
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002627 File Offset: 0x00000827
		public bool impartPlatformVelocity
		{
			get
			{
				return this._impartPlatformVelocity;
			}
			set
			{
				this._impartPlatformVelocity = value;
				if (this._characterMovement)
				{
					this._characterMovement.impartPlatformVelocity = this._impartPlatformVelocity;
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000264E File Offset: 0x0000084E
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002656 File Offset: 0x00000856
		public bool impartPlatformMovement
		{
			get
			{
				return this._impartPlatformMovement;
			}
			set
			{
				this._impartPlatformMovement = value;
				if (this._characterMovement)
				{
					this._characterMovement.impartPlatformMovement = this._impartPlatformMovement;
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000267D File Offset: 0x0000087D
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002685 File Offset: 0x00000885
		public bool impartPlatformRotation
		{
			get
			{
				return this._impartPlatformRotation;
			}
			set
			{
				this._impartPlatformRotation = value;
				if (this._characterMovement)
				{
					this._characterMovement.impartPlatformRotation = this._impartPlatformRotation;
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000026AC File Offset: 0x000008AC
		public Vector3 position
		{
			get
			{
				return this.characterMovement.position;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000026B9 File Offset: 0x000008B9
		public Quaternion rotation
		{
			get
			{
				return this.characterMovement.rotation;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000026C6 File Offset: 0x000008C6
		public unsafe Vector3 velocity
		{
			get
			{
				return *this.characterMovement.velocity;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000026D8 File Offset: 0x000008D8
		public float speed
		{
			get
			{
				return this.characterMovement.velocity.magnitude;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000026EA File Offset: 0x000008EA
		public float radius
		{
			get
			{
				return this.characterMovement.radius;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000026F7 File Offset: 0x000008F7
		public float height
		{
			get
			{
				return this.characterMovement.height;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002704 File Offset: 0x00000904
		public Character.MovementMode movementMode
		{
			get
			{
				return this._movementMode;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000270C File Offset: 0x0000090C
		public int customMovementMode
		{
			get
			{
				return this._customMovementMode;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002714 File Offset: 0x00000914
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000271C File Offset: 0x0000091C
		public PhysicsVolume physicsVolume { get; protected set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002725 File Offset: 0x00000925
		// (set) Token: 0x0600007C RID: 124 RVA: 0x0000272D File Offset: 0x0000092D
		public bool enableAutoSimulation
		{
			get
			{
				return this._enableAutoSimulation;
			}
			set
			{
				this._enableAutoSimulation = value;
				this.EnableAutoSimulationCoroutine(this._enableAutoSimulation);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002742 File Offset: 0x00000942
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000274A File Offset: 0x0000094A
		public bool isPaused { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600007F RID: 127 RVA: 0x00002754 File Offset: 0x00000954
		// (remove) Token: 0x06000080 RID: 128 RVA: 0x0000278C File Offset: 0x0000098C
		public event Character.PhysicsVolumeChangedEventHandler PhysicsVolumeChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000081 RID: 129 RVA: 0x000027C4 File Offset: 0x000009C4
		// (remove) Token: 0x06000082 RID: 130 RVA: 0x000027FC File Offset: 0x000009FC
		public event Character.MovementModeChangedEventHandler MovementModeChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000083 RID: 131 RVA: 0x00002834 File Offset: 0x00000A34
		// (remove) Token: 0x06000084 RID: 132 RVA: 0x0000286C File Offset: 0x00000A6C
		public event Character.CustomMovementModeUpdateEventHandler CustomMovementModeUpdated;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000085 RID: 133 RVA: 0x000028A4 File Offset: 0x00000AA4
		// (remove) Token: 0x06000086 RID: 134 RVA: 0x000028DC File Offset: 0x00000ADC
		public event Character.CustomRotationModeUpdateEventHandler CustomRotationModeUpdated;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000087 RID: 135 RVA: 0x00002914 File Offset: 0x00000B14
		// (remove) Token: 0x06000088 RID: 136 RVA: 0x0000294C File Offset: 0x00000B4C
		public event Character.BeforeSimulationUpdateEventHandler BeforeSimulationUpdated;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000089 RID: 137 RVA: 0x00002984 File Offset: 0x00000B84
		// (remove) Token: 0x0600008A RID: 138 RVA: 0x000029BC File Offset: 0x00000BBC
		public event Character.AfterSimulationUpdateEventHandler AfterSimulationUpdated;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600008B RID: 139 RVA: 0x000029F4 File Offset: 0x00000BF4
		// (remove) Token: 0x0600008C RID: 140 RVA: 0x00002A2C File Offset: 0x00000C2C
		public event Character.CharacterMovementUpdateEventHandler CharacterMovementUpdated;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600008D RID: 141 RVA: 0x00002A64 File Offset: 0x00000C64
		// (remove) Token: 0x0600008E RID: 142 RVA: 0x00002A9C File Offset: 0x00000C9C
		public event Character.CollidedEventHandler Collided;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600008F RID: 143 RVA: 0x00002AD4 File Offset: 0x00000CD4
		// (remove) Token: 0x06000090 RID: 144 RVA: 0x00002B0C File Offset: 0x00000D0C
		public event Character.FoundGroundEventHandler FoundGround;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000091 RID: 145 RVA: 0x00002B44 File Offset: 0x00000D44
		// (remove) Token: 0x06000092 RID: 146 RVA: 0x00002B7C File Offset: 0x00000D7C
		public event Character.LandedEventHandled Landed;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000093 RID: 147 RVA: 0x00002BB4 File Offset: 0x00000DB4
		// (remove) Token: 0x06000094 RID: 148 RVA: 0x00002BEC File Offset: 0x00000DEC
		public event Character.CrouchedEventHandler Crouched;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000095 RID: 149 RVA: 0x00002C24 File Offset: 0x00000E24
		// (remove) Token: 0x06000096 RID: 150 RVA: 0x00002C5C File Offset: 0x00000E5C
		public event Character.UnCrouchedEventHandler UnCrouched;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000097 RID: 151 RVA: 0x00002C94 File Offset: 0x00000E94
		// (remove) Token: 0x06000098 RID: 152 RVA: 0x00002CCC File Offset: 0x00000ECC
		public event Character.JumpedEventHandler Jumped;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000099 RID: 153 RVA: 0x00002D04 File Offset: 0x00000F04
		// (remove) Token: 0x0600009A RID: 154 RVA: 0x00002D3C File Offset: 0x00000F3C
		public event Character.ReachedJumpApexEventHandler ReachedJumpApex;

		// Token: 0x0600009B RID: 155 RVA: 0x00002D71 File Offset: 0x00000F71
		protected virtual void OnCustomMovementMode(float deltaTime)
		{
			Character.CustomMovementModeUpdateEventHandler customMovementModeUpdated = this.CustomMovementModeUpdated;
			if (customMovementModeUpdated == null)
			{
				return;
			}
			customMovementModeUpdated(deltaTime);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002D84 File Offset: 0x00000F84
		protected virtual void OnCustomRotationMode(float deltaTime)
		{
			Character.CustomRotationModeUpdateEventHandler customRotationModeUpdated = this.CustomRotationModeUpdated;
			if (customRotationModeUpdated == null)
			{
				return;
			}
			customRotationModeUpdated(deltaTime);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002D97 File Offset: 0x00000F97
		protected virtual void OnBeforeSimulationUpdate(float deltaTime)
		{
			Character.BeforeSimulationUpdateEventHandler beforeSimulationUpdated = this.BeforeSimulationUpdated;
			if (beforeSimulationUpdated == null)
			{
				return;
			}
			beforeSimulationUpdated(deltaTime);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002DAA File Offset: 0x00000FAA
		protected virtual void OnAfterSimulationUpdate(float deltaTime)
		{
			Character.AfterSimulationUpdateEventHandler afterSimulationUpdated = this.AfterSimulationUpdated;
			if (afterSimulationUpdated == null)
			{
				return;
			}
			afterSimulationUpdated(deltaTime);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002DBD File Offset: 0x00000FBD
		protected virtual void OnCharacterMovementUpdated(float deltaTime)
		{
			Character.CharacterMovementUpdateEventHandler characterMovementUpdated = this.CharacterMovementUpdated;
			if (characterMovementUpdated == null)
			{
				return;
			}
			characterMovementUpdated(deltaTime);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002DD0 File Offset: 0x00000FD0
		protected virtual void OnCollided(ref CollisionResult collisionResult)
		{
			Character.CollidedEventHandler collided = this.Collided;
			if (collided == null)
			{
				return;
			}
			collided(ref collisionResult);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002DE3 File Offset: 0x00000FE3
		protected virtual void OnFoundGround(ref FindGroundResult foundGround)
		{
			Character.FoundGroundEventHandler foundGround2 = this.FoundGround;
			if (foundGround2 == null)
			{
				return;
			}
			foundGround2(ref foundGround);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002DF6 File Offset: 0x00000FF6
		protected virtual void OnLanded(Vector3 landingVelocity)
		{
			Character.LandedEventHandled landed = this.Landed;
			if (landed == null)
			{
				return;
			}
			landed(landingVelocity);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002E09 File Offset: 0x00001009
		protected virtual void OnCrouched()
		{
			Character.CrouchedEventHandler crouched = this.Crouched;
			if (crouched == null)
			{
				return;
			}
			crouched();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002E1B File Offset: 0x0000101B
		protected virtual void OnUnCrouched()
		{
			Character.UnCrouchedEventHandler unCrouched = this.UnCrouched;
			if (unCrouched == null)
			{
				return;
			}
			unCrouched();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002E2D File Offset: 0x0000102D
		protected virtual void OnJumped()
		{
			Character.JumpedEventHandler jumped = this.Jumped;
			if (jumped == null)
			{
				return;
			}
			jumped();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002E3F File Offset: 0x0000103F
		protected virtual void OnReachedJumpApex()
		{
			Character.ReachedJumpApexEventHandler reachedJumpApex = this.ReachedJumpApex;
			if (reachedJumpApex == null)
			{
				return;
			}
			reachedJumpApex();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002E51 File Offset: 0x00001051
		public Vector3 GetGravityVector()
		{
			return this.gravity;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002E5C File Offset: 0x0000105C
		public Vector3 GetGravityDirection()
		{
			return this.gravity.normalized;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00002E78 File Offset: 0x00001078
		public float GetGravityMagnitude()
		{
			return this.gravity.magnitude;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00002E93 File Offset: 0x00001093
		public void SetGravityVector(Vector3 newGravityVector)
		{
			this._gravity = newGravityVector;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002E9C File Offset: 0x0000109C
		private void EnableAutoSimulationCoroutine(bool enable)
		{
			if (enable)
			{
				if (this._lateFixedUpdateCoroutine != null)
				{
					base.StopCoroutine(this._lateFixedUpdateCoroutine);
				}
				this._lateFixedUpdateCoroutine = base.StartCoroutine(this.LateFixedUpdate());
				return;
			}
			if (this._lateFixedUpdateCoroutine != null)
			{
				base.StopCoroutine(this._lateFixedUpdateCoroutine);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00002EDC File Offset: 0x000010DC
		protected virtual void CacheComponents()
		{
			this._transform = base.GetComponent<Transform>();
			this._characterMovement = base.GetComponent<CharacterMovement>();
			this._animator = base.GetComponentInChildren<Animator>();
			this._rootMotionController = base.GetComponentInChildren<RootMotionController>();
			this.characterMovement.impartPlatformMovement = this._impartPlatformMovement;
			this.characterMovement.impartPlatformRotation = this._impartPlatformRotation;
			this.characterMovement.impartPlatformVelocity = this._impartPlatformVelocity;
			this.characterMovement.enablePhysicsInteraction = this._enablePhysicsInteraction;
			this.characterMovement.physicsInteractionAffectsCharacters = this._applyPushForceToCharacters;
			this.characterMovement.pushForceScale = this._pushForceScale;
			this.mass = this._mass;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002F8B File Offset: 0x0000118B
		protected virtual void SetPhysicsVolume(PhysicsVolume newPhysicsVolume)
		{
			if (newPhysicsVolume == this.physicsVolume)
			{
				return;
			}
			this.OnPhysicsVolumeChanged(newPhysicsVolume);
			this.physicsVolume = newPhysicsVolume;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002FAC File Offset: 0x000011AC
		protected virtual void OnPhysicsVolumeChanged(PhysicsVolume newPhysicsVolume)
		{
			if (newPhysicsVolume && newPhysicsVolume.waterVolume)
			{
				this.SetMovementMode(Character.MovementMode.Swimming, 0);
			}
			else if (this.IsInWaterPhysicsVolume() && newPhysicsVolume == null && this.IsSwimming())
			{
				this.SetMovementMode(Character.MovementMode.Falling, 0);
			}
			Character.PhysicsVolumeChangedEventHandler physicsVolumeChanged = this.PhysicsVolumeChanged;
			if (physicsVolumeChanged == null)
			{
				return;
			}
			physicsVolumeChanged(newPhysicsVolume);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003008 File Offset: 0x00001208
		protected virtual void UpdatePhysicsVolume(PhysicsVolume newPhysicsVolume)
		{
			Vector3 worldCenter = this.characterMovement.worldCenter;
			if (newPhysicsVolume && newPhysicsVolume.boxCollider.ClosestPoint(worldCenter) == worldCenter)
			{
				this.SetPhysicsVolume(newPhysicsVolume);
				return;
			}
			this.SetPhysicsVolume(null);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000304C File Offset: 0x0000124C
		protected virtual void AddPhysicsVolume(Collider other)
		{
			PhysicsVolume item;
			if (other.TryGetComponent<PhysicsVolume>(out item) && !this._physicsVolumes.Contains(item))
			{
				this._physicsVolumes.Insert(0, item);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003080 File Offset: 0x00001280
		protected virtual void RemovePhysicsVolume(Collider other)
		{
			PhysicsVolume item;
			if (other.TryGetComponent<PhysicsVolume>(out item) && this._physicsVolumes.Contains(item))
			{
				this._physicsVolumes.Remove(item);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000030B4 File Offset: 0x000012B4
		protected virtual void UpdatePhysicsVolumes()
		{
			PhysicsVolume newPhysicsVolume = null;
			int num = int.MinValue;
			int i = 0;
			int count = this._physicsVolumes.Count;
			while (i < count)
			{
				PhysicsVolume physicsVolume = this._physicsVolumes[i];
				if (physicsVolume.priority > num)
				{
					num = physicsVolume.priority;
					newPhysicsVolume = physicsVolume;
				}
				i++;
			}
			this.UpdatePhysicsVolume(newPhysicsVolume);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000310B File Offset: 0x0000130B
		public virtual bool IsInWaterPhysicsVolume()
		{
			return this.physicsVolume && this.physicsVolume.waterVolume;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003127 File Offset: 0x00001327
		public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
		{
			this.characterMovement.AddForce(force, forceMode);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003136 File Offset: 0x00001336
		public void AddExplosionForce(float forceMagnitude, Vector3 origin, float explosionRadius, float upwardModifier, ForceMode forceMode = ForceMode.Force)
		{
			this.characterMovement.AddExplosionForce(forceMagnitude, origin, explosionRadius, upwardModifier, forceMode);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000314A File Offset: 0x0000134A
		public void LaunchCharacter(Vector3 launchVelocity, bool overrideVerticalVelocity = false, bool overrideLateralVelocity = false)
		{
			this.characterMovement.LaunchCharacter(launchVelocity, overrideVerticalVelocity, overrideLateralVelocity);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000315A File Offset: 0x0000135A
		public void DetectCollisions(bool detectCollisions)
		{
			this.characterMovement.detectCollisions = detectCollisions;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003168 File Offset: 0x00001368
		public void IgnoreCollision(Collider otherCollider, bool ignore = true)
		{
			this.characterMovement.IgnoreCollision(otherCollider, ignore);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003177 File Offset: 0x00001377
		public void IgnoreCollision(Rigidbody otherRigidbody, bool ignore = true)
		{
			this.characterMovement.IgnoreCollision(otherRigidbody, ignore);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003186 File Offset: 0x00001386
		public void CapsuleIgnoreCollision(Collider otherCollider, bool ignore = true)
		{
			this.characterMovement.CapsuleIgnoreCollision(otherCollider, ignore);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003195 File Offset: 0x00001395
		public void PauseGroundConstraint(float seconds = 0.1f)
		{
			this.characterMovement.PauseGroundConstraint(seconds);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000031A3 File Offset: 0x000013A3
		public void EnableGroundConstraint(bool enable)
		{
			this.characterMovement.constrainToGround = enable;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000031B1 File Offset: 0x000013B1
		public bool WasOnGround()
		{
			return this.characterMovement.wasOnGround;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000031BE File Offset: 0x000013BE
		public bool IsOnGround()
		{
			return this.characterMovement.isOnGround;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000031CB File Offset: 0x000013CB
		public bool WasOnWalkableGround()
		{
			return this.characterMovement.wasOnWalkableGround;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000031D8 File Offset: 0x000013D8
		public bool IsOnWalkableGround()
		{
			return this.characterMovement.isOnWalkableGround;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000031E5 File Offset: 0x000013E5
		public bool WasGrounded()
		{
			return this.characterMovement.wasGrounded;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000031F2 File Offset: 0x000013F2
		public bool IsGrounded()
		{
			return this.characterMovement.isGrounded;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000031FF File Offset: 0x000013FF
		public CharacterMovement GetCharacterMovement()
		{
			return this.characterMovement;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003207 File Offset: 0x00001407
		public Animator GetAnimator()
		{
			return this.animator;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000320F File Offset: 0x0000140F
		public RootMotionController GetRootMotionController()
		{
			return this.rootMotionController;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003217 File Offset: 0x00001417
		public PhysicsVolume GetPhysicsVolume()
		{
			return this.physicsVolume;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000321F File Offset: 0x0000141F
		public Vector3 GetPosition()
		{
			return this.characterMovement.position;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000322C File Offset: 0x0000142C
		public void SetPosition(Vector3 position, bool updateGround = false)
		{
			this.characterMovement.SetPosition(position, updateGround);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000323B File Offset: 0x0000143B
		public void TeleportPosition(Vector3 newPosition, bool interpolating = true, bool updateGround = false)
		{
			if (interpolating)
			{
				this.characterMovement.interpolation = RigidbodyInterpolation.None;
			}
			this.characterMovement.SetPosition(newPosition, updateGround);
			if (interpolating)
			{
				this.characterMovement.interpolation = RigidbodyInterpolation.Interpolate;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003268 File Offset: 0x00001468
		public Quaternion GetRotation()
		{
			return this.characterMovement.rotation;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003275 File Offset: 0x00001475
		public void SetRotation(Quaternion newRotation)
		{
			this.characterMovement.rotation = newRotation;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003283 File Offset: 0x00001483
		public void TeleportRotation(Quaternion newRotation, bool interpolating = true)
		{
			if (interpolating)
			{
				this.characterMovement.interpolation = RigidbodyInterpolation.None;
			}
			this.characterMovement.SetRotation(newRotation);
			if (interpolating)
			{
				this.characterMovement.interpolation = RigidbodyInterpolation.Interpolate;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000032AF File Offset: 0x000014AF
		public virtual Vector3 GetUpVector()
		{
			return this.transform.up;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000032BC File Offset: 0x000014BC
		public virtual Vector3 GetRightVector()
		{
			return this.transform.right;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000032C9 File Offset: 0x000014C9
		public virtual Vector3 GetForwardVector()
		{
			return this.transform.forward;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000032D8 File Offset: 0x000014D8
		public virtual void RotateTowards(Vector3 worldDirection, float deltaTime, bool updateYawOnly = true)
		{
			Vector3 upVector = this.GetUpVector();
			if (updateYawOnly)
			{
				worldDirection = Vector3.ProjectOnPlane(worldDirection, upVector);
			}
			if (worldDirection == Vector3.zero)
			{
				return;
			}
			Quaternion to = Quaternion.LookRotation(worldDirection, upVector);
			this.characterMovement.rotation = Quaternion.RotateTowards(this.rotation, to, this.rotationRate * deltaTime);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000332D File Offset: 0x0000152D
		protected virtual void RotateWithRootMotion()
		{
			if (this.useRootMotion && this.rootMotionController)
			{
				this.characterMovement.rotation = this.rootMotionController.ConsumeRootMotionRotation() * this.characterMovement.rotation;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000336A File Offset: 0x0000156A
		public unsafe Vector3 GetVelocity()
		{
			return *this.characterMovement.velocity;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000337C File Offset: 0x0000157C
		public unsafe void SetVelocity(Vector3 newVelocity)
		{
			*this.characterMovement.velocity = newVelocity;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000338F File Offset: 0x0000158F
		public float GetSpeed()
		{
			return this.characterMovement.velocity.magnitude;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000033A1 File Offset: 0x000015A1
		public float GetRadius()
		{
			return this.characterMovement.radius;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000033AE File Offset: 0x000015AE
		public float GetHeight()
		{
			return this.characterMovement.height;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000033BB File Offset: 0x000015BB
		public Vector3 GetMovementDirection()
		{
			return this._movementDirection;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000033C3 File Offset: 0x000015C3
		public void SetMovementDirection(Vector3 movementDirection)
		{
			this._movementDirection = movementDirection;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000033CC File Offset: 0x000015CC
		public virtual void SetYaw(float value)
		{
			this.characterMovement.rotation = Quaternion.Euler(0f, value, 0f);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000033E9 File Offset: 0x000015E9
		public virtual void AddYawInput(float value)
		{
			this._rotationInput.y = this._rotationInput.y + value;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000033FB File Offset: 0x000015FB
		public virtual void AddPitchInput(float value)
		{
			this._rotationInput.x = this._rotationInput.x + value;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000340D File Offset: 0x0000160D
		public virtual void AddRollInput(float value)
		{
			this._rotationInput.z = this._rotationInput.z + value;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000341F File Offset: 0x0000161F
		protected virtual void ConsumeRotationInput()
		{
			if (this._rotationInput != Vector3.zero)
			{
				this.characterMovement.rotation *= Quaternion.Euler(this._rotationInput);
				this._rotationInput = Vector3.zero;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000345F File Offset: 0x0000165F
		public Character.MovementMode GetMovementMode()
		{
			return this._movementMode;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003467 File Offset: 0x00001667
		public int GetCustomMovementMode()
		{
			return this._customMovementMode;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003470 File Offset: 0x00001670
		public void SetMovementMode(Character.MovementMode newMovementMode, int newCustomMode = 0)
		{
			if (newMovementMode == this._movementMode && (newMovementMode != Character.MovementMode.Custom || newCustomMode == this._customMovementMode))
			{
				return;
			}
			Character.MovementMode movementMode = this._movementMode;
			int customMovementMode = this._customMovementMode;
			this._movementMode = newMovementMode;
			this._customMovementMode = newCustomMode;
			this.OnMovementModeChanged(movementMode, customMovementMode);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000034B8 File Offset: 0x000016B8
		protected unsafe virtual void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMode)
		{
			switch (this.movementMode)
			{
			case Character.MovementMode.None:
				*this.characterMovement.velocity = Vector3.zero;
				this.characterMovement.ClearAccumulatedForces();
				break;
			case Character.MovementMode.Walking:
				this.ResetJumpState();
				if (prevMovementMode == Character.MovementMode.Flying || prevMovementMode == Character.MovementMode.Swimming)
				{
					this.characterMovement.constrainToGround = true;
				}
				if (prevMovementMode == Character.MovementMode.Falling)
				{
					this.OnLanded(this.characterMovement.landedVelocity);
				}
				break;
			case Character.MovementMode.Falling:
				if (prevMovementMode == Character.MovementMode.Flying || prevMovementMode == Character.MovementMode.Swimming)
				{
					this.characterMovement.constrainToGround = true;
				}
				break;
			case Character.MovementMode.Flying:
			case Character.MovementMode.Swimming:
				this.ResetJumpState();
				this.characterMovement.constrainToGround = false;
				break;
			}
			if (!this.IsFalling())
			{
				this._fallingTime = 0f;
			}
			this.InvokeMovementModeChangedEvent(prevMovementMode, prevCustomMode);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000357E File Offset: 0x0000177E
		protected void InvokeMovementModeChangedEvent(Character.MovementMode prevMovementMode, int prevCustomMode)
		{
			Character.MovementModeChangedEventHandler movementModeChanged = this.MovementModeChanged;
			if (movementModeChanged == null)
			{
				return;
			}
			movementModeChanged(prevMovementMode, prevCustomMode);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003592 File Offset: 0x00001792
		public virtual bool IsWalking()
		{
			return this._movementMode == Character.MovementMode.Walking;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000359D File Offset: 0x0000179D
		public virtual bool IsFalling()
		{
			return this._movementMode == Character.MovementMode.Falling;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000035A8 File Offset: 0x000017A8
		public virtual bool IsFlying()
		{
			return this._movementMode == Character.MovementMode.Flying;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000035B3 File Offset: 0x000017B3
		public virtual bool IsSwimming()
		{
			return this._movementMode == Character.MovementMode.Swimming;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000035C0 File Offset: 0x000017C0
		public virtual float GetMaxSpeed()
		{
			switch (this._movementMode)
			{
			case Character.MovementMode.Walking:
				if (!this.IsCrouched())
				{
					return this.maxWalkSpeed;
				}
				return this.maxWalkSpeedCrouched;
			case Character.MovementMode.Falling:
				return this.maxWalkSpeed;
			case Character.MovementMode.Flying:
				return this.maxFlySpeed;
			case Character.MovementMode.Swimming:
				return this.maxSwimSpeed;
			default:
				return 0f;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00003620 File Offset: 0x00001820
		public virtual float GetMinAnalogSpeed()
		{
			Character.MovementMode movementMode = this._movementMode;
			if (movementMode - Character.MovementMode.Walking <= 1)
			{
				return this.minAnalogWalkSpeed;
			}
			return 0f;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003646 File Offset: 0x00001846
		public virtual float GetMaxAcceleration()
		{
			if (this.IsFalling())
			{
				return this.maxAcceleration * this.airControl;
			}
			return this.maxAcceleration;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003664 File Offset: 0x00001864
		public virtual float GetMaxBrakingDeceleration()
		{
			switch (this._movementMode)
			{
			case Character.MovementMode.Walking:
				return this.brakingDecelerationWalking;
			case Character.MovementMode.Falling:
				if (!this.characterMovement.isOnGround)
				{
					return this.brakingDecelerationFalling;
				}
				return 0f;
			case Character.MovementMode.Flying:
				return this.brakingDecelerationFlying;
			case Character.MovementMode.Swimming:
				return this.brakingDecelerationSwimming;
			default:
				return 0f;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000036C8 File Offset: 0x000018C8
		protected virtual float ComputeAnalogInputModifier(Vector3 desiredVelocity)
		{
			float maxSpeed = this.GetMaxSpeed();
			if (desiredVelocity.sqrMagnitude > 0f && maxSpeed > 1E-08f)
			{
				return Mathf.Clamp01(desiredVelocity.magnitude / maxSpeed);
			}
			return 0f;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003708 File Offset: 0x00001908
		public virtual Vector3 ApplyVelocityBraking(Vector3 velocity, float friction, float maxBrakingDeceleration, float deltaTime)
		{
			if (velocity.isZero() || deltaTime < 1E-06f)
			{
				return velocity;
			}
			bool flag = friction == 0f;
			bool flag2 = maxBrakingDeceleration == 0f;
			if (flag && flag2)
			{
				return velocity;
			}
			Vector3 rhs = velocity;
			Vector3 b = flag2 ? Vector3.zero : (-maxBrakingDeceleration * velocity.normalized);
			float num = deltaTime;
			while (num >= 1E-06f)
			{
				float num2 = (num > 0.030303031f && !flag) ? Mathf.Min(0.030303031f, num * 0.5f) : num;
				num -= num2;
				velocity += (-friction * velocity + b) * num2;
				if (Vector3.Dot(velocity, rhs) <= 0f)
				{
					return Vector3.zero;
				}
			}
			float sqrMagnitude = velocity.sqrMagnitude;
			if (sqrMagnitude <= 1E-05f || (!flag2 && sqrMagnitude <= 0.1f))
			{
				return Vector3.zero;
			}
			return velocity;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000037EC File Offset: 0x000019EC
		public virtual Vector3 CalcVelocity(Vector3 velocity, Vector3 desiredVelocity, float friction, bool isFluid, float deltaTime)
		{
			if (deltaTime < 1E-06f)
			{
				return velocity;
			}
			float magnitude = desiredVelocity.magnitude;
			Vector3 a = (magnitude > 0f) ? (desiredVelocity / magnitude) : Vector3.zero;
			float num = this.ComputeAnalogInputModifier(desiredVelocity);
			Vector3 vector = this.GetMaxAcceleration() * num * a;
			float num2 = Mathf.Max(this.GetMinAnalogSpeed(), this.GetMaxSpeed() * num);
			bool flag = vector.isZero();
			bool flag2 = velocity.isExceeding(num2);
			if (flag || flag2)
			{
				Vector3 rhs = velocity;
				float friction2 = this.useSeparateBrakingFriction ? this.brakingFriction : friction;
				float maxBrakingDeceleration = this.useSeparateBrakingDeceleration ? this.brakingDeceleration : this.GetMaxBrakingDeceleration();
				velocity = this.ApplyVelocityBraking(velocity, friction2, maxBrakingDeceleration, deltaTime);
				if (flag2 && velocity.sqrMagnitude < num2.square() && Vector3.Dot(vector, rhs) > 0f)
				{
					velocity = rhs.normalized * num2;
				}
			}
			else
			{
				Vector3 normalized = vector.normalized;
				float magnitude2 = velocity.magnitude;
				velocity -= (velocity - normalized * magnitude2) * Mathf.Min(friction * deltaTime, 1f);
			}
			if (isFluid)
			{
				velocity *= 1f - Mathf.Min(friction * deltaTime, 1f);
			}
			if (!flag)
			{
				float maxLength = velocity.isExceeding(num2) ? velocity.magnitude : num2;
				velocity += vector * deltaTime;
				velocity = velocity.clampedTo(maxLength);
			}
			return velocity;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003968 File Offset: 0x00001B68
		public virtual Vector3 ConstrainInputVector(Vector3 inputVector)
		{
			Vector3 vector = -this.GetGravityDirection();
			if (!Mathf.Approximately(Vector3.Dot(inputVector, vector), 0f) && (this.IsWalking() || this.IsFalling()))
			{
				inputVector = Vector3.ProjectOnPlane(inputVector, vector);
			}
			return this.characterMovement.ConstrainVectorToPlane(inputVector);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000039BC File Offset: 0x00001BBC
		protected virtual void CalcDesiredVelocity(float deltaTime)
		{
			Vector3 a = Vector3.ClampMagnitude(this.GetMovementDirection(), 1f);
			Vector3 inputVector = (this.useRootMotion && this.rootMotionController) ? this.rootMotionController.ConsumeRootMotionVelocity(deltaTime) : (a * this.GetMaxSpeed());
			this._desiredVelocity = this.ConstrainInputVector(inputVector);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003A17 File Offset: 0x00001C17
		public virtual Vector3 GetDesiredVelocity()
		{
			return this._desiredVelocity;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003A20 File Offset: 0x00001C20
		public float GetSignedSlopeAngle()
		{
			Vector3 movementDirection = this.GetMovementDirection();
			if (movementDirection.isZero() || !this.IsOnGround())
			{
				return 0f;
			}
			return Mathf.Asin(Vector3.Dot(Vector3.ProjectOnPlane(movementDirection, this.characterMovement.groundNormal).normalized, -this.GetGravityDirection())) * 57.29578f;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00003A80 File Offset: 0x00001C80
		public virtual void ApplyDownwardsForce()
		{
			Rigidbody groundRigidbody = this.characterMovement.groundRigidbody;
			if (!groundRigidbody || groundRigidbody.isKinematic)
			{
				return;
			}
			Vector3 a = this.mass * this.GetGravityVector();
			groundRigidbody.AddForceAtPosition(a * this.standingDownwardForceScale, this.GetPosition());
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003AD4 File Offset: 0x00001CD4
		protected unsafe virtual void WalkingMovementMode(float deltaTime)
		{
			if (this.useRootMotion && this.rootMotionController)
			{
				*this.characterMovement.velocity = this.GetDesiredVelocity();
			}
			else
			{
				*this.characterMovement.velocity = this.CalcVelocity(*this.characterMovement.velocity, this.GetDesiredVelocity(), this.groundFriction, false, deltaTime);
			}
			if (this.applyStandingDownwardForce)
			{
				this.ApplyDownwardsForce();
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003B50 File Offset: 0x00001D50
		public virtual bool IsCrouched()
		{
			return this._isCrouched;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00003B58 File Offset: 0x00001D58
		public virtual void Crouch()
		{
			this.crouchInputPressed = true;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003B61 File Offset: 0x00001D61
		public virtual void UnCrouch()
		{
			this.crouchInputPressed = false;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00003B6A File Offset: 0x00001D6A
		protected virtual bool IsCrouchAllowed()
		{
			return this.canEverCrouch && this.IsWalking();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00003B7C File Offset: 0x00001D7C
		protected virtual bool CanUnCrouch()
		{
			return !this.characterMovement.CheckHeight(this._unCrouchedHeight);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00003B94 File Offset: 0x00001D94
		protected virtual void CheckCrouchInput()
		{
			if (!this._isCrouched && this.crouchInputPressed && this.IsCrouchAllowed())
			{
				this._isCrouched = true;
				this.characterMovement.SetHeight(this._crouchedHeight);
				this.OnCrouched();
				return;
			}
			if (this._isCrouched && (!this.crouchInputPressed || !this.IsCrouchAllowed()))
			{
				if (!this.CanUnCrouch())
				{
					return;
				}
				this._isCrouched = false;
				this.characterMovement.SetHeight(this._unCrouchedHeight);
				this.OnUnCrouched();
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00003C18 File Offset: 0x00001E18
		protected unsafe virtual void FallingMovementMode(float deltaTime)
		{
			Vector3 vector = this.GetDesiredVelocity();
			Vector3 vector2 = -this.GetGravityDirection();
			if (this.IsOnGround() && !this.IsOnWalkableGround())
			{
				Vector3 groundNormal = this.characterMovement.groundNormal;
				if (Vector3.Dot(vector, groundNormal) < 0f)
				{
					Vector3 normalized = Vector3.ProjectOnPlane(groundNormal, vector2).normalized;
					vector = Vector3.ProjectOnPlane(vector, normalized);
				}
				vector2 = Vector3.ProjectOnPlane(vector2, groundNormal).normalized;
			}
			Vector3 vector3 = Vector3.Project(*this.characterMovement.velocity, vector2);
			Vector3 vector4 = *this.characterMovement.velocity - vector3;
			vector4 = this.CalcVelocity(vector4, vector, this.fallingLateralFriction, false, deltaTime);
			vector3 += this.gravity * deltaTime;
			float maxFallSpeed = this.maxFallSpeed;
			if (this.physicsVolume)
			{
				maxFallSpeed = this.physicsVolume.maxFallSpeed;
			}
			if (Vector3.Dot(vector3, vector2) < -maxFallSpeed)
			{
				vector3 = Vector3.ClampMagnitude(vector3, maxFallSpeed);
			}
			*this.characterMovement.velocity = vector4 + vector3;
			this._fallingTime += deltaTime;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00003D40 File Offset: 0x00001F40
		public virtual bool IsJumping()
		{
			return this._isJumping;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00003D48 File Offset: 0x00001F48
		public virtual void Jump()
		{
			this.jumpInputPressed = true;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003D51 File Offset: 0x00001F51
		public virtual void StopJumping()
		{
			this.jumpInputPressed = false;
			this.jumpInputHoldTime = 0f;
			this.ResetJumpState();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003D6B File Offset: 0x00001F6B
		protected virtual void ResetJumpState()
		{
			if (!this.IsFalling())
			{
				this.jumpCurrentCount = 0;
			}
			this.jumpForceTimeRemaining = 0f;
			this._isJumping = false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003D8E File Offset: 0x00001F8E
		public virtual bool IsJumpProvidingForce()
		{
			return this.jumpForceTimeRemaining > 0f;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003DA0 File Offset: 0x00001FA0
		public virtual float GetMaxJumpHeight()
		{
			float gravityMagnitude = this.GetGravityMagnitude();
			if (gravityMagnitude > 0.0001f)
			{
				return this.jumpImpulse * this.jumpImpulse / (2f * gravityMagnitude);
			}
			return 0f;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003DD7 File Offset: 0x00001FD7
		public virtual float GetMaxJumpHeightWithJumpTime()
		{
			return this.GetMaxJumpHeight() + this.jumpImpulse * this.jumpMaxHoldTime;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003DED File Offset: 0x00001FED
		protected virtual bool IsJumpAllowed()
		{
			return (this.canJumpWhileCrouching || !this.IsCrouched()) && this.canEverJump && (this.IsWalking() || this.IsFalling());
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00003E1C File Offset: 0x0000201C
		protected virtual bool CanJump()
		{
			bool flag = this.IsJumpAllowed();
			if (flag)
			{
				if (!this._isJumping || this.jumpMaxHoldTime <= 0f)
				{
					if (this.jumpCurrentCount == 0)
					{
						flag = (this.jumpInputHoldTime <= this.jumpMaxPreGroundedTime);
						if (flag)
						{
							this.jumpInputHoldTime = 0f;
						}
					}
					else
					{
						flag = (this.jumpCurrentCount < this.jumpMaxCount && this.jumpInputHoldTime == 0f);
					}
				}
				else
				{
					flag = (this.jumpInputPressed && this.jumpInputHoldTime < this.jumpMaxHoldTime && (this.jumpCurrentCount < this.jumpMaxCount || (this._isJumping && this.jumpCurrentCount == this.jumpMaxCount)));
				}
			}
			return flag;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00003EE0 File Offset: 0x000020E0
		protected unsafe virtual bool DoJump()
		{
			Vector3 vector = -this.GetGravityDirection();
			if (this.characterMovement.isConstrainedToPlane && Mathf.Approximately(Vector3.Dot(this.characterMovement.GetPlaneConstraintNormal(), vector), 1f))
			{
				return false;
			}
			float d = Mathf.Max(Vector3.Dot(*this.characterMovement.velocity, vector), this.jumpImpulse);
			*this.characterMovement.velocity = Vector3.ProjectOnPlane(*this.characterMovement.velocity, vector) + vector * d;
			return true;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003F7C File Offset: 0x0000217C
		protected virtual void CheckJumpInput()
		{
			if (!this.jumpInputPressed)
			{
				return;
			}
			if (this.jumpCurrentCount == 0 && this.IsFalling() && this.fallingTime > this.jumpMaxPostGroundedTime)
			{
				int jumpCurrentCount = this.jumpCurrentCount;
				this.jumpCurrentCount = jumpCurrentCount + 1;
			}
			bool flag = this.CanJump() && this.DoJump();
			if (flag && !this._isJumping)
			{
				int jumpCurrentCount = this.jumpCurrentCount;
				this.jumpCurrentCount = jumpCurrentCount + 1;
				this.jumpForceTimeRemaining = this.jumpMaxHoldTime;
				this.characterMovement.PauseGroundConstraint(0.1f);
				this.SetMovementMode(Character.MovementMode.Falling, 0);
				this.OnJumped();
			}
			this._isJumping = flag;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004020 File Offset: 0x00002220
		protected virtual void UpdateJumpTimers(float deltaTime)
		{
			if (this.jumpInputPressed)
			{
				this.jumpInputHoldTime += deltaTime;
			}
			if (this.jumpForceTimeRemaining > 0f)
			{
				this.jumpForceTimeRemaining -= deltaTime;
				if (this.jumpForceTimeRemaining <= 0f)
				{
					this.ResetJumpState();
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004071 File Offset: 0x00002271
		protected virtual void NotifyJumpApex()
		{
			if (!this.notifyJumpApex)
			{
				return;
			}
			if (Vector3.Dot(this.GetVelocity(), -this.GetGravityDirection()) >= 0f)
			{
				return;
			}
			this.notifyJumpApex = false;
			this.OnReachedJumpApex();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000040A8 File Offset: 0x000022A8
		protected unsafe virtual void FlyingMovementMode(float deltaTime)
		{
			if (this.useRootMotion && this.rootMotionController)
			{
				*this.characterMovement.velocity = this.GetDesiredVelocity();
				return;
			}
			float friction = this.IsInWaterPhysicsVolume() ? this.physicsVolume.friction : this.flyingFriction;
			*this.characterMovement.velocity = this.CalcVelocity(*this.characterMovement.velocity, this.GetDesiredVelocity(), friction, true, deltaTime);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000412C File Offset: 0x0000232C
		public virtual float CalcImmersionDepth()
		{
			float result = 0f;
			if (this.IsInWaterPhysicsVolume())
			{
				float height = this.characterMovement.height;
				if (height == 0f || this.buoyancy == 0f)
				{
					result = 1f;
				}
				else
				{
					Vector3 a = -this.GetGravityDirection();
					Vector3 origin = this.GetPosition() + a * height;
					Vector3 direction = -a;
					RaycastHit raycastHit;
					result = ((!this.physicsVolume.boxCollider.Raycast(new Ray(origin, direction), out raycastHit, height)) ? 1f : (1f - Mathf.InverseLerp(0f, height, raycastHit.distance)));
				}
			}
			return result;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000041D8 File Offset: 0x000023D8
		protected unsafe virtual void SwimmingMovementMode(float deltaTime)
		{
			float num = this.CalcImmersionDepth();
			float num2 = this.buoyancy * num;
			Vector3 vector = this.GetDesiredVelocity();
			Vector3 vector2 = *this.characterMovement.velocity;
			Vector3 vector3 = -this.GetGravityDirection();
			float num3 = Vector3.Dot(vector2, vector3);
			if (num3 > this.maxSwimSpeed * 0.33f && num2 > 0f)
			{
				num3 = Mathf.Max(this.maxSwimSpeed * 0.33f, num3 * num * num);
				vector2 = Vector3.ProjectOnPlane(vector2, vector3) + vector3 * num3;
			}
			else if (num < 0.65f)
			{
				float b = Vector3.Dot(vector, vector3);
				vector = Vector3.ProjectOnPlane(vector, vector3) + vector3 * Mathf.Min(0.1f, b);
			}
			if (this.useRootMotion && this.rootMotionController)
			{
				Vector3 b2 = Vector3.Project(vector2, vector3);
				vector2 = Vector3.ProjectOnPlane(vector, vector3) + b2;
			}
			else
			{
				float friction = this.IsInWaterPhysicsVolume() ? (this.physicsVolume.friction * num) : (this.swimmingFriction * num);
				vector2 = this.CalcVelocity(vector2, vector, friction, true, deltaTime);
			}
			vector2 += this.gravity * ((1f - num2) * deltaTime);
			*this.characterMovement.velocity = vector2;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000432C File Offset: 0x0000252C
		protected virtual void CustomMovementMode(float deltaTime)
		{
			this.OnCustomMovementMode(deltaTime);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004335 File Offset: 0x00002535
		public Character.RotationMode GetRotationMode()
		{
			return this._rotationMode;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000433D File Offset: 0x0000253D
		public void SetRotationMode(Character.RotationMode rotationMode)
		{
			this._rotationMode = rotationMode;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004348 File Offset: 0x00002548
		protected virtual void UpdateRotation(float deltaTime)
		{
			if (this._rotationMode != Character.RotationMode.None)
			{
				if (this._rotationMode == Character.RotationMode.OrientRotationToMovement)
				{
					bool updateYawOnly = this.IsWalking() || this.IsFalling();
					this.RotateTowards(this._movementDirection, deltaTime, updateYawOnly);
					return;
				}
				if (this._rotationMode == Character.RotationMode.OrientRotationToViewDirection && this.camera != null)
				{
					bool updateYawOnly2 = this.IsWalking() || this.IsFalling();
					this.RotateTowards(this.cameraTransform.forward, deltaTime, updateYawOnly2);
					return;
				}
				if (this._rotationMode == Character.RotationMode.OrientWithRootMotion)
				{
					this.RotateWithRootMotion();
					return;
				}
				if (this._rotationMode == Character.RotationMode.Custom)
				{
					this.CustomRotationMode(deltaTime);
				}
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000043E7 File Offset: 0x000025E7
		protected virtual void CustomRotationMode(float deltaTime)
		{
			this.OnCustomRotationMode(deltaTime);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000043F0 File Offset: 0x000025F0
		private void BeforeSimulationUpdate(float deltaTime)
		{
			if (this.IsWalking() && !this.IsGrounded())
			{
				this.SetMovementMode(Character.MovementMode.Falling, 0);
			}
			if (this.IsFalling() && this.IsGrounded())
			{
				this.SetMovementMode(Character.MovementMode.Walking, 0);
			}
			this.UpdatePhysicsVolumes();
			this.CheckCrouchInput();
			this.CheckJumpInput();
			this.UpdateJumpTimers(deltaTime);
			this.OnBeforeSimulationUpdate(deltaTime);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004450 File Offset: 0x00002650
		private void SimulationUpdate(float deltaTime)
		{
			this.CalcDesiredVelocity(deltaTime);
			switch (this._movementMode)
			{
			case Character.MovementMode.Walking:
				this.WalkingMovementMode(deltaTime);
				break;
			case Character.MovementMode.Falling:
				this.FallingMovementMode(deltaTime);
				break;
			case Character.MovementMode.Flying:
				this.FlyingMovementMode(deltaTime);
				break;
			case Character.MovementMode.Swimming:
				this.SwimmingMovementMode(deltaTime);
				break;
			case Character.MovementMode.Custom:
				this.CustomMovementMode(deltaTime);
				break;
			}
			this.UpdateRotation(deltaTime);
			this.ConsumeRotationInput();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000044C3 File Offset: 0x000026C3
		private void AfterSimulationUpdate(float deltaTime)
		{
			this.NotifyJumpApex();
			this.OnAfterSimulationUpdate(deltaTime);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000044D2 File Offset: 0x000026D2
		private void CharacterMovementUpdate(float deltaTime)
		{
			this.characterMovement.Move(deltaTime);
			this.OnCharacterMovementUpdated(deltaTime);
			if (!this.useRootMotion && this.rootMotionController)
			{
				this.rootMotionController.FlushAccumulatedDeltas();
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004508 File Offset: 0x00002708
		public void Simulate(float deltaTime)
		{
			if (this.isPaused)
			{
				return;
			}
			this.BeforeSimulationUpdate(deltaTime);
			this.SimulationUpdate(deltaTime);
			this.AfterSimulationUpdate(deltaTime);
			this.CharacterMovementUpdate(deltaTime);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000452F File Offset: 0x0000272F
		private void OnLateFixedUpdate()
		{
			this.Simulate(Time.deltaTime);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000453C File Offset: 0x0000273C
		public bool IsPaused()
		{
			return this.isPaused;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004544 File Offset: 0x00002744
		public unsafe void Pause(bool pause, bool clearState = true)
		{
			this.isPaused = pause;
			this.characterMovement.collider.enabled = !this.isPaused;
			if (clearState)
			{
				this._movementDirection = Vector3.zero;
				this._rotationInput = Vector3.zero;
				*this.characterMovement.velocity = Vector3.zero;
				this.characterMovement.ClearAccumulatedForces();
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000045AC File Offset: 0x000027AC
		protected virtual void Reset()
		{
			this._rotationMode = Character.RotationMode.OrientRotationToMovement;
			this._rotationRate = 540f;
			this._startingMovementMode = Character.MovementMode.Walking;
			this._maxWalkSpeed = 5f;
			this._minAnalogWalkSpeed = 0f;
			this._maxAcceleration = 20f;
			this._brakingDecelerationWalking = 20f;
			this._groundFriction = 8f;
			this._canEverCrouch = true;
			this._crouchedHeight = 1.25f;
			this._unCrouchedHeight = 2f;
			this._maxWalkSpeedCrouched = 3f;
			this._maxFallSpeed = 40f;
			this._brakingDecelerationFalling = 0f;
			this._fallingLateralFriction = 0.3f;
			this._airControl = 0.3f;
			this._canEverJump = true;
			this._canJumpWhileCrouching = true;
			this._jumpMaxCount = 1;
			this._jumpImpulse = 5f;
			this._jumpMaxHoldTime = 0f;
			this._jumpMaxPreGroundedTime = 0f;
			this._jumpMaxPostGroundedTime = 0f;
			this._maxFlySpeed = 10f;
			this._brakingDecelerationFlying = 0f;
			this._flyingFriction = 1f;
			this._maxSwimSpeed = 3f;
			this._brakingDecelerationSwimming = 0f;
			this._swimmingFriction = 0f;
			this._buoyancy = 1f;
			this._gravity = new Vector3(0f, -9.81f, 0f);
			this._gravityScale = 1f;
			this._useRootMotion = false;
			this._impartPlatformVelocity = false;
			this._impartPlatformMovement = false;
			this._impartPlatformRotation = false;
			this._enablePhysicsInteraction = false;
			this._applyPushForceToCharacters = false;
			this._applyStandingDownwardForce = false;
			this._mass = 1f;
			this._pushForceScale = 1f;
			this._standingDownwardForceScale = 1f;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004764 File Offset: 0x00002964
		protected virtual void OnValidate()
		{
			this.rotationRate = this._rotationRate;
			this.maxWalkSpeed = this._maxWalkSpeed;
			this.minAnalogWalkSpeed = this._minAnalogWalkSpeed;
			this.maxAcceleration = this._maxAcceleration;
			this.brakingDecelerationWalking = this._brakingDecelerationWalking;
			this.groundFriction = this._groundFriction;
			this.crouchedHeight = this._crouchedHeight;
			this.unCrouchedHeight = this._unCrouchedHeight;
			this.maxWalkSpeedCrouched = this._maxWalkSpeedCrouched;
			this.maxFallSpeed = this._maxFallSpeed;
			this.brakingDecelerationFalling = this._brakingDecelerationFalling;
			this.fallingLateralFriction = this._fallingLateralFriction;
			this.airControl = this._airControl;
			this.jumpMaxCount = this._jumpMaxCount;
			this.jumpImpulse = this._jumpImpulse;
			this.jumpMaxHoldTime = this._jumpMaxHoldTime;
			this.jumpMaxPreGroundedTime = this._jumpMaxPreGroundedTime;
			this.jumpMaxPostGroundedTime = this._jumpMaxPostGroundedTime;
			this.maxFlySpeed = this._maxFlySpeed;
			this.brakingDecelerationFlying = this._brakingDecelerationFlying;
			this.flyingFriction = this._flyingFriction;
			this.maxSwimSpeed = this._maxSwimSpeed;
			this.brakingDecelerationSwimming = this._brakingDecelerationSwimming;
			this.swimmingFriction = this._swimmingFriction;
			this.buoyancy = this._buoyancy;
			this.gravityScale = this._gravityScale;
			this.useRootMotion = this._useRootMotion;
			if (this._characterMovement == null)
			{
				this._characterMovement = base.GetComponent<CharacterMovement>();
			}
			this.impartPlatformVelocity = this._impartPlatformVelocity;
			this.impartPlatformMovement = this._impartPlatformMovement;
			this.impartPlatformRotation = this._impartPlatformRotation;
			this.enablePhysicsInteraction = this._enablePhysicsInteraction;
			this.applyPushForceToCharacters = this._applyPushForceToCharacters;
			this.applyPushForceToCharacters = this._applyPushForceToCharacters;
			this.mass = this._mass;
			this.pushForceScale = this._pushForceScale;
			this.standingDownwardForceScale = this._standingDownwardForceScale;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000493B File Offset: 0x00002B3B
		protected virtual void Awake()
		{
			this.CacheComponents();
			this.SetMovementMode(this._startingMovementMode, 0);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004950 File Offset: 0x00002B50
		protected virtual void OnEnable()
		{
			this.characterMovement.Collided += this.OnCollided;
			this.characterMovement.FoundGround += this.OnFoundGround;
			if (this._enableAutoSimulation)
			{
				this.EnableAutoSimulationCoroutine(true);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000499C File Offset: 0x00002B9C
		protected virtual void OnDisable()
		{
			this.characterMovement.Collided -= this.OnCollided;
			this.characterMovement.FoundGround -= this.OnFoundGround;
			if (this._enableAutoSimulation)
			{
				this.EnableAutoSimulationCoroutine(false);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000049E8 File Offset: 0x00002BE8
		protected virtual void Start()
		{
			if (this._startingMovementMode == Character.MovementMode.Walking)
			{
				this.characterMovement.SetPosition(this.transform.position, true);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004A0A File Offset: 0x00002C0A
		protected virtual void OnTriggerEnter(Collider other)
		{
			this.AddPhysicsVolume(other);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004A13 File Offset: 0x00002C13
		protected virtual void OnTriggerExit(Collider other)
		{
			this.RemovePhysicsVolume(other);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004A1C File Offset: 0x00002C1C
		private IEnumerator LateFixedUpdate()
		{
			WaitForFixedUpdate waitTime = new WaitForFixedUpdate();
			for (;;)
			{
				yield return waitTime;
				this.OnLateFixedUpdate();
			}
			yield break;
		}

		// Token: 0x04000001 RID: 1
		[Space(15f)]
		[Tooltip("The Character's current rotation mode.")]
		[SerializeField]
		private Character.RotationMode _rotationMode;

		// Token: 0x04000002 RID: 2
		[Tooltip("Change in rotation per second (Deg / s).\nUsed when rotation mode is OrientRotationToMovement or OrientRotationToViewDirection.")]
		[SerializeField]
		private float _rotationRate;

		// Token: 0x04000003 RID: 3
		[Space(15f)]
		[Tooltip("The Character's default movement mode. Used at player startup.")]
		[SerializeField]
		private Character.MovementMode _startingMovementMode;

		// Token: 0x04000004 RID: 4
		[Space(15f)]
		[Tooltip("The maximum ground speed when walking.\nAlso determines maximum lateral speed when falling.")]
		[SerializeField]
		private float _maxWalkSpeed;

		// Token: 0x04000005 RID: 5
		[Tooltip("The ground speed that we should accelerate up to when walking at minimum analog stick tilt.")]
		[SerializeField]
		private float _minAnalogWalkSpeed;

		// Token: 0x04000006 RID: 6
		[Tooltip("Max Acceleration (rate of change of velocity).")]
		[SerializeField]
		private float _maxAcceleration;

		// Token: 0x04000007 RID: 7
		[Tooltip("Deceleration when walking and not applying acceleration.\nThis is a constant opposing force that directly lowers velocity by a constant value.")]
		[SerializeField]
		private float _brakingDecelerationWalking;

		// Token: 0x04000008 RID: 8
		[Tooltip("Setting that affects movement control.\nHigher values allow faster changes in direction.\nIf useSeparateBrakingFriction is false, also affects the ability to stop more quickly when braking (whenever acceleration is zero).")]
		[SerializeField]
		private float _groundFriction;

		// Token: 0x04000009 RID: 9
		[Space(15f)]
		[Tooltip("Is the character able to crouch ?")]
		[SerializeField]
		private bool _canEverCrouch;

		// Token: 0x0400000A RID: 10
		[Tooltip("If canEverCrouch == true, determines the character height when crouched.")]
		[SerializeField]
		private float _crouchedHeight;

		// Token: 0x0400000B RID: 11
		[Tooltip("If canEverCrouch == true, determines the character height when un crouched.")]
		[SerializeField]
		private float _unCrouchedHeight;

		// Token: 0x0400000C RID: 12
		[Tooltip("The maximum ground speed while crouched.")]
		[SerializeField]
		private float _maxWalkSpeedCrouched;

		// Token: 0x0400000D RID: 13
		[Space(15f)]
		[Tooltip("The maximum vertical velocity a Character can reach when falling. Eg: Terminal velocity.")]
		[SerializeField]
		private float _maxFallSpeed;

		// Token: 0x0400000E RID: 14
		[Tooltip("Lateral deceleration when falling and not applying acceleration.")]
		[SerializeField]
		private float _brakingDecelerationFalling;

		// Token: 0x0400000F RID: 15
		[Tooltip("Friction to apply to lateral movement when falling. \nIf useSeparateBrakingFriction is false, also affects the ability to stop more quickly when braking (whenever acceleration is zero).")]
		[SerializeField]
		private float _fallingLateralFriction;

		// Token: 0x04000010 RID: 16
		[Range(0f, 1f)]
		[Tooltip("When falling, amount of lateral movement control available to the Character.\n0 = no control, 1 = full control at max acceleration.")]
		[SerializeField]
		private float _airControl;

		// Token: 0x04000011 RID: 17
		[Space(15f)]
		[Tooltip("Is the character able to jump ?")]
		[SerializeField]
		private bool _canEverJump;

		// Token: 0x04000012 RID: 18
		[Tooltip("Can jump while crouching ?")]
		[SerializeField]
		private bool _canJumpWhileCrouching;

		// Token: 0x04000013 RID: 19
		[Tooltip("The max number of jumps the Character can perform.")]
		[SerializeField]
		private int _jumpMaxCount;

		// Token: 0x04000014 RID: 20
		[Tooltip("Initial velocity (instantaneous vertical velocity) when jumping.")]
		[SerializeField]
		private float _jumpImpulse;

		// Token: 0x04000015 RID: 21
		[Tooltip("The maximum time (in seconds) to hold the jump. eg: Variable height jump.")]
		[SerializeField]
		private float _jumpMaxHoldTime;

		// Token: 0x04000016 RID: 22
		[Tooltip("How early before hitting the ground you can trigger a jump (in seconds).")]
		[SerializeField]
		private float _jumpMaxPreGroundedTime;

		// Token: 0x04000017 RID: 23
		[Tooltip("How long after leaving the ground you can trigger a jump (in seconds).")]
		[SerializeField]
		private float _jumpMaxPostGroundedTime;

		// Token: 0x04000018 RID: 24
		[Space(15f)]
		[Tooltip("The maximum flying speed.")]
		[SerializeField]
		private float _maxFlySpeed;

		// Token: 0x04000019 RID: 25
		[Tooltip("Deceleration when flying and not applying acceleration.")]
		[SerializeField]
		private float _brakingDecelerationFlying;

		// Token: 0x0400001A RID: 26
		[Tooltip("Friction to apply to movement when flying.")]
		[SerializeField]
		private float _flyingFriction;

		// Token: 0x0400001B RID: 27
		[Space(15f)]
		[Tooltip("The maximum swimming speed.")]
		[SerializeField]
		private float _maxSwimSpeed;

		// Token: 0x0400001C RID: 28
		[Tooltip("Deceleration when swimming and not applying acceleration.")]
		[SerializeField]
		private float _brakingDecelerationSwimming;

		// Token: 0x0400001D RID: 29
		[Tooltip("Friction to apply to movement when swimming.")]
		[SerializeField]
		private float _swimmingFriction;

		// Token: 0x0400001E RID: 30
		[Tooltip("Water buoyancy ratio. 1 = Neutral Buoyancy, 0 = No Buoyancy.")]
		[SerializeField]
		private float _buoyancy;

		// Token: 0x0400001F RID: 31
		[Tooltip("This Character's gravity.")]
		[Space(15f)]
		[SerializeField]
		private Vector3 _gravity;

		// Token: 0x04000020 RID: 32
		[Tooltip("The degree to which this object is affected by gravity.\nCan be negative allowing to change gravity direction.")]
		[SerializeField]
		private float _gravityScale;

		// Token: 0x04000021 RID: 33
		[Space(15f)]
		[Tooltip("Should animation determines the Character's movement ?")]
		[SerializeField]
		private bool _useRootMotion;

		// Token: 0x04000022 RID: 34
		[Space(15f)]
		[Tooltip("Whether the Character moves with the moving platform it is standing on.")]
		[SerializeField]
		private bool _impartPlatformMovement;

		// Token: 0x04000023 RID: 35
		[Tooltip("Whether the Character receives the changes in rotation of the platform it is standing on.")]
		[SerializeField]
		private bool _impartPlatformRotation;

		// Token: 0x04000024 RID: 36
		[Tooltip("If true, impart the platform's velocity when jumping or falling off it.")]
		[SerializeField]
		private bool _impartPlatformVelocity;

		// Token: 0x04000025 RID: 37
		[Space(15f)]
		[Tooltip("If enabled, the player will interact with dynamic rigidbodies when walking into them.")]
		[SerializeField]
		private bool _enablePhysicsInteraction;

		// Token: 0x04000026 RID: 38
		[Tooltip("Should apply push force to characters when walking into them ?")]
		[SerializeField]
		private bool _applyPushForceToCharacters;

		// Token: 0x04000027 RID: 39
		[Tooltip("Should apply a downward force to rigidbodies we stand on ?")]
		[SerializeField]
		private bool _applyStandingDownwardForce;

		// Token: 0x04000028 RID: 40
		[Space(15f)]
		[Tooltip("This Character's mass (in Kg).Determines how the character interact against other characters or dynamic rigidbodies if enablePhysicsInteraction == true.")]
		[SerializeField]
		private float _mass;

		// Token: 0x04000029 RID: 41
		[Tooltip("Force applied to rigidbodies when walking into them (due to mass and relative velocity) is scaled by this amount.")]
		[SerializeField]
		private float _pushForceScale;

		// Token: 0x0400002A RID: 42
		[Tooltip("Force applied to rigidbodies we stand on (due to mass and gravity) is scaled by this amount.")]
		[SerializeField]
		private float _standingDownwardForceScale;

		// Token: 0x0400002B RID: 43
		[Space(15f)]
		[Tooltip("Reference to the Player's Camera.\nIf assigned, the Character's movement will be relative to this camera, otherwise movement will be relative to world axis.")]
		[SerializeField]
		private Camera _camera;

		// Token: 0x0400002C RID: 44
		protected readonly List<PhysicsVolume> _physicsVolumes = new List<PhysicsVolume>();

		// Token: 0x0400002D RID: 45
		private Coroutine _lateFixedUpdateCoroutine;

		// Token: 0x0400002E RID: 46
		private bool _enableAutoSimulation = true;

		// Token: 0x0400002F RID: 47
		private Transform _transform;

		// Token: 0x04000030 RID: 48
		private CharacterMovement _characterMovement;

		// Token: 0x04000031 RID: 49
		private Animator _animator;

		// Token: 0x04000032 RID: 50
		private RootMotionController _rootMotionController;

		// Token: 0x04000033 RID: 51
		private Transform _cameraTransform;

		// Token: 0x04000034 RID: 52
		private Character.MovementMode _movementMode;

		// Token: 0x04000035 RID: 53
		private int _customMovementMode;

		// Token: 0x04000036 RID: 54
		private bool _useSeparateBrakingFriction;

		// Token: 0x04000037 RID: 55
		private float _brakingFriction;

		// Token: 0x04000038 RID: 56
		private bool _useSeparateBrakingDeceleration;

		// Token: 0x04000039 RID: 57
		private float _brakingDeceleration;

		// Token: 0x0400003A RID: 58
		private Vector3 _movementDirection = Vector3.zero;

		// Token: 0x0400003B RID: 59
		private Vector3 _rotationInput = Vector3.zero;

		// Token: 0x0400003C RID: 60
		private Vector3 _desiredVelocity = Vector3.zero;

		// Token: 0x0400003D RID: 61
		protected bool _isCrouched;

		// Token: 0x0400003E RID: 62
		protected bool _isJumping;

		// Token: 0x0400003F RID: 63
		private float _jumpInputHoldTime;

		// Token: 0x04000040 RID: 64
		private float _jumpForceTimeRemaining;

		// Token: 0x04000041 RID: 65
		private int _jumpCurrentCount;

		// Token: 0x04000042 RID: 66
		protected float _fallingTime;

		// Token: 0x02000016 RID: 22
		public enum MovementMode
		{
			// Token: 0x040000F1 RID: 241
			None,
			// Token: 0x040000F2 RID: 242
			Walking,
			// Token: 0x040000F3 RID: 243
			Falling,
			// Token: 0x040000F4 RID: 244
			Flying,
			// Token: 0x040000F5 RID: 245
			Swimming,
			// Token: 0x040000F6 RID: 246
			Custom
		}

		// Token: 0x02000017 RID: 23
		public enum RotationMode
		{
			// Token: 0x040000F8 RID: 248
			None,
			// Token: 0x040000F9 RID: 249
			OrientRotationToMovement,
			// Token: 0x040000FA RID: 250
			OrientRotationToViewDirection,
			// Token: 0x040000FB RID: 251
			OrientWithRootMotion,
			// Token: 0x040000FC RID: 252
			Custom
		}

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x0600026C RID: 620
		public delegate void PhysicsVolumeChangedEventHandler(PhysicsVolume newPhysicsVolume);

		// Token: 0x02000019 RID: 25
		// (Invoke) Token: 0x06000270 RID: 624
		public delegate void MovementModeChangedEventHandler(Character.MovementMode prevMovementMode, int prevCustomMode);

		// Token: 0x0200001A RID: 26
		// (Invoke) Token: 0x06000274 RID: 628
		public delegate void CustomMovementModeUpdateEventHandler(float deltaTime);

		// Token: 0x0200001B RID: 27
		// (Invoke) Token: 0x06000278 RID: 632
		public delegate void CustomRotationModeUpdateEventHandler(float deltaTime);

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x0600027C RID: 636
		public delegate void BeforeSimulationUpdateEventHandler(float deltaTime);

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x06000280 RID: 640
		public delegate void AfterSimulationUpdateEventHandler(float deltaTime);

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x06000284 RID: 644
		public delegate void CharacterMovementUpdateEventHandler(float deltaTime);

		// Token: 0x0200001F RID: 31
		// (Invoke) Token: 0x06000288 RID: 648
		public delegate void CollidedEventHandler(ref CollisionResult collisionResult);

		// Token: 0x02000020 RID: 32
		// (Invoke) Token: 0x0600028C RID: 652
		public delegate void FoundGroundEventHandler(ref FindGroundResult foundGround);

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x06000290 RID: 656
		public delegate void LandedEventHandled(Vector3 landingVelocity);

		// Token: 0x02000022 RID: 34
		// (Invoke) Token: 0x06000294 RID: 660
		public delegate void CrouchedEventHandler();

		// Token: 0x02000023 RID: 35
		// (Invoke) Token: 0x06000298 RID: 664
		public delegate void UnCrouchedEventHandler();

		// Token: 0x02000024 RID: 36
		// (Invoke) Token: 0x0600029C RID: 668
		public delegate void JumpedEventHandler();

		// Token: 0x02000025 RID: 37
		// (Invoke) Token: 0x060002A0 RID: 672
		public delegate void ReachedJumpApexEventHandler();
	}
}
