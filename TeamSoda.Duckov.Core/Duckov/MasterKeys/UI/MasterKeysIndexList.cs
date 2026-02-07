using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002F0 RID: 752
	public class MasterKeysIndexList : MonoBehaviour, ISingleSelectionMenu<MasterKeysIndexEntry>
	{
		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001850 RID: 6224 RVA: 0x00059B94 File Offset: 0x00057D94
		private PrefabPool<MasterKeysIndexEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<MasterKeysIndexEntry>(this.entryPrefab, this.entryContainer, new Action<MasterKeysIndexEntry>(this.OnGetEntry), new Action<MasterKeysIndexEntry>(this.OnReleaseEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06001851 RID: 6225 RVA: 0x00059BE8 File Offset: 0x00057DE8
		// (remove) Token: 0x06001852 RID: 6226 RVA: 0x00059C20 File Offset: 0x00057E20
		internal event Action<MasterKeysIndexEntry> onEntryPointerClicked;

		// Token: 0x06001853 RID: 6227 RVA: 0x00059C55 File Offset: 0x00057E55
		private void OnGetEntry(MasterKeysIndexEntry entry)
		{
			entry.onPointerClicked += this.OnEntryPointerClicked;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x00059C69 File Offset: 0x00057E69
		private void OnReleaseEntry(MasterKeysIndexEntry entry)
		{
			entry.onPointerClicked -= this.OnEntryPointerClicked;
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x00059C7D File Offset: 0x00057E7D
		private void OnEntryPointerClicked(MasterKeysIndexEntry entry)
		{
			Action<MasterKeysIndexEntry> action = this.onEntryPointerClicked;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00059C90 File Offset: 0x00057E90
		private void Awake()
		{
			this.entryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x00059CA4 File Offset: 0x00057EA4
		internal void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (int itemID in MasterKeysManager.AllPossibleKeys)
			{
				this.Populate(itemID);
			}
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00059D04 File Offset: 0x00057F04
		private void Populate(int itemID)
		{
			MasterKeysIndexEntry masterKeysIndexEntry = this.Pool.Get(this.entryContainer);
			masterKeysIndexEntry.gameObject.SetActive(true);
			masterKeysIndexEntry.Setup(itemID, this);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00059D2A File Offset: 0x00057F2A
		public MasterKeysIndexEntry GetSelection()
		{
			return this.selection;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00059D32 File Offset: 0x00057F32
		public bool SetSelection(MasterKeysIndexEntry selection)
		{
			this.selection = selection;
			return true;
		}

		// Token: 0x040011C8 RID: 4552
		[SerializeField]
		private MasterKeysIndexEntry entryPrefab;

		// Token: 0x040011C9 RID: 4553
		[SerializeField]
		private RectTransform entryContainer;

		// Token: 0x040011CA RID: 4554
		private PrefabPool<MasterKeysIndexEntry> _pool;

		// Token: 0x040011CC RID: 4556
		private MasterKeysIndexEntry selection;
	}
}
