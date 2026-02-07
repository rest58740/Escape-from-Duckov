using System;
using System.Globalization;
using System.IO;

namespace ES3Internal
{
	// Token: 0x020000E8 RID: 232
	internal class ES3JSONWriter : ES3Writer
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x0001E869 File Offset: 0x0001CA69
		public ES3JSONWriter(Stream stream, ES3Settings settings) : this(stream, settings, true, true)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001E875 File Offset: 0x0001CA75
		internal ES3JSONWriter(Stream stream, ES3Settings settings, bool writeHeaderAndFooter, bool mergeKeys) : base(settings, writeHeaderAndFooter, mergeKeys)
		{
			this.baseWriter = new StreamWriter(stream);
			this.StartWriteFile();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001E89A File Offset: 0x0001CA9A
		internal override void WritePrimitive(int value)
		{
			this.baseWriter.Write(value);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001E8A8 File Offset: 0x0001CAA8
		internal override void WritePrimitive(float value)
		{
			this.baseWriter.Write(value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001E8C6 File Offset: 0x0001CAC6
		internal override void WritePrimitive(bool value)
		{
			this.baseWriter.Write(value ? "true" : "false");
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001E8E2 File Offset: 0x0001CAE2
		internal override void WritePrimitive(decimal value)
		{
			this.baseWriter.Write(value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001E8FB File Offset: 0x0001CAFB
		internal override void WritePrimitive(double value)
		{
			this.baseWriter.Write(value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001E919 File Offset: 0x0001CB19
		internal override void WritePrimitive(long value)
		{
			this.baseWriter.Write(value);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001E927 File Offset: 0x0001CB27
		internal override void WritePrimitive(ulong value)
		{
			this.baseWriter.Write(value);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001E935 File Offset: 0x0001CB35
		internal override void WritePrimitive(uint value)
		{
			this.baseWriter.Write(value);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001E943 File Offset: 0x0001CB43
		internal override void WritePrimitive(byte value)
		{
			this.baseWriter.Write(Convert.ToInt32(value));
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001E956 File Offset: 0x0001CB56
		internal override void WritePrimitive(sbyte value)
		{
			this.baseWriter.Write(Convert.ToInt32(value));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001E969 File Offset: 0x0001CB69
		internal override void WritePrimitive(short value)
		{
			this.baseWriter.Write(Convert.ToInt32(value));
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001E97C File Offset: 0x0001CB7C
		internal override void WritePrimitive(ushort value)
		{
			this.baseWriter.Write(Convert.ToInt32(value));
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001E98F File Offset: 0x0001CB8F
		internal override void WritePrimitive(char value)
		{
			this.WritePrimitive(value.ToString());
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001E99E File Offset: 0x0001CB9E
		internal override void WritePrimitive(byte[] value)
		{
			this.WritePrimitive(Convert.ToBase64String(value));
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001E9AC File Offset: 0x0001CBAC
		internal override void WritePrimitive(string value)
		{
			this.baseWriter.Write("\"");
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				if (c <= '/')
				{
					switch (c)
					{
					case '\b':
						this.baseWriter.Write("\\b");
						break;
					case '\t':
						this.baseWriter.Write("\\t");
						break;
					case '\n':
						this.baseWriter.Write("\\n");
						break;
					case '\v':
						goto IL_DD;
					case '\f':
						this.baseWriter.Write("\\f");
						break;
					case '\r':
						this.baseWriter.Write("\\r");
						break;
					default:
						if (c != '"' && c != '/')
						{
							goto IL_DD;
						}
						goto IL_68;
					}
				}
				else
				{
					if (c == '\\' || c == '“' || c == '”')
					{
						goto IL_68;
					}
					goto IL_DD;
				}
				IL_E9:
				i++;
				continue;
				IL_68:
				this.baseWriter.Write('\\');
				this.baseWriter.Write(c);
				goto IL_E9;
				IL_DD:
				this.baseWriter.Write(c);
				goto IL_E9;
			}
			this.baseWriter.Write("\"");
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001EAC2 File Offset: 0x0001CCC2
		internal override void WriteNull()
		{
			this.baseWriter.Write("null");
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		private static bool CharacterRequiresEscaping(char c)
		{
			return c == '"' || c == '\\' || c == '“' || c == '”';
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001EAF2 File Offset: 0x0001CCF2
		private void WriteCommaIfRequired()
		{
			if (!this.isFirstProperty)
			{
				this.baseWriter.Write(',');
			}
			else
			{
				this.isFirstProperty = false;
			}
			this.WriteNewlineAndTabs();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001EB18 File Offset: 0x0001CD18
		internal override void WriteRawProperty(string name, byte[] value)
		{
			this.StartWriteProperty(name);
			this.baseWriter.Write(this.settings.encoding.GetString(value, 0, value.Length));
			this.EndWriteProperty(name);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001EB48 File Offset: 0x0001CD48
		internal override void StartWriteFile()
		{
			if (this.writeHeaderAndFooter)
			{
				this.baseWriter.Write('{');
			}
			base.StartWriteFile();
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001EB65 File Offset: 0x0001CD65
		internal override void EndWriteFile()
		{
			base.EndWriteFile();
			this.WriteNewlineAndTabs();
			if (this.writeHeaderAndFooter)
			{
				this.baseWriter.Write('}');
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001EB88 File Offset: 0x0001CD88
		internal override void StartWriteProperty(string name)
		{
			base.StartWriteProperty(name);
			this.WriteCommaIfRequired();
			this.Write(name, ES3.ReferenceMode.ByRef);
			if (this.settings.prettyPrint)
			{
				this.baseWriter.Write(' ');
			}
			this.baseWriter.Write(':');
			if (this.settings.prettyPrint)
			{
				this.baseWriter.Write(' ');
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001EBEB File Offset: 0x0001CDEB
		internal override void EndWriteProperty(string name)
		{
			base.EndWriteProperty(name);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		internal override void StartWriteObject(string name)
		{
			base.StartWriteObject(name);
			this.isFirstProperty = true;
			this.baseWriter.Write('{');
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001EC11 File Offset: 0x0001CE11
		internal override void EndWriteObject(string name)
		{
			base.EndWriteObject(name);
			this.isFirstProperty = false;
			this.WriteNewlineAndTabs();
			this.baseWriter.Write('}');
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001EC34 File Offset: 0x0001CE34
		internal override void StartWriteCollection()
		{
			base.StartWriteCollection();
			this.baseWriter.Write('[');
			this.WriteNewlineAndTabs();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001EC4F File Offset: 0x0001CE4F
		internal override void EndWriteCollection()
		{
			base.EndWriteCollection();
			this.WriteNewlineAndTabs();
			this.baseWriter.Write(']');
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001EC6A File Offset: 0x0001CE6A
		internal override void StartWriteCollectionItem(int index)
		{
			if (index != 0)
			{
				this.baseWriter.Write(',');
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001EC7C File Offset: 0x0001CE7C
		internal override void EndWriteCollectionItem(int index)
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001EC7E File Offset: 0x0001CE7E
		internal override void StartWriteDictionary()
		{
			this.StartWriteObject(null);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001EC87 File Offset: 0x0001CE87
		internal override void EndWriteDictionary()
		{
			this.EndWriteObject(null);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001EC90 File Offset: 0x0001CE90
		internal override void StartWriteDictionaryKey(int index)
		{
			if (index != 0)
			{
				this.baseWriter.Write(',');
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001ECA2 File Offset: 0x0001CEA2
		internal override void EndWriteDictionaryKey(int index)
		{
			this.baseWriter.Write(':');
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001ECB1 File Offset: 0x0001CEB1
		internal override void StartWriteDictionaryValue(int index)
		{
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001ECB3 File Offset: 0x0001CEB3
		internal override void EndWriteDictionaryValue(int index)
		{
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001ECB5 File Offset: 0x0001CEB5
		public override void Dispose()
		{
			this.baseWriter.Dispose();
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
		public void WriteNewlineAndTabs()
		{
			if (this.settings.prettyPrint)
			{
				this.baseWriter.Write(Environment.NewLine);
				for (int i = 0; i < this.serializationDepth; i++)
				{
					this.baseWriter.Write('\t');
				}
			}
		}

		// Token: 0x04000173 RID: 371
		internal StreamWriter baseWriter;

		// Token: 0x04000174 RID: 372
		private bool isFirstProperty = true;
	}
}
