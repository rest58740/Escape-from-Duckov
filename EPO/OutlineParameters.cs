using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000014 RID: 20
	public class OutlineParameters : IDisposable
	{
		// Token: 0x0600007A RID: 122 RVA: 0x000050C0 File Offset: 0x000032C0
		public OutlineParameters(CommandBufferWrapper wrapper)
		{
			this.Buffer = wrapper;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00005170 File Offset: 0x00003370
		[TupleElementNames(new string[]
		{
			"ScaledWidth",
			"ScaledHeight"
		})]
		public ValueTuple<int, int> ScaledSize
		{
			[return: TupleElementNames(new string[]
			{
				"ScaledWidth",
				"ScaledHeight"
			})]
			get
			{
				int num = this.TargetWidth;
				int num2 = this.TargetHeight;
				switch (this.PrimaryBufferSizeMode)
				{
				case BufferSizeMode.WidthControlsHeight:
					num = this.PrimaryBufferSizeReference;
					num2 = (int)((float)this.PrimaryBufferSizeReference / ((float)this.TargetWidth / (float)this.TargetHeight));
					break;
				case BufferSizeMode.HeightControlsWidth:
					num = (int)((float)this.PrimaryBufferSizeReference / ((float)this.TargetHeight / (float)this.TargetWidth));
					num2 = this.PrimaryBufferSizeReference;
					break;
				case BufferSizeMode.Scaled:
					num = (int)((float)this.TargetWidth * this.PrimaryBufferScale);
					num2 = (int)((float)this.TargetHeight * this.PrimaryBufferScale);
					break;
				}
				if (this.EyeMask == StereoTargetEyeMask.None)
				{
					return new ValueTuple<int, int>(num, num2);
				}
				if (num % 2 != 0)
				{
					num++;
				}
				if (num2 % 2 != 0)
				{
					num2++;
				}
				return new ValueTuple<int, int>(num, num2);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005234 File Offset: 0x00003434
		public void Prepare()
		{
			if (this.OutlinablesToRender.Count == 0)
			{
				return;
			}
			this.UseInfoBuffer = (this.OutlinablesToRender.Find((Outlinable x) => x != null && ((x.DrawingMode & (OutlinableDrawingMode.Obstacle | OutlinableDrawingMode.Mask)) != (OutlinableDrawingMode)0 || x.ComplexMaskingMode > ComplexMaskingMode.None)) != null);
			if (this.UseInfoBuffer)
			{
				return;
			}
			foreach (Outlinable outlinable in this.OutlinablesToRender)
			{
				if ((outlinable.DrawingMode & OutlinableDrawingMode.Normal) != (OutlinableDrawingMode)0 && OutlineParameters.CheckDiffers(outlinable))
				{
					this.UseInfoBuffer = true;
					break;
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000052E8 File Offset: 0x000034E8
		private static bool CheckDiffers(Outlinable outlinable)
		{
			if (outlinable.RenderStyle == RenderStyle.Single)
			{
				return OutlineParameters.CheckIfNotUnit(outlinable.OutlineParameters);
			}
			return OutlineParameters.CheckIfNotUnit(outlinable.FrontParameters) || OutlineParameters.CheckIfNotUnit(outlinable.BackParameters);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005319 File Offset: 0x00003519
		private static bool CheckIfNotUnit(Outlinable.OutlineProperties parameters)
		{
			return !Mathf.Approximately(parameters.BlurShift, 1f) || !Mathf.Approximately(parameters.DilateShift, 1f);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005344 File Offset: 0x00003544
		public void Dispose()
		{
			IDisposable disposable = this.Buffer as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			UnityEngine.Object.DestroyImmediate(this.BlitMesh);
			MeshPool meshPool = this.MeshPool;
			if (meshPool != null)
			{
				meshPool.Dispose();
			}
			RTHandlePool rthandlePool = this.RTHandlePool;
			if (rthandlePool == null)
			{
				return;
			}
			rthandlePool.Dispose();
		}

		// Token: 0x0400006D RID: 109
		public readonly RTHandlePool RTHandlePool = new RTHandlePool();

		// Token: 0x0400006E RID: 110
		public readonly MeshPool MeshPool = new MeshPool();

		// Token: 0x0400006F RID: 111
		public readonly Handles Handles = new Handles();

		// Token: 0x04000070 RID: 112
		public Camera Camera;

		// Token: 0x04000071 RID: 113
		public RTHandle Target;

		// Token: 0x04000072 RID: 114
		public RTHandle DepthTarget;

		// Token: 0x04000073 RID: 115
		public CommandBufferWrapper Buffer;

		// Token: 0x04000074 RID: 116
		public DilateQuality DilateQuality;

		// Token: 0x04000075 RID: 117
		public int DilateIterations = 2;

		// Token: 0x04000076 RID: 118
		public int BlurIterations = 5;

		// Token: 0x04000077 RID: 119
		public Vector2 Scale = Vector2.one;

		// Token: 0x04000078 RID: 120
		public Rect Viewport;

		// Token: 0x04000079 RID: 121
		public long OutlineLayerMask = -1L;

		// Token: 0x0400007A RID: 122
		public int TargetWidth;

		// Token: 0x0400007B RID: 123
		public int TargetHeight;

		// Token: 0x0400007C RID: 124
		public int ScaledBufferWidth;

		// Token: 0x0400007D RID: 125
		public int ScaledBufferHeight;

		// Token: 0x0400007E RID: 126
		public float BlurShift = 1f;

		// Token: 0x0400007F RID: 127
		public float DilateShift = 1f;

		// Token: 0x04000080 RID: 128
		public bool UseHDR;

		// Token: 0x04000081 RID: 129
		public bool UseInfoBuffer;

		// Token: 0x04000082 RID: 130
		public bool IsEditorCamera;

		// Token: 0x04000083 RID: 131
		public BufferSizeMode PrimaryBufferSizeMode;

		// Token: 0x04000084 RID: 132
		public int PrimaryBufferSizeReference;

		// Token: 0x04000085 RID: 133
		public float PrimaryBufferScale = 0.1f;

		// Token: 0x04000086 RID: 134
		public StereoTargetEyeMask EyeMask;

		// Token: 0x04000087 RID: 135
		public int Antialiasing = 1;

		// Token: 0x04000088 RID: 136
		public BlurType BlurType = BlurType.Gaussian13x13;

		// Token: 0x04000089 RID: 137
		public LayerMask Mask = -1;

		// Token: 0x0400008A RID: 138
		public Mesh BlitMesh;

		// Token: 0x0400008B RID: 139
		public List<Outlinable> OutlinablesToRender = new List<Outlinable>();

		// Token: 0x0400008C RID: 140
		public readonly Dictionary<Texture, RTHandle> TextureHandleMap = new Dictionary<Texture, RTHandle>();
	}
}
