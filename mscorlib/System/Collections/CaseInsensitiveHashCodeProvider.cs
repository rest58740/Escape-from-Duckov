using System;
using System.Globalization;

namespace System.Collections
{
	// Token: 0x02000A23 RID: 2595
	[Obsolete("Please use StringComparer instead.")]
	[Serializable]
	public class CaseInsensitiveHashCodeProvider : IHashCodeProvider
	{
		// Token: 0x06005BD3 RID: 23507 RVA: 0x001354B4 File Offset: 0x001336B4
		public CaseInsensitiveHashCodeProvider()
		{
			this._compareInfo = CultureInfo.CurrentCulture.CompareInfo;
		}

		// Token: 0x06005BD4 RID: 23508 RVA: 0x001354CC File Offset: 0x001336CC
		public CaseInsensitiveHashCodeProvider(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this._compareInfo = culture.CompareInfo;
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x001354EE File Offset: 0x001336EE
		public static CaseInsensitiveHashCodeProvider Default
		{
			get
			{
				return new CaseInsensitiveHashCodeProvider();
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x001354F5 File Offset: 0x001336F5
		public static CaseInsensitiveHashCodeProvider DefaultInvariant
		{
			get
			{
				CaseInsensitiveHashCodeProvider result;
				if ((result = CaseInsensitiveHashCodeProvider.s_invariantCaseInsensitiveHashCodeProvider) == null)
				{
					result = (CaseInsensitiveHashCodeProvider.s_invariantCaseInsensitiveHashCodeProvider = new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture));
				}
				return result;
			}
		}

		// Token: 0x06005BD7 RID: 23511 RVA: 0x00135514 File Offset: 0x00133714
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			string text = obj as string;
			if (text == null)
			{
				return obj.GetHashCode();
			}
			return this._compareInfo.GetHashCode(text, CompareOptions.IgnoreCase);
		}

		// Token: 0x04003884 RID: 14468
		private static volatile CaseInsensitiveHashCodeProvider s_invariantCaseInsensitiveHashCodeProvider;

		// Token: 0x04003885 RID: 14469
		private readonly CompareInfo _compareInfo;
	}
}
