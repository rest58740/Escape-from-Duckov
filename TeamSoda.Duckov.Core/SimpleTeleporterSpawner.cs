using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B3 RID: 179
[RequireComponent(typeof(Points))]
public class SimpleTeleporterSpawner : MonoBehaviour
{
	// Token: 0x060005FE RID: 1534 RVA: 0x0001AEEC File Offset: 0x000190EC
	private void Start()
	{
		if (this.points == null)
		{
			this.points = base.GetComponent<Points>();
			if (this.points == null)
			{
				return;
			}
		}
		this.scene = SceneManager.GetActiveScene().buildIndex;
		if (LevelManager.LevelInited)
		{
			this.StartCreate();
			return;
		}
		LevelManager.OnLevelInitialized += this.StartCreate;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001AF54 File Offset: 0x00019154
	private void OnValidate()
	{
		if (this.points == null)
		{
			this.points = base.GetComponent<Points>();
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001AF70 File Offset: 0x00019170
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.StartCreate;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001AF84 File Offset: 0x00019184
	public void StartCreate()
	{
		this.scene = SceneManager.GetActiveScene().buildIndex;
		int key = this.GetKey();
		object obj;
		if (!MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj))
		{
			MultiSceneCore.Instance.inLevelData.Add(key, true);
			this.Create();
			return;
		}
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001AFDC File Offset: 0x000191DC
	private void Create()
	{
		List<Vector3> randomPoints = this.points.GetRandomPoints(this.pairCount * 2);
		for (int i = 0; i < this.pairCount; i++)
		{
			this.CreateAPair(randomPoints[i * 2], randomPoints[i * 2 + 1]);
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001B028 File Offset: 0x00019228
	private void CreateAPair(Vector3 point1, Vector3 point2)
	{
		SimpleTeleporter simpleTeleporter = this.CreateATeleporter(point1);
		SimpleTeleporter simpleTeleporter2 = this.CreateATeleporter(point2);
		simpleTeleporter.target = simpleTeleporter2.TeleportPoint;
		simpleTeleporter2.target = simpleTeleporter.TeleportPoint;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001B05D File Offset: 0x0001925D
	private SimpleTeleporter CreateATeleporter(Vector3 point)
	{
		SimpleTeleporter simpleTeleporter = Object.Instantiate<SimpleTeleporter>(this.simpleTeleporterPfb);
		MultiSceneCore.MoveToActiveWithScene(simpleTeleporter.gameObject, this.scene);
		simpleTeleporter.transform.position = point;
		return simpleTeleporter;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001B088 File Offset: 0x00019288
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int num = Mathf.RoundToInt(vector.x);
		int num2 = Mathf.RoundToInt(vector.y);
		int num3 = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int;
		vector3Int..ctor(num, num2, num3);
		return string.Format("SimpTeles_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x0400058F RID: 1423
	private int scene = -1;

	// Token: 0x04000590 RID: 1424
	[SerializeField]
	private int pairCount = 3;

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	private SimpleTeleporter simpleTeleporterPfb;

	// Token: 0x04000592 RID: 1426
	[SerializeField]
	private Points points;
}
