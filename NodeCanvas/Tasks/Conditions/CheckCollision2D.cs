using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000046 RID: 70
	[Category("System Events")]
	[Name("Check Collision 2D", 0)]
	[DoNotList]
	public class CheckCollision2D : ConditionTask<Collider2D>
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006A4F File Offset: 0x00004C4F
		protected override string info
		{
			get
			{
				return this.checkType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006A8B File Offset: 0x00004C8B
		protected override bool OnCheck()
		{
			return this.checkType == CollisionTypes.CollisionStay && this.stay;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006A9E File Offset: 0x00004C9E
		protected override void OnEnable()
		{
			base.router.onCollisionEnter2D += this.OnCollisionEnter2D;
			base.router.onCollisionExit2D += this.OnCollisionExit2D;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006ACE File Offset: 0x00004CCE
		protected override void OnDisable()
		{
			base.router.onCollisionEnter2D -= this.OnCollisionEnter2D;
			base.router.onCollisionExit2D -= this.OnCollisionExit2D;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006B00 File Offset: 0x00004D00
		private void OnCollisionEnter2D(EventData<Collision2D> data)
		{
			if (!this.specifiedTagOnly || data.value.gameObject.CompareTag(this.objectTag))
			{
				this.stay = true;
				if (this.checkType == CollisionTypes.CollisionEnter || this.checkType == CollisionTypes.CollisionStay)
				{
					this.saveGameObjectAs.value = data.value.gameObject;
					this.saveContactPoint.value = data.value.contacts[0].point;
					this.saveContactNormal.value = data.value.contacts[0].normal;
					base.YieldReturn(true);
				}
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006BB8 File Offset: 0x00004DB8
		private void OnCollisionExit2D(EventData<Collision2D> data)
		{
			if (!this.specifiedTagOnly || data.value.gameObject.CompareTag(this.objectTag))
			{
				this.stay = false;
				if (this.checkType == CollisionTypes.CollisionExit)
				{
					this.saveGameObjectAs.value = data.value.gameObject;
					base.YieldReturn(true);
				}
			}
		}

		// Token: 0x040000D8 RID: 216
		public CollisionTypes checkType;

		// Token: 0x040000D9 RID: 217
		public bool specifiedTagOnly;

		// Token: 0x040000DA RID: 218
		[TagField]
		public string objectTag = "Untagged";

		// Token: 0x040000DB RID: 219
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000DC RID: 220
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactPoint;

		// Token: 0x040000DD RID: 221
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactNormal;

		// Token: 0x040000DE RID: 222
		private bool stay;
	}
}
