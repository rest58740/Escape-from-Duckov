using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000429 RID: 1065
	public class MoveToRandomPos : ActionTask<AICharacterController>
	{
		// Token: 0x06002695 RID: 9877 RVA: 0x00085729 File Offset: 0x00083929
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0008572C File Offset: 0x0008392C
		protected override void OnExecute()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			this.targetPoint = this.RandomPoint();
			base.agent.MoveToPos(this.targetPoint);
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x00085764 File Offset: 0x00083964
		protected override void OnUpdate()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			if (base.elapsedTime > this.overTime.value)
			{
				base.EndAction(this.overTimeReturnSuccess);
				return;
			}
			if (this.useTransform && this.centerTransform.value == null)
			{
				base.EndAction(false);
				return;
			}
			if (this.syncDirectionIfNoAimTarget && base.agent.aimTarget == null)
			{
				if (this.setAimToPos && this.aimPos.isDefined)
				{
					base.agent.CharacterMainControl.SetAimPoint(this.aimPos.value);
				}
				else
				{
					Vector3 currentMoveDirection = base.agent.CharacterMainControl.CurrentMoveDirection;
					if (currentMoveDirection.magnitude > 0f)
					{
						base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + currentMoveDirection * 1000f);
					}
				}
			}
			if (!base.agent.WaitingForPathResult())
			{
				if (base.agent.ReachedEndOfPath() || !base.agent.IsMoving())
				{
					base.EndAction(true);
					return;
				}
				if (!base.agent.HasPath())
				{
					if (!this.failIfNoPath && this.retryIfNotFound)
					{
						this.targetPoint = this.RandomPoint();
						base.agent.MoveToPos(this.targetPoint);
						return;
					}
					base.EndAction(!this.failIfNoPath);
					return;
				}
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000858EB File Offset: 0x00083AEB
		protected override void OnStop()
		{
			base.agent.StopMove();
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x000858F8 File Offset: 0x00083AF8
		protected override void OnPause()
		{
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000858FC File Offset: 0x00083AFC
		private Vector3 RandomPoint()
		{
			Vector3 vector = base.agent.CharacterMainControl.transform.position;
			if (this.useTransform)
			{
				if (this.centerTransform.isDefined)
				{
					vector = this.centerTransform.value.position;
				}
			}
			else
			{
				vector = this.centerPos.value;
			}
			Vector3 vector2 = vector - base.agent.transform.position;
			vector2.y = 0f;
			if (vector2.magnitude < 0.1f)
			{
				vector2 = Random.insideUnitSphere;
				vector2.y = 0f;
			}
			vector2 = vector2.normalized;
			float num = Random.Range(-0.5f * this.randomAngle, 0.5f * this.randomAngle);
			float num2 = Random.Range(this.avoidRadius.value, this.radius.value);
			vector2 = Quaternion.Euler(0f, num, 0f) * -vector2;
			return vector + vector2 * num2;
		}

		// Token: 0x04001A43 RID: 6723
		public bool useTransform;

		// Token: 0x04001A44 RID: 6724
		public bool setAimToPos;

		// Token: 0x04001A45 RID: 6725
		[ShowIf("setAimToPos", 1)]
		public BBParameter<Vector3> aimPos;

		// Token: 0x04001A46 RID: 6726
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> centerPos;

		// Token: 0x04001A47 RID: 6727
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> centerTransform;

		// Token: 0x04001A48 RID: 6728
		public BBParameter<float> radius;

		// Token: 0x04001A49 RID: 6729
		public BBParameter<float> avoidRadius;

		// Token: 0x04001A4A RID: 6730
		public float randomAngle = 360f;

		// Token: 0x04001A4B RID: 6731
		public BBParameter<float> overTime = 8f;

		// Token: 0x04001A4C RID: 6732
		public bool overTimeReturnSuccess = true;

		// Token: 0x04001A4D RID: 6733
		private Vector3 targetPoint;

		// Token: 0x04001A4E RID: 6734
		public bool failIfNoPath;

		// Token: 0x04001A4F RID: 6735
		[ShowIf("failIfNoPath", 0)]
		public bool retryIfNotFound;

		// Token: 0x04001A50 RID: 6736
		public bool syncDirectionIfNoAimTarget = true;
	}
}
