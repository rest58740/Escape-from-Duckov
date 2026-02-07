using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200004E RID: 78
	[Category("System Events")]
	[Name("Check Trigger 2D", 0)]
	[DoNotList]
	public class CheckTrigger2D : ConditionTask<Collider2D>
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000073EB File Offset: 0x000055EB
		protected override string info
		{
			get
			{
				return this.CheckType.ToString() + (this.specifiedTagOnly ? (" '" + this.objectTag + "' tag") : "");
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007427 File Offset: 0x00005627
		protected override bool OnCheck()
		{
			return this.CheckType == TriggerTypes.TriggerStay && this.stay;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000743A File Offset: 0x0000563A
		protected override void OnEnable()
		{
			base.router.onTriggerEnter2D += this.OnTriggerEnter2D;
			base.router.onTriggerExit2D += this.OnTriggerExit2D;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000746A File Offset: 0x0000566A
		protected override void OnDisable()
		{
			base.router.onTriggerEnter2D -= this.OnTriggerEnter2D;
			base.router.onTriggerExit2D -= this.OnTriggerExit2D;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000749C File Offset: 0x0000569C
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

		// Token: 0x06000186 RID: 390 RVA: 0x00007500 File Offset: 0x00005700
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

		// Token: 0x040000F2 RID: 242
		public TriggerTypes CheckType;

		// Token: 0x040000F3 RID: 243
		public bool specifiedTagOnly;

		// Token: 0x040000F4 RID: 244
		[TagField]
		public string objectTag = "Untagged";

		// Token: 0x040000F5 RID: 245
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;

		// Token: 0x040000F6 RID: 246
		private bool stay;
	}
}
