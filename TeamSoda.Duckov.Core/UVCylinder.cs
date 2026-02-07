using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class UVCylinder : MonoBehaviour
{
	// Token: 0x06000ABD RID: 2749 RVA: 0x0002FB40 File Offset: 0x0002DD40
	private void Generate()
	{
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
		}
		this.mesh.Clear();
		new List<Vector3>();
		new List<Vector2>();
		new List<Vector3>();
		new List<int>();
		for (int i = 0; i < this.subdivision; i++)
		{
		}
	}

	// Token: 0x04000974 RID: 2420
	public float radius = 1f;

	// Token: 0x04000975 RID: 2421
	public float height = 2f;

	// Token: 0x04000976 RID: 2422
	public int subdivision = 16;

	// Token: 0x04000977 RID: 2423
	private Mesh mesh;
}
