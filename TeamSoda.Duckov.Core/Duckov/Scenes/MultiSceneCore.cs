using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Utilities;
using Eflatun.SceneReference;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Scenes
{
	// Token: 0x02000342 RID: 834
	public class MultiSceneCore : MonoBehaviour
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x000675B0 File Offset: 0x000657B0
		// (set) Token: 0x06001C47 RID: 7239 RVA: 0x000675B7 File Offset: 0x000657B7
		public static MultiSceneCore Instance { get; private set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000675BF File Offset: 0x000657BF
		public List<SubSceneEntry> SubScenes
		{
			get
			{
				return this.subScenes;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x000675C8 File Offset: 0x000657C8
		public static Scene? MainScene
		{
			get
			{
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				return new Scene?(MultiSceneCore.Instance.gameObject.scene);
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x00067600 File Offset: 0x00065800
		public static string ActiveSubSceneID
		{
			get
			{
				if (MultiSceneCore.ActiveSubScene == null)
				{
					return null;
				}
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				SubSceneEntry subSceneEntry = MultiSceneCore.Instance.SubScenes.Find((SubSceneEntry e) => e != null && MultiSceneCore.ActiveSubScene.Value.buildIndex == e.Info.BuildIndex);
				if (subSceneEntry == null)
				{
					return null;
				}
				return subSceneEntry.sceneID;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x00067668 File Offset: 0x00065868
		public static Scene? ActiveSubScene
		{
			get
			{
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				if (MultiSceneCore.Instance.isLoading)
				{
					return null;
				}
				return new Scene?(MultiSceneCore.Instance.activeSubScene);
			}
		}

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06001C4C RID: 7244 RVA: 0x000676B4 File Offset: 0x000658B4
		// (remove) Token: 0x06001C4D RID: 7245 RVA: 0x000676E8 File Offset: 0x000658E8
		public static event Action<MultiSceneCore, Scene> OnSubSceneWillBeUnloaded;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06001C4E RID: 7246 RVA: 0x0006771C File Offset: 0x0006591C
		// (remove) Token: 0x06001C4F RID: 7247 RVA: 0x00067750 File Offset: 0x00065950
		public static event Action<MultiSceneCore, Scene> OnSubSceneLoaded;

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x00067784 File Offset: 0x00065984
		public SceneInfoEntry SceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x000677AC File Offset: 0x000659AC
		public string DisplayName
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
				if (sceneInfo == null)
				{
					return "?";
				}
				return sceneInfo.DisplayName;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x000677E4 File Offset: 0x000659E4
		public string DisplaynameRaw
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
				if (sceneInfo == null)
				{
					return "?";
				}
				return sceneInfo.DisplayNameRaw;
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x0006781C File Offset: 0x00065A1C
		public static void MoveToActiveWithScene(GameObject go, int sceneBuildIndex)
		{
			if (MultiSceneCore.Instance == null)
			{
				return;
			}
			Transform setActiveWithSceneParent = MultiSceneCore.Instance.GetSetActiveWithSceneParent(sceneBuildIndex);
			go.transform.SetParent(setActiveWithSceneParent);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x00067850 File Offset: 0x00065A50
		public static void MoveToActiveWithScene(GameObject go)
		{
			int buildIndex = go.scene.buildIndex;
			MultiSceneCore.MoveToActiveWithScene(go, buildIndex);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00067874 File Offset: 0x00065A74
		public Transform GetSetActiveWithSceneParent(int sceneBuildIndex)
		{
			GameObject gameObject;
			if (this.setActiveWithSceneObjects.TryGetValue(sceneBuildIndex, out gameObject))
			{
				return gameObject.transform;
			}
			SceneInfoEntry sceneInfoEntry = SceneInfoCollection.GetSceneInfo(sceneBuildIndex);
			if (sceneInfoEntry == null)
			{
				sceneInfoEntry = new SceneInfoEntry();
				Debug.LogWarning(string.Format("BuildIndex {0} 的sceneInfo不存在", sceneBuildIndex));
			}
			GameObject gameObject2 = new GameObject(sceneInfoEntry.ID);
			gameObject2.transform.SetParent(base.transform);
			this.setActiveWithSceneObjects.Add(sceneBuildIndex, gameObject2);
			gameObject2.SetActive(sceneInfoEntry.IsLoaded);
			return gameObject2.transform;
		}

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06001C56 RID: 7254 RVA: 0x000678FC File Offset: 0x00065AFC
		// (remove) Token: 0x06001C57 RID: 7255 RVA: 0x00067930 File Offset: 0x00065B30
		public static event Action<MultiSceneCore> OnInstanceAwake;

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06001C58 RID: 7256 RVA: 0x00067964 File Offset: 0x00065B64
		// (remove) Token: 0x06001C59 RID: 7257 RVA: 0x00067998 File Offset: 0x00065B98
		public static event Action<MultiSceneCore> OnInstanceDestroy;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06001C5A RID: 7258 RVA: 0x000679CC File Offset: 0x00065BCC
		// (remove) Token: 0x06001C5B RID: 7259 RVA: 0x00067A00 File Offset: 0x00065C00
		public static event Action<string> OnSetSceneVisited;

		// Token: 0x06001C5C RID: 7260 RVA: 0x00067A34 File Offset: 0x00065C34
		private void Awake()
		{
			if (MultiSceneCore.Instance == null)
			{
				MultiSceneCore.Instance = this;
			}
			else
			{
				Debug.LogError("Multiple Multi Scene Core detected!");
			}
			Action<MultiSceneCore> onInstanceAwake = MultiSceneCore.OnInstanceAwake;
			if (onInstanceAwake != null)
			{
				onInstanceAwake(this);
			}
			if (this.playAfterLevelInit)
			{
				if (LevelManager.AfterInit)
				{
					this.PlayStinger();
					return;
				}
				LevelManager.OnAfterLevelInitialized += this.OnAfterLevelInitialized;
			}
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00067A98 File Offset: 0x00065C98
		private void OnDestroy()
		{
			Action<MultiSceneCore> onInstanceDestroy = MultiSceneCore.OnInstanceDestroy;
			if (onInstanceDestroy != null)
			{
				onInstanceDestroy(this);
			}
			LevelManager.OnAfterLevelInitialized -= this.OnAfterLevelInitialized;
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00067ABC File Offset: 0x00065CBC
		private void OnAfterLevelInitialized()
		{
			if (this.playAfterLevelInit)
			{
				this.PlayStinger();
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00067ACC File Offset: 0x00065CCC
		public void PlayStinger()
		{
			if (!string.IsNullOrWhiteSpace(this.playStinger))
			{
				AudioManager.PlayStringer(this.playStinger);
			}
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x00067AE8 File Offset: 0x00065CE8
		private void Start()
		{
			this.CreatePointsOfInterestsForLocations();
			AudioManager.StopBGM();
			AudioManager.SetState("Level", this.levelStateName);
			if (this.SceneInfo != null && !string.IsNullOrEmpty(this.SceneInfo.ID))
			{
				MultiSceneCore.SetVisited(this.SceneInfo.ID);
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00067B3A File Offset: 0x00065D3A
		public static void SetVisited(string sceneID)
		{
			SavesSystem.Save<bool>("MultiSceneCore_Visited_" + sceneID, true);
			Action<string> onSetSceneVisited = MultiSceneCore.OnSetSceneVisited;
			if (onSetSceneVisited == null)
			{
				return;
			}
			onSetSceneVisited(sceneID);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00067B5D File Offset: 0x00065D5D
		public static bool GetVisited(string sceneID)
		{
			return SavesSystem.Load<bool>("MultiSceneCore_Visited_" + sceneID);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00067B70 File Offset: 0x00065D70
		private void CreatePointsOfInterestsForLocations()
		{
			foreach (SubSceneEntry subSceneEntry in this.SubScenes)
			{
				foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
				{
					if (location.showInMap)
					{
						SimplePointOfInterest.Create(location.position, subSceneEntry.sceneID, location.DisplayNameRaw, null, true);
					}
				}
			}
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00067C1C File Offset: 0x00065E1C
		private void CreatePointsOfInterestsForTeleporters()
		{
			foreach (SubSceneEntry subSceneEntry in this.SubScenes)
			{
				foreach (SubSceneEntry.TeleporterInfo teleporterInfo in subSceneEntry.cachedTeleporters)
				{
					SimplePointOfInterest.Create(teleporterInfo.position, subSceneEntry.sceneID, "", GameplayDataSettings.UIStyle.DefaultTeleporterIcon, false).ScaleFactor = GameplayDataSettings.UIStyle.TeleporterIconScale;
				}
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00067CD4 File Offset: 0x00065ED4
		public void BeginLoadSubScene(SceneReference reference)
		{
			this.LoadSubScene(reference, true).Forget<bool>();
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x00067CE3 File Offset: 0x00065EE3
		public bool IsLoading
		{
			get
			{
				return this.isLoading;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x00067CEC File Offset: 0x00065EEC
		public static string MainSceneID
		{
			get
			{
				return SceneInfoCollection.GetSceneID(MultiSceneCore.MainScene.Value.buildIndex);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x00067D14 File Offset: 0x00065F14
		private SceneReference GetSubSceneReference(string sceneID)
		{
			SubSceneEntry subSceneEntry = this.subScenes.Find((SubSceneEntry e) => e.sceneID == sceneID);
			if (subSceneEntry == null)
			{
				return null;
			}
			return subSceneEntry.SceneReference;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00067D54 File Offset: 0x00065F54
		private UniTask<bool> LoadSubScene(SceneReference targetScene, bool withBlackScreen = true)
		{
			MultiSceneCore.<LoadSubScene>d__62 <LoadSubScene>d__;
			<LoadSubScene>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadSubScene>d__.<>4__this = this;
			<LoadSubScene>d__.targetScene = targetScene;
			<LoadSubScene>d__.withBlackScreen = withBlackScreen;
			<LoadSubScene>d__.<>1__state = -1;
			<LoadSubScene>d__.<>t__builder.Start<MultiSceneCore.<LoadSubScene>d__62>(ref <LoadSubScene>d__);
			return <LoadSubScene>d__.<>t__builder.Task;
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00067DA8 File Offset: 0x00065FA8
		private void LocalOnSubSceneWillBeUnloaded(Scene scene)
		{
			this.subScenes.Find((SubSceneEntry e) => e != null && e.Info.BuildIndex == scene.buildIndex);
			Transform setActiveWithSceneParent = this.GetSetActiveWithSceneParent(scene.buildIndex);
			Debug.Log(string.Format("Setting Active False {0}  {1}", setActiveWithSceneParent.name, scene.buildIndex));
			setActiveWithSceneParent.gameObject.SetActive(false);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00067E20 File Offset: 0x00066020
		private void LocalOnSubSceneLoaded(Scene scene)
		{
			this.subScenes.Find((SubSceneEntry e) => e != null && e.Info.BuildIndex == scene.buildIndex);
			this.GetSetActiveWithSceneParent(scene.buildIndex).gameObject.SetActive(true);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00067E70 File Offset: 0x00066070
		public UniTask<bool> LoadAndTeleport(MultiSceneLocation location)
		{
			MultiSceneCore.<LoadAndTeleport>d__65 <LoadAndTeleport>d__;
			<LoadAndTeleport>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAndTeleport>d__.<>4__this = this;
			<LoadAndTeleport>d__.location = location;
			<LoadAndTeleport>d__.<>1__state = -1;
			<LoadAndTeleport>d__.<>t__builder.Start<MultiSceneCore.<LoadAndTeleport>d__65>(ref <LoadAndTeleport>d__);
			return <LoadAndTeleport>d__.<>t__builder.Task;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00067EBC File Offset: 0x000660BC
		public UniTask<bool> LoadAndTeleport(string sceneID, Vector3 position, bool subSceneLocation = false)
		{
			MultiSceneCore.<LoadAndTeleport>d__66 <LoadAndTeleport>d__;
			<LoadAndTeleport>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAndTeleport>d__.<>4__this = this;
			<LoadAndTeleport>d__.sceneID = sceneID;
			<LoadAndTeleport>d__.position = position;
			<LoadAndTeleport>d__.subSceneLocation = subSceneLocation;
			<LoadAndTeleport>d__.<>1__state = -1;
			<LoadAndTeleport>d__.<>t__builder.Start<MultiSceneCore.<LoadAndTeleport>d__66>(ref <LoadAndTeleport>d__);
			return <LoadAndTeleport>d__.<>t__builder.Task;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00067F18 File Offset: 0x00066118
		public static void MoveToMainScene(GameObject gameObject)
		{
			if (MultiSceneCore.Instance == null)
			{
				Debug.LogError("移动到主场景失败，因为MultiSceneCore不存在");
				return;
			}
			SceneManager.MoveGameObjectToScene(gameObject, MultiSceneCore.MainScene.Value);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00067F50 File Offset: 0x00066150
		public void CacheLocations()
		{
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00067F52 File Offset: 0x00066152
		public void CacheTeleporters()
		{
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00067F54 File Offset: 0x00066154
		private Vector3 GetClosestTeleporterPosition(Vector3 pos)
		{
			float num = float.MaxValue;
			Vector3 result = pos;
			foreach (SubSceneEntry subSceneEntry in this.subScenes)
			{
				foreach (SubSceneEntry.TeleporterInfo teleporterInfo in subSceneEntry.cachedTeleporters)
				{
					float magnitude = (teleporterInfo.position - pos).magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						result = teleporterInfo.position;
					}
				}
			}
			return result;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x0006800C File Offset: 0x0006620C
		internal bool TryGetCachedPosition(MultiSceneLocation location, out Vector3 result)
		{
			return this.TryGetCachedPosition(location.SceneID, location.LocationName, out result);
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x00068024 File Offset: 0x00066224
		internal bool TryGetCachedPosition(string sceneID, string locationName, out Vector3 result)
		{
			result = default(Vector3);
			SubSceneEntry subSceneEntry = this.subScenes.Find((SubSceneEntry e) => e != null && e.sceneID == sceneID);
			return subSceneEntry != null && subSceneEntry.TryGetCachedPosition(locationName, out result);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00068070 File Offset: 0x00066270
		internal SubSceneEntry GetSubSceneInfo(Scene scene)
		{
			return this.subScenes.Find((SubSceneEntry e) => e != null && e.Info != null && e.Info.BuildIndex == scene.buildIndex);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000680A1 File Offset: 0x000662A1
		public SubSceneEntry GetSubSceneInfo()
		{
			return this.cachedSubsceneEntry;
		}

		// Token: 0x04001421 RID: 5153
		[SerializeField]
		private string levelStateName = "None";

		// Token: 0x04001422 RID: 5154
		[SerializeField]
		private string playStinger = "";

		// Token: 0x04001423 RID: 5155
		[SerializeField]
		private bool playAfterLevelInit;

		// Token: 0x04001424 RID: 5156
		[SerializeField]
		private List<SubSceneEntry> subScenes;

		// Token: 0x04001425 RID: 5157
		private Scene activeSubScene;

		// Token: 0x04001426 RID: 5158
		[HideInInspector]
		public List<int> usedCreatorIds = new List<int>();

		// Token: 0x04001427 RID: 5159
		[HideInInspector]
		public Dictionary<int, object> inLevelData = new Dictionary<int, object>();

		// Token: 0x0400142A RID: 5162
		[SerializeField]
		private bool teleportToRandomOnLevelInitialized;

		// Token: 0x0400142B RID: 5163
		private Dictionary<int, GameObject> setActiveWithSceneObjects = new Dictionary<int, GameObject>();

		// Token: 0x0400142F RID: 5167
		private bool isLoading;

		// Token: 0x04001430 RID: 5168
		private SubSceneEntry cachedSubsceneEntry;
	}
}
