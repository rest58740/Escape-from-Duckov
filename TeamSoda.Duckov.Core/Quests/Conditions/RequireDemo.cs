using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200037A RID: 890
	public class RequireDemo : Condition
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x0006EB23 File Offset: 0x0006CD23
		public override bool Evaluate()
		{
			if (this.inverse)
			{
				return !GameMetaData.Instance.IsDemo;
			}
			return GameMetaData.Instance.IsDemo;
		}

		// Token: 0x04001555 RID: 5461
		[SerializeField]
		private bool inverse;
	}
}
