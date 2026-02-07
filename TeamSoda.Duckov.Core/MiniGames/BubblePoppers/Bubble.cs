using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002E8 RID: 744
	public class Bubble : MiniGameBehaviour
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0005727D File Offset: 0x0005547D
		// (set) Token: 0x060017B1 RID: 6065 RVA: 0x00057285 File Offset: 0x00055485
		public BubblePopper Master { get; private set; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0005728E File Offset: 0x0005548E
		public float Radius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x00057296 File Offset: 0x00055496
		public int ColorIndex
		{
			get
			{
				return this.colorIndex;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x0005729E File Offset: 0x0005549E
		public Color DisplayColor
		{
			get
			{
				if (this.Master == null)
				{
					return Color.white;
				}
				return this.Master.GetDisplayColor(this.ColorIndex);
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x000572C5 File Offset: 0x000554C5
		// (set) Token: 0x060017B6 RID: 6070 RVA: 0x000572CD File Offset: 0x000554CD
		public Vector2Int Coord { get; internal set; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x000572D6 File Offset: 0x000554D6
		// (set) Token: 0x060017B8 RID: 6072 RVA: 0x000572DE File Offset: 0x000554DE
		public Vector2 MoveDirection { get; internal set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x000572E7 File Offset: 0x000554E7
		// (set) Token: 0x060017BA RID: 6074 RVA: 0x000572EF File Offset: 0x000554EF
		public Vector2 Velocity { get; internal set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x000572F8 File Offset: 0x000554F8
		// (set) Token: 0x060017BC RID: 6076 RVA: 0x00057300 File Offset: 0x00055500
		public Bubble.Status status { get; private set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x00057309 File Offset: 0x00055509
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x0005731B File Offset: 0x0005551B
		private Vector2 gPos
		{
			get
			{
				return this.graphicsRoot.localPosition;
			}
			set
			{
				this.graphicsRoot.localPosition = value;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x00057330 File Offset: 0x00055530
		private Vector2 gForce
		{
			get
			{
				return (new Vector2(Mathf.PerlinNoise(7.3f, Time.time * this.vibrationFrequency) * 2f - 1f, Mathf.PerlinNoise(0.3f, Time.time * this.vibrationFrequency) * 2f - 1f) * this.vibrationAmp - this.gPos) * this.gSpring;
			}
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000573A7 File Offset: 0x000555A7
		internal void Setup(BubblePopper master, int colorIndex)
		{
			this.Master = master;
			this.colorIndex = colorIndex;
			this.RefreshColor();
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000573BD File Offset: 0x000555BD
		public void RefreshColor()
		{
			this.image.color = this.DisplayColor;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x000573D0 File Offset: 0x000555D0
		internal void Launch(Vector2 direction)
		{
			this.MoveDirection = direction;
			this.status = Bubble.Status.Moving;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x000573E0 File Offset: 0x000555E0
		internal void NotifyExplode(Vector2Int origin)
		{
			this.status = Bubble.Status.Explode;
			Vector2Int vector2Int = this.Coord - origin;
			float magnitude = vector2Int.magnitude;
			this.explodeETA = magnitude * 0.025f;
			this.Impact(vector2Int.normalized * 5f);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00057434 File Offset: 0x00055634
		internal void NotifyAttached(Vector2Int coord)
		{
			Vector2 vector = this.Master.Layout.CoordToLocalPosition(coord);
			base.transform.position = this.Master.Layout.transform.localToWorldMatrix.MultiplyPoint(vector);
			this.status = Bubble.Status.Attached;
			this.Coord = coord;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0005748F File Offset: 0x0005568F
		public void NotifyDetached()
		{
			this.status = Bubble.Status.Detached;
			this.Velocity = Vector2.zero;
			this.explodeCountDown = this.explodeAfterDetachedFor;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000574AF File Offset: 0x000556AF
		protected override void OnUpdate(float deltaTime)
		{
			this.UpdateLogic(deltaTime);
			this.UpdateGraphics(deltaTime);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x000574BF File Offset: 0x000556BF
		private void UpdateLogic(float deltaTime)
		{
			if (this.Master == null)
			{
				return;
			}
			if (this.Master.Busy)
			{
				return;
			}
			if (this.status == Bubble.Status.Moving)
			{
				this.Master.MoveBubble(this, deltaTime);
			}
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000574F4 File Offset: 0x000556F4
		private void UpdateGraphics(float deltaTime)
		{
			if (this.status == Bubble.Status.Explode)
			{
				this.explodeETA -= deltaTime;
				if (this.explodeETA <= 0f)
				{
					FXPool.Play(this.explodeFXrefab, base.transform.position, base.transform.rotation, this.DisplayColor);
					this.Master.Release(this);
				}
			}
			if (this.status == Bubble.Status.Detached)
			{
				base.transform.localPosition += this.Velocity * deltaTime;
				this.Velocity += -Vector2.up * this.gravity;
				this.explodeCountDown -= deltaTime;
				if (this.explodeCountDown <= 0f)
				{
					this.NotifyExplode(this.Coord);
				}
			}
			this.UpdateElasticMovement(deltaTime);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000575E0 File Offset: 0x000557E0
		private void UpdateElasticMovement(float deltaTime)
		{
			float num = (Vector2.Dot(this.gVelocity, this.gForce.normalized) < 0f) ? this.gDamping : 1f;
			this.gVelocity += this.gForce * deltaTime;
			this.gVelocity = Vector2.MoveTowards(this.gVelocity, Vector2.zero, num * this.gVelocity.magnitude * deltaTime);
			this.gPos += this.gVelocity;
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00057674 File Offset: 0x00055874
		public void Impact(Vector2 velocity)
		{
			this.gVelocity = velocity;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0005767D File Offset: 0x0005587D
		internal void Rest()
		{
			this.gPos = Vector2.zero;
			this.gVelocity = Vector2.zero;
		}

		// Token: 0x04001158 RID: 4440
		[SerializeField]
		private float radius;

		// Token: 0x04001159 RID: 4441
		[SerializeField]
		private int colorIndex;

		// Token: 0x0400115A RID: 4442
		[SerializeField]
		private float gravity;

		// Token: 0x0400115B RID: 4443
		[SerializeField]
		private float explodeAfterDetachedFor = 1f;

		// Token: 0x0400115C RID: 4444
		[SerializeField]
		private ParticleSystem explodeFXrefab;

		// Token: 0x0400115D RID: 4445
		[SerializeField]
		private Image image;

		// Token: 0x0400115E RID: 4446
		[SerializeField]
		private RectTransform graphicsRoot;

		// Token: 0x0400115F RID: 4447
		[SerializeField]
		private float gSpring = 1f;

		// Token: 0x04001160 RID: 4448
		[SerializeField]
		private float gDamping = 10f;

		// Token: 0x04001161 RID: 4449
		[SerializeField]
		private float vibrationFrequency = 10f;

		// Token: 0x04001162 RID: 4450
		[SerializeField]
		private float vibrationAmp = 4f;

		// Token: 0x04001167 RID: 4455
		private float explodeETA;

		// Token: 0x04001168 RID: 4456
		private float explodeCountDown;

		// Token: 0x04001169 RID: 4457
		private Vector2 gVelocity;

		// Token: 0x02000596 RID: 1430
		public enum Status
		{
			// Token: 0x0400206C RID: 8300
			Idle,
			// Token: 0x0400206D RID: 8301
			Moving,
			// Token: 0x0400206E RID: 8302
			Attached,
			// Token: 0x0400206F RID: 8303
			Detached,
			// Token: 0x04002070 RID: 8304
			Explode
		}
	}
}
