using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002F9 RID: 761
	[CreateAssetMenu]
	public class CropDatabase : ScriptableObject
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0005B175 File Offset: 0x00059375
		public static CropDatabase Instance
		{
			get
			{
				return GameplayDataSettings.CropDatabase;
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0005B17C File Offset: 0x0005937C
		public static CropInfo? GetCropInfo(string id)
		{
			CropDatabase instance = CropDatabase.Instance;
			for (int i = 0; i < instance.entries.Count; i++)
			{
				CropInfo cropInfo = instance.entries[i];
				if (cropInfo.id == id)
				{
					return new CropInfo?(cropInfo);
				}
			}
			return null;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0005B1D0 File Offset: 0x000593D0
		internal static bool IsIdValid(string id)
		{
			return !(CropDatabase.Instance == null) && CropDatabase.Instance.entries.Any((CropInfo e) => e.id == id);
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x0005B214 File Offset: 0x00059414
		internal static bool IsSeed(int itemTypeID)
		{
			return !(CropDatabase.Instance == null) && CropDatabase.Instance.seedInfos.Any((SeedInfo e) => e.itemTypeID == itemTypeID);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0005B258 File Offset: 0x00059458
		internal static SeedInfo GetSeedInfo(int seedItemTypeID)
		{
			if (CropDatabase.Instance == null)
			{
				return default(SeedInfo);
			}
			return CropDatabase.Instance.seedInfos.FirstOrDefault((SeedInfo e) => e.itemTypeID == seedItemTypeID);
		}

		// Token: 0x04001208 RID: 4616
		[SerializeField]
		public List<CropInfo> entries = new List<CropInfo>();

		// Token: 0x04001209 RID: 4617
		[SerializeField]
		public List<SeedInfo> seedInfos = new List<SeedInfo>();
	}
}
