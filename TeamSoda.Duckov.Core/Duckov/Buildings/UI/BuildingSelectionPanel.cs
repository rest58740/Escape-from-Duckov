using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000334 RID: 820
	public class BuildingSelectionPanel : MonoBehaviour
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x00063B28 File Offset: 0x00061D28
		private PrefabPool<BuildingBtnEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<BuildingBtnEntry>(this.buildingBtnTemplate, null, new Action<BuildingBtnEntry>(this.OnGetButtonEntry), new Action<BuildingBtnEntry>(this.OnReleaseButtonEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x00063B77 File Offset: 0x00061D77
		private void OnGetButtonEntry(BuildingBtnEntry entry)
		{
			entry.onButtonClicked += this.OnButtonSelected;
			entry.onRecycleRequested += this.OnRecycleRequested;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x00063B9D File Offset: 0x00061D9D
		private void OnReleaseButtonEntry(BuildingBtnEntry entry)
		{
			entry.onButtonClicked -= this.OnButtonSelected;
			entry.onRecycleRequested -= this.OnRecycleRequested;
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x00063BC3 File Offset: 0x00061DC3
		private void OnRecycleRequested(BuildingBtnEntry entry)
		{
			Action<BuildingBtnEntry> action = this.onRecycleRequested;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x00063BD6 File Offset: 0x00061DD6
		private void OnButtonSelected(BuildingBtnEntry entry)
		{
			Action<BuildingBtnEntry> action = this.onButtonSelected;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06001B6F RID: 7023 RVA: 0x00063BEC File Offset: 0x00061DEC
		// (remove) Token: 0x06001B70 RID: 7024 RVA: 0x00063C24 File Offset: 0x00061E24
		public event Action<BuildingBtnEntry> onButtonSelected;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06001B71 RID: 7025 RVA: 0x00063C5C File Offset: 0x00061E5C
		// (remove) Token: 0x06001B72 RID: 7026 RVA: 0x00063C94 File Offset: 0x00061E94
		public event Action<BuildingBtnEntry> onRecycleRequested;

		// Token: 0x06001B73 RID: 7027 RVA: 0x00063CC9 File Offset: 0x00061EC9
		public void Show()
		{
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x00063CCB File Offset: 0x00061ECB
		internal void Setup(BuildingArea targetArea)
		{
			this.targetArea = targetArea;
			this.Refresh();
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x00063CDC File Offset: 0x00061EDC
		public void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (BuildingInfo buildingInfo in BuildingSelectionPanel.GetBuildingsToDisplay())
			{
				BuildingBtnEntry buildingBtnEntry = this.Pool.Get(null);
				buildingBtnEntry.Setup(buildingInfo);
				buildingBtnEntry.transform.SetAsLastSibling();
			}
			foreach (BuildingBtnEntry buildingBtnEntry2 in this.Pool.ActiveEntries)
			{
				if (!buildingBtnEntry2.CostEnough)
				{
					buildingBtnEntry2.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x00063D84 File Offset: 0x00061F84
		public static BuildingInfo[] GetBuildingsToDisplay()
		{
			BuildingDataCollection instance = BuildingDataCollection.Instance;
			if (instance == null)
			{
				return new BuildingInfo[0];
			}
			List<BuildingInfo> list = new List<BuildingInfo>();
			foreach (BuildingInfo item in instance.Infos)
			{
				if (item.CurrentAmount > 0 || item.RequirementsSatisfied())
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x040013A9 RID: 5033
		[SerializeField]
		private BuildingBtnEntry buildingBtnTemplate;

		// Token: 0x040013AA RID: 5034
		private PrefabPool<BuildingBtnEntry> _pool;

		// Token: 0x040013AB RID: 5035
		private BuildingArea targetArea;
	}
}
