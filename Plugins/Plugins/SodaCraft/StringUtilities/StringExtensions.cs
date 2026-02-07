using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SodaCraft.StringUtilities
{
	// Token: 0x0200000D RID: 13
	public static class StringExtensions
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002A98 File Offset: 0x00000C98
		public static string Format(this string format, params string[] content)
		{
			string result = format;
			try
			{
				result = string.Format(format, content);
			}
			catch
			{
				Debug.LogError("Format Error Occured!:\nFormat: " + format);
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public static string Format(this string format, object contentStruct)
		{
			if (contentStruct == null)
			{
				return format;
			}
			Type type = contentStruct.GetType();
			string text = format;
			string[] placeHolders = format.GetPlaceHolders();
			int i = 0;
			while (i < placeHolders.Length)
			{
				string text2 = placeHolders[i];
				PropertyInfo property = type.GetProperty(text2);
				object value;
				if (property != null)
				{
					value = property.GetValue(contentStruct);
					if (value != null)
					{
						goto IL_64;
					}
				}
				else
				{
					FieldInfo field = type.GetField(text2);
					if (!(field == null))
					{
						value = field.GetValue(contentStruct);
						if (value != null)
						{
							goto IL_64;
						}
					}
				}
				IL_8B:
				i++;
				continue;
				IL_64:
				string text3 = value.ToString();
				string text4 = "{" + text2 + "}";
				text = text.Replace(text4, text3);
				goto IL_8B;
			}
			return text;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B7C File Offset: 0x00000D7C
		private static string[] GetPlaceHolders(this string format)
		{
			bool flag = false;
			StringExtensions.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.bufferHead = 0;
			CS$<>8__locals1.buffer = new char[64];
			int num = 0;
			string[] array = new string[64];
			for (int i = 0; i < format.Length; i++)
			{
				char c = format.get_Chars(i);
				if (flag)
				{
					if (c == '}')
					{
						array[num] = StringExtensions.<GetPlaceHolders>g__ReadBuffer|2_0(ref CS$<>8__locals1);
						num++;
						flag = false;
						if (num >= 64)
						{
							break;
						}
					}
					else
					{
						CS$<>8__locals1.buffer[CS$<>8__locals1.bufferHead] = c;
						int bufferHead = CS$<>8__locals1.bufferHead;
						CS$<>8__locals1.bufferHead = bufferHead + 1;
						if (CS$<>8__locals1.bufferHead >= 64)
						{
							break;
						}
					}
				}
				else if (c == '{')
				{
					flag = true;
					CS$<>8__locals1.bufferHead = 0;
				}
			}
			string[] array2 = new string[num];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = array[j];
			}
			return array2;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C4E File Offset: 0x00000E4E
		[CompilerGenerated]
		internal static string <GetPlaceHolders>g__ReadBuffer|2_0(ref StringExtensions.<>c__DisplayClass2_0 A_0)
		{
			return new string(A_0.buffer, 0, A_0.bufferHead);
		}
	}
}
