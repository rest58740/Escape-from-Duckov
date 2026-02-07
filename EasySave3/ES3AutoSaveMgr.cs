using System;
using System.Collections.Generic;
using System.Linq;
using ES3Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000006 RID: 6
[IncludeInSettings(true)]
public class ES3AutoSaveMgr : MonoBehaviour
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000010 RID: 16 RVA: 0x00002248 File Offset: 0x00000448
	public static ES3AutoSaveMgr Current
	{
		get
		{
			if (ES3AutoSaveMgr._current == null)
			{
				GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
				foreach (GameObject gameObject in rootGameObjects)
				{
					if (gameObject.name == "Easy Save 3 Manager")
					{
						return ES3AutoSaveMgr._current = gameObject.GetComponent<ES3AutoSaveMgr>();
					}
				}
				GameObject[] array = rootGameObjects;
				for (int i = 0; i < array.Length; i++)
				{
					if ((ES3AutoSaveMgr._current = array[i].GetComponentInChildren<ES3AutoSaveMgr>()) != null)
					{
						return ES3AutoSaveMgr._current;
					}
				}
			}
			return ES3AutoSaveMgr._current;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000011 RID: 17 RVA: 0x000022D7 File Offset: 0x000004D7
	public static ES3AutoSaveMgr Instance
	{
		get
		{
			return ES3AutoSaveMgr.Current;
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000022E0 File Offset: 0x000004E0
	public void Save()
	{
		if (this.autoSaves == null || this.autoSaves.Count == 0)
		{
			return;
		}
		if (this.settings.location == ES3.Location.Cache && !ES3.FileExists(this.settings))
		{
			ES3.CacheFile(this.settings);
		}
		if (this.autoSaves == null || this.autoSaves.Count == 0)
		{
			ES3.DeleteKey(this.key, this.settings);
		}
		else
		{
			List<GameObject> list = new List<GameObject>();
			foreach (ES3AutoSave es3AutoSave in this.autoSaves)
			{
				if (es3AutoSave != null && es3AutoSave.enabled)
				{
					list.Add(es3AutoSave.gameObject);
				}
			}
			ES3.Save<GameObject[]>(this.key, (from x in list
			orderby ES3AutoSaveMgr.GetDepth(x.transform)
			select x).ToArray<GameObject>(), this.settings);
		}
		if (this.settings.location == ES3.Location.Cache && ES3.FileExists(this.settings))
		{
			ES3.StoreCachedFile(this.settings);
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002418 File Offset: 0x00000618
	public void Load()
	{
		try
		{
			if (this.settings.location == ES3.Location.Cache && !ES3.FileExists(this.settings))
			{
				ES3.CacheFile(this.settings);
			}
		}
		catch
		{
		}
		ES3ReferenceMgrBase.GetManagerFromScene(base.gameObject.scene).Awake();
		ES3.Load<GameObject[]>(this.key, new GameObject[0], this.settings);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002490 File Offset: 0x00000690
	private void Start()
	{
		if (this.loadEvent == ES3AutoSaveMgr.LoadEvent.Start)
		{
			this.Load();
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000024A1 File Offset: 0x000006A1
	public void Awake()
	{
		ES3AutoSaveMgr.managers.Add(base.gameObject.scene, this);
		this.GetAutoSaves();
		if (this.loadEvent == ES3AutoSaveMgr.LoadEvent.Awake)
		{
			this.Load();
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000024CE File Offset: 0x000006CE
	private void OnApplicationQuit()
	{
		if (this.saveEvent == ES3AutoSaveMgr.SaveEvent.OnApplicationQuit)
		{
			this.Save();
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000024DF File Offset: 0x000006DF
	private void OnApplicationPause(bool paused)
	{
		if ((this.saveEvent == ES3AutoSaveMgr.SaveEvent.OnApplicationPause || (Application.isMobilePlatform && this.saveEvent == ES3AutoSaveMgr.SaveEvent.OnApplicationQuit)) && paused)
		{
			this.Save();
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000250C File Offset: 0x0000070C
	public static void AddAutoSave(ES3AutoSave autoSave)
	{
		if (autoSave == null)
		{
			return;
		}
		ES3AutoSaveMgr es3AutoSaveMgr;
		if (ES3AutoSaveMgr.managers.TryGetValue(autoSave.gameObject.scene, out es3AutoSaveMgr))
		{
			es3AutoSaveMgr.autoSaves.Add(autoSave);
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000254C File Offset: 0x0000074C
	public static void RemoveAutoSave(ES3AutoSave autoSave)
	{
		if (autoSave == null)
		{
			return;
		}
		ES3AutoSaveMgr es3AutoSaveMgr;
		if (ES3AutoSaveMgr.managers.TryGetValue(autoSave.gameObject.scene, out es3AutoSaveMgr))
		{
			es3AutoSaveMgr.autoSaves.Remove(autoSave);
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000258C File Offset: 0x0000078C
	public void GetAutoSaves()
	{
		this.autoSaves = new HashSet<ES3AutoSave>();
		foreach (GameObject gameObject in base.gameObject.scene.GetRootGameObjects())
		{
			this.autoSaves.UnionWith(gameObject.GetComponentsInChildren<ES3AutoSave>(true));
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000025DC File Offset: 0x000007DC
	private static int GetDepth(Transform t)
	{
		int num = 0;
		while (t.parent != null)
		{
			t = t.parent;
			num++;
		}
		return num;
	}

	// Token: 0x0400000B RID: 11
	public static ES3AutoSaveMgr _current = null;

	// Token: 0x0400000C RID: 12
	public static Dictionary<Scene, ES3AutoSaveMgr> managers = new Dictionary<Scene, ES3AutoSaveMgr>();

	// Token: 0x0400000D RID: 13
	public string key = Guid.NewGuid().ToString();

	// Token: 0x0400000E RID: 14
	public ES3AutoSaveMgr.SaveEvent saveEvent = ES3AutoSaveMgr.SaveEvent.OnApplicationQuit;

	// Token: 0x0400000F RID: 15
	public ES3AutoSaveMgr.LoadEvent loadEvent = ES3AutoSaveMgr.LoadEvent.Start;

	// Token: 0x04000010 RID: 16
	public ES3SerializableSettings settings = new ES3SerializableSettings("AutoSave.es3", ES3.Location.Cache);

	// Token: 0x04000011 RID: 17
	public HashSet<ES3AutoSave> autoSaves = new HashSet<ES3AutoSave>();

	// Token: 0x020000EB RID: 235
	public enum LoadEvent
	{
		// Token: 0x0400017B RID: 379
		None,
		// Token: 0x0400017C RID: 380
		Awake,
		// Token: 0x0400017D RID: 381
		Start
	}

	// Token: 0x020000EC RID: 236
	public enum SaveEvent
	{
		// Token: 0x0400017F RID: 383
		None,
		// Token: 0x04000180 RID: 384
		OnApplicationQuit,
		// Token: 0x04000181 RID: 385
		OnApplicationPause
	}
}
