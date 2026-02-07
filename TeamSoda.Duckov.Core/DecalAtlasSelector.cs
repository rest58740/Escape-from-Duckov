using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x02000144 RID: 324
[DisallowMultipleComponent]
public class DecalAtlasSelector : MonoBehaviour
{
	// Token: 0x06000A89 RID: 2697 RVA: 0x0002D928 File Offset: 0x0002BB28
	private void OnValidate()
	{
		if (this.projector == null)
		{
			this.projector = base.GetComponent<DecalProjector>();
		}
		if (this.projector == null || this.rows <= 0 || this.columns <= 0)
		{
			return;
		}
		int num = this.rows * this.columns;
		int num2 = Mathf.Clamp(this.index, 0, num - 1);
		Vector2 vector;
		vector..ctor(1f / (float)this.columns, 1f / (float)this.rows);
		Vector2 uvBias;
		uvBias..ctor((float)(num2 % this.columns) * vector.x, 1f - vector.y - (float)(num2 / this.columns) * vector.y);
		this.projector.uvScale = vector;
		this.projector.uvBias = uvBias;
	}

	// Token: 0x04000946 RID: 2374
	[Header("Atlas 设置")]
	[Min(1f)]
	public int rows = 1;

	// Token: 0x04000947 RID: 2375
	[Min(1f)]
	public int columns = 1;

	// Token: 0x04000948 RID: 2376
	[Min(0f)]
	public int index;

	// Token: 0x04000949 RID: 2377
	private DecalProjector projector;
}
