using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000008 RID: 8
[ModifierID("Only One Edge")]
public class OnlyOneEdgeModifier : ProceduralImageModifier
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600002A RID: 42 RVA: 0x000026F1 File Offset: 0x000008F1
	// (set) Token: 0x0600002B RID: 43 RVA: 0x000026F9 File Offset: 0x000008F9
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

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600002C RID: 44 RVA: 0x0000270D File Offset: 0x0000090D
	// (set) Token: 0x0600002D RID: 45 RVA: 0x00002715 File Offset: 0x00000915
	public OnlyOneEdgeModifier.ProceduralImageEdge Side
	{
		get
		{
			return this.side;
		}
		set
		{
			this.side = value;
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002720 File Offset: 0x00000920
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		switch (this.side)
		{
		case OnlyOneEdgeModifier.ProceduralImageEdge.Top:
			return new Vector4(this.radius, this.radius, 0f, 0f);
		case OnlyOneEdgeModifier.ProceduralImageEdge.Bottom:
			return new Vector4(0f, 0f, this.radius, this.radius);
		case OnlyOneEdgeModifier.ProceduralImageEdge.Left:
			return new Vector4(this.radius, 0f, 0f, this.radius);
		case OnlyOneEdgeModifier.ProceduralImageEdge.Right:
			return new Vector4(0f, this.radius, this.radius, 0f);
		default:
			return new Vector4(0f, 0f, 0f, 0f);
		}
	}

	// Token: 0x04000010 RID: 16
	[SerializeField]
	private float radius;

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private OnlyOneEdgeModifier.ProceduralImageEdge side;

	// Token: 0x02000018 RID: 24
	public enum ProceduralImageEdge
	{
		// Token: 0x0400002F RID: 47
		Top,
		// Token: 0x04000030 RID: 48
		Bottom,
		// Token: 0x04000031 RID: 49
		Left,
		// Token: 0x04000032 RID: 50
		Right
	}
}
