using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C8 RID: 968
	public class ItemShortcutEditorPanel : MonoBehaviour
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x0007A18C File Offset: 0x0007838C
		private PrefabPool<ItemShortcutEditorEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<ItemShortcutEditorEntry>(this.entryTemplate, this.entryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.entryTemplate.gameObject.SetActive(false);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0007A1E5 File Offset: 0x000783E5
		private void OnEnable()
		{
			this.Setup();
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0007A1F0 File Offset: 0x000783F0
		private void Setup()
		{
			this.EntryPool.ReleaseAll();
			for (int i = 0; i <= ItemShortcut.MaxIndex; i++)
			{
				ItemShortcutEditorEntry itemShortcutEditorEntry = this.EntryPool.Get(this.entryTemplate.transform.parent);
				itemShortcutEditorEntry.Setup(i);
				itemShortcutEditorEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x04001794 RID: 6036
		[SerializeField]
		private ItemShortcutEditorEntry entryTemplate;

		// Token: 0x04001795 RID: 6037
		private PrefabPool<ItemShortcutEditorEntry> _entryPool;
	}
}
