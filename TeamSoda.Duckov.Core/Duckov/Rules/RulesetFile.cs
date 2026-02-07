using System;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x0200040F RID: 1039
	[CreateAssetMenu(menuName = "Duckov/Ruleset")]
	public class RulesetFile : ScriptableObject
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x0008325A File Offset: 0x0008145A
		public Ruleset Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x040019A1 RID: 6561
		[SerializeField]
		private Ruleset data;
	}
}
