using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Sirenix.Serialization
{
	// Token: 0x02000012 RID: 18
	public class JsonDataReader : BaseDataReader
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000898C File Offset: 0x00006B8C
		public JsonDataReader() : this(null, null)
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00008998 File Offset: 0x00006B98
		public JsonDataReader(Stream stream, DeserializationContext context) : base(stream, context)
		{
			Dictionary<Type, Delegate> dictionary = new Dictionary<Type, Delegate>();
			dictionary.Add(typeof(char), delegate()
			{
				char result;
				this.ReadChar(out result);
				return result;
			});
			dictionary.Add(typeof(sbyte), delegate()
			{
				sbyte result;
				this.ReadSByte(out result);
				return result;
			});
			dictionary.Add(typeof(short), delegate()
			{
				short result;
				this.ReadInt16(out result);
				return result;
			});
			dictionary.Add(typeof(int), delegate()
			{
				int result;
				this.ReadInt32(out result);
				return result;
			});
			dictionary.Add(typeof(long), delegate()
			{
				long result;
				this.ReadInt64(out result);
				return result;
			});
			dictionary.Add(typeof(byte), delegate()
			{
				byte result;
				this.ReadByte(out result);
				return result;
			});
			dictionary.Add(typeof(ushort), delegate()
			{
				ushort result;
				this.ReadUInt16(out result);
				return result;
			});
			dictionary.Add(typeof(uint), delegate()
			{
				uint result;
				this.ReadUInt32(out result);
				return result;
			});
			dictionary.Add(typeof(ulong), delegate()
			{
				ulong result;
				this.ReadUInt64(out result);
				return result;
			});
			dictionary.Add(typeof(decimal), delegate()
			{
				decimal result;
				this.ReadDecimal(out result);
				return result;
			});
			dictionary.Add(typeof(bool), delegate()
			{
				bool result;
				this.ReadBoolean(out result);
				return result;
			});
			dictionary.Add(typeof(float), delegate()
			{
				float result;
				this.ReadSingle(out result);
				return result;
			});
			dictionary.Add(typeof(double), delegate()
			{
				double result;
				this.ReadDouble(out result);
				return result;
			});
			dictionary.Add(typeof(Guid), delegate()
			{
				Guid result;
				this.ReadGuid(out result);
				return result;
			});
			this.primitiveArrayReaders = dictionary;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00008B4F File Offset: 0x00006D4F
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00008B57 File Offset: 0x00006D57
		public override Stream Stream
		{
			get
			{
				return base.Stream;
			}
			set
			{
				base.Stream = value;
				this.reader = new JsonTextReader(base.Stream, base.Context);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008B77 File Offset: 0x00006D77
		public override void Dispose()
		{
			this.reader.Dispose();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008B84 File Offset: 0x00006D84
		public override EntryType PeekEntry(out string name)
		{
			if (this.peekedEntryType != null)
			{
				name = this.peekedEntryName;
				return this.peekedEntryType.Value;
			}
			EntryType entryType;
			this.reader.ReadToNextEntry(out name, out this.peekedEntryContent, out entryType);
			this.peekedEntryName = name;
			this.peekedEntryType = new EntryType?(entryType);
			return entryType;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00008BDC File Offset: 0x00006DDC
		public override bool EnterNode(out Type type)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.StartOfNode;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				string name = this.peekedEntryName;
				int id = -1;
				this.ReadToNextEntry();
				if (this.peekedEntryName == "$id")
				{
					if (!int.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref id))
					{
						base.Context.Config.DebugContext.LogError("Failed to parse id: " + this.peekedEntryContent);
						id = -1;
					}
					this.ReadToNextEntry();
				}
				if (this.peekedEntryName == "$type" && this.peekedEntryContent != null && this.peekedEntryContent.Length > 0)
				{
					entryType = this.peekedEntryType;
					entryType2 = EntryType.Integer;
					if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
					{
						int num;
						if (this.ReadInt32(out num))
						{
							if (!this.seenTypes.TryGetValue(num, ref type))
							{
								base.Context.Config.DebugContext.LogError("Missing type id for node with reference id " + id.ToString() + ": " + num.ToString());
							}
						}
						else
						{
							base.Context.Config.DebugContext.LogError("Failed to read type id for node with reference id " + id.ToString());
							type = null;
						}
					}
					else
					{
						int num2 = 1;
						int num3 = -1;
						int num4 = this.peekedEntryContent.IndexOf('|');
						if (num4 >= 0)
						{
							num2 = num4 + 1;
							string text = this.peekedEntryContent.Substring(1, num4 - 1);
							if (!int.TryParse(text, 511, CultureInfo.InvariantCulture, ref num3))
							{
								num3 = -1;
							}
						}
						type = base.Context.Binder.BindToType(this.peekedEntryContent.Substring(num2, this.peekedEntryContent.Length - (1 + num2)), base.Context.Config.DebugContext);
						if (num3 >= 0)
						{
							this.seenTypes[num3] = type;
						}
						this.peekedEntryType = default(EntryType?);
					}
				}
				else
				{
					type = null;
				}
				base.PushNode(name, id, type);
				return true;
			}
			this.SkipEntry();
			type = null;
			return false;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008E10 File Offset: 0x00007010
		public override bool ExitNode()
		{
			this.PeekEntry();
			EntryType? entryType;
			EntryType entryType2;
			for (;;)
			{
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfNode;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					break;
				}
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfStream;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					break;
				}
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfArray;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					base.Context.Config.DebugContext.LogError("Data layout mismatch; skipping past array boundary when exiting node.");
					this.peekedEntryType = default(EntryType?);
				}
				this.SkipEntry();
			}
			entryType = this.peekedEntryType;
			entryType2 = EntryType.EndOfNode;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				this.peekedEntryType = default(EntryType?);
				base.PopNode(base.CurrentNodeName);
				return true;
			}
			return false;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008EE8 File Offset: 0x000070E8
		public override bool EnterArray(out long length)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.StartOfArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				this.SkipEntry();
				length = 0L;
				return false;
			}
			base.PushArray();
			if (this.peekedEntryName != "$rlength")
			{
				base.Context.Config.DebugContext.LogError("Array entry wasn't preceded by an array length entry!");
				length = 0L;
				return true;
			}
			int num;
			if (!int.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref num))
			{
				base.Context.Config.DebugContext.LogError("Failed to parse array length: " + this.peekedEntryContent);
				length = 0L;
				return true;
			}
			length = (long)num;
			this.ReadToNextEntry();
			if (this.peekedEntryName != "$rcontent")
			{
				base.Context.Config.DebugContext.LogError("Failed to find regular array content entry after array length entry!");
				length = 0L;
				return true;
			}
			this.peekedEntryType = default(EntryType?);
			return true;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008FF0 File Offset: 0x000071F0
		public override bool ExitArray()
		{
			this.PeekEntry();
			EntryType? entryType;
			EntryType entryType2;
			for (;;)
			{
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfArray;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					break;
				}
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfStream;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					break;
				}
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfNode;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					base.Context.Config.DebugContext.LogError("Data layout mismatch; skipping past node boundary when exiting array.");
					this.peekedEntryType = default(EntryType?);
				}
				this.SkipEntry();
			}
			entryType = this.peekedEntryType;
			entryType2 = EntryType.EndOfArray;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				this.peekedEntryType = default(EntryType?);
				base.PopArray();
				return true;
			}
			return false;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000090C4 File Offset: 0x000072C4
		public override bool ReadPrimitiveArray<T>(out T[] array)
		{
			if (!FormatterUtilities.IsPrimitiveArrayType(typeof(T)))
			{
				throw new ArgumentException("Type " + typeof(T).Name + " is not a valid primitive array type.");
			}
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.PrimitiveArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				this.SkipEntry();
				array = null;
				return false;
			}
			base.PushArray();
			if (this.peekedEntryName != "$plength")
			{
				base.Context.Config.DebugContext.LogError("Array entry wasn't preceded by an array length entry!");
				array = null;
				return false;
			}
			int num;
			if (!int.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref num))
			{
				base.Context.Config.DebugContext.LogError("Failed to parse array length: " + this.peekedEntryContent);
				array = null;
				return false;
			}
			this.ReadToNextEntry();
			if (this.peekedEntryName != "$pcontent")
			{
				base.Context.Config.DebugContext.LogError("Failed to find primitive array content entry after array length entry!");
				array = null;
				return false;
			}
			this.peekedEntryType = default(EntryType?);
			Func<T> func = (Func<T>)this.primitiveArrayReaders[typeof(T)];
			array = new T[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = func.Invoke();
			}
			this.ExitArray();
			return true;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00009244 File Offset: 0x00007444
		public override bool ReadBoolean(out bool value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Boolean;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					value = (this.peekedEntryContent == "true");
					return true;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			value = false;
			return false;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000092B0 File Offset: 0x000074B0
		public override bool ReadInternalReference(out int id)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.InternalReference;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					return this.ReadAnyIntReference(out id);
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			id = -1;
			return false;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00009310 File Offset: 0x00007510
		public override bool ReadExternalReference(out int index)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.ExternalReferenceByIndex;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					return this.ReadAnyIntReference(out index);
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			index = -1;
			return false;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009370 File Offset: 0x00007570
		public override bool ReadExternalReference(out Guid guid)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.ExternalReferenceByGuid;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				string text = this.peekedEntryContent;
				if (text.StartsWith("$guidref"))
				{
					text = text.Substring("$guidref".Length + 1);
				}
				try
				{
					guid = new Guid(text);
					return true;
				}
				catch (FormatException)
				{
					guid = Guid.Empty;
					return false;
				}
				catch (OverflowException)
				{
					guid = Guid.Empty;
					return false;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			guid = Guid.Empty;
			return false;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009440 File Offset: 0x00007640
		public override bool ReadExternalReference(out string id)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.ExternalReferenceByString;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				id = this.peekedEntryContent;
				if (id.StartsWith("$strref"))
				{
					id = id.Substring("$strref".Length + 1);
				}
				else if (id.StartsWith("$fstrref"))
				{
					id = id.Substring("$fstrref".Length + 2, id.Length - ("$fstrref".Length + 3));
				}
				this.MarkEntryConsumed();
				return true;
			}
			this.SkipEntry();
			id = null;
			return false;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000094E8 File Offset: 0x000076E8
		public override bool ReadChar(out char value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.String;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					value = this.peekedEntryContent.get_Chars(1);
					return true;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			value = '\0';
			return false;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009550 File Offset: 0x00007750
		public override bool ReadString(out string value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.String;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					value = this.peekedEntryContent.Substring(1, this.peekedEntryContent.Length - 2);
					return true;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			value = null;
			return false;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000095C4 File Offset: 0x000077C4
		public override bool ReadGuid(out Guid value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Guid;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					value = new Guid(this.peekedEntryContent);
					return true;
				}
				catch (FormatException)
				{
					value = Guid.Empty;
					return false;
				}
				catch (OverflowException)
				{
					value = Guid.Empty;
					return false;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			value = Guid.Empty;
			return false;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009670 File Offset: 0x00007870
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

		// Token: 0x06000158 RID: 344 RVA: 0x000096AC File Offset: 0x000078AC
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

		// Token: 0x06000159 RID: 345 RVA: 0x000096E8 File Offset: 0x000078E8
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

		// Token: 0x0600015A RID: 346 RVA: 0x00009724 File Offset: 0x00007924
		public override bool ReadInt64(out long value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Integer;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (long.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref value))
					{
						return true;
					}
					base.Context.Config.DebugContext.LogError("Failed to parse long from: " + this.peekedEntryContent);
					return false;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			value = 0L;
			return false;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000097C0 File Offset: 0x000079C0
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

		// Token: 0x0600015C RID: 348 RVA: 0x000097FC File Offset: 0x000079FC
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

		// Token: 0x0600015D RID: 349 RVA: 0x00009838 File Offset: 0x00007A38
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

		// Token: 0x0600015E RID: 350 RVA: 0x00009874 File Offset: 0x00007A74
		public override bool ReadUInt64(out ulong value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Integer;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (ulong.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref value))
					{
						return true;
					}
					base.Context.Config.DebugContext.LogError("Failed to parse ulong from: " + this.peekedEntryContent);
					return false;
				}
				finally
				{
					this.MarkEntryConsumed();
				}
			}
			this.SkipEntry();
			value = 0UL;
			return false;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00009910 File Offset: 0x00007B10
		public override bool ReadDecimal(out decimal value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.FloatingPoint;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				entryType = this.peekedEntryType;
				entryType2 = EntryType.Integer;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					goto IL_8E;
				}
			}
			try
			{
				if (decimal.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref value))
				{
					return true;
				}
				base.Context.Config.DebugContext.LogError("Failed to parse decimal from: " + this.peekedEntryContent);
				return false;
			}
			finally
			{
				this.MarkEntryConsumed();
			}
			IL_8E:
			this.SkipEntry();
			value = default(decimal);
			return false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000099CC File Offset: 0x00007BCC
		public override bool ReadSingle(out float value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.FloatingPoint;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				entryType = this.peekedEntryType;
				entryType2 = EntryType.Integer;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					goto IL_8E;
				}
			}
			try
			{
				if (float.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref value))
				{
					return true;
				}
				base.Context.Config.DebugContext.LogError("Failed to parse float from: " + this.peekedEntryContent);
				return false;
			}
			finally
			{
				this.MarkEntryConsumed();
			}
			IL_8E:
			this.SkipEntry();
			value = 0f;
			return false;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009A88 File Offset: 0x00007C88
		public override bool ReadDouble(out double value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.FloatingPoint;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				entryType = this.peekedEntryType;
				entryType2 = EntryType.Integer;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					goto IL_8E;
				}
			}
			try
			{
				if (double.TryParse(this.peekedEntryContent, 511, CultureInfo.InvariantCulture, ref value))
				{
					return true;
				}
				base.Context.Config.DebugContext.LogError("Failed to parse double from: " + this.peekedEntryContent);
				return false;
			}
			finally
			{
				this.MarkEntryConsumed();
			}
			IL_8E:
			this.SkipEntry();
			value = 0.0;
			return false;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009B48 File Offset: 0x00007D48
		public override bool ReadNull()
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Null;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				this.MarkEntryConsumed();
				return true;
			}
			this.SkipEntry();
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009B88 File Offset: 0x00007D88
		public override void PrepareNewSerializationSession()
		{
			base.PrepareNewSerializationSession();
			this.peekedEntryType = default(EntryType?);
			this.peekedEntryContent = null;
			this.peekedEntryName = null;
			this.seenTypes.Clear();
			this.reader.Reset();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009BC0 File Offset: 0x00007DC0
		public override string GetDataDump()
		{
			if (!this.Stream.CanSeek)
			{
				return "Json data stream cannot seek; cannot dump data.";
			}
			long position = this.Stream.Position;
			byte[] array = new byte[this.Stream.Length];
			this.Stream.Position = 0L;
			this.Stream.Read(array, 0, array.Length);
			this.Stream.Position = position;
			return "Json: " + Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009C44 File Offset: 0x00007E44
		protected override EntryType PeekEntry()
		{
			string text;
			return this.PeekEntry(out text);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009C5C File Offset: 0x00007E5C
		protected override EntryType ReadToNextEntry()
		{
			this.peekedEntryType = default(EntryType?);
			string text;
			return this.PeekEntry(out text);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009C80 File Offset: 0x00007E80
		private void MarkEntryConsumed()
		{
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.EndOfArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				entryType = this.peekedEntryType;
				entryType2 = EntryType.EndOfNode;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					this.peekedEntryType = default(EntryType?);
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009CD4 File Offset: 0x00007ED4
		private bool ReadAnyIntReference(out int value)
		{
			int num = -1;
			for (int i = 0; i < this.peekedEntryContent.Length; i++)
			{
				if (this.peekedEntryContent.get_Chars(i) == ':')
				{
					num = i;
					break;
				}
			}
			if (num == -1 || num == this.peekedEntryContent.Length - 1)
			{
				base.Context.Config.DebugContext.LogError("Failed to parse id from: " + this.peekedEntryContent);
			}
			string text = this.peekedEntryContent.Substring(num + 1);
			if (int.TryParse(text, 511, CultureInfo.InvariantCulture, ref value))
			{
				return true;
			}
			base.Context.Config.DebugContext.LogError("Failed to parse id: " + text);
			value = -1;
			return false;
		}

		// Token: 0x04000060 RID: 96
		private JsonTextReader reader;

		// Token: 0x04000061 RID: 97
		private EntryType? peekedEntryType;

		// Token: 0x04000062 RID: 98
		private string peekedEntryName;

		// Token: 0x04000063 RID: 99
		private string peekedEntryContent;

		// Token: 0x04000064 RID: 100
		private Dictionary<int, Type> seenTypes = new Dictionary<int, Type>(16);

		// Token: 0x04000065 RID: 101
		private readonly Dictionary<Type, Delegate> primitiveArrayReaders;
	}
}
