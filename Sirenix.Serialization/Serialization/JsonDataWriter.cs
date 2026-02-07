using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Sirenix.Serialization
{
	// Token: 0x02000013 RID: 19
	public class JsonDataWriter : BaseDataWriter
	{
		// Token: 0x06000177 RID: 375 RVA: 0x00009EDF File Offset: 0x000080DF
		public JsonDataWriter() : this(null, null, true)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00009EEC File Offset: 0x000080EC
		public JsonDataWriter(Stream stream, SerializationContext context, bool formatAsReadable = true) : base(stream, context)
		{
			this.FormatAsReadable = formatAsReadable;
			this.justStarted = true;
			this.EnableTypeOptimization = true;
			Dictionary<Type, Delegate> dictionary = new Dictionary<Type, Delegate>();
			dictionary.Add(typeof(char), new Action<string, char>(this.WriteChar));
			dictionary.Add(typeof(sbyte), new Action<string, sbyte>(this.WriteSByte));
			dictionary.Add(typeof(short), new Action<string, short>(this.WriteInt16));
			dictionary.Add(typeof(int), new Action<string, int>(this.WriteInt32));
			dictionary.Add(typeof(long), new Action<string, long>(this.WriteInt64));
			dictionary.Add(typeof(byte), new Action<string, byte>(this.WriteByte));
			dictionary.Add(typeof(ushort), new Action<string, ushort>(this.WriteUInt16));
			dictionary.Add(typeof(uint), new Action<string, uint>(this.WriteUInt32));
			dictionary.Add(typeof(ulong), new Action<string, ulong>(this.WriteUInt64));
			dictionary.Add(typeof(decimal), new Action<string, decimal>(this.WriteDecimal));
			dictionary.Add(typeof(bool), new Action<string, bool>(this.WriteBoolean));
			dictionary.Add(typeof(float), new Action<string, float>(this.WriteSingle));
			dictionary.Add(typeof(double), new Action<string, double>(this.WriteDouble));
			dictionary.Add(typeof(Guid), new Action<string, Guid>(this.WriteGuid));
			this.primitiveTypeWriters = dictionary;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A0D6 File Offset: 0x000082D6
		public void MarkJustStarted()
		{
			this.justStarted = true;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A0DF File Offset: 0x000082DF
		public override void FlushToStream()
		{
			if (this.bufferIndex > 0)
			{
				this.Stream.Write(this.buffer, 0, this.bufferIndex);
				this.bufferIndex = 0;
			}
			base.FlushToStream();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A10F File Offset: 0x0000830F
		public override void BeginReferenceNode(string name, Type type, int id)
		{
			this.WriteEntry(name, "{");
			base.PushNode(name, id, type);
			this.forceNoSeparatorNextLine = true;
			this.WriteInt32("$id", id);
			if (type != null)
			{
				this.WriteTypeEntry(type);
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A149 File Offset: 0x00008349
		public override void BeginStructNode(string name, Type type)
		{
			this.WriteEntry(name, "{");
			base.PushNode(name, -1, type);
			this.forceNoSeparatorNextLine = true;
			if (type != null)
			{
				this.WriteTypeEntry(type);
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A178 File Offset: 0x00008378
		public override void EndNode(string name)
		{
			base.PopNode(name);
			this.StartNewLine(true);
			this.EnsureBufferSpace(1);
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array[num] = 125;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A1B4 File Offset: 0x000083B4
		public override void BeginArrayNode(long length)
		{
			this.WriteInt64("$rlength", length);
			this.WriteEntry("$rcontent", "[");
			this.forceNoSeparatorNextLine = true;
			base.PushArray();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A1E0 File Offset: 0x000083E0
		public override void EndArrayNode()
		{
			base.PopArray();
			this.StartNewLine(true);
			this.EnsureBufferSpace(1);
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array[num] = 93;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000A21C File Offset: 0x0000841C
		public override void WritePrimitiveArray<T>(T[] array)
		{
			if (!FormatterUtilities.IsPrimitiveArrayType(typeof(T)))
			{
				throw new ArgumentException("Type " + typeof(T).Name + " is not a valid primitive array type.");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Action<string, T> action = (Action<string, T>)this.primitiveTypeWriters[typeof(T)];
			this.WriteInt64("$plength", (long)array.Length);
			this.WriteEntry("$pcontent", "[");
			this.forceNoSeparatorNextLine = true;
			base.PushArray();
			for (int i = 0; i < array.Length; i++)
			{
				action.Invoke(null, array[i]);
			}
			base.PopArray();
			this.StartNewLine(true);
			this.EnsureBufferSpace(1);
			byte[] array2 = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 93;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A2FD File Offset: 0x000084FD
		public override void WriteBoolean(string name, bool value)
		{
			this.WriteEntry(name, value ? "true" : "false");
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A315 File Offset: 0x00008515
		public override void WriteByte(string name, byte value)
		{
			this.WriteUInt64(name, (ulong)value);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000A320 File Offset: 0x00008520
		public override void WriteChar(string name, char value)
		{
			this.WriteString(name, value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000A335 File Offset: 0x00008535
		public override void WriteDecimal(string name, decimal value)
		{
			this.WriteEntry(name, value.ToString("G", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A34F File Offset: 0x0000854F
		public override void WriteDouble(string name, double value)
		{
			this.WriteEntry(name, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000A369 File Offset: 0x00008569
		public override void WriteInt32(string name, int value)
		{
			this.WriteInt64(name, (long)value);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000A374 File Offset: 0x00008574
		public override void WriteInt64(string name, long value)
		{
			this.WriteEntry(name, value.ToString("D", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000A38E File Offset: 0x0000858E
		public override void WriteNull(string name)
		{
			this.WriteEntry(name, "null");
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000A39C File Offset: 0x0000859C
		public override void WriteInternalReference(string name, int id)
		{
			this.WriteEntry(name, "$iref:" + id.ToString("D", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000A369 File Offset: 0x00008569
		public override void WriteSByte(string name, sbyte value)
		{
			this.WriteInt64(name, (long)value);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000A369 File Offset: 0x00008569
		public override void WriteInt16(string name, short value)
		{
			this.WriteInt64(name, (long)value);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000A3C0 File Offset: 0x000085C0
		public override void WriteSingle(string name, float value)
		{
			this.WriteEntry(name, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000A3DC File Offset: 0x000085DC
		public override void WriteString(string name, string value)
		{
			this.StartNewLine(false);
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(name.Length + value.Length + 6);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 34;
				for (int i = 0; i < name.Length; i++)
				{
					byte[] array2 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array2[num] = (byte)name.get_Chars(i);
				}
				byte[] array3 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array3[num] = 34;
				byte[] array4 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array4[num] = 58;
				if (this.FormatAsReadable)
				{
					byte[] array5 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array5[num] = 32;
				}
			}
			else
			{
				this.EnsureBufferSpace(value.Length + 2);
			}
			byte[] array6 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array6[num] = 34;
			this.Buffer_WriteString_WithEscape(value);
			byte[] array7 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array7[num] = 34;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A4F7 File Offset: 0x000086F7
		public override void WriteGuid(string name, Guid value)
		{
			this.WriteEntry(name, value.ToString("D", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A315 File Offset: 0x00008515
		public override void WriteUInt32(string name, uint value)
		{
			this.WriteUInt64(name, (ulong)value);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A511 File Offset: 0x00008711
		public override void WriteUInt64(string name, ulong value)
		{
			this.WriteEntry(name, value.ToString("D", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000A52B File Offset: 0x0000872B
		public override void WriteExternalReference(string name, int index)
		{
			this.WriteEntry(name, "$eref:" + index.ToString("D", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A54F File Offset: 0x0000874F
		public override void WriteExternalReference(string name, Guid guid)
		{
			this.WriteEntry(name, "$guidref:" + guid.ToString("D", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000A574 File Offset: 0x00008774
		public override void WriteExternalReference(string name, string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.WriteEntry(name, "$fstrref");
			this.EnsureBufferSpace(id.Length + 3);
			byte[] array = this.buffer;
			int num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array[num] = 58;
			byte[] array2 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array2[num] = 34;
			this.Buffer_WriteString_WithEscape(id);
			byte[] array3 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array3[num] = 34;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000A315 File Offset: 0x00008515
		public override void WriteUInt16(string name, ushort value)
		{
			this.WriteUInt64(name, (ulong)value);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000021B8 File Offset: 0x000003B8
		public override void Dispose()
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000A5FE File Offset: 0x000087FE
		public override void PrepareNewSerializationSession()
		{
			base.PrepareNewSerializationSession();
			this.seenTypes.Clear();
			this.justStarted = true;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000A618 File Offset: 0x00008818
		public override string GetDataDump()
		{
			if (!this.Stream.CanRead)
			{
				return "Json data stream for writing cannot be read; cannot dump data.";
			}
			if (!this.Stream.CanSeek)
			{
				return "Json data stream cannot seek; cannot dump data.";
			}
			long position = this.Stream.Position;
			byte[] array = new byte[position];
			this.Stream.Position = 0L;
			this.Stream.Read(array, 0, (int)position);
			this.Stream.Position = position;
			return "Json: " + Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A6A4 File Offset: 0x000088A4
		private void WriteEntry(string name, string contents)
		{
			this.StartNewLine(false);
			if (name != null)
			{
				this.EnsureBufferSpace(name.Length + contents.Length + 4);
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 34;
				for (int i = 0; i < name.Length; i++)
				{
					byte[] array2 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array2[num] = (byte)name.get_Chars(i);
				}
				byte[] array3 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array3[num] = 34;
				byte[] array4 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array4[num] = 58;
				if (this.FormatAsReadable)
				{
					byte[] array5 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array5[num] = 32;
				}
			}
			else
			{
				this.EnsureBufferSpace(contents.Length);
			}
			for (int j = 0; j < contents.Length; j++)
			{
				byte[] array6 = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array6[num] = (byte)contents.get_Chars(j);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000A7B4 File Offset: 0x000089B4
		private void WriteEntry(string name, string contents, char surroundContentsWith)
		{
			this.StartNewLine(false);
			int num;
			if (name != null)
			{
				this.EnsureBufferSpace(name.Length + contents.Length + 6);
				byte[] array = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 34;
				for (int i = 0; i < name.Length; i++)
				{
					byte[] array2 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array2[num] = (byte)name.get_Chars(i);
				}
				byte[] array3 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array3[num] = 34;
				byte[] array4 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array4[num] = 58;
				if (this.FormatAsReadable)
				{
					byte[] array5 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array5[num] = 32;
				}
			}
			else
			{
				this.EnsureBufferSpace(contents.Length + 2);
			}
			byte[] array6 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array6[num] = (byte)surroundContentsWith;
			for (int j = 0; j < contents.Length; j++)
			{
				byte[] array7 = this.buffer;
				num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array7[num] = (byte)contents.get_Chars(j);
			}
			byte[] array8 = this.buffer;
			num = this.bufferIndex;
			this.bufferIndex = num + 1;
			array8[num] = (byte)surroundContentsWith;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000A8FC File Offset: 0x00008AFC
		private void WriteTypeEntry(Type type)
		{
			if (!this.EnableTypeOptimization)
			{
				this.WriteString("$type", base.Context.Binder.BindToName(type, base.Context.Config.DebugContext));
				return;
			}
			int count;
			if (this.seenTypes.TryGetValue(type, ref count))
			{
				this.WriteInt32("$type", count);
				return;
			}
			count = this.seenTypes.Count;
			this.seenTypes.Add(type, count);
			this.WriteString("$type", count.ToString() + "|" + base.Context.Binder.BindToName(type, base.Context.Config.DebugContext));
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		private void StartNewLine(bool noSeparator = false)
		{
			if (this.justStarted)
			{
				this.justStarted = false;
				return;
			}
			if (!noSeparator && !this.forceNoSeparatorNextLine)
			{
				this.EnsureBufferSpace(1);
				byte[] array = this.buffer;
				int num = this.bufferIndex;
				this.bufferIndex = num + 1;
				array[num] = 44;
			}
			this.forceNoSeparatorNextLine = false;
			if (this.FormatAsReadable)
			{
				int num2 = base.NodeDepth * 4;
				this.EnsureBufferSpace(JsonDataWriter.NEW_LINE.Length + num2);
				for (int i = 0; i < JsonDataWriter.NEW_LINE.Length; i++)
				{
					byte[] array2 = this.buffer;
					int num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array2[num] = (byte)JsonDataWriter.NEW_LINE.get_Chars(i);
				}
				for (int j = 0; j < num2; j++)
				{
					byte[] array3 = this.buffer;
					int num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array3[num] = 32;
				}
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000AA88 File Offset: 0x00008C88
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

		// Token: 0x0600019D RID: 413 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		private void Buffer_WriteString_WithEscape(string str)
		{
			this.EnsureBufferSpace(str.Length);
			for (int i = 0; i < str.Length; i++)
			{
				char c = str.get_Chars(i);
				if (c < '\0' || c > '\u007f')
				{
					this.EnsureBufferSpace(str.Length - i + 6);
					byte[] array = this.buffer;
					int num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array[num] = 92;
					byte[] array2 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array2[num] = 117;
					int num2 = (int)(c >> 8);
					byte b = (byte)c;
					uint num3 = JsonDataWriter.ByteToHexCharLookup[num2];
					byte[] array3 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array3[num] = (byte)num3;
					byte[] array4 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array4[num] = (byte)(num3 >> 16);
					num3 = JsonDataWriter.ByteToHexCharLookup[(int)b];
					byte[] array5 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array5[num] = (byte)num3;
					byte[] array6 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array6[num] = (byte)(num3 >> 16);
				}
				else
				{
					this.EnsureBufferSpace(2);
					int num;
					if (c <= '\r')
					{
						if (c == '\0')
						{
							byte[] array7 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array7[num] = 92;
							byte[] array8 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array8[num] = 48;
							goto IL_3A5;
						}
						switch (c)
						{
						case '\a':
						{
							byte[] array9 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array9[num] = 92;
							byte[] array10 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array10[num] = 97;
							goto IL_3A5;
						}
						case '\b':
						{
							byte[] array11 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array11[num] = 92;
							byte[] array12 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array12[num] = 98;
							goto IL_3A5;
						}
						case '\t':
						{
							byte[] array13 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array13[num] = 92;
							byte[] array14 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array14[num] = 116;
							goto IL_3A5;
						}
						case '\n':
						{
							byte[] array15 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array15[num] = 92;
							byte[] array16 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array16[num] = 110;
							goto IL_3A5;
						}
						case '\f':
						{
							byte[] array17 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array17[num] = 92;
							byte[] array18 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array18[num] = 102;
							goto IL_3A5;
						}
						case '\r':
						{
							byte[] array19 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array19[num] = 92;
							byte[] array20 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array20[num] = 114;
							goto IL_3A5;
						}
						}
					}
					else
					{
						if (c == '"')
						{
							byte[] array21 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array21[num] = 92;
							byte[] array22 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array22[num] = 34;
							goto IL_3A5;
						}
						if (c == '\\')
						{
							byte[] array23 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array23[num] = 92;
							byte[] array24 = this.buffer;
							num = this.bufferIndex;
							this.bufferIndex = num + 1;
							array24[num] = 92;
							goto IL_3A5;
						}
					}
					byte[] array25 = this.buffer;
					num = this.bufferIndex;
					this.bufferIndex = num + 1;
					array25[num] = (byte)c;
				}
				IL_3A5:;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000AE84 File Offset: 0x00009084
		private static uint[] CreateByteToHexLookup()
		{
			uint[] array = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				string text = i.ToString("x2", CultureInfo.InvariantCulture);
				array[i] = (uint)(text.get_Chars(0) + ((uint)text.get_Chars(1) << 16));
			}
			return array;
		}

		// Token: 0x04000066 RID: 102
		private static readonly uint[] ByteToHexCharLookup = JsonDataWriter.CreateByteToHexLookup();

		// Token: 0x04000067 RID: 103
		private static readonly string NEW_LINE = Environment.NewLine;

		// Token: 0x04000068 RID: 104
		private bool justStarted;

		// Token: 0x04000069 RID: 105
		private bool forceNoSeparatorNextLine;

		// Token: 0x0400006A RID: 106
		private Dictionary<Type, Delegate> primitiveTypeWriters;

		// Token: 0x0400006B RID: 107
		private Dictionary<Type, int> seenTypes = new Dictionary<Type, int>(16);

		// Token: 0x0400006C RID: 108
		private byte[] buffer = new byte[102400];

		// Token: 0x0400006D RID: 109
		private int bufferIndex;

		// Token: 0x0400006E RID: 110
		public bool FormatAsReadable;

		// Token: 0x0400006F RID: 111
		public bool EnableTypeOptimization;
	}
}
