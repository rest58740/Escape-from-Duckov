using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200002E RID: 46
	[Category("Movement")]
	[Description("Check if a path exists for the agent and optionaly save the resulting path positions")]
	public class PathExists : ConditionTask<NavMeshAgent>
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00004A40 File Offset: 0x00002C40
		protected override bool OnCheck()
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			base.agent.CalculatePath(this.targetPosition.value, navMeshPath);
			this.savePathAs.value = navMeshPath.corners.ToList<Vector3>();
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}

		// Token: 0x0400008C RID: 140
		public BBParameter<Vector3> targetPosition;

		// Token: 0x0400008D RID: 141
		[BlackboardOnly]
		public BBParameter<List<Vector3>> savePathAs;
	}
}
