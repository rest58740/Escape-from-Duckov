using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200041F RID: 1055
	public class CheckHurt : ConditionTask<AICharacterController>
	{
		// Token: 0x06002663 RID: 9827 RVA: 0x00085084 File Offset: 0x00083284
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x00085087 File Offset: 0x00083287
		protected override void OnEnable()
		{
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x00085089 File Offset: 0x00083289
		protected override void OnDisable()
		{
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0008508C File Offset: 0x0008328C
		protected override bool OnCheck()
		{
			if (base.agent == null)
			{
				return false;
			}
			bool result = false;
			DamageInfo damageInfo = default(DamageInfo);
			if (base.agent.IsHurt(this.hurtTimeThreshold, this.damageThreshold, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				if (this.cacheFromCharacterDmgReceiver.isDefined)
				{
					this.cacheFromCharacterDmgReceiver.value = damageInfo.fromCharacter.mainDamageReceiver;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001A27 RID: 6695
		public float hurtTimeThreshold = 0.2f;

		// Token: 0x04001A28 RID: 6696
		public int damageThreshold = 3;

		// Token: 0x04001A29 RID: 6697
		public BBParameter<DamageReceiver> cacheFromCharacterDmgReceiver;
	}
}
