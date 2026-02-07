using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace UI_Spline_Renderer
{
	// Token: 0x0200000B RID: 11
	[BurstCompile]
	internal struct SplineExtrudeJob : IJob
	{
		// Token: 0x06000035 RID: 53 RVA: 0x0000300E File Offset: 0x0000120E
		public void Execute()
		{
			this.Evaluate();
			this.ExtrudeSpline();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000301C File Offset: 0x0000121C
		private void Evaluate()
		{
			this.length = this.spline.GetLength();
			for (int i = 0; i < this.edgeCount; i++)
			{
				float num = (float)i / (float)(this.edgeCount - 1);
				num = num.Remap(0f, 1f, this.clipRange.x, this.clipRange.y);
				float3 @float;
				float3 float2;
				float3 float3;
				SplineUtility.Evaluate<NativeSpline>(this.spline, num, ref @float, ref float2, ref float3);
				this.evaluatedPos[i] = @float;
				this.evaluatedTan[i] = float2;
				this.evaluatedNor[i] = float3;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000030BC File Offset: 0x000012BC
		private void ExtrudeSpline()
		{
			if (this.spline.Count < 2)
			{
				return;
			}
			float3 @float = float3.zero;
			for (int i = 0; i < this.edgeCount; i++)
			{
				float num = (float)i / (float)(this.edgeCount - 1);
				num = num.Remap(0f, 1f, this.clipRange.x, this.clipRange.y);
				float3 float2 = this.evaluatedPos[i];
				float3 float3 = this.evaluatedTan[i];
				float3 float4 = this.evaluatedNor[i];
				if (float3.x == 0f && float3.y == 0f && float3.z == 0f)
				{
					float3 float5 = (i == 0) ? float2 : @float;
					float3 = ((i == this.edgeCount - 1) ? float2 : this.evaluatedPos[i + 1]) - float5;
				}
				float widthAt = this.GetWidthAt(num);
				float vat = this.GetVAt(num, i);
				Color colorAt = this.GetColorAt(num);
				UIVertex uivertex;
				UIVertex uivertex2;
				InternalUtility.ExtrudeEdge(widthAt, vat, colorAt, ref float2, float3, float4, this.keepBillboard, this.keepZeroZ, this.uvMultiplier, this.uvOffset, out uivertex, out uivertex2);
				this.vertices.Add(ref uivertex);
				this.vertices.Add(ref uivertex2);
				@float = float2;
				if (i > 0)
				{
					int3 @int = new int3(2 * i + this.startIdx, 2 * i - 1 + this.startIdx, 2 * i - 2 + this.startIdx);
					this.triangles.Add(ref @int);
					@int = new int3(2 * i + 1 + this.startIdx, 2 * i - 1 + this.startIdx, 2 * i + this.startIdx);
					this.triangles.Add(ref @int);
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003277 File Offset: 0x00001477
		private float GetWidthAt(float t)
		{
			return this.width * this.widthCurve.Evaluate(t);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000328C File Offset: 0x0000148C
		private Color GetColorAt(float t)
		{
			return this.color * this.colorGradient.Evaluate(t);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000032A8 File Offset: 0x000014A8
		private float GetVAt(float t, int i)
		{
			switch (this.uvMode)
			{
			case UVMode.Tile:
				return this.length / this.width * t;
			case UVMode.RepeatPerSegment:
				return (float)i;
			case UVMode.Stretch:
				return t;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0400000F RID: 15
		[ReadOnly]
		public NativeSpline spline;

		// Token: 0x04000010 RID: 16
		[ReadOnly]
		public NativeCurve widthCurve;

		// Token: 0x04000011 RID: 17
		[ReadOnly]
		public float width;

		// Token: 0x04000012 RID: 18
		[ReadOnly]
		public bool keepZeroZ;

		// Token: 0x04000013 RID: 19
		[ReadOnly]
		public bool keepBillboard;

		// Token: 0x04000014 RID: 20
		[ReadOnly]
		public int startIdx;

		// Token: 0x04000015 RID: 21
		[ReadOnly]
		public float2 clipRange;

		// Token: 0x04000016 RID: 22
		[ReadOnly]
		public float2 uvMultiplier;

		// Token: 0x04000017 RID: 23
		[ReadOnly]
		public float2 uvOffset;

		// Token: 0x04000018 RID: 24
		[ReadOnly]
		public UVMode uvMode;

		// Token: 0x04000019 RID: 25
		[ReadOnly]
		public Color color;

		// Token: 0x0400001A RID: 26
		[ReadOnly]
		internal NativeColorGradient colorGradient;

		// Token: 0x0400001B RID: 27
		[ReadOnly]
		public int edgeCount;

		// Token: 0x0400001C RID: 28
		public NativeArray<float3> evaluatedPos;

		// Token: 0x0400001D RID: 29
		public NativeArray<float3> evaluatedTan;

		// Token: 0x0400001E RID: 30
		public NativeArray<float3> evaluatedNor;

		// Token: 0x0400001F RID: 31
		[WriteOnly]
		public NativeList<UIVertex> vertices;

		// Token: 0x04000020 RID: 32
		[WriteOnly]
		public NativeList<int3> triangles;

		// Token: 0x04000021 RID: 33
		private float v;

		// Token: 0x04000022 RID: 34
		private float length;
	}
}
