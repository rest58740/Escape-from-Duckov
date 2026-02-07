using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Fishing
{
	// Token: 0x0200021C RID: 540
	[CreateAssetMenu(menuName = "Fishing/Fishing Pool")]
	public class FishingPool : ScriptableObject
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x00040ADC File Offset: 0x0003ECDC
		public int GetRandom(WeightModifications[] modifications)
		{
			if (this.entries == null || this.entries.Count <= 0)
			{
				Debug.LogError("Fishing Pool " + base.name + " 里面没有配置任何项目，返回-1");
				return -1;
			}
			if (modifications != null && modifications.Length != 0)
			{
				return this.entries.GetRandomWeighted(delegate(FishingPoolEntry e)
				{
					foreach (WeightModifications weightModifications in modifications)
					{
						if (weightModifications.id == e.ID)
						{
							return e.Weight + weightModifications.addAmount;
						}
					}
					return e.Weight;
				}, 0f).ID;
			}
			return this.entries.GetRandomWeighted((FishingPoolEntry e) => e.Weight, 0f).ID;
		}

		// Token: 0x04000D32 RID: 3378
		[SerializeField]
		private List<FishingPoolEntry> entries;
	}
}
