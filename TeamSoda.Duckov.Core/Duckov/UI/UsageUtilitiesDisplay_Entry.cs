using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B5 RID: 949
	public class UsageUtilitiesDisplay_Entry : MonoBehaviour
	{
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002179 RID: 8569 RVA: 0x0007548C File Offset: 0x0007368C
		// (set) Token: 0x0600217A RID: 8570 RVA: 0x00075494 File Offset: 0x00073694
		public UsageBehavior Target { get; private set; }

		// Token: 0x0600217B RID: 8571 RVA: 0x000754A0 File Offset: 0x000736A0
		internal void Setup(UsageBehavior cur)
		{
			this.text.text = cur.DisplaySettings.Description;
		}

		// Token: 0x040016D0 RID: 5840
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
