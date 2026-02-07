using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x0200000D RID: 13
	[RequireComponent(typeof(BoxCollider))]
	public class PhysicsVolume : MonoBehaviour
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009FCD File Offset: 0x000081CD
		public BoxCollider boxCollider
		{
			get
			{
				if (this._collider == null)
				{
					this._collider = base.GetComponent<BoxCollider>();
				}
				return this._collider;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00009FEF File Offset: 0x000081EF
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00009FF7 File Offset: 0x000081F7
		public int priority
		{
			get
			{
				return this._priority;
			}
			set
			{
				this._priority = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000A000 File Offset: 0x00008200
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000A008 File Offset: 0x00008208
		public float friction
		{
			get
			{
				return this._friction;
			}
			set
			{
				this._friction = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000A01B File Offset: 0x0000821B
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000A023 File Offset: 0x00008223
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000A036 File Offset: 0x00008236
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000A03E File Offset: 0x0000823E
		public bool waterVolume
		{
			get
			{
				return this._waterVolume;
			}
			set
			{
				this._waterVolume = value;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000A047 File Offset: 0x00008247
		protected virtual void OnReset()
		{
			this.priority = 0;
			this.friction = 0.5f;
			this.maxFallSpeed = 40f;
			this.waterVolume = true;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A06D File Offset: 0x0000826D
		protected virtual void OnOnValidate()
		{
			this.friction = this._friction;
			this.maxFallSpeed = this._maxFallSpeed;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A087 File Offset: 0x00008287
		protected virtual void OnAwake()
		{
			this.boxCollider.isTrigger = true;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A095 File Offset: 0x00008295
		private void Reset()
		{
			this.OnReset();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A09D File Offset: 0x0000829D
		private void OnValidate()
		{
			this.OnOnValidate();
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000A0A5 File Offset: 0x000082A5
		private void Awake()
		{
			this.OnAwake();
		}

		// Token: 0x040000D0 RID: 208
		[Tooltip("Determines which PhysicsVolume takes precedence if they overlap (higher value == higher priority).")]
		[SerializeField]
		private int _priority;

		// Token: 0x040000D1 RID: 209
		[Tooltip("Determines the amount of friction applied by the volume as Character using CharacterMovement moves through it.\nThe higher this value, the harder it will feel to move through the volume.")]
		[SerializeField]
		private float _friction;

		// Token: 0x040000D2 RID: 210
		[Tooltip("Determines the terminal velocity of Characters using CharacterMovement when falling.")]
		[SerializeField]
		private float _maxFallSpeed;

		// Token: 0x040000D3 RID: 211
		[Tooltip("Determines if the volume contains a fluid, like water.")]
		[SerializeField]
		private bool _waterVolume;

		// Token: 0x040000D4 RID: 212
		private BoxCollider _collider;
	}
}
