using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200003C RID: 60
	internal struct ShapeDrawCall
	{
		// Token: 0x06000C19 RID: 3097 RVA: 0x00018458 File Offset: 0x00016658
		public ShapeDrawCall(ShapeDrawState drawState, Matrix4x4 matrix, MaterialPropertyBlock mpbOverride = null)
		{
			this.count = 1;
			this.drawState = drawState;
			this.matrix = matrix;
			this.instanced = false;
			this.usingOverrideMpb = (mpbOverride != null);
			this.mpb = (this.usingOverrideMpb ? mpbOverride : ObjectPool<MaterialPropertyBlock>.Alloc());
			this.matrices = null;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000184A8 File Offset: 0x000166A8
		public ShapeDrawCall(ShapeDrawState drawState, int count, Matrix4x4[] matrices, MaterialPropertyBlock mpbOverride = null)
		{
			this.count = count;
			this.drawState = drawState;
			this.matrices = matrices;
			this.instanced = true;
			this.usingOverrideMpb = (mpbOverride != null);
			this.mpb = (this.usingOverrideMpb ? mpbOverride : ObjectPool<MaterialPropertyBlock>.Alloc());
			this.matrix = default(Matrix4x4);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00018500 File Offset: 0x00016700
		public void AddToCommandBuffer(CommandBuffer cmd)
		{
			if (this.instanced)
			{
				cmd.DrawMeshInstanced(this.drawState.mesh, this.drawState.submesh, this.drawState.mat, 0, this.matrices, this.count, this.mpb);
				return;
			}
			cmd.DrawMesh(this.drawState.mesh, this.matrix, this.drawState.mat, this.drawState.submesh, 0, this.mpb);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00018584 File Offset: 0x00016784
		public void Cleanup()
		{
			if (!this.usingOverrideMpb)
			{
				this.mpb.Clear();
				ObjectPool<MaterialPropertyBlock>.Free(this.mpb);
			}
			else
			{
				this.mpb = null;
			}
			if (this.instanced)
			{
				ArrayPool<Matrix4x4>.Free(this.matrices);
			}
			this.drawState.mat = null;
			this.drawState.mesh = null;
		}

		// Token: 0x040001A0 RID: 416
		public ShapeDrawState drawState;

		// Token: 0x040001A1 RID: 417
		public MaterialPropertyBlock mpb;

		// Token: 0x040001A2 RID: 418
		private bool usingOverrideMpb;

		// Token: 0x040001A3 RID: 419
		public int count;

		// Token: 0x040001A4 RID: 420
		public Matrix4x4 matrix;

		// Token: 0x040001A5 RID: 421
		public Matrix4x4[] matrices;

		// Token: 0x040001A6 RID: 422
		private bool instanced;
	}
}
