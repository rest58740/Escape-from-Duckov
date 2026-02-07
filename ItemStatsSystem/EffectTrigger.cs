using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000011 RID: 17
	[RequireComponent(typeof(Effect))]
	public class EffectTrigger : EffectComponent, ISelfValidator
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003439 File Offset: 0x00001639
		protected override Color ActiveLabelColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectTrigger;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003445 File Offset: 0x00001645
		public override string DisplayName
		{
			get
			{
				return "未命名触发器(" + base.GetType().Name + ")";
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003461 File Offset: 0x00001661
		protected void Trigger(bool positive = true)
		{
			base.Master.Trigger(new EffectTriggerEventContext(this, positive));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003475 File Offset: 0x00001675
		protected void TriggerPositive()
		{
			this.Trigger(true);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000347E File Offset: 0x0000167E
		protected void TriggerNegative()
		{
			this.Trigger(false);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003488 File Offset: 0x00001688
		public override void Validate(SelfValidationResult result)
		{
			base.Validate(result);
			if (base.Master != null && !base.Master.Triggers.Contains(this))
			{
				result.AddError("Master 中不包含本 Filter。").WithFix("将此 Filter 添加到 Master 中。", delegate()
				{
					base.Master.AddEffectComponent(this);
				}, true);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000034E0 File Offset: 0x000016E0
		protected virtual void OnDisable()
		{
			this.Trigger(false);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000034E9 File Offset: 0x000016E9
		protected virtual void OnMasterSetTargetItem(Effect effect, Item item)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000034EB File Offset: 0x000016EB
		internal void NotifySetItem(Effect effect, Item targetItem)
		{
			this.OnMasterSetTargetItem(effect, targetItem);
		}
	}
}
