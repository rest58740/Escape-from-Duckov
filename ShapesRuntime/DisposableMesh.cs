using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000017 RID: 23
	public class DisposableMesh : IDisposable
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00007599 File Offset: 0x00005799
		public static int ActiveMeshCount
		{
			get
			{
				return DisposableMesh.activeMeshCount;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000075A0 File Offset: 0x000057A0
		protected void EnsureMeshExists()
		{
			if (!this.hasData)
			{
				Debug.LogError("Mesh requested, but there's no data to generate a mesh from");
				return;
			}
			if (!this.hasMesh || this.mesh == null)
			{
				this.mesh = ShapesMeshPool.GetMesh();
				DisposableMesh.activeMeshCount++;
				this.hasMesh = true;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000075F4 File Offset: 0x000057F4
		internal void RegisterToCommandBuffer(DrawCommand cmd)
		{
			DisposableMesh.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cmd = cmd;
			if (this.usedByCommands == null)
			{
				this.usedByCommands = ListPool<DrawCommand>.Alloc();
				this.<RegisterToCommandBuffer>g__Add|10_0(ref CS$<>8__locals1);
				return;
			}
			if (!this.usedByCommands.Contains(CS$<>8__locals1.cmd))
			{
				this.<RegisterToCommandBuffer>g__Add|10_0(ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00007648 File Offset: 0x00005848
		internal void ReleaseFromCommand(DrawCommand cmd)
		{
			this.usedByCommands.Remove(cmd);
			if (this.usedByCommands.Count == 0 && this.disposeWhenFullyReleased)
			{
				this.Dispose();
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00007674 File Offset: 0x00005874
		public void Dispose()
		{
			this.disposeWhenFullyReleased = true;
			bool flag = this.usedByCommands != null;
			if (flag && this.usedByCommands.Count == 0)
			{
				ListPool<DrawCommand>.Free(this.usedByCommands);
				this.usedByCommands = null;
				flag = false;
			}
			if (this.hasMesh && !flag)
			{
				ShapesMeshPool.Release(this.mesh);
				DisposableMesh.activeMeshCount--;
				this.hasMesh = false;
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000076DF File Offset: 0x000058DF
		protected void ClearMesh()
		{
			if (this.hasMesh)
			{
				this.mesh.Clear();
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000076F4 File Offset: 0x000058F4
		protected virtual bool ExternallyDirty()
		{
			return false;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000076F7 File Offset: 0x000058F7
		protected virtual void UpdateMesh()
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000076FC File Offset: 0x000058FC
		protected bool EnsureMeshIsReadyToRender(out Mesh outMesh, Action updateMesh)
		{
			if (!this.hasData)
			{
				outMesh = null;
				return false;
			}
			if (!this.hasMesh)
			{
				this.EnsureMeshExists();
				updateMesh.Invoke();
				this.meshDirty = false;
			}
			else if (this.meshDirty)
			{
				updateMesh.Invoke();
				this.meshDirty = false;
			}
			outMesh = this.mesh;
			return this.hasMesh;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000775E File Offset: 0x0000595E
		[CompilerGenerated]
		private void <RegisterToCommandBuffer>g__Add|10_0(ref DisposableMesh.<>c__DisplayClass10_0 A_1)
		{
			this.usedByCommands.Add(A_1.cmd);
			A_1.cmd.cachedMeshes.Add(this);
		}

		// Token: 0x0400009C RID: 156
		private static int activeMeshCount;

		// Token: 0x0400009D RID: 157
		protected Mesh mesh;

		// Token: 0x0400009E RID: 158
		protected bool meshDirty;

		// Token: 0x0400009F RID: 159
		protected bool hasData;

		// Token: 0x040000A0 RID: 160
		private bool hasMesh;

		// Token: 0x040000A1 RID: 161
		private bool disposeWhenFullyReleased;

		// Token: 0x040000A2 RID: 162
		internal List<DrawCommand> usedByCommands;
	}
}
