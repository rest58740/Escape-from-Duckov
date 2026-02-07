using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000010 RID: 16
	[RequireComponent(typeof(Effect))]
	public class EffectFilter : EffectComponent, ISelfValidator
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003363 File Offset: 0x00001563
		protected override Color ActiveLabelColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectFilter;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000336F File Offset: 0x0000156F
		public override string DisplayName
		{
			get
			{
				return "未命名过滤器(" + base.GetType().Name + ")";
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000338B File Offset: 0x0000158B
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003393 File Offset: 0x00001593
		protected bool IgnoreNegativeTrigger
		{
			get
			{
				return this.ignoreNegativeTrigger;
			}
			set
			{
				this.ignoreNegativeTrigger = value;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000339C File Offset: 0x0000159C
		public bool Evaluate(EffectTriggerEventContext context)
		{
			return !base.enabled || (!context.positive && this.IgnoreNegativeTrigger) || this.OnEvaluate(context);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000033C1 File Offset: 0x000015C1
		protected virtual bool OnEvaluate(EffectTriggerEventContext context)
		{
			return true;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000033C4 File Offset: 0x000015C4
		public override void Validate(SelfValidationResult result)
		{
			base.Validate(result);
			if (base.Master != null && !base.Master.Filters.Contains(this))
			{
				result.AddError("Master 中不包含本 Filter。").WithFix("将此 Filter 添加到 Master 中。", delegate()
				{
					base.Master.AddEffectComponent(this);
				}, true);
			}
		}

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private bool ignoreNegativeTrigger = true;
	}
}
