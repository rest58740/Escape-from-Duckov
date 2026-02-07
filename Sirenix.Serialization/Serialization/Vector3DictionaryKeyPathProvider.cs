using System;
using System.Globalization;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000097 RID: 151
	public sealed class Vector3DictionaryKeyPathProvider : BaseDictionaryKeyPathProvider<Vector3>
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0002007B File Offset: 0x0001E27B
		public override string ProviderID
		{
			get
			{
				return "v3";
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00020084 File Offset: 0x0001E284
		public override int Compare(Vector3 x, Vector3 y)
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
			return num;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000200D4 File Offset: 0x0001E2D4
		public override Vector3 GetKeyFromPathString(string pathStr)
		{
			int num = pathStr.IndexOf('|');
			int num2 = pathStr.IndexOf('|', num + 1);
			string text = pathStr.Substring(1, num - 1).Trim();
			string text2 = pathStr.Substring(num + 1, num2 - (num + 1)).Trim();
			string text3 = pathStr.Substring(num2 + 1, pathStr.Length - (num2 + 2)).Trim();
			return new Vector3(float.Parse(text), float.Parse(text2), float.Parse(text3));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0002014C File Offset: 0x0001E34C
		public override string GetPathStringFromKey(Vector3 key)
		{
			string text = key.x.ToString("R", CultureInfo.InvariantCulture);
			string text2 = key.y.ToString("R", CultureInfo.InvariantCulture);
			string text3 = key.z.ToString("R", CultureInfo.InvariantCulture);
			return string.Concat(new string[]
			{
				"(",
				text,
				"|",
				text2,
				"|",
				text3,
				")"
			}).Replace('.', ',');
		}
	}
}
