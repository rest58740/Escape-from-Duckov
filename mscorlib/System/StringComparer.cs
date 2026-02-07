using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System
{
	// Token: 0x02000187 RID: 391
	[Serializable]
	public abstract class StringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x000414FF File Offset: 0x0003F6FF
		public static StringComparer InvariantCulture
		{
			get
			{
				return StringComparer.s_invariantCulture;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00041506 File Offset: 0x0003F706
		public static StringComparer InvariantCultureIgnoreCase
		{
			get
			{
				return StringComparer.s_invariantCultureIgnoreCase;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0004150D File Offset: 0x0003F70D
		public static StringComparer CurrentCulture
		{
			get
			{
				return new CultureAwareComparer(CultureInfo.CurrentCulture, CompareOptions.None);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0004151A File Offset: 0x0003F71A
		public static StringComparer CurrentCultureIgnoreCase
		{
			get
			{
				return new CultureAwareComparer(CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00041527 File Offset: 0x0003F727
		public static StringComparer Ordinal
		{
			get
			{
				return StringComparer.s_ordinal;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0004152E File Offset: 0x0003F72E
		public static StringComparer OrdinalIgnoreCase
		{
			get
			{
				return StringComparer.s_ordinalIgnoreCase;
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00041538 File Offset: 0x0003F738
		public static StringComparer FromComparison(StringComparison comparisonType)
		{
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return StringComparer.CurrentCulture;
			case StringComparison.CurrentCultureIgnoreCase:
				return StringComparer.CurrentCultureIgnoreCase;
			case StringComparison.InvariantCulture:
				return StringComparer.InvariantCulture;
			case StringComparison.InvariantCultureIgnoreCase:
				return StringComparer.InvariantCultureIgnoreCase;
			case StringComparison.Ordinal:
				return StringComparer.Ordinal;
			case StringComparison.OrdinalIgnoreCase:
				return StringComparer.OrdinalIgnoreCase;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00041598 File Offset: 0x0003F798
		public static StringComparer Create(CultureInfo culture, bool ignoreCase)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return new CultureAwareComparer(culture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x000415B5 File Offset: 0x0003F7B5
		public static StringComparer Create(CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentException("culture");
			}
			return new CultureAwareComparer(culture, options);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x000415CC File Offset: 0x0003F7CC
		public int Compare(object x, object y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			string text = x as string;
			if (text != null)
			{
				string text2 = y as string;
				if (text2 != null)
				{
					return this.Compare(text, text2);
				}
			}
			IComparable comparable = x as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(y);
			}
			throw new ArgumentException("At least one object must implement IComparable.");
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00041624 File Offset: 0x0003F824
		public bool Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			string text = x as string;
			if (text != null)
			{
				string text2 = y as string;
				if (text2 != null)
				{
					return this.Equals(text, text2);
				}
			}
			return x.Equals(y);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00041664 File Offset: 0x0003F864
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			string text = obj as string;
			if (text != null)
			{
				return this.GetHashCode(text);
			}
			return obj.GetHashCode();
		}

		// Token: 0x06000FAA RID: 4010
		public abstract int Compare(string x, string y);

		// Token: 0x06000FAB RID: 4011
		public abstract bool Equals(string x, string y);

		// Token: 0x06000FAC RID: 4012
		public abstract int GetHashCode(string obj);

		// Token: 0x040012EE RID: 4846
		private static readonly CultureAwareComparer s_invariantCulture = new CultureAwareComparer(CultureInfo.InvariantCulture, CompareOptions.None);

		// Token: 0x040012EF RID: 4847
		private static readonly CultureAwareComparer s_invariantCultureIgnoreCase = new CultureAwareComparer(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);

		// Token: 0x040012F0 RID: 4848
		private static readonly OrdinalCaseSensitiveComparer s_ordinal = new OrdinalCaseSensitiveComparer();

		// Token: 0x040012F1 RID: 4849
		private static readonly OrdinalIgnoreCaseComparer s_ordinalIgnoreCase = new OrdinalIgnoreCaseComparer();
	}
}
