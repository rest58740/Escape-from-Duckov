using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000155 RID: 341
[RequireComponent(typeof(Points))]
[ExecuteInEditMode]
public class PrefabLineGenrator : MonoBehaviour, IOnPointsChanged
{
	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0002FD37 File Offset: 0x0002DF37
	private List<Vector3> originPoints
	{
		get
		{
			return this.points.points;
		}
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0002FD44 File Offset: 0x0002DF44
	public void OnPointsChanged()
	{
	}

	// Token: 0x0400097C RID: 2428
	[SerializeField]
	private float prefabLength = 2f;

	// Token: 0x0400097D RID: 2429
	public PrefabLineGenrator.SapwnInfo spawnPrefab;

	// Token: 0x0400097E RID: 2430
	public PrefabLineGenrator.SapwnInfo startPrefab;

	// Token: 0x0400097F RID: 2431
	public PrefabLineGenrator.SapwnInfo endPrefab;

	// Token: 0x04000980 RID: 2432
	[SerializeField]
	private Points points;

	// Token: 0x04000981 RID: 2433
	[SerializeField]
	[HideInInspector]
	private List<BoxCollider> colliderObjects;

	// Token: 0x04000982 RID: 2434
	[SerializeField]
	private float updateTick = 0.5f;

	// Token: 0x04000983 RID: 2435
	private float lastModifyTime;

	// Token: 0x04000984 RID: 2436
	private bool dirty;

	// Token: 0x04000985 RID: 2437
	public List<Vector3> searchedPointsLocalSpace;

	// Token: 0x020004C6 RID: 1222
	[Serializable]
	public struct SapwnInfo
	{
		// Token: 0x060027ED RID: 10221 RVA: 0x000905F4 File Offset: 0x0008E7F4
		public GameObject GetRandomPrefab()
		{
			if (this.prefabs.Count < 1)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < this.prefabs.Count; i++)
			{
				num += this.prefabs[i].weight;
			}
			float num2 = Random.Range(0f, num);
			for (int j = 0; j < this.prefabs.Count; j++)
			{
				if (num2 <= this.prefabs[j].weight)
				{
					return this.prefabs[j].prefab;
				}
				num2 -= this.prefabs[j].weight;
			}
			return this.prefabs[this.prefabs.Count - 1].prefab;
		}

		// Token: 0x04001D1A RID: 7450
		public List<PrefabLineGenrator.PrefabPair> prefabs;

		// Token: 0x04001D1B RID: 7451
		public float rotateOffset;

		// Token: 0x04001D1C RID: 7452
		[Range(0f, 1f)]
		public float flatten;

		// Token: 0x04001D1D RID: 7453
		public Vector3 posOffset;
	}

	// Token: 0x020004C7 RID: 1223
	[Serializable]
	public struct PrefabPair
	{
		// Token: 0x04001D1E RID: 7454
		public GameObject prefab;

		// Token: 0x04001D1F RID: 7455
		public float weight;
	}
}
