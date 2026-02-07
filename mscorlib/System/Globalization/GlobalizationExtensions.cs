using System;

namespace System.Globalization
{
	// Token: 0x02000965 RID: 2405
	public static class GlobalizationExtensions
	{
		// Token: 0x0600553F RID: 21823 RVA: 0x0011DE94 File Offset: 0x0011C094
		public static StringComparer GetStringComparer(this CompareInfo compareInfo, CompareOptions options)
		{
			if (compareInfo == null)
			{
				throw new ArgumentNullException("compareInfo");
			}
			if (options == CompareOptions.Ordinal)
			{
				return StringComparer.Ordinal;
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return StringComparer.OrdinalIgnoreCase;
			}
			return new CultureAwareComparer(compareInfo, options);
		}
	}
}
