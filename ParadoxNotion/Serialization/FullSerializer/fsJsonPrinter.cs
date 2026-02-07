using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A9 RID: 169
	public static class fsJsonPrinter
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x00012870 File Offset: 0x00010A70
		private static void InsertSpacing(TextWriter stream, int count)
		{
			for (int i = 0; i < count; i++)
			{
				stream.Write("    ");
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00012894 File Offset: 0x00010A94
		private static string EscapeString(string str)
		{
			bool flag = false;
			int i = 0;
			while (i < str.Length)
			{
				char c = str.get_Chars(i);
				int num = Convert.ToInt32(c);
				if (num < 0 || num > 127)
				{
					flag = true;
					break;
				}
				if (c <= '\r')
				{
					if (c == '\0')
					{
						goto IL_5D;
					}
					switch (c)
					{
					case '\a':
					case '\b':
					case '\t':
					case '\n':
					case '\f':
					case '\r':
						goto IL_5D;
					}
				}
				else if (c == '"' || c == '\\')
				{
					goto IL_5D;
				}
				IL_5F:
				if (!flag)
				{
					i++;
					continue;
				}
				break;
				IL_5D:
				flag = true;
				goto IL_5F;
			}
			if (!flag)
			{
				return str;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < str.Length; j++)
			{
				char c2 = str.get_Chars(j);
				int num2 = Convert.ToInt32(c2);
				if (num2 < 0 || num2 > 127)
				{
					stringBuilder.Append(string.Format("\\u{0:x4} ", num2).Trim());
				}
				else
				{
					if (c2 <= '\r')
					{
						if (c2 == '\0')
						{
							stringBuilder.Append("\\0");
							goto IL_18E;
						}
						switch (c2)
						{
						case '\a':
							stringBuilder.Append("\\a");
							goto IL_18E;
						case '\b':
							stringBuilder.Append("\\b");
							goto IL_18E;
						case '\t':
							stringBuilder.Append("\\t");
							goto IL_18E;
						case '\n':
							stringBuilder.Append("\\n");
							goto IL_18E;
						case '\f':
							stringBuilder.Append("\\f");
							goto IL_18E;
						case '\r':
							stringBuilder.Append("\\r");
							goto IL_18E;
						}
					}
					else
					{
						if (c2 == '"')
						{
							stringBuilder.Append("\\\"");
							goto IL_18E;
						}
						if (c2 == '\\')
						{
							stringBuilder.Append("\\\\");
							goto IL_18E;
						}
					}
					stringBuilder.Append(c2);
				}
				IL_18E:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00012A48 File Offset: 0x00010C48
		private static void BuildCompressedString(fsData data, TextWriter stream)
		{
			switch (data.Type)
			{
			case fsDataType.Array:
			{
				stream.Write('[');
				bool flag = false;
				foreach (fsData data2 in data.AsList)
				{
					if (flag)
					{
						stream.Write(',');
					}
					flag = true;
					fsJsonPrinter.BuildCompressedString(data2, stream);
				}
				stream.Write(']');
				return;
			}
			case fsDataType.Object:
			{
				stream.Write('{');
				bool flag2 = false;
				foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
				{
					if (flag2)
					{
						stream.Write(',');
					}
					flag2 = true;
					stream.Write('"');
					stream.Write(keyValuePair.Key);
					stream.Write('"');
					stream.Write(":");
					fsJsonPrinter.BuildCompressedString(keyValuePair.Value, stream);
				}
				stream.Write('}');
				return;
			}
			case fsDataType.Double:
				stream.Write(fsJsonPrinter.ConvertDoubleToString(data.AsDouble));
				return;
			case fsDataType.Int64:
				stream.Write(data.AsInt64);
				return;
			case fsDataType.Boolean:
				if (data.AsBool)
				{
					stream.Write("true");
					return;
				}
				stream.Write("false");
				return;
			case fsDataType.String:
				stream.Write('"');
				stream.Write(fsJsonPrinter.EscapeString(data.AsString));
				stream.Write('"');
				return;
			case fsDataType.Null:
				stream.Write("null");
				return;
			default:
				return;
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00012BE4 File Offset: 0x00010DE4
		private static void BuildPrettyString(fsData data, TextWriter stream, int depth)
		{
			switch (data.Type)
			{
			case fsDataType.Array:
			{
				if (data.AsList.Count == 0)
				{
					stream.Write("[]");
					return;
				}
				bool flag = false;
				stream.Write('[');
				stream.WriteLine();
				foreach (fsData data2 in data.AsList)
				{
					if (flag)
					{
						stream.Write(',');
						stream.WriteLine();
					}
					flag = true;
					fsJsonPrinter.InsertSpacing(stream, depth + 1);
					fsJsonPrinter.BuildPrettyString(data2, stream, depth + 1);
				}
				stream.WriteLine();
				fsJsonPrinter.InsertSpacing(stream, depth);
				stream.Write(']');
				return;
			}
			case fsDataType.Object:
			{
				stream.Write('{');
				stream.WriteLine();
				bool flag2 = false;
				foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
				{
					if (flag2)
					{
						stream.Write(',');
						stream.WriteLine();
					}
					flag2 = true;
					fsJsonPrinter.InsertSpacing(stream, depth + 1);
					stream.Write('"');
					stream.Write(keyValuePair.Key);
					stream.Write('"');
					stream.Write(": ");
					fsJsonPrinter.BuildPrettyString(keyValuePair.Value, stream, depth + 1);
				}
				stream.WriteLine();
				fsJsonPrinter.InsertSpacing(stream, depth);
				stream.Write('}');
				return;
			}
			case fsDataType.Double:
				stream.Write(fsJsonPrinter.ConvertDoubleToString(data.AsDouble));
				return;
			case fsDataType.Int64:
				stream.Write(data.AsInt64);
				return;
			case fsDataType.Boolean:
				if (data.AsBool)
				{
					stream.Write("true");
					return;
				}
				stream.Write("false");
				return;
			case fsDataType.String:
				stream.Write('"');
				stream.Write(fsJsonPrinter.EscapeString(data.AsString));
				stream.Write('"');
				return;
			case fsDataType.Null:
				stream.Write("null");
				return;
			default:
				return;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00012DE4 File Offset: 0x00010FE4
		public static string ToJson(fsData data, bool pretty)
		{
			if (pretty)
			{
				return fsJsonPrinter.PrettyJson(data);
			}
			return fsJsonPrinter.CompressedJson(data);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00012DF6 File Offset: 0x00010FF6
		public static void PrettyJson(fsData data, TextWriter outputStream)
		{
			fsJsonPrinter.BuildPrettyString(data, outputStream, 0);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00012E00 File Offset: 0x00011000
		public static string PrettyJson(fsData data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string result;
			using (StringWriter stringWriter = new StringWriter(stringBuilder))
			{
				fsJsonPrinter.BuildPrettyString(data, stringWriter, 0);
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00012E48 File Offset: 0x00011048
		public static void CompressedJson(fsData data, StreamWriter outputStream)
		{
			fsJsonPrinter.BuildCompressedString(data, outputStream);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00012E54 File Offset: 0x00011054
		public static string CompressedJson(fsData data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string result;
			using (StringWriter stringWriter = new StringWriter(stringBuilder))
			{
				fsJsonPrinter.BuildCompressedString(data, stringWriter);
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00012E9C File Offset: 0x0001109C
		private static string ConvertDoubleToString(double d)
		{
			if (double.IsInfinity(d) || double.IsNaN(d))
			{
				return d.ToString(CultureInfo.InvariantCulture);
			}
			string text = d.ToString(CultureInfo.InvariantCulture);
			if (!text.Contains(".") && !text.Contains("e") && !text.Contains("E"))
			{
				text += ".0";
			}
			return text;
		}
	}
}
