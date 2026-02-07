using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sirenix.Utilities.Unsafe
{
	// Token: 0x02000038 RID: 56
	public static class UnsafeUtilities
	{
		// Token: 0x06000240 RID: 576 RVA: 0x0000D53E File Offset: 0x0000B73E
		public static T[] StructArrayFromBytes<T>(byte[] bytes, int byteLength) where T : struct
		{
			return UnsafeUtilities.StructArrayFromBytes<T>(bytes, 0, 0);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D548 File Offset: 0x0000B748
		public static T[] StructArrayFromBytes<T>(byte[] bytes, int byteLength, int byteOffset) where T : struct
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (byteLength <= 0)
			{
				throw new ArgumentException("Byte length must be larger than zero.");
			}
			if (byteOffset < 0)
			{
				throw new ArgumentException("Byte offset must be larger than or equal to zero.");
			}
			int num = Marshal.SizeOf(typeof(T));
			if (byteOffset % 8 != 0)
			{
				throw new ArgumentException("Byte offset must be divisible by " + 8.ToString() + " (IE, sizeof(ulong))");
			}
			if (byteLength + byteOffset >= bytes.Length)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Given byte array of size ",
					bytes.Length.ToString(),
					" is not large enough to copy requested number of bytes ",
					byteLength.ToString(),
					"."
				}));
			}
			if ((byteLength - byteOffset) % num != 0)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"The length in the given byte array (",
					bytes.Length.ToString(),
					", and ",
					(bytes.Length - byteOffset).ToString(),
					" minus byteOffset ",
					byteOffset.ToString(),
					") to convert to type ",
					typeof(T).Name,
					" is not divisible by the size of ",
					typeof(T).Name,
					" (",
					num.ToString(),
					")."
				}));
			}
			int num2 = (bytes.Length - byteOffset) / num;
			T[] array = new T[num2];
			UnsafeUtilities.MemoryCopy(bytes, array, byteLength, byteOffset, 0);
			return array;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public static byte[] StructArrayToBytes<T>(T[] array) where T : struct
		{
			byte[] array2 = null;
			return UnsafeUtilities.StructArrayToBytes<T>(array, ref array2, 0);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		public static byte[] StructArrayToBytes<T>(T[] array, ref byte[] bytes, int byteOffset) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (byteOffset < 0)
			{
				throw new ArgumentException("Byte offset must be larger than or equal to zero.");
			}
			int num = Marshal.SizeOf(typeof(T));
			int num2 = num * array.Length;
			if (bytes == null)
			{
				bytes = new byte[num2 + byteOffset];
			}
			else if (bytes.Length + byteOffset > num2)
			{
				throw new ArgumentException("Byte array must be at least " + (bytes.Length + byteOffset).ToString() + " long with the given byteOffset.");
			}
			UnsafeUtilities.MemoryCopy(array, bytes, num2, 0, byteOffset);
			return bytes;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000D76C File Offset: 0x0000B96C
		public unsafe static string StringFromBytes(byte[] buffer, int charLength, bool needs16BitSupport)
		{
			int num = needs16BitSupport ? (charLength * 2) : charLength;
			if (buffer.Length < num)
			{
				throw new ArgumentException("Buffer is not large enough to contain the given string; a size of at least " + num.ToString() + " is required.");
			}
			GCHandle gchandle = default(GCHandle);
			string text = new string('\0', charLength);
			try
			{
				gchandle = GCHandle.Alloc(buffer, 3);
				if (needs16BitSupport)
				{
					if (BitConverter.IsLittleEndian)
					{
						try
						{
							fixed (string text2 = text)
							{
								char* ptr = text2;
								if (ptr != null)
								{
									ptr += RuntimeHelpers.OffsetToStringData / 2;
								}
								ushort* ptr2 = (ushort*)gchandle.AddrOfPinnedObject().ToPointer();
								ushort* ptr3 = (ushort*)ptr;
								for (int i = 0; i < num; i += 2)
								{
									*(ptr3++) = *(ptr2++);
								}
								return text;
							}
						}
						finally
						{
							string text2 = null;
						}
					}
					try
					{
						fixed (string text3 = text)
						{
							char* ptr4 = text3;
							if (ptr4 != null)
							{
								ptr4 += RuntimeHelpers.OffsetToStringData / 2;
							}
							byte* ptr5 = (byte*)gchandle.AddrOfPinnedObject().ToPointer();
							byte* ptr6 = (byte*)ptr4;
							for (int j = 0; j < num; j += 2)
							{
								*ptr6 = ptr5[1];
								ptr6[1] = *ptr5;
								ptr5 += 2;
								ptr6 += 2;
							}
							return text;
						}
					}
					finally
					{
						string text3 = null;
					}
				}
				if (BitConverter.IsLittleEndian)
				{
					try
					{
						fixed (string text4 = text)
						{
							char* ptr7 = text4;
							if (ptr7 != null)
							{
								ptr7 += RuntimeHelpers.OffsetToStringData / 2;
							}
							byte* ptr8 = (byte*)gchandle.AddrOfPinnedObject().ToPointer();
							byte* ptr9 = (byte*)ptr7;
							for (int k = 0; k < num; k++)
							{
								*(ptr9++) = *(ptr8++);
								ptr9++;
							}
							return text;
						}
					}
					finally
					{
						string text4 = null;
					}
				}
				try
				{
					fixed (string text5 = text)
					{
						char* ptr10 = text5;
						if (ptr10 != null)
						{
							ptr10 += RuntimeHelpers.OffsetToStringData / 2;
						}
						byte* ptr11 = (byte*)gchandle.AddrOfPinnedObject().ToPointer();
						byte* ptr12 = (byte*)ptr10;
						for (int l = 0; l < num; l++)
						{
							ptr12++;
							*(ptr12++) = *(ptr11++);
						}
					}
				}
				finally
				{
					string text5 = null;
				}
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return text;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		public unsafe static int StringToBytes(byte[] buffer, string value, bool needs16BitSupport)
		{
			int num = needs16BitSupport ? (value.Length * 2) : value.Length;
			if (buffer.Length < num)
			{
				throw new ArgumentException("Buffer is not large enough to contain the given string; a size of at least " + num.ToString() + " is required.");
			}
			GCHandle gchandle = default(GCHandle);
			try
			{
				gchandle = GCHandle.Alloc(buffer, 3);
				if (needs16BitSupport)
				{
					if (BitConverter.IsLittleEndian)
					{
						try
						{
							fixed (string text = value)
							{
								char* ptr = text;
								if (ptr != null)
								{
									ptr += RuntimeHelpers.OffsetToStringData / 2;
								}
								ushort* ptr2 = (ushort*)ptr;
								ushort* ptr3 = (ushort*)gchandle.AddrOfPinnedObject().ToPointer();
								for (int i = 0; i < num; i += 2)
								{
									*(ptr3++) = *(ptr2++);
								}
								return num;
							}
						}
						finally
						{
							string text = null;
						}
					}
					try
					{
						fixed (string text2 = value)
						{
							char* ptr4 = text2;
							if (ptr4 != null)
							{
								ptr4 += RuntimeHelpers.OffsetToStringData / 2;
							}
							byte* ptr5 = (byte*)ptr4;
							byte* ptr6 = (byte*)gchandle.AddrOfPinnedObject().ToPointer();
							for (int j = 0; j < num; j += 2)
							{
								*ptr6 = ptr5[1];
								ptr6[1] = *ptr5;
								ptr5 += 2;
								ptr6 += 2;
							}
							return num;
						}
					}
					finally
					{
						string text2 = null;
					}
				}
				if (BitConverter.IsLittleEndian)
				{
					try
					{
						fixed (string text3 = value)
						{
							char* ptr7 = text3;
							if (ptr7 != null)
							{
								ptr7 += RuntimeHelpers.OffsetToStringData / 2;
							}
							byte* ptr8 = (byte*)ptr7;
							byte* ptr9 = (byte*)gchandle.AddrOfPinnedObject().ToPointer();
							for (int k = 0; k < num; k++)
							{
								ptr8++;
								*(ptr9++) = *(ptr8++);
							}
							return num;
						}
					}
					finally
					{
						string text3 = null;
					}
				}
				try
				{
					fixed (string text4 = value)
					{
						char* ptr10 = text4;
						if (ptr10 != null)
						{
							ptr10 += RuntimeHelpers.OffsetToStringData / 2;
						}
						byte* ptr11 = (byte*)ptr10;
						byte* ptr12 = (byte*)gchandle.AddrOfPinnedObject().ToPointer();
						for (int l = 0; l < num; l++)
						{
							*(ptr12++) = *(ptr11++);
							ptr11++;
						}
					}
				}
				finally
				{
					string text4 = null;
				}
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return num;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		public unsafe static void MemoryCopy(object from, object to, int byteCount, int fromByteOffset, int toByteOffset)
		{
			GCHandle gchandle = default(GCHandle);
			GCHandle gchandle2 = default(GCHandle);
			if (fromByteOffset % 8 != 0 || toByteOffset % 8 != 0)
			{
				throw new ArgumentException("Byte offset must be divisible by " + 8.ToString() + " (IE, sizeof(ulong))");
			}
			try
			{
				int num = byteCount % 8;
				int num2 = (byteCount - num) / 8;
				int num3 = fromByteOffset / 8;
				int num4 = toByteOffset / 8;
				gchandle = GCHandle.Alloc(from, 3);
				gchandle2 = GCHandle.Alloc(to, 3);
				ulong* ptr = (ulong*)gchandle.AddrOfPinnedObject().ToPointer();
				ulong* ptr2 = (ulong*)gchandle2.AddrOfPinnedObject().ToPointer();
				if (num3 > 0)
				{
					ptr += num3;
				}
				if (num4 > 0)
				{
					ptr2 += num4;
				}
				for (int i = 0; i < num2; i++)
				{
					*(ptr2++) = *(ptr++);
				}
				if (num > 0)
				{
					byte* ptr3 = (byte*)ptr;
					byte* ptr4 = (byte*)ptr2;
					for (int j = 0; j < num; j++)
					{
						*(ptr4++) = *(ptr3++);
					}
				}
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (gchandle2.IsAllocated)
				{
					gchandle2.Free();
				}
			}
		}
	}
}
