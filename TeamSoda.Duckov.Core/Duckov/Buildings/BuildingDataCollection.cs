using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x0200032A RID: 810
	[CreateAssetMenu]
	public class BuildingDataCollection : ScriptableObject
	{
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x00061C14 File Offset: 0x0005FE14
		public static BuildingDataCollection Instance
		{
			get
			{
				return GameplayDataSettings.BuildingDataCollection;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x00061C1B File Offset: 0x0005FE1B
		public ReadOnlyCollection<BuildingInfo> Infos
		{
			get
			{
				if (this.readonlyInfos == null)
				{
					this.readonlyInfos = new ReadOnlyCollection<BuildingInfo>(this.infos);
				}
				return this.readonlyInfos;
			}
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00061C3C File Offset: 0x0005FE3C
		internal static BuildingInfo GetInfo(string id)
		{
			if (BuildingDataCollection.Instance == null)
			{
				return default(BuildingInfo);
			}
			return BuildingDataCollection.Instance.infos.FirstOrDefault((BuildingInfo e) => e.id == id);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00061C88 File Offset: 0x0005FE88
		internal static Building GetPrefab(string prefabName)
		{
			if (BuildingDataCollection.Instance == null)
			{
				return null;
			}
			return BuildingDataCollection.Instance.prefabs.FirstOrDefault((Building e) => e != null && e.name == prefabName);
		}

		// Token: 0x0400135A RID: 4954
		[SerializeField]
		private List<BuildingInfo> infos = new List<BuildingInfo>();

		// Token: 0x0400135B RID: 4955
		[SerializeField]
		private List<Building> prefabs;

		// Token: 0x0400135C RID: 4956
		public ReadOnlyCollection<BuildingInfo> readonlyInfos;
	}
}
