using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000106 RID: 262
public class ExitCreator : MonoBehaviour
{
	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00027879 File Offset: 0x00025A79
	private int minExitCount
	{
		get
		{
			return LevelConfig.MinExitCount;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00027880 File Offset: 0x00025A80
	private int maxExitCount
	{
		get
		{
			return LevelConfig.MaxExitCount;
		}
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00027888 File Offset: 0x00025A88
	public void Spawn()
	{
		int num = Random.Range(this.minExitCount, this.maxExitCount + 1);
		if (MultiSceneCore.Instance == null)
		{
			return;
		}
		List<ValueTuple<string, SubSceneEntry.Location>> list = new List<ValueTuple<string, SubSceneEntry.Location>>();
		foreach (SubSceneEntry subSceneEntry in MultiSceneCore.Instance.SubScenes)
		{
			foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
			{
				if (this.IsPathCompitable(location))
				{
					list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry.sceneID, location));
				}
			}
		}
		list.Sort(new Comparison<ValueTuple<string, SubSceneEntry.Location>>(this.compareExit));
		if (num > list.Count)
		{
			num = list.Count;
		}
		Vector3 vector;
		MiniMapSettings.TryGetMinimapPosition(LevelManager.Instance.MainCharacter.transform.position, out vector);
		int num2 = Mathf.RoundToInt((float)list.Count * 0.8f);
		if (num > num2)
		{
			num2 = num;
		}
		for (int i = 0; i < num; i++)
		{
			int index = Random.Range(0, num2);
			num2--;
			ValueTuple<string, SubSceneEntry.Location> valueTuple = list[index];
			list.RemoveAt(index);
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(valueTuple.Item1);
			this.CreateExit(valueTuple.Item2.position, sceneInfo.BuildIndex, i);
		}
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00027A10 File Offset: 0x00025C10
	private int compareExit([TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})] ValueTuple<string, SubSceneEntry.Location> a, [TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})] ValueTuple<string, SubSceneEntry.Location> b)
	{
		Vector3 vector;
		if (!MiniMapSettings.TryGetMinimapPosition(LevelManager.Instance.MainCharacter.transform.position, out vector))
		{
			return -1;
		}
		Vector3 vector2;
		if (!MiniMapSettings.TryGetMinimapPosition(a.Item2.position, a.Item1, out vector2))
		{
			return -1;
		}
		Vector3 vector3;
		if (!MiniMapSettings.TryGetMinimapPosition(b.Item2.position, b.Item1, out vector3))
		{
			return -1;
		}
		float num = Vector3.Distance(vector, vector2);
		float num2 = Vector3.Distance(vector, vector3);
		if (num > num2)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00027A8C File Offset: 0x00025C8C
	private bool IsPathCompitable(SubSceneEntry.Location location)
	{
		string path = location.path;
		int num = path.IndexOf('/');
		return num != -1 && path.Substring(0, num) == "Exits";
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00027AC4 File Offset: 0x00025CC4
	private void CreateExit(Vector3 position, int sceneBuildIndex, int debugIndex)
	{
		GameObject go = Object.Instantiate<GameObject>(this.exitPrefab, position, Quaternion.identity);
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.MoveToActiveWithScene(go, sceneBuildIndex);
		}
		this.SpawnMapElement(position, sceneBuildIndex, debugIndex);
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00027B00 File Offset: 0x00025D00
	private void SpawnMapElement(Vector3 position, int sceneBuildIndex, int debugIndex)
	{
		SimplePointOfInterest simplePointOfInterest = new GameObject("MapElement").AddComponent<SimplePointOfInterest>();
		simplePointOfInterest.transform.position = position;
		if (MultiSceneCore.Instance != null)
		{
			simplePointOfInterest.Color = this.iconColor;
			simplePointOfInterest.ShadowColor = this.shadowColor;
			simplePointOfInterest.ShadowDistance = this.shadowDistance;
			simplePointOfInterest.IsArea = false;
			simplePointOfInterest.ScaleFactor = 1f;
			string sceneID = SceneInfoCollection.GetSceneID(sceneBuildIndex);
			simplePointOfInterest.Setup(this.icon, this.exitNameKey, false, sceneID);
			SceneManager.MoveGameObjectToScene(simplePointOfInterest.gameObject, MultiSceneCore.MainScene.Value);
		}
	}

	// Token: 0x0400080A RID: 2058
	public GameObject exitPrefab;

	// Token: 0x0400080B RID: 2059
	[LocalizationKey("Default")]
	public string exitNameKey;

	// Token: 0x0400080C RID: 2060
	[SerializeField]
	private Sprite icon;

	// Token: 0x0400080D RID: 2061
	[SerializeField]
	private Color iconColor = Color.white;

	// Token: 0x0400080E RID: 2062
	[SerializeField]
	private Color shadowColor = Color.white;

	// Token: 0x0400080F RID: 2063
	[SerializeField]
	private float shadowDistance;
}
