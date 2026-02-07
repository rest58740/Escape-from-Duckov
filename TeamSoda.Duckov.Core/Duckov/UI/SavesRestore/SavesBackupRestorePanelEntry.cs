using System;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.SavesRestore
{
	// Token: 0x02000406 RID: 1030
	public class SavesBackupRestorePanelEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x000826BF File Offset: 0x000808BF
		public SavesSystem.BackupInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000826C7 File Offset: 0x000808C7
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.NotifyClicked(this);
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000826D8 File Offset: 0x000808D8
		internal void Setup(SavesBackupRestorePanel master, SavesSystem.BackupInfo info)
		{
			this.master = master;
			this.info = info;
			if (info.time_raw <= 0L)
			{
				this.timeText.text = "???";
				return;
			}
			this.timeText.text = info.Time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
		}

		// Token: 0x04001961 RID: 6497
		[SerializeField]
		private TextMeshProUGUI timeText;

		// Token: 0x04001962 RID: 6498
		private SavesBackupRestorePanel master;

		// Token: 0x04001963 RID: 6499
		private SavesSystem.BackupInfo info;
	}
}
