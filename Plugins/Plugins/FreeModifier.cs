using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000007 RID: 7
[ModifierID("Free")]
public class FreeModifier : ProceduralImageModifier
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000025 RID: 37 RVA: 0x00002638 File Offset: 0x00000838
	// (set) Token: 0x06000026 RID: 38 RVA: 0x00002640 File Offset: 0x00000840
	public Vector4 Radius
	{
		get
		{
			return this.radius;
		}
		set
		{
			this.radius = value;
			base._Graphic.SetVerticesDirty();
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002654 File Offset: 0x00000854
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		return this.radius;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000265C File Offset: 0x0000085C
	protected void OnValidate()
	{
		this.radius.x = Mathf.Max(0f, this.radius.x);
		this.radius.y = Mathf.Max(0f, this.radius.y);
		this.radius.z = Mathf.Max(0f, this.radius.z);
		this.radius.w = Mathf.Max(0f, this.radius.w);
	}

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private Vector4 radius;
}
