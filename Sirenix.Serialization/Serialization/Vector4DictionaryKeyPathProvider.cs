using System;
using System.Globalization;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000098 RID: 152
	public sealed class Vector4DictionaryKeyPathProvider : BaseDictionaryKeyPathProvider<Vector4>
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x000201E6 File Offset: 0x0001E3E6
		public override string ProviderID
		{
			get
			{
				return "v4";
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000201F0 File Offset: 0x0001E3F0
		public override int Compare(Vector4 x, Vector4 y)
		{
			int num = x.x.CompareTo(y.x);
			if (num == 0)
			{
				num = x.y.CompareTo(y.y);
			}
			if (num == 0)
			{
				num = x.z.CompareTo(y.z);
			}
			if (num == 0)
			{
				num = x.w.CompareTo(y.w);
			}
			return num;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00020254 File Offset: 0x0001E454
		public override Vector4 GetKeyFromPathString(string pathStr)
		{
			int num = pathStr.IndexOf('|');
			int num2 = pathStr.IndexOf('|', num + 1);
			int num3 = pathStr.IndexOf('|', num2 + 1);
			string text = pathStr.Substring(1, num - 1).Trim();
			string text2 = pathStr.Substring(num + 1, num2 - (num + 1)).Trim();
			string text3 = pathStr.Substring(num2 + 1, num3 - (num2 + 1)).Trim();
			string text4 = pathStr.Substring(num3 + 1, pathStr.Length - (num3 + 2)).Trim();
			return new Vector4(float.Parse(text), float.Parse(text2), float.Parse(text3), float.Parse(text4));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000202F8 File Offset: 0x0001E4F8
		public override string GetPathStringFromKey(Vector4 key)
		{
			string text = key.x.ToString("R", CultureInfo.InvariantCulture);
			string text2 = key.y.ToString("R", CultureInfo.InvariantCulture);
			string text3 = key.z.ToString("R", CultureInfo.InvariantCulture);
			string text4 = key.w.ToString("R", CultureInfo.InvariantCulture);
			return string.Concat(new string[]
			{
				"(",
				text,
				"|",
				text2,
				"|",
				text3,
				"|",
				text4,
				")"
			}).Replace('.', ',');
		}
	}
}
