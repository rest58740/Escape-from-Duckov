using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BakeryLightMesh : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x000022E8 File Offset: 0x000004E8
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		if (component != null)
		{
			Gizmos.DrawWireSphere(component.bounds.center, this.cutoff);
		}
	}

	// Token: 0x0400004E RID: 78
	public int UID;

	// Token: 0x0400004F RID: 79
	public Color color = Color.white;

	// Token: 0x04000050 RID: 80
	public float intensity = 1f;

	// Token: 0x04000051 RID: 81
	public Texture2D texture;

	// Token: 0x04000052 RID: 82
	public float cutoff = 100f;

	// Token: 0x04000053 RID: 83
	public int samples = 256;

	// Token: 0x04000054 RID: 84
	public int samples2 = 16;

	// Token: 0x04000055 RID: 85
	public int samples2_previous = 16;

	// Token: 0x04000056 RID: 86
	public int bitmask = 1;

	// Token: 0x04000057 RID: 87
	public bool selfShadow = true;

	// Token: 0x04000058 RID: 88
	public bool bakeToIndirect = true;

	// Token: 0x04000059 RID: 89
	public bool shadowmask;

	// Token: 0x0400005A RID: 90
	public float indirectIntensity = 1f;

	// Token: 0x0400005B RID: 91
	public bool shadowmaskFalloff;

	// Token: 0x0400005C RID: 92
	public int maskChannel;

	// Token: 0x0400005D RID: 93
	public int lmid = -2;

	// Token: 0x0400005E RID: 94
	public static int lightsChanged;

	// Token: 0x0400005F RID: 95
	private static GameObject objShownError;
}
