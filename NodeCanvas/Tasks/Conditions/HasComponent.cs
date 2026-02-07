using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000025 RID: 37
	[Category("GameObject")]
	public class HasComponent<T> : ConditionTask<Transform> where T : Component
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000443E File Offset: 0x0000263E
		protected override bool OnCheck()
		{
			return base.agent.GetComponent<T>() != null;
		}
	}
}
