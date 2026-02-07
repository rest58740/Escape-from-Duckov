using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Scenes
{
	// Token: 0x02000347 RID: 839
	[ExecuteAlways]
	public class SceneLocationsProvider : MonoBehaviour
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x000683FD File Offset: 0x000665FD
		public static ReadOnlyCollection<SceneLocationsProvider> ActiveProviders
		{
			get
			{
				if (SceneLocationsProvider._activeProviders_ReadOnly == null)
				{
					SceneLocationsProvider._activeProviders_ReadOnly = new ReadOnlyCollection<SceneLocationsProvider>(SceneLocationsProvider.activeProviders);
				}
				return SceneLocationsProvider._activeProviders_ReadOnly;
			}
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0006841C File Offset: 0x0006661C
		public static SceneLocationsProvider GetProviderOfScene(SceneReference sceneReference)
		{
			if (sceneReference == null)
			{
				return null;
			}
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == sceneReference.BuildIndex);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00068458 File Offset: 0x00066658
		public static SceneLocationsProvider GetProviderOfScene(Scene scene)
		{
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == scene.buildIndex);
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x00068488 File Offset: 0x00066688
		internal static SceneLocationsProvider GetProviderOfScene(int sceneBuildIndex)
		{
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == sceneBuildIndex);
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000684B8 File Offset: 0x000666B8
		public static Transform GetLocation(SceneReference scene, string name)
		{
			if (scene.UnsafeReason != null)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(scene.BuildIndex, name);
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000684D0 File Offset: 0x000666D0
		public static Transform GetLocation(int sceneBuildIndex, string name)
		{
			SceneLocationsProvider providerOfScene = SceneLocationsProvider.GetProviderOfScene(sceneBuildIndex);
			if (providerOfScene == null)
			{
				return null;
			}
			return providerOfScene.GetLocation(name);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000684F8 File Offset: 0x000666F8
		public static Transform GetLocation(string sceneID, string name)
		{
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(sceneID);
			if (sceneInfo == null)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(sceneInfo.BuildIndex, name);
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0006851D File Offset: 0x0006671D
		private void Awake()
		{
			SceneLocationsProvider.activeProviders.Add(this);
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0006852A File Offset: 0x0006672A
		private void OnDestroy()
		{
			SceneLocationsProvider.activeProviders.Remove(this);
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00068538 File Offset: 0x00066738
		public Transform GetLocation(string path)
		{
			string[] array = path.Split('/', StringSplitOptions.None);
			Transform transform = base.transform;
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					transform = transform.Find(text);
					if (transform == null)
					{
						return null;
					}
				}
			}
			return transform;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x00068584 File Offset: 0x00066784
		public bool TryGetPath(Transform value, out string path)
		{
			path = "";
			Transform transform = value;
			List<Transform> list = new List<Transform>();
			while (transform != null && transform != base.transform)
			{
				list.Insert(0, transform);
				transform = transform.parent;
			}
			if (transform != base.transform)
			{
				return false;
			}
			this.sb.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					this.sb.Append('/');
				}
				this.sb.Append(list[i].name);
			}
			path = this.sb.ToString();
			return true;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00068630 File Offset: 0x00066830
		[return: TupleElementNames(new string[]
		{
			"path",
			"worldPosition",
			"gameObject"
		})]
		public List<ValueTuple<string, Vector3, GameObject>> GetAllPathsAndItsPosition()
		{
			List<ValueTuple<string, Vector3, GameObject>> list = new List<ValueTuple<string, Vector3, GameObject>>();
			Stack<Transform> stack = new Stack<Transform>();
			stack.Push(base.transform);
			while (stack.Count > 0)
			{
				Transform transform = stack.Pop();
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = transform.GetChild(i);
					string item;
					if (this.TryGetPath(child, out item))
					{
						list.Add(new ValueTuple<string, Vector3, GameObject>(item, child.transform.position, child.gameObject));
						stack.Push(child);
					}
				}
			}
			return list;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000686C0 File Offset: 0x000668C0
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			foreach (Transform transform in base.transform.GetComponentsInChildren<Transform>())
			{
				if (transform.childCount == 0)
				{
					Gizmos.DrawSphere(transform.position, 1.5f);
				}
			}
		}

		// Token: 0x0400143D RID: 5181
		private static List<SceneLocationsProvider> activeProviders = new List<SceneLocationsProvider>();

		// Token: 0x0400143E RID: 5182
		private static ReadOnlyCollection<SceneLocationsProvider> _activeProviders_ReadOnly;

		// Token: 0x0400143F RID: 5183
		private StringBuilder sb = new StringBuilder();
	}
}
