using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B4 RID: 180
	[Category("Movement/Pathfinding")]
	[Description("Makes the agent wander randomly within the navigation map")]
	public class Wander : ActionTask<NavMeshAgent>
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x0000BBC7 File Offset: 0x00009DC7
		protected override void OnExecute()
		{
			base.agent.speed = this.speed.value;
			this.DoWander();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		protected override void OnUpdate()
		{
			if (!base.agent.pathPending && base.agent.remainingDistance <= base.agent.stoppingDistance + this.keepDistance.value)
			{
				if (this.repeat)
				{
					this.DoWander();
					return;
				}
				base.EndAction();
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BC3C File Offset: 0x00009E3C
		private void DoWander()
		{
			float num = this.minWanderDistance.value;
			float num2 = this.maxWanderDistance.value;
			num = Mathf.Clamp(num, 0.01f, num2);
			num2 = Mathf.Clamp(num2, num, num2);
			Vector3 vector = base.agent.transform.position;
			while ((vector - base.agent.transform.position).magnitude < num)
			{
				vector = Random.insideUnitSphere * num2 + base.agent.transform.position;
			}
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(vector, out navMeshHit, base.agent.height * 2f, -1))
			{
				base.agent.SetDestination(navMeshHit.position);
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000BCFC File Offset: 0x00009EFC
		protected override void OnPause()
		{
			this.OnStop();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BD04 File Offset: 0x00009F04
		protected override void OnStop()
		{
			if (base.agent.gameObject.activeSelf)
			{
				base.agent.ResetPath();
			}
		}

		// Token: 0x04000219 RID: 537
		[Tooltip("The speed to wander with.")]
		public BBParameter<float> speed = 4f;

		// Token: 0x0400021A RID: 538
		[Tooltip("The distance to keep from each wander point.")]
		public BBParameter<float> keepDistance = 0.1f;

		// Token: 0x0400021B RID: 539
		[Tooltip("A wander point can't be closer than this distance")]
		public BBParameter<float> minWanderDistance = 5f;

		// Token: 0x0400021C RID: 540
		[Tooltip("A wander point can't be further than this distance")]
		public BBParameter<float> maxWanderDistance = 20f;

		// Token: 0x0400021D RID: 541
		[Tooltip("If enabled, will keep wandering forever. If not, only one wander point will be performed.")]
		public bool repeat = true;
	}
}
