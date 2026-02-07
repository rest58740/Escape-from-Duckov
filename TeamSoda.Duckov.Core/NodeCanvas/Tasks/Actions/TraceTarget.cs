using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000437 RID: 1079
	public class TraceTarget : ActionTask<AICharacterController>
	{
		// Token: 0x060026EC RID: 9964 RVA: 0x0008683E File Offset: 0x00084A3E
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x00086844 File Offset: 0x00084A44
		protected override void OnExecute()
		{
			if (base.agent == null || (this.traceTargetTransform && this.centerTransform.value == null))
			{
				base.EndAction(false);
				return;
			}
			Vector3 pos = this.traceTargetTransform ? this.centerTransform.value.position : this.centerPosition.value;
			base.agent.MoveToPos(pos);
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000868B4 File Offset: 0x00084AB4
		protected override void OnUpdate()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			Vector3 vector = (this.traceTargetTransform && this.centerTransform.value != null) ? this.centerTransform.value.position : this.centerPosition.value;
			if (base.elapsedTime > this.overTime.value)
			{
				base.EndAction(this.overTimeReturnSuccess);
				return;
			}
			if (Vector3.Distance(vector, base.agent.transform.position) < this.stopDistance.value)
			{
				base.EndAction(true);
				return;
			}
			this.recalculatePathTimer -= Time.deltaTime;
			if (this.recalculatePathTimer <= 0f)
			{
				this.recalculatePathTimer = this.recalculatePathTimeSpace;
				base.agent.MoveToPos(vector);
			}
			else if (!base.agent.WaitingForPathResult())
			{
				if (!base.agent.IsMoving() || base.agent.ReachedEndOfPath())
				{
					base.EndAction(true);
					return;
				}
				if (!base.agent.HasPath())
				{
					if (!this.failIfNoPath && this.retryIfNotFound)
					{
						base.agent.MoveToPos(vector);
						return;
					}
					base.EndAction(!this.failIfNoPath);
					return;
				}
			}
			if (this.syncDirectionIfNoAimTarget && base.agent.aimTarget == null)
			{
				Vector3 currentMoveDirection = base.agent.CharacterMainControl.CurrentMoveDirection;
				if (currentMoveDirection.magnitude > 0f)
				{
					base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + currentMoveDirection * 1000f);
				}
			}
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x00086A6C File Offset: 0x00084C6C
		protected override void OnStop()
		{
			base.agent.StopMove();
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x00086A79 File Offset: 0x00084C79
		protected override void OnPause()
		{
		}

		// Token: 0x04001A79 RID: 6777
		public bool traceTargetTransform = true;

		// Token: 0x04001A7A RID: 6778
		[ShowIf("traceTargetTransform", 0)]
		public BBParameter<Vector3> centerPosition;

		// Token: 0x04001A7B RID: 6779
		[ShowIf("traceTargetTransform", 1)]
		public BBParameter<Transform> centerTransform;

		// Token: 0x04001A7C RID: 6780
		public BBParameter<float> stopDistance;

		// Token: 0x04001A7D RID: 6781
		public BBParameter<float> overTime = 8f;

		// Token: 0x04001A7E RID: 6782
		public bool overTimeReturnSuccess = true;

		// Token: 0x04001A7F RID: 6783
		private Vector3 targetPoint;

		// Token: 0x04001A80 RID: 6784
		public bool failIfNoPath;

		// Token: 0x04001A81 RID: 6785
		[ShowIf("failIfNoPath", 0)]
		public bool retryIfNotFound;

		// Token: 0x04001A82 RID: 6786
		private float recalculatePathTimeSpace = 0.15f;

		// Token: 0x04001A83 RID: 6787
		private float recalculatePathTimer = 0.15f;

		// Token: 0x04001A84 RID: 6788
		public bool syncDirectionIfNoAimTarget = true;
	}
}
