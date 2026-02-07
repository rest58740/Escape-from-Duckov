using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECM2
{
	// Token: 0x0200000B RID: 11
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
	public sealed class CharacterMovement : MonoBehaviour
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00005133 File Offset: 0x00003333
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00005140 File Offset: 0x00003340
		public bool AllowPushCharacters
		{
			get
			{
				return this._advanced.allowPushCharacters;
			}
			set
			{
				if (value)
				{
					this.collisionLayers |= 1 << LayerMask.NameToLayer("Character");
					this._advanced.enablePhysicsInteraction = true;
					this._advanced.allowPushCharacters = true;
					return;
				}
				this.collisionLayers &= ~(1 << LayerMask.NameToLayer("Character"));
				this._advanced.enablePhysicsInteraction = false;
				this._advanced.allowPushCharacters = false;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000051CE File Offset: 0x000033CE
		public new Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000051D6 File Offset: 0x000033D6
		public Rigidbody rigidbody
		{
			get
			{
				return this._rigidbody;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000051DE File Offset: 0x000033DE
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000051EB File Offset: 0x000033EB
		public RigidbodyInterpolation interpolation
		{
			get
			{
				return this.rigidbody.interpolation;
			}
			set
			{
				this.rigidbody.interpolation = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000051F9 File Offset: 0x000033F9
		public Collider collider
		{
			get
			{
				return this._capsuleCollider;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005201 File Offset: 0x00003401
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00005209 File Offset: 0x00003409
		public Transform rootTransform
		{
			get
			{
				return this._rootTransform;
			}
			set
			{
				this._rootTransform = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00005212 File Offset: 0x00003412
		// (set) Token: 0x06000157 RID: 343 RVA: 0x0000521A File Offset: 0x0000341A
		public Vector3 rootTransformOffset
		{
			get
			{
				return this._rootTransformOffset;
			}
			set
			{
				this._rootTransformOffset = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00005223 File Offset: 0x00003423
		// (set) Token: 0x06000159 RID: 345 RVA: 0x0000522B File Offset: 0x0000342B
		public Vector3 position
		{
			get
			{
				return this.GetPosition();
			}
			set
			{
				this.SetPosition(value, false);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00005235 File Offset: 0x00003435
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000523D File Offset: 0x0000343D
		public Quaternion rotation
		{
			get
			{
				return this.GetRotation();
			}
			set
			{
				this.SetRotation(value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00005246 File Offset: 0x00003446
		public Vector3 worldCenter
		{
			get
			{
				return this.position + this.rotation * this._capsuleCenter;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005264 File Offset: 0x00003464
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000526C File Offset: 0x0000346C
		public Vector3 updatedPosition { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005275 File Offset: 0x00003475
		// (set) Token: 0x06000160 RID: 352 RVA: 0x0000527D File Offset: 0x0000347D
		public Quaternion updatedRotation { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005286 File Offset: 0x00003486
		public ref Vector3 velocity
		{
			get
			{
				return ref this._velocity;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000528E File Offset: 0x0000348E
		public float speed
		{
			get
			{
				return this._velocity.magnitude;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000529B File Offset: 0x0000349B
		public float forwardSpeed
		{
			get
			{
				return this._velocity.dot(this.transform.forward);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000052B3 File Offset: 0x000034B3
		public float sidewaysSpeed
		{
			get
			{
				return this._velocity.dot(this.transform.right);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000052CB File Offset: 0x000034CB
		// (set) Token: 0x06000166 RID: 358 RVA: 0x000052D3 File Offset: 0x000034D3
		public float radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this.SetDimensions(value, this._height);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000052E2 File Offset: 0x000034E2
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000052EA File Offset: 0x000034EA
		public float height
		{
			get
			{
				return this._height;
			}
			set
			{
				this.SetDimensions(this._radius, value);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000052F9 File Offset: 0x000034F9
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00005301 File Offset: 0x00003501
		public float slopeLimit
		{
			get
			{
				return this._slopeLimit;
			}
			set
			{
				this._slopeLimit = Mathf.Clamp(value, 0f, 89f);
				this._minSlopeLimit = Mathf.Cos((this._slopeLimit + 0.01f) * 0.017453292f);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00005336 File Offset: 0x00003536
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000533E File Offset: 0x0000353E
		public float stepOffset
		{
			get
			{
				return this._stepOffset;
			}
			set
			{
				this._stepOffset = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00005351 File Offset: 0x00003551
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00005359 File Offset: 0x00003559
		public float perchOffset
		{
			get
			{
				return this._perchOffset;
			}
			set
			{
				this._perchOffset = Mathf.Clamp(value, 0f, this._radius);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00005372 File Offset: 0x00003572
		// (set) Token: 0x06000170 RID: 368 RVA: 0x0000537A File Offset: 0x0000357A
		public float perchAdditionalHeight
		{
			get
			{
				return this._perchAdditionalHeight;
			}
			set
			{
				this._perchAdditionalHeight = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000538D File Offset: 0x0000358D
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00005395 File Offset: 0x00003595
		public bool slopeLimitOverride
		{
			get
			{
				return this._slopeLimitOverride;
			}
			set
			{
				this._slopeLimitOverride = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000539E File Offset: 0x0000359E
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000053A6 File Offset: 0x000035A6
		public bool useFlatTop
		{
			get
			{
				return this._useFlatTop;
			}
			set
			{
				this._useFlatTop = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000053AF File Offset: 0x000035AF
		// (set) Token: 0x06000176 RID: 374 RVA: 0x000053B7 File Offset: 0x000035B7
		public bool useFlatBaseForGroundChecks
		{
			get
			{
				return this._useFlatBaseForGroundChecks;
			}
			set
			{
				this._useFlatBaseForGroundChecks = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000053C0 File Offset: 0x000035C0
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000053C8 File Offset: 0x000035C8
		public LayerMask collisionLayers
		{
			get
			{
				return this._collisionLayers;
			}
			set
			{
				this._collisionLayers = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000053D1 File Offset: 0x000035D1
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000053D9 File Offset: 0x000035D9
		public QueryTriggerInteraction triggerInteraction
		{
			get
			{
				return this._triggerInteraction;
			}
			set
			{
				this._triggerInteraction = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000053E2 File Offset: 0x000035E2
		// (set) Token: 0x0600017C RID: 380 RVA: 0x000053EA File Offset: 0x000035EA
		public bool detectCollisions
		{
			get
			{
				return this._detectCollisions;
			}
			set
			{
				this._detectCollisions = value;
				if (this._capsuleCollider)
				{
					this._capsuleCollider.enabled = this._detectCollisions;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00005411 File Offset: 0x00003611
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00005419 File Offset: 0x00003619
		public CollisionFlags collisionFlags { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00005422 File Offset: 0x00003622
		public bool isConstrainedToPlane
		{
			get
			{
				return this._planeConstraint > PlaneConstraint.None;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000542D File Offset: 0x0000362D
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00005435 File Offset: 0x00003635
		public bool constrainToGround
		{
			get
			{
				return this._isConstrainedToGround;
			}
			set
			{
				this._isConstrainedToGround = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000543E File Offset: 0x0000363E
		public bool isConstrainedToGround
		{
			get
			{
				return this._isConstrainedToGround && this._unconstrainedTimer == 0f;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00005457 File Offset: 0x00003657
		public bool isGroundConstraintPaused
		{
			get
			{
				return this._isConstrainedToGround && this._unconstrainedTimer > 0f;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005470 File Offset: 0x00003670
		public float unconstrainedTimer
		{
			get
			{
				return this._unconstrainedTimer;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00005478 File Offset: 0x00003678
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00005480 File Offset: 0x00003680
		public bool wasOnGround { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00005489 File Offset: 0x00003689
		public bool isOnGround
		{
			get
			{
				return this._currentGround.hitGround;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00005496 File Offset: 0x00003696
		// (set) Token: 0x06000189 RID: 393 RVA: 0x0000549E File Offset: 0x0000369E
		public bool wasOnWalkableGround { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000054A7 File Offset: 0x000036A7
		public bool isOnWalkableGround
		{
			get
			{
				return this._currentGround.isWalkableGround;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000054B4 File Offset: 0x000036B4
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000054BC File Offset: 0x000036BC
		public bool wasGrounded { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000054C5 File Offset: 0x000036C5
		public bool isGrounded
		{
			get
			{
				return this.isOnWalkableGround && this.isConstrainedToGround;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000054D7 File Offset: 0x000036D7
		public float groundDistance
		{
			get
			{
				return this._currentGround.groundDistance;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000054E4 File Offset: 0x000036E4
		public Vector3 groundPoint
		{
			get
			{
				return this._currentGround.point;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000054F1 File Offset: 0x000036F1
		public Vector3 groundNormal
		{
			get
			{
				return this._currentGround.normal;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000054FE File Offset: 0x000036FE
		public Vector3 groundSurfaceNormal
		{
			get
			{
				return this._currentGround.surfaceNormal;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000550B File Offset: 0x0000370B
		public Collider groundCollider
		{
			get
			{
				return this._currentGround.collider;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00005518 File Offset: 0x00003718
		public Transform groundTransform
		{
			get
			{
				return this._currentGround.transform;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00005525 File Offset: 0x00003725
		public Rigidbody groundRigidbody
		{
			get
			{
				return this._currentGround.rigidbody;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00005532 File Offset: 0x00003732
		public FindGroundResult currentGround
		{
			get
			{
				return this._currentGround;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000553A File Offset: 0x0000373A
		public CharacterMovement.MovingPlatform movingPlatform
		{
			get
			{
				return this._movingPlatform;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00005542 File Offset: 0x00003742
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000554A File Offset: 0x0000374A
		public Vector3 landedVelocity { get; private set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00005553 File Offset: 0x00003753
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000555B File Offset: 0x0000375B
		public bool fastPlatformMove { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00005564 File Offset: 0x00003764
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00005571 File Offset: 0x00003771
		public bool impartPlatformMovement
		{
			get
			{
				return this._advanced.impartPlatformMovement;
			}
			set
			{
				this._advanced.impartPlatformMovement = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000557F File Offset: 0x0000377F
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000558C File Offset: 0x0000378C
		public bool impartPlatformRotation
		{
			get
			{
				return this._advanced.impartPlatformRotation;
			}
			set
			{
				this._advanced.impartPlatformRotation = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000559A File Offset: 0x0000379A
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000055A7 File Offset: 0x000037A7
		public bool impartPlatformVelocity
		{
			get
			{
				return this._advanced.impartPlatformVelocity;
			}
			set
			{
				this._advanced.impartPlatformVelocity = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000055B5 File Offset: 0x000037B5
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000055C2 File Offset: 0x000037C2
		public bool enablePhysicsInteraction
		{
			get
			{
				return this._advanced.enablePhysicsInteraction;
			}
			set
			{
				this._advanced.enablePhysicsInteraction = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000055D0 File Offset: 0x000037D0
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000055DD File Offset: 0x000037DD
		public bool physicsInteractionAffectsCharacters
		{
			get
			{
				return this._advanced.allowPushCharacters;
			}
			set
			{
				this._advanced.allowPushCharacters = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000055EB File Offset: 0x000037EB
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000055F3 File Offset: 0x000037F3
		public float pushForceScale
		{
			get
			{
				return this._pushForceScale;
			}
			set
			{
				this._pushForceScale = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005606 File Offset: 0x00003806
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000560E File Offset: 0x0000380E
		public CharacterMovement.ColliderFilterCallback colliderFilterCallback { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00005617 File Offset: 0x00003817
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000561F File Offset: 0x0000381F
		public CharacterMovement.CollisionBehaviourCallback collisionBehaviourCallback { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00005628 File Offset: 0x00003828
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00005630 File Offset: 0x00003830
		public CharacterMovement.CollisionResponseCallback collisionResponseCallback { get; set; }

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060001AD RID: 429 RVA: 0x0000563C File Offset: 0x0000383C
		// (remove) Token: 0x060001AE RID: 430 RVA: 0x00005674 File Offset: 0x00003874
		public event CharacterMovement.CollidedEventHandler Collided;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060001AF RID: 431 RVA: 0x000056AC File Offset: 0x000038AC
		// (remove) Token: 0x060001B0 RID: 432 RVA: 0x000056E4 File Offset: 0x000038E4
		public event CharacterMovement.FoundGroundEventHandler FoundGround;

		// Token: 0x060001B1 RID: 433 RVA: 0x0000571C File Offset: 0x0000391C
		private void OnCollided()
		{
			if (this.Collided == null)
			{
				return;
			}
			for (int i = 0; i < this._collisionCount; i++)
			{
				this.Collided(ref this._collisionResults[i]);
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000575A File Offset: 0x0000395A
		private void OnFoundGround()
		{
			CharacterMovement.FoundGroundEventHandler foundGround = this.FoundGround;
			if (foundGround == null)
			{
				return;
			}
			foundGround(ref this._currentGround);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005774 File Offset: 0x00003974
		private Vector3 FindOpposingNormal(Vector3 sweepDirDenorm, ref RaycastHit inHit)
		{
			Vector3 normal = inHit.normal;
			Vector3 origin = inHit.point - sweepDirDenorm;
			float distance = sweepDirDenorm.magnitude * 2f;
			Vector3 direction = sweepDirDenorm / sweepDirDenorm.magnitude;
			RaycastHit raycastHit;
			if (this.Raycast(origin, direction, distance, this._collisionLayers, out raycastHit, 0.0042499998f))
			{
				normal = raycastHit.normal;
			}
			return normal;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000057D8 File Offset: 0x000039D8
		private static Vector3 FindBoxOpposingNormal(Vector3 sweepDirDenorm, ref RaycastHit inHit)
		{
			Transform transform = inHit.transform;
			Vector3 vector = transform.InverseTransformDirection(inHit.normal);
			Vector3 vector2 = transform.InverseTransformDirection(sweepDirDenorm);
			Vector3 direction = vector;
			float num = float.MaxValue;
			for (int i = 0; i < 3; i++)
			{
				if (vector[i] > 0.0001f)
				{
					float num2 = vector2[i];
					if (num2 < num)
					{
						num = num2;
						direction = Vector3.zero;
						direction[i] = 1f;
					}
				}
				else if (vector[i] < -0.0001f)
				{
					float num3 = -vector2[i];
					if (num3 < num)
					{
						num = num3;
						direction = Vector3.zero;
						direction[i] = -1f;
					}
				}
			}
			return transform.TransformDirection(direction);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005898 File Offset: 0x00003A98
		private static Vector3 FindBoxOpposingNormal(Vector3 displacement, Vector3 hitNormal, Transform hitTransform)
		{
			Vector3 vector = hitTransform.InverseTransformDirection(hitNormal);
			Vector3 vector2 = hitTransform.InverseTransformDirection(displacement);
			Vector3 direction = vector;
			float num = float.MaxValue;
			for (int i = 0; i < 3; i++)
			{
				if (vector[i] > 0.0001f)
				{
					float num2 = vector2[i];
					if (num2 < num)
					{
						num = num2;
						direction = Vector3.zero;
						direction[i] = 1f;
					}
				}
				else if (vector[i] < -0.0001f)
				{
					float num3 = -vector2[i];
					if (num3 < num)
					{
						num = num3;
						direction = Vector3.zero;
						direction[i] = -1f;
					}
				}
			}
			return hitTransform.TransformDirection(direction);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000594C File Offset: 0x00003B4C
		private static Vector3 FindTerrainOpposingNormal(ref RaycastHit inHit)
		{
			TerrainCollider terrainCollider = inHit.collider as TerrainCollider;
			if (terrainCollider != null)
			{
				Vector3 vector = terrainCollider.transform.InverseTransformPoint(inHit.point);
				TerrainData terrainData = terrainCollider.terrainData;
				return terrainData.GetInterpolatedNormal(vector.x / terrainData.size.x, vector.z / terrainData.size.z);
			}
			return inHit.normal;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000059B8 File Offset: 0x00003BB8
		private Vector3 FindGeomOpposingNormal(Vector3 sweepDirDenorm, ref RaycastHit inHit)
		{
			if (inHit.collider is SphereCollider || inHit.collider is CapsuleCollider)
			{
				return inHit.normal;
			}
			if (inHit.collider is BoxCollider)
			{
				return CharacterMovement.FindBoxOpposingNormal(sweepDirDenorm, ref inHit);
			}
			MeshCollider meshCollider = inHit.collider as MeshCollider;
			if (meshCollider != null && !meshCollider.convex)
			{
				Mesh sharedMesh = meshCollider.sharedMesh;
				if (sharedMesh && sharedMesh.isReadable && !this._advanced.useFastGeomNormalPath)
				{
					return MeshUtility.FindMeshOpposingNormal(sharedMesh, ref inHit);
				}
				return this.FindOpposingNormal(sweepDirDenorm, ref inHit);
			}
			else
			{
				MeshCollider meshCollider2 = inHit.collider as MeshCollider;
				if (meshCollider2 != null && meshCollider2.convex)
				{
					return this.FindOpposingNormal(sweepDirDenorm, ref inHit);
				}
				if (inHit.collider is TerrainCollider && !this._advanced.useFastGeomNormalPath)
				{
					return CharacterMovement.FindTerrainOpposingNormal(ref inHit);
				}
				return inHit.normal;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005A8E File Offset: 0x00003C8E
		public static bool IsFinite(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005AA3 File Offset: 0x00003CA3
		public static bool IsFinite(Vector3 value)
		{
			return CharacterMovement.IsFinite(value.x) && CharacterMovement.IsFinite(value.y) && CharacterMovement.IsFinite(value.z);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00005ACC File Offset: 0x00003CCC
		private static Vector3 ApplyVelocityBraking(Vector3 currentVelocity, float friction, float deceleration, float deltaTime)
		{
			bool flag = friction == 0f;
			bool flag2 = deceleration == 0f;
			if (flag && flag2)
			{
				return currentVelocity;
			}
			Vector3 rhs = currentVelocity;
			Vector3 b = flag2 ? Vector3.zero : (-deceleration * currentVelocity.normalized);
			currentVelocity += (-friction * currentVelocity + b) * deltaTime;
			if (Vector3.Dot(currentVelocity, rhs) <= 0f)
			{
				return Vector3.zero;
			}
			float sqrMagnitude = currentVelocity.sqrMagnitude;
			if (sqrMagnitude <= 1E-05f || (!flag2 && sqrMagnitude <= 0.01f))
			{
				return Vector3.zero;
			}
			return currentVelocity;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00005B5D File Offset: 0x00003D5D
		private static float ComputeAnalogInputModifier(Vector3 desiredVelocity, float maxSpeed)
		{
			if (maxSpeed > 0f && desiredVelocity.sqrMagnitude > 0f)
			{
				return Mathf.Clamp01(desiredVelocity.magnitude / maxSpeed);
			}
			return 0f;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005B8C File Offset: 0x00003D8C
		private static Vector3 CalcVelocity(Vector3 currentVelocity, Vector3 desiredVelocity, float maxSpeed, float acceleration, float deceleration, float friction, float brakingFriction, float deltaTime)
		{
			float magnitude = desiredVelocity.magnitude;
			Vector3 a = (magnitude > 0f) ? (desiredVelocity / magnitude) : Vector3.zero;
			float num = CharacterMovement.ComputeAnalogInputModifier(desiredVelocity, maxSpeed);
			Vector3 vector = acceleration * num * a;
			float num2 = Mathf.Max(0f, maxSpeed * num);
			bool flag = vector.isZero();
			bool flag2 = currentVelocity.isExceeding(num2);
			if (flag || flag2)
			{
				Vector3 rhs = currentVelocity;
				currentVelocity = CharacterMovement.ApplyVelocityBraking(currentVelocity, brakingFriction, deceleration, deltaTime);
				if (flag2 && currentVelocity.sqrMagnitude < num2.square() && Vector3.Dot(vector, rhs) > 0f)
				{
					currentVelocity = rhs.normalized * num2;
				}
			}
			else
			{
				currentVelocity -= (currentVelocity - a * currentVelocity.magnitude) * Mathf.Min(friction * deltaTime, 1f);
			}
			if (!flag)
			{
				float maxLength = currentVelocity.isExceeding(num2) ? currentVelocity.magnitude : num2;
				currentVelocity += vector * deltaTime;
				currentVelocity = currentVelocity.clampedTo(maxLength);
			}
			return currentVelocity;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005C98 File Offset: 0x00003E98
		private unsafe static Vector3 GetRigidbodyVelocity(Rigidbody rigidbody, Vector3 worldPoint)
		{
			if (rigidbody == null)
			{
				return Vector3.zero;
			}
			CharacterMovement characterMovement;
			if (!rigidbody.TryGetComponent<CharacterMovement>(out characterMovement))
			{
				return rigidbody.GetPointVelocity(worldPoint);
			}
			return *characterMovement.velocity;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005CD1 File Offset: 0x00003ED1
		private static bool IsWalkable(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.Walkable) > CollisionBehaviour.Default;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005CD9 File Offset: 0x00003ED9
		private static bool IsNotWalkable(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.NotWalkable) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005CE1 File Offset: 0x00003EE1
		private static bool CanPerchOn(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.CanPerchOn) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00005CE9 File Offset: 0x00003EE9
		private static bool CanNotPerchOn(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.CanNotPerchOn) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005CF1 File Offset: 0x00003EF1
		private static bool CanStepOn(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.CanStepOn) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00005CFA File Offset: 0x00003EFA
		private static bool CanNotStepOn(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.CanNotStepOn) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00005D03 File Offset: 0x00003F03
		private static bool CanRideOn(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.CanRideOn) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00005D0C File Offset: 0x00003F0C
		private static bool CanNotRideOn(CollisionBehaviour behaviourFlags)
		{
			return (behaviourFlags & CollisionBehaviour.CanNotRideOn) > CollisionBehaviour.Default;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00005D18 File Offset: 0x00003F18
		private static void MakeCapsule(float radius, float height, out Vector3 center, out Vector3 bottomCenter, out Vector3 topCenter)
		{
			radius = Mathf.Max(radius, 0f);
			height = Mathf.Max(height, radius * 2f);
			center = height * 0.5f * Vector3.up;
			float num = height - radius * 2f;
			bottomCenter = center - num * 0.5f * Vector3.up;
			topCenter = center + num * 0.5f * Vector3.up;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public void SetDimensions(float characterRadius, float characterHeight)
		{
			this._radius = Mathf.Max(characterRadius, 0f);
			this._height = Mathf.Max(characterHeight, characterRadius * 2f);
			CharacterMovement.MakeCapsule(this._radius, this._height, out this._capsuleCenter, out this._capsuleBottomCenter, out this._capsuleTopCenter);
			if (this._capsuleCollider)
			{
				this._capsuleCollider.radius = this._radius;
				this._capsuleCollider.height = this._height;
				this._capsuleCollider.center = this._capsuleCenter;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00005E3C File Offset: 0x0000403C
		public void SetHeight(float characterHeight)
		{
			this._height = Mathf.Max(characterHeight, this._radius * 2f);
			CharacterMovement.MakeCapsule(this._radius, this._height, out this._capsuleCenter, out this._capsuleBottomCenter, out this._capsuleTopCenter);
			if (this._capsuleCollider)
			{
				this._capsuleCollider.height = this._height;
				this._capsuleCollider.center = this._capsuleCenter;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00005EB4 File Offset: 0x000040B4
		private void CacheComponents()
		{
			this._transform = base.GetComponent<Transform>();
			this._rigidbody = base.GetComponent<Rigidbody>();
			if (this._rigidbody)
			{
				this._rigidbody.drag = 0f;
				this._rigidbody.angularDrag = 0f;
				this._rigidbody.useGravity = false;
				this._rigidbody.isKinematic = true;
			}
			this._capsuleCollider = base.GetComponent<CapsuleCollider>();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00005F2A File Offset: 0x0000412A
		public Vector3 GetPlaneConstraintNormal()
		{
			return this._constraintPlaneNormal;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00005F34 File Offset: 0x00004134
		public void SetPlaneConstraint(PlaneConstraint constrainAxis, Vector3 planeNormal)
		{
			this._planeConstraint = constrainAxis;
			switch (this._planeConstraint)
			{
			case PlaneConstraint.None:
				this._constraintPlaneNormal = Vector3.zero;
				if (this._rigidbody)
				{
					this._rigidbody.constraints = RigidbodyConstraints.None;
					return;
				}
				break;
			case PlaneConstraint.ConstrainXAxis:
				this._constraintPlaneNormal = Vector3.right;
				if (this._rigidbody)
				{
					this._rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
					return;
				}
				break;
			case PlaneConstraint.ConstrainYAxis:
				this._constraintPlaneNormal = Vector3.up;
				if (this._rigidbody)
				{
					this._rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
					return;
				}
				break;
			case PlaneConstraint.ConstrainZAxis:
				this._constraintPlaneNormal = Vector3.forward;
				if (this._rigidbody)
				{
					this._rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
					return;
				}
				break;
			case PlaneConstraint.Custom:
				this._constraintPlaneNormal = planeNormal;
				if (this._rigidbody)
				{
					this._rigidbody.constraints = RigidbodyConstraints.None;
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000602C File Offset: 0x0000422C
		public Vector3 ConstrainDirectionToPlane(Vector3 direction)
		{
			return this.ConstrainVectorToPlane(direction).normalized;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00006048 File Offset: 0x00004248
		public Vector3 ConstrainVectorToPlane(Vector3 vector)
		{
			if (!this.isConstrainedToPlane)
			{
				return vector;
			}
			return vector.projectedOnPlane(this._constraintPlaneNormal);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00006060 File Offset: 0x00004260
		private void ResetCollisionFlags()
		{
			this.collisionFlags = CollisionFlags.None;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006069 File Offset: 0x00004269
		private void UpdateCollisionFlags(HitLocation hitLocation)
		{
			this.collisionFlags |= (CollisionFlags)hitLocation;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000607C File Offset: 0x0000427C
		private HitLocation ComputeHitLocation(Vector3 inNormal)
		{
			float num = inNormal.dot(this._characterUp);
			if (num > 0.01f)
			{
				return HitLocation.Below;
			}
			if (num >= -0.01f)
			{
				return HitLocation.Sides;
			}
			return HitLocation.Above;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000060AC File Offset: 0x000042AC
		private bool IsWalkable(Collider inCollider, Vector3 inNormal)
		{
			if (this.ComputeHitLocation(inNormal) != HitLocation.Below)
			{
				return false;
			}
			if (this.collisionBehaviourCallback != null)
			{
				CollisionBehaviour behaviourFlags = this.collisionBehaviourCallback(inCollider);
				if (CharacterMovement.IsWalkable(behaviourFlags))
				{
					return Vector3.Dot(inNormal, this._characterUp) > 0.017452f;
				}
				if (CharacterMovement.IsNotWalkable(behaviourFlags))
				{
					return Vector3.Dot(inNormal, this._characterUp) > 1f;
				}
			}
			float num = this._minSlopeLimit;
			SlopeLimitBehaviour slopeLimitBehaviour;
			if (this._slopeLimitOverride && inCollider.TryGetComponent<SlopeLimitBehaviour>(out slopeLimitBehaviour))
			{
				switch (slopeLimitBehaviour.walkableSlopeBehaviour)
				{
				case SlopeBehaviour.Walkable:
					num = 0.017452f;
					break;
				case SlopeBehaviour.NotWalkable:
					num = 1f;
					break;
				case SlopeBehaviour.Override:
					num = slopeLimitBehaviour.slopeLimitCos;
					break;
				}
			}
			return Vector3.Dot(inNormal, this._characterUp) > num;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00006170 File Offset: 0x00004370
		private Vector3 ComputeBlockingNormal(Vector3 inNormal, bool isWalkable)
		{
			if ((this.isGrounded || this._hasLanded) && !isWalkable)
			{
				Vector3 vector = (this._hasLanded ? this._foundGround.normal : this._currentGround.normal).perpendicularTo(inNormal).perpendicularTo(this._characterUp);
				if (Vector3.Dot(vector, inNormal) < 0f)
				{
					vector = -vector;
				}
				if (!vector.isZero())
				{
					inNormal = vector;
				}
				return inNormal;
			}
			return inNormal;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000061E8 File Offset: 0x000043E8
		private bool ShouldFilter(Collider otherCollider)
		{
			if (otherCollider == this._capsuleCollider || otherCollider.attachedRigidbody == this.rigidbody)
			{
				return true;
			}
			if (this._ignoredColliders.Contains(otherCollider))
			{
				return true;
			}
			Rigidbody attachedRigidbody = otherCollider.attachedRigidbody;
			return (attachedRigidbody && this._ignoredRigidbodies.Contains(attachedRigidbody)) || (this.colliderFilterCallback != null && this.colliderFilterCallback(otherCollider));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000625D File Offset: 0x0000445D
		public void CapsuleIgnoreCollision(Collider otherCollider, bool ignore = true)
		{
			if (otherCollider == null)
			{
				return;
			}
			Physics.IgnoreCollision(this._capsuleCollider, otherCollider, ignore);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006276 File Offset: 0x00004476
		public void IgnoreCollision(Collider otherCollider, bool ignore = true)
		{
			if (otherCollider == null)
			{
				return;
			}
			if (ignore)
			{
				this._ignoredColliders.Add(otherCollider);
				return;
			}
			this._ignoredColliders.Remove(otherCollider);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000062A0 File Offset: 0x000044A0
		public void IgnoreCollision(Rigidbody otherRigidbody, bool ignore = true)
		{
			if (otherRigidbody == null)
			{
				return;
			}
			if (ignore)
			{
				this._ignoredRigidbodies.Add(otherRigidbody);
				return;
			}
			this._ignoredRigidbodies.Remove(otherRigidbody);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000062CA File Offset: 0x000044CA
		private void ClearCollisionResults()
		{
			this._collisionCount = 0;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000062D4 File Offset: 0x000044D4
		private void AddCollisionResult(ref CollisionResult collisionResult)
		{
			this.UpdateCollisionFlags(collisionResult.hitLocation);
			if (collisionResult.rigidbody)
			{
				if (collisionResult.rigidbody == this._movingPlatform.platform)
				{
					return;
				}
				for (int i = 0; i < this._collisionCount; i++)
				{
					if (collisionResult.rigidbody == this._collisionResults[i].rigidbody)
					{
						return;
					}
				}
			}
			if (this._collisionCount < 16)
			{
				CollisionResult[] collisionResults = this._collisionResults;
				int collisionCount = this._collisionCount;
				this._collisionCount = collisionCount + 1;
				collisionResults[collisionCount] = collisionResult;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000636F File Offset: 0x0000456F
		public int GetCollisionCount()
		{
			return this._collisionCount;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006377 File Offset: 0x00004577
		public CollisionResult GetCollisionResult(int index)
		{
			return this._collisionResults[index];
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006388 File Offset: 0x00004588
		private bool ComputeInflatedMTD(Vector3 characterPosition, Quaternion characterRotation, float mtdInflation, Collider hitCollider, Transform hitTransform, out Vector3 mtdDirection, out float mtdDistance)
		{
			mtdDirection = Vector3.zero;
			mtdDistance = 0f;
			this._capsuleCollider.radius = this._radius + mtdInflation * 1f;
			this._capsuleCollider.height = this._height + mtdInflation * 2f;
			Vector3 vector;
			float f;
			bool flag = Physics.ComputePenetration(this._capsuleCollider, characterPosition, characterRotation, hitCollider, hitTransform.position, hitTransform.rotation, out vector, out f);
			if (flag)
			{
				if (CharacterMovement.IsFinite(vector))
				{
					mtdDirection = vector;
					mtdDistance = Mathf.Max(Mathf.Abs(f) - mtdInflation, 0f) + 0.0001f;
				}
				else
				{
					Debug.LogWarning("Warning: ComputeInflatedMTD_Internal: MTD returned NaN " + vector.ToString("F4"));
				}
			}
			this._capsuleCollider.radius = this._radius;
			this._capsuleCollider.height = this._height;
			return flag;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006469 File Offset: 0x00004669
		private bool ComputeMTD(Vector3 characterPosition, Quaternion characterRotation, Collider hitCollider, Transform hitTransform, out Vector3 mtdDirection, out float mtdDistance)
		{
			return this.ComputeInflatedMTD(characterPosition, characterRotation, 0.0025f, hitCollider, hitTransform, out mtdDirection, out mtdDistance) || this.ComputeInflatedMTD(characterPosition, characterRotation, 0.0175f, hitCollider, hitTransform, out mtdDirection, out mtdDistance);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000649C File Offset: 0x0000469C
		private void ResolveOverlaps(CharacterMovement.DepenetrationBehaviour depenetrationBehaviour = CharacterMovement.DepenetrationBehaviour.IgnoreNone)
		{
			if (!this.detectCollisions)
			{
				return;
			}
			bool flag = (depenetrationBehaviour & CharacterMovement.DepenetrationBehaviour.IgnoreStatic) > CharacterMovement.DepenetrationBehaviour.IgnoreNone;
			bool flag2 = (depenetrationBehaviour & CharacterMovement.DepenetrationBehaviour.IgnoreDynamic) > CharacterMovement.DepenetrationBehaviour.IgnoreNone;
			bool flag3 = (depenetrationBehaviour & CharacterMovement.DepenetrationBehaviour.IgnoreKinematic) > CharacterMovement.DepenetrationBehaviour.IgnoreNone;
			for (int i = 0; i < this._advanced.maxDepenetrationIterations; i++)
			{
				Vector3 point = this.updatedPosition + this._transformedCapsuleTopCenter;
				int num = Physics.OverlapCapsuleNonAlloc(this.updatedPosition + this._transformedCapsuleBottomCenter, point, this._radius, this._overlaps, this._collisionLayers, this.triggerInteraction);
				if (num == 0)
				{
					break;
				}
				for (int j = 0; j < num; j++)
				{
					Collider collider = this._overlaps[j];
					if (!this.ShouldFilter(collider))
					{
						Rigidbody attachedRigidbody = collider.attachedRigidbody;
						if (!flag || !(attachedRigidbody == null))
						{
							if (attachedRigidbody)
							{
								bool isKinematic = attachedRigidbody.isKinematic;
								if ((flag3 && isKinematic) || (flag2 && !isKinematic))
								{
									goto IL_24A;
								}
							}
							Vector3 vector;
							float num2;
							if (this.ComputeMTD(this.updatedPosition, this.updatedRotation, collider, collider.transform, out vector, out num2))
							{
								vector = this.ConstrainDirectionToPlane(vector);
								HitLocation hitLocation = this.ComputeHitLocation(vector);
								bool isWalkable = this.IsWalkable(collider, vector);
								Vector3 vector2 = this.ComputeBlockingNormal(vector, isWalkable);
								this.updatedPosition += vector2 * (num2 + 0.00125f);
								if (this._collisionCount < 16)
								{
									Vector3 vector3;
									if (hitLocation == HitLocation.Above)
									{
										vector3 = this.updatedPosition + this._transformedCapsuleTopCenter - vector * this._radius;
									}
									else if (hitLocation == HitLocation.Below)
									{
										vector3 = this.updatedPosition + this._transformedCapsuleBottomCenter - vector * this._radius;
									}
									else
									{
										vector3 = this.updatedPosition + this._transformedCapsuleCenter - vector * this._radius;
									}
									CollisionResult collisionResult = new CollisionResult
									{
										startPenetrating = true,
										hitLocation = hitLocation,
										isWalkable = isWalkable,
										position = this.updatedPosition,
										velocity = this._velocity,
										otherVelocity = CharacterMovement.GetRigidbodyVelocity(attachedRigidbody, vector3),
										point = vector3,
										normal = vector2,
										surfaceNormal = vector2,
										collider = collider
									};
									this.AddCollisionResult(ref collisionResult);
								}
							}
						}
					}
					IL_24A:;
				}
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006718 File Offset: 0x00004918
		public int OverlapTest(Vector3 characterPosition, Quaternion characterRotation, float testRadius, float testHeight, int layerMask, Collider[] results, QueryTriggerInteraction queryTriggerInteraction)
		{
			Vector3 vector;
			Vector3 point;
			Vector3 point2;
			CharacterMovement.MakeCapsule(testRadius, testHeight, out vector, out point, out point2);
			Vector3 point3 = characterPosition + characterRotation * point2;
			int num = Physics.OverlapCapsuleNonAlloc(characterPosition + characterRotation * point, point3, testRadius, results, layerMask, queryTriggerInteraction);
			if (num == 0)
			{
				return 0;
			}
			int num2 = num;
			for (int i = 0; i < num; i++)
			{
				Collider otherCollider = results[i];
				if (this.ShouldFilter(otherCollider) && i < --num2)
				{
					results[i] = results[num2];
				}
			}
			return num2;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000679C File Offset: 0x0000499C
		public Collider[] OverlapTest(Vector3 characterPosition, Quaternion characterRotation, float testRadius, float testHeight, int layerMask, QueryTriggerInteraction queryTriggerInteraction, out int overlapCount)
		{
			overlapCount = this.OverlapTest(characterPosition, characterRotation, testRadius, testHeight, layerMask, this._overlaps, queryTriggerInteraction);
			return this._overlaps;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000067C8 File Offset: 0x000049C8
		public Collider[] OverlapTest(int layerMask, QueryTriggerInteraction queryTriggerInteraction, out int overlapCount)
		{
			overlapCount = this.OverlapTest(this.position, this.rotation, this.radius, this.height, layerMask, this._overlaps, queryTriggerInteraction);
			return this._overlaps;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006804 File Offset: 0x00004A04
		public bool CheckCapsule()
		{
			this.IgnoreCollision(this._movingPlatform.platform, true);
			int num = this.OverlapTest(this.position, this.rotation, this.radius, this.height, this.collisionLayers, this._overlaps, this.triggerInteraction);
			this.IgnoreCollision(this._movingPlatform.platform, false);
			return num > 0;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006870 File Offset: 0x00004A70
		public bool CheckHeight(float testHeight)
		{
			this.IgnoreCollision(this._movingPlatform.platform, true);
			int num = this.OverlapTest(this.position, this.rotation, this.radius, testHeight, this.collisionLayers, this._overlaps, this.triggerInteraction);
			this.IgnoreCollision(this._movingPlatform.platform, false);
			return num > 0;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000068D4 File Offset: 0x00004AD4
		public bool IsWithinEdgeTolerance(Vector3 characterPosition, Vector3 inPoint, float testRadius)
		{
			float sqrMagnitude = (inPoint - characterPosition).projectedOnPlane(this._characterUp).sqrMagnitude;
			float num = Mathf.Max(0.0016f, testRadius - 0.0015f);
			return sqrMagnitude < num * num;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006912 File Offset: 0x00004B12
		private bool ShouldCheckForValidLandingSpot(ref CollisionResult inCollision)
		{
			return inCollision.hitLocation == HitLocation.Below && inCollision.normal != inCollision.surfaceNormal && this.IsWithinEdgeTolerance(this.updatedPosition, inCollision.point, this._radius);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00006950 File Offset: 0x00004B50
		private bool IsValidLandingSpot(Vector3 characterPosition, ref CollisionResult inCollision)
		{
			if (!inCollision.isWalkable)
			{
				return false;
			}
			if (inCollision.hitLocation != HitLocation.Below)
			{
				return false;
			}
			if (!this.IsWithinEdgeTolerance(characterPosition, inCollision.point, this._radius))
			{
				inCollision.isWalkable = false;
				return false;
			}
			FindGroundResult foundGround;
			this.FindGround(characterPosition, out foundGround);
			inCollision.isWalkable = foundGround.isWalkableGround;
			if (inCollision.isWalkable)
			{
				this._foundGround = foundGround;
				return true;
			}
			return false;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000069B8 File Offset: 0x00004BB8
		public bool Raycast(Vector3 origin, Vector3 direction, float distance, int layerMask, out RaycastHit hitResult, float thickness = 0f)
		{
			hitResult = default(RaycastHit);
			int num = (thickness == 0f) ? Physics.RaycastNonAlloc(origin, direction, this._hits, distance, layerMask, this.triggerInteraction) : Physics.SphereCastNonAlloc(origin - direction * thickness, thickness, direction, this._hits, distance, layerMask, this.triggerInteraction);
			if (num == 0)
			{
				return false;
			}
			float num2 = float.PositiveInfinity;
			int num3 = -1;
			for (int i = 0; i < num; i++)
			{
				ref RaycastHit ptr = ref this._hits[i];
				if (ptr.distance > 0f && !this.ShouldFilter(ptr.collider) && ptr.distance < num2)
				{
					num2 = ptr.distance;
					num3 = i;
				}
			}
			if (num3 != -1)
			{
				hitResult = this._hits[num3];
				return true;
			}
			return false;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00006A88 File Offset: 0x00004C88
		private bool CapsuleCast(Vector3 characterPosition, float castRadius, Vector3 castDirection, float castDistance, int layerMask, out RaycastHit hitResult, out bool startPenetrating)
		{
			hitResult = default(RaycastHit);
			startPenetrating = false;
			Vector3 point = characterPosition + this._transformedCapsuleTopCenter;
			int num = Physics.CapsuleCastNonAlloc(characterPosition + this._transformedCapsuleBottomCenter, point, castRadius, castDirection, this._hits, castDistance, layerMask, this.triggerInteraction);
			if (num == 0)
			{
				return false;
			}
			float num2 = float.PositiveInfinity;
			int num3 = -1;
			for (int i = 0; i < num; i++)
			{
				ref RaycastHit ptr = ref this._hits[i];
				if (!this.ShouldFilter(ptr.collider))
				{
					if (ptr.distance <= 0f)
					{
						startPenetrating = true;
					}
					else if (ptr.distance < num2)
					{
						num2 = ptr.distance;
						num3 = i;
					}
				}
			}
			if (num3 != -1)
			{
				hitResult = this._hits[num3];
				return true;
			}
			return false;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00006B54 File Offset: 0x00004D54
		private static void SortArray(RaycastHit[] array, int length)
		{
			for (int i = 1; i < length; i++)
			{
				RaycastHit raycastHit = array[i];
				int num = 0;
				int num2 = i - 1;
				while (num2 >= 0 && num != 1)
				{
					if (raycastHit.distance < array[num2].distance)
					{
						array[num2 + 1] = array[num2];
						num2--;
						array[num2 + 1] = raycastHit;
					}
					else
					{
						num = 1;
					}
				}
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00006BBC File Offset: 0x00004DBC
		private bool CapsuleCastEx(Vector3 characterPosition, float castRadius, Vector3 castDirection, float castDistance, int layerMask, out RaycastHit hitResult, out bool startPenetrating, out Vector3 recoverDirection, out float recoverDistance, bool ignoreNonBlockingOverlaps = false)
		{
			hitResult = default(RaycastHit);
			startPenetrating = false;
			recoverDirection = default(Vector3);
			recoverDistance = 0f;
			Vector3 point = characterPosition + this._transformedCapsuleTopCenter;
			int num = Physics.CapsuleCastNonAlloc(characterPosition + this._transformedCapsuleBottomCenter, point, castRadius, castDirection, this._hits, castDistance, layerMask, this.triggerInteraction);
			if (num == 0)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				ref RaycastHit ptr = ref this._hits[i];
				Vector3 vector;
				float num2;
				if (!this.ShouldFilter(ptr.collider) && ptr.distance <= 0f && this.ComputeMTD(characterPosition, this.updatedRotation, ptr.collider, ptr.collider.transform, out vector, out num2))
				{
					vector = this.ConstrainDirectionToPlane(vector);
					HitLocation hitLocation = this.ComputeHitLocation(vector);
					Vector3 point2;
					if (hitLocation == HitLocation.Above)
					{
						point2 = characterPosition + this._transformedCapsuleTopCenter - vector * this._radius;
					}
					else if (hitLocation == HitLocation.Below)
					{
						point2 = characterPosition + this._transformedCapsuleBottomCenter - vector * this._radius;
					}
					else
					{
						point2 = characterPosition + this._transformedCapsuleCenter - vector * this._radius;
					}
					Vector3 normal = this.ComputeBlockingNormal(vector, this.IsWalkable(ptr.collider, vector));
					ptr.point = point2;
					ptr.normal = normal;
					ptr.distance = -num2;
				}
			}
			if (num > 2)
			{
				CharacterMovement.SortArray(this._hits, num);
			}
			float num3 = float.PositiveInfinity;
			int num4 = -1;
			for (int j = 0; j < num; j++)
			{
				ref RaycastHit ptr2 = ref this._hits[j];
				if (!this.ShouldFilter(ptr2.collider))
				{
					if (ptr2.distance <= 0f && !ptr2.point.isZero())
					{
						float num5 = Vector3.Dot(castDirection, ptr2.normal);
						if ((!ignoreNonBlockingOverlaps || num5 <= 0f) && num5 < num3)
						{
							num3 = num5;
							num4 = j;
						}
					}
					else if (num4 == -1)
					{
						num4 = j;
						break;
					}
				}
			}
			if (num4 >= 0)
			{
				hitResult = this._hits[num4];
				if (hitResult.distance <= 0f)
				{
					startPenetrating = true;
					recoverDirection = hitResult.normal;
					recoverDistance = Mathf.Abs(hitResult.distance);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00006E30 File Offset: 0x00005030
		private bool SweepTest(Vector3 sweepOrigin, float sweepRadius, Vector3 sweepDirection, float sweepDistance, int sweepLayerMask, out RaycastHit hitResult, out bool startPenetrating)
		{
			hitResult = default(RaycastHit);
			RaycastHit raycastHit;
			bool flag = this.CapsuleCast(sweepOrigin, sweepRadius, sweepDirection, sweepDistance + sweepRadius, sweepLayerMask, out raycastHit, out startPenetrating) && raycastHit.distance <= sweepDistance;
			float num = sweepRadius + 0.01f;
			RaycastHit raycastHit2;
			bool flag3;
			bool flag2 = this.CapsuleCast(sweepOrigin, num, sweepDirection, sweepDistance + num, sweepLayerMask, out raycastHit2, out flag3) && raycastHit2.distance <= sweepDistance;
			if (!flag && !flag2)
			{
				return false;
			}
			if (!flag2)
			{
				hitResult = raycastHit;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.01f);
			}
			else if (flag && raycastHit.distance < raycastHit2.distance)
			{
				hitResult = raycastHit;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.01f);
			}
			else
			{
				hitResult = raycastHit2;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.001f);
			}
			return true;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006F30 File Offset: 0x00005130
		private bool SweepTestEx(Vector3 sweepOrigin, float sweepRadius, Vector3 sweepDirection, float sweepDistance, int sweepLayerMask, out RaycastHit hitResult, out bool startPenetrating, out Vector3 recoverDirection, out float recoverDistance, bool ignoreBlockingOverlaps = false)
		{
			hitResult = default(RaycastHit);
			RaycastHit raycastHit;
			bool flag = this.CapsuleCastEx(sweepOrigin, sweepRadius, sweepDirection, sweepDistance + sweepRadius, sweepLayerMask, out raycastHit, out startPenetrating, out recoverDirection, out recoverDistance, ignoreBlockingOverlaps) && raycastHit.distance <= sweepDistance;
			if (flag & startPenetrating)
			{
				hitResult = raycastHit;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.001f);
				return true;
			}
			float num = sweepRadius + 0.01f;
			RaycastHit raycastHit2;
			bool flag3;
			bool flag2 = this.CapsuleCast(sweepOrigin, num, sweepDirection, sweepDistance + num, sweepLayerMask, out raycastHit2, out flag3) && raycastHit2.distance <= sweepDistance;
			if (!flag && !flag2)
			{
				return false;
			}
			if (!flag2)
			{
				hitResult = raycastHit;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.01f);
			}
			else if (flag && raycastHit.distance < raycastHit2.distance)
			{
				hitResult = raycastHit;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.01f);
			}
			else
			{
				hitResult = raycastHit2;
				hitResult.distance = Mathf.Max(0f, hitResult.distance - 0.001f);
			}
			return true;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007068 File Offset: 0x00005268
		private bool ResolvePenetration(Vector3 displacement, Vector3 proposedAdjustment)
		{
			Vector3 vector = this.ConstrainVectorToPlane(proposedAdjustment);
			if (vector.isZero())
			{
				return false;
			}
			if (this.OverlapTest(this.updatedPosition + vector, this.updatedRotation, this._radius + 0.001f, this._height, this._collisionLayers, this._overlaps, this.triggerInteraction) <= 0)
			{
				this.updatedPosition += vector;
				return true;
			}
			Vector3 updatedPosition = this.updatedPosition;
			RaycastHit raycastHit;
			bool flag;
			Vector3 a;
			float num;
			if (!this.CapsuleCastEx(this.updatedPosition, this._radius, vector.normalized, vector.magnitude, this._collisionLayers, out raycastHit, out flag, out a, out num, true))
			{
				this.updatedPosition += vector;
			}
			else
			{
				this.updatedPosition += vector.normalized * Mathf.Max(raycastHit.distance - 0.001f, 0f);
			}
			bool flag2 = this.updatedPosition != updatedPosition;
			if (!flag2 && flag)
			{
				Vector3 vector2 = a * (num + 0.01f + 0.00125f);
				Vector3 vector3 = vector + vector2;
				if (vector2 != vector && !vector3.isZero())
				{
					updatedPosition = this.updatedPosition;
					bool flag3;
					Vector3 vector4;
					float num2;
					if (!this.CapsuleCastEx(this.updatedPosition, this._radius, vector3.normalized, vector3.magnitude, this._collisionLayers, out raycastHit, out flag3, out vector4, out num2, true))
					{
						this.updatedPosition += vector3;
					}
					else
					{
						this.updatedPosition += vector3.normalized * Mathf.Max(raycastHit.distance - 0.001f, 0f);
					}
					flag2 = (this.updatedPosition != updatedPosition);
				}
			}
			if (!flag2)
			{
				Vector3 vector5 = this.ConstrainVectorToPlane(displacement);
				if (!vector5.isZero())
				{
					updatedPosition = this.updatedPosition;
					Vector3 b = vector + vector5;
					bool flag3;
					Vector3 vector4;
					float num2;
					if (!this.CapsuleCastEx(this.updatedPosition, this._radius, b.normalized, b.magnitude, this._collisionLayers, out raycastHit, out flag3, out vector4, out num2, true))
					{
						this.updatedPosition += b;
					}
					else
					{
						this.updatedPosition += b.normalized * Mathf.Max(raycastHit.distance - 0.001f, 0f);
					}
					flag2 = (this.updatedPosition != updatedPosition);
					if (!flag2 && Vector3.Dot(vector5, vector) > 0f)
					{
						updatedPosition = this.updatedPosition;
						if (!this.CapsuleCastEx(this.updatedPosition, this._radius, vector5.normalized, vector5.magnitude, this._collisionLayers, out raycastHit, out flag3, out vector4, out num2, true))
						{
							this.updatedPosition += vector5;
						}
						else
						{
							this.updatedPosition += vector5.normalized * Mathf.Max(raycastHit.distance - 0.001f, 0f);
						}
						flag2 = (this.updatedPosition != updatedPosition);
					}
				}
			}
			return flag2;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000073B8 File Offset: 0x000055B8
		private bool MovementSweepTest(Vector3 characterPosition, Vector3 inVelocity, Vector3 displacement, out CollisionResult collisionResult)
		{
			collisionResult = default(CollisionResult);
			Vector3 vector = characterPosition;
			Vector3 normalized = displacement.normalized;
			float radius = this._radius;
			float magnitude = displacement.magnitude;
			int sweepLayerMask = this._collisionLayers;
			RaycastHit hitResult;
			bool flag2;
			Vector3 a;
			float num;
			bool flag = this.SweepTestEx(vector, radius, normalized, magnitude, sweepLayerMask, out hitResult, out flag2, out a, out num, false);
			if (flag2)
			{
				Vector3 proposedAdjustment = a * (num + 0.01f + 0.00125f);
				if (this.ResolvePenetration(displacement, proposedAdjustment))
				{
					vector = this.updatedPosition;
					Vector3 vector2;
					float num2;
					flag = this.SweepTestEx(vector, radius, normalized, magnitude, sweepLayerMask, out hitResult, out flag2, out vector2, out num2, false);
				}
			}
			if (!flag)
			{
				return false;
			}
			HitLocation hitLocation = this.ComputeHitLocation(hitResult.normal);
			Vector3 vector3 = normalized * hitResult.distance;
			Vector3 remainingDisplacement = displacement - vector3;
			Vector3 position = vector + vector3;
			Vector3 vector4 = hitResult.normal;
			bool isWalkable = false;
			if (hitLocation == HitLocation.Below)
			{
				vector4 = this.FindGeomOpposingNormal(displacement, ref hitResult);
				isWalkable = this.IsWalkable(hitResult.collider, vector4);
			}
			collisionResult = new CollisionResult
			{
				startPenetrating = flag2,
				hitLocation = hitLocation,
				isWalkable = isWalkable,
				position = position,
				velocity = inVelocity,
				otherVelocity = CharacterMovement.GetRigidbodyVelocity(hitResult.rigidbody, hitResult.point),
				point = hitResult.point,
				normal = hitResult.normal,
				surfaceNormal = vector4,
				displacementToHit = vector3,
				remainingDisplacement = remainingDisplacement,
				collider = hitResult.collider,
				hitResult = hitResult
			};
			return true;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000755B File Offset: 0x0000575B
		public unsafe bool MovementSweepTest(Vector3 characterPosition, Vector3 sweepDirection, float sweepDistance, out CollisionResult collisionResult)
		{
			return this.MovementSweepTest(characterPosition, *this.velocity, sweepDirection * sweepDistance, out collisionResult);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007578 File Offset: 0x00005778
		private Vector3 HandleSlopeBoosting(Vector3 slideResult, Vector3 displacement, Vector3 inNormal)
		{
			Vector3 vector = slideResult;
			float num = Vector3.Dot(vector, this._characterUp);
			if (num > 0f)
			{
				float num2 = Vector3.Dot(displacement, this._characterUp);
				if (num - num2 > 0.0001f)
				{
					if (num2 > 0f)
					{
						float d = num2 / num;
						vector *= d;
					}
					else
					{
						vector = Vector3.zero;
					}
					Vector3 thisVector = (slideResult - vector).projectedOnPlane(this._characterUp);
					Vector3 normalized = inNormal.projectedOnPlane(this._characterUp).normalized;
					Vector3 b = thisVector.projectedOnPlane(normalized);
					vector += b;
				}
			}
			return vector;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000760C File Offset: 0x0000580C
		private Vector3 ComputeSlideVector(Vector3 displacement, Vector3 inNormal, bool isWalkable)
		{
			if (this.isGrounded)
			{
				if (isWalkable)
				{
					displacement = displacement.tangentTo(inNormal, this._characterUp);
				}
				else
				{
					Vector3 normal = inNormal.perpendicularTo(this.groundNormal).perpendicularTo(inNormal);
					displacement = displacement.projectedOnPlane(inNormal);
					displacement = displacement.tangentTo(normal, this._characterUp);
				}
			}
			else if (isWalkable)
			{
				if (this._isConstrainedToGround)
				{
					displacement = displacement.projectedOnPlane(this._characterUp);
				}
				displacement = displacement.projectedOnPlane(inNormal);
			}
			else
			{
				Vector3 vector = displacement.projectedOnPlane(inNormal);
				if (this._isConstrainedToGround)
				{
					vector = this.HandleSlopeBoosting(vector, displacement, inNormal);
				}
				displacement = vector;
			}
			return this.ConstrainVectorToPlane(displacement);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000076AC File Offset: 0x000058AC
		private int SlideAlongSurface(int iteration, Vector3 inputDisplacement, ref Vector3 inVelocity, ref Vector3 displacement, ref CollisionResult inHit, ref Vector3 prevNormal)
		{
			if (this.useFlatTop && inHit.hitLocation == HitLocation.Above)
			{
				Vector3 vector = CharacterMovement.FindBoxOpposingNormal(displacement, inHit.normal, inHit.transform);
				if (inHit.normal != vector)
				{
					inHit.normal = vector;
					inHit.surfaceNormal = vector;
				}
			}
			inHit.normal = this.ComputeBlockingNormal(inHit.normal, inHit.isWalkable);
			if (inHit.isWalkable && this.isConstrainedToGround)
			{
				inVelocity = this.ComputeSlideVector(inVelocity, inHit.normal, true);
				displacement = this.ComputeSlideVector(displacement, inHit.normal, true);
			}
			else
			{
				if (iteration == 0)
				{
					inVelocity = this.ComputeSlideVector(inVelocity, inHit.normal, inHit.isWalkable);
					displacement = this.ComputeSlideVector(displacement, inHit.normal, inHit.isWalkable);
					iteration++;
				}
				else if (iteration == 1)
				{
					Vector3 vector2 = prevNormal.perpendicularTo(inHit.normal);
					Vector3 vector3 = inputDisplacement.projectedOnPlane(vector2);
					Vector3 vector4 = this.ComputeSlideVector(displacement, inHit.normal, inHit.isWalkable);
					vector4 = vector4.projectedOnPlane(vector2);
					if (vector3.dot(vector4) <= 0f || prevNormal.dot(inHit.normal) < 0f)
					{
						inVelocity = this.ConstrainVectorToPlane(inVelocity.projectedOn(vector2));
						displacement = this.ConstrainVectorToPlane(displacement.projectedOn(vector2));
						iteration++;
					}
					else
					{
						inVelocity = this.ComputeSlideVector(inVelocity, inHit.normal, inHit.isWalkable);
						displacement = this.ComputeSlideVector(displacement, inHit.normal, inHit.isWalkable);
					}
				}
				else
				{
					inVelocity = Vector3.zero;
					displacement = Vector3.zero;
				}
				prevNormal = inHit.normal;
			}
			return iteration;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000078CC File Offset: 0x00005ACC
		private void PerformMovement(float deltaTime)
		{
			CharacterMovement.DepenetrationBehaviour depenetrationBehaviour = (!this.enablePhysicsInteraction) ? CharacterMovement.DepenetrationBehaviour.IgnoreDynamic : CharacterMovement.DepenetrationBehaviour.IgnoreNone;
			this.ResolveOverlaps(depenetrationBehaviour);
			if (this.isGrounded)
			{
				this._velocity = this._velocity.projectedOnPlane(this._characterUp);
			}
			Vector3 vector = this._velocity * deltaTime;
			if (this.isGrounded)
			{
				vector = vector.tangentTo(this.groundNormal, this._characterUp);
				vector = this.ConstrainVectorToPlane(vector);
			}
			Vector3 inputDisplacement = vector;
			int iteration = 0;
			Vector3 vector2 = default(Vector3);
			for (int i = 0; i < this._collisionCount; i++)
			{
				ref CollisionResult ptr = ref this._collisionResults[i];
				if (vector.dot(ptr.normal) < 0f)
				{
					if (this.isConstrainedToGround && !this.isOnWalkableGround)
					{
						if (this.IsValidLandingSpot(this.updatedPosition, ref ptr))
						{
							this._hasLanded = true;
							this.landedVelocity = ptr.velocity;
						}
						else if (ptr.hitLocation == HitLocation.Below)
						{
							FindGroundResult foundGround;
							this.FindGround(this.updatedPosition, out foundGround);
							ptr.isWalkable = foundGround.isWalkableGround;
							if (ptr.isWalkable)
							{
								this._foundGround = foundGround;
								this._hasLanded = true;
								this.landedVelocity = ptr.velocity;
							}
						}
						if (!this._hasLanded && ptr.hitLocation == HitLocation.Below)
						{
							this._foundGround.SetFromSweepResult(true, false, this.updatedPosition, ptr.point, ptr.normal, ptr.surfaceNormal, ptr.collider, ptr.hitResult.distance);
						}
					}
					iteration = this.SlideAlongSurface(iteration, inputDisplacement, ref this._velocity, ref vector, ref ptr, ref vector2);
				}
			}
			int maxMovementIterations = this._advanced.maxMovementIterations;
			CollisionResult collisionResult;
			while (this.detectCollisions && maxMovementIterations-- > 0 && vector.sqrMagnitude > this._advanced.minMoveDistanceSqr && this.MovementSweepTest(this.updatedPosition, this._velocity, vector, out collisionResult))
			{
				this.updatedPosition += collisionResult.displacementToHit;
				vector = collisionResult.remainingDisplacement;
				CollisionResult collisionResult2;
				if (this.isGrounded && !collisionResult.isWalkable && this.CanStepUp(collisionResult.collider) && this.StepUp(ref collisionResult, out collisionResult2))
				{
					this.updatedPosition = collisionResult2.position;
					vector = Vector3.zero;
					break;
				}
				if (this.isConstrainedToGround && !this.isOnWalkableGround)
				{
					if (this.IsValidLandingSpot(this.updatedPosition, ref collisionResult))
					{
						this._hasLanded = true;
						this.landedVelocity = collisionResult.velocity;
					}
					else if (this.ShouldCheckForValidLandingSpot(ref collisionResult))
					{
						FindGroundResult foundGround2;
						this.FindGround(this.updatedPosition, out foundGround2);
						collisionResult.isWalkable = foundGround2.isWalkableGround;
						if (collisionResult.isWalkable)
						{
							this._foundGround = foundGround2;
							this._hasLanded = true;
							this.landedVelocity = collisionResult.velocity;
						}
					}
					if (!this._hasLanded && collisionResult.hitLocation == HitLocation.Below)
					{
						float distance = collisionResult.hitResult.distance;
						Vector3 surfaceNormal = collisionResult.surfaceNormal;
						this._foundGround.SetFromSweepResult(true, false, this.updatedPosition, distance, ref collisionResult.hitResult, surfaceNormal);
					}
				}
				iteration = this.SlideAlongSurface(iteration, inputDisplacement, ref this._velocity, ref vector, ref collisionResult, ref vector2);
				this.AddCollisionResult(ref collisionResult);
			}
			if (vector.sqrMagnitude > this._advanced.minMoveDistanceSqr)
			{
				this.updatedPosition += vector;
			}
			if (this.isGrounded || this._hasLanded)
			{
				this._velocity = this._velocity.projectedOnPlane(this._characterUp).normalized * this._velocity.magnitude;
				this._velocity = this.ConstrainVectorToPlane(this._velocity);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007C90 File Offset: 0x00005E90
		private bool CanPerchOn(Collider otherCollider)
		{
			if (otherCollider == null)
			{
				return false;
			}
			if (this.collisionBehaviourCallback != null)
			{
				CollisionBehaviour behaviourFlags = this.collisionBehaviourCallback(otherCollider);
				if (CharacterMovement.CanPerchOn(behaviourFlags))
				{
					return true;
				}
				if (CharacterMovement.CanNotPerchOn(behaviourFlags))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007CD2 File Offset: 0x00005ED2
		private float GetPerchRadiusThreshold()
		{
			return Mathf.Max(0f, this._radius - this.perchOffset);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007CEB File Offset: 0x00005EEB
		private float GetValidPerchRadius(Collider otherCollider)
		{
			if (!this.CanPerchOn(otherCollider))
			{
				return 0.0011f;
			}
			return Mathf.Clamp(this._perchOffset, 0.0011f, this._radius);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007D14 File Offset: 0x00005F14
		private bool ShouldComputePerchResult(Vector3 characterPosition, ref RaycastHit inHit)
		{
			if (this.GetPerchRadiusThreshold() <= 0.0015f)
			{
				return false;
			}
			float sqrMagnitude = (inHit.point - characterPosition).projectedOnPlane(this._characterUp).sqrMagnitude;
			float validPerchRadius = this.GetValidPerchRadius(inHit.collider);
			return sqrMagnitude > validPerchRadius.square();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007D68 File Offset: 0x00005F68
		private bool CapsuleCast(Vector3 point1, Vector3 point2, float castRadius, Vector3 castDirection, float castDistance, int castLayerMask, out RaycastHit hitResult, out bool startPenetrating)
		{
			hitResult = default(RaycastHit);
			startPenetrating = false;
			int num = Physics.CapsuleCastNonAlloc(point1, point2, castRadius, castDirection, this._hits, castDistance, castLayerMask, this.triggerInteraction);
			if (num == 0)
			{
				return false;
			}
			float num2 = float.PositiveInfinity;
			int num3 = -1;
			for (int i = 0; i < num; i++)
			{
				ref RaycastHit ptr = ref this._hits[i];
				if (!this.ShouldFilter(ptr.collider))
				{
					if (ptr.distance <= 0f)
					{
						startPenetrating = true;
					}
					else if (ptr.distance < num2)
					{
						num2 = ptr.distance;
						num3 = i;
					}
				}
			}
			if (num3 != -1)
			{
				hitResult = this._hits[num3];
				return true;
			}
			return false;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007E14 File Offset: 0x00006014
		private bool BoxCast(Vector3 center, Vector3 halfExtents, Quaternion orientation, Vector3 castDirection, float castDistance, int castLayerMask, out RaycastHit hitResult, out bool startPenetrating)
		{
			hitResult = default(RaycastHit);
			startPenetrating = false;
			int num = Physics.BoxCastNonAlloc(center, halfExtents, castDirection, this._hits, orientation, castDistance, castLayerMask, this.triggerInteraction);
			if (num == 0)
			{
				return false;
			}
			float num2 = float.PositiveInfinity;
			int num3 = -1;
			for (int i = 0; i < num; i++)
			{
				ref RaycastHit ptr = ref this._hits[i];
				if (!this.ShouldFilter(ptr.collider))
				{
					if (ptr.distance <= 0f)
					{
						startPenetrating = true;
					}
					else if (ptr.distance < num2)
					{
						num2 = ptr.distance;
						num3 = i;
					}
				}
			}
			if (num3 != -1)
			{
				hitResult = this._hits[num3];
				return true;
			}
			return false;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007EC0 File Offset: 0x000060C0
		private bool GroundSweepTest(Vector3 characterPosition, float capsuleRadius, float capsuleHalfHeight, float sweepDistance, out RaycastHit hitResult, out bool startPenetrating)
		{
			bool flag;
			if (!this.useFlatBaseForGroundChecks)
			{
				Vector3 a = characterPosition + this._transformedCapsuleCenter;
				Vector3 point = a - this._characterUp * (capsuleHalfHeight - capsuleRadius);
				Vector3 point2 = a + this._characterUp * (capsuleHalfHeight - capsuleRadius);
				Vector3 castDirection = -1f * this._characterUp;
				flag = this.CapsuleCast(point, point2, capsuleRadius, castDirection, sweepDistance, this._collisionLayers, out hitResult, out startPenetrating);
			}
			else
			{
				Vector3 center = characterPosition + this._transformedCapsuleCenter;
				Vector3 halfExtents = new Vector3(capsuleRadius * 0.707f, capsuleHalfHeight, capsuleRadius * 0.707f);
				Quaternion quaternion = this.rotation * Quaternion.Euler(0f, -this.rotation.eulerAngles.y, 0f);
				Vector3 castDirection2 = -1f * this._characterUp;
				LayerMask collisionLayers = this._collisionLayers;
				flag = this.BoxCast(center, halfExtents, quaternion * Quaternion.Euler(0f, 45f, 0f), castDirection2, sweepDistance, collisionLayers, out hitResult, out startPenetrating);
				if (!flag && !startPenetrating)
				{
					flag = this.BoxCast(center, halfExtents, quaternion, castDirection2, sweepDistance, collisionLayers, out hitResult, out startPenetrating);
				}
			}
			return flag;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008008 File Offset: 0x00006208
		public void ComputeGroundDistance(Vector3 characterPosition, float sweepRadius, float sweepDistance, float castDistance, out FindGroundResult outGroundResult)
		{
			outGroundResult = default(FindGroundResult);
			if (sweepDistance < castDistance)
			{
				return;
			}
			float radius = this._radius;
			float num = this._height * 0.5f;
			bool flag = false;
			bool flag2 = false;
			if (sweepDistance > 0f && sweepRadius > 0f)
			{
				float num2 = (num - radius) * 0.100000024f;
				float capsuleHalfHeight = num - num2;
				float sweepDistance2 = sweepDistance + num2;
				RaycastHit raycastHit;
				flag = this.GroundSweepTest(characterPosition, sweepRadius, capsuleHalfHeight, sweepDistance2, out raycastHit, out flag2);
				if (flag || flag2)
				{
					if (flag2 || !this.IsWithinEdgeTolerance(characterPosition, raycastHit.point, sweepRadius))
					{
						num2 = (num - radius) * 0.9f;
						float num3 = Mathf.Max(0.0011f, sweepRadius - 0.0015f - 0.0001f);
						capsuleHalfHeight = Mathf.Max(num3, num - num2);
						sweepDistance2 = sweepDistance + num2;
						flag = this.GroundSweepTest(characterPosition, num3, capsuleHalfHeight, sweepDistance2, out raycastHit, out flag2);
					}
					if (flag && !flag2)
					{
						float num4 = Mathf.Max(-Mathf.Max(0.024f, radius), raycastHit.distance - num2);
						Vector3 a = -1f * this._characterUp;
						Vector3 position = characterPosition + a * num4;
						Vector3 vector = raycastHit.normal;
						bool isWalkable = false;
						bool flag3 = num4 <= sweepDistance && this.ComputeHitLocation(raycastHit.normal) == HitLocation.Below;
						if (flag3)
						{
							if (this.useFlatBaseForGroundChecks)
							{
								isWalkable = this.IsWalkable(raycastHit.collider, vector);
							}
							else
							{
								vector = this.FindGeomOpposingNormal(a * sweepDistance, ref raycastHit);
								isWalkable = this.IsWalkable(raycastHit.collider, vector);
							}
						}
						outGroundResult.SetFromSweepResult(flag3, isWalkable, position, num4, ref raycastHit, vector);
						if (outGroundResult.isWalkableGround)
						{
							return;
						}
					}
				}
			}
			if (!flag && !flag2)
			{
				return;
			}
			if (castDistance > 0f)
			{
				Vector3 origin = characterPosition + this._transformedCapsuleCenter;
				Vector3 direction = -1f * this._characterUp;
				float num5 = num;
				float distance = castDistance + num5;
				RaycastHit raycastHit2;
				flag = this.Raycast(origin, direction, distance, this._collisionLayers, out raycastHit2, 0f);
				if (flag && raycastHit2.distance > 0f)
				{
					float num6 = Mathf.Max(-Mathf.Max(0.024f, radius), raycastHit2.distance - num5);
					if (num6 <= castDistance && this.IsWalkable(raycastHit2.collider, raycastHit2.normal))
					{
						outGroundResult.SetFromRaycastResult(true, true, outGroundResult.position, outGroundResult.groundDistance, num6, ref raycastHit2);
						return;
					}
				}
			}
			outGroundResult.isWalkable = false;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008280 File Offset: 0x00006480
		private bool ComputePerchResult(Vector3 characterPosition, float testRadius, float inMaxGroundDistance, ref RaycastHit inHit, out FindGroundResult perchGroundResult)
		{
			perchGroundResult = default(FindGroundResult);
			if (inMaxGroundDistance <= 0f)
			{
				return false;
			}
			float num = Mathf.Max(0f, Vector3.Dot(inHit.point - characterPosition, this._characterUp));
			float castDistance = Mathf.Max(0f, inMaxGroundDistance - num);
			float sweepDistance = Mathf.Max(0f, inMaxGroundDistance) + this._radius;
			this.ComputeGroundDistance(characterPosition, testRadius, sweepDistance, castDistance, out perchGroundResult);
			if (!perchGroundResult.isWalkable)
			{
				return false;
			}
			if (num + perchGroundResult.groundDistance > inMaxGroundDistance)
			{
				perchGroundResult.isWalkable = false;
				return false;
			}
			return true;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008314 File Offset: 0x00006514
		public void FindGround(Vector3 characterPosition, out FindGroundResult outGroundResult)
		{
			if (!this._detectCollisions)
			{
				outGroundResult = default(FindGroundResult);
				return;
			}
			float num = this.isGrounded ? 0.0241f : -0.024f;
			float num2 = Mathf.Max(0.024f, this.stepOffset + num);
			this.ComputeGroundDistance(characterPosition, this._radius, num2, num2, out outGroundResult);
			if (outGroundResult.hitGround && !outGroundResult.isRaycastResult)
			{
				Vector3 position = outGroundResult.position;
				if (this.ShouldComputePerchResult(position, ref outGroundResult.hitResult))
				{
					float num3 = num2;
					if (this.isGrounded)
					{
						num3 += this.perchAdditionalHeight;
					}
					float validPerchRadius = this.GetValidPerchRadius(outGroundResult.collider);
					FindGroundResult findGroundResult;
					if (this.ComputePerchResult(position, validPerchRadius, num3, ref outGroundResult.hitResult, out findGroundResult))
					{
						if (0.021499999f - outGroundResult.groundDistance + findGroundResult.groundDistance >= num3)
						{
							outGroundResult.groundDistance = 0.021499999f;
						}
						if (!outGroundResult.isWalkableGround)
						{
							float groundDistance = outGroundResult.groundDistance;
							float castDistance = Mathf.Max(0.019f, groundDistance);
							outGroundResult.SetFromRaycastResult(true, true, outGroundResult.position, groundDistance, castDistance, ref findGroundResult.hitResult);
							return;
						}
					}
					else
					{
						outGroundResult.isWalkable = false;
					}
				}
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00008430 File Offset: 0x00006630
		private void AdjustGroundHeight()
		{
			if (!this._currentGround.isWalkableGround || !this.isConstrainedToGround)
			{
				return;
			}
			float num = this._currentGround.groundDistance;
			if (this._currentGround.isRaycastResult)
			{
				if (num < 0.019f && this._currentGround.raycastDistance >= 0.019f)
				{
					return;
				}
				num = this._currentGround.raycastDistance;
			}
			if (num < 0.019f || num > 0.024f)
			{
				float num2 = Vector3.Dot(this.updatedPosition, this._characterUp);
				float num3 = 0.021499999f - num;
				Vector3 b = this._characterUp * num3;
				Vector3 updatedPosition = this.updatedPosition;
				Vector3 normalized = b.normalized;
				float radius = this._radius;
				float magnitude = b.magnitude;
				int sweepLayerMask = this._collisionLayers;
				RaycastHit raycastHit;
				bool flag;
				Vector3 vector;
				float num4;
				if (!this.SweepTestEx(updatedPosition, radius, normalized, magnitude, sweepLayerMask, out raycastHit, out flag, out vector, out num4, true) && !flag)
				{
					this.updatedPosition += b;
					this._currentGround.groundDistance = this._currentGround.groundDistance + num3;
				}
				else if (num3 > 0f)
				{
					this.updatedPosition += normalized * raycastHit.distance;
					float num5 = Vector3.Dot(this.updatedPosition, this._characterUp);
					this._currentGround.groundDistance = this._currentGround.groundDistance + (num5 - num2);
				}
				else
				{
					this.updatedPosition += normalized * raycastHit.distance;
					float num6 = Vector3.Dot(this.updatedPosition, this._characterUp);
					this._currentGround.groundDistance = num6 - num2;
				}
			}
			if (this._rootTransform)
			{
				this._rootTransform.localPosition = this._rootTransformOffset - new Vector3(0f, 0.021499999f, 0f);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000860C File Offset: 0x0000680C
		private bool CanStepUp(Collider otherCollider)
		{
			if (otherCollider == null)
			{
				return false;
			}
			if (this.collisionBehaviourCallback != null)
			{
				CollisionBehaviour behaviourFlags = this.collisionBehaviourCallback(otherCollider);
				if (CharacterMovement.CanStepOn(behaviourFlags))
				{
					return true;
				}
				if (CharacterMovement.CanNotStepOn(behaviourFlags))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00008650 File Offset: 0x00006850
		private bool StepUp(ref CollisionResult inCollision, out CollisionResult stepResult)
		{
			stepResult = default(CollisionResult);
			if (inCollision.hitLocation == HitLocation.Above)
			{
				return false;
			}
			float num = Vector3.Dot(inCollision.position, this._characterUp);
			float num2 = num;
			float num3 = Mathf.Max(0f, this._currentGround.GetDistanceToGround());
			num -= num3;
			float num4 = Mathf.Max(0f, this.stepOffset - num3);
			float num5 = this.stepOffset + 0.048f;
			bool flag = !this.IsWithinEdgeTolerance(inCollision.position, inCollision.point, this._radius + 0.01f);
			if (!this._currentGround.isRaycastResult && !flag)
			{
				num2 = Vector3.Dot(this.groundPoint, this._characterUp);
			}
			else
			{
				num2 -= this._currentGround.groundDistance;
			}
			if (Vector3.Dot(inCollision.point, this._characterUp) <= num)
			{
				return false;
			}
			Vector3 vector = inCollision.position;
			Vector3 vector2 = this._characterUp;
			float radius = this._radius;
			float num6 = num4;
			int sweepLayerMask = this._collisionLayers;
			RaycastHit raycastHit;
			bool flag3;
			bool flag2 = this.SweepTest(vector, radius, vector2, num6, sweepLayerMask, out raycastHit, out flag3);
			if (flag3)
			{
				return false;
			}
			if (!flag2)
			{
				vector += vector2 * num6;
			}
			else
			{
				vector += vector2 * raycastHit.distance;
			}
			Vector3 remainingDisplacement = inCollision.remainingDisplacement;
			Vector3 vector3 = this.ConstrainVectorToPlane(Vector3.ProjectOnPlane(remainingDisplacement, this._characterUp));
			num6 = remainingDisplacement.magnitude;
			vector2 = vector3.normalized;
			flag2 = this.SweepTest(vector, radius, vector2, num6, sweepLayerMask, out raycastHit, out flag3);
			if (flag3)
			{
				return false;
			}
			if (flag2)
			{
				return false;
			}
			vector += vector2 * num6;
			vector2 = -this._characterUp;
			num6 = num5;
			flag2 = this.SweepTest(vector, radius, vector2, num6, sweepLayerMask, out raycastHit, out flag3);
			if (!flag2 || flag3)
			{
				return false;
			}
			float num7 = Vector3.Dot(raycastHit.point, this._characterUp) - num2;
			if (num7 > this.stepOffset)
			{
				return false;
			}
			Vector3 vector4 = vector + vector2 * raycastHit.distance;
			if (this.OverlapTest(vector4, this.updatedRotation, this._radius, this._height, this._collisionLayers, this._overlaps, this.triggerInteraction) > 0)
			{
				return false;
			}
			Vector3 vector5 = this.FindGeomOpposingNormal(vector2 * num6, ref raycastHit);
			if (!this.IsWalkable(raycastHit.collider, vector5))
			{
				if (Vector3.Dot(remainingDisplacement, vector5) < 0f)
				{
					return false;
				}
				if (Vector3.Dot(vector4, this._characterUp) > Vector3.Dot(inCollision.position, this._characterUp))
				{
					return false;
				}
			}
			if (!this.IsWithinEdgeTolerance(vector4, raycastHit.point, this._radius + 0.01f))
			{
				return false;
			}
			if (num7 > 0f && !this.CanStepUp(raycastHit.collider))
			{
				return false;
			}
			stepResult = new CollisionResult
			{
				position = vector4
			};
			return true;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008951 File Offset: 0x00006B51
		public void PauseGroundConstraint(float unconstrainedTime = 0.1f)
		{
			this._unconstrainedTimer = Mathf.Max(0f, unconstrainedTime);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00008964 File Offset: 0x00006B64
		private void UpdateCurrentGround(ref FindGroundResult inGroundResult)
		{
			this.wasOnGround = this.isOnGround;
			this.wasOnWalkableGround = this.isOnWalkableGround;
			this.wasGrounded = this.isGrounded;
			this._currentGround = inGroundResult;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008998 File Offset: 0x00006B98
		private int SlideAlongSurface(int iteration, Vector3 inputDisplacement, ref Vector3 displacement, ref CollisionResult inHit, ref Vector3 prevNormal)
		{
			inHit.normal = this.ComputeBlockingNormal(inHit.normal, inHit.isWalkable);
			if (inHit.isWalkable && this.isConstrainedToGround)
			{
				displacement = this.ComputeSlideVector(displacement, inHit.normal, true);
			}
			else
			{
				if (iteration == 0)
				{
					displacement = this.ComputeSlideVector(displacement, inHit.normal, inHit.isWalkable);
					iteration++;
				}
				else if (iteration == 1)
				{
					Vector3 vector = prevNormal.perpendicularTo(inHit.normal);
					Vector3 vector2 = inputDisplacement.projectedOnPlane(vector);
					Vector3 vector3 = this.ComputeSlideVector(displacement, inHit.normal, inHit.isWalkable);
					vector3 = vector3.projectedOnPlane(vector);
					if (vector2.dot(vector3) <= 0f || prevNormal.dot(inHit.normal) < 0f)
					{
						displacement = this.ConstrainVectorToPlane(displacement.projectedOn(vector));
						iteration++;
					}
					else
					{
						displacement = this.ComputeSlideVector(displacement, inHit.normal, inHit.isWalkable);
					}
				}
				else
				{
					displacement = Vector3.zero;
				}
				prevNormal = inHit.normal;
			}
			return iteration;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008AE4 File Offset: 0x00006CE4
		private void MoveAndSlide(Vector3 displacement)
		{
			Vector3 inputDisplacement = displacement;
			int iteration = 0;
			Vector3 vector = default(Vector3);
			int maxMovementIterations = this._advanced.maxMovementIterations;
			CollisionResult collisionResult;
			while (maxMovementIterations-- > 0 && displacement.sqrMagnitude > this._advanced.minMoveDistanceSqr && this.MovementSweepTest(this.updatedPosition, default(Vector3), displacement, out collisionResult))
			{
				this.updatedPosition += collisionResult.displacementToHit;
				displacement = collisionResult.remainingDisplacement;
				iteration = this.SlideAlongSurface(iteration, inputDisplacement, ref displacement, ref collisionResult, ref vector);
				this.AddCollisionResult(ref collisionResult);
			}
			if (displacement.sqrMagnitude > this._advanced.minMoveDistanceSqr)
			{
				this.updatedPosition += displacement;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00008BA0 File Offset: 0x00006DA0
		private bool CanRideOn(Collider otherCollider)
		{
			if (otherCollider == null)
			{
				return false;
			}
			if (this.collisionBehaviourCallback != null)
			{
				CollisionBehaviour behaviourFlags = this.collisionBehaviourCallback(otherCollider);
				if (CharacterMovement.CanRideOn(behaviourFlags) && otherCollider.attachedRigidbody)
				{
					return true;
				}
				if (CharacterMovement.CanNotRideOn(behaviourFlags) && otherCollider.attachedRigidbody)
				{
					return false;
				}
			}
			return otherCollider.attachedRigidbody;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00008C06 File Offset: 0x00006E06
		private void IgnoreCurrentPlatform(bool ignore)
		{
			this.IgnoreCollision(this._movingPlatform.platform, ignore);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008C1A File Offset: 0x00006E1A
		public void AttachTo(Rigidbody parent)
		{
			this._parentPlatform = parent;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008C24 File Offset: 0x00006E24
		private unsafe void UpdateCurrentPlatform()
		{
			this._lastVelocityOnMovingPlatform = Vector3.zero;
			this._movingPlatform.lastPlatform = this._movingPlatform.platform;
			if (this._parentPlatform)
			{
				this._movingPlatform.platform = this._parentPlatform;
			}
			else if (this.isGrounded && this.CanRideOn(this.groundCollider))
			{
				this._movingPlatform.platform = this.groundCollider.attachedRigidbody;
			}
			else
			{
				this._movingPlatform.platform = null;
			}
			if (this._movingPlatform.platform != null)
			{
				Transform transform = this._movingPlatform.platform.transform;
				this._movingPlatform.position = this.updatedPosition;
				this._movingPlatform.localPosition = transform.InverseTransformPoint(this.updatedPosition);
				this._movingPlatform.rotation = this.updatedRotation;
				this._movingPlatform.localRotation = Quaternion.Inverse(transform.rotation) * this.updatedRotation;
				this._lastVelocityOnMovingPlatform = *this.velocity;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00008D3C File Offset: 0x00006F3C
		private void UpdatePlatformMovement(float deltaTime)
		{
			Vector3 platformVelocity = this._movingPlatform.platformVelocity;
			if (!this._movingPlatform.platform)
			{
				this._movingPlatform.platformVelocity = Vector3.zero;
			}
			else
			{
				Transform transform = this._movingPlatform.platform.transform;
				Vector3 vector = transform.TransformPoint(this._movingPlatform.localPosition) - this._movingPlatform.position;
				this._movingPlatform.deltaPosition = vector;
				this._movingPlatform.platformVelocity = ((deltaTime > 0f) ? (vector / deltaTime) : Vector3.zero);
				if (this.impartPlatformRotation)
				{
					Quaternion quaternion = transform.rotation * this._movingPlatform.localRotation * Quaternion.Inverse(this._movingPlatform.rotation);
					this._movingPlatform.deltaRotation = quaternion;
					Vector3 normalized = Vector3.ProjectOnPlane(quaternion * this.updatedRotation * Vector3.forward, this._characterUp).normalized;
					this.updatedRotation = Quaternion.LookRotation(normalized, this._characterUp);
				}
			}
			if (this.impartPlatformMovement && this._movingPlatform.platformVelocity.sqrMagnitude > 0f)
			{
				if (this.fastPlatformMove)
				{
					this.updatedPosition += this._movingPlatform.platformVelocity * deltaTime;
				}
				else
				{
					this.IgnoreCurrentPlatform(true);
					this.MoveAndSlide(this._movingPlatform.platformVelocity * deltaTime);
					this.IgnoreCurrentPlatform(false);
				}
			}
			if (this.impartPlatformVelocity && this._movingPlatform.lastPlatform && this._movingPlatform.platform != this._movingPlatform.lastPlatform)
			{
				this._velocity -= this._movingPlatform.platformVelocity;
				this._velocity += platformVelocity;
			}
			if (this.impartPlatformVelocity && this._movingPlatform.lastPlatform == null && this._movingPlatform.platform)
			{
				this._velocity = this._lastVelocityOnMovingPlatform - this._movingPlatform.platformVelocity;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00008F7C File Offset: 0x0000717C
		private void ComputeDynamicCollisionResponse(ref CollisionResult inCollisionResult, out Vector3 characterImpulse, out Vector3 otherImpulse)
		{
			characterImpulse = default(Vector3);
			otherImpulse = default(Vector3);
			float num = 0f;
			Rigidbody rigidbody = inCollisionResult.rigidbody;
			CharacterMovement characterMovement;
			if (!rigidbody.isKinematic || rigidbody.TryGetComponent<CharacterMovement>(out characterMovement))
			{
				float mass = this.rigidbody.mass;
				num = mass / (mass + inCollisionResult.rigidbody.mass);
			}
			Vector3 normal = inCollisionResult.normal;
			float num2 = Vector3.Dot(inCollisionResult.velocity, normal);
			float num3 = Vector3.Dot(inCollisionResult.otherVelocity, normal);
			if (num2 < 0f)
			{
				characterImpulse += num2 * normal;
			}
			if (num3 > num2)
			{
				Vector3 a = (num3 - num2) * normal;
				characterImpulse += a * (1f - num);
				otherImpulse -= a * num;
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000905C File Offset: 0x0000725C
		private unsafe void ResolveDynamicCollisions()
		{
			if (!this.enablePhysicsInteraction)
			{
				return;
			}
			for (int i = 0; i < this._collisionCount; i++)
			{
				ref CollisionResult ptr = ref this._collisionResults[i];
				if (!ptr.isWalkable)
				{
					Rigidbody rigidbody = ptr.rigidbody;
					if (!(rigidbody == null))
					{
						Vector3 b;
						Vector3 a;
						this.ComputeDynamicCollisionResponse(ref ptr, out b, out a);
						CharacterMovement.CollisionResponseCallback collisionResponseCallback = this.collisionResponseCallback;
						if (collisionResponseCallback != null)
						{
							collisionResponseCallback(ref ptr, ref b, ref a);
						}
						CharacterMovement characterMovement;
						if (rigidbody.TryGetComponent<CharacterMovement>(out characterMovement))
						{
							if (this.physicsInteractionAffectsCharacters)
							{
								*this.velocity += b;
								*characterMovement.velocity += a * this.pushForceScale;
							}
						}
						else
						{
							this._velocity += b;
							if (!rigidbody.isKinematic)
							{
								rigidbody.AddForceAtPosition(a * this.pushForceScale, ptr.point, ForceMode.VelocityChange);
							}
						}
					}
				}
			}
			if (this.isGrounded)
			{
				this._velocity = this._velocity.projectedOnPlane(this._characterUp).normalized * this._velocity.magnitude;
			}
			this._velocity = this.ConstrainVectorToPlane(this._velocity);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000091A4 File Offset: 0x000073A4
		public void SetPosition(Vector3 newPosition, bool updateGround = false)
		{
			this.updatedPosition = newPosition;
			if (updateGround)
			{
				FindGroundResult findGroundResult;
				this.FindGround(this.updatedPosition, out findGroundResult);
				this.UpdateCurrentGround(ref findGroundResult);
				this.AdjustGroundHeight();
				this.UpdateCurrentPlatform();
			}
			bool activeSelf = base.gameObject.activeSelf;
			if (activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.rigidbody.position = this.updatedPosition;
			this.transform.position = this.updatedPosition;
			if (activeSelf)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009227 File Offset: 0x00007427
		public Vector3 GetPosition()
		{
			return this.transform.position;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009234 File Offset: 0x00007434
		public Vector3 GetFootPosition()
		{
			return this.transform.position - this.transform.up * 0.021499999f;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000925B File Offset: 0x0000745B
		public void SetRotation(Quaternion newRotation)
		{
			this.updatedRotation = newRotation;
			this.rigidbody.rotation = this.updatedRotation;
			this.transform.rotation = this.updatedRotation;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00009286 File Offset: 0x00007486
		public Quaternion GetRotation()
		{
			return this.transform.rotation;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009294 File Offset: 0x00007494
		public void SetPositionAndRotation(Vector3 newPosition, Quaternion newRotation, bool updateGround = false)
		{
			this.updatedPosition = newPosition;
			this.updatedRotation = newRotation;
			if (updateGround)
			{
				FindGroundResult findGroundResult;
				this.FindGround(this.updatedPosition, out findGroundResult);
				this.UpdateCurrentGround(ref findGroundResult);
				this.AdjustGroundHeight();
				this.UpdateCurrentPlatform();
			}
			this.rigidbody.position = this.updatedPosition;
			this.rigidbody.rotation = this.updatedRotation;
			this.transform.SetPositionAndRotation(this.updatedPosition, this.updatedRotation);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00009310 File Offset: 0x00007510
		public void RotateTowards(Vector3 worldDirection, float maxDegreesDelta, bool updateYawOnly = true)
		{
			Vector3 up = this.transform.up;
			if (updateYawOnly)
			{
				worldDirection = worldDirection.projectedOnPlane(up);
			}
			if (worldDirection == Vector3.zero)
			{
				return;
			}
			Quaternion to = Quaternion.LookRotation(worldDirection, up);
			this.rotation = Quaternion.RotateTowards(this.rotation, to, maxDegreesDelta);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009360 File Offset: 0x00007560
		private void UpdateCachedFields()
		{
			this._hasLanded = false;
			this._foundGround = default(FindGroundResult);
			this.updatedPosition = this.transform.position;
			this.updatedRotation = this.transform.rotation;
			this._characterUp = this.updatedRotation * Vector3.up;
			this._transformedCapsuleCenter = this.updatedRotation * this._capsuleCenter;
			this._transformedCapsuleTopCenter = this.updatedRotation * this._capsuleTopCenter;
			this._transformedCapsuleBottomCenter = this.updatedRotation * this._capsuleBottomCenter;
			this.ResetCollisionFlags();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009403 File Offset: 0x00007603
		public void ClearAccumulatedForces()
		{
			this._pendingForces = Vector3.zero;
			this._pendingImpulses = Vector3.zero;
			this._pendingLaunchVelocity = Vector3.zero;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009428 File Offset: 0x00007628
		public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
		{
			switch (forceMode)
			{
			case ForceMode.Force:
				this._pendingForces += force / this.rigidbody.mass;
				return;
			case ForceMode.Impulse:
				this._pendingImpulses += force / this.rigidbody.mass;
				return;
			case ForceMode.VelocityChange:
				this._pendingImpulses += force;
				break;
			case (ForceMode)3:
			case (ForceMode)4:
				break;
			case ForceMode.Acceleration:
				this._pendingForces += force;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000094C0 File Offset: 0x000076C0
		public void AddExplosionForce(float strength, Vector3 origin, float radius, float upwardModifier, ForceMode forceMode = ForceMode.Force)
		{
			Vector3 vector = this.worldCenter - origin;
			float magnitude = vector.magnitude;
			if (magnitude > radius)
			{
				return;
			}
			Vector3 normalized = vector.normalized;
			float num = 1f - Mathf.Clamp01(magnitude / radius);
			Vector3 vector2 = normalized * (strength * num);
			if (upwardModifier != 0f)
			{
				vector2 += Vector3.up * (upwardModifier * num);
			}
			this.AddForce(vector2, forceMode);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009530 File Offset: 0x00007730
		public void LaunchCharacter(Vector3 launchVelocity, bool overrideVerticalVelocity = false, bool overrideLateralVelocity = false)
		{
			Vector3 vector = launchVelocity;
			Vector3 up = this.transform.up;
			if (!overrideLateralVelocity)
			{
				vector += this._velocity.projectedOnPlane(up);
			}
			if (!overrideVerticalVelocity)
			{
				vector += this._velocity.projectedOn(up);
			}
			this._pendingLaunchVelocity = vector;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00009580 File Offset: 0x00007780
		private void UpdateVelocity(Vector3 newVelocity, float deltaTime)
		{
			this._velocity = newVelocity;
			this._velocity += this._pendingForces * deltaTime;
			this._velocity += this._pendingImpulses;
			if (this._pendingLaunchVelocity.sqrMagnitude > 0f)
			{
				this._velocity = this._pendingLaunchVelocity;
			}
			this.ClearAccumulatedForces();
			this._velocity = this.ConstrainVectorToPlane(this._velocity);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00009600 File Offset: 0x00007800
		public CollisionFlags Move(Vector3 newVelocity, float deltaTime)
		{
			this.UpdateCachedFields();
			this.ClearCollisionResults();
			this.UpdateVelocity(newVelocity, deltaTime);
			this.UpdatePlatformMovement(deltaTime);
			this.PerformMovement(deltaTime);
			if (this.isGrounded || this._hasLanded)
			{
				this.FindGround(this.updatedPosition, out this._foundGround);
			}
			this.UpdateCurrentGround(ref this._foundGround);
			if (this._unconstrainedTimer > 0f)
			{
				this._unconstrainedTimer -= deltaTime;
				if (this._unconstrainedTimer <= 0f)
				{
					this._unconstrainedTimer = 0f;
				}
			}
			this.AdjustGroundHeight();
			this.UpdateCurrentPlatform();
			this.ResolveDynamicCollisions();
			this.SetPositionAndRotation(this.updatedPosition, this.updatedRotation, false);
			this.OnCollided();
			if (!this.wasOnWalkableGround && this.isOnGround)
			{
				this.OnFoundGround();
			}
			return this.collisionFlags;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000096D7 File Offset: 0x000078D7
		public CollisionFlags Move(float deltaTime)
		{
			return this.Move(this._velocity, deltaTime);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000096E8 File Offset: 0x000078E8
		public unsafe CollisionFlags SimpleMove(Vector3 desiredVelocity, float maxSpeed, float acceleration, float deceleration, float friction, float brakingFriction, Vector3 gravity, bool onlyHorizontal, float deltaTime)
		{
			if (this.isGrounded)
			{
				*this.velocity = CharacterMovement.CalcVelocity(*this.velocity, desiredVelocity, maxSpeed, acceleration, deceleration, friction, brakingFriction, deltaTime);
			}
			else
			{
				Vector3 planeNormal = -1f * gravity.normalized;
				Vector3 vector = onlyHorizontal ? (*this.velocity).projectedOnPlane(planeNormal) : (*this.velocity);
				if (onlyHorizontal)
				{
					desiredVelocity = desiredVelocity.projectedOnPlane(planeNormal);
				}
				if (this.isOnGround)
				{
					Vector3 vector2 = this.groundNormal;
					if (desiredVelocity.dot(vector2) < 0f)
					{
						vector2 = vector2.projectedOnPlane(planeNormal).normalized;
						desiredVelocity = desiredVelocity.projectedOnPlane(vector2);
					}
				}
				vector = CharacterMovement.CalcVelocity(vector, desiredVelocity, maxSpeed, acceleration, deceleration, friction, brakingFriction, deltaTime);
				if (onlyHorizontal)
				{
					*this.velocity += Vector3.ProjectOnPlane(vector - *this.velocity, planeNormal);
				}
				else
				{
					*this.velocity += vector - *this.velocity;
				}
				*this.velocity += gravity * deltaTime;
			}
			return this.Move(deltaTime);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009834 File Offset: 0x00007A34
		[ContextMenu("Init Collision Layers from Collision Matrix")]
		private void InitCollisionMask()
		{
			int layer = base.gameObject.layer;
			this._collisionLayers = 0;
			for (int i = 0; i < 32; i++)
			{
				if (!Physics.GetIgnoreLayerCollision(layer, i))
				{
					this._collisionLayers |= 1 << i;
				}
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000988C File Offset: 0x00007A8C
		public void SetState(Vector3 inPosition, Quaternion inRotation, Vector3 inVelocity, bool inConstrainedToGround, float inUnconstrainedTimer, bool inHitGround, bool inIsWalkable)
		{
			this._velocity = inVelocity;
			this._isConstrainedToGround = inConstrainedToGround;
			this._unconstrainedTimer = Mathf.Max(0f, inUnconstrainedTimer);
			this._currentGround.hitGround = inHitGround;
			this._currentGround.isWalkable = inIsWalkable;
			this.SetPositionAndRotation(inPosition, inRotation, this.isGrounded);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000098E4 File Offset: 0x00007AE4
		private void Reset()
		{
			this.SetDimensions(0.5f, 2f);
			this.SetPlaneConstraint(PlaneConstraint.None, Vector3.zero);
			this._slopeLimit = 45f;
			this._stepOffset = 0.45f;
			this._perchOffset = 0.5f;
			this._perchAdditionalHeight = 0.4f;
			this._triggerInteraction = QueryTriggerInteraction.Ignore;
			this._advanced.Reset();
			this._isConstrainedToGround = true;
			this._pushForceScale = 1f;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009960 File Offset: 0x00007B60
		private void OnValidate()
		{
			this.SetDimensions(this._radius, this._height);
			this.SetPlaneConstraint(this._planeConstraint, this._constraintPlaneNormal);
			this.slopeLimit = this._slopeLimit;
			this.stepOffset = this._stepOffset;
			this.perchOffset = this._perchOffset;
			this.perchAdditionalHeight = this._perchAdditionalHeight;
			this._advanced.OnValidate();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000099CC File Offset: 0x00007BCC
		private void Awake()
		{
			this.CacheComponents();
			this.SetDimensions(this._radius, this._height);
			this.SetPlaneConstraint(this._planeConstraint, this._constraintPlaneNormal);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000099F8 File Offset: 0x00007BF8
		private void OnEnable()
		{
			this.updatedPosition = this.transform.position;
			this.updatedRotation = this.transform.rotation;
			this.UpdateCachedFields();
		}

		// Token: 0x04000081 RID: 129
		private const float kKindaSmallNumber = 0.0001f;

		// Token: 0x04000082 RID: 130
		private const float kHemisphereLimit = 0.01f;

		// Token: 0x04000083 RID: 131
		private const int kMaxCollisionCount = 16;

		// Token: 0x04000084 RID: 132
		private const int kMaxOverlapCount = 16;

		// Token: 0x04000085 RID: 133
		private const float kSweepEdgeRejectDistance = 0.0015f;

		// Token: 0x04000086 RID: 134
		private const float kMinGroundDistance = 0.019f;

		// Token: 0x04000087 RID: 135
		private const float kMaxGroundDistance = 0.024f;

		// Token: 0x04000088 RID: 136
		private const float kAvgGroundDistance = 0.021499999f;

		// Token: 0x04000089 RID: 137
		private const float kMinWalkableSlopeLimit = 1f;

		// Token: 0x0400008A RID: 138
		private const float kMaxWalkableSlopeLimit = 0.017452f;

		// Token: 0x0400008B RID: 139
		private const float kPenetrationOffset = 0.00125f;

		// Token: 0x0400008C RID: 140
		private const float kContactOffset = 0.01f;

		// Token: 0x0400008D RID: 141
		private const float kSmallContactOffset = 0.001f;

		// Token: 0x0400008E RID: 142
		[Space(15f)]
		[Tooltip("Allow to constrain the Character so movement along the locked axis is not possible.")]
		[SerializeField]
		private PlaneConstraint _planeConstraint;

		// Token: 0x0400008F RID: 143
		[Space(15f)]
		[SerializeField]
		[Tooltip("The root transform in the avatar.")]
		private Transform _rootTransform;

		// Token: 0x04000090 RID: 144
		[SerializeField]
		[Tooltip("The root transform will be positioned at this offset from foot position.")]
		private Vector3 _rootTransformOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000091 RID: 145
		[Space(15f)]
		[Tooltip("The Character's capsule collider radius.")]
		[SerializeField]
		private float _radius;

		// Token: 0x04000092 RID: 146
		[Tooltip("The Character's capsule collider height")]
		[SerializeField]
		private float _height;

		// Token: 0x04000093 RID: 147
		[Space(15f)]
		[Tooltip("The maximum angle (in degrees) for a walkable surface.")]
		[SerializeField]
		private float _slopeLimit;

		// Token: 0x04000094 RID: 148
		[Tooltip("The maximum height (in meters) for a valid step.")]
		[SerializeField]
		private float _stepOffset;

		// Token: 0x04000095 RID: 149
		[Tooltip("Allow a Character to perch on the edge of a surface if the horizontal distance from the Character's position to the edge is closer than this.\nNote that characters will not fall off if they are within stepOffset of a walkable surface below.")]
		[SerializeField]
		private float _perchOffset;

		// Token: 0x04000096 RID: 150
		[Tooltip("When perching on a ledge, add this additional distance to stepOffset when determining how high above a walkable ground we can perch.\nNote that we still enforce stepOffset to start the step up, this just allows the Character to hang off the edge or step slightly higher off the ground.")]
		[SerializeField]
		private float _perchAdditionalHeight;

		// Token: 0x04000097 RID: 151
		[Space(15f)]
		[Tooltip("If enabled, colliders with SlopeLimitBehaviour component will be able to override this slope limit.")]
		[SerializeField]
		private bool _slopeLimitOverride;

		// Token: 0x04000098 RID: 152
		[Tooltip("When enabled, will treat head collisions as if the character is using a shape with a flat top.")]
		[SerializeField]
		private bool _useFlatTop;

		// Token: 0x04000099 RID: 153
		[Tooltip("Performs ground checks as if the character is using a shape with a flat base.This avoids the situation where characters slowly lower off the side of a ledge (as their capsule 'balances' on the edge).")]
		[SerializeField]
		private bool _useFlatBaseForGroundChecks;

		// Token: 0x0400009A RID: 154
		[Space(15f)]
		[Tooltip("Character collision layers mask.")]
		[SerializeField]
		private LayerMask _collisionLayers = 1;

		// Token: 0x0400009B RID: 155
		[Tooltip("Overrides the global Physics.queriesHitTriggers to specify whether queries (raycast, spherecast, overlap tests, etc.) hit Triggers by default. Use Ignore for queries to ignore trigger Colliders.")]
		[SerializeField]
		private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

		// Token: 0x0400009C RID: 156
		[Space(15f)]
		[SerializeField]
		private CharacterMovement.Advanced _advanced;

		// Token: 0x0400009D RID: 157
		private Transform _transform;

		// Token: 0x0400009E RID: 158
		private Rigidbody _rigidbody;

		// Token: 0x0400009F RID: 159
		private CapsuleCollider _capsuleCollider;

		// Token: 0x040000A0 RID: 160
		private Vector3 _capsuleCenter;

		// Token: 0x040000A1 RID: 161
		private Vector3 _capsuleTopCenter;

		// Token: 0x040000A2 RID: 162
		private Vector3 _capsuleBottomCenter;

		// Token: 0x040000A3 RID: 163
		private readonly HashSet<Rigidbody> _ignoredRigidbodies = new HashSet<Rigidbody>();

		// Token: 0x040000A4 RID: 164
		private readonly HashSet<Collider> _ignoredColliders = new HashSet<Collider>();

		// Token: 0x040000A5 RID: 165
		private readonly RaycastHit[] _hits = new RaycastHit[16];

		// Token: 0x040000A6 RID: 166
		private readonly Collider[] _overlaps = new Collider[16];

		// Token: 0x040000A7 RID: 167
		private int _collisionCount;

		// Token: 0x040000A8 RID: 168
		private readonly CollisionResult[] _collisionResults = new CollisionResult[16];

		// Token: 0x040000A9 RID: 169
		[SerializeField]
		[HideInInspector]
		private float _minSlopeLimit;

		// Token: 0x040000AA RID: 170
		private bool _detectCollisions = true;

		// Token: 0x040000AB RID: 171
		private bool _isConstrainedToGround = true;

		// Token: 0x040000AC RID: 172
		private float _unconstrainedTimer;

		// Token: 0x040000AD RID: 173
		private Vector3 _constraintPlaneNormal;

		// Token: 0x040000AE RID: 174
		private Vector3 _characterUp;

		// Token: 0x040000AF RID: 175
		private Vector3 _transformedCapsuleCenter;

		// Token: 0x040000B0 RID: 176
		private Vector3 _transformedCapsuleTopCenter;

		// Token: 0x040000B1 RID: 177
		private Vector3 _transformedCapsuleBottomCenter;

		// Token: 0x040000B2 RID: 178
		private Vector3 _velocity;

		// Token: 0x040000B3 RID: 179
		private Vector3 _pendingForces;

		// Token: 0x040000B4 RID: 180
		private Vector3 _pendingImpulses;

		// Token: 0x040000B5 RID: 181
		private Vector3 _pendingLaunchVelocity;

		// Token: 0x040000B6 RID: 182
		private float _pushForceScale = 1f;

		// Token: 0x040000B7 RID: 183
		private bool _hasLanded;

		// Token: 0x040000B8 RID: 184
		private FindGroundResult _foundGround;

		// Token: 0x040000B9 RID: 185
		private FindGroundResult _currentGround;

		// Token: 0x040000BA RID: 186
		private Rigidbody _parentPlatform;

		// Token: 0x040000BB RID: 187
		private CharacterMovement.MovingPlatform _movingPlatform;

		// Token: 0x040000BC RID: 188
		private Vector3 _lastVelocityOnMovingPlatform;

		// Token: 0x02000027 RID: 39
		[Flags]
		private enum DepenetrationBehaviour
		{
			// Token: 0x04000102 RID: 258
			IgnoreNone = 0,
			// Token: 0x04000103 RID: 259
			IgnoreStatic = 1,
			// Token: 0x04000104 RID: 260
			IgnoreDynamic = 2,
			// Token: 0x04000105 RID: 261
			IgnoreKinematic = 4
		}

		// Token: 0x02000028 RID: 40
		[Serializable]
		public struct Advanced
		{
			// Token: 0x17000099 RID: 153
			// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000A8B8 File Offset: 0x00008AB8
			public float minMoveDistanceSqr
			{
				get
				{
					return this.minMoveDistance * this.minMoveDistance;
				}
			}

			// Token: 0x060002AA RID: 682 RVA: 0x0000A8C7 File Offset: 0x00008AC7
			public void Reset()
			{
				this.minMoveDistance = 0f;
				this.maxMovementIterations = 5;
				this.maxDepenetrationIterations = 1;
				this.enablePhysicsInteraction = false;
				this.allowPushCharacters = false;
				this.impartPlatformMovement = false;
				this.impartPlatformRotation = false;
				this.impartPlatformVelocity = false;
			}

			// Token: 0x060002AB RID: 683 RVA: 0x0000A905 File Offset: 0x00008B05
			public void OnValidate()
			{
				this.minMoveDistance = Mathf.Max(this.minMoveDistance, 0f);
				this.maxMovementIterations = Mathf.Max(this.maxMovementIterations, 1);
				this.maxDepenetrationIterations = Mathf.Max(this.maxDepenetrationIterations, 1);
			}

			// Token: 0x04000106 RID: 262
			[Tooltip("The minimum move distance of the character controller. If the character tries to move less than this distance, it will not move at all. This can be used to reduce jitter. In most situations this value should be left at 0.")]
			public float minMoveDistance;

			// Token: 0x04000107 RID: 263
			[Tooltip("Max number of iterations used during movement.")]
			public int maxMovementIterations;

			// Token: 0x04000108 RID: 264
			[Tooltip("Max number of iterations used to resolve penetrations.")]
			public int maxDepenetrationIterations;

			// Token: 0x04000109 RID: 265
			[Tooltip("When enable, FindGeomOpposingNormal will use a faster path (approximation) sacrificing accuracy.")]
			public bool useFastGeomNormalPath;

			// Token: 0x0400010A RID: 266
			[Space(15f)]
			[Tooltip("If enabled, the character will interact with dynamic rigidbodies when walking into them.")]
			public bool enablePhysicsInteraction;

			// Token: 0x0400010B RID: 267
			[Tooltip("If enabled, the character will interact with other characters when walking into them.")]
			public bool allowPushCharacters;

			// Token: 0x0400010C RID: 268
			[Tooltip("If enabled, the character will move with the moving platform it is standing on.")]
			public bool impartPlatformMovement;

			// Token: 0x0400010D RID: 269
			[Tooltip("If enabled, the character will rotate (yaw-only) with the moving platform it is standing on.")]
			public bool impartPlatformRotation;

			// Token: 0x0400010E RID: 270
			[Tooltip("If enabled, impart the platform's velocity when jumping or falling off it.")]
			public bool impartPlatformVelocity;
		}

		// Token: 0x02000029 RID: 41
		public struct MovingPlatform
		{
			// Token: 0x0400010F RID: 271
			public Rigidbody lastPlatform;

			// Token: 0x04000110 RID: 272
			public Rigidbody platform;

			// Token: 0x04000111 RID: 273
			public Vector3 position;

			// Token: 0x04000112 RID: 274
			public Vector3 localPosition;

			// Token: 0x04000113 RID: 275
			public Vector3 deltaPosition;

			// Token: 0x04000114 RID: 276
			public Quaternion rotation;

			// Token: 0x04000115 RID: 277
			public Quaternion localRotation;

			// Token: 0x04000116 RID: 278
			public Quaternion deltaRotation;

			// Token: 0x04000117 RID: 279
			public Vector3 platformVelocity;
		}

		// Token: 0x0200002A RID: 42
		// (Invoke) Token: 0x060002AD RID: 685
		public delegate bool ColliderFilterCallback(Collider collider);

		// Token: 0x0200002B RID: 43
		// (Invoke) Token: 0x060002B1 RID: 689
		public delegate CollisionBehaviour CollisionBehaviourCallback(Collider collider);

		// Token: 0x0200002C RID: 44
		// (Invoke) Token: 0x060002B5 RID: 693
		public delegate void CollisionResponseCallback(ref CollisionResult inCollisionResult, ref Vector3 characterImpulse, ref Vector3 otherImpulse);

		// Token: 0x0200002D RID: 45
		// (Invoke) Token: 0x060002B9 RID: 697
		public delegate void CollidedEventHandler(ref CollisionResult collisionResult);

		// Token: 0x0200002E RID: 46
		// (Invoke) Token: 0x060002BD RID: 701
		public delegate void FoundGroundEventHandler(ref FindGroundResult foundGround);
	}
}
