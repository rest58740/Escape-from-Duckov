using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200073F RID: 1855
	[ComVisible(true)]
	public struct GCHandle
	{
		// Token: 0x06004122 RID: 16674 RVA: 0x000E1DFF File Offset: 0x000DFFFF
		private GCHandle(IntPtr h)
		{
			this.handle = h;
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x000E1E08 File Offset: 0x000E0008
		private GCHandle(object obj)
		{
			this = new GCHandle(obj, GCHandleType.Normal);
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x000E1E12 File Offset: 0x000E0012
		internal GCHandle(object value, GCHandleType type)
		{
			if (type < GCHandleType.Weak || type > GCHandleType.Pinned)
			{
				type = GCHandleType.Normal;
			}
			this.handle = GCHandle.GetTargetHandle(value, IntPtr.Zero, type);
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06004125 RID: 16677 RVA: 0x000E1E31 File Offset: 0x000E0031
		public bool IsAllocated
		{
			get
			{
				return this.handle != IntPtr.Zero;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06004126 RID: 16678 RVA: 0x000E1E43 File Offset: 0x000E0043
		// (set) Token: 0x06004127 RID: 16679 RVA: 0x000E1E63 File Offset: 0x000E0063
		public object Target
		{
			get
			{
				if (!this.IsAllocated)
				{
					throw new InvalidOperationException("Handle is not allocated");
				}
				return GCHandle.GetTarget(this.handle);
			}
			set
			{
				this.handle = GCHandle.GetTargetHandle(value, this.handle, (GCHandleType)(-1));
			}
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x000E1E78 File Offset: 0x000E0078
		public IntPtr AddrOfPinnedObject()
		{
			IntPtr addrOfPinnedObject = GCHandle.GetAddrOfPinnedObject(this.handle);
			if (addrOfPinnedObject == (IntPtr)(-1))
			{
				throw new ArgumentException("Object contains non-primitive or non-blittable data.");
			}
			if (addrOfPinnedObject == (IntPtr)(-2))
			{
				throw new InvalidOperationException("Handle is not pinned.");
			}
			return addrOfPinnedObject;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x000E1EB8 File Offset: 0x000E00B8
		public static GCHandle Alloc(object value)
		{
			return new GCHandle(value);
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x000E1EC0 File Offset: 0x000E00C0
		public static GCHandle Alloc(object value, GCHandleType type)
		{
			return new GCHandle(value, type);
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x000E1ECC File Offset: 0x000E00CC
		public void Free()
		{
			IntPtr intPtr = this.handle;
			if (intPtr != IntPtr.Zero && Interlocked.CompareExchange(ref this.handle, IntPtr.Zero, intPtr) == intPtr)
			{
				GCHandle.FreeHandle(intPtr);
				return;
			}
			throw new InvalidOperationException("Handle is not initialized.");
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x000E1F17 File Offset: 0x000E0117
		public static explicit operator IntPtr(GCHandle value)
		{
			return value.handle;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x000E1F1F File Offset: 0x000E011F
		public static explicit operator GCHandle(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				throw new InvalidOperationException("GCHandle value cannot be zero");
			}
			if (!GCHandle.CheckCurrentDomain(value))
			{
				throw new ArgumentException("GCHandle value belongs to a different domain");
			}
			return new GCHandle(value);
		}

		// Token: 0x0600412E RID: 16686
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CheckCurrentDomain(IntPtr handle);

		// Token: 0x0600412F RID: 16687
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object GetTarget(IntPtr handle);

		// Token: 0x06004130 RID: 16688
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetTargetHandle(object obj, IntPtr handle, GCHandleType type);

		// Token: 0x06004131 RID: 16689
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeHandle(IntPtr handle);

		// Token: 0x06004132 RID: 16690
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetAddrOfPinnedObject(IntPtr handle);

		// Token: 0x06004133 RID: 16691 RVA: 0x000E1F52 File Offset: 0x000E0152
		public static bool operator ==(GCHandle a, GCHandle b)
		{
			return a.handle == b.handle;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x000E1F65 File Offset: 0x000E0165
		public static bool operator !=(GCHandle a, GCHandle b)
		{
			return !(a == b);
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x000E1F71 File Offset: 0x000E0171
		public override bool Equals(object o)
		{
			return o is GCHandle && this == (GCHandle)o;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x000E1F8E File Offset: 0x000E018E
		public override int GetHashCode()
		{
			return this.handle.GetHashCode();
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x000E1F9B File Offset: 0x000E019B
		public static GCHandle FromIntPtr(IntPtr value)
		{
			return (GCHandle)value;
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x000E1FA3 File Offset: 0x000E01A3
		public static IntPtr ToIntPtr(GCHandle value)
		{
			return (IntPtr)value;
		}

		// Token: 0x04002BC1 RID: 11201
		private IntPtr handle;
	}
}
