using System;
using Duckov.Buffs;
using ItemStatsSystem;

// Token: 0x02000088 RID: 136
[MenuPath("Buff/Buff层数")]
public class BuffLayersGreaterOrEqual : EffectFilter
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060004ED RID: 1261 RVA: 0x000168E8 File Offset: 0x00014AE8
	public override string DisplayName
	{
		get
		{
			return "buff层数大于等于";
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060004EE RID: 1262 RVA: 0x000168EF File Offset: 0x00014AEF
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

	// Token: 0x060004EF RID: 1263 RVA: 0x00016929 File Offset: 0x00014B29
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		return this.buff && this.buff.CurrentLayers >= this.layers;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00016950 File Offset: 0x00014B50
	private void OnDestroy()
	{
	}

	// Token: 0x0400042D RID: 1069
	public Buff buff;

	// Token: 0x0400042E RID: 1070
	public int layers = 1;

	// Token: 0x0400042F RID: 1071
	private CharacterMainControl _mainControl;
}
