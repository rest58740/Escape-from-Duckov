using System;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x02000344 RID: 836
	[Serializable]
	public struct MultiSceneLocation
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x000681E6 File Offset: 0x000663E6
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x000681EE File Offset: 0x000663EE
		public Transform LocationTransform
		{
			get
			{
				return this.GetLocationTransform();
			}
			private set
			{
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000681F0 File Offset: 0x000663F0
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x000681F8 File Offset: 0x000663F8
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
			set
			{
				this.sceneID = value;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x00068204 File Offset: 0x00066404
		public SceneReference Scene
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
				if (sceneInfo == null)
				{
					return null;
				}
				return sceneInfo.SceneReference;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x00068228 File Offset: 0x00066428
		// (set) Token: 0x06001C84 RID: 7300 RVA: 0x00068230 File Offset: 0x00066430
		public string LocationName
		{
			get
			{
				return this.locationName;
			}
			set
			{
				this.locationName = value;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x00068239 File Offset: 0x00066439
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayName);
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00068246 File Offset: 0x00066446
		public Transform GetLocationTransform()
		{
			if (this.Scene == null)
			{
				return null;
			}
			if (this.Scene.UnsafeReason != null)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(this.Scene, this.locationName);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00068274 File Offset: 0x00066474
		public bool TryGetLocationPosition(out Vector3 result)
		{
			result = default(Vector3);
			if (MultiSceneCore.Instance == null)
			{
				return false;
			}
			if (MultiSceneCore.Instance.TryGetCachedPosition(this.sceneID, this.locationName, out result))
			{
				return true;
			}
			Transform location = SceneLocationsProvider.GetLocation(this.sceneID, this.locationName);
			if (location != null)
			{
				result = location.transform.position;
				return true;
			}
			return false;
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000682E1 File Offset: 0x000664E1
		internal string GetDisplayName()
		{
			return this.DisplayName;
		}

		// Token: 0x04001436 RID: 5174
		[SerializeField]
		private string sceneID;

		// Token: 0x04001437 RID: 5175
		[SerializeField]
		private string locationName;

		// Token: 0x04001438 RID: 5176
		[SerializeField]
		private string displayName;
	}
}
