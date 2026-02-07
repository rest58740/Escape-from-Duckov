using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B0 RID: 176
	[Category("Movement/Pathfinding")]
	[Description("Flees away from the target")]
	public class Flee : ActionTask<NavMeshAgent>
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000B338 File Offset: 0x00009538
		protected override string info
		{
			get
			{
				return string.Format("Flee from {0}", this.target);
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B34C File Offset: 0x0000954C
		protected override void OnExecute()
		{
			if (this.target.value == null)
			{
				base.EndAction(false);
				return;
			}
			base.agent.speed = this.speed.value;
			if ((base.agent.transform.position - this.target.value.transform.position).magnitude >= this.fledDistance.value)
			{
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B3D4 File Offset: 0x000095D4
		protected override void OnUpdate()
		{
			if (this.target.value == null)
			{
				base.EndAction(false);
				return;
			}
			Vector3 position = this.target.value.transform.position;
			if ((base.agent.transform.position - position).magnitude >= this.fledDistance.value)
			{
				base.EndAction(true);
				return;
			}
			Vector3 destination = position + (base.agent.transform.position - position).normalized * (this.fledDistance.value + this.lookAhead.value + base.agent.stoppingDistance);
			if (!base.agent.SetDestination(destination))
			{
				base.EndAction(false);
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B4A7 File Offset: 0x000096A7
		protected override void OnPause()
		{
			this.OnStop();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000B4AF File Offset: 0x000096AF
		protected override void OnStop()
		{
			if (base.agent.gameObject.activeSelf)
			{
				base.agent.ResetPath();
			}
		}

		// Token: 0x04000207 RID: 519
		[RequiredField]
		[Tooltip("The target to flee from.")]
		public BBParameter<GameObject> target;

		// Token: 0x04000208 RID: 520
		[Tooltip("The speed to flee.")]
		public BBParameter<float> speed = 4f;

		// Token: 0x04000209 RID: 521
		[Tooltip("The distance to flee at.")]
		public BBParameter<float> fledDistance = 10f;

		// Token: 0x0400020A RID: 522
		[Tooltip("A distance to look away from the target for valid flee destination.")]
		public BBParameter<float> lookAhead = 2f;
	}
}
