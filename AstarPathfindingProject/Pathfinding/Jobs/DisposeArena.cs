using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Pathfinding.Jobs
{
	// Token: 0x02000180 RID: 384
	public class DisposeArena
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x0003D53D File Offset: 0x0003B73D
		public void Add<[IsUnmanaged] T>(NativeArray<T> data) where T : struct, ValueType
		{
			if (this.buffer == null)
			{
				this.buffer = ListPool<NativeArray<byte>>.Claim();
			}
			this.buffer.Add(data.Reinterpret<byte>(UnsafeUtility.SizeOf<T>()));
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003D56C File Offset: 0x0003B76C
		public unsafe void Add<[IsUnmanaged] T>(NativeList<T> data) where T : struct, ValueType
		{
			NativeList<byte> item = *UnsafeUtility.As<NativeList<T>, NativeList<byte>>(ref data);
			if (this.buffer2 == null)
			{
				this.buffer2 = ListPool<NativeList<byte>>.Claim();
			}
			this.buffer2.Add(item);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003D5A8 File Offset: 0x0003B7A8
		public unsafe void Add<[IsUnmanaged] T>(NativeQueue<T> data) where T : struct, ValueType
		{
			NativeQueue<byte> item = *UnsafeUtility.As<NativeQueue<T>, NativeQueue<byte>>(ref data);
			if (this.buffer3 == null)
			{
				this.buffer3 = ListPool<NativeQueue<byte>>.Claim();
			}
			this.buffer3.Add(item);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003D5E4 File Offset: 0x0003B7E4
		public unsafe void Remove<[IsUnmanaged] T>(NativeArray<T> data) where T : struct, ValueType
		{
			if (this.buffer == null)
			{
				return;
			}
			void* unsafeBufferPointerWithoutChecks = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(data);
			for (int i = 0; i < this.buffer.Count; i++)
			{
				if (NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.buffer[i]) == unsafeBufferPointerWithoutChecks)
				{
					this.buffer.RemoveAtSwapBack(i);
					return;
				}
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0003D638 File Offset: 0x0003B838
		public void Add<T>(T data) where T : IArenaDisposable
		{
			data.DisposeWith(this);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0003D648 File Offset: 0x0003B848
		public void Add(GCHandle handle)
		{
			if (this.gcHandles == null)
			{
				this.gcHandles = ListPool<GCHandle>.Claim();
			}
			this.gcHandles.Add(handle);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0003D66C File Offset: 0x0003B86C
		public void DisposeAll()
		{
			if (this.buffer != null)
			{
				for (int i = 0; i < this.buffer.Count; i++)
				{
					this.buffer[i].Dispose();
				}
				ListPool<NativeArray<byte>>.Release(ref this.buffer);
			}
			if (this.buffer2 != null)
			{
				for (int j = 0; j < this.buffer2.Count; j++)
				{
					this.buffer2[j].Dispose();
				}
				ListPool<NativeList<byte>>.Release(ref this.buffer2);
			}
			if (this.buffer3 != null)
			{
				for (int k = 0; k < this.buffer3.Count; k++)
				{
					this.buffer3[k].Dispose();
				}
				ListPool<NativeQueue<byte>>.Release(ref this.buffer3);
			}
			if (this.gcHandles != null)
			{
				for (int l = 0; l < this.gcHandles.Count; l++)
				{
					this.gcHandles[l].Free();
				}
				ListPool<GCHandle>.Release(ref this.gcHandles);
			}
		}

		// Token: 0x0400074F RID: 1871
		private List<NativeArray<byte>> buffer;

		// Token: 0x04000750 RID: 1872
		private List<NativeList<byte>> buffer2;

		// Token: 0x04000751 RID: 1873
		private List<NativeQueue<byte>> buffer3;

		// Token: 0x04000752 RID: 1874
		private List<GCHandle> gcHandles;
	}
}
