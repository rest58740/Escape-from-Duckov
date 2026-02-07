using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B3 RID: 179
	[Category("Movement/Pathfinding")]
	[Description("Move Randomly or Progressively between various game object positions taken from the list provided")]
	public class Patrol : ActionTask<NavMeshAgent>
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000B8AD File Offset: 0x00009AAD
		protected override string info
		{
			get
			{
				return string.Format("{0} Patrol {1}", this.patrolMode, this.targetList);
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		protected override void OnExecute()
		{
			if (this.targetList.value.Count == 0)
			{
				base.EndAction(false);
				return;
			}
			if (this.targetList.value.Count == 1)
			{
				this.index = 0;
			}
			else
			{
				if (this.patrolMode.value == Patrol.PatrolMode.Random)
				{
					int num;
					for (num = this.index; num == this.index; num = Random.Range(0, this.targetList.value.Count))
					{
					}
					this.index = num;
				}
				if (this.patrolMode.value == Patrol.PatrolMode.Progressive)
				{
					this.index = (int)Mathf.Repeat((float)(this.index + 1), (float)this.targetList.value.Count);
				}
			}
			GameObject gameObject = this.targetList.value[this.index];
			if (gameObject == null)
			{
				base.EndAction(false);
				return;
			}
			Vector3 position = gameObject.transform.position;
			base.agent.speed = this.speed.value;
			if ((base.agent.transform.position - position).magnitude < base.agent.stoppingDistance + this.keepDistance.value)
			{
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000BA08 File Offset: 0x00009C08
		protected override void OnUpdate()
		{
			Vector3 position = this.targetList.value[this.index].transform.position;
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

		// Token: 0x060002EE RID: 750 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		protected override void OnPause()
		{
			this.OnStop();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		protected override void OnStop()
		{
			if (this.lastRequest != null && base.agent.gameObject.activeSelf)
			{
				base.agent.ResetPath();
			}
			this.lastRequest = default(Vector3?);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000BB00 File Offset: 0x00009D00
		public override void OnDrawGizmosSelected()
		{
			if (base.agent && this.targetList.value != null)
			{
				foreach (GameObject gameObject in this.targetList.value)
				{
					if (gameObject != null)
					{
						Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
					}
				}
			}
		}

		// Token: 0x04000213 RID: 531
		[RequiredField]
		[Tooltip("A list of gameobjects patrol points.")]
		public BBParameter<List<GameObject>> targetList;

		// Token: 0x04000214 RID: 532
		[Tooltip("The mode to use for patrol (progressive or random)")]
		public BBParameter<Patrol.PatrolMode> patrolMode = Patrol.PatrolMode.Random;

		// Token: 0x04000215 RID: 533
		public BBParameter<float> speed = 4f;

		// Token: 0x04000216 RID: 534
		public BBParameter<float> keepDistance = 0.1f;

		// Token: 0x04000217 RID: 535
		private int index = -1;

		// Token: 0x04000218 RID: 536
		private Vector3? lastRequest;

		// Token: 0x02000140 RID: 320
		public enum PatrolMode
		{
			// Token: 0x04000399 RID: 921
			Progressive,
			// Token: 0x0400039A RID: 922
			Random
		}
	}
}
