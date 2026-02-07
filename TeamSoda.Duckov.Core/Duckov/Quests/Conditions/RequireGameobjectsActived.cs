using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200037C RID: 892
	public class RequireGameobjectsActived : Condition
	{
		// Token: 0x06001F45 RID: 8005 RVA: 0x0006EB64 File Offset: 0x0006CD64
		public override bool Evaluate()
		{
			foreach (GameObject gameObject in this.targets)
			{
				if (gameObject == null || !gameObject.activeInHierarchy)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001559 RID: 5465
		[SerializeField]
		private GameObject[] targets;
	}
}
