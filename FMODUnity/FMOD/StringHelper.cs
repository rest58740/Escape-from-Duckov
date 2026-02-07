using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FMOD
{
	// Token: 0x02000055 RID: 85
	internal static class StringHelper
	{
		// Token: 0x060003F4 RID: 1012 RVA: 0x00004D64 File Offset: 0x00002F64
		public static StringHelper.ThreadSafeEncoding GetFreeHelper()
		{
			List<StringHelper.ThreadSafeEncoding> obj = StringHelper.encoders;
			StringHelper.ThreadSafeEncoding result;
			lock (obj)
			{
				StringHelper.ThreadSafeEncoding threadSafeEncoding = null;
				for (int i = 0; i < StringHelper.encoders.Count; i++)
				{
					if (!StringHelper.encoders[i].InUse())
					{
						threadSafeEncoding = StringHelper.encoders[i];
						break;
					}
				}
				if (threadSafeEncoding == null)
				{
					threadSafeEncoding = new StringHelper.ThreadSafeEncoding();
					StringHelper.encoders.Add(threadSafeEncoding);
				}
				threadSafeEncoding.SetInUse();
				result = threadSafeEncoding;
			}
			return result;
		}

		// Token: 0x04000268 RID: 616
		private static List<StringHelper.ThreadSafeEncoding> encoders = new List<StringHelper.ThreadSafeEncoding>(1);

		// Token: 0x02000132 RID: 306
		public class ThreadSafeEncoding : IDisposable
		{
			// Token: 0x060007E0 RID: 2016 RVA: 0x0000C409 File Offset: 0x0000A609
			public bool InUse()
			{
				return this.inUse;
			}

			// Token: 0x060007E1 RID: 2017 RVA: 0x0000C411 File Offset: 0x0000A611
			public void SetInUse()
			{
				this.inUse = true;
			}

			// Token: 0x060007E2 RID: 2018 RVA: 0x0000C41C File Offset: 0x0000A61C
			private int roundUpPowerTwo(int number)
			{
				int i;
				for (i = 1; i <= number; i *= 2)
				{
				}
				return i;
			}

			// Token: 0x060007E3 RID: 2019 RVA: 0x0000C438 File Offset: 0x0000A638
			public byte[] byteFromStringUTF8(string s)
			{
				if (s == null)
				{
					return null;
				}
				if (this.encoding.GetMaxByteCount(s.Length) + 1 > this.encodedBuffer.Length)
				{
					int num = this.encoding.GetByteCount(s) + 1;
					if (num > this.encodedBuffer.Length)
					{
						this.encodedBuffer = new byte[this.roundUpPowerTwo(num)];
					}
				}
				int bytes = this.encoding.GetBytes(s, 0, s.Length, this.encodedBuffer, 0);
				this.encodedBuffer[bytes] = 0;
				return this.encodedBuffer;
			}

			// Token: 0x060007E4 RID: 2020 RVA: 0x0000C4BD File Offset: 0x0000A6BD
			public IntPtr intptrFromStringUTF8(string s)
			{
				if (s == null)
				{
					return IntPtr.Zero;
				}
				this.gcHandle = GCHandle.Alloc(this.byteFromStringUTF8(s), GCHandleType.Pinned);
				return this.gcHandle.AddrOfPinnedObject();
			}

			// Token: 0x060007E5 RID: 2021 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
			public string stringFromNative(IntPtr nativePtr)
			{
				if (nativePtr == IntPtr.Zero)
				{
					return "";
				}
				int num = 0;
				while (Marshal.ReadByte(nativePtr, num) != 0)
				{
					num++;
				}
				if (num == 0)
				{
					return "";
				}
				if (num > this.encodedBuffer.Length)
				{
					this.encodedBuffer = new byte[this.roundUpPowerTwo(num)];
				}
				Marshal.Copy(nativePtr, this.encodedBuffer, 0, num);
				if (this.encoding.GetMaxCharCount(num) > this.decodedBuffer.Length)
				{
					int charCount = this.encoding.GetCharCount(this.encodedBuffer, 0, num);
					if (charCount > this.decodedBuffer.Length)
					{
						this.decodedBuffer = new char[this.roundUpPowerTwo(charCount)];
					}
				}
				int chars = this.encoding.GetChars(this.encodedBuffer, 0, num, this.decodedBuffer, 0);
				return new string(this.decodedBuffer, 0, chars);
			}

			// Token: 0x060007E6 RID: 2022 RVA: 0x0000C5BC File Offset: 0x0000A7BC
			public void Dispose()
			{
				if (this.gcHandle.IsAllocated)
				{
					this.gcHandle.Free();
				}
				List<StringHelper.ThreadSafeEncoding> encoders = StringHelper.encoders;
				lock (encoders)
				{
					this.inUse = false;
				}
			}

			// Token: 0x04000686 RID: 1670
			private UTF8Encoding encoding = new UTF8Encoding();

			// Token: 0x04000687 RID: 1671
			private byte[] encodedBuffer = new byte[128];

			// Token: 0x04000688 RID: 1672
			private char[] decodedBuffer = new char[128];

			// Token: 0x04000689 RID: 1673
			private bool inUse;

			// Token: 0x0400068A RID: 1674
			private GCHandle gcHandle;
		}
	}
}
