using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200005B RID: 91
	[Name("Play Animation", 0)]
	[Category("Animator")]
	public class MecanimPlayAnimation : ActionTask<Animator>
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00008230 File Offset: 0x00006430
		protected override string info
		{
			get
			{
				return "Anim '" + this.stateName.ToString() + "'";
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000824C File Offset: 0x0000644C
		protected override void OnExecute()
		{
			if (string.IsNullOrEmpty(this.stateName.value))
			{
				base.EndAction();
				return;
			}
			this.played = false;
			AnimatorStateInfo currentAnimatorStateInfo = base.agent.GetCurrentAnimatorStateInfo(this.layerIndex.value);
			base.agent.CrossFade(this.stateName.value, this.transitTime / currentAnimatorStateInfo.length, this.layerIndex.value, 0f, 0f);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000082CC File Offset: 0x000064CC
		protected override void OnUpdate()
		{
			this.stateInfo = base.agent.GetCurrentAnimatorStateInfo(this.layerIndex.value);
			if (this.waitUntilFinish)
			{
				if (this.stateInfo.IsName(this.stateName.value))
				{
					this.played = true;
					if (base.elapsedTime >= this.stateInfo.length / base.agent.speed)
					{
						base.EndAction();
						return;
					}
				}
				else if (this.played)
				{
					base.EndAction();
					return;
				}
			}
			else if (base.elapsedTime >= this.transitTime)
			{
				base.EndAction();
			}
		}

		// Token: 0x04000118 RID: 280
		public BBParameter<int> layerIndex;

		// Token: 0x04000119 RID: 281
		[RequiredField]
		public BBParameter<string> stateName;

		// Token: 0x0400011A RID: 282
		[SliderField(0, 1)]
		public float transitTime = 0.25f;

		// Token: 0x0400011B RID: 283
		public bool waitUntilFinish;

		// Token: 0x0400011C RID: 284
		private AnimatorStateInfo stateInfo;

		// Token: 0x0400011D RID: 285
		private bool played;
	}
}
