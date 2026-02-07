using System;
using System.Collections.Generic;
using Duckov.Utilities;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x0200012E RID: 302
[CreateAssetMenu]
public class SceneInfoCollection : ScriptableObject
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0002BB4C File Offset: 0x00029D4C
	internal static SceneInfoCollection Instance
	{
		get
		{
			GameplayDataSettings.SceneManagementData sceneManagement = GameplayDataSettings.SceneManagement;
			if (sceneManagement == null)
			{
				return null;
			}
			return sceneManagement.SceneInfoCollection;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0002BB5E File Offset: 0x00029D5E
	public static List<SceneInfoEntry> Entries
	{
		get
		{
			if (SceneInfoCollection.Instance == null)
			{
				return null;
			}
			return SceneInfoCollection.Instance.entries;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0002BB7C File Offset: 0x00029D7C
	public SceneInfoEntry InstanceGetSceneInfo(string id)
	{
		return this.entries.Find((SceneInfoEntry e) => e.ID == id);
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0002BBB0 File Offset: 0x00029DB0
	public string InstanceGetSceneID(int buildIndex)
	{
		SceneInfoEntry sceneInfoEntry = this.entries.Find((SceneInfoEntry e) => e != null && e.SceneReference.UnsafeReason == null && e.SceneReference.BuildIndex == buildIndex);
		if (sceneInfoEntry == null)
		{
			return null;
		}
		return sceneInfoEntry.ID;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0002BBED File Offset: 0x00029DED
	internal string InstanceGetSceneID(SceneReference sceneRef)
	{
		if (sceneRef.UnsafeReason != null)
		{
			return null;
		}
		return this.InstanceGetSceneID(sceneRef.BuildIndex);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0002BC08 File Offset: 0x00029E08
	internal SceneReference InstanceGetSceneReferencce(string requireSceneID)
	{
		SceneInfoEntry sceneInfoEntry = this.InstanceGetSceneInfo(requireSceneID);
		if (sceneInfoEntry == null)
		{
			return null;
		}
		return sceneInfoEntry.SceneReference;
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0002BC28 File Offset: 0x00029E28
	public static SceneInfoEntry GetSceneInfo(string sceneID)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneInfo(sceneID);
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0002BC44 File Offset: 0x00029E44
	public static string GetSceneID(SceneReference sceneRef)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneID(sceneRef);
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0002BC60 File Offset: 0x00029E60
	public static string GetSceneID(int buildIndex)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneID(buildIndex);
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0002BC7C File Offset: 0x00029E7C
	internal static int GetBuildIndex(string overrideSceneID)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return -1;
		}
		SceneInfoEntry sceneInfoEntry = SceneInfoCollection.Instance.InstanceGetSceneInfo(overrideSceneID);
		if (sceneInfoEntry == null)
		{
			return -1;
		}
		return sceneInfoEntry.BuildIndex;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0002BCB0 File Offset: 0x00029EB0
	internal static SceneInfoEntry GetSceneInfo(int sceneBuildIndex)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.entries.Find((SceneInfoEntry e) => e.BuildIndex == sceneBuildIndex);
	}

	// Token: 0x040008D9 RID: 2265
	public const string BaseSceneID = "Base";

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private List<SceneInfoEntry> entries;
}
