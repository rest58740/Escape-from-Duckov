using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000D RID: 13
	[MenuPath("Debug/Log Item Name")]
	public class LogItemNameAction : EffectAction
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000313A File Offset: 0x0000133A
		public override string DisplayName
		{
			get
			{
				return "Log 物品名称";
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003141 File Offset: 0x00001341
		protected override void OnTriggeredPositive()
		{
			if (base.Master.Item == null)
			{
				Debug.Log("物品不存在");
				return;
			}
			Debug.Log(base.Master.Item.DisplayName);
		}
	}
}
