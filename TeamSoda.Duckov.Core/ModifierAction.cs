using System;
using Duckov.Buffs;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class ModifierAction : EffectAction
{
	// Token: 0x060004E1 RID: 1249 RVA: 0x000165C4 File Offset: 0x000147C4
	protected override void Awake()
	{
		base.Awake();
		this.modifier = new Modifier(this.ModifierType, this.modifierValue, this.overrideOrder, this.overrideOrderValue, base.Master);
		this.targetStatHash = this.targetStatKey.GetHashCode();
		if (this.buff)
		{
			this.buff.OnLayerChangedEvent += this.OnBuffLayerChanged;
		}
		this.OnBuffLayerChanged();
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001663B File Offset: 0x0001483B
	private void OnBuffLayerChanged()
	{
		if (!this.buff)
		{
			return;
		}
		if (this.modifier == null)
		{
			return;
		}
		this.modifier.Value = this.modifierValue * (float)this.buff.CurrentLayers;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00016674 File Offset: 0x00014874
	protected override void OnTriggered(bool positive)
	{
		if (base.Master.Item == null)
		{
			return;
		}
		Item characterItem = base.Master.Item.GetCharacterItem();
		if (characterItem == null)
		{
			return;
		}
		if (positive)
		{
			if (this.targetStat != null)
			{
				this.targetStat.RemoveModifier(this.modifier);
				this.targetStat = null;
			}
			this.targetStat = characterItem.GetStat(this.targetStatHash);
			this.targetStat.AddModifier(this.modifier);
			return;
		}
		if (this.targetStat != null)
		{
			this.targetStat.RemoveModifier(this.modifier);
			this.targetStat = null;
		}
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0001671C File Offset: 0x0001491C
	private void OnDestroy()
	{
		if (this.targetStat != null)
		{
			this.targetStat.RemoveModifier(this.modifier);
			this.targetStat = null;
		}
		if (this.buff)
		{
			this.buff.OnLayerChangedEvent -= this.OnBuffLayerChanged;
		}
	}

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private Buff buff;

	// Token: 0x0400041F RID: 1055
	public string targetStatKey;

	// Token: 0x04000420 RID: 1056
	private int targetStatHash;

	// Token: 0x04000421 RID: 1057
	public ModifierType ModifierType;

	// Token: 0x04000422 RID: 1058
	public float modifierValue;

	// Token: 0x04000423 RID: 1059
	public bool overrideOrder;

	// Token: 0x04000424 RID: 1060
	public int overrideOrderValue;

	// Token: 0x04000425 RID: 1061
	private Modifier modifier;

	// Token: 0x04000426 RID: 1062
	private Stat targetStat;
}
