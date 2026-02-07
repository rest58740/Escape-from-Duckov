using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x02000343 RID: 835
	[Serializable]
	public class SubSceneEntry
	{
		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x000680E8 File Offset: 0x000662E8
		public string AmbientSound
		{
			get
			{
				return this.overrideAmbientSound;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x000680F0 File Offset: 0x000662F0
		public bool IsInDoor
		{
			get
			{
				return this.isInDoor;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x000680F8 File Offset: 0x000662F8
		public SceneInfoEntry Info
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.sceneID);
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x00068108 File Offset: 0x00066308
		public SceneReference SceneReference
		{
			get
			{
				SceneInfoEntry info = this.Info;
				if (info == null)
				{
					Debug.LogWarning("未找到场景" + this.sceneID + "的相关信息，获取SceneReference失败。");
					return null;
				}
				return info.SceneReference;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x00068144 File Offset: 0x00066344
		public string DisplayName
		{
			get
			{
				SceneInfoEntry info = this.Info;
				if (info == null)
				{
					return this.sceneID;
				}
				return info.DisplayName;
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00068168 File Offset: 0x00066368
		internal bool TryGetCachedPosition(string locationPath, out Vector3 result)
		{
			result = default(Vector3);
			if (this.cachedLocations == null)
			{
				return false;
			}
			SubSceneEntry.Location location = this.cachedLocations.Find((SubSceneEntry.Location e) => e.path == locationPath);
			if (location == null)
			{
				return false;
			}
			result = location.position;
			return true;
		}

		// Token: 0x04001431 RID: 5169
		[SceneID]
		public string sceneID;

		// Token: 0x04001432 RID: 5170
		[SerializeField]
		private string overrideAmbientSound = "Default";

		// Token: 0x04001433 RID: 5171
		[SerializeField]
		private bool isInDoor;

		// Token: 0x04001434 RID: 5172
		public List<SubSceneEntry.Location> cachedLocations = new List<SubSceneEntry.Location>();

		// Token: 0x04001435 RID: 5173
		public List<SubSceneEntry.TeleporterInfo> cachedTeleporters = new List<SubSceneEntry.TeleporterInfo>();

		// Token: 0x02000602 RID: 1538
		[Serializable]
		public class Location
		{
			// Token: 0x170007C1 RID: 1985
			// (get) Token: 0x06002A7C RID: 10876 RVA: 0x0009E68E File Offset: 0x0009C88E
			public string DisplayName
			{
				get
				{
					return this.displayName;
				}
			}

			// Token: 0x170007C2 RID: 1986
			// (get) Token: 0x06002A7D RID: 10877 RVA: 0x0009E696 File Offset: 0x0009C896
			// (set) Token: 0x06002A7E RID: 10878 RVA: 0x0009E69E File Offset: 0x0009C89E
			public string DisplayNameRaw
			{
				get
				{
					return this.displayName;
				}
				set
				{
					this.displayName = value;
				}
			}

			// Token: 0x040021D9 RID: 8665
			public string path;

			// Token: 0x040021DA RID: 8666
			public Vector3 position;

			// Token: 0x040021DB RID: 8667
			public bool showInMap;

			// Token: 0x040021DC RID: 8668
			[SerializeField]
			private string displayName;
		}

		// Token: 0x02000603 RID: 1539
		[Serializable]
		public class TeleporterInfo
		{
			// Token: 0x040021DD RID: 8669
			public Vector3 position;

			// Token: 0x040021DE RID: 8670
			public MultiSceneLocation target;

			// Token: 0x040021DF RID: 8671
			public Vector3 nearestTeleporterPositionToTarget;
		}
	}
}
