using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000E RID: 14
	[RequireComponent(typeof(Effect))]
	public class EffectAction : EffectComponent, ISelfValidator
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000317E File Offset: 0x0000137E
		protected override Color ActiveLabelColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectAction;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000318A File Offset: 0x0000138A
		public override string DisplayName
		{
			get
			{
				return "未命名动作(" + base.GetType().Name + ")";
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000031A6 File Offset: 0x000013A6
		internal void NotifyTriggered(EffectTriggerEventContext context)
		{
			if (!base.enabled)
			{
				return;
			}
			this.OnTriggered(context.positive);
			if (context.positive)
			{
				this.OnTriggeredPositive();
				return;
			}
			this.OnTriggeredNegative();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000031D2 File Offset: 0x000013D2
		protected virtual void OnTriggered(bool positive)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000031D4 File Offset: 0x000013D4
		protected virtual void OnTriggeredPositive()
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000031D6 File Offset: 0x000013D6
		protected virtual void OnTriggeredNegative()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000031D8 File Offset: 0x000013D8
		private void OnDisable()
		{
			this.OnTriggeredNegative();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000031E0 File Offset: 0x000013E0
		public override void Validate(SelfValidationResult result)
		{
			base.Validate(result);
			if (base.Master != null && !base.Master.Actions.Contains(this))
			{
				result.AddError("Master 中不包含本 Filter。").WithFix("将此 Filter 添加到 Master 中。", delegate()
				{
					base.Master.AddEffectComponent(this);
				}, true);
			}
		}
	}
}
