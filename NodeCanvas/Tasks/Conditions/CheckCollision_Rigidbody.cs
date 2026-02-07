using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000043 RID: 67
	[Category("System Events")]
	[Name("Check Collision", 0)]
	public class CheckCollision_Rigidbody : ConditionTask<Rigidbody>
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000064E6 File Offset: 0x000046E6
		protected override string info
		{
			get
			{
				return this.checkType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006522 File Offset: 0x00004722
		protected override void OnEnable()
		{
			base.router.onCollisionEnter += this.OnCollisionEnter;
			base.router.onCollisionExit += this.OnCollisionExit;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006552 File Offset: 0x00004752
		protected override void OnDisable()
		{
			base.router.onCollisionEnter -= this.OnCollisionEnter;
			base.router.onCollisionExit -= this.OnCollisionExit;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006582 File Offset: 0x00004782
		protected override bool OnCheck()
		{
			return this.checkType == CollisionTypes.CollisionStay && this.stay;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006598 File Offset: 0x00004798
		public void OnCollisionEnter(EventData<Collision> data)
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

		// Token: 0x06000137 RID: 311 RVA: 0x00006640 File Offset: 0x00004840
		public void OnCollisionExit(EventData<Collision> data)
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

		// Token: 0x040000C3 RID: 195
		public CollisionTypes checkType;

		// Token: 0x040000C4 RID: 196
		public bool specifiedTagOnly;

		// Token: 0x040000C5 RID: 197
		[TagField]
		public string objectTag = "Untagged";

		// Token: 0x040000C6 RID: 198
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000C7 RID: 199
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactPoint;

		// Token: 0x040000C8 RID: 200
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactNormal;

		// Token: 0x040000C9 RID: 201
		private bool stay;
	}
}
