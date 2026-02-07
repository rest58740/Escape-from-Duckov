using System;
using ItemStatsSystem;

// Token: 0x02000089 RID: 137
[MenuPath("角色/角色正在奔跑")]
public class CharacterIsRunning : EffectFilter
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00016961 File Offset: 0x00014B61
	public override string DisplayName
	{
		get
		{
			return "角色正在奔跑";
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00016968 File Offset: 0x00014B68
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

	// Token: 0x060004F4 RID: 1268 RVA: 0x000169A2 File Offset: 0x00014BA2
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		return this.MainControl.Running;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000169AF File Offset: 0x00014BAF
	private void OnDestroy()
	{
	}

	// Token: 0x04000430 RID: 1072
	private CharacterMainControl _mainControl;
}
