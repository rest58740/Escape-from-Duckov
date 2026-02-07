using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000020 RID: 32
	public class ModifierDescriptionCollection : ItemComponent, ICollection<ModifierDescription>, IEnumerable<ModifierDescription>, IEnumerable
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000775C File Offset: 0x0000595C
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007778 File Offset: 0x00005978
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00007769 File Offset: 0x00005969
		public bool ModifierEnable
		{
			get
			{
				return this._modifierEnableCache;
			}
			set
			{
				this._modifierEnableCache = value;
				this.ReapplyModifiers();
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007780 File Offset: 0x00005980
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007783 File Offset: 0x00005983
		internal override void OnInitialize()
		{
			base.Master.onItemTreeChanged += this.OnItemTreeChange;
			base.Master.onDurabilityChanged += this.OnDurabilityChange;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000077B3 File Offset: 0x000059B3
		private void OnDurabilityChange(Item item)
		{
			this.ReapplyModifiers();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000077BB File Offset: 0x000059BB
		private void OnDestroy()
		{
			if (base.Master)
			{
				base.Master.onItemTreeChanged -= this.OnItemTreeChange;
				base.Master.onDurabilityChanged -= this.OnDurabilityChange;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000077F8 File Offset: 0x000059F8
		private void OnItemTreeChange(Item item)
		{
			this.ReapplyModifiers();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007800 File Offset: 0x00005A00
		public void ReapplyModifiers()
		{
			if (base.Master == null)
			{
				return;
			}
			bool flag = this.ModifierEnable;
			if (base.Master.UseDurability && base.Master.Durability <= 0f)
			{
				flag = false;
			}
			if (!flag)
			{
				foreach (ModifierDescription modifierDescription in this.list)
				{
					modifierDescription.Release();
				}
				return;
			}
			foreach (ModifierDescription modifierDescription2 in this.list)
			{
				modifierDescription2.ReapplyModifier(this);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000078CC File Offset: 0x00005ACC
		public void Add(ModifierDescription item)
		{
			this.list.Add(item);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000078DC File Offset: 0x00005ADC
		public void Clear()
		{
			if (this.list == null)
			{
				this.list = new List<ModifierDescription>();
			}
			foreach (ModifierDescription modifierDescription in this.list)
			{
				modifierDescription.Release();
			}
			this.list.Clear();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000794C File Offset: 0x00005B4C
		public bool Contains(ModifierDescription item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000795A File Offset: 0x00005B5A
		public void CopyTo(ModifierDescription[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007969 File Offset: 0x00005B69
		public bool Remove(ModifierDescription item)
		{
			if (item != null && this.list.Contains(item))
			{
				item.Release();
			}
			return this.list.Remove(item);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000798E File Offset: 0x00005B8E
		public IEnumerator<ModifierDescription> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000079A0 File Offset: 0x00005BA0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000079B2 File Offset: 0x00005BB2
		public ModifierDescription Find(Predicate<ModifierDescription> predicate)
		{
			return this.list.Find(predicate);
		}

		// Token: 0x0400009D RID: 157
		private bool _modifierEnableCache = true;

		// Token: 0x0400009E RID: 158
		[SerializeField]
		private List<ModifierDescription> list = new List<ModifierDescription>();
	}
}
