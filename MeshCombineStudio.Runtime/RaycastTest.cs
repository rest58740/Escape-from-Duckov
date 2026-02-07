using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[ExecuteInEditMode]
public class RaycastTest : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x00002108 File Offset: 0x00000308
	private void Update()
	{
		if (this.createTriangle)
		{
			this.createTriangle = false;
			this.CreateTriangle();
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002120 File Offset: 0x00000320
	private void CreateTriangle()
	{
		Mesh mesh = new Mesh();
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 1f);
		Vector3 vector3 = new Vector3(0f, 1f, 0f);
		float x = 0.01f;
		Vector3 vector4 = new Vector3(x, 0f, 0f);
		Vector3 vector5 = new Vector3(x, 0f, 1f);
		Vector3 vector6 = new Vector3(x, 1f, 0f);
		Vector3[] vertices = new Vector3[]
		{
			vector,
			vector2,
			vector3,
			vector6,
			vector5,
			vector4
		};
		int[] triangles = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5
		};
		mesh.name = "Triangle";
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		base.GetComponent<MeshFilter>().sharedMesh = mesh;
		base.GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002238 File Offset: 0x00000438
	private void Swap<T>(ref T v1, ref T v2)
	{
		T t = v1;
		v1 = v2;
		v2 = t;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002260 File Offset: 0x00000460
	private void OnDrawGizmos()
	{
		if (!this.mr)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 min = this.mr.bounds.min;
		Vector3 left = Vector3.left;
		Physics.queriesHitBackfaces = true;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (Physics.Raycast(new Ray
		{
			origin = position,
			direction = left
		}, out this.hitInfo, 10000f))
		{
			if (Vector3.Dot(left, this.hitInfo.normal) >= 0f)
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawLine(this.hitInfo.point, this.hitInfo.point + this.hitInfo.normal);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(position, this.hitInfo.point);
		}
		else
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(position, position + left.normalized * 1000f);
		}
		Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
		Mesh sharedMesh2 = this.mr.GetComponent<MeshFilter>().sharedMesh;
		Vector3[] vertices = sharedMesh2.vertices;
		int[] triangles = sharedMesh2.triangles;
		for (int i = 0; i < triangles.Length; i += 3)
		{
			Vector3 a = this.mr.transform.TransformPoint(vertices[triangles[i + 2]]);
			Vector3 b = this.mr.transform.TransformPoint(vertices[triangles[i]]);
			Vector3 c = this.mr.transform.TransformPoint(vertices[triangles[i + 1]]);
			TriangleTest triangleTest = default(TriangleTest);
			triangleTest.a = a;
			triangleTest.b = b;
			triangleTest.c = c;
			triangleTest.Calc();
			if (Physics.CheckBox(triangleTest.a + triangleTest.dirAb / 2f + (triangleTest.c - triangleTest.h1) / 2f, new Vector3(0.05f, triangleTest.h, triangleTest.ab) / 2f, Quaternion.LookRotation(triangleTest.dirAb, triangleTest.dirAc)))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(triangleTest.a, triangleTest.b);
				Gizmos.DrawLine(triangleTest.b, triangleTest.c);
				Gizmos.DrawLine(triangleTest.c, triangleTest.a);
				Gizmos.DrawLine(triangleTest.c, triangleTest.h1);
			}
			else
			{
				Gizmos.color = Color.green;
			}
		}
		Physics.queriesHitBackfaces = false;
	}

	// Token: 0x04000003 RID: 3
	public MeshRenderer mr;

	// Token: 0x04000004 RID: 4
	public Collider collider;

	// Token: 0x04000005 RID: 5
	public LayerMask layerMask;

	// Token: 0x04000006 RID: 6
	public bool createTriangle;

	// Token: 0x04000007 RID: 7
	public int triangleIndex;

	// Token: 0x04000008 RID: 8
	private RaycastHit hitInfo;

	// Token: 0x04000009 RID: 9
	public bool step2;

	// Token: 0x0400000A RID: 10
	public bool drawTriangle;
}
