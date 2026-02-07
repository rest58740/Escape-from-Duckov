using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using UnityEngine;

namespace Duckvo.Beacons
{
	// Token: 0x0200022B RID: 555
	public class BeaconManager : MonoBehaviour
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x00041E51 File Offset: 0x00040051
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x00041E58 File Offset: 0x00040058
		public static BeaconManager Instance { get; private set; }

		// Token: 0x060010DF RID: 4319 RVA: 0x00041E60 File Offset: 0x00040060
		private void Awake()
		{
			BeaconManager.Instance = this;
			this.Load();
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00041E7F File Offset: 0x0004007F
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00041E92 File Offset: 0x00040092
		public void Load()
		{
			if (SavesSystem.KeyExisits("BeaconManager"))
			{
				this.data = SavesSystem.Load<BeaconManager.Data>("BeaconManager");
			}
			if (this.data.entries == null)
			{
				this.data.entries = new List<BeaconManager.BeaconStatus>();
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00041ECD File Offset: 0x000400CD
		public void Save()
		{
			SavesSystem.Save<BeaconManager.Data>("BeaconManager", this.data);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00041EE0 File Offset: 0x000400E0
		public static void UnlockBeacon(string id, int index)
		{
			if (BeaconManager.Instance == null)
			{
				return;
			}
			if (BeaconManager.GetBeaconUnlocked(id, index))
			{
				return;
			}
			BeaconManager.Instance.data.entries.Add(new BeaconManager.BeaconStatus
			{
				beaconID = id,
				beaconIndex = index
			});
			Action<string, int> onBeaconUnlocked = BeaconManager.OnBeaconUnlocked;
			if (onBeaconUnlocked == null)
			{
				return;
			}
			onBeaconUnlocked(id, index);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00041F44 File Offset: 0x00040144
		public static bool GetBeaconUnlocked(string id, int index)
		{
			return !(BeaconManager.Instance == null) && BeaconManager.Instance.data.entries.Any((BeaconManager.BeaconStatus e) => e.beaconID == id && e.beaconIndex == index);
		}

		// Token: 0x04000D82 RID: 3458
		private BeaconManager.Data data;

		// Token: 0x04000D83 RID: 3459
		public static Action<string, int> OnBeaconUnlocked;

		// Token: 0x04000D84 RID: 3460
		private const string SaveKey = "BeaconManager";

		// Token: 0x02000524 RID: 1316
		[Serializable]
		public struct BeaconStatus
		{
			// Token: 0x04001EA2 RID: 7842
			public string beaconID;

			// Token: 0x04001EA3 RID: 7843
			public int beaconIndex;
		}

		// Token: 0x02000525 RID: 1317
		[Serializable]
		public struct Data
		{
			// Token: 0x04001EA4 RID: 7844
			public List<BeaconManager.BeaconStatus> entries;
		}
	}
}
