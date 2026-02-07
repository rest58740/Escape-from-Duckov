using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029F RID: 671
	public class GoldMinerEntity : MiniGameBehaviour
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600162C RID: 5676 RVA: 0x00052A2F File Offset: 0x00050C2F
		// (set) Token: 0x0600162D RID: 5677 RVA: 0x00052A37 File Offset: 0x00050C37
		public GoldMiner master { get; private set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600162E RID: 5678 RVA: 0x00052A40 File Offset: 0x00050C40
		public string TypeID
		{
			get
			{
				return this.typeID;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x00052A48 File Offset: 0x00050C48
		public float Speed
		{
			get
			{
				return this.speed;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001630 RID: 5680 RVA: 0x00052A50 File Offset: 0x00050C50
		// (set) Token: 0x06001631 RID: 5681 RVA: 0x00052A58 File Offset: 0x00050C58
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00052A61 File Offset: 0x00050C61
		public void SetMaster(GoldMiner master)
		{
			this.master = master;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00052A6A File Offset: 0x00050C6A
		public void NotifyAttached(Hook hook)
		{
			Action<GoldMinerEntity, Hook> onAttached = this.OnAttached;
			if (onAttached != null)
			{
				onAttached(this, hook);
			}
			FXPool.Play(this.contactFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00052AA1 File Offset: 0x00050CA1
		public void NotifyBeginRetrieving()
		{
			FXPool.Play(this.beginMoveFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00052AC5 File Offset: 0x00050CC5
		internal void Explode(Vector3 origin)
		{
			Object.Destroy(base.gameObject);
			FXPool.Play(this.explodeFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00052AF4 File Offset: 0x00050CF4
		internal void NotifyResolved(GoldMiner game)
		{
			Action<GoldMinerEntity, GoldMiner> onResolved = this.OnResolved;
			if (onResolved != null)
			{
				onResolved(this, game);
			}
			FXPool.Play(this.resolveFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x04001062 RID: 4194
		[SerializeField]
		private string typeID;

		// Token: 0x04001063 RID: 4195
		[SerializeField]
		public GoldMinerEntity.Size size;

		// Token: 0x04001064 RID: 4196
		[SerializeField]
		public GoldMinerEntity.Tag[] tags;

		// Token: 0x04001065 RID: 4197
		[SerializeField]
		private int value;

		// Token: 0x04001066 RID: 4198
		[SerializeField]
		private float speed = 1f;

		// Token: 0x04001067 RID: 4199
		[SerializeField]
		private ParticleSystem contactFX;

		// Token: 0x04001068 RID: 4200
		[SerializeField]
		private ParticleSystem beginMoveFX;

		// Token: 0x04001069 RID: 4201
		[SerializeField]
		private ParticleSystem resolveFX;

		// Token: 0x0400106A RID: 4202
		[SerializeField]
		private ParticleSystem explodeFX;

		// Token: 0x0400106B RID: 4203
		public Action<GoldMinerEntity, Hook> OnAttached;

		// Token: 0x0400106C RID: 4204
		public Action<GoldMinerEntity, GoldMiner> OnResolved;

		// Token: 0x0200058D RID: 1421
		public enum Size
		{
			// Token: 0x04002041 RID: 8257
			XS = -2,
			// Token: 0x04002042 RID: 8258
			S,
			// Token: 0x04002043 RID: 8259
			M,
			// Token: 0x04002044 RID: 8260
			L,
			// Token: 0x04002045 RID: 8261
			XL
		}

		// Token: 0x0200058E RID: 1422
		public enum Tag
		{
			// Token: 0x04002047 RID: 8263
			None,
			// Token: 0x04002048 RID: 8264
			Rock,
			// Token: 0x04002049 RID: 8265
			Gold,
			// Token: 0x0400204A RID: 8266
			Diamond,
			// Token: 0x0400204B RID: 8267
			Mine,
			// Token: 0x0400204C RID: 8268
			Chest,
			// Token: 0x0400204D RID: 8269
			Pig,
			// Token: 0x0400204E RID: 8270
			Cable
		}
	}
}
