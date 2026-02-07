using System;
using System.Collections.Generic;
using System.IO;

namespace Sirenix.Serialization
{
	// Token: 0x02000014 RID: 20
	public class JsonTextReader : IDisposable
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000AEEA File Offset: 0x000090EA
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000AEF2 File Offset: 0x000090F2
		public DeserializationContext Context { get; private set; }

		// Token: 0x060001A2 RID: 418 RVA: 0x0000AEFC File Offset: 0x000090FC
		public JsonTextReader(Stream stream, DeserializationContext context)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("Cannot read from stream");
			}
			this.reader = new StreamReader(stream);
			this.Context = context;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000AF61 File Offset: 0x00009161
		public void Reset()
		{
			this.peekedChar = default(char?);
			if (this.emergencyPlayback != null)
			{
				this.emergencyPlayback.Clear();
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000021B8 File Offset: 0x000003B8
		public void Dispose()
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000AF84 File Offset: 0x00009184
		public void ReadToNextEntry(out string name, out string valueContent, out EntryType entry)
		{
			int num = -1;
			bool flag = false;
			this.bufferIndex = -1;
			while (!this.reader.EndOfStream)
			{
				char c = this.PeekChar();
				if (flag)
				{
					char? c2 = this.lastReadChar;
					int? num2 = (c2 != null) ? new int?((int)c2.GetValueOrDefault()) : default(int?);
					int num3 = 92;
					if (num2.GetValueOrDefault() == num3 & num2 != null)
					{
						if (c == '\\')
						{
							this.lastReadChar = default(char?);
							this.SkipChar();
							continue;
						}
						if (c <= 'b')
						{
							if (c != '0' && c != 'a' && c != 'b')
							{
								goto IL_28C;
							}
						}
						else if (c != 'f' && c != 'n')
						{
							switch (c)
							{
							case 'r':
							case 't':
								break;
							case 's':
								goto IL_28C;
							case 'u':
							{
								this.SkipChar();
								char c3 = this.ConsumeChar();
								char c4 = this.ConsumeChar();
								char c5 = this.ConsumeChar();
								char c6 = this.ConsumeChar();
								if (this.IsHex(c3) && this.IsHex(c4) && this.IsHex(c5) && this.IsHex(c6))
								{
									c = this.ParseHexChar(c3, c4, c5, c6);
									this.lastReadChar = new char?(c);
									this.buffer[this.bufferIndex] = c;
									continue;
								}
								this.Context.Config.DebugContext.LogError(string.Concat(new string[]
								{
									"A wild non-hex value appears at position ",
									this.reader.BaseStream.Position.ToString(),
									"! \\-u-",
									c3.ToString(),
									"-",
									c4.ToString(),
									"-",
									c5.ToString(),
									"-",
									c6.ToString(),
									"; current buffer: '",
									new string(this.buffer, 0, this.bufferIndex + 1),
									"'. If the error handling policy is resilient, an attempt will be made to recover from this emergency without a fatal parse error..."
								}));
								this.lastReadChar = default(char?);
								if (this.emergencyPlayback == null)
								{
									this.emergencyPlayback = new Queue<char>(5);
								}
								this.emergencyPlayback.Enqueue('u');
								this.emergencyPlayback.Enqueue(c3);
								this.emergencyPlayback.Enqueue(c4);
								this.emergencyPlayback.Enqueue(c5);
								this.emergencyPlayback.Enqueue(c6);
								continue;
							}
							default:
								goto IL_28C;
							}
						}
						c = JsonTextReader.UnescapeDictionary[c];
						this.lastReadChar = new char?(c);
						this.buffer[this.bufferIndex] = c;
						this.SkipChar();
						continue;
					}
				}
				IL_28C:
				if (!flag && c == ':' && num == -1)
				{
					num = this.bufferIndex + 1;
				}
				EntryType? entryType;
				if (c == '"')
				{
					if (flag)
					{
						char? c2 = this.lastReadChar;
						int? num2 = (c2 != null) ? new int?((int)c2.GetValueOrDefault()) : default(int?);
						int num3 = 92;
						if (num2.GetValueOrDefault() == num3 & num2 != null)
						{
							this.lastReadChar = new char?('"');
							this.buffer[this.bufferIndex] = '"';
							this.SkipChar();
							continue;
						}
					}
					this.ReadCharIntoBuffer();
					flag = !flag;
				}
				else if (flag)
				{
					this.ReadCharIntoBuffer();
				}
				else if (char.IsWhiteSpace(c))
				{
					this.SkipChar();
				}
				else if (JsonTextReader.EntryDelineators.TryGetValue(c, ref entryType))
				{
					if (entryType != null)
					{
						entry = entryType.Value;
						EntryType entryType2 = entry;
						if (entryType2 <= EntryType.EndOfNode)
						{
							if (entryType2 == EntryType.StartOfNode)
							{
								this.ConsumeChar();
								EntryType entryType3;
								this.ParseEntryFromBuffer(out name, out valueContent, out entryType3, num, new EntryType?(EntryType.StartOfNode));
								return;
							}
							if (entryType2 == EntryType.EndOfNode)
							{
								if (this.bufferIndex == -1)
								{
									this.ConsumeChar();
									name = null;
									valueContent = null;
									return;
								}
								this.ParseEntryFromBuffer(out name, out valueContent, out entry, num, default(EntryType?));
								return;
							}
						}
						else if (entryType2 != EntryType.EndOfArray)
						{
							if (entryType2 == EntryType.PrimitiveArray)
							{
								this.ConsumeChar();
								EntryType entryType4;
								this.ParseEntryFromBuffer(out name, out valueContent, out entryType4, num, new EntryType?(EntryType.PrimitiveArray));
								return;
							}
						}
						else
						{
							if (this.bufferIndex == -1)
							{
								this.ConsumeChar();
								name = null;
								valueContent = null;
								return;
							}
							this.ParseEntryFromBuffer(out name, out valueContent, out entry, num, default(EntryType?));
							return;
						}
						throw new NotImplementedException();
					}
					this.SkipChar();
					if (this.bufferIndex != -1)
					{
						this.ParseEntryFromBuffer(out name, out valueContent, out entry, num, default(EntryType?));
						return;
					}
				}
				else
				{
					this.ReadCharIntoBuffer();
				}
			}
			if (this.bufferIndex == -1)
			{
				name = null;
				valueContent = null;
				entry = EntryType.EndOfStream;
				return;
			}
			this.ParseEntryFromBuffer(out name, out valueContent, out entry, num, new EntryType?(EntryType.EndOfStream));
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000B41C File Offset: 0x0000961C
		private void ParseEntryFromBuffer(out string name, out string valueContent, out EntryType entry, int valueSeparatorIndex, EntryType? hintEntry)
		{
			EntryType? entryType2;
			EntryType entryType3;
			if (this.bufferIndex >= 0)
			{
				if (valueSeparatorIndex == -1)
				{
					if (hintEntry != null)
					{
						name = null;
						valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
						entry = hintEntry.Value;
						return;
					}
					name = null;
					valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
					EntryType? entryType = this.GuessPrimitiveType(valueContent);
					if (entryType != null)
					{
						entry = entryType.Value;
						return;
					}
					entry = EntryType.Invalid;
					return;
				}
				else
				{
					if (this.buffer[0] == '"')
					{
						name = new string(this.buffer, 1, valueSeparatorIndex - 2);
					}
					else
					{
						name = new string(this.buffer, 0, valueSeparatorIndex);
					}
					if (StringComparer.Ordinal.Equals(name, "$rcontent"))
					{
						entryType2 = hintEntry;
						entryType3 = EntryType.StartOfArray;
						if (entryType2.GetValueOrDefault() == entryType3 & entryType2 != null)
						{
							valueContent = null;
							entry = EntryType.StartOfArray;
							return;
						}
					}
					if (StringComparer.Ordinal.Equals(name, "$pcontent"))
					{
						entryType2 = hintEntry;
						entryType3 = EntryType.StartOfArray;
						if (entryType2.GetValueOrDefault() == entryType3 & entryType2 != null)
						{
							valueContent = null;
							entry = EntryType.PrimitiveArray;
							return;
						}
					}
					if (StringComparer.Ordinal.Equals(name, "$iref"))
					{
						name = null;
						valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
						entry = EntryType.InternalReference;
						return;
					}
					if (StringComparer.Ordinal.Equals(name, "$eref"))
					{
						name = null;
						valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
						entry = EntryType.ExternalReferenceByIndex;
						return;
					}
					if (StringComparer.Ordinal.Equals(name, "$guidref"))
					{
						name = null;
						valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
						entry = EntryType.ExternalReferenceByGuid;
						return;
					}
					if (StringComparer.Ordinal.Equals(name, "$strref"))
					{
						name = null;
						valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
						entry = EntryType.ExternalReferenceByString;
						return;
					}
					if (StringComparer.Ordinal.Equals(name, "$fstrref"))
					{
						name = null;
						valueContent = new string(this.buffer, 0, this.bufferIndex + 1);
						entry = EntryType.ExternalReferenceByString;
						return;
					}
					if (this.bufferIndex >= valueSeparatorIndex)
					{
						valueContent = new string(this.buffer, valueSeparatorIndex + 1, this.bufferIndex - valueSeparatorIndex);
					}
					else
					{
						valueContent = null;
					}
					if (valueContent != null)
					{
						if (StringComparer.Ordinal.Equals(name, "$rlength"))
						{
							entry = EntryType.StartOfArray;
							return;
						}
						if (StringComparer.Ordinal.Equals(name, "$plength"))
						{
							entry = EntryType.PrimitiveArray;
							return;
						}
						if (valueContent.Length == 0 && hintEntry != null)
						{
							entry = hintEntry.Value;
							return;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(valueContent, "null"))
						{
							entry = EntryType.Null;
							return;
						}
						if (StringComparer.Ordinal.Equals(valueContent, "{"))
						{
							entry = EntryType.StartOfNode;
							return;
						}
						if (StringComparer.Ordinal.Equals(valueContent, "}"))
						{
							entry = EntryType.EndOfNode;
							return;
						}
						if (StringComparer.Ordinal.Equals(valueContent, "["))
						{
							entry = EntryType.StartOfArray;
							return;
						}
						if (StringComparer.Ordinal.Equals(valueContent, "]"))
						{
							entry = EntryType.EndOfArray;
							return;
						}
						if (valueContent.StartsWith("$iref", 4))
						{
							entry = EntryType.InternalReference;
							return;
						}
						if (valueContent.StartsWith("$eref", 4))
						{
							entry = EntryType.ExternalReferenceByIndex;
							return;
						}
						if (valueContent.StartsWith("$guidref", 4))
						{
							entry = EntryType.ExternalReferenceByGuid;
							return;
						}
						if (valueContent.StartsWith("$strref", 4))
						{
							entry = EntryType.ExternalReferenceByString;
							return;
						}
						if (valueContent.StartsWith("$fstrref", 4))
						{
							entry = EntryType.ExternalReferenceByString;
							return;
						}
						EntryType? entryType4 = this.GuessPrimitiveType(valueContent);
						if (entryType4 != null)
						{
							entry = entryType4.Value;
							return;
						}
					}
				}
			}
			if (hintEntry != null)
			{
				name = null;
				valueContent = null;
				entry = hintEntry.Value;
				return;
			}
			if (this.bufferIndex == -1)
			{
				this.Context.Config.DebugContext.LogError("Failed to parse empty entry in the stream.");
			}
			else
			{
				this.Context.Config.DebugContext.LogError("Tried and failed to parse entry with content '" + new string(this.buffer, 0, this.bufferIndex + 1) + "'.");
			}
			entryType2 = hintEntry;
			entryType3 = EntryType.EndOfStream;
			if (entryType2.GetValueOrDefault() == entryType3 & entryType2 != null)
			{
				name = null;
				valueContent = null;
				entry = EntryType.EndOfStream;
				return;
			}
			name = null;
			valueContent = null;
			entry = EntryType.Invalid;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B843 File Offset: 0x00009A43
		private bool IsHex(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B86C File Offset: 0x00009A6C
		private uint ParseSingleChar(char c, uint multiplier)
		{
			uint result = 0U;
			if (c >= '0' && c <= '9')
			{
				result = (uint)(c - '0') * multiplier;
			}
			else if (c >= 'A' && c <= 'F')
			{
				result = (uint)(c - 'A' + '\n') * multiplier;
			}
			else if (c >= 'a' && c <= 'f')
			{
				result = (uint)(c - 'a' + '\n') * multiplier;
			}
			return result;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B8BC File Offset: 0x00009ABC
		private char ParseHexChar(char c1, char c2, char c3, char c4)
		{
			uint num = this.ParseSingleChar(c1, 4096U);
			uint num2 = this.ParseSingleChar(c2, 256U);
			uint num3 = this.ParseSingleChar(c3, 16U);
			uint num4 = this.ParseSingleChar(c4, 1U);
			char result;
			try
			{
				result = (char)(num + num2 + num3 + num4);
			}
			catch (Exception)
			{
				this.Context.Config.DebugContext.LogError(string.Concat(new string[]
				{
					"Could not parse invalid hex values: ",
					c1.ToString(),
					c2.ToString(),
					c3.ToString(),
					c4.ToString()
				}));
				result = ' ';
			}
			return result;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000B96C File Offset: 0x00009B6C
		private char ReadCharIntoBuffer()
		{
			this.bufferIndex++;
			if (this.bufferIndex >= this.buffer.Length - 1)
			{
				char[] array = new char[this.buffer.Length * 2];
				Buffer.BlockCopy(this.buffer, 0, array, 0, this.buffer.Length * 2);
				this.buffer = array;
			}
			char c = this.ConsumeChar();
			this.buffer[this.bufferIndex] = c;
			this.lastReadChar = new char?(c);
			return c;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000B9EC File Offset: 0x00009BEC
		private EntryType? GuessPrimitiveType(string content)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(content, "null"))
			{
				return new EntryType?(EntryType.Null);
			}
			if (content.Length >= 2 && content.get_Chars(0) == '"' && content.get_Chars(content.Length - 1) == '"')
			{
				return new EntryType?(EntryType.String);
			}
			if (content.Length == 36 && content.LastIndexOf('-') > 0)
			{
				return new EntryType?(EntryType.Guid);
			}
			if (content.Contains(".") || content.Contains(","))
			{
				return new EntryType?(EntryType.FloatingPoint);
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(content, "true") || StringComparer.OrdinalIgnoreCase.Equals(content, "false"))
			{
				return new EntryType?(EntryType.Boolean);
			}
			if (content.Length >= 1)
			{
				return new EntryType?(EntryType.Integer);
			}
			return default(EntryType?);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		private char PeekChar()
		{
			if (this.peekedChar == null)
			{
				if (this.emergencyPlayback != null && this.emergencyPlayback.Count > 0)
				{
					this.peekedChar = new char?(this.emergencyPlayback.Dequeue());
				}
				else
				{
					this.peekedChar = new char?((char)this.reader.Read());
				}
			}
			return this.peekedChar.Value;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000BB2C File Offset: 0x00009D2C
		private void SkipChar()
		{
			if (this.peekedChar != null)
			{
				this.peekedChar = default(char?);
				return;
			}
			if (this.emergencyPlayback != null && this.emergencyPlayback.Count > 0)
			{
				this.emergencyPlayback.Dequeue();
				return;
			}
			this.reader.Read();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000BB84 File Offset: 0x00009D84
		private char ConsumeChar()
		{
			if (this.peekedChar != null)
			{
				char? c = this.peekedChar;
				this.peekedChar = default(char?);
				return c.Value;
			}
			if (this.emergencyPlayback != null && this.emergencyPlayback.Count > 0)
			{
				return this.emergencyPlayback.Dequeue();
			}
			return (char)this.reader.Read();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		// Note: this type is marked as 'beforefieldinit'.
		static JsonTextReader()
		{
			Dictionary<char, EntryType?> dictionary = new Dictionary<char, EntryType?>();
			dictionary.Add('{', new EntryType?(EntryType.StartOfNode));
			dictionary.Add('}', new EntryType?(EntryType.EndOfNode));
			dictionary.Add(',', default(EntryType?));
			dictionary.Add('[', new EntryType?(EntryType.PrimitiveArray));
			dictionary.Add(']', new EntryType?(EntryType.EndOfArray));
			JsonTextReader.EntryDelineators = dictionary;
			Dictionary<char, char> dictionary2 = new Dictionary<char, char>();
			dictionary2.Add('a', '\a');
			dictionary2.Add('b', '\b');
			dictionary2.Add('f', '\f');
			dictionary2.Add('n', '\n');
			dictionary2.Add('r', '\r');
			dictionary2.Add('t', '\t');
			dictionary2.Add('0', '\0');
			JsonTextReader.UnescapeDictionary = dictionary2;
		}

		// Token: 0x04000070 RID: 112
		private static readonly Dictionary<char, EntryType?> EntryDelineators;

		// Token: 0x04000071 RID: 113
		private static readonly Dictionary<char, char> UnescapeDictionary;

		// Token: 0x04000072 RID: 114
		private StreamReader reader;

		// Token: 0x04000073 RID: 115
		private int bufferIndex;

		// Token: 0x04000074 RID: 116
		private char[] buffer = new char[256];

		// Token: 0x04000075 RID: 117
		private char? lastReadChar;

		// Token: 0x04000076 RID: 118
		private char? peekedChar;

		// Token: 0x04000077 RID: 119
		private Queue<char> emergencyPlayback;
	}
}
