using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000077 RID: 119
	[Category("✫ Blackboard/Lists")]
	public class ShuffleList : ActionTask
	{
		// Token: 0x0600022C RID: 556 RVA: 0x0000919C File Offset: 0x0000739C
		protected override void OnExecute()
		{
			IList value = this.targetList.value;
			for (int i = value.Count - 1; i > 0; i--)
			{
				int num = (int)Mathf.Floor(Random.value * (float)(i + 1));
				object obj = value[i];
				value[i] = value[num];
				value[num] = obj;
			}
			base.EndAction();
		}

		// Token: 0x04000168 RID: 360
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<IList> targetList;
	}
}
