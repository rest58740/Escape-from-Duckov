using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ES3Internal
{
	// Token: 0x020000D5 RID: 213
	[DisallowMultipleComponent]
	[Serializable]
	public abstract class ES3ReferenceMgrBase : MonoBehaviour
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001B0F0 File Offset: 0x000192F0
		public static ES3ReferenceMgrBase Current
		{
			get
			{
				if (ES3ReferenceMgrBase._current == null)
				{
					ES3ReferenceMgrBase managerFromScene = ES3ReferenceMgrBase.GetManagerFromScene(SceneManager.GetActiveScene());
					if (managerFromScene != null)
					{
						ES3ReferenceMgrBase.mgrs.Add(ES3ReferenceMgrBase._current = managerFromScene);
					}
				}
				return ES3ReferenceMgrBase._current;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001B138 File Offset: 0x00019338
		public static ES3ReferenceMgrBase GetManagerFromScene(Scene scene)
		{
			GameObject[] rootGameObjects;
			try
			{
				rootGameObjects = scene.GetRootGameObjects();
			}
			catch
			{
				return null;
			}
			ES3ReferenceMgr es3ReferenceMgr = null;
			foreach (GameObject gameObject in rootGameObjects)
			{
				if (gameObject.name == "Easy Save 3 Manager")
				{
					es3ReferenceMgr = gameObject.GetComponent<ES3ReferenceMgr>();
					break;
				}
			}
			if (es3ReferenceMgr == null)
			{
				GameObject[] array = rootGameObjects;
				int i = 0;
				while (i < array.Length && !((es3ReferenceMgr = array[i].GetComponentInChildren<ES3ReferenceMgr>()) != null))
				{
					i++;
				}
			}
			return es3ReferenceMgr;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0001B1D0 File Offset: 0x000193D0
		public bool IsInitialised
		{
			get
			{
				return this.idRef.Count > 0;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0001B1E0 File Offset: 0x000193E0
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0001B270 File Offset: 0x00019470
		public ES3RefIdDictionary refId
		{
			get
			{
				if (this._refId == null)
				{
					this._refId = new ES3RefIdDictionary();
					foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in this.idRef)
					{
						if (keyValuePair.Value != null)
						{
							this._refId[keyValuePair.Value] = keyValuePair.Key;
						}
					}
				}
				return this._refId;
			}
			set
			{
				this._refId = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0001B279 File Offset: 0x00019479
		public ES3GlobalReferences GlobalReferences
		{
			get
			{
				return ES3GlobalReferences.Instance;
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001B280 File Offset: 0x00019480
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
			ES3ReferenceMgrBase._current = null;
			ES3ReferenceMgrBase.mgrs = new HashSet<ES3ReferenceMgrBase>();
			ES3ReferenceMgrBase.rng = null;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001B298 File Offset: 0x00019498
		internal void Awake()
		{
			if (ES3ReferenceMgrBase._current != null && ES3ReferenceMgrBase._current != this)
			{
				ES3ReferenceMgrBase current = ES3ReferenceMgrBase._current;
				if (ES3ReferenceMgrBase.Current != null)
				{
					this.RemoveNullValues();
					ES3ReferenceMgrBase._current = current;
				}
			}
			else
			{
				ES3ReferenceMgrBase._current = this;
			}
			ES3ReferenceMgrBase.mgrs.Add(this);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001B2F2 File Offset: 0x000194F2
		private void OnDestroy()
		{
			if (ES3ReferenceMgrBase._current == this)
			{
				ES3ReferenceMgrBase._current = null;
			}
			ES3ReferenceMgrBase.mgrs.Remove(this);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001B314 File Offset: 0x00019514
		public void Merge(ES3ReferenceMgrBase otherMgr)
		{
			foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in otherMgr.idRef)
			{
				this.Add(keyValuePair.Value, keyValuePair.Key);
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001B378 File Offset: 0x00019578
		public long Get(UnityEngine.Object obj)
		{
			if (!ES3ReferenceMgrBase.mgrs.Contains(this))
			{
				ES3ReferenceMgrBase.mgrs.Add(this);
			}
			foreach (ES3ReferenceMgrBase es3ReferenceMgrBase in ES3ReferenceMgrBase.mgrs)
			{
				if (!(es3ReferenceMgrBase == null))
				{
					if (obj == null)
					{
						return -1L;
					}
					long result;
					if (es3ReferenceMgrBase.refId.TryGetValue(obj, out result))
					{
						return result;
					}
				}
			}
			return -1L;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001B40C File Offset: 0x0001960C
		internal UnityEngine.Object Get(long id, Type type, bool suppressWarnings = false)
		{
			if (!ES3ReferenceMgrBase.mgrs.Contains(this))
			{
				ES3ReferenceMgrBase.mgrs.Add(this);
			}
			foreach (ES3ReferenceMgrBase es3ReferenceMgrBase in ES3ReferenceMgrBase.mgrs)
			{
				if (!(es3ReferenceMgrBase == null))
				{
					if (id == -1L)
					{
						return null;
					}
					UnityEngine.Object @object;
					if (es3ReferenceMgrBase.idRef.TryGetValue(id, out @object))
					{
						if (@object == null)
						{
							return null;
						}
						return @object;
					}
				}
			}
			if (this.GlobalReferences != null)
			{
				UnityEngine.Object object2 = this.GlobalReferences.Get(id);
				if (object2 != null)
				{
					return object2;
				}
			}
			if (type != null)
			{
				ES3Debug.LogWarning(string.Concat(new string[]
				{
					"Reference for ",
					(type != null) ? type.ToString() : null,
					" with ID ",
					id.ToString(),
					" could not be found in Easy Save's reference manager. See <a href=\"https://docs.moodkie.com/easy-save-3/es3-guides/saving-and-loading-references/#reference-could-not-be-found-warning\">the Saving and Loading References guide</a> for more information."
				}), this, 0);
			}
			else
			{
				ES3Debug.LogWarning("Reference with ID " + id.ToString() + " could not be found in Easy Save's reference manager. See <a href=\"https://docs.moodkie.com/easy-save-3/es3-guides/saving-and-loading-references/#reference-could-not-be-found-warning\">the Saving and Loading References guide</a> for more information.", this, 0);
			}
			return null;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001B544 File Offset: 0x00019744
		public UnityEngine.Object Get(long id, bool suppressWarnings = false)
		{
			return this.Get(id, null, suppressWarnings);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001B550 File Offset: 0x00019750
		public ES3Prefab GetPrefab(long id, bool suppressWarnings = false)
		{
			if (!ES3ReferenceMgrBase.mgrs.Contains(this))
			{
				ES3ReferenceMgrBase.mgrs.Add(this);
			}
			foreach (ES3ReferenceMgrBase es3ReferenceMgrBase in ES3ReferenceMgrBase.mgrs)
			{
				if (!(es3ReferenceMgrBase == null))
				{
					foreach (ES3Prefab es3Prefab in es3ReferenceMgrBase.prefabs)
					{
						if (es3Prefab != null && es3Prefab.prefabId == id)
						{
							return es3Prefab;
						}
					}
				}
			}
			if (!suppressWarnings)
			{
				ES3Debug.LogWarning("Prefab with ID " + id.ToString() + " could not be found in Easy Save's reference manager. Try pressing the Refresh References button on the ES3ReferenceMgr Component of the Easy Save 3 Manager in your scene, or exit play mode and right-click the prefab and select Easy Save 3 > Add Reference(s) to Manager.", this, 0);
			}
			return null;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001B634 File Offset: 0x00019834
		public long GetPrefab(ES3Prefab prefabToFind, bool suppressWarnings = false)
		{
			if (!ES3ReferenceMgrBase.mgrs.Contains(this))
			{
				ES3ReferenceMgrBase.mgrs.Add(this);
			}
			using (HashSet<ES3ReferenceMgrBase>.Enumerator enumerator = ES3ReferenceMgrBase.mgrs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current == null))
					{
						foreach (ES3Prefab es3Prefab in this.prefabs)
						{
							if (es3Prefab == prefabToFind)
							{
								return es3Prefab.prefabId;
							}
						}
					}
				}
			}
			if (!suppressWarnings)
			{
				ES3Debug.LogWarning("Prefab with name " + prefabToFind.name + " could not be found in Easy Save's reference manager. Try pressing the Refresh References button on the ES3ReferenceMgr Component of the Easy Save 3 Manager in your scene, or exit play mode and right-click the prefab and select Easy Save 3 > Add Reference(s) to Manager.", prefabToFind, 0);
			}
			return -1L;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001B710 File Offset: 0x00019910
		public long Add(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return -1L;
			}
			if (!ES3ReferenceMgrBase.CanBeSaved(obj))
			{
				return -1L;
			}
			long num;
			if (this.refId.TryGetValue(obj, out num))
			{
				return num;
			}
			if (this.GlobalReferences != null)
			{
				num = this.GlobalReferences.GetOrAdd(obj);
				if (num != -1L)
				{
					this.Add(obj, num);
					return num;
				}
			}
			object @lock = this._lock;
			long result;
			lock (@lock)
			{
				num = ES3ReferenceMgrBase.GetNewRefID();
				result = this.Add(obj, num);
			}
			return result;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001B7B0 File Offset: 0x000199B0
		public long Add(UnityEngine.Object obj, long id)
		{
			if (obj == null)
			{
				return -1L;
			}
			if (!ES3ReferenceMgrBase.CanBeSaved(obj))
			{
				return -1L;
			}
			if (id == -1L)
			{
				id = ES3ReferenceMgrBase.GetNewRefID();
			}
			object @lock = this._lock;
			lock (@lock)
			{
				this.idRef[id] = obj;
				if (obj != null)
				{
					this.refId[obj] = id;
				}
			}
			return id;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001B834 File Offset: 0x00019A34
		public bool AddPrefab(ES3Prefab prefab)
		{
			if (!this.prefabs.Contains(prefab))
			{
				this.prefabs.Add(prefab);
				return true;
			}
			return false;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001B854 File Offset: 0x00019A54
		public void Remove(UnityEngine.Object obj)
		{
			if (!ES3ReferenceMgrBase.mgrs.Contains(this))
			{
				ES3ReferenceMgrBase.mgrs.Add(this);
			}
			Func<KeyValuePair<long, UnityEngine.Object>, bool> <>9__0;
			foreach (ES3ReferenceMgrBase es3ReferenceMgrBase in ES3ReferenceMgrBase.mgrs)
			{
				if (!(es3ReferenceMgrBase == null) && (Application.isPlaying || !(es3ReferenceMgrBase != this)))
				{
					object @lock = es3ReferenceMgrBase._lock;
					lock (@lock)
					{
						es3ReferenceMgrBase.refId.Remove(obj);
						IEnumerable<KeyValuePair<long, UnityEngine.Object>> source = es3ReferenceMgrBase.idRef;
						Func<KeyValuePair<long, UnityEngine.Object>, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((KeyValuePair<long, UnityEngine.Object> kvp) => kvp.Value == obj));
						}
						foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in source.Where(predicate).ToList<KeyValuePair<long, UnityEngine.Object>>())
						{
							es3ReferenceMgrBase.idRef.Remove(keyValuePair.Key);
						}
					}
				}
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001B9A0 File Offset: 0x00019BA0
		public void Remove(long referenceID)
		{
			Func<KeyValuePair<UnityEngine.Object, long>, bool> <>9__0;
			foreach (ES3ReferenceMgrBase es3ReferenceMgrBase in ES3ReferenceMgrBase.mgrs)
			{
				if (!(es3ReferenceMgrBase == null))
				{
					object @lock = es3ReferenceMgrBase._lock;
					lock (@lock)
					{
						es3ReferenceMgrBase.idRef.Remove(referenceID);
						IEnumerable<KeyValuePair<UnityEngine.Object, long>> refId = es3ReferenceMgrBase.refId;
						Func<KeyValuePair<UnityEngine.Object, long>, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((KeyValuePair<UnityEngine.Object, long> kvp) => kvp.Value == referenceID));
						}
						foreach (KeyValuePair<UnityEngine.Object, long> keyValuePair in refId.Where(predicate).ToList<KeyValuePair<UnityEngine.Object, long>>())
						{
							es3ReferenceMgrBase.refId.Remove(keyValuePair.Key);
						}
					}
				}
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001BAC0 File Offset: 0x00019CC0
		public void RemoveNullValues()
		{
			foreach (long key in (from pair in this.idRef
			where pair.Value == null
			select pair.Key).ToList<long>())
			{
				this.idRef.Remove(key);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001BB68 File Offset: 0x00019D68
		public void RemoveNullOrInvalidValues()
		{
			foreach (long key in (from pair in this.idRef
			where pair.Value == null || !ES3ReferenceMgrBase.CanBeSaved(pair.Value) || this.excludeObjects.Contains(pair.Value)
			select pair.Key).ToList<long>())
			{
				this.idRef.Remove(key);
			}
			if (this.GlobalReferences != null)
			{
				this.GlobalReferences.RemoveInvalidKeys();
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001BC14 File Offset: 0x00019E14
		public void Clear()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this.refId.Clear();
				this.idRef.Clear();
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0001BC64 File Offset: 0x00019E64
		public bool Contains(UnityEngine.Object obj)
		{
			return this.refId.ContainsKey(obj);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001BC72 File Offset: 0x00019E72
		public bool Contains(long referenceID)
		{
			return this.idRef.ContainsKey(referenceID);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001BC80 File Offset: 0x00019E80
		public void ChangeId(long oldId, long newId)
		{
			this.idRef.ChangeKey(oldId, newId);
			this.refId = null;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001BC98 File Offset: 0x00019E98
		internal static long GetNewRefID()
		{
			if (ES3ReferenceMgrBase.rng == null)
			{
				ES3ReferenceMgrBase.rng = new System.Random();
			}
			byte[] array = new byte[8];
			ES3ReferenceMgrBase.rng.NextBytes(array);
			return Math.Abs(BitConverter.ToInt64(array, 0) % long.MaxValue);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001BCDE File Offset: 0x00019EDE
		internal static bool CanBeSaved(UnityEngine.Object obj)
		{
			return true;
		}

		// Token: 0x04000125 RID: 293
		internal object _lock = new object();

		// Token: 0x04000126 RID: 294
		public const string referencePropertyName = "_ES3Ref";

		// Token: 0x04000127 RID: 295
		private static ES3ReferenceMgrBase _current = null;

		// Token: 0x04000128 RID: 296
		private static HashSet<ES3ReferenceMgrBase> mgrs = new HashSet<ES3ReferenceMgrBase>();

		// Token: 0x04000129 RID: 297
		[NonSerialized]
		public List<UnityEngine.Object> excludeObjects = new List<UnityEngine.Object>();

		// Token: 0x0400012A RID: 298
		private static System.Random rng;

		// Token: 0x0400012B RID: 299
		[HideInInspector]
		public bool openPrefabs;

		// Token: 0x0400012C RID: 300
		public List<ES3Prefab> prefabs = new List<ES3Prefab>();

		// Token: 0x0400012D RID: 301
		[SerializeField]
		public ES3IdRefDictionary idRef = new ES3IdRefDictionary();

		// Token: 0x0400012E RID: 302
		private ES3RefIdDictionary _refId;
	}
}
