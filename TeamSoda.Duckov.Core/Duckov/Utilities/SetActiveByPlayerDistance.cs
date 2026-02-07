using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Duckov.Utilities
{
	// Token: 0x02000417 RID: 1047
	public class SetActiveByPlayerDistance : MonoBehaviour
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x00084032 File Offset: 0x00082232
		// (set) Token: 0x06002607 RID: 9735 RVA: 0x00084039 File Offset: 0x00082239
		public static SetActiveByPlayerDistance Instance { get; private set; }

		// Token: 0x06002608 RID: 9736 RVA: 0x00084044 File Offset: 0x00082244
		private static List<GameObject> GetListByScene(int sceneBuildIndex, bool createIfNotExist = true)
		{
			List<GameObject> result;
			if (SetActiveByPlayerDistance.listsOfScenes.TryGetValue(sceneBuildIndex, out result))
			{
				return result;
			}
			if (createIfNotExist)
			{
				List<GameObject> list = new List<GameObject>();
				SetActiveByPlayerDistance.listsOfScenes[sceneBuildIndex] = list;
				return list;
			}
			return null;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0008407A File Offset: 0x0008227A
		private static List<GameObject> GetListByScene(Scene scene, bool createIfNotExist = true)
		{
			return SetActiveByPlayerDistance.GetListByScene(scene.buildIndex, createIfNotExist);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x00084089 File Offset: 0x00082289
		public static void Register(GameObject gameObject, int sceneBuildIndex)
		{
			SetActiveByPlayerDistance.GetListByScene(sceneBuildIndex, true).Add(gameObject);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00084098 File Offset: 0x00082298
		public static bool Unregister(GameObject gameObject, int sceneBuildIndex)
		{
			List<GameObject> listByScene = SetActiveByPlayerDistance.GetListByScene(sceneBuildIndex, false);
			return listByScene != null && listByScene.Remove(gameObject);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000840B9 File Offset: 0x000822B9
		public static void Register(GameObject gameObject, Scene scene)
		{
			SetActiveByPlayerDistance.Register(gameObject, scene.buildIndex);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000840C8 File Offset: 0x000822C8
		public static void Unregister(GameObject gameObject, Scene scene)
		{
			SetActiveByPlayerDistance.Unregister(gameObject, scene.buildIndex);
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x000840D8 File Offset: 0x000822D8
		public float Distance
		{
			get
			{
				return this.distance;
			}
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000840E0 File Offset: 0x000822E0
		private void Awake()
		{
			if (SetActiveByPlayerDistance.Instance == null)
			{
				SetActiveByPlayerDistance.Instance = this;
			}
			this.CleanUp();
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			this.cachedActiveScene = SceneManager.GetActiveScene();
			this.RefreshCache();
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x00084120 File Offset: 0x00082320
		private void CleanUp()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, List<GameObject>> keyValuePair in SetActiveByPlayerDistance.listsOfScenes)
			{
				List<GameObject> value = keyValuePair.Value;
				value.RemoveAll((GameObject e) => e == null);
				if (value == null || value.Count < 1)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (int key in list)
			{
				SetActiveByPlayerDistance.listsOfScenes.Remove(key);
			}
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00084200 File Offset: 0x00082400
		private void OnActiveSceneChanged(Scene prev, Scene cur)
		{
			this.RefreshCache();
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x00084208 File Offset: 0x00082408
		private void RefreshCache()
		{
			this.cachedActiveScene = SceneManager.GetActiveScene();
			this.cachedListRef = SetActiveByPlayerDistance.GetListByScene(this.cachedActiveScene, true);
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x00084227 File Offset: 0x00082427
		private Transform PlayerTransform
		{
			get
			{
				if (!this.cachedPlayerTransform)
				{
					CharacterMainControl main = CharacterMainControl.Main;
					this.cachedPlayerTransform = ((main != null) ? main.transform : null);
				}
				return this.cachedPlayerTransform;
			}
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00084254 File Offset: 0x00082454
		private void FixedUpdate()
		{
			if (this.PlayerTransform == null)
			{
				return;
			}
			if (this.cachedListRef == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.cachedListRef)
			{
				if (!(gameObject == null))
				{
					bool active = (this.PlayerTransform.position - gameObject.transform.position).sqrMagnitude < this.distance * this.distance;
					gameObject.gameObject.SetActive(active);
				}
			}
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x00084300 File Offset: 0x00082500
		private void DebugRegister(GameObject go)
		{
			SetActiveByPlayerDistance.Register(go, go.gameObject.scene);
		}

		// Token: 0x040019F1 RID: 6641
		private static Dictionary<int, List<GameObject>> listsOfScenes = new Dictionary<int, List<GameObject>>();

		// Token: 0x040019F2 RID: 6642
		[SerializeField]
		private float distance = 100f;

		// Token: 0x040019F3 RID: 6643
		private Scene cachedActiveScene;

		// Token: 0x040019F4 RID: 6644
		private List<GameObject> cachedListRef;

		// Token: 0x040019F5 RID: 6645
		private Transform cachedPlayerTransform;
	}
}
