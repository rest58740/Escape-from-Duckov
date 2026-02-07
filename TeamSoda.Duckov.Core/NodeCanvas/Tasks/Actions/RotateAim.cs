using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200042F RID: 1071
	public class RotateAim : ActionTask<AICharacterController>
	{
		// Token: 0x060026BA RID: 9914 RVA: 0x0008602A File Offset: 0x0008422A
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x00086030 File Offset: 0x00084230
		protected override void OnExecute()
		{
			this.time = Random.Range(this.timeRange.value.x, this.timeRange.value.y);
			this.startDir = base.agent.CharacterMainControl.CurrentAimDirection;
			base.agent.SetTarget(null);
			if (this.shoot)
			{
				base.agent.CharacterMainControl.Trigger(true, true, false);
			}
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000860A8 File Offset: 0x000842A8
		protected override void OnUpdate()
		{
			this.currentAngle = this.angle * base.elapsedTime / this.time;
			Vector3 vector = Quaternion.Euler(0f, this.currentAngle, 0f) * this.startDir;
			base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + vector * 100f);
			if (this.shoot)
			{
				base.agent.CharacterMainControl.Trigger(true, true, false);
			}
			if (base.elapsedTime > this.time)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x00086155 File Offset: 0x00084355
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0008616A File Offset: 0x0008436A
		protected override void OnPause()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x04001A5A RID: 6746
		private Vector3 startDir;

		// Token: 0x04001A5B RID: 6747
		public float angle;

		// Token: 0x04001A5C RID: 6748
		private float currentAngle;

		// Token: 0x04001A5D RID: 6749
		public BBParameter<Vector2> timeRange;

		// Token: 0x04001A5E RID: 6750
		private float time;

		// Token: 0x04001A5F RID: 6751
		public bool shoot;
	}
}
