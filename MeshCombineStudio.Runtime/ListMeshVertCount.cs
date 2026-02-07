using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
[ExecuteInEditMode]
public class ListMeshVertCount : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Update()
	{
		if (this.listVertCount)
		{
			this.listVertCount = false;
			this.ListVertCount();
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
	private void ListVertCount()
	{
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>(this.includeInActive);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Mesh sharedMesh = componentsInChildren[i].sharedMesh;
			if (!(sharedMesh == null))
			{
				num += sharedMesh.vertexCount;
				num2 += sharedMesh.triangles.Length;
			}
		}
		Debug.Log(string.Concat(new string[]
		{
			base.gameObject.name,
			" Vertices ",
			num.ToString(),
			"  Triangles ",
			num2.ToString()
		}));
	}

	// Token: 0x04000001 RID: 1
	public bool includeInActive;

	// Token: 0x04000002 RID: 2
	public bool listVertCount;
}
