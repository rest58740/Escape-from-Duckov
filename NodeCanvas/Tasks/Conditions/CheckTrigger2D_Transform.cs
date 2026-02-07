using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200004D RID: 77
	[Category("System Events")]
	[Name("Check Trigger 2D", 0)]
	[Description("The agent is type of Transform so that Triggers can either work with a Collider or a Rigidbody attached.")]
	public class CheckTrigger2D_Transform : ConditionTask<Transform>
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007267 File Offset: 0x00005467
		protected override string info
		{
			get
			{
				return this.CheckType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000072A3 File Offset: 0x000054A3
		protected override bool OnCheck()
		{
			return this.CheckType == TriggerTypes.TriggerStay && this.stay;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000072B6 File Offset: 0x000054B6
		protected override void OnEnable()
		{
			base.router.onTriggerEnter2D += this.OnTriggerEnter2D;
			base.router.onTriggerExit2D += this.OnTriggerExit2D;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000072E6 File Offset: 0x000054E6
		protected override void OnDisable()
		{
			base.router.onTriggerEnter2D -= this.OnTriggerEnter2D;
			base.router.onTriggerExit2D -= this.OnTriggerExit2D;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007318 File Offset: 0x00005518
		public void OnTriggerEnter2D(EventData<Collider2D> data)
		{
			if (!this.specifiedTagOnly || data.value.gameObject.CompareTag(this.objectTag))
			{
				this.stay = true;
				if (this.CheckType == TriggerTypes.TriggerEnter || this.CheckType == TriggerTypes.TriggerStay)
				{
					this.saveGameObjectAs.value = data.value.gameObject;
					base.YieldReturn(true);
				}
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000737C File Offset: 0x0000557C
		public void OnTriggerExit2D(EventData<Collider2D> data)
		{
			if (!this.specifiedTagOnly || data.value.gameObject.CompareTag(this.objectTag))
			{
				this.stay = false;
				if (this.CheckType == TriggerTypes.TriggerExit)
				{
					this.saveGameObjectAs.value = data.value.gameObject;
					base.YieldReturn(true);
				}
			}
		}

		// Token: 0x040000ED RID: 237
		public TriggerTypes CheckType;

		// Token: 0x040000EE RID: 238
		public bool specifiedTagOnly;

		// Token: 0x040000EF RID: 239
		[TagField]
		public string objectTag = "Untagged";

		// Token: 0x040000F0 RID: 240
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000F1 RID: 241
		private bool stay;
	}
}
