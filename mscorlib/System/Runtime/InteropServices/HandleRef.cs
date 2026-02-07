using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006C5 RID: 1733
	public readonly struct HandleRef
	{
		// Token: 0x06003FD7 RID: 16343 RVA: 0x000DFE57 File Offset: 0x000DE057
		public HandleRef(object wrapper, IntPtr handle)
		{
			this._wrapper = wrapper;
			this._handle = handle;
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x000DFE67 File Offset: 0x000DE067
		public object Wrapper
		{
			get
			{
				return this._wrapper;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x000DFE6F File Offset: 0x000DE06F
		public IntPtr Handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x000DFE6F File Offset: 0x000DE06F
		public static explicit operator IntPtr(HandleRef value)
		{
			return value._handle;
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x000DFE6F File Offset: 0x000DE06F
		public static IntPtr ToIntPtr(HandleRef value)
		{
			return value._handle;
		}

		// Token: 0x040029F3 RID: 10739
		private readonly object _wrapper;

		// Token: 0x040029F4 RID: 10740
		private readonly IntPtr _handle;
	}
}
