using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000026 RID: 38
	[Category("GameObject")]
	public class IsActive : ConditionTask<Transform>
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000445E File Offset: 0x0000265E
		protected override string info
		{
			get
			{
				return base.agentInfo + " is Active";
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004470 File Offset: 0x00002670
		protected override bool OnCheck()
		{
			return base.agent.gameObject.activeInHierarchy;
		}
	}
}
