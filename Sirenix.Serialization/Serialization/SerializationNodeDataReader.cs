using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Sirenix.Serialization
{
	// Token: 0x02000016 RID: 22
	public class SerializationNodeDataReader : BaseDataReader
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x0000BC98 File Offset: 0x00009E98
		public SerializationNodeDataReader(DeserializationContext context) : base(null, context)
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
			this.primitiveTypeReaders = dictionary;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000BE49 File Offset: 0x0000A049
		private bool IndexIsValid
		{
			get
			{
				return this.nodes != null && this.currentIndex >= 0 && this.currentIndex < this.nodes.Count;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000BE71 File Offset: 0x0000A071
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000BE8C File Offset: 0x0000A08C
		public List<SerializationNode> Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new List<SerializationNode>();
				}
				return this.nodes;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.nodes = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000BE9E File Offset: 0x0000A09E
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000BE9E File Offset: 0x0000A09E
		public override Stream Stream
		{
			get
			{
				throw new NotSupportedException("This data reader has no stream.");
			}
			set
			{
				throw new NotSupportedException("This data reader has no stream.");
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000BEAA File Offset: 0x0000A0AA
		public override void Dispose()
		{
			this.nodes = null;
			this.currentIndex = -1;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000BEBA File Offset: 0x0000A0BA
		public override void PrepareNewSerializationSession()
		{
			base.PrepareNewSerializationSession();
			this.currentIndex = -1;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public override EntryType PeekEntry(out string name)
		{
			if (this.peekedEntryType != null)
			{
				name = this.peekedEntryName;
				return this.peekedEntryType.Value;
			}
			this.currentIndex++;
			if (this.IndexIsValid)
			{
				SerializationNode serializationNode = this.nodes[this.currentIndex];
				this.peekedEntryName = serializationNode.Name;
				this.peekedEntryType = new EntryType?(serializationNode.Entry);
				this.peekedEntryData = serializationNode.Data;
			}
			else
			{
				this.peekedEntryName = null;
				this.peekedEntryType = new EntryType?(EntryType.EndOfStream);
				this.peekedEntryData = null;
			}
			name = this.peekedEntryName;
			return this.peekedEntryType.Value;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BF7C File Offset: 0x0000A17C
		public override bool EnterArray(out long length)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.StartOfArray;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				base.PushArray();
				if (!long.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref length))
				{
					length = 0L;
					base.Context.Config.DebugContext.LogError("Failed to parse array length from data '" + this.peekedEntryData + "'.");
				}
				this.ConsumeCurrentEntry();
				return true;
			}
			this.SkipEntry();
			length = 0L;
			return false;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000C010 File Offset: 0x0000A210
		public override bool EnterNode(out Type type)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.StartOfNode;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				string text = this.peekedEntryData;
				int id = -1;
				type = null;
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = null;
					int num = text.IndexOf("|", 2);
					int num2;
					if (num >= 0)
					{
						text2 = text.Substring(num + 1);
						string text3 = text.Substring(0, num);
						if (int.TryParse(text3, 511, CultureInfo.InvariantCulture, ref num2))
						{
							id = num2;
						}
						else
						{
							base.Context.Config.DebugContext.LogError(string.Concat(new string[]
							{
								"Failed to parse id string '",
								text3,
								"' from data '",
								text,
								"'."
							}));
						}
					}
					else if (int.TryParse(text, ref num2))
					{
						id = num2;
					}
					else
					{
						text2 = text;
					}
					if (text2 != null)
					{
						type = base.Context.Binder.BindToType(text2, base.Context.Config.DebugContext);
					}
				}
				this.ConsumeCurrentEntry();
				base.PushNode(this.peekedEntryName, id, type);
				return true;
			}
			this.SkipEntry();
			type = null;
			return false;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000C140 File Offset: 0x0000A340
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
					this.ConsumeCurrentEntry();
				}
				this.SkipEntry();
			}
			entryType = this.peekedEntryType;
			entryType2 = EntryType.EndOfArray;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				this.ConsumeCurrentEntry();
				base.PopArray();
				return true;
			}
			return false;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000C204 File Offset: 0x0000A404
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
					this.ConsumeCurrentEntry();
				}
				this.SkipEntry();
			}
			entryType = this.peekedEntryType;
			entryType2 = EntryType.EndOfNode;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				this.ConsumeCurrentEntry();
				base.PopNode(base.CurrentNodeName);
				return true;
			}
			return false;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		public override bool ReadBoolean(out bool value)
		{
			this.PeekEntry();
			bool result;
			try
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.Boolean;
				if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
				{
					value = (this.peekedEntryData == "true");
					result = true;
				}
				else
				{
					value = false;
					result = false;
				}
			}
			finally
			{
				this.ConsumeCurrentEntry();
			}
			return result;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000C334 File Offset: 0x0000A534
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

		// Token: 0x060001BF RID: 447 RVA: 0x0000C370 File Offset: 0x0000A570
		public override bool ReadChar(out char value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.String;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (this.peekedEntryData.Length == 1)
					{
						value = this.peekedEntryData.get_Chars(0);
						return true;
					}
					base.Context.Config.DebugContext.LogWarning("Expected string of length 1 for char entry.");
					value = '\0';
					return false;
				}
				finally
				{
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			value = '\0';
			return false;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C408 File Offset: 0x0000A608
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
					goto IL_93;
				}
			}
			try
			{
				if (!decimal.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref value))
				{
					base.Context.Config.DebugContext.LogError("Failed to parse decimal value from entry data '" + this.peekedEntryData + "'.");
					return false;
				}
				return true;
			}
			finally
			{
				this.ConsumeCurrentEntry();
			}
			IL_93:
			this.SkipEntry();
			value = default(decimal);
			return false;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
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
					goto IL_93;
				}
			}
			try
			{
				if (!double.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref value))
				{
					base.Context.Config.DebugContext.LogError("Failed to parse double value from entry data '" + this.peekedEntryData + "'.");
					return false;
				}
				return true;
			}
			finally
			{
				this.ConsumeCurrentEntry();
			}
			IL_93:
			this.SkipEntry();
			value = 0.0;
			return false;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C58C File Offset: 0x0000A78C
		public override bool ReadExternalReference(out Guid guid)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.ExternalReferenceByGuid;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if ((guid = new Guid(this.peekedEntryData)) != Guid.Empty)
					{
						return true;
					}
					guid = Guid.Empty;
					return false;
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
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			guid = Guid.Empty;
			return false;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C654 File Offset: 0x0000A854
		public override bool ReadExternalReference(out string id)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.ExternalReferenceByString;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				id = this.peekedEntryData;
				this.ConsumeCurrentEntry();
				return true;
			}
			this.SkipEntry();
			id = null;
			return false;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
		public override bool ReadExternalReference(out int index)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.ExternalReferenceByIndex;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (!int.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref index))
					{
						base.Context.Config.DebugContext.LogError("Failed to parse external index reference integer value from entry data '" + this.peekedEntryData + "'.");
						return false;
					}
					return true;
				}
				finally
				{
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			index = 0;
			return false;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000C740 File Offset: 0x0000A940
		public override bool ReadGuid(out Guid value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Guid;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if ((value = new Guid(this.peekedEntryData)) != Guid.Empty)
					{
						return true;
					}
					value = Guid.Empty;
					return false;
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
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			value = Guid.Empty;
			return false;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C808 File Offset: 0x0000AA08
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

		// Token: 0x060001C7 RID: 455 RVA: 0x0000C844 File Offset: 0x0000AA44
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

		// Token: 0x060001C8 RID: 456 RVA: 0x0000C880 File Offset: 0x0000AA80
		public override bool ReadInt64(out long value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Integer;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (!long.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref value))
					{
						base.Context.Config.DebugContext.LogError("Failed to parse integer value from entry data '" + this.peekedEntryData + "'.");
						return false;
					}
					return true;
				}
				finally
				{
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			value = 0L;
			return false;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C920 File Offset: 0x0000AB20
		public override bool ReadInternalReference(out int id)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.InternalReference;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (!int.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref id))
					{
						base.Context.Config.DebugContext.LogError("Failed to parse internal reference id integer value from entry data '" + this.peekedEntryData + "'.");
						return false;
					}
					return true;
				}
				finally
				{
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			id = 0;
			return false;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
		public override bool ReadNull()
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Null;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				this.ConsumeCurrentEntry();
				return true;
			}
			this.SkipEntry();
			return false;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000CA00 File Offset: 0x0000AC00
		public override bool ReadPrimitiveArray<T>(out T[] array)
		{
			if (!FormatterUtilities.IsPrimitiveArrayType(typeof(T)))
			{
				throw new ArgumentException("Type " + typeof(T).Name + " is not a valid primitive array type.");
			}
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.PrimitiveArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				this.SkipEntry();
				array = null;
				return false;
			}
			if (typeof(T) == typeof(byte))
			{
				array = (T[])ProperBitConverter.HexStringToBytes(this.peekedEntryData);
				return true;
			}
			this.PeekEntry();
			entryType = this.peekedEntryType;
			entryType2 = EntryType.PrimitiveArray;
			if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
			{
				DebugContext debugContext = base.Context.Config.DebugContext;
				string[] array2 = new string[5];
				array2[0] = "Expected entry of type '";
				array2[1] = EntryType.StartOfArray.ToString();
				array2[2] = "' when reading primitive array but got entry of type '";
				int num = 3;
				entryType = this.peekedEntryType;
				array2[num] = entryType.ToString();
				array2[4] = "'.";
				debugContext.LogError(string.Concat(array2));
				this.SkipEntry();
				array = new T[0];
				return false;
			}
			long num2;
			if (!long.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref num2))
			{
				base.Context.Config.DebugContext.LogError("Failed to parse primitive array length from entry data '" + this.peekedEntryData + "'.");
				this.SkipEntry();
				array = new T[0];
				return false;
			}
			this.ConsumeCurrentEntry();
			base.PushArray();
			array = new T[num2];
			Func<T> func = (Func<T>)this.primitiveTypeReaders[typeof(T)];
			int num3 = 0;
			while ((long)num3 < num2)
			{
				array[num3] = func.Invoke();
				num3++;
			}
			this.ExitArray();
			return true;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000CBDC File Offset: 0x0000ADDC
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

		// Token: 0x060001CD RID: 461 RVA: 0x0000CC18 File Offset: 0x0000AE18
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
					goto IL_93;
				}
			}
			try
			{
				if (!float.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref value))
				{
					base.Context.Config.DebugContext.LogError("Failed to parse float value from entry data '" + this.peekedEntryData + "'.");
					return false;
				}
				return true;
			}
			finally
			{
				this.ConsumeCurrentEntry();
			}
			IL_93:
			this.SkipEntry();
			value = 0f;
			return false;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		public override bool ReadString(out string value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.String;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				value = this.peekedEntryData;
				this.ConsumeCurrentEntry();
				return true;
			}
			this.SkipEntry();
			value = null;
			return false;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000CD24 File Offset: 0x0000AF24
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

		// Token: 0x060001D0 RID: 464 RVA: 0x0000CD60 File Offset: 0x0000AF60
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

		// Token: 0x060001D1 RID: 465 RVA: 0x0000CD9C File Offset: 0x0000AF9C
		public override bool ReadUInt64(out ulong value)
		{
			this.PeekEntry();
			EntryType? entryType = this.peekedEntryType;
			EntryType entryType2 = EntryType.Integer;
			if (entryType.GetValueOrDefault() == entryType2 & entryType != null)
			{
				try
				{
					if (!ulong.TryParse(this.peekedEntryData, 511, CultureInfo.InvariantCulture, ref value))
					{
						base.Context.Config.DebugContext.LogError("Failed to parse integer value from entry data '" + this.peekedEntryData + "'.");
						return false;
					}
					return true;
				}
				finally
				{
					this.ConsumeCurrentEntry();
				}
			}
			this.SkipEntry();
			value = 0UL;
			return false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000CE3C File Offset: 0x0000B03C
		public override string GetDataDump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Nodes: \n\n");
			for (int i = 0; i < this.nodes.Count; i++)
			{
				SerializationNode serializationNode = this.nodes[i];
				stringBuilder.Append("    - Name: " + serializationNode.Name);
				if (i == this.currentIndex)
				{
					stringBuilder.AppendLine("    <<<< READ POSITION");
				}
				else
				{
					stringBuilder.AppendLine();
				}
				StringBuilder stringBuilder2 = stringBuilder;
				string text = "      Entry: ";
				int entry = (int)serializationNode.Entry;
				stringBuilder2.AppendLine(text + entry.ToString());
				stringBuilder.AppendLine("      Data: " + serializationNode.Data);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
		private void ConsumeCurrentEntry()
		{
			if (this.peekedEntryType != null)
			{
				EntryType? entryType = this.peekedEntryType;
				EntryType entryType2 = EntryType.EndOfStream;
				if (!(entryType.GetValueOrDefault() == entryType2 & entryType != null))
				{
					this.peekedEntryType = default(EntryType?);
				}
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000CF38 File Offset: 0x0000B138
		protected override EntryType PeekEntry()
		{
			string text;
			return this.PeekEntry(out text);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000CF50 File Offset: 0x0000B150
		protected override EntryType ReadToNextEntry()
		{
			this.ConsumeCurrentEntry();
			string text;
			return this.PeekEntry(out text);
		}

		// Token: 0x0400007C RID: 124
		private string peekedEntryName;

		// Token: 0x0400007D RID: 125
		private EntryType? peekedEntryType;

		// Token: 0x0400007E RID: 126
		private string peekedEntryData;

		// Token: 0x0400007F RID: 127
		private int currentIndex = -1;

		// Token: 0x04000080 RID: 128
		private List<SerializationNode> nodes;

		// Token: 0x04000081 RID: 129
		private Dictionary<Type, Delegate> primitiveTypeReaders;
	}
}
