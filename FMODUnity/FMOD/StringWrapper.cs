using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000054 RID: 84
	public struct StringWrapper
	{
		// Token: 0x060003F0 RID: 1008 RVA: 0x00004C7E File Offset: 0x00002E7E
		public StringWrapper(IntPtr ptr)
		{
			this.nativeUtf8Ptr = ptr;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00004C88 File Offset: 0x00002E88
		public static implicit operator string(StringWrapper fstring)
		{
			string result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = freeHelper.stringFromNative(fstring.nativeUtf8Ptr);
			}
			return result;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public bool StartsWith(byte[] prefix)
		{
			if (this.nativeUtf8Ptr == IntPtr.Zero)
			{
				return false;
			}
			for (int i = 0; i < prefix.Length; i++)
			{
				if (Marshal.ReadByte(this.nativeUtf8Ptr, i) != prefix[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00004D0C File Offset: 0x00002F0C
		public bool Equals(byte[] comparison)
		{
			if (this.nativeUtf8Ptr == IntPtr.Zero)
			{
				return false;
			}
			for (int i = 0; i < comparison.Length; i++)
			{
				if (Marshal.ReadByte(this.nativeUtf8Ptr, i) != comparison[i])
				{
					return false;
				}
			}
			return Marshal.ReadByte(this.nativeUtf8Ptr, comparison.Length) == 0;
		}

		// Token: 0x04000267 RID: 615
		private IntPtr nativeUtf8Ptr;
	}
}
