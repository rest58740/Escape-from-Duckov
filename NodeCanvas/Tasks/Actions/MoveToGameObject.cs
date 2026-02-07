using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B1 RID: 177
	[Name("Seek (GameObject)", 0)]
	[Category("Movement/Pathfinding")]
	public class MoveToGameObject : ActionTask<NavMeshAgent>
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000B506 File Offset: 0x00009706
		protected override string info
		{
			get
			{
				string text = "Seek ";
				BBParameter<GameObject> bbparameter = this.target;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000B524 File Offset: 0x00009724
		protected override void OnExecute()
		{
			if (this.target.value == null)
			{
				base.EndAction(false);
				return;
			}
			base.agent.speed = this.speed.value;
			if (Vector3.Distance(base.agent.transform.position, this.target.value.transform.position) <= base.agent.stoppingDistance + this.keepDistance.value)
			{
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B5B0 File Offset: 0x000097B0
		protected override void OnUpdate()
		{
			if (this.target.value == null)
			{
				base.EndAction(false);
				return;
			}
			Vector3 position = this.target.value.transform.position;
			Vector3? vector = this.lastRequest;
			Vector3 rhs = position;
			if ((vector == null || (vector != null && vector.GetValueOrDefault() != rhs)) && !base.agent.SetDestination(position))
			{
				base.EndAction(false);
				return;
			}
			this.lastRequest = new Vector3?(position);
			if (!base.agent.pathPending && base.agent.remainingDistance <= base.agent.stoppingDistance + this.keepDistance.value)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B678 File Offset: 0x00009878
		protected override void OnPause()
		{
			this.OnStop();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000B680 File Offset: 0x00009880
		protected override void OnStop()
		{
			if (base.agent.gameObject.activeSelf)
			{
				base.agent.ResetPath();
			}
			this.lastRequest = default(Vector3?);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000B6AB File Offset: 0x000098AB
		public override void OnDrawGizmosSelected()
		{
			if (this.target.value != null)
			{
				Gizmos.DrawWireSphere(this.target.value.transform.position, this.keepDistance.value);
			}
		}

		// Token: 0x0400020B RID: 523
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x0400020C RID: 524
		public BBParameter<float> speed = 4f;

		// Token: 0x0400020D RID: 525
		public BBParameter<float> keepDistance = 0.1f;

		// Token: 0x0400020E RID: 526
		private Vector3? lastRequest;
	}
}
