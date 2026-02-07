using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x02000994 RID: 2452
	[ComVisible(true)]
	[Serializable]
	public sealed class NumberFormatInfo : ICloneable, IFormatProvider
	{
		// Token: 0x0600578E RID: 22414 RVA: 0x00127B83 File Offset: 0x00125D83
		public NumberFormatInfo() : this(null)
		{
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x00127B8C File Offset: 0x00125D8C
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if (this.numberDecimalSeparator != this.numberGroupSeparator)
			{
				this.validForParseAsNumber = true;
			}
			else
			{
				this.validForParseAsNumber = false;
			}
			if (this.numberDecimalSeparator != this.numberGroupSeparator && this.numberDecimalSeparator != this.currencyGroupSeparator && this.currencyDecimalSeparator != this.numberGroupSeparator && this.currencyDecimalSeparator != this.currencyGroupSeparator)
			{
				this.validForParseAsCurrency = true;
				return;
			}
			this.validForParseAsCurrency = false;
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x00127C17 File Offset: 0x00125E17
		private static void VerifyDecimalSeparator(string decSep, string propertyName)
		{
			if (decSep == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			if (decSep.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Decimal separator cannot be the empty string."));
			}
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x00127C45 File Offset: 0x00125E45
		private static void VerifyGroupSeparator(string groupSep, string propertyName)
		{
			if (groupSep == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("String reference not set to an instance of a String."));
			}
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x00127C5C File Offset: 0x00125E5C
		private static void VerifyNativeDigits(string[] nativeDig, string propertyName)
		{
			if (nativeDig == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("Array cannot be null."));
			}
			if (nativeDig.Length != 10)
			{
				throw new ArgumentException(Environment.GetResourceString("The NativeDigits array must contain exactly ten members."), propertyName);
			}
			for (int i = 0; i < nativeDig.Length; i++)
			{
				if (nativeDig[i] == null)
				{
					throw new ArgumentNullException(propertyName, Environment.GetResourceString("Found a null value within an array."));
				}
				if (nativeDig[i].Length != 1)
				{
					if (nativeDig[i].Length != 2)
					{
						throw new ArgumentException(Environment.GetResourceString("Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit."), propertyName);
					}
					if (!char.IsSurrogatePair(nativeDig[i][0], nativeDig[i][1]))
					{
						throw new ArgumentException(Environment.GetResourceString("Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit."), propertyName);
					}
				}
				if (CharUnicodeInfo.GetDecimalDigitValue(nativeDig[i], 0) != i && CharUnicodeInfo.GetUnicodeCategory(nativeDig[i], 0) != UnicodeCategory.PrivateUse)
				{
					throw new ArgumentException(Environment.GetResourceString("Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit."), propertyName);
				}
			}
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x00127D3A File Offset: 0x00125F3A
		private static void VerifyDigitSubstitution(DigitShapes digitSub, string propertyName)
		{
			if (digitSub > DigitShapes.NativeNational)
			{
				throw new ArgumentException(Environment.GetResourceString("The DigitSubstitution property must be of a valid member of the DigitShapes enumeration. Valid entries include Context, NativeNational or None."), propertyName);
			}
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x00127D54 File Offset: 0x00125F54
		[SecuritySafeCritical]
		internal NumberFormatInfo(CultureData cultureData)
		{
			if (GlobalizationMode.Invariant)
			{
				this.m_isInvariant = true;
				return;
			}
			if (cultureData != null)
			{
				cultureData.GetNFIValues(this);
				if (cultureData.IsInvariantCulture)
				{
					this.m_isInvariant = true;
				}
			}
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x00127EE8 File Offset: 0x001260E8
		private void VerifyWritable()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Instance is read-only."));
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005798 RID: 22424 RVA: 0x00127F02 File Offset: 0x00126102
		public static NumberFormatInfo InvariantInfo
		{
			get
			{
				if (NumberFormatInfo.invariantInfo == null)
				{
					NumberFormatInfo.invariantInfo = NumberFormatInfo.ReadOnly(new NumberFormatInfo
					{
						m_isInvariant = true
					});
				}
				return NumberFormatInfo.invariantInfo;
			}
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x00127F2C File Offset: 0x0012612C
		public static NumberFormatInfo GetInstance(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo != null && !cultureInfo.m_isInherited)
			{
				NumberFormatInfo numberFormatInfo = cultureInfo.numInfo;
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
				return cultureInfo.NumberFormat;
			}
			else
			{
				NumberFormatInfo numberFormatInfo = formatProvider as NumberFormatInfo;
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
				if (formatProvider != null)
				{
					numberFormatInfo = (formatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo);
					if (numberFormatInfo != null)
					{
						return numberFormatInfo;
					}
				}
				return NumberFormatInfo.CurrentInfo;
			}
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x00127F8F File Offset: 0x0012618F
		public object Clone()
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)base.MemberwiseClone();
			numberFormatInfo.isReadOnly = false;
			return numberFormatInfo;
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x0600579B RID: 22427 RVA: 0x00127FA3 File Offset: 0x001261A3
		// (set) Token: 0x0600579C RID: 22428 RVA: 0x00127FAC File Offset: 0x001261AC
		public int CurrencyDecimalDigits
		{
			get
			{
				return this.currencyDecimalDigits;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("CurrencyDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 99));
				}
				this.VerifyWritable();
				this.currencyDecimalDigits = value;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x0600579D RID: 22429 RVA: 0x00127FFB File Offset: 0x001261FB
		// (set) Token: 0x0600579E RID: 22430 RVA: 0x00128003 File Offset: 0x00126203
		public string CurrencyDecimalSeparator
		{
			get
			{
				return this.currencyDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyDecimalSeparator(value, "CurrencyDecimalSeparator");
				this.currencyDecimalSeparator = value;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x0600579F RID: 22431 RVA: 0x0012801D File Offset: 0x0012621D
		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x00128028 File Offset: 0x00126228
		internal static void CheckGroupSize(string propName, int[] groupSize)
		{
			int i = 0;
			while (i < groupSize.Length)
			{
				if (groupSize[i] < 1)
				{
					if (i == groupSize.Length - 1 && groupSize[i] == 0)
					{
						return;
					}
					throw new ArgumentException(Environment.GetResourceString("Every element in the value array should be between one and nine, except for the last element, which can be zero."), propName);
				}
				else
				{
					if (groupSize[i] > 9)
					{
						throw new ArgumentException(Environment.GetResourceString("Every element in the value array should be between one and nine, except for the last element, which can be zero."), propName);
					}
					i++;
				}
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x00128080 File Offset: 0x00126280
		// (set) Token: 0x060057A2 RID: 22434 RVA: 0x00128094 File Offset: 0x00126294
		public int[] CurrencyGroupSizes
		{
			get
			{
				return (int[])this.currencyGroupSizes.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("CurrencyGroupSizes", Environment.GetResourceString("Object cannot be null."));
				}
				this.VerifyWritable();
				int[] groupSize = (int[])value.Clone();
				NumberFormatInfo.CheckGroupSize("CurrencyGroupSizes", groupSize);
				this.currencyGroupSizes = groupSize;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060057A3 RID: 22435 RVA: 0x001280DD File Offset: 0x001262DD
		// (set) Token: 0x060057A4 RID: 22436 RVA: 0x001280F0 File Offset: 0x001262F0
		public int[] NumberGroupSizes
		{
			get
			{
				return (int[])this.numberGroupSizes.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("NumberGroupSizes", Environment.GetResourceString("Object cannot be null."));
				}
				this.VerifyWritable();
				int[] groupSize = (int[])value.Clone();
				NumberFormatInfo.CheckGroupSize("NumberGroupSizes", groupSize);
				this.numberGroupSizes = groupSize;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060057A5 RID: 22437 RVA: 0x00128139 File Offset: 0x00126339
		// (set) Token: 0x060057A6 RID: 22438 RVA: 0x0012814C File Offset: 0x0012634C
		public int[] PercentGroupSizes
		{
			get
			{
				return (int[])this.percentGroupSizes.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PercentGroupSizes", Environment.GetResourceString("Object cannot be null."));
				}
				this.VerifyWritable();
				int[] groupSize = (int[])value.Clone();
				NumberFormatInfo.CheckGroupSize("PercentGroupSizes", groupSize);
				this.percentGroupSizes = groupSize;
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060057A7 RID: 22439 RVA: 0x00128195 File Offset: 0x00126395
		// (set) Token: 0x060057A8 RID: 22440 RVA: 0x0012819D File Offset: 0x0012639D
		public string CurrencyGroupSeparator
		{
			get
			{
				return this.currencyGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyGroupSeparator(value, "CurrencyGroupSeparator");
				this.currencyGroupSeparator = value;
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060057A9 RID: 22441 RVA: 0x001281B7 File Offset: 0x001263B7
		// (set) Token: 0x060057AA RID: 22442 RVA: 0x001281BF File Offset: 0x001263BF
		public string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("CurrencySymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.currencySymbol = value;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060057AB RID: 22443 RVA: 0x001281E8 File Offset: 0x001263E8
		public static NumberFormatInfo CurrentInfo
		{
			get
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				if (!currentCulture.m_isInherited)
				{
					NumberFormatInfo numInfo = currentCulture.numInfo;
					if (numInfo != null)
					{
						return numInfo;
					}
				}
				return (NumberFormatInfo)currentCulture.GetFormat(typeof(NumberFormatInfo));
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x0012822B File Offset: 0x0012642B
		// (set) Token: 0x060057AD RID: 22445 RVA: 0x00128233 File Offset: 0x00126433
		public string NaNSymbol
		{
			get
			{
				return this.nanSymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("NaNSymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.nanSymbol = value;
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x0012825A File Offset: 0x0012645A
		// (set) Token: 0x060057AF RID: 22447 RVA: 0x00128264 File Offset: 0x00126464
		public int CurrencyNegativePattern
		{
			get
			{
				return this.currencyNegativePattern;
			}
			set
			{
				if (value < 0 || value > 15)
				{
					throw new ArgumentOutOfRangeException("CurrencyNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 15));
				}
				this.VerifyWritable();
				this.currencyNegativePattern = value;
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x001282B3 File Offset: 0x001264B3
		// (set) Token: 0x060057B1 RID: 22449 RVA: 0x001282BC File Offset: 0x001264BC
		public int NumberNegativePattern
		{
			get
			{
				return this.numberNegativePattern;
			}
			set
			{
				if (value < 0 || value > 4)
				{
					throw new ArgumentOutOfRangeException("NumberNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 4));
				}
				this.VerifyWritable();
				this.numberNegativePattern = value;
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060057B2 RID: 22450 RVA: 0x00128309 File Offset: 0x00126509
		// (set) Token: 0x060057B3 RID: 22451 RVA: 0x00128314 File Offset: 0x00126514
		public int PercentPositivePattern
		{
			get
			{
				return this.percentPositivePattern;
			}
			set
			{
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("PercentPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 3));
				}
				this.VerifyWritable();
				this.percentPositivePattern = value;
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060057B4 RID: 22452 RVA: 0x00128361 File Offset: 0x00126561
		// (set) Token: 0x060057B5 RID: 22453 RVA: 0x0012836C File Offset: 0x0012656C
		public int PercentNegativePattern
		{
			get
			{
				return this.percentNegativePattern;
			}
			set
			{
				if (value < 0 || value > 11)
				{
					throw new ArgumentOutOfRangeException("PercentNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 11));
				}
				this.VerifyWritable();
				this.percentNegativePattern = value;
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060057B6 RID: 22454 RVA: 0x001283BB File Offset: 0x001265BB
		// (set) Token: 0x060057B7 RID: 22455 RVA: 0x001283C3 File Offset: 0x001265C3
		public string NegativeInfinitySymbol
		{
			get
			{
				return this.negativeInfinitySymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("NegativeInfinitySymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.negativeInfinitySymbol = value;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060057B8 RID: 22456 RVA: 0x001283EA File Offset: 0x001265EA
		// (set) Token: 0x060057B9 RID: 22457 RVA: 0x001283F2 File Offset: 0x001265F2
		public string NegativeSign
		{
			get
			{
				return this.negativeSign;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("NegativeSign", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.negativeSign = value;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x00128419 File Offset: 0x00126619
		// (set) Token: 0x060057BB RID: 22459 RVA: 0x00128424 File Offset: 0x00126624
		public int NumberDecimalDigits
		{
			get
			{
				return this.numberDecimalDigits;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("NumberDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 99));
				}
				this.VerifyWritable();
				this.numberDecimalDigits = value;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060057BC RID: 22460 RVA: 0x00128473 File Offset: 0x00126673
		// (set) Token: 0x060057BD RID: 22461 RVA: 0x0012847B File Offset: 0x0012667B
		public string NumberDecimalSeparator
		{
			get
			{
				return this.numberDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyDecimalSeparator(value, "NumberDecimalSeparator");
				this.numberDecimalSeparator = value;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060057BE RID: 22462 RVA: 0x00128495 File Offset: 0x00126695
		// (set) Token: 0x060057BF RID: 22463 RVA: 0x0012849D File Offset: 0x0012669D
		public string NumberGroupSeparator
		{
			get
			{
				return this.numberGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyGroupSeparator(value, "NumberGroupSeparator");
				this.numberGroupSeparator = value;
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060057C0 RID: 22464 RVA: 0x001284B7 File Offset: 0x001266B7
		// (set) Token: 0x060057C1 RID: 22465 RVA: 0x001284C0 File Offset: 0x001266C0
		public int CurrencyPositivePattern
		{
			get
			{
				return this.currencyPositivePattern;
			}
			set
			{
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("CurrencyPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 3));
				}
				this.VerifyWritable();
				this.currencyPositivePattern = value;
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060057C2 RID: 22466 RVA: 0x0012850D File Offset: 0x0012670D
		// (set) Token: 0x060057C3 RID: 22467 RVA: 0x00128515 File Offset: 0x00126715
		public string PositiveInfinitySymbol
		{
			get
			{
				return this.positiveInfinitySymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PositiveInfinitySymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.positiveInfinitySymbol = value;
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x060057C4 RID: 22468 RVA: 0x0012853C File Offset: 0x0012673C
		// (set) Token: 0x060057C5 RID: 22469 RVA: 0x00128544 File Offset: 0x00126744
		public string PositiveSign
		{
			get
			{
				return this.positiveSign;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PositiveSign", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.positiveSign = value;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060057C6 RID: 22470 RVA: 0x0012856B File Offset: 0x0012676B
		// (set) Token: 0x060057C7 RID: 22471 RVA: 0x00128574 File Offset: 0x00126774
		public int PercentDecimalDigits
		{
			get
			{
				return this.percentDecimalDigits;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PercentDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 99));
				}
				this.VerifyWritable();
				this.percentDecimalDigits = value;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060057C8 RID: 22472 RVA: 0x001285C3 File Offset: 0x001267C3
		// (set) Token: 0x060057C9 RID: 22473 RVA: 0x001285CB File Offset: 0x001267CB
		public string PercentDecimalSeparator
		{
			get
			{
				return this.percentDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyDecimalSeparator(value, "PercentDecimalSeparator");
				this.percentDecimalSeparator = value;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060057CA RID: 22474 RVA: 0x001285E5 File Offset: 0x001267E5
		// (set) Token: 0x060057CB RID: 22475 RVA: 0x001285ED File Offset: 0x001267ED
		public string PercentGroupSeparator
		{
			get
			{
				return this.percentGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyGroupSeparator(value, "PercentGroupSeparator");
				this.percentGroupSeparator = value;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060057CC RID: 22476 RVA: 0x00128607 File Offset: 0x00126807
		// (set) Token: 0x060057CD RID: 22477 RVA: 0x0012860F File Offset: 0x0012680F
		public string PercentSymbol
		{
			get
			{
				return this.percentSymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PercentSymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.percentSymbol = value;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060057CE RID: 22478 RVA: 0x00128636 File Offset: 0x00126836
		// (set) Token: 0x060057CF RID: 22479 RVA: 0x0012863E File Offset: 0x0012683E
		public string PerMilleSymbol
		{
			get
			{
				return this.perMilleSymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PerMilleSymbol", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.VerifyWritable();
				this.perMilleSymbol = value;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060057D0 RID: 22480 RVA: 0x00128665 File Offset: 0x00126865
		// (set) Token: 0x060057D1 RID: 22481 RVA: 0x00128677 File Offset: 0x00126877
		[ComVisible(false)]
		public string[] NativeDigits
		{
			get
			{
				return (string[])this.nativeDigits.Clone();
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyNativeDigits(value, "NativeDigits");
				this.nativeDigits = value;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060057D2 RID: 22482 RVA: 0x00128691 File Offset: 0x00126891
		// (set) Token: 0x060057D3 RID: 22483 RVA: 0x00128699 File Offset: 0x00126899
		[ComVisible(false)]
		public DigitShapes DigitSubstitution
		{
			get
			{
				return (DigitShapes)this.digitSubstitution;
			}
			set
			{
				this.VerifyWritable();
				NumberFormatInfo.VerifyDigitSubstitution(value, "DigitSubstitution");
				this.digitSubstitution = (int)value;
			}
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x001286B3 File Offset: 0x001268B3
		public object GetFormat(Type formatType)
		{
			if (!(formatType == typeof(NumberFormatInfo)))
			{
				return null;
			}
			return this;
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x001286CA File Offset: 0x001268CA
		public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
		{
			if (nfi == null)
			{
				throw new ArgumentNullException("nfi");
			}
			if (nfi.IsReadOnly)
			{
				return nfi;
			}
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)nfi.MemberwiseClone();
			numberFormatInfo.isReadOnly = true;
			return numberFormatInfo;
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x001286F8 File Offset: 0x001268F8
		internal static void ValidateParseStyleInteger(NumberStyles style)
		{
			if ((style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("An undefined NumberStyles value is being used."), "style");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None && (style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber."));
			}
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x00128745 File Offset: 0x00126945
		internal static void ValidateParseStyleFloatingPoint(NumberStyles style)
		{
			if ((style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("An undefined NumberStyles value is being used."), "style");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("The number style AllowHexSpecifier is not supported on floating point data types."));
			}
		}

		// Token: 0x04003668 RID: 13928
		private static volatile NumberFormatInfo invariantInfo;

		// Token: 0x04003669 RID: 13929
		internal int[] numberGroupSizes = new int[]
		{
			3
		};

		// Token: 0x0400366A RID: 13930
		internal int[] currencyGroupSizes = new int[]
		{
			3
		};

		// Token: 0x0400366B RID: 13931
		internal int[] percentGroupSizes = new int[]
		{
			3
		};

		// Token: 0x0400366C RID: 13932
		internal string positiveSign = "+";

		// Token: 0x0400366D RID: 13933
		internal string negativeSign = "-";

		// Token: 0x0400366E RID: 13934
		internal string numberDecimalSeparator = ".";

		// Token: 0x0400366F RID: 13935
		internal string numberGroupSeparator = ",";

		// Token: 0x04003670 RID: 13936
		internal string currencyGroupSeparator = ",";

		// Token: 0x04003671 RID: 13937
		internal string currencyDecimalSeparator = ".";

		// Token: 0x04003672 RID: 13938
		internal string currencySymbol = "¤";

		// Token: 0x04003673 RID: 13939
		internal string ansiCurrencySymbol;

		// Token: 0x04003674 RID: 13940
		internal string nanSymbol = "NaN";

		// Token: 0x04003675 RID: 13941
		internal string positiveInfinitySymbol = "Infinity";

		// Token: 0x04003676 RID: 13942
		internal string negativeInfinitySymbol = "-Infinity";

		// Token: 0x04003677 RID: 13943
		internal string percentDecimalSeparator = ".";

		// Token: 0x04003678 RID: 13944
		internal string percentGroupSeparator = ",";

		// Token: 0x04003679 RID: 13945
		internal string percentSymbol = "%";

		// Token: 0x0400367A RID: 13946
		internal string perMilleSymbol = "‰";

		// Token: 0x0400367B RID: 13947
		[OptionalField(VersionAdded = 2)]
		internal string[] nativeDigits = new string[]
		{
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9"
		};

		// Token: 0x0400367C RID: 13948
		[OptionalField(VersionAdded = 1)]
		internal int m_dataItem;

		// Token: 0x0400367D RID: 13949
		internal int numberDecimalDigits = 2;

		// Token: 0x0400367E RID: 13950
		internal int currencyDecimalDigits = 2;

		// Token: 0x0400367F RID: 13951
		internal int currencyPositivePattern;

		// Token: 0x04003680 RID: 13952
		internal int currencyNegativePattern;

		// Token: 0x04003681 RID: 13953
		internal int numberNegativePattern = 1;

		// Token: 0x04003682 RID: 13954
		internal int percentPositivePattern;

		// Token: 0x04003683 RID: 13955
		internal int percentNegativePattern;

		// Token: 0x04003684 RID: 13956
		internal int percentDecimalDigits = 2;

		// Token: 0x04003685 RID: 13957
		[OptionalField(VersionAdded = 2)]
		internal int digitSubstitution = 1;

		// Token: 0x04003686 RID: 13958
		internal bool isReadOnly;

		// Token: 0x04003687 RID: 13959
		[OptionalField(VersionAdded = 1)]
		internal bool m_useUserOverride;

		// Token: 0x04003688 RID: 13960
		[OptionalField(VersionAdded = 2)]
		internal bool m_isInvariant;

		// Token: 0x04003689 RID: 13961
		[OptionalField(VersionAdded = 1)]
		internal bool validForParseAsNumber = true;

		// Token: 0x0400368A RID: 13962
		[OptionalField(VersionAdded = 1)]
		internal bool validForParseAsCurrency = true;

		// Token: 0x0400368B RID: 13963
		private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);
	}
}
