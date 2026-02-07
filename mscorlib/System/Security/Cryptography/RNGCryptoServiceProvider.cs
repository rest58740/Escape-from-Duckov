using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	// Token: 0x020004CC RID: 1228
	[ComVisible(true)]
	public sealed class RNGCryptoServiceProvider : RandomNumberGenerator
	{
		// Token: 0x06003129 RID: 12585 RVA: 0x000B5ADF File Offset: 0x000B3CDF
		static RNGCryptoServiceProvider()
		{
			if (RNGCryptoServiceProvider.RngOpen())
			{
				RNGCryptoServiceProvider._lock = new object();
			}
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000B5AF2 File Offset: 0x000B3CF2
		public RNGCryptoServiceProvider()
		{
			this._handle = RNGCryptoServiceProvider.RngInitialize(null, IntPtr.Zero);
			this.Check();
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000B5B14 File Offset: 0x000B3D14
		public unsafe RNGCryptoServiceProvider(byte[] rgb)
		{
			fixed (byte[] array = rgb)
			{
				byte* seed;
				if (rgb == null || array.Length == 0)
				{
					seed = null;
				}
				else
				{
					seed = &array[0];
				}
				this._handle = RNGCryptoServiceProvider.RngInitialize(seed, (rgb != null) ? ((IntPtr)rgb.Length) : IntPtr.Zero);
			}
			this.Check();
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000B5AF2 File Offset: 0x000B3CF2
		public RNGCryptoServiceProvider(CspParameters cspParams)
		{
			this._handle = RNGCryptoServiceProvider.RngInitialize(null, IntPtr.Zero);
			this.Check();
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000B5B68 File Offset: 0x000B3D68
		public unsafe RNGCryptoServiceProvider(string str)
		{
			if (str == null)
			{
				this._handle = RNGCryptoServiceProvider.RngInitialize(null, IntPtr.Zero);
			}
			else
			{
				byte[] bytes = Encoding.UTF8.GetBytes(str);
				byte[] array;
				byte* seed;
				if ((array = bytes) == null || array.Length == 0)
				{
					seed = null;
				}
				else
				{
					seed = &array[0];
				}
				this._handle = RNGCryptoServiceProvider.RngInitialize(seed, (IntPtr)bytes.Length);
				array = null;
			}
			this.Check();
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000B5BD2 File Offset: 0x000B3DD2
		private void Check()
		{
			if (this._handle == IntPtr.Zero)
			{
				throw new CryptographicException(Locale.GetText("Couldn't access random source."));
			}
		}

		// Token: 0x0600312F RID: 12591
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RngOpen();

		// Token: 0x06003130 RID: 12592
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr RngInitialize(byte* seed, IntPtr seed_length);

		// Token: 0x06003131 RID: 12593
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr RngGetBytes(IntPtr handle, byte* data, IntPtr data_length);

		// Token: 0x06003132 RID: 12594
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RngClose(IntPtr handle);

		// Token: 0x06003133 RID: 12595 RVA: 0x000B5BF8 File Offset: 0x000B3DF8
		public unsafe override void GetBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			fixed (byte[] array = data)
			{
				byte* data2;
				if (data == null || array.Length == 0)
				{
					data2 = null;
				}
				else
				{
					data2 = &array[0];
				}
				if (RNGCryptoServiceProvider._lock == null)
				{
					this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data2, (IntPtr)((long)data.Length));
				}
				else
				{
					object @lock = RNGCryptoServiceProvider._lock;
					lock (@lock)
					{
						this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data2, (IntPtr)((long)data.Length));
					}
				}
			}
			this.Check();
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000B5C9C File Offset: 0x000B3E9C
		internal unsafe void GetBytes(byte* data, IntPtr data_length)
		{
			if (RNGCryptoServiceProvider._lock == null)
			{
				this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data, data_length);
			}
			else
			{
				object @lock = RNGCryptoServiceProvider._lock;
				lock (@lock)
				{
					this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data, data_length);
				}
			}
			this.Check();
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000B5D0C File Offset: 0x000B3F0C
		public unsafe override void GetNonZeroBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] array = new byte[(long)data.Length * 2L];
			long num = 0L;
			while (num < (long)data.Length)
			{
				byte[] array2;
				byte* data2;
				if ((array2 = array) == null || array2.Length == 0)
				{
					data2 = null;
				}
				else
				{
					data2 = &array2[0];
				}
				this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data2, (IntPtr)((long)array.Length));
				array2 = null;
				this.Check();
				long num2 = 0L;
				while (num2 < (long)array.Length && num != (long)data.Length)
				{
					checked
					{
						if (array[(int)((IntPtr)num2)] != 0)
						{
							long num3 = num;
							num = unchecked(num3 + 1L);
							data[(int)((IntPtr)num3)] = array[(int)((IntPtr)num2)];
						}
					}
					num2 += 1L;
				}
			}
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000B5DA8 File Offset: 0x000B3FA8
		~RNGCryptoServiceProvider()
		{
			if (this._handle != IntPtr.Zero)
			{
				RNGCryptoServiceProvider.RngClose(this._handle);
				this._handle = IntPtr.Zero;
			}
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000B5DF8 File Offset: 0x000B3FF8
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0400226B RID: 8811
		private static object _lock;

		// Token: 0x0400226C RID: 8812
		private IntPtr _handle;
	}
}
