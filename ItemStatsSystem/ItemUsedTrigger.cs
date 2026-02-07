using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000016 RID: 22
	public class ItemUsedTrigger : EffectTrigger
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003EE9 File Offset: 0x000020E9
		public override string DisplayName
		{
			get
			{
				return "当物品被使用";
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003EF0 File Offset: 0x000020F0
		private void OnEnable()
		{
			if (base.Master != null && base.Master.Item != null)
			{
				base.Master.Item.onUse += this.OnItemUsed;
				return;
			}
			Debug.LogError("因为找不到对象，未能注册物品使用事件。");
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003F48 File Offset: 0x00002148
		private new void OnDisable()
		{
			if (base.Master == null)
			{
				return;
			}
			if (base.Master.Item == null)
			{
				return;
			}
			base.Master.Item.onUse -= this.OnItemUsed;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003F94 File Offset: 0x00002194
		private void OnItemUsed(Item item, object user)
		{
			base.Trigger(true);
		}
	}
}
