using System;
using System.Globalization;

namespace System.Collections
{
	// Token: 0x02000A22 RID: 2594
	[Serializable]
	public class CaseInsensitiveComparer : IComparer
	{
		// Token: 0x06005BCE RID: 23502 RVA: 0x0013540E File Offset: 0x0013360E
		public CaseInsensitiveComparer()
		{
			this._compareInfo = CultureInfo.CurrentCulture.CompareInfo;
		}

		// Token: 0x06005BCF RID: 23503 RVA: 0x00135426 File Offset: 0x00133626
		public CaseInsensitiveComparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this._compareInfo = culture.CompareInfo;
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x00135448 File Offset: 0x00133648
		public static CaseInsensitiveComparer Default
		{
			get
			{
				return new CaseInsensitiveComparer(CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x00135454 File Offset: 0x00133654
		public static CaseInsensitiveComparer DefaultInvariant
		{
			get
			{
				if (CaseInsensitiveComparer.s_InvariantCaseInsensitiveComparer == null)
				{
					CaseInsensitiveComparer.s_InvariantCaseInsensitiveComparer = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);
				}
				return CaseInsensitiveComparer.s_InvariantCaseInsensitiveComparer;
			}
		}

		// Token: 0x06005BD2 RID: 23506 RVA: 0x00135478 File Offset: 0x00133678
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this._compareInfo.Compare(text, text2, CompareOptions.IgnoreCase);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04003882 RID: 14466
		private CompareInfo _compareInfo;

		// Token: 0x04003883 RID: 14467
		private static volatile CaseInsensitiveComparer s_InvariantCaseInsensitiveComparer;
	}
}
