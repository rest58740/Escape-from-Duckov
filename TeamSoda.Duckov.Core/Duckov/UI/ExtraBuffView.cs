using System;
using Duckov.Buffs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039D RID: 925
	public class ExtraBuffView : View
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x0007132C File Offset: 0x0006F52C
		private PrefabPool<ExtraBuffViewEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<ExtraBuffViewEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00071365 File Offset: 0x0006F565
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Setup();
			this.fadeGroup.Show();
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0007137E File Offset: 0x0006F57E
		protected override void OnClose()
		{
			this.TearDown();
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00071398 File Offset: 0x0006F598
		private void Setup()
		{
			if (CharacterMainControl.Main == null)
			{
				return;
			}
			this.buffManager = CharacterMainControl.Main.GetBuffManager();
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff += this.OnAddBuff;
			this.buffManager.onRemoveBuff += this.OnRemoveBuff;
			this.Refresh();
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00071406 File Offset: 0x0006F606
		private void TearDown()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff -= this.OnAddBuff;
			this.buffManager.onRemoveBuff -= this.OnRemoveBuff;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00071445 File Offset: 0x0006F645
		private void OnAddBuff(CharacterBuffManager manager, Buff buff)
		{
			this.Refresh();
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0007144D File Offset: 0x0006F64D
		private void OnRemoveBuff(CharacterBuffManager manager, Buff buff)
		{
			this.Refresh();
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00071458 File Offset: 0x0006F658
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (Buff buff in this.buffManager.Buffs)
			{
				if (!(buff == null) && buff.DisplayInExtraView)
				{
					this.Pool.Get(null).Setup(this, buff);
				}
			}
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x000714D4 File Offset: 0x0006F6D4
		internal void NotifyPointerEnter(ExtraBuffViewEntry extraBuffViewEntry)
		{
			this.hoverPanel.NotifyEnter(extraBuffViewEntry);
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000714E2 File Offset: 0x0006F6E2
		internal void NotifyPointerExit(ExtraBuffViewEntry extraBuffViewEntry)
		{
			this.hoverPanel.NotifyExit(extraBuffViewEntry);
		}

		// Token: 0x040015FD RID: 5629
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015FE RID: 5630
		[SerializeField]
		private ExtraBuffViewHoverPanel hoverPanel;

		// Token: 0x040015FF RID: 5631
		[SerializeField]
		private ExtraBuffViewEntry entryTemplate;

		// Token: 0x04001600 RID: 5632
		private PrefabPool<ExtraBuffViewEntry> _pool;

		// Token: 0x04001601 RID: 5633
		private CharacterBuffManager buffManager;
	}
}
