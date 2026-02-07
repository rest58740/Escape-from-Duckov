using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B2 RID: 178
	[Name("Seek (Vector3)", 0)]
	[Category("Movement/Pathfinding")]
	public class MoveToPosition : ActionTask<NavMeshAgent>
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000B70D File Offset: 0x0000990D
		protected override string info
		{
			get
			{
				string text = "Seek ";
				BBParameter<Vector3> bbparameter = this.targetPosition;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B72C File Offset: 0x0000992C
		protected override void OnExecute()
		{
			base.agent.speed = this.speed.value;
			if (Vector3.Distance(base.agent.transform.position, this.targetPosition.value) < base.agent.stoppingDistance + this.keepDistance.value)
			{
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B790 File Offset: 0x00009990
		protected override void OnUpdate()
		{
			Vector3? vector = this.lastRequest;
			Vector3 value = this.targetPosition.value;
			if ((vector == null || (vector != null && vector.GetValueOrDefault() != value)) && !base.agent.SetDestination(this.targetPosition.value))
			{
				base.EndAction(false);
				return;
			}
			this.lastRequest = new Vector3?(this.targetPosition.value);
			if (!base.agent.pathPending && base.agent.remainingDistance <= base.agent.stoppingDistance + this.keepDistance.value)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B845 File Offset: 0x00009A45
		protected override void OnPause()
		{
			this.OnStop();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B84D File Offset: 0x00009A4D
		protected override void OnStop()
		{
			if (this.lastRequest != null && base.agent.gameObject.activeSelf)
			{
				base.agent.ResetPath();
			}
			this.lastRequest = default(Vector3?);
		}

		// Token: 0x0400020F RID: 527
		public BBParameter<Vector3> targetPosition;

		// Token: 0x04000210 RID: 528
		public BBParameter<float> speed = 4f;

		// Token: 0x04000211 RID: 529
		public BBParameter<float> keepDistance = 0.1f;

		// Token: 0x04000212 RID: 530
		private Vector3? lastRequest;
	}
}
