using System;
using ItemStatsSystem;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000439 RID: 1081
	public class UseDrug : ActionTask<AICharacterController>
	{
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x00086C2D File Offset: 0x00084E2D
		protected override string info
		{
			get
			{
				if (!this.stopMove)
				{
					return "打药";
				}
				return "原地打药";
			}
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x00086C42 File Offset: 0x00084E42
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x00086C48 File Offset: 0x00084E48
		protected override void OnExecute()
		{
			Item drugItem = base.agent.GetDrugItem();
			if (drugItem == null)
			{
				base.EndAction(false);
				return;
			}
			base.agent.CharacterMainControl.UseItem(drugItem);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x00086C84 File Offset: 0x00084E84
		protected override void OnUpdate()
		{
			if (this.stopMove && base.agent.IsMoving())
			{
				base.agent.StopMove();
			}
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				base.EndAction(false);
				return;
			}
			if (!base.agent.CharacterMainControl.useItemAction.Running)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x00086CF6 File Offset: 0x00084EF6
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x04001A88 RID: 6792
		public bool stopMove;
	}
}
