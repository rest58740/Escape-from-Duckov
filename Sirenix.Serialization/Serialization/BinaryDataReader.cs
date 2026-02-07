using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sirenix.Serialization.Utilities.Unsafe;

namespace Sirenix.Serialization
{
	// Token: 0x0200000C RID: 12
	public class BinaryDataReader : BaseDataReader
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00002865 File Offset: 0x00000A65
		public BinaryDataReader() : base(null, null)
		{
			this.internalBufferBackup = this.buffer;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002898 File Offset: 0x00000A98
		public BinaryDataReader(Stream stream, DeserializationContext context) : base(stream, context)
		{
			this.internalBufferBackup = this.buffer;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000021B8 File Offset: 0x000003B8
		public override void Dispose()
		{
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000028CC File Offset: 0x00000ACC
		public override EntryType PeekEntry(out string name)
		{
			if (this.peekedEntryType != null)
			{
				name = this.peekedEntryName;
				return this.peekedEntryType.Value;
			}
			BinaryEntryType binaryEntryType;
			if (!this.HasBufferData(1))
			{
				binaryEntryType = BinaryEntryType.EndOfStream;
			}
			else
			{
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				binaryEntryType = array[num];
			}
			this.peekedBinaryEntryType = binaryEntryType;
			switch (this.peekedBinaryEntryType)
			{
			case BinaryEntryType.NamedStartOfReferenceNode:
			case BinaryEntryType.NamedStartOfStructNode:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.StartOfNode);
				goto IL_5A4;
			case BinaryEntryType.UnnamedStartOfReferenceNode:
			case BinaryEntryType.UnnamedStartOfStructNode:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.StartOfNode);
				goto IL_5A4;
			case BinaryEntryType.EndOfNode:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.EndOfNode);
				goto IL_5A4;
			case BinaryEntryType.StartOfArray:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.StartOfArray);
				goto IL_5A4;
			case BinaryEntryType.EndOfArray:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.EndOfArray);
				goto IL_5A4;
			case BinaryEntryType.PrimitiveArray:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.PrimitiveArray);
				goto IL_5A4;
			case BinaryEntryType.NamedInternalReference:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.InternalReference);
				goto IL_5A4;
			case BinaryEntryType.UnnamedInternalReference:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.InternalReference);
				goto IL_5A4;
			case BinaryEntryType.NamedExternalReferenceByIndex:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.ExternalReferenceByIndex);
				goto IL_5A4;
			case BinaryEntryType.UnnamedExternalReferenceByIndex:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.ExternalReferenceByIndex);
				goto IL_5A4;
			case BinaryEntryType.NamedExternalReferenceByGuid:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.ExternalReferenceByGuid);
				goto IL_5A4;
			case BinaryEntryType.UnnamedExternalReferenceByGuid:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.ExternalReferenceByGuid);
				goto IL_5A4;
			case BinaryEntryType.NamedSByte:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedSByte:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedByte:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedByte:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedShort:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedShort:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedUShort:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedUShort:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedInt:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedInt:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedUInt:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedUInt:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedLong:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedLong:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedULong:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.UnnamedULong:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Integer);
				goto IL_5A4;
			case BinaryEntryType.NamedFloat:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.FloatingPoint);
				goto IL_5A4;
			case BinaryEntryType.UnnamedFloat:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.FloatingPoint);
				goto IL_5A4;
			case BinaryEntryType.NamedDouble:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.FloatingPoint);
				goto IL_5A4;
			case BinaryEntryType.UnnamedDouble:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.FloatingPoint);
				goto IL_5A4;
			case BinaryEntryType.NamedDecimal:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.FloatingPoint);
				goto IL_5A4;
			case BinaryEntryType.UnnamedDecimal:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.FloatingPoint);
				goto IL_5A4;
			case BinaryEntryType.NamedChar:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.String);
				goto IL_5A4;
			case BinaryEntryType.UnnamedChar:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.String);
				goto IL_5A4;
			case BinaryEntryType.NamedString:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.String);
				goto IL_5A4;
			case BinaryEntryType.UnnamedString:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.String);
				goto IL_5A4;
			case BinaryEntryType.NamedGuid:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Guid);
				goto IL_5A4;
			case BinaryEntryType.UnnamedGuid:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Guid);
				goto IL_5A4;
			case BinaryEntryType.NamedBoolean:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Boolean);
				goto IL_5A4;
			case BinaryEntryType.UnnamedBoolean:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Boolean);
				goto IL_5A4;
			case BinaryEntryType.NamedNull:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.Null);
				goto IL_5A4;
			case BinaryEntryType.UnnamedNull:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.Null);
				goto IL_5A4;
			case BinaryEntryType.TypeName:
			case BinaryEntryType.TypeID:
				this.peekedBinaryEntryType = BinaryEntryType.Invalid;
				this.peekedEntryType = new EntryType?(EntryType.Invalid);
				throw new InvalidOperationException("Invalid binary data stream: BinaryEntryType.TypeName and BinaryEntryType.TypeID must never be peeked by the binary reader.");
			case BinaryEntryType.EndOfStream:
				name = null;
				this.peekedEntryName = null;
				this.peekedEntryType = new EntryType?(EntryType.EndOfStream);
				goto IL_5A4;
			case BinaryEntryType.NamedExternalReferenceByString:
				name = this.ReadStringValue();
				this.peekedEntryType = new EntryType?(EntryType.ExternalReferenceByString);
				goto IL_5A4;
			case BinaryEntryType.UnnamedExternalReferenceByString:
				name = null;
				this.peekedEntryType = new EntryType?(EntryType.ExternalReferenceByString);
				goto IL_5A4;
			}
			name = null;
			this.peekedBinaryEntryType = BinaryEntryType.Invalid;
			this.peekedEntryType = new EntryType?(EntryType.Invalid);
			string text = "Invalid binary data stream: could not parse peeked BinaryEntryType byte '";
			byte b = (byte)this.peekedBinaryEntryType;
			throw new InvalidOperationException(text + b.ToString() + "' into a known entry type.");
			IL_5A4:
			this.peekedEntryName = name;
			return this.peekedEntryType.Value;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002E90 File Offset: 0x00001090
		public override bool EnterArray(out long length)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.StartOfArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				this.SkipEntry();
				length = 0L;
				return false;
			}
			base.PushArray();
			this.MarkEntryContentConsumed();
			if (!this.UNSAFE_Read_8_Int64(out length))
			{
				return false;
			}
			if (length < 0L)
			{
				length = 0L;
				base.Context.Config.DebugContext.LogError("Invalid array length: " + length.ToString() + ".");
				return false;
			}
			return true;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002F2C File Offset: 0x0000112C
		public override bool EnterNode(out Type type)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedStartOfReferenceNode || this.peekedBinaryEntryType == BinaryEntryType.UnnamedStartOfReferenceNode)
			{
				this.MarkEntryContentConsumed();
				type = this.ReadTypeEntry();
				int id;
				if (!this.UNSAFE_Read_4_Int32(out id))
				{
					type = null;
					return false;
				}
				base.PushNode(this.peekedEntryName, id, type);
				return true;
			}
			else
			{
				if (this.peekedBinaryEntryType == BinaryEntryType.NamedStartOfStructNode || this.peekedBinaryEntryType == BinaryEntryType.UnnamedStartOfStructNode)
				{
					type = this.ReadTypeEntry();
					base.PushNode(this.peekedEntryName, -1, type);
					this.MarkEntryContentConsumed();
					return true;
				}
				this.SkipEntry();
				type = null;
				return false;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002FCC File Offset: 0x000011CC
		public override bool ExitArray()
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			while (this.peekedBinaryEntryType != BinaryEntryType.EndOfArray && this.peekedBinaryEntryType != BinaryEntryType.EndOfStream)
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.EndOfNode;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					base.Context.Config.DebugContext.LogError("Data layout mismatch; skipping past node boundary when exiting array.");
					this.MarkEntryContentConsumed();
				}
				this.SkipEntry();
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.EndOfArray)
			{
				this.MarkEntryContentConsumed();
				base.PopArray();
				return true;
			}
			return false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003060 File Offset: 0x00001260
		public override bool ExitNode()
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			while (this.peekedBinaryEntryType != BinaryEntryType.EndOfNode && this.peekedBinaryEntryType != BinaryEntryType.EndOfStream)
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.EndOfArray;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					base.Context.Config.DebugContext.LogError("Data layout mismatch; skipping past array boundary when exiting node.");
					this.MarkEntryContentConsumed();
				}
				this.SkipEntry();
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.EndOfNode)
			{
				this.MarkEntryContentConsumed();
				base.PopNode(base.CurrentNodeName);
				return true;
			}
			return false;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000030FC File Offset: 0x000012FC
		public unsafe override bool ReadPrimitiveArray<T>(out T[] array)
		{
			if (!FormatterUtilities.IsPrimitiveArrayType(typeof(T)))
			{
				throw new ArgumentException("Type " + typeof(T).Name + " is not a valid primitive array type.");
			}
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.PrimitiveArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				this.SkipEntry();
				array = null;
				return false;
			}
			this.MarkEntryContentConsumed();
			int num;
			int num2;
			if (!this.UNSAFE_Read_4_Int32(out num) || !this.UNSAFE_Read_4_Int32(out num2))
			{
				array = null;
				return false;
			}
			int num3 = num * num2;
			if (!this.HasBufferData(num3))
			{
				this.bufferIndex = this.bufferEnd;
				array = null;
				return false;
			}
			if (typeof(T) == typeof(byte))
			{
				byte[] array2 = new byte[num3];
				Buffer.BlockCopy(this.buffer, this.bufferIndex, array2, 0, num3);
				array = (T[])array2;
				this.bufferIndex += num3;
				return true;
			}
			array = new T[num];
			if (BitConverter.IsLittleEndian)
			{
				GCHandle gchandle = GCHandle.Alloc(array, 3);
				try
				{
					try
					{
						byte[] array3;
						byte* ptr;
						if ((array3 = this.buffer) == null || array3.Length == 0)
						{
							ptr = null;
						}
						else
						{
							ptr = &array3[0];
						}
						void* from = (void*)(ptr + this.bufferIndex);
						void* to = gchandle.AddrOfPinnedObject().ToPointer();
						UnsafeUtilities.MemoryCopy(from, to, num3);
						goto IL_1BE;
					}
					finally
					{
						byte[] array3 = null;
					}
				}
				finally
				{
					gchandle.Free();
				}
			}
			Func<byte[], int, T> func = (Func<byte[], int, T>)BinaryDataReader.PrimitiveFromByteMethods[typeof(T)];
			for (int i = 0; i < num; i++)
			{
				array[i] = func.Invoke(this.buffer, this.bufferIndex + i * num2);
			}
			IL_1BE:
			this.bufferIndex += num3;
			return true;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003300 File Offset: 0x00001500
		public override bool ReadBoolean(out bool value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Boolean;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				this.SkipEntry();
				value = false;
				return false;
			}
			this.MarkEntryContentConsumed();
			if (this.HasBufferData(1))
			{
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				value = (array[num] == 1);
				return true;
			}
			value = false;
			return false;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003380 File Offset: 0x00001580
		public override bool ReadSByte(out sbyte value)
		{
			long num;
			if (this.ReadInt64(out num))
			{
				try
				{
					value = checked((sbyte)num);
				}
				catch (OverflowException)
				{
					value = 0;
				}
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000033BC File Offset: 0x000015BC
		public override bool ReadByte(out byte value)
		{
			ulong num;
			if (this.ReadUInt64(out num))
			{
				try
				{
					value = checked((byte)num);
				}
				catch (OverflowException)
				{
					value = 0;
				}
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000033F8 File Offset: 0x000015F8
		public override bool ReadInt16(out short value)
		{
			long num;
			if (this.ReadInt64(out num))
			{
				try
				{
					value = checked((short)num);
				}
				catch (OverflowException)
				{
					value = 0;
				}
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003434 File Offset: 0x00001634
		public override bool ReadUInt16(out ushort value)
		{
			ulong num;
			if (this.ReadUInt64(out num))
			{
				try
				{
					value = checked((ushort)num);
				}
				catch (OverflowException)
				{
					value = 0;
				}
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003470 File Offset: 0x00001670
		public override bool ReadInt32(out int value)
		{
			long num;
			if (this.ReadInt64(out num))
			{
				try
				{
					value = checked((int)num);
				}
				catch (OverflowException)
				{
					value = 0;
				}
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000034AC File Offset: 0x000016AC
		public override bool ReadUInt32(out uint value)
		{
			ulong num;
			if (this.ReadUInt64(out num))
			{
				try
				{
					value = checked((uint)num);
				}
				catch (OverflowException)
				{
					value = 0U;
				}
				return true;
			}
			value = 0U;
			return false;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000034E8 File Offset: 0x000016E8
		public override bool ReadInt64(out long value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Integer;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					switch (this.peekedBinaryEntryType)
					{
					case BinaryEntryType.NamedSByte:
					case BinaryEntryType.UnnamedSByte:
					{
						sbyte b;
						if (!this.UNSAFE_Read_1_SByte(out b))
						{
							value = 0L;
							return false;
						}
						value = (long)b;
						break;
					}
					case BinaryEntryType.NamedByte:
					case BinaryEntryType.UnnamedByte:
					{
						byte b2;
						if (!this.UNSAFE_Read_1_Byte(out b2))
						{
							value = 0L;
							return false;
						}
						value = (long)((ulong)b2);
						break;
					}
					case BinaryEntryType.NamedShort:
					case BinaryEntryType.UnnamedShort:
					{
						short num;
						if (!this.UNSAFE_Read_2_Int16(out num))
						{
							value = 0L;
							return false;
						}
						value = (long)num;
						break;
					}
					case BinaryEntryType.NamedUShort:
					case BinaryEntryType.UnnamedUShort:
					{
						ushort num2;
						if (!this.UNSAFE_Read_2_UInt16(out num2))
						{
							value = 0L;
							return false;
						}
						value = (long)((ulong)num2);
						break;
					}
					case BinaryEntryType.NamedInt:
					case BinaryEntryType.UnnamedInt:
					{
						int num3;
						if (!this.UNSAFE_Read_4_Int32(out num3))
						{
							value = 0L;
							return false;
						}
						value = (long)num3;
						break;
					}
					case BinaryEntryType.NamedUInt:
					case BinaryEntryType.UnnamedUInt:
					{
						uint num4;
						if (!this.UNSAFE_Read_4_UInt32(out num4))
						{
							value = 0L;
							return false;
						}
						value = (long)((ulong)num4);
						break;
					}
					case BinaryEntryType.NamedLong:
					case BinaryEntryType.UnnamedLong:
						if (!this.UNSAFE_Read_8_Int64(out value))
						{
							return false;
						}
						break;
					case BinaryEntryType.NamedULong:
					case BinaryEntryType.UnnamedULong:
					{
						ulong num5;
						if (!this.UNSAFE_Read_8_UInt64(out num5))
						{
							value = 0L;
							return false;
						}
						if (num5 > 9223372036854775807UL)
						{
							value = 0L;
							return false;
						}
						value = (long)num5;
						break;
					}
					default:
						throw new InvalidOperationException();
					}
					return true;
				}
				finally
				{
					this.MarkEntryContentConsumed();
				}
			}
			this.SkipEntry();
			value = 0L;
			return false;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000036B0 File Offset: 0x000018B0
		public override bool ReadUInt64(out ulong value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Integer;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					switch (this.peekedBinaryEntryType)
					{
					case BinaryEntryType.NamedSByte:
					case BinaryEntryType.UnnamedSByte:
					case BinaryEntryType.NamedByte:
					case BinaryEntryType.UnnamedByte:
					{
						byte b;
						if (!this.UNSAFE_Read_1_Byte(out b))
						{
							value = 0UL;
							return false;
						}
						value = (ulong)b;
						break;
					}
					case BinaryEntryType.NamedShort:
					case BinaryEntryType.UnnamedShort:
					{
						short num;
						if (!this.UNSAFE_Read_2_Int16(out num))
						{
							value = 0UL;
							return false;
						}
						if (num < 0)
						{
							value = 0UL;
							return false;
						}
						value = (ulong)((long)num);
						break;
					}
					case BinaryEntryType.NamedUShort:
					case BinaryEntryType.UnnamedUShort:
					{
						ushort num2;
						if (!this.UNSAFE_Read_2_UInt16(out num2))
						{
							value = 0UL;
							return false;
						}
						value = (ulong)num2;
						break;
					}
					case BinaryEntryType.NamedInt:
					case BinaryEntryType.UnnamedInt:
					{
						int num3;
						if (!this.UNSAFE_Read_4_Int32(out num3))
						{
							value = 0UL;
							return false;
						}
						if (num3 < 0)
						{
							value = 0UL;
							return false;
						}
						value = (ulong)((long)num3);
						break;
					}
					case BinaryEntryType.NamedUInt:
					case BinaryEntryType.UnnamedUInt:
					{
						uint num4;
						if (!this.UNSAFE_Read_4_UInt32(out num4))
						{
							value = 0UL;
							return false;
						}
						value = (ulong)num4;
						break;
					}
					case BinaryEntryType.NamedLong:
					case BinaryEntryType.UnnamedLong:
					{
						long num5;
						if (!this.UNSAFE_Read_8_Int64(out num5))
						{
							value = 0UL;
							return false;
						}
						if (num5 < 0L)
						{
							value = 0UL;
							return false;
						}
						value = (ulong)num5;
						break;
					}
					case BinaryEntryType.NamedULong:
					case BinaryEntryType.UnnamedULong:
						if (!this.UNSAFE_Read_8_UInt64(out value))
						{
							return false;
						}
						break;
					default:
						throw new InvalidOperationException();
					}
					return true;
				}
				finally
				{
					this.MarkEntryContentConsumed();
				}
			}
			this.SkipEntry();
			value = 0UL;
			return false;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003870 File Offset: 0x00001A70
		public override bool ReadChar(out char value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedChar || this.peekedBinaryEntryType == BinaryEntryType.UnnamedChar)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_2_Char(out value);
			}
			if (this.peekedBinaryEntryType != BinaryEntryType.NamedString && this.peekedBinaryEntryType != BinaryEntryType.UnnamedString)
			{
				this.SkipEntry();
				value = '\0';
				return false;
			}
			this.MarkEntryContentConsumed();
			string text2 = this.ReadStringValue();
			if (text2 == null || text2.Length == 0)
			{
				value = '\0';
				return false;
			}
			value = text2.get_Chars(0);
			return true;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000038FC File Offset: 0x00001AFC
		public override bool ReadSingle(out float value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedFloat || this.peekedBinaryEntryType == BinaryEntryType.UnnamedFloat)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_4_Float32(out value);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedDouble || this.peekedBinaryEntryType == BinaryEntryType.UnnamedDouble)
			{
				this.MarkEntryContentConsumed();
				double num;
				if (!this.UNSAFE_Read_8_Float64(out num))
				{
					value = 0f;
					return false;
				}
				try
				{
					value = (float)num;
				}
				catch (OverflowException)
				{
					value = 0f;
				}
				return true;
			}
			else if (this.peekedBinaryEntryType == BinaryEntryType.NamedDecimal || this.peekedBinaryEntryType == BinaryEntryType.UnnamedDecimal)
			{
				this.MarkEntryContentConsumed();
				decimal num2;
				if (!this.UNSAFE_Read_16_Decimal(out num2))
				{
					value = 0f;
					return false;
				}
				try
				{
					value = (float)num2;
				}
				catch (OverflowException)
				{
					value = 0f;
				}
				return true;
			}
			else
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.Integer;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					this.SkipEntry();
					value = 0f;
					return false;
				}
				long num3;
				if (!this.ReadInt64(out num3))
				{
					value = 0f;
					return false;
				}
				try
				{
					value = (float)num3;
				}
				catch (OverflowException)
				{
					value = 0f;
				}
				return true;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003A44 File Offset: 0x00001C44
		public override bool ReadDouble(out double value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedDouble || this.peekedBinaryEntryType == BinaryEntryType.UnnamedDouble)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_8_Float64(out value);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedFloat || this.peekedBinaryEntryType == BinaryEntryType.UnnamedFloat)
			{
				this.MarkEntryContentConsumed();
				float num;
				if (!this.UNSAFE_Read_4_Float32(out num))
				{
					value = 0.0;
					return false;
				}
				value = (double)num;
				return true;
			}
			else if (this.peekedBinaryEntryType == BinaryEntryType.NamedDecimal || this.peekedBinaryEntryType == BinaryEntryType.UnnamedDecimal)
			{
				this.MarkEntryContentConsumed();
				decimal num2;
				if (!this.UNSAFE_Read_16_Decimal(out num2))
				{
					value = 0.0;
					return false;
				}
				try
				{
					value = (double)num2;
				}
				catch (OverflowException)
				{
					value = 0.0;
				}
				return true;
			}
			else
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.Integer;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					this.SkipEntry();
					value = 0.0;
					return false;
				}
				long num3;
				if (!this.ReadInt64(out num3))
				{
					value = 0.0;
					return false;
				}
				try
				{
					value = (double)num3;
				}
				catch (OverflowException)
				{
					value = 0.0;
				}
				return true;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003B8C File Offset: 0x00001D8C
		public override bool ReadDecimal(out decimal value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedDecimal || this.peekedBinaryEntryType == BinaryEntryType.UnnamedDecimal)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_16_Decimal(out value);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedDouble || this.peekedBinaryEntryType == BinaryEntryType.UnnamedDouble)
			{
				this.MarkEntryContentConsumed();
				double num;
				if (!this.UNSAFE_Read_8_Float64(out num))
				{
					value = default(decimal);
					return false;
				}
				try
				{
					value = (decimal)num;
				}
				catch (OverflowException)
				{
					value = default(decimal);
				}
				return true;
			}
			else if (this.peekedBinaryEntryType == BinaryEntryType.NamedFloat || this.peekedBinaryEntryType == BinaryEntryType.UnnamedFloat)
			{
				this.MarkEntryContentConsumed();
				float num2;
				if (!this.UNSAFE_Read_4_Float32(out num2))
				{
					value = default(decimal);
					return false;
				}
				try
				{
					value = (decimal)num2;
				}
				catch (OverflowException)
				{
					value = default(decimal);
				}
				return true;
			}
			else
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.Integer;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					this.SkipEntry();
					value = default(decimal);
					return false;
				}
				long num3;
				if (!this.ReadInt64(out num3))
				{
					value = default(decimal);
					return false;
				}
				try
				{
					value = num3;
				}
				catch (OverflowException)
				{
					value = default(decimal);
				}
				return true;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003CE8 File Offset: 0x00001EE8
		public override bool ReadExternalReference(out Guid guid)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedExternalReferenceByGuid || this.peekedBinaryEntryType == BinaryEntryType.UnnamedExternalReferenceByGuid)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_16_Guid(out guid);
			}
			this.SkipEntry();
			guid = default(Guid);
			return false;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003D3C File Offset: 0x00001F3C
		public override bool ReadGuid(out Guid value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedGuid || this.peekedBinaryEntryType == BinaryEntryType.UnnamedGuid)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_16_Guid(out value);
			}
			this.SkipEntry();
			value = default(Guid);
			return false;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003D90 File Offset: 0x00001F90
		public override bool ReadExternalReference(out int index)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedExternalReferenceByIndex || this.peekedBinaryEntryType == BinaryEntryType.UnnamedExternalReferenceByIndex)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_4_Int32(out index);
			}
			this.SkipEntry();
			index = -1;
			return false;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003DE0 File Offset: 0x00001FE0
		public override bool ReadExternalReference(out string id)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedExternalReferenceByString || this.peekedBinaryEntryType == BinaryEntryType.UnnamedExternalReferenceByString)
			{
				id = this.ReadStringValue();
				this.MarkEntryContentConsumed();
				return id != null;
			}
			this.SkipEntry();
			id = null;
			return false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003E38 File Offset: 0x00002038
		public override bool ReadNull()
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedNull || this.peekedBinaryEntryType == BinaryEntryType.UnnamedNull)
			{
				this.MarkEntryContentConsumed();
				return true;
			}
			this.SkipEntry();
			return false;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003E80 File Offset: 0x00002080
		public override bool ReadInternalReference(out int id)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedInternalReference || this.peekedBinaryEntryType == BinaryEntryType.UnnamedInternalReference)
			{
				this.MarkEntryContentConsumed();
				return this.UNSAFE_Read_4_Int32(out id);
			}
			this.SkipEntry();
			id = -1;
			return false;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003ED0 File Offset: 0x000020D0
		public override bool ReadString(out string value)
		{
			if (this.peekedEntryType == null)
			{
				string text;
				this.PeekEntry(out text);
			}
			if (this.peekedBinaryEntryType == BinaryEntryType.NamedString || this.peekedBinaryEntryType == BinaryEntryType.UnnamedString)
			{
				value = this.ReadStringValue();
				this.MarkEntryContentConsumed();
				return value != null;
			}
			this.SkipEntry();
			value = null;
			return false;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003F28 File Offset: 0x00002128
		public override void PrepareNewSerializationSession()
		{
			base.PrepareNewSerializationSession();
			this.peekedEntryType = default(EntryType?);
			this.peekedEntryName = null;
			this.peekedBinaryEntryType = BinaryEntryType.Invalid;
			this.types.Clear();
			this.bufferIndex = 0;
			this.bufferEnd = 0;
			this.buffer = this.internalBufferBackup;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003F7C File Offset: 0x0000217C
		public unsafe override string GetDataDump()
		{
			byte[] array;
			if (this.bufferEnd == this.buffer.Length)
			{
				array = this.buffer;
			}
			else
			{
				array = new byte[this.bufferEnd];
				byte[] array2;
				void* from;
				if ((array2 = this.buffer) == null || array2.Length == 0)
				{
					from = null;
				}
				else
				{
					from = (void*)(&array2[0]);
				}
				byte[] array3;
				void* to;
				if ((array3 = array) == null || array3.Length == 0)
				{
					to = null;
				}
				else
				{
					to = (void*)(&array3[0]);
				}
				UnsafeUtilities.MemoryCopy(from, to, array.Length);
				array3 = null;
				array2 = null;
			}
			return "Binary hex dump: " + ProperBitConverter.BytesToHexString(array, true);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004008 File Offset: 0x00002208
		[MethodImpl(256)]
		private unsafe string ReadStringValue()
		{
			byte b;
			if (!this.UNSAFE_Read_1_Byte(out b))
			{
				return null;
			}
			int num;
			if (!this.UNSAFE_Read_4_Int32(out num))
			{
				return null;
			}
			string text = new string(' ', num);
			byte[] array;
			if (b == 0)
			{
				byte* ptr;
				if ((array = this.buffer) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (string text2 = text)
				{
					char* ptr2 = text2;
					if (ptr2 != null)
					{
						ptr2 += RuntimeHelpers.OffsetToStringData / 2;
					}
					byte* ptr3 = ptr + this.bufferIndex;
					byte* ptr4 = (byte*)ptr2;
					if (BitConverter.IsLittleEndian)
					{
						for (int i = 0; i < num; i++)
						{
							*(ptr4++) = *(ptr3++);
							ptr4++;
						}
					}
					else
					{
						for (int j = 0; j < num; j++)
						{
							ptr4++;
							*(ptr4++) = *(ptr3++);
						}
					}
				}
				array = null;
				this.bufferIndex += num;
				return text;
			}
			int num2 = num * 2;
			byte* ptr5;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr5 = null;
			}
			else
			{
				ptr5 = &array[0];
			}
			fixed (string text3 = text)
			{
				char* ptr6 = text3;
				if (ptr6 != null)
				{
					ptr6 += RuntimeHelpers.OffsetToStringData / 2;
				}
				if (BitConverter.IsLittleEndian)
				{
					BinaryDataReader.Struct256Bit* ptr7 = (BinaryDataReader.Struct256Bit*)(ptr5 + this.bufferIndex);
					BinaryDataReader.Struct256Bit* ptr8 = (BinaryDataReader.Struct256Bit*)ptr6;
					byte* ptr9 = (byte*)(ptr6 + num2 / 2);
					while (ptr8 + 1 < (BinaryDataReader.Struct256Bit*)ptr9)
					{
						BinaryDataReader.Struct256Bit* ptr10 = ptr8;
						ptr8 = ptr10 + 1;
						ref BinaryDataReader.Struct256Bit ptr11 = ref *ptr10;
						ptr10 = ptr7;
						ptr7 = ptr10 + 1;
						ptr11 = *ptr10;
					}
					byte* ptr12 = (byte*)ptr7;
					byte* ptr13 = (byte*)ptr8;
					while (ptr13 < ptr9)
					{
						*(ptr13++) = *(ptr12++);
					}
				}
				else
				{
					byte* ptr14 = ptr5 + this.bufferIndex;
					byte* ptr15 = (byte*)ptr6;
					for (int k = 0; k < num; k++)
					{
						*ptr15 = ptr14[1];
						ptr15[1] = *ptr14;
						ptr14 += 2;
						ptr15 += 2;
					}
				}
			}
			array = null;
			this.bufferIndex += num2;
			return text;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000041F4 File Offset: 0x000023F4
		[MethodImpl(256)]
		private void SkipStringValue()
		{
			byte b;
			if (!this.UNSAFE_Read_1_Byte(out b))
			{
				return;
			}
			int num;
			if (!this.UNSAFE_Read_4_Int32(out num))
			{
				return;
			}
			if (b != 0)
			{
				num *= 2;
			}
			if (this.HasBufferData(num))
			{
				this.bufferIndex += num;
				return;
			}
			this.bufferIndex = this.bufferEnd;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004244 File Offset: 0x00002444
		private void SkipPeekedEntryContent()
		{
			if (this.peekedEntryType != null)
			{
				try
				{
					switch (this.peekedBinaryEntryType)
					{
					case BinaryEntryType.NamedStartOfReferenceNode:
					case BinaryEntryType.UnnamedStartOfReferenceNode:
						this.ReadTypeEntry();
						if (!this.SkipBuffer(4))
						{
						}
						break;
					case BinaryEntryType.NamedStartOfStructNode:
					case BinaryEntryType.UnnamedStartOfStructNode:
						this.ReadTypeEntry();
						break;
					case BinaryEntryType.StartOfArray:
						this.SkipBuffer(8);
						break;
					case BinaryEntryType.PrimitiveArray:
					{
						int num;
						int num2;
						if (this.UNSAFE_Read_4_Int32(out num) && this.UNSAFE_Read_4_Int32(out num2))
						{
							this.SkipBuffer(num * num2);
						}
						break;
					}
					case BinaryEntryType.NamedInternalReference:
					case BinaryEntryType.UnnamedInternalReference:
					case BinaryEntryType.NamedExternalReferenceByIndex:
					case BinaryEntryType.UnnamedExternalReferenceByIndex:
					case BinaryEntryType.NamedInt:
					case BinaryEntryType.UnnamedInt:
					case BinaryEntryType.NamedUInt:
					case BinaryEntryType.UnnamedUInt:
					case BinaryEntryType.NamedFloat:
					case BinaryEntryType.UnnamedFloat:
						this.SkipBuffer(4);
						break;
					case BinaryEntryType.NamedExternalReferenceByGuid:
					case BinaryEntryType.UnnamedExternalReferenceByGuid:
					case BinaryEntryType.NamedDecimal:
					case BinaryEntryType.UnnamedDecimal:
					case BinaryEntryType.NamedGuid:
					case BinaryEntryType.UnnamedGuid:
						this.SkipBuffer(8);
						break;
					case BinaryEntryType.NamedSByte:
					case BinaryEntryType.UnnamedSByte:
					case BinaryEntryType.NamedByte:
					case BinaryEntryType.UnnamedByte:
					case BinaryEntryType.NamedBoolean:
					case BinaryEntryType.UnnamedBoolean:
						this.SkipBuffer(1);
						break;
					case BinaryEntryType.NamedShort:
					case BinaryEntryType.UnnamedShort:
					case BinaryEntryType.NamedUShort:
					case BinaryEntryType.UnnamedUShort:
					case BinaryEntryType.NamedChar:
					case BinaryEntryType.UnnamedChar:
						this.SkipBuffer(2);
						break;
					case BinaryEntryType.NamedLong:
					case BinaryEntryType.UnnamedLong:
					case BinaryEntryType.NamedULong:
					case BinaryEntryType.UnnamedULong:
					case BinaryEntryType.NamedDouble:
					case BinaryEntryType.UnnamedDouble:
						this.SkipBuffer(8);
						break;
					case BinaryEntryType.NamedString:
					case BinaryEntryType.UnnamedString:
					case BinaryEntryType.NamedExternalReferenceByString:
					case BinaryEntryType.UnnamedExternalReferenceByString:
						this.SkipStringValue();
						break;
					case BinaryEntryType.TypeName:
						base.Context.Config.DebugContext.LogError("Parsing error in binary data reader: should not be able to peek a TypeName entry.");
						this.SkipBuffer(4);
						this.ReadStringValue();
						break;
					case BinaryEntryType.TypeID:
						base.Context.Config.DebugContext.LogError("Parsing error in binary data reader: should not be able to peek a TypeID entry.");
						this.SkipBuffer(4);
						break;
					}
				}
				finally
				{
					this.MarkEntryContentConsumed();
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000444C File Offset: 0x0000264C
		[MethodImpl(256)]
		private bool SkipBuffer(int amount)
		{
			int num = this.bufferIndex + amount;
			if (num > this.bufferEnd)
			{
				this.bufferIndex = this.bufferEnd;
				return false;
			}
			this.bufferIndex = num;
			return true;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004484 File Offset: 0x00002684
		[MethodImpl(256)]
		private Type ReadTypeEntry()
		{
			if (!this.HasBufferData(1))
			{
				return null;
			}
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			BinaryEntryType binaryEntryType = array[num];
			Type type;
			if (binaryEntryType == BinaryEntryType.TypeID)
			{
				int num2;
				if (!this.UNSAFE_Read_4_Int32(out num2))
				{
					return null;
				}
				if (!this.types.TryGetValue(num2, ref type))
				{
					base.Context.Config.DebugContext.LogError(string.Concat(new string[]
					{
						"Missing type ID during deserialization: ",
						num2.ToString(),
						" at node ",
						base.CurrentNodeName,
						" and depth ",
						base.CurrentNodeDepth.ToString(),
						" and id ",
						base.CurrentNodeId.ToString()
					}));
				}
			}
			else if (binaryEntryType == BinaryEntryType.TypeName)
			{
				int num2;
				if (!this.UNSAFE_Read_4_Int32(out num2))
				{
					return null;
				}
				string text = this.ReadStringValue();
				type = ((text == null) ? null : base.Context.Binder.BindToType(text, base.Context.Config.DebugContext));
				this.types.Add(num2, type);
			}
			else if (binaryEntryType == BinaryEntryType.UnnamedNull)
			{
				type = null;
			}
			else
			{
				type = null;
				base.Context.Config.DebugContext.LogError("Expected TypeName, TypeID or UnnamedNull entry flag for reading type data, but instead got the entry flag: " + binaryEntryType.ToString() + ".");
			}
			return type;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000045E5 File Offset: 0x000027E5
		[MethodImpl(256)]
		private void MarkEntryContentConsumed()
		{
			this.peekedEntryType = default(EntryType?);
			this.peekedEntryName = null;
			this.peekedBinaryEntryType = BinaryEntryType.Invalid;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004604 File Offset: 0x00002804
		protected override EntryType PeekEntry()
		{
			string text;
			return this.PeekEntry(out text);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000461C File Offset: 0x0000281C
		protected override EntryType ReadToNextEntry()
		{
			this.SkipPeekedEntryContent();
			string text;
			return this.PeekEntry(out text);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004638 File Offset: 0x00002838
		[MethodImpl(256)]
		private bool UNSAFE_Read_1_Byte(out byte value)
		{
			if (this.HasBufferData(1))
			{
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				value = array[num];
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004670 File Offset: 0x00002870
		[MethodImpl(256)]
		private bool UNSAFE_Read_1_SByte(out sbyte value)
		{
			if (this.HasBufferData(1))
			{
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				value = array[num];
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000046A8 File Offset: 0x000028A8
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_2_Int16(out short value)
		{
			if (this.HasBufferData(2))
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
					value = *(short*)(ptr + this.bufferIndex);
				}
				else
				{
					short num = 0;
					byte* ptr2 = (byte*)(&num) + 1;
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr2--) = *(ptr3++);
					*ptr2 = *ptr3;
					value = num;
				}
				array = null;
				this.bufferIndex += 2;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0;
			return false;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000473C File Offset: 0x0000293C
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_2_UInt16(out ushort value)
		{
			if (this.HasBufferData(2))
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
					value = *(ushort*)(ptr + this.bufferIndex);
				}
				else
				{
					ushort num = 0;
					byte* ptr2 = (byte*)(&num) + 1;
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr2--) = *(ptr3++);
					*ptr2 = *ptr3;
					value = num;
				}
				array = null;
				this.bufferIndex += 2;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0;
			return false;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000047D0 File Offset: 0x000029D0
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_2_Char(out char value)
		{
			if (this.HasBufferData(2))
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
					value = (char)(*(ushort*)(ptr + this.bufferIndex));
				}
				else
				{
					char c = '\0';
					byte* ptr2 = (byte*)(&c) + 1;
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr2--) = *(ptr3++);
					*ptr2 = *ptr3;
					value = c;
				}
				array = null;
				this.bufferIndex += 2;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = '\0';
			return false;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004864 File Offset: 0x00002A64
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_4_Int32(out int value)
		{
			if (this.HasBufferData(4))
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
					value = *(int*)(ptr + this.bufferIndex);
				}
				else
				{
					int num = 0;
					byte* ptr2 = (byte*)(&num) + 3;
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr2--) = *(ptr3++);
					*(ptr2--) = *(ptr3++);
					*(ptr2--) = *(ptr3++);
					*ptr2 = *ptr3;
					value = num;
				}
				array = null;
				this.bufferIndex += 4;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0;
			return false;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004914 File Offset: 0x00002B14
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_4_UInt32(out uint value)
		{
			if (this.HasBufferData(4))
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
					value = *(uint*)(ptr + this.bufferIndex);
				}
				else
				{
					uint num = 0U;
					byte* ptr2 = (byte*)(&num) + 3;
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr2--) = *(ptr3++);
					*(ptr2--) = *(ptr3++);
					*(ptr2--) = *(ptr3++);
					*ptr2 = *ptr3;
					value = num;
				}
				array = null;
				this.bufferIndex += 4;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0U;
			return false;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000049C4 File Offset: 0x00002BC4
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_4_Float32(out float value)
		{
			if (this.HasBufferData(4))
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
					if (ArchitectureInfo.Architecture_Supports_Unaligned_Float32_Reads)
					{
						value = *(float*)(ptr + this.bufferIndex);
					}
					else
					{
						float num = 0f;
						*(int*)(&num) = *(int*)(ptr + this.bufferIndex);
						value = num;
					}
				}
				else
				{
					float num2 = 0f;
					byte* ptr2 = (byte*)(&num2) + 3;
					byte* ptr3 = ptr + this.bufferIndex;
					*(ptr2--) = *(ptr3++);
					*(ptr2--) = *(ptr3++);
					*(ptr2--) = *(ptr3++);
					*ptr2 = *ptr3;
					value = num2;
				}
				array = null;
				this.bufferIndex += 4;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0f;
			return false;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004AA4 File Offset: 0x00002CA4
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_8_Int64(out long value)
		{
			if (this.HasBufferData(8))
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
						value = *(long*)(ptr + this.bufferIndex);
					}
					else
					{
						long num = 0L;
						int* ptr2 = (int*)(&num);
						int* ptr3 = (int*)(ptr + this.bufferIndex);
						*(ptr2++) = *(ptr3++);
						*ptr2 = *ptr3;
						value = num;
					}
				}
				else
				{
					long num2 = 0L;
					byte* ptr4 = (byte*)(&num2) + 7;
					byte* ptr5 = ptr + this.bufferIndex;
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*ptr4 = *ptr5;
					value = num2;
				}
				array = null;
				this.bufferIndex += 8;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0L;
			return false;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004BD8 File Offset: 0x00002DD8
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_8_UInt64(out ulong value)
		{
			if (this.HasBufferData(8))
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
						value = (ulong)(*(long*)(ptr + this.bufferIndex));
					}
					else
					{
						ulong num = 0UL;
						int* ptr2 = (int*)(&num);
						int* ptr3 = (int*)(ptr + this.bufferIndex);
						*(ptr2++) = *(ptr3++);
						*ptr2 = *ptr3;
						value = num;
					}
				}
				else
				{
					ulong num2 = 0UL;
					byte* ptr4 = (byte*)(&num2) + 7;
					byte* ptr5 = ptr + this.bufferIndex;
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*ptr4 = *ptr5;
					value = num2;
				}
				array = null;
				this.bufferIndex += 8;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0UL;
			return false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004D0C File Offset: 0x00002F0C
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_8_Float64(out double value)
		{
			if (this.HasBufferData(8))
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
						value = *(double*)(ptr + this.bufferIndex);
					}
					else
					{
						double num = 0.0;
						int* ptr2 = (int*)(&num);
						int* ptr3 = (int*)(ptr + this.bufferIndex);
						*(ptr2++) = *(ptr3++);
						*ptr2 = *ptr3;
						value = num;
					}
				}
				else
				{
					double num2 = 0.0;
					byte* ptr4 = (byte*)(&num2) + 7;
					byte* ptr5 = ptr + this.bufferIndex;
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*ptr4 = *ptr5;
					value = num2;
				}
				array = null;
				this.bufferIndex += 8;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = 0.0;
			return false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004E54 File Offset: 0x00003054
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_16_Decimal(out decimal value)
		{
			if (this.HasBufferData(16))
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
						value = *(decimal*)(ptr + this.bufferIndex);
					}
					else
					{
						decimal num = default(decimal);
						int* ptr2 = (int*)(&num);
						int* ptr3 = (int*)(ptr + this.bufferIndex);
						*(ptr2++) = *(ptr3++);
						*(ptr2++) = *(ptr3++);
						*(ptr2++) = *(ptr3++);
						*ptr2 = *ptr3;
						value = num;
					}
				}
				else
				{
					decimal num2 = default(decimal);
					byte* ptr4 = (byte*)(&num2) + 15;
					byte* ptr5 = ptr + this.bufferIndex;
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*ptr4 = *ptr5;
					value = num2;
				}
				array = null;
				this.bufferIndex += 16;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = default(decimal);
			return false;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005044 File Offset: 0x00003244
		[MethodImpl(256)]
		private unsafe bool UNSAFE_Read_16_Guid(out Guid value)
		{
			if (this.HasBufferData(16))
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
						value = *(Guid*)(ptr + this.bufferIndex);
					}
					else
					{
						Guid guid = default(Guid);
						int* ptr2 = (int*)(&guid);
						int* ptr3 = (int*)(ptr + this.bufferIndex);
						*(ptr2++) = *(ptr3++);
						*(ptr2++) = *(ptr3++);
						*(ptr2++) = *(ptr3++);
						*ptr2 = *ptr3;
						value = guid;
					}
				}
				else
				{
					Guid guid2 = default(Guid);
					byte* ptr4 = (byte*)(&guid2);
					byte* ptr5 = ptr + this.bufferIndex;
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*(ptr4++) = *(ptr5++);
					*ptr4 = *(ptr5++);
					ptr4 += 6;
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*(ptr4--) = *(ptr5++);
					*ptr4 = *ptr5;
					value = guid2;
				}
				array = null;
				this.bufferIndex += 16;
				return true;
			}
			this.bufferIndex = this.bufferEnd;
			value = default(Guid);
			return false;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000522F File Offset: 0x0000342F
		[MethodImpl(256)]
		private bool HasBufferData(int amount)
		{
			if (this.bufferEnd == 0)
			{
				this.ReadEntireStreamToBuffer();
			}
			return this.bufferIndex + amount <= this.bufferEnd;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005254 File Offset: 0x00003454
		private void ReadEntireStreamToBuffer()
		{
			this.bufferIndex = 0;
			if (this.Stream is MemoryStream)
			{
				try
				{
					this.buffer = (this.Stream as MemoryStream).GetBuffer();
					this.bufferEnd = (int)this.Stream.Length;
					this.bufferIndex = (int)this.Stream.Position;
					return;
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			this.buffer = this.internalBufferBackup;
			int num = (int)(this.Stream.Length - this.Stream.Position);
			if (this.buffer.Length >= num)
			{
				this.Stream.Read(this.buffer, 0, num);
			}
			else
			{
				this.buffer = new byte[num];
				this.Stream.Read(this.buffer, 0, num);
				if (num <= 10485760)
				{
					this.internalBufferBackup = this.buffer;
				}
			}
			this.bufferIndex = 0;
			this.bufferEnd = num;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005350 File Offset: 0x00003550
		// Note: this type is marked as 'beforefieldinit'.
		static BinaryDataReader()
		{
			Dictionary<Type, Delegate> dictionary = new Dictionary<Type, Delegate>();
			dictionary.Add(typeof(char), (byte[] b, int i) => (char)ProperBitConverter.ToUInt16(b, i));
			dictionary.Add(typeof(byte), (byte[] b, int i) => b[i]);
			dictionary.Add(typeof(sbyte), (byte[] b, int i) => (sbyte)b[i]);
			dictionary.Add(typeof(bool), (byte[] b, int i) => b[i] > 0);
			dictionary.Add(typeof(short), new Func<byte[], int, short>(ProperBitConverter.ToInt16));
			dictionary.Add(typeof(int), new Func<byte[], int, int>(ProperBitConverter.ToInt32));
			dictionary.Add(typeof(long), new Func<byte[], int, long>(ProperBitConverter.ToInt64));
			dictionary.Add(typeof(ushort), new Func<byte[], int, ushort>(ProperBitConverter.ToUInt16));
			dictionary.Add(typeof(uint), new Func<byte[], int, uint>(ProperBitConverter.ToUInt32));
			dictionary.Add(typeof(ulong), new Func<byte[], int, ulong>(ProperBitConverter.ToUInt64));
			dictionary.Add(typeof(decimal), new Func<byte[], int, decimal>(ProperBitConverter.ToDecimal));
			dictionary.Add(typeof(float), new Func<byte[], int, float>(ProperBitConverter.ToSingle));
			dictionary.Add(typeof(double), new Func<byte[], int, double>(ProperBitConverter.ToDouble));
			dictionary.Add(typeof(Guid), new Func<byte[], int, Guid>(ProperBitConverter.ToGuid));
			BinaryDataReader.PrimitiveFromByteMethods = dictionary;
		}

		// Token: 0x0400000F RID: 15
		private static readonly Dictionary<Type, Delegate> PrimitiveFromByteMethods;

		// Token: 0x04000010 RID: 16
		private byte[] internalBufferBackup;

		// Token: 0x04000011 RID: 17
		private byte[] buffer = new byte[102400];

		// Token: 0x04000012 RID: 18
		private int bufferIndex;

		// Token: 0x04000013 RID: 19
		private int bufferEnd;

		// Token: 0x04000014 RID: 20
		private EntryType? peekedEntryType;

		// Token: 0x04000015 RID: 21
		private BinaryEntryType peekedBinaryEntryType;

		// Token: 0x04000016 RID: 22
		private string peekedEntryName;

		// Token: 0x04000017 RID: 23
		private Dictionary<int, Type> types = new Dictionary<int, Type>(16);

		// Token: 0x020000DA RID: 218
		private struct Struct256Bit
		{
			// Token: 0x0400022F RID: 559
			public decimal d1;

			// Token: 0x04000230 RID: 560
			public decimal d2;
		}
	}
}
