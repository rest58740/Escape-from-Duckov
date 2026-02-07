using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000AF RID: 175
	[Name("Find Closest NavMesh Edge", 0)]
	[Category("Movement/Pathfinding")]
	[Description("Find the closes Navigation Mesh position to the target position")]
	public class FindClosestEdge : ActionTask
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x0000B2F0 File Offset: 0x000094F0
		protected override void OnExecute()
		{
			if (NavMesh.FindClosestEdge(this.targetPosition.value, out this.hit, -1))
			{
				this.saveFoundPosition.value = this.hit.position;
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x04000204 RID: 516
		public BBParameter<Vector3> targetPosition;

		// Token: 0x04000205 RID: 517
		[BlackboardOnly]
		public BBParameter<Vector3> saveFoundPosition;

		// Token: 0x04000206 RID: 518
		private NavMeshHit hit;
	}
}
