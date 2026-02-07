using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200000B RID: 11
	[Name("Is In Transition", 0)]
	[Category("Animator")]
	public class MecanimIsInTransition : ConditionTask<Animator>
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000025F5 File Offset: 0x000007F5
		protected override string info
		{
			get
			{
				return "Mec.Is In Transition";
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000025FC File Offset: 0x000007FC
		protected override bool OnCheck()
		{
			return base.agent.IsInTransition(this.layerIndex.value);
		}

		// Token: 0x04000019 RID: 25
		public BBParameter<int> layerIndex;
	}
}
