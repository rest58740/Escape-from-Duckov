using System;
using System.Collections.Generic;
using EPOOutline.Utility;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000021 RID: 33
	public class RTHandlePool : IDisposable
	{
		// Token: 0x060000CB RID: 203 RVA: 0x00006385 File Offset: 0x00004585
		public RTHandle Allocate(Texture target)
		{
			RTHandle free = this.textureSegment.GetFree();
			free.SetTexture(target);
			return free;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006399 File Offset: 0x00004599
		public RTHandle Allocate(RenderTargetIdentifier target)
		{
			RTHandle free = this.rtiSegment.GetFree();
			free.SetRenderTargetIdentifier(target);
			return free;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000063AD File Offset: 0x000045AD
		public void ReleaseAll()
		{
			this.textureSegment.ReleaseAll();
			this.rtiSegment.ReleaseAll();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000063C5 File Offset: 0x000045C5
		public void Dispose()
		{
			this.textureSegment.Dispose();
			this.rtiSegment.Dispose();
		}

		// Token: 0x040000C1 RID: 193
		private readonly RTHandlePool.PoolSegment textureSegment = new RTHandlePool.PoolSegment();

		// Token: 0x040000C2 RID: 194
		private readonly RTHandlePool.PoolSegment rtiSegment = new RTHandlePool.PoolSegment();

		// Token: 0x02000032 RID: 50
		private class PoolSegment : IDisposable
		{
			// Token: 0x0600012C RID: 300 RVA: 0x0000719C File Offset: 0x0000539C
			public RTHandle GetFree()
			{
				if (this.free.Count != 0)
				{
					return this.free.Dequeue();
				}
				RTHandle rthandle = OutlineEffect.HandleSystem.Alloc(default(RenderTargetIdentifier));
				this.free.Enqueue(rthandle);
				this.allocated.Add(rthandle);
				return rthandle;
			}

			// Token: 0x0600012D RID: 301 RVA: 0x000071F0 File Offset: 0x000053F0
			public void ReleaseAll()
			{
				this.free.Clear();
				foreach (RTHandle item in this.allocated)
				{
					this.free.Enqueue(item);
				}
			}

			// Token: 0x0600012E RID: 302 RVA: 0x00007254 File Offset: 0x00005454
			public void Dispose()
			{
				foreach (RTHandle rthandle in this.allocated)
				{
					RTHandleUtility.RemoveDelegates(rthandle);
					rthandle.Release();
				}
				this.allocated.Clear();
				this.free.Clear();
			}

			// Token: 0x0400010D RID: 269
			private List<RTHandle> allocated = new List<RTHandle>();

			// Token: 0x0400010E RID: 270
			private Queue<RTHandle> free = new Queue<RTHandle>();
		}
	}
}
