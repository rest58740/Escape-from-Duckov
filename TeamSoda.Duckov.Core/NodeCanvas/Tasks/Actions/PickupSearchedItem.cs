using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200042A RID: 1066
	public class PickupSearchedItem : ActionTask<AICharacterController>
	{
		// Token: 0x0600269C RID: 9884 RVA: 0x00085A32 File Offset: 0x00083C32
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x00085A38 File Offset: 0x00083C38
		protected override void OnExecute()
		{
			if (base.agent == null || base.agent.CharacterMainControl == null || base.agent.searchedPickup == null)
			{
				base.EndAction(false);
				return;
			}
			if (Vector3.Distance(base.agent.transform.position, base.agent.searchedPickup.transform.position) > 1.5f)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.searchedPickup.ItemAgent != null)
			{
				base.agent.CharacterMainControl.PickupItem(base.agent.searchedPickup.ItemAgent.Item);
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x00085AF8 File Offset: 0x00083CF8
		protected override void OnUpdate()
		{
		}
	}
}
