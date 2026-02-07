using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MeshCombineStudio
{
	// Token: 0x0200003F RID: 63
	public static class Methods
	{
		// Token: 0x06000156 RID: 342 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		public static HideFlags CustomToHideFlags(CustomHideFlags customHideFlags)
		{
			HideFlags hideFlags = HideFlags.None;
			if ((customHideFlags & CustomHideFlags.HideInHierarchy) != (CustomHideFlags)0)
			{
				hideFlags |= HideFlags.HideInHierarchy;
			}
			if ((customHideFlags & CustomHideFlags.HideInInspector) != (CustomHideFlags)0)
			{
				hideFlags |= HideFlags.HideInInspector;
			}
			if ((customHideFlags & CustomHideFlags.DontSaveInEditor) != (CustomHideFlags)0)
			{
				hideFlags |= HideFlags.DontSaveInEditor;
			}
			if ((customHideFlags & CustomHideFlags.NotEditable) != (CustomHideFlags)0)
			{
				hideFlags |= HideFlags.NotEditable;
			}
			if ((customHideFlags & CustomHideFlags.DontSaveInBuild) != (CustomHideFlags)0)
			{
				hideFlags |= HideFlags.DontSaveInBuild;
			}
			if ((customHideFlags & CustomHideFlags.DontUnloadUnusedAsset) != (CustomHideFlags)0)
			{
				hideFlags |= HideFlags.DontUnloadUnusedAsset;
			}
			return hideFlags;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000D144 File Offset: 0x0000B344
		public static CustomHideFlags HideFlagsToCustom(HideFlags hideFlags)
		{
			CustomHideFlags customHideFlags = (CustomHideFlags)0;
			if ((hideFlags & HideFlags.HideInHierarchy) != HideFlags.None)
			{
				customHideFlags |= CustomHideFlags.HideInHierarchy;
			}
			if ((hideFlags & HideFlags.HideInInspector) != HideFlags.None)
			{
				customHideFlags |= CustomHideFlags.HideInInspector;
			}
			if ((hideFlags & HideFlags.DontSaveInEditor) != HideFlags.None)
			{
				customHideFlags |= CustomHideFlags.DontSaveInEditor;
			}
			if ((hideFlags & HideFlags.NotEditable) != HideFlags.None)
			{
				customHideFlags |= CustomHideFlags.NotEditable;
			}
			if ((hideFlags & HideFlags.DontSaveInBuild) != HideFlags.None)
			{
				customHideFlags |= CustomHideFlags.DontSaveInBuild;
			}
			if ((hideFlags & HideFlags.DontUnloadUnusedAsset) != HideFlags.None)
			{
				customHideFlags |= CustomHideFlags.DontUnloadUnusedAsset;
			}
			return customHideFlags;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000D190 File Offset: 0x0000B390
		public static int GetFirstLayerOfLayerMask(LayerMask layerMask)
		{
			for (int i = 0; i < 32; i++)
			{
				int result = 1 << i;
				if ((i & layerMask) != 0)
				{
					return result;
				}
			}
			return -1;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000D1BE File Offset: 0x0000B3BE
		public static bool IsLayerInLayerMask(LayerMask layerMask, int layer)
		{
			return layerMask == (layerMask | 1 << layer);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000D1D8 File Offset: 0x0000B3D8
		public static void SetMeshRenderersActive(FastList<MeshRenderer> mrs, bool active)
		{
			for (int i = 0; i < mrs.Count; i++)
			{
				mrs.items[i].enabled = active;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000D204 File Offset: 0x0000B404
		public static void SetCachedGOSActive(FastList<CachedGameObject> cachedGOS, bool active)
		{
			for (int i = 0; i < cachedGOS.Count; i++)
			{
				cachedGOS.items[i].mr.enabled = active;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000D238 File Offset: 0x0000B438
		public static void SetTag(GameObject go, string tag)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].tag = tag;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000D264 File Offset: 0x0000B464
		public static void SetTagWhenCollider(GameObject go, string tag)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].GetComponent<Collider>() != null)
				{
					componentsInChildren[i].tag = tag;
				}
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public static void SetTagAndLayer(GameObject go, string tag, int layer)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].tag = tag;
				componentsInChildren[i].gameObject.layer = layer;
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000D2DC File Offset: 0x0000B4DC
		public static void SetLayer(GameObject go, int layer)
		{
			go.layer = layer;
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000D313 File Offset: 0x0000B513
		public static bool LayerMaskContainsLayer(int layerMask, int layer)
		{
			return (1 << layer & layerMask) != 0;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000D320 File Offset: 0x0000B520
		public static int GetFirstLayerInLayerMask(int layerMask)
		{
			for (int i = 0; i < 32; i++)
			{
				if ((layerMask & Mathw.bits[i]) != 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000D348 File Offset: 0x0000B548
		public static bool Contains(string compare, string name)
		{
			List<string> list = new List<string>();
			int num;
			do
			{
				num = name.IndexOf("*");
				if (num != -1)
				{
					if (num != 0)
					{
						list.Add(name.Substring(0, num));
					}
					if (num == name.Length - 1)
					{
						break;
					}
					name = name.Substring(num + 1);
				}
			}
			while (num != -1);
			list.Add(name);
			for (int i = 0; i < list.Count; i++)
			{
				if (!compare.Contains(list[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		public static T[] Search<T>(GameObject parentGO = null)
		{
			GameObject[] array;
			if (parentGO == null)
			{
				array = SceneManager.GetActiveScene().GetRootGameObjects();
			}
			else
			{
				array = new GameObject[]
				{
					parentGO
				};
			}
			if (array == null)
			{
				return null;
			}
			if (typeof(T) == typeof(GameObject))
			{
				List<GameObject> list = new List<GameObject>();
				for (int i = 0; i < array.Length; i++)
				{
					Transform[] componentsInChildren = array[i].GetComponentsInChildren<Transform>(true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						list.Add(componentsInChildren[j].gameObject);
					}
				}
				return list.ToArray() as T[];
			}
			if (parentGO == null)
			{
				List<T> list2 = new List<T>();
				for (int k = 0; k < array.Length; k++)
				{
					list2.AddRange(array[k].GetComponentsInChildren<T>(true));
				}
				return list2.ToArray();
			}
			return parentGO.GetComponentsInChildren<T>(true);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000D4A4 File Offset: 0x0000B6A4
		public static FastList<GameObject> GetAllRootGameObjects()
		{
			FastList<GameObject> fastList = new FastList<GameObject>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					fastList.AddRange(sceneAt.GetRootGameObjects());
				}
			}
			return fastList;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		public static T[] SearchParent<T>(GameObject parentGO, bool searchInActiveGameObjects) where T : Component
		{
			if (parentGO == null)
			{
				return Methods.SearchAllScenes<T>(searchInActiveGameObjects).ToArray();
			}
			if (!searchInActiveGameObjects && !parentGO.activeInHierarchy)
			{
				return null;
			}
			if (typeof(T) == typeof(GameObject))
			{
				Transform[] componentsInChildren = parentGO.GetComponentsInChildren<Transform>(searchInActiveGameObjects);
				GameObject[] array = new GameObject[componentsInChildren.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = componentsInChildren[i].gameObject;
				}
				return array as T[];
			}
			return parentGO.GetComponentsInChildren<T>(searchInActiveGameObjects);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000D56C File Offset: 0x0000B76C
		public static T[] SearchScene<T>(Scene scene, bool searchInActiveGameObjects) where T : Component
		{
			if (!scene.isLoaded)
			{
				return null;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			FastList<T> fastList = new FastList<T>();
			foreach (GameObject parentGO in rootGameObjects)
			{
				fastList.AddRange(Methods.SearchParent<T>(parentGO, searchInActiveGameObjects));
			}
			return fastList.ToArray();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
		public static FastList<T> SearchAllScenes<T>(bool searchInActiveGameObjects) where T : Component
		{
			FastList<T> fastList = new FastList<T>();
			FastList<GameObject> allRootGameObjects = Methods.GetAllRootGameObjects();
			for (int i = 0; i < allRootGameObjects.Count; i++)
			{
				T[] arrayItems = Methods.SearchParent<T>(allRootGameObjects.items[i], searchInActiveGameObjects);
				fastList.AddRange(arrayItems);
			}
			return fastList;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		public static T Find<T>(GameObject parentGO, string name) where T : Component
		{
			T[] array = Methods.SearchParent<T>(parentGO, true);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name == name)
				{
					return array[i];
				}
			}
			return default(T);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000D64C File Offset: 0x0000B84C
		public static void SetCollidersActive(Collider[] colliders, bool active, string[] nameList)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				for (int j = 0; j < nameList.Length; j++)
				{
					if (colliders[i].name.Contains(nameList[j]))
					{
						colliders[i].enabled = active;
					}
				}
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000D690 File Offset: 0x0000B890
		public static void SelectChildrenWithMeshRenderer(GameObject[] parentGOs)
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000D692 File Offset: 0x0000B892
		public static void SelectChildrenWithMeshRenderer(Transform t)
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000D694 File Offset: 0x0000B894
		public static void DestroyChildren(Transform t)
		{
			while (t.childCount > 0)
			{
				Transform child = t.GetChild(0);
				child.parent = null;
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000D6B9 File Offset: 0x0000B8B9
		public static void Destroy(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(go);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000D6CB File Offset: 0x0000B8CB
		public static void Destroy(Component c)
		{
			if (c == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(c);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		public static void SetChildrenActive(Transform t, bool active)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				t.GetChild(i).gameObject.SetActive(active);
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000D710 File Offset: 0x0000B910
		public static void SnapBoundsAndPreserveArea(ref Bounds bounds, float snapSize, Vector3 offset)
		{
			Vector3 vector = Mathw.Snap(bounds.center, snapSize) + offset;
			bounds.size += Mathw.Abs(vector - bounds.center) * 2f;
			bounds.center = vector;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000D763 File Offset: 0x0000B963
		public static void ListRemoveAt<T>(List<T> list, int index)
		{
			list[index] = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000D788 File Offset: 0x0000B988
		public static void CopyComponent(Component component, GameObject target)
		{
			Type type = component.GetType();
			target.AddComponent(type);
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
			{
				propertyInfo.SetValue(target.GetComponent(type), propertyInfo.GetValue(component, null), null);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
		public static Transform GetChildRootTransform(Transform t, Transform rootT)
		{
			MCSDynamicObject componentInParent = t.GetComponentInParent<MCSDynamicObject>();
			if (componentInParent)
			{
				return componentInParent.transform;
			}
			return rootT;
		}
	}
}
