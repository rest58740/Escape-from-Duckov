using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000078 RID: 120
	[Category("✫ Blackboard/Lists")]
	[Description("Will sort the gameobjects in the target list by their distance to the agent (closer first) and save that list to the blackboard")]
	public class SortGameObjectListByDistance : ActionTask<Transform>
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00009205 File Offset: 0x00007405
		protected override string info
		{
			get
			{
				string text = "Sort ";
				BBParameter<List<GameObject>> bbparameter = this.targetList;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = " by distance as ";
				BBParameter<List<GameObject>> bbparameter2 = this.saveAs;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000923C File Offset: 0x0000743C
		protected override void OnExecute()
		{
			this.saveAs.value = (from go in this.targetList.value
			orderby Vector3.Distance(go.transform.position, base.agent.position)
			select go).ToList<GameObject>();
			if (this.reverse)
			{
				this.saveAs.value.Reverse();
			}
			base.EndAction();
		}

		// Token: 0x04000169 RID: 361
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<GameObject>> targetList;

		// Token: 0x0400016A RID: 362
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveAs;

		// Token: 0x0400016B RID: 363
		public bool reverse;
	}
}
