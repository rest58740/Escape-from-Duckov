using System;
using System.Collections.Generic;
using System.Linq;

namespace Eflatun.SceneReference.Utility
{
	// Token: 0x02000010 RID: 16
	internal static class Utils
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000027C0 File Offset: 0x000009C0
		public static bool IsAddressablesPackagePresent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000027C3 File Offset: 0x000009C3
		public static string WithoutExtension(this string path)
		{
			return path.BeforeLast('.');
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000027CD File Offset: 0x000009CD
		public static bool IncludesFlag<T>(this T composite, T flag) where T : Enum
		{
			return composite.HasFlag(flag);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000027E4 File Offset: 0x000009E4
		public static string BeforeLast(this string source, char chr)
		{
			int num = source.LastIndexOf(chr);
			if (num >= 0)
			{
				return source.Substring(0, num);
			}
			return source;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002807 File Offset: 0x00000A07
		public static bool IsValidGuid(this string guid)
		{
			return guid.Length == 32 && guid.ToUpper().All(new Func<char, bool>("0123456789ABCDEF".Contains));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002830 File Offset: 0x00000A30
		public static string GuardGuidAgainstNullOrWhitespace(this string guid)
		{
			if (!string.IsNullOrWhiteSpace(guid))
			{
				return guid;
			}
			return "00000000000000000000000000000000";
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002841 File Offset: 0x00000A41
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> readOnly)
		{
			return new Dictionary<TKey, TValue>(readOnly);
		}

		// Token: 0x0400002C RID: 44
		public const string AllZeroGuid = "00000000000000000000000000000000";
	}
}
