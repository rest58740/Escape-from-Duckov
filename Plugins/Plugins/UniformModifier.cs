using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x0200000A RID: 10
[ModifierID("Uniform")]
public class UniformModifier : ProceduralImageModifier
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000032 RID: 50 RVA: 0x00002808 File Offset: 0x00000A08
	// (set) Token: 0x06000033 RID: 51 RVA: 0x00002810 File Offset: 0x00000A10
	public float Radius
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

	// Token: 0x06000034 RID: 52 RVA: 0x00002824 File Offset: 0x00000A24
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		float num = this.radius;
		return new Vector4(num, num, num, num);
	}

	// Token: 0x04000012 RID: 18
	[SerializeField]
	private float radius;
}
