using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.UI.SavesRestore
{
	// Token: 0x02000405 RID: 1029
	public class SavesBackupRestorePanel : MonoBehaviour
	{
		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x00082528 File Offset: 0x00080728
		private PrefabPool<SavesBackupRestorePanelEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<SavesBackupRestorePanelEntry>(this.template, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x00082561 File Offset: 0x00080761
		private void Awake()
		{
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x00082563 File Offset: 0x00080763
		public void Open(int savesSlot)
		{
			this.slot = savesSlot;
			this.Refresh();
			this.fadeGroup.Show();
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x0008257D File Offset: 0x0008077D
		public void Close()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x0008258A File Offset: 0x0008078A
		public void Confirm()
		{
			this.confirm = true;
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00082593 File Offset: 0x00080793
		public void Cancel()
		{
			this.cancel = true;
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x0008259C File Offset: 0x0008079C
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			List<SavesSystem.BackupInfo> list = SavesSystem.GetBackupList(this.slot).ToList<SavesSystem.BackupInfo>();
			list.Sort(delegate(SavesSystem.BackupInfo a, SavesSystem.BackupInfo b)
			{
				if (a.Time < b.Time)
				{
					return 1;
				}
				return -1;
			});
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				SavesSystem.BackupInfo backupInfo = list[i];
				if (backupInfo.exists)
				{
					this.Pool.Get(null).Setup(this, backupInfo);
					num++;
				}
			}
			this.noBackupIndicator.SetActive(num <= 0);
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x00082638 File Offset: 0x00080838
		internal void NotifyClicked(SavesBackupRestorePanelEntry button)
		{
			if (this.recovering)
			{
				return;
			}
			SavesSystem.BackupInfo info = button.Info;
			if (!info.exists)
			{
				return;
			}
			this.RecoverTask(info).Forget();
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x0008266C File Offset: 0x0008086C
		private UniTask RecoverTask(SavesSystem.BackupInfo info)
		{
			SavesBackupRestorePanel.<RecoverTask>d__21 <RecoverTask>d__;
			<RecoverTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RecoverTask>d__.<>4__this = this;
			<RecoverTask>d__.info = info;
			<RecoverTask>d__.<>1__state = -1;
			<RecoverTask>d__.<>t__builder.Start<SavesBackupRestorePanel.<RecoverTask>d__21>(ref <RecoverTask>d__);
			return <RecoverTask>d__.<>t__builder.Task;
		}

		// Token: 0x04001955 RID: 6485
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001956 RID: 6486
		[SerializeField]
		private FadeGroup confirmFadeGroup;

		// Token: 0x04001957 RID: 6487
		[SerializeField]
		private FadeGroup resultFadeGroup;

		// Token: 0x04001958 RID: 6488
		[SerializeField]
		private TextMeshProUGUI[] slotIndexTexts;

		// Token: 0x04001959 RID: 6489
		[SerializeField]
		private TextMeshProUGUI[] backupTimeTexts;

		// Token: 0x0400195A RID: 6490
		[SerializeField]
		private SavesBackupRestorePanelEntry template;

		// Token: 0x0400195B RID: 6491
		[SerializeField]
		private GameObject noBackupIndicator;

		// Token: 0x0400195C RID: 6492
		private PrefabPool<SavesBackupRestorePanelEntry> _pool;

		// Token: 0x0400195D RID: 6493
		private int slot;

		// Token: 0x0400195E RID: 6494
		private bool recovering;

		// Token: 0x0400195F RID: 6495
		private bool confirm;

		// Token: 0x04001960 RID: 6496
		private bool cancel;
	}
}
