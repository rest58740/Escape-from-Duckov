using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200004B RID: 75
	[Category("System Events")]
	[Description("The agent is type of Transform so that Triggers can either work with a Collider or a Rigidbody attached.")]
	[Name("Check Trigger", 0)]
	public class CheckTrigger_Transform : ConditionTask<Transform>
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00006F60 File Offset: 0x00005160
		protected override string info
		{
			get
			{
				return this.checkType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006F9C File Offset: 0x0000519C
		protected override bool OnCheck()
		{
			return this.checkType == TriggerTypes.TriggerStay && this.stay;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006FAF File Offset: 0x000051AF
		protected override void OnEnable()
		{
			base.router.onTriggerEnter += this.OnTriggerEnter;
			base.router.onTriggerExit += this.OnTriggerExit;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006FDF File Offset: 0x000051DF
		protected override void OnDisable()
		{
			base.router.onTriggerEnter -= this.OnTriggerEnter;
			base.router.onTriggerExit -= this.OnTriggerExit;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007010 File Offset: 0x00005210
		public void OnTriggerEnter(EventData<Collider> data)
		{
			if (!this.specifiedTagOnly || data.value.gameObject.CompareTag(this.objectTag))
			{
				this.stay = true;
				if (this.checkType == TriggerTypes.TriggerEnter || this.checkType == TriggerTypes.TriggerStay)
				{
					this.saveGameObjectAs.value = data.value.gameObject;
					base.YieldReturn(true);
				}
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007074 File Offset: 0x00005274
		public void OnTriggerExit(EventData<Collider> data)
		{
			if (!this.specifiedTagOnly || data.value.gameObject.CompareTag(this.objectTag))
			{
				this.stay = false;
				if (this.checkType == TriggerTypes.TriggerExit)
				{
					this.saveGameObjectAs.value = data.value.gameObject;
					base.YieldReturn(true);
				}
			}
		}

		// Token: 0x040000E3 RID: 227
		public TriggerTypes checkType;

		// Token: 0x040000E4 RID: 228
		public bool specifiedTagOnly;

		// Token: 0x040000E5 RID: 229
		[TagField]
		[ShowIf("specifiedTagOnly", 1)]
		public string objectTag = "Untagged";

		// Token: 0x040000E6 RID: 230
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000E7 RID: 231
		private bool stay;
	}
}
