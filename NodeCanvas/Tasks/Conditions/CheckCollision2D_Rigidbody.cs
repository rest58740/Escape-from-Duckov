using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000045 RID: 69
	[Category("System Events")]
	[Name("Check Collision 2D", 0)]
	public class CheckCollision2D_Rigidbody : ConditionTask<Rigidbody2D>
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006877 File Offset: 0x00004A77
		protected override string info
		{
			get
			{
				return this.checkType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000068B3 File Offset: 0x00004AB3
		protected override bool OnCheck()
		{
			return this.checkType == CollisionTypes.CollisionStay && this.stay;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000068C6 File Offset: 0x00004AC6
		protected override void OnEnable()
		{
			base.router.onCollisionEnter2D += this.OnCollisionEnter2D;
			base.router.onCollisionExit2D += this.OnCollisionExit2D;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000068F6 File Offset: 0x00004AF6
		protected override void OnDisable()
		{
			base.router.onCollisionEnter2D -= this.OnCollisionEnter2D;
			base.router.onCollisionExit2D -= this.OnCollisionExit2D;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006928 File Offset: 0x00004B28
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

		// Token: 0x06000145 RID: 325 RVA: 0x000069E0 File Offset: 0x00004BE0
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

		// Token: 0x040000D1 RID: 209
		public CollisionTypes checkType;

		// Token: 0x040000D2 RID: 210
		public bool specifiedTagOnly;

		// Token: 0x040000D3 RID: 211
		[TagField]
		public string objectTag = "Untagged";

		// Token: 0x040000D4 RID: 212
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000D5 RID: 213
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactPoint;

		// Token: 0x040000D6 RID: 214
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactNormal;

		// Token: 0x040000D7 RID: 215
		private bool stay;
	}
}
