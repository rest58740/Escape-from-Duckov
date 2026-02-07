using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200004C RID: 76
	[Category("System Events")]
	[DoNotList]
	public class CheckTrigger : ConditionTask<Collider>
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000070E3 File Offset: 0x000052E3
		protected override string info
		{
			get
			{
				return this.checkType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000711F File Offset: 0x0000531F
		protected override bool OnCheck()
		{
			return this.checkType == TriggerTypes.TriggerStay && this.stay;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007132 File Offset: 0x00005332
		protected override void OnEnable()
		{
			base.router.onTriggerEnter += this.OnTriggerEnter;
			base.router.onTriggerExit += this.OnTriggerExit;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007162 File Offset: 0x00005362
		protected override void OnDisable()
		{
			base.router.onTriggerEnter -= this.OnTriggerEnter;
			base.router.onTriggerExit -= this.OnTriggerExit;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007194 File Offset: 0x00005394
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

		// Token: 0x06000178 RID: 376 RVA: 0x000071F8 File Offset: 0x000053F8
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

		// Token: 0x040000E8 RID: 232
		public TriggerTypes checkType;

		// Token: 0x040000E9 RID: 233
		public bool specifiedTagOnly;

		// Token: 0x040000EA RID: 234
		[TagField]
		[ShowIf("specifiedTagOnly", 1)]
		public string objectTag = "Untagged";

		// Token: 0x040000EB RID: 235
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000EC RID: 236
		private bool stay;
	}
}
