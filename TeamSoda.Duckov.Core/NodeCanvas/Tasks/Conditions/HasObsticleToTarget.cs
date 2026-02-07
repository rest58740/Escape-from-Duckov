using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000423 RID: 1059
	public class HasObsticleToTarget : ConditionTask<AICharacterController>
	{
		// Token: 0x06002672 RID: 9842 RVA: 0x00085279 File Offset: 0x00083479
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0008527C File Offset: 0x0008347C
		protected override void OnEnable()
		{
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0008527E File Offset: 0x0008347E
		protected override void OnDisable()
		{
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x00085280 File Offset: 0x00083480
		protected override bool OnCheck()
		{
			return base.agent.hasObsticleToTarget;
		}

		// Token: 0x04001A32 RID: 6706
		public float hurtTimeThreshold = 0.2f;

		// Token: 0x04001A33 RID: 6707
		public int damageThreshold = 3;

		// Token: 0x04001A34 RID: 6708
		public BBParameter<DamageReceiver> cacheFromCharacterDmgReceiver;
	}
}
