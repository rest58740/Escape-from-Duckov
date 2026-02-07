using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200042D RID: 1069
	public class ReleaseItemSkillIfHas : ActionTask<AICharacterController>
	{
		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x00085CAE File Offset: 0x00083EAE
		private float chance
		{
			get
			{
				if (!base.agent)
				{
					return 0f;
				}
				return base.agent.itemSkillChance;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x00085CCE File Offset: 0x00083ECE
		public float checkTimeSpace
		{
			get
			{
				if (!base.agent)
				{
					return 999f;
				}
				return base.agent.itemSkillCoolTime;
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x00085CEE File Offset: 0x00083EEE
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x00085CF4 File Offset: 0x00083EF4
		protected override void OnExecute()
		{
			this.skillRefrence = null;
			if (Time.time - this.checkTimeMarker < this.checkTimeSpace)
			{
				base.EndAction(false);
				return;
			}
			this.checkTimeMarker = Time.time;
			if (Random.Range(0f, 1f) > this.chance)
			{
				base.EndAction(false);
				return;
			}
			ItemSetting_Skill itemSkill = base.agent.GetItemSkill(this.random);
			if (!itemSkill)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.CharacterMainControl.CurrentAction && base.agent.CharacterMainControl.CurrentAction.Running)
			{
				base.EndAction(false);
				return;
			}
			this.skillRefrence = itemSkill;
			base.agent.CharacterMainControl.ChangeHoldItem(itemSkill.Item);
			base.agent.CharacterMainControl.SetSkill(SkillTypes.itemSkill, itemSkill.Skill, itemSkill.gameObject);
			if (!base.agent.CharacterMainControl.StartSkillAim(SkillTypes.itemSkill))
			{
				base.EndAction(false);
				return;
			}
			this.readyTime = itemSkill.Skill.SkillContext.skillReadyTime;
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x00085E14 File Offset: 0x00084014
		protected override void OnUpdate()
		{
			if (!this.skillRefrence)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.searchedEnemy)
			{
				base.agent.CharacterMainControl.SetAimPoint(base.agent.searchedEnemy.transform.position);
			}
			if (base.elapsedTime > this.readyTime + 0.1f)
			{
				base.agent.CharacterMainControl.ReleaseSkill(SkillTypes.itemSkill);
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x00085E9B File Offset: 0x0008409B
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.CancleSkill();
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x04001A54 RID: 6740
		public bool random = true;

		// Token: 0x04001A55 RID: 6741
		private float checkTimeMarker = -1f;

		// Token: 0x04001A56 RID: 6742
		private float readyTime;

		// Token: 0x04001A57 RID: 6743
		private ItemSetting_Skill skillRefrence;
	}
}
