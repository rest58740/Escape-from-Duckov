using System;
using ItemStatsSystem;

// Token: 0x0200008A RID: 138
[MenuPath("弱属性")]
public class ElementFactorFilter : EffectFilter
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000169B9 File Offset: 0x00014BB9
	public override string DisplayName
	{
		get
		{
			return string.Format("如果{0}系数{1}{2}", this.element, (this.type == ElementFactorFilter.ElementFactorFilterTypes.GreaterThan) ? "大于" : "小于", this.compareTo);
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x000169EF File Offset: 0x00014BEF
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00016A2C File Offset: 0x00014C2C
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		if (!this.MainControl)
		{
			return false;
		}
		if (!this.MainControl.Health)
		{
			return false;
		}
		float num = this.MainControl.Health.ElementFactor(this.element);
		if (this.type != ElementFactorFilter.ElementFactorFilterTypes.GreaterThan)
		{
			return num < this.compareTo;
		}
		return num > this.compareTo;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00016A8E File Offset: 0x00014C8E
	private void OnDestroy()
	{
	}

	// Token: 0x04000431 RID: 1073
	public ElementFactorFilter.ElementFactorFilterTypes type;

	// Token: 0x04000432 RID: 1074
	public float compareTo = 1f;

	// Token: 0x04000433 RID: 1075
	public ElementTypes element;

	// Token: 0x04000434 RID: 1076
	private CharacterMainControl _mainControl;

	// Token: 0x0200045C RID: 1116
	public enum ElementFactorFilterTypes
	{
		// Token: 0x04001B46 RID: 6982
		GreaterThan,
		// Token: 0x04001B47 RID: 6983
		LessThan
	}
}
