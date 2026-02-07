using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000006 RID: 6
[ExecuteInEditMode]
public class Voxelize : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x000028BB File Offset: 0x00000ABB
	public void Update()
	{
		if (this.voxelize)
		{
			this.voxelize = false;
			this.VoxelizeMesh(base.transform, this.voxelResolution, this.voxelizeLayer);
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000028E8 File Offset: 0x00000AE8
	private Voxelize.VoxelizedMesh VoxelizeMesh(Transform t, float voxelResolution, int voxelizeLayer)
	{
		Physics.queriesHitBackfaces = false;
		MeshRenderer component = t.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return null;
		}
		MeshFilter component2 = t.GetComponent<MeshFilter>();
		if (component2 == null)
		{
			return null;
		}
		Mesh sharedMesh = component2.sharedMesh;
		if (sharedMesh == null)
		{
			return null;
		}
		Voxelize.VoxelizedMesh voxelizedMesh = new Voxelize.VoxelizedMesh();
		Voxelize.voxelizedLookup[sharedMesh] = voxelizedMesh;
		Transform parent = t.parent;
		Vector3 position = t.position;
		Quaternion rotation = t.rotation;
		Vector3 localScale = t.localScale;
		t.parent = null;
		t.position = Vector3.zero;
		t.rotation = Quaternion.identity;
		t.localScale = Vector3.one;
		int layer = t.gameObject.layer;
		t.gameObject.layer = voxelizeLayer;
		LayerMask voxelizeLayerMask = 1 << voxelizeLayer;
		Bounds bounds = component.bounds;
		Vector3 size = bounds.size;
		Voxelize.Int3 @int = new Voxelize.Int3(Mathf.CeilToInt(size.x / voxelResolution), Mathf.CeilToInt(size.y / voxelResolution), Mathf.CeilToInt(size.z / voxelResolution));
		@int += new Voxelize.Int3(2, 2, 2);
		int num = Mathf.CeilToInt((float)@int.x / 8f);
		voxelizedMesh.voxels = @int;
		size = new Vector3((float)@int.x * voxelResolution, (float)@int.y * voxelResolution, (float)@int.z * voxelResolution);
		bounds.size = size;
		voxelizedMesh.bounds = bounds;
		byte[,,] array = new byte[num, @int.y, @int.z];
		Ray ray = default(Ray);
		Ray ray2 = default(Ray);
		ray.direction = Vector3.forward;
		ray2.direction = Vector3.back;
		Vector3 min = bounds.min;
		Vector3 vector = min;
		vector.z = bounds.max.z;
		Debug.Log(string.Concat(new string[]
		{
			Voxelize.PrintVector3(component.bounds.size),
			" new size ",
			Voxelize.PrintVector3(size),
			" voxels ",
			@int.ToString()
		}));
		int num2 = 0;
		Vector3 b = Vector3.one * voxelResolution * 0.5f;
		Vector3 b2 = min + b;
		try
		{
			for (int i = 0; i < @int.x; i++)
			{
				int num3 = i / 8;
				byte b3 = Voxelize.bits[i - num3 * 8];
				for (int j = 0; j < @int.y; j++)
				{
					Vector3 origin = min + new Vector3(((float)i + 0.5f) * voxelResolution, ((float)j + 0.5f) * voxelResolution, 0f);
					ray.origin = origin;
					origin.z = vector.z;
					ray2.origin = origin;
					Voxelize.intersectList.Clear();
					Voxelize.MultiCast(ray, Voxelize.intersectList, 0.001f, size.z, voxelizeLayerMask);
					Voxelize.MultiCast(ray2, Voxelize.intersectList, -0.001f, size.z, voxelizeLayerMask);
					Voxelize.intersectList.Sort();
					float num4 = (float)Voxelize.intersectList.Count / 2f;
					if (num4 == (float)((int)num4))
					{
						for (int k = 0; k < Voxelize.intersectList.Count; k += 2)
						{
							int num5 = Mathf.RoundToInt((Voxelize.intersectList[k] - min.z) / voxelResolution);
							int num6 = Mathf.RoundToInt((Voxelize.intersectList[k + 1] - min.z) / voxelResolution);
							for (int l = num5; l < num6; l++)
							{
								Vector3 position2 = new Vector3((float)i * voxelResolution, (float)j * voxelResolution, (float)l * voxelResolution) + b2;
								position2 = t.TransformPoint(position2);
								ref byte ptr = ref array[num3, j, l];
								ptr |= b3;
								num2++;
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
		Debug.Log(t.name + " voxels " + num2.ToString());
		voxelizedMesh.volume = array;
		t.gameObject.layer = layer;
		t.parent = parent;
		t.position = position;
		t.rotation = rotation;
		t.localScale = localScale;
		return voxelizedMesh;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002D48 File Offset: 0x00000F48
	private static string PrintVector3(Vector3 v)
	{
		return string.Concat(new string[]
		{
			"(",
			v.x.ToString(),
			", ",
			v.y.ToString(),
			", ",
			v.z.ToString(),
			")"
		});
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002DB0 File Offset: 0x00000FB0
	private static void MultiCast(Ray ray, List<float> points, float hitOffset, float maxDistance, LayerMask voxelizeLayerMask)
	{
		RaycastHit raycastHit;
		while (Physics.Raycast(ray, out raycastHit, maxDistance, voxelizeLayerMask))
		{
			points.Add(raycastHit.point.z);
			Vector3 origin = ray.origin;
			ray.origin = new Vector3(origin.x, origin.y, raycastHit.point.z + hitOffset);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002E14 File Offset: 0x00001014
	private static void Report(Voxelize.VoxelizedMesh vm, float voxelResolution)
	{
		int num = (int)voxelResolution / 8;
		for (int i = 0; i < num; i++)
		{
			int num2 = 0;
			while ((float)num2 < voxelResolution)
			{
				int num3 = 0;
				while ((float)num3 < voxelResolution)
				{
					Debug.Log(vm.volume[i, num2, num3]);
					num3++;
				}
				num2++;
			}
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002E64 File Offset: 0x00001064
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		this.DrawVolume(base.transform, this.voxelResolution);
		Gizmos.color = Color.white;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002E8C File Offset: 0x0000108C
	public void DrawVolume(Transform t, float voxelResolution)
	{
		MeshRenderer component = t.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		MeshFilter component2 = t.GetComponent<MeshFilter>();
		if (component2 == null)
		{
			return;
		}
		Mesh sharedMesh = component2.sharedMesh;
		if (sharedMesh == null)
		{
			return;
		}
		Voxelize.VoxelizedMesh voxelizedMesh;
		Voxelize.voxelizedLookup.TryGetValue(sharedMesh, out voxelizedMesh);
		if (voxelizedMesh == null)
		{
			return;
		}
		byte[,,] volume = voxelizedMesh.volume;
		if (volume == null)
		{
			return;
		}
		Vector3 min = voxelizedMesh.bounds.min;
		Vector3 size = t.lossyScale * voxelResolution;
		Vector3 b = Vector3.one * voxelResolution * 0.5f;
		Voxelize.Int3 voxels = voxelizedMesh.voxels;
		Gizmos.DrawWireCube(component.bounds.center, component.bounds.size);
		for (int i = 0; i < voxels.x; i++)
		{
			int num = i / 8;
			int num2 = i - num * 8;
			for (int j = 0; j < voxels.y; j++)
			{
				for (int k = 0; k < voxels.z; k++)
				{
					if ((volume[num, j, k] & Voxelize.bits[num2]) > 0)
					{
						Vector3 position = new Vector3(min.x + (float)i * voxelResolution, min.y + (float)j * voxelResolution, min.z + (float)k * voxelResolution) + b;
						Gizmos.DrawWireCube(t.TransformPoint(position), size);
					}
				}
			}
		}
	}

	// Token: 0x0400001C RID: 28
	private static readonly byte[] bits = new byte[]
	{
		1,
		2,
		4,
		8,
		16,
		32,
		64,
		128
	};

	// Token: 0x0400001D RID: 29
	private static Dictionary<Mesh, Voxelize.VoxelizedMesh> voxelizedLookup = new Dictionary<Mesh, Voxelize.VoxelizedMesh>();

	// Token: 0x0400001E RID: 30
	private static List<float> intersectList = new List<float>();

	// Token: 0x0400001F RID: 31
	private const byte insideVoxel = 1;

	// Token: 0x04000020 RID: 32
	private const byte outsideVoxel = 2;

	// Token: 0x04000021 RID: 33
	public int voxelizeLayer;

	// Token: 0x04000022 RID: 34
	public float voxelResolution;

	// Token: 0x04000023 RID: 35
	public bool voxelize;

	// Token: 0x02000054 RID: 84
	public class VoxelizedMesh
	{
		// Token: 0x04000222 RID: 546
		public byte[,,] volume;

		// Token: 0x04000223 RID: 547
		public Bounds bounds;

		// Token: 0x04000224 RID: 548
		public Voxelize.Int3 voxels;
	}

	// Token: 0x02000055 RID: 85
	public struct Int3
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x0000F057 File Offset: 0x0000D257
		public Int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000F070 File Offset: 0x0000D270
		public static Voxelize.Int3 operator +(Voxelize.Int3 a, Voxelize.Int3 b)
		{
			Voxelize.Int3 result;
			result.x = a.x + b.x;
			result.y = a.y + b.y;
			result.z = a.z + b.z;
			return result;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x04000225 RID: 549
		public int x;

		// Token: 0x04000226 RID: 550
		public int y;

		// Token: 0x04000227 RID: 551
		public int z;
	}
}
