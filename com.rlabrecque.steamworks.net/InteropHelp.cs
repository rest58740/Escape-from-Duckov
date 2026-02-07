using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	// Token: 0x0200018C RID: 396
	public class InteropHelp
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x0000D5EB File Offset: 0x0000B7EB
		public static void TestIfPlatformSupported()
		{
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0000D5ED File Offset: 0x0000B7ED
		public static void TestIfAvailableClient()
		{
			InteropHelp.TestIfPlatformSupported();
			if (CSteamAPIContext.GetSteamClient() == IntPtr.Zero && !CSteamAPIContext.Init())
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0000D617 File Offset: 0x0000B817
		public static void TestIfAvailableGameServer()
		{
			InteropHelp.TestIfPlatformSupported();
			if (CSteamGameServerAPIContext.GetSteamClient() == IntPtr.Zero && !CSteamGameServerAPIContext.Init())
			{
				throw new InvalidOperationException("Steamworks GameServer is not initialized.");
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0000D644 File Offset: 0x0000B844
		public static string PtrToStringUTF8(IntPtr nativeUtf8)
		{
			if (nativeUtf8 == IntPtr.Zero)
			{
				return null;
			}
			int num = 0;
			while (Marshal.ReadByte(nativeUtf8, num) != 0)
			{
				num++;
			}
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			Marshal.Copy(nativeUtf8, array, 0, array.Length);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0000D698 File Offset: 0x0000B898
		public static string ByteArrayToStringUTF8(byte[] buffer)
		{
			int num = 0;
			while (num < buffer.Length && buffer[num] != 0)
			{
				num++;
			}
			return Encoding.UTF8.GetString(buffer, 0, num);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public static void StringToByteArrayUTF8(string str, byte[] outArrayBuffer, int outArrayBufferSize)
		{
			outArrayBuffer = new byte[outArrayBufferSize];
			int bytes = Encoding.UTF8.GetBytes(str, 0, str.Length, outArrayBuffer, 0);
			outArrayBuffer[bytes] = 0;
		}

		// Token: 0x020001DA RID: 474
		public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x06000BCA RID: 3018 RVA: 0x00011410 File Offset: 0x0000F610
			public UTF8StringHandle(string str) : base(true)
			{
				if (str == null)
				{
					base.SetHandle(IntPtr.Zero);
					return;
				}
				byte[] array = new byte[Encoding.UTF8.GetByteCount(str) + 1];
				Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				base.SetHandle(intPtr);
			}

			// Token: 0x06000BCB RID: 3019 RVA: 0x00011476 File Offset: 0x0000F676
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		// Token: 0x020001DB RID: 475
		public class SteamParamStringArray
		{
			// Token: 0x06000BCC RID: 3020 RVA: 0x0001148C File Offset: 0x0000F68C
			public SteamParamStringArray(IList<string> strings)
			{
				if (strings == null)
				{
					this.m_pSteamParamStringArray = IntPtr.Zero;
					return;
				}
				this.m_Strings = new IntPtr[strings.Count];
				for (int i = 0; i < strings.Count; i++)
				{
					byte[] array = new byte[Encoding.UTF8.GetByteCount(strings[i]) + 1];
					Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, array, 0);
					this.m_Strings[i] = Marshal.AllocHGlobal(array.Length);
					Marshal.Copy(array, 0, this.m_Strings[i], array.Length);
				}
				this.m_ptrStrings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * this.m_Strings.Length);
				SteamParamStringArray_t steamParamStringArray_t = new SteamParamStringArray_t
				{
					m_ppStrings = this.m_ptrStrings,
					m_nNumStrings = this.m_Strings.Length
				};
				Marshal.Copy(this.m_Strings, 0, steamParamStringArray_t.m_ppStrings, this.m_Strings.Length);
				this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SteamParamStringArray_t)));
				Marshal.StructureToPtr<SteamParamStringArray_t>(steamParamStringArray_t, this.m_pSteamParamStringArray, false);
			}

			// Token: 0x06000BCD RID: 3021 RVA: 0x000115B8 File Offset: 0x0000F7B8
			protected override void Finalize()
			{
				try
				{
					if (this.m_Strings != null)
					{
						IntPtr[] strings = this.m_Strings;
						for (int i = 0; i < strings.Length; i++)
						{
							Marshal.FreeHGlobal(strings[i]);
						}
					}
					if (this.m_ptrStrings != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_ptrStrings);
					}
					if (this.m_pSteamParamStringArray != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x06000BCE RID: 3022 RVA: 0x00011640 File Offset: 0x0000F840
			public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}

			// Token: 0x04000B6A RID: 2922
			private IntPtr[] m_Strings;

			// Token: 0x04000B6B RID: 2923
			private IntPtr m_ptrStrings;

			// Token: 0x04000B6C RID: 2924
			private IntPtr m_pSteamParamStringArray;
		}
	}
}
