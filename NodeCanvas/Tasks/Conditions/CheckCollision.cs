using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000044 RID: 68
	[Category("System Events")]
	[DoNotList]
	public class CheckCollision : ConditionTask<Collider>
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000066AF File Offset: 0x000048AF
		protected override string info
		{
			get
			{
				return this.checkType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000066EB File Offset: 0x000048EB
		protected override void OnEnable()
		{
			base.router.onCollisionEnter += this.OnCollisionEnter;
			base.router.onCollisionExit += this.OnCollisionExit;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000671B File Offset: 0x0000491B
		protected override void OnDisable()
		{
			base.router.onCollisionEnter -= this.OnCollisionEnter;
			base.router.onCollisionExit -= this.OnCollisionExit;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000674B File Offset: 0x0000494B
		protected override bool OnCheck()
		{
			return this.checkType == CollisionTypes.CollisionStay && this.stay;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006760 File Offset: 0x00004960
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

		// Token: 0x0600013E RID: 318 RVA: 0x00006808 File Offset: 0x00004A08
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

		// Token: 0x040000CA RID: 202
		public CollisionTypes checkType;

		// Token: 0x040000CB RID: 203
		public bool specifiedTagOnly;

		// Token: 0x040000CC RID: 204
		[TagField]
		public string objectTag = "Untagged";

		// Token: 0x040000CD RID: 205
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000CE RID: 206
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactPoint;

		// Token: 0x040000CF RID: 207
		[BlackboardOnly]
		public BBParameter<Vector3> saveContactNormal;

		// Token: 0x040000D0 RID: 208
		private bool stay;
	}
}
