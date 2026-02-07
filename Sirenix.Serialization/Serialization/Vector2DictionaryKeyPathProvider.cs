using System;
using System.Globalization;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000096 RID: 150
	public sealed class Vector2DictionaryKeyPathProvider : BaseDictionaryKeyPathProvider<Vector2>
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001FF72 File Offset: 0x0001E172
		public override string ProviderID
		{
			get
			{
				return "v2";
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001FF7C File Offset: 0x0001E17C
		public override int Compare(Vector2 x, Vector2 y)
		{
			int num = x.x.CompareTo(y.x);
			if (num == 0)
			{
				num = x.y.CompareTo(y.y);
			}
			return num;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001FFB4 File Offset: 0x0001E1B4
		public override Vector2 GetKeyFromPathString(string pathStr)
		{
			int num = pathStr.IndexOf('|');
			string text = pathStr.Substring(1, num - 1).Trim();
			string text2 = pathStr.Substring(num + 1, pathStr.Length - (num + 2)).Trim();
			return new Vector2(float.Parse(text), float.Parse(text2));
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00020004 File Offset: 0x0001E204
		public override string GetPathStringFromKey(Vector2 key)
		{
			string text = key.x.ToString("R", CultureInfo.InvariantCulture);
			string text2 = key.y.ToString("R", CultureInfo.InvariantCulture);
			return string.Concat(new string[]
			{
				"(",
				text,
				"|",
				text2,
				")"
			}).Replace('.', ',');
		}
	}
}
