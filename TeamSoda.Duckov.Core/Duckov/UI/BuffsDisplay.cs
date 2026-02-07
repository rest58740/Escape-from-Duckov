using System;
using System.Collections.Generic;
using Duckov.Buffs;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000390 RID: 912
	public class BuffsDisplay : MonoBehaviour
	{
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x0006FD8C File Offset: 0x0006DF8C
		private PrefabPool<BuffsDisplayEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<BuffsDisplayEntry>(this.prefab, base.transform, delegate(BuffsDisplayEntry e)
					{
						this.activeEntries.Add(e);
					}, delegate(BuffsDisplayEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0006FDE0 File Offset: 0x0006DFE0
		public void ReleaseEntry(BuffsDisplayEntry entry)
		{
			this.EntryPool.Release(entry);
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0006FDEE File Offset: 0x0006DFEE
		private void Awake()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.OnLevelInitialized();
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0006FE0E File Offset: 0x0006E00E
		private void OnDestroy()
		{
			this.UnregisterEvents();
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0006FE28 File Offset: 0x0006E028
		private void OnLevelInitialized()
		{
			this.UnregisterEvents();
			this.buffManager = LevelManager.Instance.MainCharacter.GetBuffManager();
			foreach (Buff buff in this.buffManager.Buffs)
			{
				this.OnAddBuff(this.buffManager, buff);
			}
			this.RegisterEvents();
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0006FEA4 File Offset: 0x0006E0A4
		private void RegisterEvents()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff += this.OnAddBuff;
			this.buffManager.onRemoveBuff += this.OnRemoveBuff;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x0006FEE3 File Offset: 0x0006E0E3
		private void UnregisterEvents()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff -= this.OnAddBuff;
			this.buffManager.onRemoveBuff -= this.OnRemoveBuff;
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0006FF22 File Offset: 0x0006E122
		private void OnAddBuff(CharacterBuffManager manager, Buff buff)
		{
			if (buff.Hide)
			{
				return;
			}
			this.EntryPool.Get(null).Setup(this, buff);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0006FF40 File Offset: 0x0006E140
		private void OnRemoveBuff(CharacterBuffManager manager, Buff buff)
		{
			BuffsDisplayEntry buffsDisplayEntry = this.activeEntries.Find((BuffsDisplayEntry e) => e.Target == buff);
			if (buffsDisplayEntry == null)
			{
				return;
			}
			buffsDisplayEntry.Release();
		}

		// Token: 0x040015A9 RID: 5545
		[SerializeField]
		private BuffsDisplayEntry prefab;

		// Token: 0x040015AA RID: 5546
		private PrefabPool<BuffsDisplayEntry> _entryPool;

		// Token: 0x040015AB RID: 5547
		private List<BuffsDisplayEntry> activeEntries = new List<BuffsDisplayEntry>();

		// Token: 0x040015AC RID: 5548
		private CharacterBuffManager buffManager;
	}
}
