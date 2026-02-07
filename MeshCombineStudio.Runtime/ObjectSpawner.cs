using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200001D RID: 29
	[ExecuteInEditMode]
	public class ObjectSpawner : MonoBehaviour
	{
		// Token: 0x06000095 RID: 149 RVA: 0x0000723C File Offset: 0x0000543C
		private void Awake()
		{
			this.t = base.transform;
			if (this.spawnInRuntime && Application.isPlaying)
			{
				this.Spawn();
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000725F File Offset: 0x0000545F
		private void Update()
		{
			if (this.spawn)
			{
				this.spawn = false;
				this.Spawn();
			}
			if (this.deleteChildren)
			{
				this.deleteChildren = false;
				this.DeleteChildren();
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000728C File Offset: 0x0000548C
		public void DeleteChildren()
		{
			Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (this.t != componentsInChildren[i] && componentsInChildren[i] != null)
				{
					UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000072D8 File Offset: 0x000054D8
		private void SetObjectsActive(bool active)
		{
			for (int i = 0; i < this.objects.Length; i++)
			{
				this.objects[i].SetActive(active);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00007308 File Offset: 0x00005508
		public void Spawn()
		{
			this.SetObjectsActive(true);
			Bounds bounds = default(Bounds);
			bounds.center = base.transform.position;
			bounds.size = this.spawnArea;
			float x = bounds.min.x;
			float x2 = bounds.max.x;
			float y = bounds.min.y;
			float y2 = bounds.max.y;
			float z = bounds.min.z;
			float z2 = bounds.max.z;
			int maxExclusive = this.objects.Length;
			float num = this.metersBetweenSpawning * 0.5f;
			float num2 = base.transform.lossyScale.y * 0.5f;
			int num3 = 0;
			for (float num4 = z; num4 < z2; num4 += this.metersBetweenSpawning)
			{
				for (float num5 = x; num5 < x2; num5 += this.metersBetweenSpawning)
				{
					for (float num6 = y; num6 < y2; num6 += this.metersBetweenSpawning)
					{
						int num7 = UnityEngine.Random.Range(0, maxExclusive);
						if (UnityEngine.Random.value < this.density)
						{
							Vector3 vector = new Vector3(num5 + UnityEngine.Random.Range(-num, num), y + UnityEngine.Random.Range(0f, bounds.size.y) * UnityEngine.Random.Range(this.heightRange.x, this.heightRange.y), num4 + UnityEngine.Random.Range(-num, num));
							if (vector.x >= x && vector.x <= x2 && vector.y >= y && vector.y <= y2 && vector.z >= z && vector.z <= z2)
							{
								vector.y += num2;
								Vector3 euler = new Vector3(UnityEngine.Random.Range(0f, this.rotationRange.x), UnityEngine.Random.Range(0f, this.rotationRange.y), UnityEngine.Random.Range(0f, this.rotationRange.z));
								GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objects[num7], vector, Quaternion.Euler(euler));
								float num8 = UnityEngine.Random.Range(this.scaleRange.x, this.scaleRange.y) * this.scaleMulti;
								gameObject.transform.localScale = new Vector3(num8, num8, num8);
								gameObject.transform.parent = this.t;
								num3++;
							}
						}
					}
				}
			}
			this.SetObjectsActive(false);
			Debug.Log("Spawned " + num3.ToString());
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000075B2 File Offset: 0x000057B2
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireCube(base.transform.position + new Vector3(0f, 0f, 0f), this.spawnArea);
		}

		// Token: 0x040000EB RID: 235
		public GameObject[] objects;

		// Token: 0x040000EC RID: 236
		public Vector3 spawnArea = new Vector3(512f, 512f, 512f);

		// Token: 0x040000ED RID: 237
		public float density = 0.5f;

		// Token: 0x040000EE RID: 238
		public Vector2 scaleRange = new Vector2(0.5f, 2f);

		// Token: 0x040000EF RID: 239
		public Vector3 rotationRange = new Vector3(5f, 360f, 5f);

		// Token: 0x040000F0 RID: 240
		public Vector2 heightRange = new Vector2(0f, 1f);

		// Token: 0x040000F1 RID: 241
		public float scaleMulti = 1f;

		// Token: 0x040000F2 RID: 242
		public float metersBetweenSpawning = 2f;

		// Token: 0x040000F3 RID: 243
		public bool spawnInRuntime;

		// Token: 0x040000F4 RID: 244
		public bool spawn;

		// Token: 0x040000F5 RID: 245
		public bool deleteChildren;

		// Token: 0x040000F6 RID: 246
		private Transform t;
	}
}
