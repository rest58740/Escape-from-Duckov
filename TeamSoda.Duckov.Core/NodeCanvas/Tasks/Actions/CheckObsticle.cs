using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000426 RID: 1062
	public class CheckObsticle : ActionTask<AICharacterController>
	{
		// Token: 0x06002681 RID: 9857 RVA: 0x00085328 File Offset: 0x00083528
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0008532C File Offset: 0x0008352C
		protected override void OnExecute()
		{
			this.isHurtSearch = false;
			DamageInfo damageInfo = default(DamageInfo);
			if (base.agent.IsHurt(1.5f, 1, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				this.isHurtSearch = true;
			}
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x00085384 File Offset: 0x00083584
		private void Check()
		{
			this.waitingResult = true;
			Vector3 vector = this.useTransform ? this.targetTransform.value.position : this.targetPoint.value;
			vector += Vector3.up * 0.4f;
			Vector3 start = base.agent.transform.position + Vector3.up * 0.4f;
			ItemAgent_Gun gun = base.agent.CharacterMainControl.GetGun();
			if (gun && gun.muzzle)
			{
				start = gun.muzzle.position - gun.muzzle.forward * 0.1f;
			}
			LevelManager.Instance.AIMainBrain.AddCheckObsticleTask(start, vector, base.agent.CharacterMainControl.ThermalOn, this.isHurtSearch, new Action<bool>(this.OnCheckFinished));
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x00085478 File Offset: 0x00083678
		private void OnCheckFinished(bool result)
		{
			if (base.agent.gameObject == null)
			{
				return;
			}
			base.agent.hasObsticleToTarget = result;
			this.waitingResult = false;
			if (base.isRunning)
			{
				base.EndAction(this.alwaysSuccess || result);
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000854C6 File Offset: 0x000836C6
		protected override void OnUpdate()
		{
			if (!this.waitingResult)
			{
				this.Check();
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000854D6 File Offset: 0x000836D6
		protected override void OnStop()
		{
			this.waitingResult = false;
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000854DF File Offset: 0x000836DF
		protected override void OnPause()
		{
		}

		// Token: 0x04001A36 RID: 6710
		public bool useTransform;

		// Token: 0x04001A37 RID: 6711
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x04001A38 RID: 6712
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> targetPoint;

		// Token: 0x04001A39 RID: 6713
		public bool alwaysSuccess;

		// Token: 0x04001A3A RID: 6714
		private bool waitingResult;

		// Token: 0x04001A3B RID: 6715
		private bool isHurtSearch;
	}
}
