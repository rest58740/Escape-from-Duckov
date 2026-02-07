using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000430 RID: 1072
	public class SearchEnemyAround : ActionTask<AICharacterController>
	{
		// Token: 0x060026C0 RID: 9920 RVA: 0x00086187 File Offset: 0x00084387
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0008618C File Offset: 0x0008438C
		protected override void OnExecute()
		{
			DamageInfo damageInfo = default(DamageInfo);
			this.isHurtSearch = false;
			if (base.agent.IsHurt(1.5f, 1, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				this.isHurtSearch = true;
			}
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000861E4 File Offset: 0x000843E4
		private void Search()
		{
			this.waitingSearchResult = true;
			float num = this.useSight ? base.agent.sightAngle : this.searchAngle.value;
			float num2 = this.useSight ? (base.agent.sightDistance * this.sightDistanceMultiplier.value) : this.searchDistance.value;
			if (this.isHurtSearch)
			{
				num2 *= 2f;
			}
			if (this.affactByNightVisionAbility && base.agent.CharacterMainControl)
			{
				float nightVisionAbility = base.agent.CharacterMainControl.NightVisionAbility;
				num *= Mathf.Lerp(TimeOfDayController.NightViewAngleFactor, 1f, nightVisionAbility);
			}
			bool flag = this.useSight || this.checkObsticle;
			this.searchStartTimeMarker = Time.time;
			bool thermalOn = base.agent.CharacterMainControl.ThermalOn;
			LevelManager.Instance.AIMainBrain.AddSearchTask(base.agent.transform.position + Vector3.up * 1.5f, base.agent.CharacterMainControl.CurrentAimDirection, num, num2, base.agent.CharacterMainControl.Team, flag, thermalOn, this.isHurtSearch, this.searchPickup ? base.agent.wantItem : -1, new Action<DamageReceiver, InteractablePickup>(this.OnSearchFinished));
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x00086344 File Offset: 0x00084544
		private void OnSearchFinished(DamageReceiver dmgReceiver, InteractablePickup pickup)
		{
			if (base.agent.gameObject == null)
			{
				return;
			}
			float time = Time.time;
			float num = this.searchStartTimeMarker;
			if (dmgReceiver != null)
			{
				this.result.value = dmgReceiver;
			}
			else if (this.setNullIfNotFound)
			{
				this.result.value = null;
			}
			if (pickup != null)
			{
				this.pickupResult.value = pickup;
			}
			this.waitingSearchResult = false;
			if (base.isRunning)
			{
				base.EndAction(this.alwaysSuccess || this.result.value != null || this.pickupResult != null);
			}
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000863F1 File Offset: 0x000845F1
		protected override void OnUpdate()
		{
			if (!this.waitingSearchResult)
			{
				this.Search();
			}
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x00086401 File Offset: 0x00084601
		protected override void OnStop()
		{
			this.waitingSearchResult = false;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0008640A File Offset: 0x0008460A
		protected override void OnPause()
		{
		}

		// Token: 0x04001A60 RID: 6752
		public bool useSight;

		// Token: 0x04001A61 RID: 6753
		public bool affactByNightVisionAbility;

		// Token: 0x04001A62 RID: 6754
		[ShowIf("useSight", 0)]
		public BBParameter<float> searchAngle = 180f;

		// Token: 0x04001A63 RID: 6755
		[ShowIf("useSight", 0)]
		public BBParameter<float> searchDistance;

		// Token: 0x04001A64 RID: 6756
		[ShowIf("useSight", 1)]
		public BBParameter<float> sightDistanceMultiplier = 1f;

		// Token: 0x04001A65 RID: 6757
		[ShowIf("useSight", 0)]
		public bool checkObsticle = true;

		// Token: 0x04001A66 RID: 6758
		public BBParameter<DamageReceiver> result;

		// Token: 0x04001A67 RID: 6759
		public BBParameter<InteractablePickup> pickupResult;

		// Token: 0x04001A68 RID: 6760
		public bool searchPickup;

		// Token: 0x04001A69 RID: 6761
		public bool alwaysSuccess;

		// Token: 0x04001A6A RID: 6762
		public bool setNullIfNotFound;

		// Token: 0x04001A6B RID: 6763
		private bool waitingSearchResult;

		// Token: 0x04001A6C RID: 6764
		private float searchStartTimeMarker;

		// Token: 0x04001A6D RID: 6765
		private bool isHurtSearch;
	}
}
