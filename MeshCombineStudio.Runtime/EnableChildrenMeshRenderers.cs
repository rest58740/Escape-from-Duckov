using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
[ExecuteInEditMode]
public class EnableChildrenMeshRenderers : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x00003033 File Offset: 0x00001233
	private void Update()
	{
		if (this.execute)
		{
			this.execute = false;
			this.Execute();
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000304C File Offset: 0x0000124C
	private void Execute()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
	}

	// Token: 0x04000024 RID: 36
	public bool execute;
}
