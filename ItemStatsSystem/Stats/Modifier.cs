using System;
using UnityEngine;

namespace ItemStatsSystem.Stats
{
	// Token: 0x02000027 RID: 39
	public class Modifier
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00008202 File Offset: 0x00006402
		public int Order
		{
			get
			{
				if (this.overrideOrder)
				{
					return this.overrideOrderValue;
				}
				return (int)this.type;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00008219 File Offset: 0x00006419
		public ModifierType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00008221 File Offset: 0x00006421
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00008229 File Offset: 0x00006429
		public float Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (value != this.value)
				{
					this.value = value;
					if (this.target != null)
					{
						this.target.SetDirty();
					}
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000824E File Offset: 0x0000644E
		public object Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008256 File Offset: 0x00006456
		public Modifier(ModifierType type, float value, object source) : this(type, value, false, 0, source)
		{
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00008263 File Offset: 0x00006463
		public Modifier(ModifierType type, float value, bool overrideOrder, int overrideOrderValue, object source)
		{
			this.type = type;
			this.value = value;
			this.overrideOrder = overrideOrder;
			this.overrideOrderValue = overrideOrderValue;
			this.source = source;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008290 File Offset: 0x00006490
		public void NotifyAddedToStat(Stat stat)
		{
			if (this.target != null && this.target != stat)
			{
				Debug.LogError("Modifier被赋予给了多了个不同的Stat");
				this.target.RemoveModifier(this);
			}
			this.target = stat;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000082C1 File Offset: 0x000064C1
		public void RemoveFromTarget()
		{
			if (this.target != null)
			{
				this.target.RemoveModifier(this);
				this.target = null;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000082E0 File Offset: 0x000064E0
		public override string ToString()
		{
			string text = "";
			bool flag = this.value > 0f;
			ModifierType modifierType = this.type;
			if (modifierType != ModifierType.Add)
			{
				if (modifierType != ModifierType.PercentageAdd)
				{
					if (modifierType == ModifierType.PercentageMultiply)
					{
						text = string.Format("x(1{0}{1})", flag ? "+" : "", this.value);
					}
				}
				else
				{
					text = string.Format("x(..{0}{1})", flag ? "+" : "", this.value);
				}
			}
			else
			{
				text = string.Format("{0}{1}", flag ? "+" : "", this.value);
			}
			string text2 = flag ? "#55FF55" : "#FF5555";
			text = string.Concat(new string[]
			{
				"<color=",
				text2,
				">",
				text,
				"</color>"
			});
			return text + " 来自 " + this.source.ToString();
		}

		// Token: 0x040000B6 RID: 182
		private Stat target;

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private ModifierType type;

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		private float value;

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		private bool overrideOrder;

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private int overrideOrderValue;

		// Token: 0x040000BB RID: 187
		[SerializeField]
		private object source;

		// Token: 0x040000BC RID: 188
		public static readonly Comparison<Modifier> OrderComparison = (Modifier a, Modifier b) => a.Order - b.Order;
	}
}
