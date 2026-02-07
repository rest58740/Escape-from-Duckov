using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000434 RID: 1076
	public class Shoot : ActionTask<AICharacterController>
	{
		// Token: 0x060026DB RID: 9947 RVA: 0x000865CD File Offset: 0x000847CD
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x000865D0 File Offset: 0x000847D0
		protected override string info
		{
			get
			{
				return string.Format("Shoot {0}to{1} sec.", this.shootTimeRange.value.x, this.shootTimeRange.value.y);
			}
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x00086608 File Offset: 0x00084808
		protected override void OnExecute()
		{
			this.semiTimer = this.semiTimeSpace;
			base.agent.CharacterMainControl.Trigger(true, true, false);
			if (!base.agent.shootCanMove)
			{
				base.agent.StopMove();
			}
			this.shootTime = Random.Range(this.shootTimeRange.value.x, this.shootTimeRange.value.y);
			if (this.shootTime <= 0f)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x0008668C File Offset: 0x0008488C
		protected override void OnUpdate()
		{
			bool triggerThisFrame = false;
			this.semiTimer += Time.deltaTime;
			if (!base.agent.shootCanMove)
			{
				base.agent.StopMove();
			}
			if (this.semiTimer >= this.semiTimeSpace)
			{
				this.semiTimer = 0f;
				triggerThisFrame = true;
			}
			base.agent.CharacterMainControl.Trigger(true, triggerThisFrame, false);
			if (base.elapsedTime >= this.shootTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x00086708 File Offset: 0x00084908
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x0008671D File Offset: 0x0008491D
		protected override void OnPause()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x04001A74 RID: 6772
		public BBParameter<Vector2> shootTimeRange;

		// Token: 0x04001A75 RID: 6773
		private float shootTime;

		// Token: 0x04001A76 RID: 6774
		public float semiTimeSpace = 0.35f;

		// Token: 0x04001A77 RID: 6775
		private float semiTimer;
	}
}
