using System;
using Shapes;
using UnityEngine;

// Token: 0x020000CD RID: 205
[ExecuteAlways]
public class ShapesSkillLine : MonoBehaviour
{
	// Token: 0x06000682 RID: 1666 RVA: 0x0001D81F File Offset: 0x0001BA1F
	private void Awake()
	{
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0001D824 File Offset: 0x0001BA24
	public void DrawLine()
	{
		if (!this.cam)
		{
			if (LevelManager.Instance)
			{
				this.cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			if (!this.cam)
			{
				return;
			}
		}
		if (this.points.Length == 0)
		{
			return;
		}
		using (Draw.Command(this.cam, 550))
		{
			Draw.LineGeometry = 1;
			Draw.BlendMode = this.blendMode;
			Draw.ThicknessSpace = 0;
			Draw.Thickness = this.lineThickness;
			Draw.ZTest = 8;
			if (!this.worldSpace)
			{
				Draw.Matrix = base.transform.localToWorldMatrix;
			}
			for (int i = 0; i < this.points.Length - 1; i++)
			{
				Draw.Sphere(this.points[i], this.dotRadius, this.colors[i]);
				Draw.Line(this.points[i], this.points[i + 1], this.colors[i]);
			}
			Draw.Sphere(this.points[this.points.Length - 1], this.dotRadius, this.colors[this.colors.Length - 1]);
			if (this.hitObsticle)
			{
				Draw.Sphere(this.hitPoint, this.dotRadius, this.colors[0]);
			}
		}
	}

	// Token: 0x04000651 RID: 1617
	public Vector3[] points;

	// Token: 0x04000652 RID: 1618
	public Color[] colors;

	// Token: 0x04000653 RID: 1619
	public Vector3 hitPoint;

	// Token: 0x04000654 RID: 1620
	public bool hitObsticle;

	// Token: 0x04000655 RID: 1621
	public ShapesBlendMode blendMode;

	// Token: 0x04000656 RID: 1622
	public bool worldSpace;

	// Token: 0x04000657 RID: 1623
	public float dotRadius = 0.02f;

	// Token: 0x04000658 RID: 1624
	public float lineThickness = 0.02f;

	// Token: 0x04000659 RID: 1625
	private Camera cam;
}
