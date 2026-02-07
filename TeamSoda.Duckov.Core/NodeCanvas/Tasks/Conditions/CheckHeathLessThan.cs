using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200041D RID: 1053
	public class CheckHeathLessThan : ConditionTask<AICharacterController>
	{
		// Token: 0x06002659 RID: 9817 RVA: 0x00084FB5 File Offset: 0x000831B5
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x00084FB8 File Offset: 0x000831B8
		protected override void OnEnable()
		{
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x00084FBA File Offset: 0x000831BA
		protected override void OnDisable()
		{
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00084FBC File Offset: 0x000831BC
		protected override bool OnCheck()
		{
			if (Time.time - this.checkTimeMarker < this.checkTimeSpace)
			{
				return false;
			}
			this.checkTimeMarker = Time.time;
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				return false;
			}
			Health health = base.agent.CharacterMainControl.Health;
			return health && health.CurrentHealth / health.MaxHealth <= this.percent;
		}

		// Token: 0x04001A24 RID: 6692
		public float percent;

		// Token: 0x04001A25 RID: 6693
		private float checkTimeMarker = -1f;

		// Token: 0x04001A26 RID: 6694
		public float checkTimeSpace = 1.5f;
	}
}
