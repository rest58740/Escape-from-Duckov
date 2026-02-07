using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sirenix.Serialization.Utilities;
using Sirenix.Serialization.Utilities.Unsafe;

namespace Sirenix.Serialization
{
	// Token: 0x0200000D RID: 13
	public class BinaryDataWriter : BaseDataWriter
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00005501 File Offset: 0x00003701
		public BinaryDataWriter() : base(null, null)
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000553A File Offset: 0x0000373A
		public BinaryDataWriter(Stream stream, SerializationContext context) : base(stream, context)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005574 File Offset: 0x00003774
		public override void BeginArrayNode(long length)
		{
			this.EnsureBufferSpace(9);
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array[num] = 6;
			this.UNSAFE_WriteToBuffer_8_Int64(length);
			base.PushArray();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000055B0 File Offset: 0x000037B0
		public override void BeginReferenceNode(string name, Type type, int id)
		{
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 1;
				this.WriteStringFast(name);
			}
			else
			{
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = 2;
			}
			this.WriteType(type);
			this.EnsureBufferSpace(4);
			this.UNSAFE_WriteToBuffer_4_Int32(id);
			base.PushNode(name, id, type);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005628 File Offset: 0x00003828
		public override void BeginStructNode(string name, Type type)
		{
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 3;
				this.WriteStringFast(name);
			}
			else
			{
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = 4;
			}
			this.WriteType(type);
			base.PushNode(name, -1, type);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005691 File Offset: 0x00003891
		public override void Dispose()
		{
			this.FlushToStream();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000569C File Offset: 0x0000389C
		public override void EndArrayNode()
		{
			base.PopArray();
			this.EnsureBufferSpace(1);
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array[num] = 7;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000056D0 File Offset: 0x000038D0
		public override void EndNode(string name)
		{
			base.PopNode(name);
			this.EnsureBufferSpace(1);
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array[num] = 5;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005704 File Offset: 0x00003904
		private static void WritePrimitiveArray_byte(BinaryDataWriter writer, object o)
		{
			byte[] array = o as byte[];
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num = writer.bufferIndex;
			writer.bufferIndex = num + 1;
			array2[num] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(1);
			writer.FlushToStream();
			writer.Stream.Write(array, 0, array.Length);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005760 File Offset: 0x00003960
		private unsafe static void WritePrimitiveArray_sbyte(BinaryDataWriter writer, object o)
		{
			sbyte[] array = o as sbyte[];
			int num = 1;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (writer.TryEnsureBufferSpace(num2))
			{
				byte[] array3;
				byte* ptr;
				if ((array3 = writer.buffer) == null || array3.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array3[0];
				}
				sbyte[] array4;
				void* from;
				if ((array4 = array) == null || array4.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array4[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array4 = null;
				array3 = null;
				writer.bufferIndex += num2;
				return;
			}
			writer.FlushToStream();
			using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
			{
				UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
				writer.Stream.Write(buffer.Array, 0, num2);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005870 File Offset: 0x00003A70
		private unsafe static void WritePrimitiveArray_bool(BinaryDataWriter writer, object o)
		{
			bool[] array = o as bool[];
			int num = 1;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (writer.TryEnsureBufferSpace(num2))
			{
				byte[] array3;
				byte* ptr;
				if ((array3 = writer.buffer) == null || array3.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array3[0];
				}
				bool[] array4;
				void* from;
				if ((array4 = array) == null || array4.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array4[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array4 = null;
				array3 = null;
				writer.bufferIndex += num2;
				return;
			}
			writer.FlushToStream();
			using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
			{
				UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
				writer.Stream.Write(buffer.Array, 0, num2);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005980 File Offset: 0x00003B80
		private unsafe static void WritePrimitiveArray_char(BinaryDataWriter writer, object o)
		{
			char[] array = o as char[];
			int num = 2;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, (ushort)array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				char[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_2_Char(array[j]);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005AEC File Offset: 0x00003CEC
		private unsafe static void WritePrimitiveArray_short(BinaryDataWriter writer, object o)
		{
			short[] array = o as short[];
			int num = 2;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				short[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_2_Int16(array[j]);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005C58 File Offset: 0x00003E58
		private unsafe static void WritePrimitiveArray_int(BinaryDataWriter writer, object o)
		{
			int[] array = o as int[];
			int num = 4;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				int[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_4_Int32(array[j]);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005DC4 File Offset: 0x00003FC4
		private unsafe static void WritePrimitiveArray_long(BinaryDataWriter writer, object o)
		{
			long[] array = o as long[];
			int num = 8;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				long[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_8_Int64(array[j]);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005F30 File Offset: 0x00004130
		private unsafe static void WritePrimitiveArray_ushort(BinaryDataWriter writer, object o)
		{
			ushort[] array = o as ushort[];
			int num = 2;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				ushort[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_2_UInt16(array[j]);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000609C File Offset: 0x0000429C
		private unsafe static void WritePrimitiveArray_uint(BinaryDataWriter writer, object o)
		{
			uint[] array = o as uint[];
			int num = 4;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				uint[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_4_UInt32(array[j]);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006208 File Offset: 0x00004408
		private unsafe static void WritePrimitiveArray_ulong(BinaryDataWriter writer, object o)
		{
			ulong[] array = o as ulong[];
			int num = 8;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				ulong[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_8_UInt64(array[j]);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006374 File Offset: 0x00004574
		private unsafe static void WritePrimitiveArray_decimal(BinaryDataWriter writer, object o)
		{
			decimal[] array = o as decimal[];
			int num = 16;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				decimal[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_16_Decimal(array[j]);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000064E8 File Offset: 0x000046E8
		private unsafe static void WritePrimitiveArray_float(BinaryDataWriter writer, object o)
		{
			float[] array = o as float[];
			int num = 4;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				float[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_4_Float32(array[j]);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006654 File Offset: 0x00004854
		private unsafe static void WritePrimitiveArray_double(BinaryDataWriter writer, object o)
		{
			double[] array = o as double[];
			int num = 8;
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				double[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_8_Float64(array[j]);
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000067C0 File Offset: 0x000049C0
		private unsafe static void WritePrimitiveArray_Guid(BinaryDataWriter writer, object o)
		{
			Guid[] array = o as Guid[];
			int num = sizeof(Guid);
			int num2 = array.Length * num;
			writer.EnsureBufferSpace(9);
			byte[] array2 = writer.buffer;
			int num3 = writer.bufferIndex;
			writer.bufferIndex = num3 + 1;
			array2[num3] = 8;
			writer.UNSAFE_WriteToBuffer_4_Int32(array.Length);
			writer.UNSAFE_WriteToBuffer_4_Int32(num);
			if (!writer.TryEnsureBufferSpace(num2))
			{
				writer.FlushToStream();
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num2))
				{
					if (BitConverter.IsLittleEndian)
					{
						UnsafeUtilities.MemoryCopy(array, buffer.Array, num2, 0, 0);
					}
					else
					{
						byte[] array3 = buffer.Array;
						for (int i = 0; i < array.Length; i++)
						{
							ProperBitConverter.GetBytes(array3, i * num, array[i]);
						}
					}
					writer.Stream.Write(buffer.Array, 0, num2);
				}
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				byte[] array4;
				byte* ptr;
				if ((array4 = writer.buffer) == null || array4.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array4[0];
				}
				Guid[] array5;
				void* from;
				if ((array5 = array) == null || array5.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array5[0]);
				}
				void* to = (void*)(ptr + writer.bufferIndex);
				UnsafeUtilities.MemoryCopy(from, to, num2);
				array5 = null;
				array4 = null;
				writer.bufferIndex += num2;
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				writer.UNSAFE_WriteToBuffer_16_Guid(array[j]);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006938 File Offset: 0x00004B38
		public override void WritePrimitiveArray<T>(T[] array)
		{
			Action<BinaryDataWriter, object> action;
			if (!BinaryDataWriter.PrimitiveArrayWriters.TryGetValue(typeof(T), ref action))
			{
				throw new ArgumentException("Type " + typeof(T).Name + " is not a valid primitive array type.");
			}
			action.Invoke(this, array);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000698C File Offset: 0x00004B8C
		public override void WriteBoolean(string name, bool value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 43;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = ((value > false) ? 1 : 0);
				return;
			}
			this.EnsureBufferSpace(2);
			byte[] array3 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array3[num] = 44;
			byte[] array4 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array4[num] = ((value > false) ? 1 : 0);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006A28 File Offset: 0x00004C28
		public override void WriteByte(string name, byte value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 17;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = value;
				return;
			}
			this.EnsureBufferSpace(2);
			byte[] array3 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array3[num] = 18;
			byte[] array4 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array4[num] = value;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006ABC File Offset: 0x00004CBC
		public override void WriteChar(string name, char value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 37;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(2);
				this.UNSAFE_WriteToBuffer_2_Char(value);
				return;
			}
			this.EnsureBufferSpace(3);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 38;
			this.UNSAFE_WriteToBuffer_2_Char(value);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006B2C File Offset: 0x00004D2C
		public override void WriteDecimal(string name, decimal value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 35;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(16);
				this.UNSAFE_WriteToBuffer_16_Decimal(value);
				return;
			}
			this.EnsureBufferSpace(17);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 36;
			this.UNSAFE_WriteToBuffer_16_Decimal(value);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006BA0 File Offset: 0x00004DA0
		public override void WriteDouble(string name, double value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 33;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(8);
				this.UNSAFE_WriteToBuffer_8_Float64(value);
				return;
			}
			this.EnsureBufferSpace(9);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 34;
			this.UNSAFE_WriteToBuffer_8_Float64(value);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006C10 File Offset: 0x00004E10
		public override void WriteGuid(string name, Guid value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 41;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(16);
				this.UNSAFE_WriteToBuffer_16_Guid(value);
				return;
			}
			this.EnsureBufferSpace(17);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 42;
			this.UNSAFE_WriteToBuffer_16_Guid(value);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006C84 File Offset: 0x00004E84
		public override void WriteExternalReference(string name, Guid guid)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 13;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(16);
				this.UNSAFE_WriteToBuffer_16_Guid(guid);
				return;
			}
			this.EnsureBufferSpace(17);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 14;
			this.UNSAFE_WriteToBuffer_16_Guid(guid);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006CF8 File Offset: 0x00004EF8
		public override void WriteExternalReference(string name, int index)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 11;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(4);
				this.UNSAFE_WriteToBuffer_4_Int32(index);
				return;
			}
			this.EnsureBufferSpace(5);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 12;
			this.UNSAFE_WriteToBuffer_4_Int32(index);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006D68 File Offset: 0x00004F68
		public override void WriteExternalReference(string name, string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 50;
				this.WriteStringFast(name);
			}
			else
			{
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = 51;
			}
			this.WriteStringFast(id);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public override void WriteInt32(string name, int value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 23;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(4);
				this.UNSAFE_WriteToBuffer_4_Int32(value);
				return;
			}
			this.EnsureBufferSpace(5);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 24;
			this.UNSAFE_WriteToBuffer_4_Int32(value);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006E48 File Offset: 0x00005048
		public override void WriteInt64(string name, long value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 27;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(8);
				this.UNSAFE_WriteToBuffer_8_Int64(value);
				return;
			}
			this.EnsureBufferSpace(9);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 28;
			this.UNSAFE_WriteToBuffer_8_Int64(value);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006EB8 File Offset: 0x000050B8
		public override void WriteNull(string name)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 45;
				this.WriteStringFast(name);
				return;
			}
			this.EnsureBufferSpace(1);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 46;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006F14 File Offset: 0x00005114
		public override void WriteInternalReference(string name, int id)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 9;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(4);
				this.UNSAFE_WriteToBuffer_4_Int32(id);
				return;
			}
			this.EnsureBufferSpace(5);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 10;
			this.UNSAFE_WriteToBuffer_4_Int32(id);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006F84 File Offset: 0x00005184
		public override void WriteSByte(string name, sbyte value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 15;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = (byte)value;
				return;
			}
			this.EnsureBufferSpace(2);
			byte[] array3 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array3[num] = 16;
			byte[] array4 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array4[num] = (byte)value;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000701C File Offset: 0x0000521C
		public override void WriteInt16(string name, short value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 19;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(2);
				this.UNSAFE_WriteToBuffer_2_Int16(value);
				return;
			}
			this.EnsureBufferSpace(3);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 20;
			this.UNSAFE_WriteToBuffer_2_Int16(value);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000708C File Offset: 0x0000528C
		public override void WriteSingle(string name, float value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 31;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(4);
				this.UNSAFE_WriteToBuffer_4_Float32(value);
				return;
			}
			this.EnsureBufferSpace(5);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 32;
			this.UNSAFE_WriteToBuffer_4_Float32(value);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000070FC File Offset: 0x000052FC
		public override void WriteString(string name, string value)
		{
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 39;
				this.WriteStringFast(name);
			}
			else
			{
				this.EnsureBufferSpace(1);
				byte[] array2 = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = 40;
			}
			this.WriteStringFast(value);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007160 File Offset: 0x00005360
		public override void WriteUInt32(string name, uint value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 25;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(4);
				this.UNSAFE_WriteToBuffer_4_UInt32(value);
				return;
			}
			this.EnsureBufferSpace(5);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 26;
			this.UNSAFE_WriteToBuffer_4_UInt32(value);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000071D0 File Offset: 0x000053D0
		public override void WriteUInt64(string name, ulong value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 29;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(8);
				this.UNSAFE_WriteToBuffer_8_UInt64(value);
				return;
			}
			this.EnsureBufferSpace(9);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 30;
			this.UNSAFE_WriteToBuffer_8_UInt64(value);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007240 File Offset: 0x00005440
		public override void WriteUInt16(string name, ushort value)
		{
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 21;
				this.WriteStringFast(name);
				this.EnsureBufferSpace(2);
				this.UNSAFE_WriteToBuffer_2_UInt16(value);
				return;
			}
			this.EnsureBufferSpace(3);
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 22;
			this.UNSAFE_WriteToBuffer_2_UInt16(value);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000072AF File Offset: 0x000054AF
		public override void PrepareNewSerializationSession()
		{
			base.PrepareNewSerializationSession();
			this.types.Clear();
			this.bufferIndex = 0;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000072CC File Offset: 0x000054CC
		public override string GetDataDump()
		{
			if (!this.Stream.CanRead)
			{
				return "Binary data stream for writing cannot be read; cannot dump data.";
			}
			if (!this.Stream.CanSeek)
			{
				return "Binary data stream cannot seek; cannot dump data.";
			}
			this.FlushToStream();
			long position = this.Stream.Position;
			byte[] array = new byte[position];
			this.Stream.Position = 0L;
			this.Stream.Read(array, 0, (int)position);
			this.Stream.Position = position;
			return "Binary hex dump: " + ProperBitConverter.BytesToHexString(array, true);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007354 File Offset: 0x00005554
		[MethodImpl(256)]
		private void WriteType(Type type)
		{
			int num;
			if (type == null)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 46;
				return;
			}
			int count;
			if (this.types.TryGetValue(type, ref count))
			{
				this.EnsureBufferSpace(5);
				byte[] array2 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array2[num] = 48;
				this.UNSAFE_WriteToBuffer_4_Int32(count);
				return;
			}
			count = this.types.Count;
			this.types.Add(type, count);
			this.EnsureBufferSpace(5);
			byte[] array3 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array3[num] = 47;
			this.UNSAFE_WriteToBuffer_4_Int32(count);
			this.WriteStringFast(base.Context.Binder.BindToName(type, base.Context.Config.DebugContext));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007430 File Offset: 0x00005630
		private unsafe void WriteStringFast(string value)
		{
			bool flag = true;
			if (this.CompressStringsTo8BitWhenPossible)
			{
				flag = false;
				for (int i = 0; i < value.Length; i++)
				{
					if (value.get_Chars(i) > 'ÿ')
					{
						flag = true;
						break;
					}
				}
			}
			int num;
			if (flag)
			{
				num = value.Length * 2;
				if (this.TryEnsureBufferSpace(num + 5))
				{
					byte[] array = this.buffer;
					int num2 = this.bufferIndex;
					this.bufferIndex = num2 + 1;
					array[num2] = 1;
					this.UNSAFE_WriteToBuffer_4_Int32(value.Length);
					if (BitConverter.IsLittleEndian)
					{
						byte[] array2;
						byte* ptr;
						if ((array2 = this.buffer) == null || array2.Length == 0)
						{
							ptr = null;
						}
						else
						{
							ptr = &array2[0];
						}
						fixed (string text = value)
						{
							char* ptr2 = text;
							if (ptr2 != null)
							{
								ptr2 += RuntimeHelpers.OffsetToStringData / 2;
							}
							BinaryDataWriter.Struct256Bit* ptr3 = (BinaryDataWriter.Struct256Bit*)(ptr + this.bufferIndex);
							BinaryDataWriter.Struct256Bit* ptr4 = (BinaryDataWriter.Struct256Bit*)ptr2;
							byte* ptr5 = (byte*)(ptr3 + num / sizeof(BinaryDataWriter.Struct256Bit));
							while (ptr3 + 1 == (BinaryDataWriter.Struct256Bit*)ptr5)
							{
								BinaryDataWriter.Struct256Bit* ptr6 = ptr3;
								ptr3 = ptr6 + 1;
								ref BinaryDataWriter.Struct256Bit ptr7 = ref *ptr6;
								ptr6 = ptr4;
								ptr4 = ptr6 + 1;
								ptr7 = *ptr6;
							}
							char* ptr8 = (char*)ptr3;
							char* ptr9 = (char*)ptr4;
							while (ptr8 < (char*)ptr5)
							{
								*(ptr8++) = *(ptr9++);
							}
						}
						array2 = null;
					}
					else
					{
						byte[] array2;
						byte* ptr10;
						if ((array2 = this.buffer) == null || array2.Length == 0)
						{
							ptr10 = null;
						}
						else
						{
							ptr10 = &array2[0];
						}
						fixed (string text2 = value)
						{
							char* ptr11 = text2;
							if (ptr11 != null)
							{
								ptr11 += RuntimeHelpers.OffsetToStringData / 2;
							}
							byte* ptr12 = ptr10 + this.bufferIndex;
							byte* ptr13 = (byte*)ptr11;
							for (int j = 0; j < num; j += 2)
							{
								*ptr12 = ptr13[1];
								ptr12[1] = *ptr13;
								ptr13 += 2;
								ptr12 += 2;
							}
						}
						array2 = null;
					}
					this.bufferIndex += num;
					return;
				}
				this.FlushToStream();
				this.Stream.WriteByte(1);
				ProperBitConverter.GetBytes(this.small_buffer, 0, value.Length);
				this.Stream.Write(this.small_buffer, 0, 4);
				using (Buffer<byte> buffer = Buffer<byte>.Claim(num))
				{
					byte[] array3 = buffer.Array;
					UnsafeUtilities.StringToBytes(array3, value, true);
					this.Stream.Write(array3, 0, num);
					return;
				}
			}
			num = value.Length;
			if (this.TryEnsureBufferSpace(num + 5))
			{
				byte[] array4 = this.buffer;
				int num2 = this.bufferIndex;
				this.bufferIndex = num2 + 1;
				array4[num2] = 0;
				this.UNSAFE_WriteToBuffer_4_Int32(value.Length);
				for (int k = 0; k < num; k++)
				{
					byte[] array5 = this.buffer;
					num2 = this.bufferIndex;
					this.bufferIndex = num2 + 1;
					array5[num2] = (byte)value.get_Chars(k);
				}
				return;
			}
			this.FlushToStream();
			this.Stream.WriteByte(0);
			ProperBitConverter.GetBytes(this.small_buffer, 0, value.Length);
			this.Stream.Write(this.small_buffer, 0, 4);
			using (Buffer<byte> buffer2 = Buffer<byte>.Claim(value.Length))
			{
				byte[] array6 = buffer2.Array;
				for (int l = 0; l < value.Length; l++)
				{
					array6[l] = (byte)value.get_Chars(l);
				}
				this.Stream.Write(array6, 0, value.Length);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000777C File Offset: 0x0000597C
		public override void FlushToStream()
		{
			if (this.bufferIndex > 0)
			{
				this.Stream.Write(this.buffer, 0, this.bufferIndex);
				this.bufferIndex = 0;
			}
			base.FlushToStream();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000077AC File Offset: 0x000059AC
		[MethodImpl(256)]
		private unsafe void UNSAFE_WriteToBuffer_2_Char(char value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				*(short*)(ptr + this.bufferIndex) = (short)value;
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 1;
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 2;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007818 File Offset: 0x00005A18
		[MethodImpl(256)]
		private unsafe void UNSAFE_WriteToBuffer_2_Int16(short value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				*(short*)(ptr + this.bufferIndex) = value;
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 1;
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 2;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007884 File Offset: 0x00005A84
		[MethodImpl(256)]
		private unsafe void UNSAFE_WriteToBuffer_2_UInt16(ushort value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				*(short*)(ptr + this.bufferIndex) = (short)value;
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 1;
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 2;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000078F0 File Offset: 0x00005AF0
		[MethodImpl(256)]
		private unsafe void UNSAFE_WriteToBuffer_4_Int32(int value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				*(int*)(ptr + this.bufferIndex) = value;
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 3;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 4;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007974 File Offset: 0x00005B74
		[MethodImpl(256)]
		private unsafe void UNSAFE_WriteToBuffer_4_UInt32(uint value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				*(int*)(ptr + this.bufferIndex) = (int)value;
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 3;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 4;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000079F8 File Offset: 0x00005BF8
		[MethodImpl(256)]
		private unsafe void UNSAFE_WriteToBuffer_4_Float32(float value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				if (ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites)
				{
					*(float*)(ptr + this.bufferIndex) = value;
				}
				else
				{
					byte* ptr2 = (byte*)(&value);
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr3++) = *(ptr2++);
					*(ptr3++) = *(ptr2++);
					*(ptr3++) = *(ptr2++);
					*ptr3 = *ptr2;
				}
			}
			else
			{
				byte* ptr4 = ptr + this.bufferIndex;
				byte* ptr5 = (byte*)(&value) + 3;
				*(ptr4++) = *(ptr5--);
				*(ptr4++) = *(ptr5--);
				*(ptr4++) = *(ptr5--);
				*ptr4 = *ptr5;
			}
			array = null;
			this.bufferIndex += 4;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007ACC File Offset: 0x00005CCC
		[MethodImpl(8)]
		private unsafe void UNSAFE_WriteToBuffer_8_Int64(long value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				if (ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites)
				{
					*(long*)(ptr + this.bufferIndex) = value;
				}
				else
				{
					BinaryDataWriter.SixtyFourBitValueToByteUnion sixtyFourBitValueToByteUnion = default(BinaryDataWriter.SixtyFourBitValueToByteUnion);
					sixtyFourBitValueToByteUnion.longValue = value;
					this.buffer[this.bufferIndex] = sixtyFourBitValueToByteUnion.b0;
					this.buffer[this.bufferIndex + 1] = sixtyFourBitValueToByteUnion.b1;
					this.buffer[this.bufferIndex + 2] = sixtyFourBitValueToByteUnion.b2;
					this.buffer[this.bufferIndex + 3] = sixtyFourBitValueToByteUnion.b3;
					this.buffer[this.bufferIndex + 4] = sixtyFourBitValueToByteUnion.b4;
					this.buffer[this.bufferIndex + 5] = sixtyFourBitValueToByteUnion.b5;
					this.buffer[this.bufferIndex + 6] = sixtyFourBitValueToByteUnion.b6;
					this.buffer[this.bufferIndex + 7] = sixtyFourBitValueToByteUnion.b7;
				}
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 7;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 8;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007C58 File Offset: 0x00005E58
		[MethodImpl(8)]
		private unsafe void UNSAFE_WriteToBuffer_8_UInt64(ulong value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				if (ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites)
				{
					*(long*)(ptr + this.bufferIndex) = (long)value;
				}
				else
				{
					BinaryDataWriter.SixtyFourBitValueToByteUnion sixtyFourBitValueToByteUnion = default(BinaryDataWriter.SixtyFourBitValueToByteUnion);
					sixtyFourBitValueToByteUnion.ulongValue = value;
					this.buffer[this.bufferIndex] = sixtyFourBitValueToByteUnion.b0;
					this.buffer[this.bufferIndex + 1] = sixtyFourBitValueToByteUnion.b1;
					this.buffer[this.bufferIndex + 2] = sixtyFourBitValueToByteUnion.b2;
					this.buffer[this.bufferIndex + 3] = sixtyFourBitValueToByteUnion.b3;
					this.buffer[this.bufferIndex + 4] = sixtyFourBitValueToByteUnion.b4;
					this.buffer[this.bufferIndex + 5] = sixtyFourBitValueToByteUnion.b5;
					this.buffer[this.bufferIndex + 6] = sixtyFourBitValueToByteUnion.b6;
					this.buffer[this.bufferIndex + 7] = sixtyFourBitValueToByteUnion.b7;
				}
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 7;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 8;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007DE4 File Offset: 0x00005FE4
		[MethodImpl(8)]
		private unsafe void UNSAFE_WriteToBuffer_8_Float64(double value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				if (ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites)
				{
					*(double*)(ptr + this.bufferIndex) = value;
				}
				else
				{
					BinaryDataWriter.SixtyFourBitValueToByteUnion sixtyFourBitValueToByteUnion = default(BinaryDataWriter.SixtyFourBitValueToByteUnion);
					sixtyFourBitValueToByteUnion.doubleValue = value;
					this.buffer[this.bufferIndex] = sixtyFourBitValueToByteUnion.b0;
					this.buffer[this.bufferIndex + 1] = sixtyFourBitValueToByteUnion.b1;
					this.buffer[this.bufferIndex + 2] = sixtyFourBitValueToByteUnion.b2;
					this.buffer[this.bufferIndex + 3] = sixtyFourBitValueToByteUnion.b3;
					this.buffer[this.bufferIndex + 4] = sixtyFourBitValueToByteUnion.b4;
					this.buffer[this.bufferIndex + 5] = sixtyFourBitValueToByteUnion.b5;
					this.buffer[this.bufferIndex + 6] = sixtyFourBitValueToByteUnion.b6;
					this.buffer[this.bufferIndex + 7] = sixtyFourBitValueToByteUnion.b7;
				}
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 7;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 8;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007F70 File Offset: 0x00006170
		[MethodImpl(8)]
		private unsafe void UNSAFE_WriteToBuffer_16_Decimal(decimal value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				if (ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites)
				{
					*(decimal*)(ptr + this.bufferIndex) = value;
				}
				else
				{
					BinaryDataWriter.OneTwentyEightBitValueToByteUnion oneTwentyEightBitValueToByteUnion = default(BinaryDataWriter.OneTwentyEightBitValueToByteUnion);
					oneTwentyEightBitValueToByteUnion.decimalValue = value;
					this.buffer[this.bufferIndex] = oneTwentyEightBitValueToByteUnion.b0;
					this.buffer[this.bufferIndex + 1] = oneTwentyEightBitValueToByteUnion.b1;
					this.buffer[this.bufferIndex + 2] = oneTwentyEightBitValueToByteUnion.b2;
					this.buffer[this.bufferIndex + 3] = oneTwentyEightBitValueToByteUnion.b3;
					this.buffer[this.bufferIndex + 4] = oneTwentyEightBitValueToByteUnion.b4;
					this.buffer[this.bufferIndex + 5] = oneTwentyEightBitValueToByteUnion.b5;
					this.buffer[this.bufferIndex + 6] = oneTwentyEightBitValueToByteUnion.b6;
					this.buffer[this.bufferIndex + 7] = oneTwentyEightBitValueToByteUnion.b7;
					this.buffer[this.bufferIndex + 8] = oneTwentyEightBitValueToByteUnion.b8;
					this.buffer[this.bufferIndex + 9] = oneTwentyEightBitValueToByteUnion.b9;
					this.buffer[this.bufferIndex + 10] = oneTwentyEightBitValueToByteUnion.b10;
					this.buffer[this.bufferIndex + 11] = oneTwentyEightBitValueToByteUnion.b11;
					this.buffer[this.bufferIndex + 12] = oneTwentyEightBitValueToByteUnion.b12;
					this.buffer[this.bufferIndex + 13] = oneTwentyEightBitValueToByteUnion.b13;
					this.buffer[this.bufferIndex + 14] = oneTwentyEightBitValueToByteUnion.b14;
					this.buffer[this.bufferIndex + 15] = oneTwentyEightBitValueToByteUnion.b15;
				}
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value) + 15;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 16;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00008224 File Offset: 0x00006424
		[MethodImpl(8)]
		private unsafe void UNSAFE_WriteToBuffer_16_Guid(Guid value)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			if (BitConverter.IsLittleEndian)
			{
				if (ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites)
				{
					*(Guid*)(ptr + this.bufferIndex) = value;
				}
				else
				{
					BinaryDataWriter.OneTwentyEightBitValueToByteUnion oneTwentyEightBitValueToByteUnion = default(BinaryDataWriter.OneTwentyEightBitValueToByteUnion);
					oneTwentyEightBitValueToByteUnion.guidValue = value;
					this.buffer[this.bufferIndex] = oneTwentyEightBitValueToByteUnion.b0;
					this.buffer[this.bufferIndex + 1] = oneTwentyEightBitValueToByteUnion.b1;
					this.buffer[this.bufferIndex + 2] = oneTwentyEightBitValueToByteUnion.b2;
					this.buffer[this.bufferIndex + 3] = oneTwentyEightBitValueToByteUnion.b3;
					this.buffer[this.bufferIndex + 4] = oneTwentyEightBitValueToByteUnion.b4;
					this.buffer[this.bufferIndex + 5] = oneTwentyEightBitValueToByteUnion.b5;
					this.buffer[this.bufferIndex + 6] = oneTwentyEightBitValueToByteUnion.b6;
					this.buffer[this.bufferIndex + 7] = oneTwentyEightBitValueToByteUnion.b7;
					this.buffer[this.bufferIndex + 8] = oneTwentyEightBitValueToByteUnion.b8;
					this.buffer[this.bufferIndex + 9] = oneTwentyEightBitValueToByteUnion.b9;
					this.buffer[this.bufferIndex + 10] = oneTwentyEightBitValueToByteUnion.b10;
					this.buffer[this.bufferIndex + 11] = oneTwentyEightBitValueToByteUnion.b11;
					this.buffer[this.bufferIndex + 12] = oneTwentyEightBitValueToByteUnion.b12;
					this.buffer[this.bufferIndex + 13] = oneTwentyEightBitValueToByteUnion.b13;
					this.buffer[this.bufferIndex + 14] = oneTwentyEightBitValueToByteUnion.b14;
					this.buffer[this.bufferIndex + 15] = oneTwentyEightBitValueToByteUnion.b15;
				}
			}
			else
			{
				byte* ptr2 = ptr + this.bufferIndex;
				byte* ptr3 = (byte*)(&value);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *(ptr3++);
				*(ptr2++) = *ptr3;
				ptr3 += 6;
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*(ptr2++) = *(ptr3--);
				*ptr2 = *ptr3;
			}
			array = null;
			this.bufferIndex += 16;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000084D4 File Offset: 0x000066D4
		[MethodImpl(256)]
		private void EnsureBufferSpace(int space)
		{
			int num = this.buffer.Length;
			if (space > num)
			{
				throw new Exception("Insufficient buffer capacity");
			}
			if (this.bufferIndex + space > num)
			{
				this.FlushToStream();
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000850C File Offset: 0x0000670C
		[MethodImpl(256)]
		private bool TryEnsureBufferSpace(int space)
		{
			int num = this.buffer.Length;
			if (space > num)
			{
				return false;
			}
			if (this.bufferIndex + space > num)
			{
				this.FlushToStream();
			}
			return true;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000853C File Offset: 0x0000673C
		// Note: this type is marked as 'beforefieldinit'.
		static BinaryDataWriter()
		{
			Dictionary<Type, Delegate> dictionary = new Dictionary<Type, Delegate>(FastTypeComparer.Instance);
			dictionary.Add(typeof(char), delegate(byte[] b, int i, char v)
			{
				ProperBitConverter.GetBytes(b, i, (ushort)v);
			});
			dictionary.Add(typeof(byte), delegate(byte[] b, int i, byte v)
			{
				b[i] = v;
			});
			dictionary.Add(typeof(sbyte), delegate(byte[] b, int i, sbyte v)
			{
				b[i] = (byte)v;
			});
			dictionary.Add(typeof(bool), delegate(byte[] b, int i, bool v)
			{
				b[i] = ((v > false) ? 1 : 0);
			});
			dictionary.Add(typeof(short), new Action<byte[], int, short>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(int), new Action<byte[], int, int>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(long), new Action<byte[], int, long>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(ushort), new Action<byte[], int, ushort>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(uint), new Action<byte[], int, uint>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(ulong), new Action<byte[], int, ulong>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(decimal), new Action<byte[], int, decimal>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(float), new Action<byte[], int, float>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(double), new Action<byte[], int, double>(ProperBitConverter.GetBytes));
			dictionary.Add(typeof(Guid), new Action<byte[], int, Guid>(ProperBitConverter.GetBytes));
			BinaryDataWriter.PrimitiveGetBytesMethods = dictionary;
			Dictionary<Type, int> dictionary2 = new Dictionary<Type, int>(FastTypeComparer.Instance);
			dictionary2.Add(typeof(char), 2);
			dictionary2.Add(typeof(byte), 1);
			dictionary2.Add(typeof(sbyte), 1);
			dictionary2.Add(typeof(bool), 1);
			dictionary2.Add(typeof(short), 2);
			dictionary2.Add(typeof(int), 4);
			dictionary2.Add(typeof(long), 8);
			dictionary2.Add(typeof(ushort), 2);
			dictionary2.Add(typeof(uint), 4);
			dictionary2.Add(typeof(ulong), 8);
			dictionary2.Add(typeof(decimal), 16);
			dictionary2.Add(typeof(float), 4);
			dictionary2.Add(typeof(double), 8);
			dictionary2.Add(typeof(Guid), 16);
			BinaryDataWriter.PrimitiveSizes = dictionary2;
			Dictionary<Type, Action<BinaryDataWriter, object>> dictionary3 = new Dictionary<Type, Action<BinaryDataWriter, object>>(FastTypeComparer.Instance);
			dictionary3.Add(typeof(char), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_char));
			dictionary3.Add(typeof(sbyte), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_sbyte));
			dictionary3.Add(typeof(short), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_short));
			dictionary3.Add(typeof(int), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_int));
			dictionary3.Add(typeof(long), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_long));
			dictionary3.Add(typeof(byte), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_byte));
			dictionary3.Add(typeof(ushort), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_ushort));
			dictionary3.Add(typeof(uint), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_uint));
			dictionary3.Add(typeof(ulong), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_ulong));
			dictionary3.Add(typeof(decimal), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_decimal));
			dictionary3.Add(typeof(bool), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_bool));
			dictionary3.Add(typeof(float), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_float));
			dictionary3.Add(typeof(double), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_double));
			dictionary3.Add(typeof(Guid), new Action<BinaryDataWriter, object>(BinaryDataWriter.WritePrimitiveArray_Guid));
			BinaryDataWriter.PrimitiveArrayWriters = dictionary3;
		}

		// Token: 0x04000018 RID: 24
		private static readonly Dictionary<Type, Delegate> PrimitiveGetBytesMethods;

		// Token: 0x04000019 RID: 25
		private static readonly Dictionary<Type, int> PrimitiveSizes;

		// Token: 0x0400001A RID: 26
		private readonly byte[] small_buffer = new byte[16];

		// Token: 0x0400001B RID: 27
		private readonly byte[] buffer = new byte[102400];

		// Token: 0x0400001C RID: 28
		private int bufferIndex;

		// Token: 0x0400001D RID: 29
		private readonly Dictionary<Type, int> types = new Dictionary<Type, int>(16, FastTypeComparer.Instance);

		// Token: 0x0400001E RID: 30
		public bool CompressStringsTo8BitWhenPossible;

		// Token: 0x0400001F RID: 31
		private static readonly Dictionary<Type, Action<BinaryDataWriter, object>> PrimitiveArrayWriters;

		// Token: 0x020000DC RID: 220
		private struct Struct256Bit
		{
			// Token: 0x04000232 RID: 562
			public decimal d1;

			// Token: 0x04000233 RID: 563
			public decimal d2;
		}

		// Token: 0x020000DD RID: 221
		[StructLayout(2, Size = 8)]
		private struct SixtyFourBitValueToByteUnion
		{
			// Token: 0x04000234 RID: 564
			[FieldOffset(0)]
			public byte b0;

			// Token: 0x04000235 RID: 565
			[FieldOffset(1)]
			public byte b1;

			// Token: 0x04000236 RID: 566
			[FieldOffset(2)]
			public byte b2;

			// Token: 0x04000237 RID: 567
			[FieldOffset(3)]
			public byte b3;

			// Token: 0x04000238 RID: 568
			[FieldOffset(4)]
			public byte b4;

			// Token: 0x04000239 RID: 569
			[FieldOffset(5)]
			public byte b5;

			// Token: 0x0400023A RID: 570
			[FieldOffset(6)]
			public byte b6;

			// Token: 0x0400023B RID: 571
			[FieldOffset(7)]
			public byte b7;

			// Token: 0x0400023C RID: 572
			[FieldOffset(0)]
			public double doubleValue;

			// Token: 0x0400023D RID: 573
			[FieldOffset(0)]
			public ulong ulongValue;

			// Token: 0x0400023E RID: 574
			[FieldOffset(0)]
			public long longValue;
		}

		// Token: 0x020000DE RID: 222
		[StructLayout(2, Size = 16)]
		private struct OneTwentyEightBitValueToByteUnion
		{
			// Token: 0x0400023F RID: 575
			[FieldOffset(0)]
			public byte b0;

			// Token: 0x04000240 RID: 576
			[FieldOffset(1)]
			public byte b1;

			// Token: 0x04000241 RID: 577
			[FieldOffset(2)]
			public byte b2;

			// Token: 0x04000242 RID: 578
			[FieldOffset(3)]
			public byte b3;

			// Token: 0x04000243 RID: 579
			[FieldOffset(4)]
			public byte b4;

			// Token: 0x04000244 RID: 580
			[FieldOffset(5)]
			public byte b5;

			// Token: 0x04000245 RID: 581
			[FieldOffset(6)]
			public byte b6;

			// Token: 0x04000246 RID: 582
			[FieldOffset(7)]
			public byte b7;

			// Token: 0x04000247 RID: 583
			[FieldOffset(8)]
			public byte b8;

			// Token: 0x04000248 RID: 584
			[FieldOffset(9)]
			public byte b9;

			// Token: 0x04000249 RID: 585
			[FieldOffset(10)]
			public byte b10;

			// Token: 0x0400024A RID: 586
			[FieldOffset(11)]
			public byte b11;

			// Token: 0x0400024B RID: 587
			[FieldOffset(12)]
			public byte b12;

			// Token: 0x0400024C RID: 588
			[FieldOffset(13)]
			public byte b13;

			// Token: 0x0400024D RID: 589
			[FieldOffset(14)]
			public byte b14;

			// Token: 0x0400024E RID: 590
			[FieldOffset(15)]
			public byte b15;

			// Token: 0x0400024F RID: 591
			[FieldOffset(0)]
			public Guid guidValue;

			// Token: 0x04000250 RID: 592
			[FieldOffset(0)]
			public decimal decimalValue;
		}
	}
}
