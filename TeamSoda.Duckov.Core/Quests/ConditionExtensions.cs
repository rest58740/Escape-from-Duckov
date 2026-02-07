using System;
using System.Collections.Generic;

namespace Duckov.Quests
{
	// Token: 0x02000349 RID: 841
	public static class ConditionExtensions
	{
		// Token: 0x06001CA8 RID: 7336 RVA: 0x00068740 File Offset: 0x00066940
		public static bool Satisfied(this IEnumerable<Condition> conditions)
		{
			foreach (Condition condition in conditions)
			{
				if (!(condition == null) && !condition.Evaluate())
				{
					return false;
				}
			}
			return true;
		}
	}
}
