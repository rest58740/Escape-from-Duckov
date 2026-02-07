using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x0200000D RID: 13
	[ExecuteAlways]
	public class ProceduralTree : ImmediateModeShapeDrawer
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003B70 File Offset: 0x00001D70
		public override void DrawShapes(Camera cam)
		{
			using (Draw.Command(cam, RenderPassEvent.BeforeRenderingPostProcessing))
			{
				Draw.ResetAllDrawStates();
				Draw.BlendMode = 2;
				Draw.Thickness = this.lineThickness;
				Draw.LineGeometry = (this.use3D ? 2 : 0);
				Draw.ThicknessSpace = 0;
				Draw.Color = this.lineColor;
				Random.InitState(this.seed);
				this.currentLineCount = 0;
				this.BranchFrom(Draw.Matrix);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003BFC File Offset: 0x00001DFC
		private void BranchFrom(Matrix4x4 mtx)
		{
			int num = this.currentLineCount;
			this.currentLineCount = num + 1;
			if (num >= this.lineCount)
			{
				return;
			}
			Draw.Matrix = mtx;
			float y = Mathf.Lerp(this.branchLengthMin, this.branchLengthMax, Random.value);
			Vector3 vector = new Vector3(0f, y, 0f);
			Draw.Line(Vector3.zero, vector);
			Draw.Translate(vector);
			int num2 = Random.Range(this.branchesMin, this.branchesMax + 1);
			for (int i = 0; i < num2; i++)
			{
				using (Draw.MatrixScope)
				{
					float num3 = Mathf.Lerp(-this.maxAngDeviation, this.maxAngDeviation, ShapesMath.RandomGaussian(0f, 1f));
					if (this.use3D)
					{
						Draw.Rotate(num3, ShapesMath.GetRandomPerpendicularVector(Vector3.up));
					}
					else
					{
						Draw.Rotate(num3);
					}
					this.mtxQueue.Enqueue(Draw.Matrix);
				}
			}
			while (this.mtxQueue.Count > 0)
			{
				this.BranchFrom(this.mtxQueue.Dequeue());
			}
		}

		// Token: 0x0400005D RID: 93
		[Header("Line Style")]
		[Range(0f, 0.1f)]
		public float lineThickness = 0.1f;

		// Token: 0x0400005E RID: 94
		public Color lineColor = Color.white;

		// Token: 0x0400005F RID: 95
		[Header("Tree shape")]
		public int seed;

		// Token: 0x04000060 RID: 96
		[Range(1f, 2000f)]
		public int lineCount = 100;

		// Token: 0x04000061 RID: 97
		[Range(0f, 4f)]
		public int branchesMin = 1;

		// Token: 0x04000062 RID: 98
		[Range(1f, 5f)]
		public int branchesMax = 5;

		// Token: 0x04000063 RID: 99
		[Range(0f, 1f)]
		public float branchLengthMin = 0.25f;

		// Token: 0x04000064 RID: 100
		[Range(0f, 1f)]
		public float branchLengthMax = 1f;

		// Token: 0x04000065 RID: 101
		[Range(0f, 3.1415927f)]
		public float maxAngDeviation = 1.0471976f;

		// Token: 0x04000066 RID: 102
		public bool use3D;

		// Token: 0x04000067 RID: 103
		private int currentLineCount;

		// Token: 0x04000068 RID: 104
		private readonly Queue<Matrix4x4> mtxQueue = new Queue<Matrix4x4>();
	}
}
