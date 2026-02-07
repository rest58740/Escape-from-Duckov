using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000033 RID: 51
	internal readonly struct SpanCompat<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000E311 File Offset: 0x0000C511
		public int Length
		{
			get
			{
				return (int)this.length;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000E319 File Offset: 0x0000C519
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe SpanCompat(void* ptr, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (length > 0 && ptr == null)
			{
				throw new ArgumentNullException();
			}
			this.ptr = (T*)ptr;
			this.length = (uint)length;
		}

		// Token: 0x17000019 RID: 25
		public unsafe T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= (int)this.length)
				{
					throw new IndexOutOfRangeException();
				}
				return ref this.ptr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x040000BB RID: 187
		internal unsafe readonly T* ptr;

		// Token: 0x040000BC RID: 188
		internal readonly uint length;
	}
}
