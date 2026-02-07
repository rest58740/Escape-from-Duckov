using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace System
{
	// Token: 0x020000CD RID: 205
	[Serializable]
	public sealed class String : IComparable, IEnumerable, IEnumerable<char>, IComparable<string>, IEquatable<string>, IConvertible, ICloneable
	{
		// Token: 0x060004DF RID: 1247 RVA: 0x00017E10 File Offset: 0x00016010
		private unsafe static int CompareOrdinalIgnoreCaseHelper(string strA, string strB)
		{
			int num = Math.Min(strA.Length, strB.Length);
			fixed (char* ptr = &strA._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &strB._firstChar)
				{
					char* ptr4 = ptr3;
					char* ptr5 = ptr2;
					char* ptr6 = ptr4;
					while (num != 0)
					{
						int num2 = (int)(*ptr5);
						int num3 = (int)(*ptr6);
						if (num2 - 97 <= 25)
						{
							num2 -= 32;
						}
						if (num3 - 97 <= 25)
						{
							num3 -= 32;
						}
						if (num2 != num3)
						{
							return num2 - num3;
						}
						ptr5++;
						ptr6++;
						num--;
					}
					return strA.Length - strB.Length;
				}
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00017EA5 File Offset: 0x000160A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool EqualsHelper(string strA, string strB)
		{
			return SpanHelpers.SequenceEqual(Unsafe.As<char, byte>(strA.GetRawStringData()), Unsafe.As<char, byte>(strB.GetRawStringData()), (ulong)((long)strA.Length * 2L));
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00017ECC File Offset: 0x000160CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CompareOrdinalHelper(string strA, int indexA, int countA, string strB, int indexB, int countB)
		{
			return SpanHelpers.SequenceCompareTo(Unsafe.Add<char>(strA.GetRawStringData(), indexA), countA, Unsafe.Add<char>(strB.GetRawStringData(), indexB), countB);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00017EF0 File Offset: 0x000160F0
		private unsafe static bool EqualsIgnoreCaseAsciiHelper(string strA, string strB)
		{
			int num = strA.Length;
			fixed (char* ptr = &strA._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &strB._firstChar)
				{
					char* ptr4 = ptr3;
					char* ptr5 = ptr2;
					char* ptr6 = ptr4;
					while (num != 0)
					{
						int num2 = (int)(*ptr5);
						int num3 = (int)(*ptr6);
						if (num2 != num3 && ((num2 | 32) != (num3 | 32) || (num2 | 32) - 97 > 25))
						{
							return false;
						}
						ptr5++;
						ptr6++;
						num--;
					}
					return true;
				}
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00017F60 File Offset: 0x00016160
		private unsafe static int CompareOrdinalHelper(string strA, string strB)
		{
			int i = Math.Min(strA.Length, strB.Length);
			fixed (char* ptr = &strA._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &strB._firstChar)
				{
					char* ptr4 = ptr3;
					char* ptr5 = ptr2;
					char* ptr6 = ptr4;
					if (ptr5[1] == ptr6[1])
					{
						i -= 2;
						ptr5 += 2;
						ptr6 += 2;
						while (i >= 12)
						{
							if (*(long*)ptr5 == *(long*)ptr6)
							{
								if (*(long*)(ptr5 + 4) == *(long*)(ptr6 + 4))
								{
									if (*(long*)(ptr5 + 8) == *(long*)(ptr6 + 8))
									{
										i -= 12;
										ptr5 += 12;
										ptr6 += 12;
										continue;
									}
									ptr5 += 4;
									ptr6 += 4;
								}
								ptr5 += 4;
								ptr6 += 4;
							}
							if (*(int*)ptr5 == *(int*)ptr6)
							{
								ptr5 += 2;
								ptr6 += 2;
							}
							IL_10E:
							if (*ptr5 != *ptr6)
							{
								return (int)(*ptr5 - *ptr6);
							}
							goto IL_11E;
						}
						while (i > 0)
						{
							if (*(int*)ptr5 != *(int*)ptr6)
							{
								goto IL_10E;
							}
							i -= 2;
							ptr5 += 2;
							ptr6 += 2;
						}
						return strA.Length - strB.Length;
					}
					IL_11E:
					return (int)(ptr5[1] - ptr6[1]);
				}
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00018096 File Offset: 0x00016296
		public static int Compare(string strA, string strB)
		{
			return string.Compare(strA, strB, StringComparison.CurrentCulture);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000180A0 File Offset: 0x000162A0
		public static int Compare(string strA, string strB, bool ignoreCase)
		{
			StringComparison comparisonType = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
			return string.Compare(strA, strB, comparisonType);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000180C0 File Offset: 0x000162C0
		public static int Compare(string strA, string strB, StringComparison comparisonType)
		{
			if (strA == strB)
			{
				string.CheckStringComparison(comparisonType);
				return 0;
			}
			if (strA == null)
			{
				string.CheckStringComparison(comparisonType);
				return -1;
			}
			if (strB == null)
			{
				string.CheckStringComparison(comparisonType);
				return 1;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.Compare(strA, strB, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.Compare(strA, strB, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				if (strA._firstChar != strB._firstChar)
				{
					return (int)(strA._firstChar - strB._firstChar);
				}
				return string.CompareOrdinalHelper(strA, strB);
			case StringComparison.OrdinalIgnoreCase:
				return CompareInfo.CompareOrdinalIgnoreCase(strA, 0, strA.Length, strB, 0, strB.Length);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001819A File Offset: 0x0001639A
		public static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.CompareInfo.Compare(strA, strB, options);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000181B8 File Offset: 0x000163B8
		public static int Compare(string strA, string strB, bool ignoreCase, CultureInfo culture)
		{
			CompareOptions options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
			return string.Compare(strA, strB, culture, options);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000181D6 File Offset: 0x000163D6
		public static int Compare(string strA, int indexA, string strB, int indexB, int length)
		{
			return string.Compare(strA, indexA, strB, indexB, length, false);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000181E4 File Offset: 0x000163E4
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase)
		{
			int num = length;
			int num2 = length;
			if (strA != null)
			{
				num = Math.Min(num, strA.Length - indexA);
			}
			if (strB != null)
			{
				num2 = Math.Min(num2, strB.Length - indexB);
			}
			CompareOptions options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, options);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001823C File Offset: 0x0001643C
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
		{
			CompareOptions options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
			return string.Compare(strA, indexA, strB, indexB, length, culture, options);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00018260 File Offset: 0x00016460
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			int num = length;
			int num2 = length;
			if (strA != null)
			{
				num = Math.Min(num, strA.Length - indexA);
			}
			if (strB != null)
			{
				num2 = Math.Min(num2, strB.Length - indexB);
			}
			return culture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, options);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000182BC File Offset: 0x000164BC
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType)
		{
			string.CheckStringComparison(comparisonType);
			if (strA == null || strB == null)
			{
				if (strA == strB)
				{
					return 0;
				}
				if (strA != null)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
				}
				if (indexA < 0 || indexB < 0)
				{
					throw new ArgumentOutOfRangeException((indexA < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (strA.Length - indexA < 0 || strB.Length - indexB < 0)
				{
					throw new ArgumentOutOfRangeException((strA.Length - indexA < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (length == 0 || (strA == strB && indexA == indexB))
				{
					return 0;
				}
				int num = Math.Min(length, strA.Length - indexA);
				int num2 = Math.Min(length, strB.Length - indexB);
				switch (comparisonType)
				{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
				case StringComparison.InvariantCulture:
					return CompareInfo.Invariant.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
				case StringComparison.InvariantCultureIgnoreCase:
					return CompareInfo.Invariant.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return string.CompareOrdinalHelper(strA, indexA, num, strB, indexB, num2);
				case StringComparison.OrdinalIgnoreCase:
					return CompareInfo.CompareOrdinalIgnoreCase(strA, indexA, num, strB, indexB, num2);
				default:
					throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001841B File Offset: 0x0001661B
		public static int CompareOrdinal(string strA, string strB)
		{
			if (strA == strB)
			{
				return 0;
			}
			if (strA == null)
			{
				return -1;
			}
			if (strB == null)
			{
				return 1;
			}
			if (strA._firstChar != strB._firstChar)
			{
				return (int)(strA._firstChar - strB._firstChar);
			}
			return string.CompareOrdinalHelper(strA, strB);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00018450 File Offset: 0x00016650
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int CompareOrdinal(ReadOnlySpan<char> strA, ReadOnlySpan<char> strB)
		{
			return SpanHelpers.SequenceCompareTo(MemoryMarshal.GetReference<char>(strA), strA.Length, MemoryMarshal.GetReference<char>(strB), strB.Length);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00018474 File Offset: 0x00016674
		public static int CompareOrdinal(string strA, int indexA, string strB, int indexB, int length)
		{
			if (strA == null || strB == null)
			{
				if (strA == strB)
				{
					return 0;
				}
				if (strA != null)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException("length", "Count cannot be less than zero.");
				}
				if (indexA < 0 || indexB < 0)
				{
					throw new ArgumentOutOfRangeException((indexA < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				int num = Math.Min(length, strA.Length - indexA);
				int num2 = Math.Min(length, strB.Length - indexB);
				if (num < 0 || num2 < 0)
				{
					throw new ArgumentOutOfRangeException((num < 0) ? "indexA" : "indexB", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (length == 0 || (strA == strB && indexA == indexB))
				{
					return 0;
				}
				return string.CompareOrdinalHelper(strA, indexA, num, strB, indexB, num2);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00018528 File Offset: 0x00016728
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			string text = value as string;
			if (text == null)
			{
				throw new ArgumentException("Object must be of type String.");
			}
			return this.CompareTo(text);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00018096 File Offset: 0x00016296
		public int CompareTo(string strB)
		{
			return string.Compare(this, strB, StringComparison.CurrentCulture);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00018556 File Offset: 0x00016756
		public bool EndsWith(string value)
		{
			return this.EndsWith(value, StringComparison.CurrentCulture);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00018560 File Offset: 0x00016760
		public bool EndsWith(string value, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this == value)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (value.Length == 0)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IsSuffix(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IsSuffix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Length >= value.Length && string.CompareOrdinalHelper(this, this.Length - value.Length, value.Length, value, 0, value.Length) == 0;
			case StringComparison.OrdinalIgnoreCase:
				return this.Length >= value.Length && CompareInfo.CompareOrdinalIgnoreCase(this, this.Length - value.Length, value.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00018675 File Offset: 0x00016875
		public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this == value || (culture ?? CultureInfo.CurrentCulture).CompareInfo.IsSuffix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000186A8 File Offset: 0x000168A8
		public bool EndsWith(char value)
		{
			int length = this.Length;
			return length != 0 && this[length - 1] == value;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000186D0 File Offset: 0x000168D0
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			string text = obj as string;
			return text != null && this.Length == text.Length && string.EqualsHelper(this, text);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00018706 File Offset: 0x00016906
		public bool Equals(string value)
		{
			return this == value || (value != null && this.Length == value.Length && string.EqualsHelper(this, value));
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001872C File Offset: 0x0001692C
		public bool Equals(string value, StringComparison comparisonType)
		{
			if (this == value)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (value == null)
			{
				string.CheckStringComparison(comparisonType);
				return false;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.None) == 0;
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.IgnoreCase) == 0;
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.Compare(this, value, CompareOptions.None) == 0;
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.Compare(this, value, CompareOptions.IgnoreCase) == 0;
			case StringComparison.Ordinal:
				return this.Length == value.Length && string.EqualsHelper(this, value);
			case StringComparison.OrdinalIgnoreCase:
				return this.Length == value.Length && CompareInfo.CompareOrdinalIgnoreCase(this, 0, this.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00018811 File Offset: 0x00016A11
		public static bool Equals(string a, string b)
		{
			return a == b || (a != null && b != null && a.Length == b.Length && string.EqualsHelper(a, b));
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00018838 File Offset: 0x00016A38
		public static bool Equals(string a, string b, StringComparison comparisonType)
		{
			if (a == b)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (a == null || b == null)
			{
				string.CheckStringComparison(comparisonType);
				return false;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0;
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0;
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.Compare(a, b, CompareOptions.None) == 0;
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.Compare(a, b, CompareOptions.IgnoreCase) == 0;
			case StringComparison.Ordinal:
				return a.Length == b.Length && string.EqualsHelper(a, b);
			case StringComparison.OrdinalIgnoreCase:
				return a.Length == b.Length && CompareInfo.CompareOrdinalIgnoreCase(a, 0, a.Length, b, 0, b.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00018920 File Offset: 0x00016B20
		public static bool operator ==(string a, string b)
		{
			return string.Equals(a, b);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00018929 File Offset: 0x00016B29
		public static bool operator !=(string a, string b)
		{
			return !string.Equals(a, b);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00018935 File Offset: 0x00016B35
		public override int GetHashCode()
		{
			return this.GetLegacyNonRandomizedHashCode();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001893D File Offset: 0x00016B3D
		public int GetHashCode(StringComparison comparisonType)
		{
			return StringComparer.FromComparison(comparisonType).GetHashCode(this);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001894C File Offset: 0x00016B4C
		internal unsafe int GetLegacyNonRandomizedHashCode()
		{
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				int num = 5381;
				int num2 = num;
				char* ptr3 = ptr2;
				int num3;
				while ((num3 = (int)(*ptr3)) != 0)
				{
					num = ((num << 5) + num ^ num3);
					num3 = (int)ptr3[1];
					if (num3 == 0)
					{
						break;
					}
					num2 = ((num2 << 5) + num2 ^ num3);
					ptr3 += 2;
				}
				return num + num2 * 1566083941;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000189A0 File Offset: 0x00016BA0
		public bool StartsWith(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this.StartsWith(value, StringComparison.CurrentCulture);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000189B8 File Offset: 0x00016BB8
		public bool StartsWith(string value, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this == value)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			if (value.Length == 0)
			{
				string.CheckStringComparison(comparisonType);
				return true;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IsPrefix(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IsPrefix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Length >= value.Length && this._firstChar == value._firstChar && (value.Length == 1 || SpanHelpers.SequenceEqual(Unsafe.As<char, byte>(this.GetRawStringData()), Unsafe.As<char, byte>(value.GetRawStringData()), (ulong)((long)value.Length * 2L)));
			case StringComparison.OrdinalIgnoreCase:
				return this.Length >= value.Length && CompareInfo.CompareOrdinalIgnoreCase(this, 0, value.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00018ADB File Offset: 0x00016CDB
		public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this == value || (culture ?? CultureInfo.CurrentCulture).CompareInfo.IsPrefix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00018B0E File Offset: 0x00016D0E
		public bool StartsWith(char value)
		{
			return this.Length != 0 && this._firstChar == value;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00018B23 File Offset: 0x00016D23
		internal static void CheckStringComparison(StringComparison comparisonType)
		{
			if (comparisonType - StringComparison.CurrentCulture > 5)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.NotSupported_StringComparison, ExceptionArgument.comparisonType);
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00018B34 File Offset: 0x00016D34
		private unsafe static void FillStringChecked(string dest, int destPos, string src)
		{
			if (src.Length > dest.Length - destPos)
			{
				throw new IndexOutOfRangeException();
			}
			fixed (char* ptr = &dest._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &src._firstChar)
				{
					char* smem = ptr3;
					string.wstrcpy(ptr2 + destPos, smem, src.Length);
				}
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00018B81 File Offset: 0x00016D81
		public static string Concat(object arg0)
		{
			if (arg0 == null)
			{
				return string.Empty;
			}
			return arg0.ToString();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00018B92 File Offset: 0x00016D92
		public static string Concat(object arg0, object arg1)
		{
			if (arg0 == null)
			{
				arg0 = string.Empty;
			}
			if (arg1 == null)
			{
				arg1 = string.Empty;
			}
			return arg0.ToString() + arg1.ToString();
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00018BB9 File Offset: 0x00016DB9
		public static string Concat(object arg0, object arg1, object arg2)
		{
			if (arg0 == null)
			{
				arg0 = string.Empty;
			}
			if (arg1 == null)
			{
				arg1 = string.Empty;
			}
			if (arg2 == null)
			{
				arg2 = string.Empty;
			}
			return arg0.ToString() + arg1.ToString() + arg2.ToString();
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00018BF0 File Offset: 0x00016DF0
		public static string Concat(params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (args.Length <= 1)
			{
				string result;
				if (args.Length != 0)
				{
					object obj = args[0];
					if ((result = ((obj != null) ? obj.ToString() : null)) == null)
					{
						return string.Empty;
					}
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
			string[] array = new string[args.Length];
			int num = 0;
			for (int i = 0; i < args.Length; i++)
			{
				object obj2 = args[i];
				string text = ((obj2 != null) ? obj2.ToString() : null) ?? string.Empty;
				array[i] = text;
				num += text.Length;
				if (num < 0)
				{
					throw new OutOfMemoryException();
				}
			}
			if (num == 0)
			{
				return string.Empty;
			}
			string text2 = string.FastAllocateString(num);
			int num2 = 0;
			foreach (string text3 in array)
			{
				string.FillStringChecked(text2, num2, text3);
				num2 += text3.Length;
			}
			return text2;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00018CC4 File Offset: 0x00016EC4
		public static string Concat<T>(IEnumerable<T> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (typeof(T) == typeof(char))
			{
				using (IEnumerator<char> enumerator = Unsafe.As<IEnumerable<char>>(values).GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return string.Empty;
					}
					char c = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						return string.CreateFromChar(c);
					}
					StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
					stringBuilder.Append(c);
					do
					{
						c = enumerator.Current;
						stringBuilder.Append(c);
					}
					while (enumerator.MoveNext());
					return StringBuilderCache.GetStringAndRelease(stringBuilder);
				}
			}
			string result;
			using (IEnumerator<T> enumerator2 = values.GetEnumerator())
			{
				if (!enumerator2.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					T t = enumerator2.Current;
					string text = (t != null) ? t.ToString() : null;
					if (!enumerator2.MoveNext())
					{
						result = (text ?? string.Empty);
					}
					else
					{
						StringBuilder stringBuilder2 = StringBuilderCache.Acquire(16);
						stringBuilder2.Append(text);
						do
						{
							t = enumerator2.Current;
							if (t != null)
							{
								stringBuilder2.Append(t.ToString());
							}
						}
						while (enumerator2.MoveNext());
						result = StringBuilderCache.GetStringAndRelease(stringBuilder2);
					}
				}
			}
			return result;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00018E40 File Offset: 0x00017040
		public static string Concat(IEnumerable<string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string result;
			using (IEnumerator<string> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					string text = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						result = (text ?? string.Empty);
					}
					else
					{
						StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
						stringBuilder.Append(text);
						do
						{
							stringBuilder.Append(enumerator.Current);
						}
						while (enumerator.MoveNext());
						result = StringBuilderCache.GetStringAndRelease(stringBuilder);
					}
				}
			}
			return result;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00018ED8 File Offset: 0x000170D8
		public static string Concat(string str0, string str1)
		{
			if (string.IsNullOrEmpty(str0))
			{
				if (string.IsNullOrEmpty(str1))
				{
					return string.Empty;
				}
				return str1;
			}
			else
			{
				if (string.IsNullOrEmpty(str1))
				{
					return str0;
				}
				int length = str0.Length;
				string text = string.FastAllocateString(length + str1.Length);
				string.FillStringChecked(text, 0, str0);
				string.FillStringChecked(text, length, str1);
				return text;
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00018F2C File Offset: 0x0001712C
		public static string Concat(string str0, string str1, string str2)
		{
			if (string.IsNullOrEmpty(str0))
			{
				return str1 + str2;
			}
			if (string.IsNullOrEmpty(str1))
			{
				return str0 + str2;
			}
			if (string.IsNullOrEmpty(str2))
			{
				return str0 + str1;
			}
			string text = string.FastAllocateString(str0.Length + str1.Length + str2.Length);
			string.FillStringChecked(text, 0, str0);
			string.FillStringChecked(text, str0.Length, str1);
			string.FillStringChecked(text, str0.Length + str1.Length, str2);
			return text;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00018FAC File Offset: 0x000171AC
		public static string Concat(string str0, string str1, string str2, string str3)
		{
			if (string.IsNullOrEmpty(str0))
			{
				return str1 + str2 + str3;
			}
			if (string.IsNullOrEmpty(str1))
			{
				return str0 + str2 + str3;
			}
			if (string.IsNullOrEmpty(str2))
			{
				return str0 + str1 + str3;
			}
			if (string.IsNullOrEmpty(str3))
			{
				return str0 + str1 + str2;
			}
			string text = string.FastAllocateString(str0.Length + str1.Length + str2.Length + str3.Length);
			string.FillStringChecked(text, 0, str0);
			string.FillStringChecked(text, str0.Length, str1);
			string.FillStringChecked(text, str0.Length + str1.Length, str2);
			string.FillStringChecked(text, str0.Length + str1.Length + str2.Length, str3);
			return text;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00019064 File Offset: 0x00017264
		public static string Concat(params string[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length <= 1)
			{
				string result;
				if (values.Length != 0)
				{
					if ((result = values[0]) == null)
					{
						return string.Empty;
					}
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
			long num = 0L;
			foreach (string text in values)
			{
				if (text != null)
				{
					num += (long)text.Length;
				}
			}
			if (num > 2147483647L)
			{
				throw new OutOfMemoryException();
			}
			int num2 = (int)num;
			if (num2 == 0)
			{
				return string.Empty;
			}
			string text2 = string.FastAllocateString(num2);
			int num3 = 0;
			foreach (string text3 in values)
			{
				if (!string.IsNullOrEmpty(text3))
				{
					int length = text3.Length;
					if (length > num2 - num3)
					{
						num3 = -1;
						break;
					}
					string.FillStringChecked(text2, num3, text3);
					num3 += length;
				}
			}
			if (num3 != num2)
			{
				return string.Concat((string[])values.Clone());
			}
			return text2;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00019143 File Offset: 0x00017343
		public static string Format(string format, object arg0)
		{
			return string.FormatHelper(null, format, new ParamsArray(arg0));
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00019152 File Offset: 0x00017352
		public static string Format(string format, object arg0, object arg1)
		{
			return string.FormatHelper(null, format, new ParamsArray(arg0, arg1));
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00019162 File Offset: 0x00017362
		public static string Format(string format, object arg0, object arg1, object arg2)
		{
			return string.FormatHelper(null, format, new ParamsArray(arg0, arg1, arg2));
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00019173 File Offset: 0x00017373
		public static string Format(string format, params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}
			return string.FormatHelper(null, format, new ParamsArray(args));
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001919A File Offset: 0x0001739A
		public static string Format(IFormatProvider provider, string format, object arg0)
		{
			return string.FormatHelper(provider, format, new ParamsArray(arg0));
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000191A9 File Offset: 0x000173A9
		public static string Format(IFormatProvider provider, string format, object arg0, object arg1)
		{
			return string.FormatHelper(provider, format, new ParamsArray(arg0, arg1));
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000191B9 File Offset: 0x000173B9
		public static string Format(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
		{
			return string.FormatHelper(provider, format, new ParamsArray(arg0, arg1, arg2));
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000191CB File Offset: 0x000173CB
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}
			return string.FormatHelper(provider, format, new ParamsArray(args));
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000191F2 File Offset: 0x000173F2
		private static string FormatHelper(IFormatProvider provider, string format, ParamsArray args)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			return StringBuilderCache.GetStringAndRelease(StringBuilderCache.Acquire(format.Length + args.Length * 8).AppendFormatHelper(provider, format, args));
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00019224 File Offset: 0x00017424
		public unsafe string Insert(int startIndex, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			int length = this.Length;
			int length2 = value.Length;
			if (length == 0)
			{
				return value;
			}
			if (length2 == 0)
			{
				return this;
			}
			string text = string.FastAllocateString(length + length2);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &value._firstChar)
				{
					char* smem = ptr3;
					fixed (char* ptr4 = &text._firstChar)
					{
						char* ptr5 = ptr4;
						string.wstrcpy(ptr5, ptr2, startIndex);
						string.wstrcpy(ptr5 + startIndex, smem, length2);
						string.wstrcpy(ptr5 + startIndex + length2, ptr2 + startIndex, length - startIndex);
					}
				}
			}
			return text;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000192D2 File Offset: 0x000174D2
		public static string Join(char separator, params string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return string.Join(separator, value, 0, value.Length);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000192ED File Offset: 0x000174ED
		public unsafe static string Join(char separator, params object[] values)
		{
			return string.JoinCore(&separator, 1, values);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000192F9 File Offset: 0x000174F9
		public unsafe static string Join<T>(char separator, IEnumerable<T> values)
		{
			return string.JoinCore<T>(&separator, 1, values);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019305 File Offset: 0x00017505
		public unsafe static string Join(char separator, string[] value, int startIndex, int count)
		{
			return string.JoinCore(&separator, 1, value, startIndex, count);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00019313 File Offset: 0x00017513
		public static string Join(string separator, params string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return string.Join(separator, value, 0, value.Length);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019330 File Offset: 0x00017530
		public unsafe static string Join(string separator, params object[] values)
		{
			separator = (separator ?? string.Empty);
			fixed (char* ptr = &separator._firstChar)
			{
				return string.JoinCore(ptr, separator.Length, values);
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00019360 File Offset: 0x00017560
		public unsafe static string Join<T>(string separator, IEnumerable<T> values)
		{
			separator = (separator ?? string.Empty);
			fixed (char* ptr = &separator._firstChar)
			{
				return string.JoinCore<T>(ptr, separator.Length, values);
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00019390 File Offset: 0x00017590
		public static string Join(string separator, IEnumerable<string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string result;
			using (IEnumerator<string> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					string text = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						result = (text ?? string.Empty);
					}
					else
					{
						StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
						stringBuilder.Append(text);
						do
						{
							stringBuilder.Append(separator);
							stringBuilder.Append(enumerator.Current);
						}
						while (enumerator.MoveNext());
						result = StringBuilderCache.GetStringAndRelease(stringBuilder);
					}
				}
			}
			return result;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00019430 File Offset: 0x00017630
		public unsafe static string Join(string separator, string[] value, int startIndex, int count)
		{
			separator = (separator ?? string.Empty);
			fixed (char* ptr = &separator._firstChar)
			{
				return string.JoinCore(ptr, separator.Length, value, startIndex, count);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00019460 File Offset: 0x00017660
		private unsafe static string JoinCore(char* separator, int separatorLength, object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				return string.Empty;
			}
			object obj = values[0];
			string text = (obj != null) ? obj.ToString() : null;
			if (values.Length == 1)
			{
				return text ?? string.Empty;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			stringBuilder.Append(text);
			for (int i = 1; i < values.Length; i++)
			{
				stringBuilder.Append(separator, separatorLength);
				object obj2 = values[i];
				if (obj2 != null)
				{
					stringBuilder.Append(obj2.ToString());
				}
			}
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000194E8 File Offset: 0x000176E8
		private unsafe static string JoinCore<T>(char* separator, int separatorLength, IEnumerable<T> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string result;
			using (IEnumerator<T> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					T t = enumerator.Current;
					string text = (t != null) ? t.ToString() : null;
					if (!enumerator.MoveNext())
					{
						result = (text ?? string.Empty);
					}
					else
					{
						StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
						stringBuilder.Append(text);
						do
						{
							t = enumerator.Current;
							stringBuilder.Append(separator, separatorLength);
							if (t != null)
							{
								stringBuilder.Append(t.ToString());
							}
						}
						while (enumerator.MoveNext());
						result = StringBuilderCache.GetStringAndRelease(stringBuilder);
					}
				}
			}
			return result;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000195C0 File Offset: 0x000177C0
		private unsafe static string JoinCore(char* separator, int separatorLength, string[] value, int startIndex, int count)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (startIndex > value.Length - count)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index and count must refer to a location within the buffer.");
			}
			if (count <= 1)
			{
				string result;
				if (count != 0)
				{
					if ((result = value[startIndex]) == null)
					{
						return string.Empty;
					}
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
			long num = (long)(count - 1) * (long)separatorLength;
			if (num > 2147483647L)
			{
				throw new OutOfMemoryException();
			}
			int num2 = (int)num;
			int i = startIndex;
			int num3 = startIndex + count;
			while (i < num3)
			{
				string text = value[i];
				if (text != null)
				{
					num2 += text.Length;
					if (num2 < 0)
					{
						throw new OutOfMemoryException();
					}
				}
				i++;
			}
			string text2 = string.FastAllocateString(num2);
			int num4 = 0;
			int j = startIndex;
			int num5 = startIndex + count;
			while (j < num5)
			{
				string text3 = value[j];
				if (text3 != null)
				{
					int length = text3.Length;
					if (length > num2 - num4)
					{
						num4 = -1;
						break;
					}
					string.FillStringChecked(text2, num4, text3);
					num4 += length;
				}
				if (j < num5 - 1)
				{
					fixed (char* ptr = &text2._firstChar)
					{
						char* ptr2 = ptr;
						if (separatorLength == 1)
						{
							ptr2[num4] = *separator;
						}
						else
						{
							string.wstrcpy(ptr2 + num4, separator, separatorLength);
						}
					}
					num4 += separatorLength;
				}
				j++;
			}
			if (num4 != num2)
			{
				return string.JoinCore(separator, separatorLength, (string[])value.Clone(), startIndex, count);
			}
			return text2;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00019724 File Offset: 0x00017924
		public string PadLeft(int totalWidth)
		{
			return this.PadLeft(totalWidth, ' ');
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00019730 File Offset: 0x00017930
		public unsafe string PadLeft(int totalWidth, char paddingChar)
		{
			if (totalWidth < 0)
			{
				throw new ArgumentOutOfRangeException("totalWidth", "Non-negative number required.");
			}
			int length = this.Length;
			int num = totalWidth - length;
			if (num <= 0)
			{
				return this;
			}
			string text = string.FastAllocateString(totalWidth);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				for (int i = 0; i < num; i++)
				{
					ptr2[i] = paddingChar;
				}
				fixed (char* ptr3 = &this._firstChar)
				{
					char* smem = ptr3;
					string.wstrcpy(ptr2 + num, smem, length);
				}
			}
			return text;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000197B2 File Offset: 0x000179B2
		public string PadRight(int totalWidth)
		{
			return this.PadRight(totalWidth, ' ');
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000197C0 File Offset: 0x000179C0
		public unsafe string PadRight(int totalWidth, char paddingChar)
		{
			if (totalWidth < 0)
			{
				throw new ArgumentOutOfRangeException("totalWidth", "Non-negative number required.");
			}
			int length = this.Length;
			int num = totalWidth - length;
			if (num <= 0)
			{
				return this;
			}
			string text = string.FastAllocateString(totalWidth);
			fixed (char* ptr = &text._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &this._firstChar)
				{
					char* smem = ptr3;
					string.wstrcpy(ptr2, smem, length);
				}
				for (int i = 0; i < num; i++)
				{
					ptr2[length + i] = paddingChar;
				}
			}
			return text;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00019840 File Offset: 0x00017A40
		public unsafe string Remove(int startIndex, int count)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			int length = this.Length;
			if (count > length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Index and count must refer to a location within the string.");
			}
			if (count == 0)
			{
				return this;
			}
			int num = length - count;
			if (num == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(num);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char* ptr3 = &text._firstChar)
				{
					char* ptr4 = ptr3;
					string.wstrcpy(ptr4, ptr2, startIndex);
					string.wstrcpy(ptr4 + startIndex, ptr2 + startIndex + count, num - startIndex);
				}
			}
			return text;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000198E6 File Offset: 0x00017AE6
		public string Remove(int startIndex)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "startIndex must be less than length of string.");
			}
			return this.Substring(0, startIndex);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001991D File Offset: 0x00017B1D
		public string Replace(string oldValue, string newValue, bool ignoreCase, CultureInfo culture)
		{
			return this.ReplaceCore(oldValue, newValue, culture, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00019930 File Offset: 0x00017B30
		public string Replace(string oldValue, string newValue, StringComparison comparisonType)
		{
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.CurrentCulture, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.InvariantCulture, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Replace(oldValue, newValue);
			case StringComparison.OrdinalIgnoreCase:
				return this.ReplaceCore(oldValue, newValue, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000199C4 File Offset: 0x00017BC4
		private unsafe string ReplaceCore(string oldValue, string newValue, CultureInfo culture, CompareOptions options)
		{
			if (oldValue == null)
			{
				throw new ArgumentNullException("oldValue");
			}
			if (oldValue.Length == 0)
			{
				throw new ArgumentException("String cannot be of zero length.", "oldValue");
			}
			if (newValue == null)
			{
				newValue = string.Empty;
			}
			CultureInfo cultureInfo = culture ?? CultureInfo.CurrentCulture;
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			int num = 0;
			int num2 = 0;
			bool flag = false;
			CompareInfo compareInfo = cultureInfo.CompareInfo;
			for (;;)
			{
				int num3 = compareInfo.IndexOf(this, oldValue, num, this.Length - num, options, &num2);
				if (num3 >= 0)
				{
					stringBuilder.Append(this, num, num3 - num);
					stringBuilder.Append(newValue);
					num = num3 + num2;
					flag = true;
				}
				else
				{
					if (!flag)
					{
						break;
					}
					stringBuilder.Append(this, num, this.Length - num);
				}
				if (num3 < 0)
				{
					goto Block_7;
				}
			}
			StringBuilderCache.Release(stringBuilder);
			return this;
			Block_7:
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00019A84 File Offset: 0x00017C84
		public unsafe string Replace(char oldChar, char newChar)
		{
			if (oldChar == newChar)
			{
				return this;
			}
			int num = this.Length;
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				while (num > 0 && *ptr2 != oldChar)
				{
					num--;
					ptr2++;
				}
			}
			if (num == 0)
			{
				return this;
			}
			string text = string.FastAllocateString(this.Length);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr3 = ptr;
				fixed (char* ptr4 = &text._firstChar)
				{
					char* ptr5 = ptr4;
					int num2 = this.Length - num;
					if (num2 > 0)
					{
						string.wstrcpy(ptr5, ptr3, num2);
					}
					char* ptr6 = ptr3 + num2;
					char* ptr7 = ptr5 + num2;
					do
					{
						char c = *ptr6;
						if (c == oldChar)
						{
							c = newChar;
						}
						*ptr7 = c;
						num--;
						ptr6++;
						ptr7++;
					}
					while (num > 0);
				}
			}
			return text;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00019B44 File Offset: 0x00017D44
		public unsafe string Replace(string oldValue, string newValue)
		{
			if (oldValue == null)
			{
				throw new ArgumentNullException("oldValue");
			}
			if (oldValue.Length == 0)
			{
				throw new ArgumentException("String cannot be of zero length.", "oldValue");
			}
			if (newValue == null)
			{
				newValue = string.Empty;
			}
			Span<int> initialSpan = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(initialSpan);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				int i = 0;
				int num = this.Length - oldValue.Length;
				IL_B6:
				while (i <= num)
				{
					char* ptr3 = ptr2 + i;
					for (int j = 0; j < oldValue.Length; j++)
					{
						if (ptr3[j] != oldValue[j])
						{
							i++;
							goto IL_B6;
						}
					}
					valueListBuilder.Append(i);
					i += oldValue.Length;
				}
			}
			if (valueListBuilder.Length == 0)
			{
				return this;
			}
			string result = this.ReplaceHelper(oldValue.Length, newValue, valueListBuilder.AsSpan());
			valueListBuilder.Dispose();
			return result;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00019C38 File Offset: 0x00017E38
		private unsafe string ReplaceHelper(int oldValueLength, string newValue, ReadOnlySpan<int> indices)
		{
			long num = (long)this.Length + (long)(newValue.Length - oldValueLength) * (long)indices.Length;
			if (num > 2147483647L)
			{
				throw new OutOfMemoryException();
			}
			string text = string.FastAllocateString((int)num);
			Span<char> span = new Span<char>(text.GetRawStringData(), text.Length);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < indices.Length; i++)
			{
				int num4 = *indices[i];
				int num5 = num4 - num2;
				if (num5 != 0)
				{
					this.AsSpan(num2, num5).CopyTo(span.Slice(num3));
					num3 += num5;
				}
				num2 = num4 + oldValueLength;
				newValue.AsSpan().CopyTo(span.Slice(num3));
				num3 += newValue.Length;
			}
			this.AsSpan(num2).CopyTo(span.Slice(num3));
			return text;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00019D0F File Offset: 0x00017F0F
		public string[] Split(char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(new ReadOnlySpan<char>(ref separator, 1), int.MaxValue, options);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00019D25 File Offset: 0x00017F25
		public string[] Split(char separator, int count, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(new ReadOnlySpan<char>(ref separator, 1), count, options);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00019D37 File Offset: 0x00017F37
		public string[] Split(params char[] separator)
		{
			return this.SplitInternal(separator, int.MaxValue, StringSplitOptions.None);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00019D4B File Offset: 0x00017F4B
		public string[] Split(char[] separator, int count)
		{
			return this.SplitInternal(separator, count, StringSplitOptions.None);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00019D5B File Offset: 0x00017F5B
		public string[] Split(char[] separator, StringSplitOptions options)
		{
			return this.SplitInternal(separator, int.MaxValue, options);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00019D6F File Offset: 0x00017F6F
		public string[] Split(char[] separator, int count, StringSplitOptions options)
		{
			return this.SplitInternal(separator, count, options);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00019D80 File Offset: 0x00017F80
		private unsafe string[] SplitInternal(ReadOnlySpan<char> separators, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				throw new ArgumentException(SR.Format("Illegal enum value: {0}.", options));
			}
			bool flag = options == StringSplitOptions.RemoveEmptyEntries;
			if (count == 0 || (flag && this.Length == 0))
			{
				return Array.Empty<string>();
			}
			if (count == 1)
			{
				return new string[]
				{
					this
				};
			}
			Span<int> initialSpan = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(initialSpan);
			this.MakeSeparatorList(separators, ref valueListBuilder);
			ReadOnlySpan<int> sepList = valueListBuilder.AsSpan();
			if (sepList.Length == 0)
			{
				return new string[]
				{
					this
				};
			}
			string[] result = flag ? this.SplitOmitEmptyEntries(sepList, default(ReadOnlySpan<int>), 1, count) : this.SplitKeepEmptyEntries(sepList, default(ReadOnlySpan<int>), 1, count);
			valueListBuilder.Dispose();
			return result;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019E59 File Offset: 0x00018059
		public string[] Split(string separator, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(separator ?? string.Empty, null, int.MaxValue, options);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00019E72 File Offset: 0x00018072
		public string[] Split(string separator, int count, StringSplitOptions options = StringSplitOptions.None)
		{
			return this.SplitInternal(separator ?? string.Empty, null, count, options);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00019E87 File Offset: 0x00018087
		public string[] Split(string[] separator, StringSplitOptions options)
		{
			return this.SplitInternal(null, separator, int.MaxValue, options);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00019E97 File Offset: 0x00018097
		public string[] Split(string[] separator, int count, StringSplitOptions options)
		{
			return this.SplitInternal(null, separator, count, options);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00019EA4 File Offset: 0x000180A4
		private unsafe string[] SplitInternal(string separator, string[] separators, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				throw new ArgumentException(SR.Format("Illegal enum value: {0}.", (int)options));
			}
			bool flag = options == StringSplitOptions.RemoveEmptyEntries;
			bool flag2 = separator != null;
			if (!flag2 && (separators == null || separators.Length == 0))
			{
				return this.SplitInternal(null, count, options);
			}
			if (count == 0 || (flag && this.Length == 0))
			{
				return Array.Empty<string>();
			}
			if (count == 1 || (flag2 && separator.Length == 0))
			{
				return new string[]
				{
					this
				};
			}
			if (flag2)
			{
				return this.SplitInternal(separator, count, options);
			}
			Span<int> initialSpan = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(initialSpan);
			Span<int> initialSpan2 = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder2 = new ValueListBuilder<int>(initialSpan2);
			this.MakeSeparatorList(separators, ref valueListBuilder, ref valueListBuilder2);
			ReadOnlySpan<int> sepList = valueListBuilder.AsSpan();
			ReadOnlySpan<int> lengthList = valueListBuilder2.AsSpan();
			if (sepList.Length == 0)
			{
				return new string[]
				{
					this
				};
			}
			string[] result = flag ? this.SplitOmitEmptyEntries(sepList, lengthList, 0, count) : this.SplitKeepEmptyEntries(sepList, lengthList, 0, count);
			valueListBuilder.Dispose();
			valueListBuilder2.Dispose();
			return result;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00019FDC File Offset: 0x000181DC
		private unsafe string[] SplitInternal(string separator, int count, StringSplitOptions options)
		{
			Span<int> initialSpan = new Span<int>(stackalloc byte[(UIntPtr)512], 128);
			ValueListBuilder<int> valueListBuilder = new ValueListBuilder<int>(initialSpan);
			this.MakeSeparatorList(separator, ref valueListBuilder);
			ReadOnlySpan<int> sepList = valueListBuilder.AsSpan();
			if (sepList.Length == 0)
			{
				return new string[]
				{
					this
				};
			}
			string[] result = (options == StringSplitOptions.RemoveEmptyEntries) ? this.SplitOmitEmptyEntries(sepList, default(ReadOnlySpan<int>), separator.Length, count) : this.SplitKeepEmptyEntries(sepList, default(ReadOnlySpan<int>), separator.Length, count);
			valueListBuilder.Dispose();
			return result;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001A064 File Offset: 0x00018264
		private unsafe string[] SplitKeepEmptyEntries(ReadOnlySpan<int> sepList, ReadOnlySpan<int> lengthList, int defaultLength, int count)
		{
			int num = 0;
			int num2 = 0;
			count--;
			int num3 = (sepList.Length < count) ? sepList.Length : count;
			string[] array = new string[num3 + 1];
			int num4 = 0;
			while (num4 < num3 && num < this.Length)
			{
				array[num2++] = this.Substring(num, *sepList[num4] - num);
				num = *sepList[num4] + (lengthList.IsEmpty ? defaultLength : (*lengthList[num4]));
				num4++;
			}
			if (num < this.Length && num3 >= 0)
			{
				array[num2] = this.Substring(num);
			}
			else if (num2 == num3)
			{
				array[num2] = string.Empty;
			}
			return array;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001A118 File Offset: 0x00018318
		private unsafe string[] SplitOmitEmptyEntries(ReadOnlySpan<int> sepList, ReadOnlySpan<int> lengthList, int defaultLength, int count)
		{
			int length = sepList.Length;
			int num = (length < count) ? (length + 1) : count;
			string[] array = new string[num];
			int num2 = 0;
			int num3 = 0;
			int i = 0;
			while (i < length && num2 < this.Length)
			{
				if (*sepList[i] - num2 > 0)
				{
					array[num3++] = this.Substring(num2, *sepList[i] - num2);
				}
				num2 = *sepList[i] + (lengthList.IsEmpty ? defaultLength : (*lengthList[i]));
				if (num3 == count - 1)
				{
					while (i < length - 1)
					{
						if (num2 != *sepList[++i])
						{
							break;
						}
						num2 += (lengthList.IsEmpty ? defaultLength : (*lengthList[i]));
					}
					break;
				}
				i++;
			}
			if (num2 < this.Length)
			{
				array[num3++] = this.Substring(num2);
			}
			string[] array2 = array;
			if (num3 != num)
			{
				array2 = new string[num3];
				for (int j = 0; j < num3; j++)
				{
					array2[j] = array[j];
				}
			}
			return array2;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001A238 File Offset: 0x00018438
		private unsafe void MakeSeparatorList(ReadOnlySpan<char> separators, ref ValueListBuilder<int> sepListBuilder)
		{
			switch (separators.Length)
			{
			case 0:
				for (int i = 0; i < this.Length; i++)
				{
					if (char.IsWhiteSpace(this[i]))
					{
						sepListBuilder.Append(i);
					}
				}
				return;
			case 1:
			{
				char c = (char)(*separators[0]);
				for (int j = 0; j < this.Length; j++)
				{
					if (this[j] == c)
					{
						sepListBuilder.Append(j);
					}
				}
				return;
			}
			case 2:
			{
				char c = (char)(*separators[0]);
				char c2 = (char)(*separators[1]);
				for (int k = 0; k < this.Length; k++)
				{
					char c3 = this[k];
					if (c3 == c || c3 == c2)
					{
						sepListBuilder.Append(k);
					}
				}
				return;
			}
			case 3:
			{
				char c = (char)(*separators[0]);
				char c2 = (char)(*separators[1]);
				char c4 = (char)(*separators[2]);
				for (int l = 0; l < this.Length; l++)
				{
					char c5 = this[l];
					if (c5 == c || c5 == c2 || c5 == c4)
					{
						sepListBuilder.Append(l);
					}
				}
				return;
			}
			default:
			{
				string.ProbabilisticMap probabilisticMap = default(string.ProbabilisticMap);
				uint* charMap = (uint*)(&probabilisticMap);
				string.InitializeProbabilisticMap(charMap, separators);
				for (int m = 0; m < this.Length; m++)
				{
					char c6 = this[m];
					if (string.IsCharBitSet(charMap, (byte)c6) && string.IsCharBitSet(charMap, (byte)(c6 >> 8)) && separators.Contains(c6))
					{
						sepListBuilder.Append(m);
					}
				}
				return;
			}
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001A3C4 File Offset: 0x000185C4
		private void MakeSeparatorList(string separator, ref ValueListBuilder<int> sepListBuilder)
		{
			int length = separator.Length;
			for (int i = 0; i < this.Length; i++)
			{
				if (this[i] == separator[0] && length <= this.Length - i && (length == 1 || this.AsSpan(i, length).SequenceEqual(separator)))
				{
					sepListBuilder.Append(i);
					i += length - 1;
				}
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001A42C File Offset: 0x0001862C
		private void MakeSeparatorList(string[] separators, ref ValueListBuilder<int> sepListBuilder, ref ValueListBuilder<int> lengthListBuilder)
		{
			int num = separators.Length;
			for (int i = 0; i < this.Length; i++)
			{
				foreach (string text in separators)
				{
					if (!string.IsNullOrEmpty(text))
					{
						int length = text.Length;
						if (this[i] == text[0] && length <= this.Length - i && (length == 1 || this.AsSpan(i, length).SequenceEqual(text)))
						{
							sepListBuilder.Append(i);
							lengthListBuilder.Append(length);
							i += length - 1;
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001A4B9 File Offset: 0x000186B9
		public string Substring(int startIndex)
		{
			return this.Substring(startIndex, this.Length - startIndex);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001A4CC File Offset: 0x000186CC
		public string Substring(int startIndex, int length)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "startIndex cannot be larger than length of string.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (startIndex > this.Length - length)
			{
				throw new ArgumentOutOfRangeException("length", "Index and length must refer to a location within the string.");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			if (startIndex == 0 && length == this.Length)
			{
				return this;
			}
			return this.InternalSubString(startIndex, length);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001A554 File Offset: 0x00018754
		private unsafe string InternalSubString(int startIndex, int length)
		{
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text._firstChar)
			{
				char* dmem = ptr;
				fixed (char* ptr2 = &this._firstChar)
				{
					char* ptr3 = ptr2;
					string.wstrcpy(dmem, ptr3 + startIndex, length);
				}
			}
			return text;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001A58C File Offset: 0x0001878C
		public string ToLower()
		{
			return CultureInfo.CurrentCulture.TextInfo.ToLower(this);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001A59E File Offset: 0x0001879E
		public string ToLower(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToLower(this);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001A5BA File Offset: 0x000187BA
		public string ToLowerInvariant()
		{
			return CultureInfo.InvariantCulture.TextInfo.ToLower(this);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001A5CC File Offset: 0x000187CC
		public string ToUpper()
		{
			return CultureInfo.CurrentCulture.TextInfo.ToUpper(this);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001A5DE File Offset: 0x000187DE
		public string ToUpper(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToUpper(this);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001A5FA File Offset: 0x000187FA
		public string ToUpperInvariant()
		{
			return CultureInfo.InvariantCulture.TextInfo.ToUpper(this);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001A60C File Offset: 0x0001880C
		public string Trim()
		{
			return this.TrimWhiteSpaceHelper(string.TrimType.Both);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001A615 File Offset: 0x00018815
		public unsafe string Trim(char trimChar)
		{
			return this.TrimHelper(&trimChar, 1, string.TrimType.Both);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001A624 File Offset: 0x00018824
		public unsafe string Trim(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				return this.TrimWhiteSpaceHelper(string.TrimType.Both);
			}
			fixed (char* ptr = &trimChars[0])
			{
				char* trimChars2 = ptr;
				return this.TrimHelper(trimChars2, trimChars.Length, string.TrimType.Both);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001A656 File Offset: 0x00018856
		public string TrimStart()
		{
			return this.TrimWhiteSpaceHelper(string.TrimType.Head);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001A65F File Offset: 0x0001885F
		public unsafe string TrimStart(char trimChar)
		{
			return this.TrimHelper(&trimChar, 1, string.TrimType.Head);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001A66C File Offset: 0x0001886C
		public unsafe string TrimStart(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				return this.TrimWhiteSpaceHelper(string.TrimType.Head);
			}
			fixed (char* ptr = &trimChars[0])
			{
				char* trimChars2 = ptr;
				return this.TrimHelper(trimChars2, trimChars.Length, string.TrimType.Head);
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001A69E File Offset: 0x0001889E
		public string TrimEnd()
		{
			return this.TrimWhiteSpaceHelper(string.TrimType.Tail);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001A6A7 File Offset: 0x000188A7
		public unsafe string TrimEnd(char trimChar)
		{
			return this.TrimHelper(&trimChar, 1, string.TrimType.Tail);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001A6B4 File Offset: 0x000188B4
		public unsafe string TrimEnd(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				return this.TrimWhiteSpaceHelper(string.TrimType.Tail);
			}
			fixed (char* ptr = &trimChars[0])
			{
				char* trimChars2 = ptr;
				return this.TrimHelper(trimChars2, trimChars.Length, string.TrimType.Tail);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A6E8 File Offset: 0x000188E8
		private string TrimWhiteSpaceHelper(string.TrimType trimType)
		{
			int num = this.Length - 1;
			int num2 = 0;
			if (trimType != string.TrimType.Tail)
			{
				num2 = 0;
				while (num2 < this.Length && char.IsWhiteSpace(this[num2]))
				{
					num2++;
				}
			}
			if (trimType != string.TrimType.Head)
			{
				num = this.Length - 1;
				while (num >= num2 && char.IsWhiteSpace(this[num]))
				{
					num--;
				}
			}
			return this.CreateTrimmedString(num2, num);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A750 File Offset: 0x00018950
		private unsafe string TrimHelper(char* trimChars, int trimCharsLength, string.TrimType trimType)
		{
			int i = this.Length - 1;
			int j = 0;
			if (trimType != string.TrimType.Tail)
			{
				for (j = 0; j < this.Length; j++)
				{
					char c = this[j];
					int num = 0;
					while (num < trimCharsLength && trimChars[num] != c)
					{
						num++;
					}
					if (num == trimCharsLength)
					{
						break;
					}
				}
			}
			if (trimType != string.TrimType.Head)
			{
				for (i = this.Length - 1; i >= j; i--)
				{
					char c2 = this[i];
					int num2 = 0;
					while (num2 < trimCharsLength && trimChars[num2] != c2)
					{
						num2++;
					}
					if (num2 == trimCharsLength)
					{
						break;
					}
				}
			}
			return this.CreateTrimmedString(j, i);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001A7EC File Offset: 0x000189EC
		private string CreateTrimmedString(int start, int end)
		{
			int num = end - start + 1;
			if (num == this.Length)
			{
				return this;
			}
			if (num != 0)
			{
				return this.InternalSubString(start, num);
			}
			return string.Empty;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A81B File Offset: 0x00018A1B
		public bool Contains(string value)
		{
			return this.IndexOf(value, StringComparison.Ordinal) >= 0;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A82B File Offset: 0x00018A2B
		public bool Contains(string value, StringComparison comparisonType)
		{
			return this.IndexOf(value, comparisonType) >= 0;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A83B File Offset: 0x00018A3B
		public bool Contains(char value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A84A File Offset: 0x00018A4A
		public bool Contains(char value, StringComparison comparisonType)
		{
			return this.IndexOf(value, comparisonType) != -1;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A85A File Offset: 0x00018A5A
		public int IndexOf(char value)
		{
			return SpanHelpers.IndexOf(ref this._firstChar, value, this.Length);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A86E File Offset: 0x00018A6E
		public int IndexOf(char value, int startIndex)
		{
			return this.IndexOf(value, startIndex, this.Length - startIndex);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001A880 File Offset: 0x00018A80
		public int IndexOf(char value, StringComparison comparisonType)
		{
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CompareInfo.Invariant.IndexOf(this, value, CompareOptions.OrdinalIgnoreCase);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001A924 File Offset: 0x00018B24
		public int IndexOf(char value, int startIndex, int count)
		{
			if (startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			int num = SpanHelpers.IndexOf(Unsafe.Add<char>(ref this._firstChar, startIndex), value, count);
			if (num != -1)
			{
				return num + startIndex;
			}
			return num;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001A982 File Offset: 0x00018B82
		public int IndexOfAny(char[] anyOf)
		{
			return this.IndexOfAny(anyOf, 0, this.Length);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001A992 File Offset: 0x00018B92
		public int IndexOfAny(char[] anyOf, int startIndex)
		{
			return this.IndexOfAny(anyOf, startIndex, this.Length - startIndex);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001A9A4 File Offset: 0x00018BA4
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			if (anyOf == null)
			{
				throw new ArgumentNullException("anyOf");
			}
			if (startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (anyOf.Length == 2)
			{
				return this.IndexOfAny(anyOf[0], anyOf[1], startIndex, count);
			}
			if (anyOf.Length == 3)
			{
				return this.IndexOfAny(anyOf[0], anyOf[1], anyOf[2], startIndex, count);
			}
			if (anyOf.Length > 3)
			{
				return this.IndexOfCharArray(anyOf, startIndex, count);
			}
			if (anyOf.Length == 1)
			{
				return this.IndexOf(anyOf[0], startIndex, count);
			}
			return -1;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001AA44 File Offset: 0x00018C44
		private unsafe int IndexOfAny(char value1, char value2, int startIndex, int count)
		{
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + startIndex;
				while (count > 0)
				{
					char c = *ptr3;
					if (c == value1 || c == value2)
					{
						return (int)((long)(ptr3 - ptr2));
					}
					c = ptr3[1];
					if (c == value1 || c == value2)
					{
						if (count != 1)
						{
							return (int)((long)(ptr3 - ptr2)) + 1;
						}
						return -1;
					}
					else
					{
						ptr3 += 2;
						count -= 2;
					}
				}
				return -1;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001AAA8 File Offset: 0x00018CA8
		private unsafe int IndexOfAny(char value1, char value2, char value3, int startIndex, int count)
		{
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + startIndex;
				while (count > 0)
				{
					char c = *ptr3;
					if (c == value1 || c == value2 || c == value3)
					{
						return (int)((long)(ptr3 - ptr2));
					}
					ptr3++;
					count--;
				}
				return -1;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001AAF0 File Offset: 0x00018CF0
		private unsafe int IndexOfCharArray(char[] anyOf, int startIndex, int count)
		{
			string.ProbabilisticMap probabilisticMap = default(string.ProbabilisticMap);
			uint* charMap = (uint*)(&probabilisticMap);
			string.InitializeProbabilisticMap(charMap, anyOf);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + startIndex;
				while (count > 0)
				{
					int num = (int)(*ptr3);
					if (string.IsCharBitSet(charMap, (byte)num) && string.IsCharBitSet(charMap, (byte)(num >> 8)) && string.ArrayContains((char)num, anyOf))
					{
						return (int)((long)(ptr3 - ptr2));
					}
					count--;
					ptr3++;
				}
				return -1;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001AB6C File Offset: 0x00018D6C
		private unsafe static void InitializeProbabilisticMap(uint* charMap, ReadOnlySpan<char> anyOf)
		{
			bool flag = false;
			for (int i = 0; i < anyOf.Length; i++)
			{
				int num = (int)(*anyOf[i]);
				string.SetCharBit(charMap, (byte)num);
				num >>= 8;
				if (num == 0)
				{
					flag = true;
				}
				else
				{
					string.SetCharBit(charMap, (byte)num);
				}
			}
			if (flag)
			{
				*charMap |= 1U;
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001ABC0 File Offset: 0x00018DC0
		private static bool ArrayContains(char searchChar, char[] anyOf)
		{
			for (int i = 0; i < anyOf.Length; i++)
			{
				if (anyOf[i] == searchChar)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001ABE4 File Offset: 0x00018DE4
		private unsafe static bool IsCharBitSet(uint* charMap, byte value)
		{
			return (charMap[value & 7] & 1U << (value >> 3)) > 0U;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001ABFB File Offset: 0x00018DFB
		private unsafe static void SetCharBit(uint* charMap, byte value)
		{
			charMap[value & 7] |= 1U << (value >> 3);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001AC11 File Offset: 0x00018E11
		public int IndexOf(string value)
		{
			return this.IndexOf(value, StringComparison.CurrentCulture);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001AC1B File Offset: 0x00018E1B
		public int IndexOf(string value, int startIndex)
		{
			return this.IndexOf(value, startIndex, StringComparison.CurrentCulture);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001AC28 File Offset: 0x00018E28
		public int IndexOf(string value, int startIndex, int count)
		{
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			return this.IndexOf(value, startIndex, count, StringComparison.CurrentCulture);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001AC7B File Offset: 0x00018E7B
		public int IndexOf(string value, StringComparison comparisonType)
		{
			return this.IndexOf(value, 0, this.Length, comparisonType);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001AC8C File Offset: 0x00018E8C
		public int IndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return this.IndexOf(value, startIndex, this.Length - startIndex, comparisonType);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001ACA0 File Offset: 0x00018EA0
		public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex > this.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CompareInfo.Invariant.IndexOf(this, value, startIndex, count, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CompareInfo.Invariant.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CompareInfo.Invariant.IndexOfOrdinal(this, value, startIndex, count, false);
			case StringComparison.OrdinalIgnoreCase:
				return CompareInfo.Invariant.IndexOfOrdinal(this, value, startIndex, count, true);
			default:
				throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001AD91 File Offset: 0x00018F91
		public int LastIndexOf(char value)
		{
			return SpanHelpers.LastIndexOf(ref this._firstChar, value, this.Length);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001ADA5 File Offset: 0x00018FA5
		public int LastIndexOf(char value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001ADB4 File Offset: 0x00018FB4
		public int LastIndexOf(char value, int startIndex, int count)
		{
			if (this.Length == 0)
			{
				return -1;
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > startIndex + 1)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			int num = startIndex + 1 - count;
			int num2 = SpanHelpers.LastIndexOf(Unsafe.Add<char>(ref this._firstChar, num), value, count);
			if (num2 != -1)
			{
				return num2 + num;
			}
			return num2;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001AE1D File Offset: 0x0001901D
		public int LastIndexOfAny(char[] anyOf)
		{
			return this.LastIndexOfAny(anyOf, this.Length - 1, this.Length);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001AE34 File Offset: 0x00019034
		public int LastIndexOfAny(char[] anyOf, int startIndex)
		{
			return this.LastIndexOfAny(anyOf, startIndex, startIndex + 1);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001AE44 File Offset: 0x00019044
		public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
		{
			if (anyOf == null)
			{
				throw new ArgumentNullException("anyOf");
			}
			if (this.Length == 0)
			{
				return -1;
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count - 1 > startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (anyOf.Length > 1)
			{
				return this.LastIndexOfCharArray(anyOf, startIndex, count);
			}
			if (anyOf.Length == 1)
			{
				return this.LastIndexOf(anyOf[0], startIndex, count);
			}
			return -1;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001AEC0 File Offset: 0x000190C0
		private unsafe int LastIndexOfCharArray(char[] anyOf, int startIndex, int count)
		{
			string.ProbabilisticMap probabilisticMap = default(string.ProbabilisticMap);
			uint* charMap = (uint*)(&probabilisticMap);
			string.InitializeProbabilisticMap(charMap, anyOf);
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + startIndex;
				while (count > 0)
				{
					int num = (int)(*ptr3);
					if (string.IsCharBitSet(charMap, (byte)num) && string.IsCharBitSet(charMap, (byte)(num >> 8)) && string.ArrayContains((char)num, anyOf))
					{
						return (int)((long)(ptr3 - ptr2));
					}
					count--;
					ptr3--;
				}
				return -1;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001AF3A File Offset: 0x0001913A
		public int LastIndexOf(string value)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length, StringComparison.CurrentCulture);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001AF52 File Offset: 0x00019152
		public int LastIndexOf(string value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1, StringComparison.CurrentCulture);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001AF60 File Offset: 0x00019160
		public int LastIndexOf(string value, int startIndex, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			return this.LastIndexOf(value, startIndex, count, StringComparison.CurrentCulture);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001AF80 File Offset: 0x00019180
		public int LastIndexOf(string value, StringComparison comparisonType)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length, comparisonType);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001AF98 File Offset: 0x00019198
		public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1, comparisonType);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001AFA8 File Offset: 0x000191A8
		public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				if (value.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (startIndex < 0 || startIndex > this.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (startIndex == this.Length)
				{
					startIndex--;
					if (count > 0)
					{
						count--;
					}
				}
				if (count < 0 || startIndex - count + 1 < 0)
				{
					throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
				}
				if (value.Length == 0)
				{
					return startIndex;
				}
				switch (comparisonType)
				{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.InvariantCulture:
					return CompareInfo.Invariant.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
				case StringComparison.InvariantCultureIgnoreCase:
					return CompareInfo.Invariant.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return CompareInfo.Invariant.LastIndexOfOrdinal(this, value, startIndex, count, false);
				case StringComparison.OrdinalIgnoreCase:
					return CompareInfo.Invariant.LastIndexOfOrdinal(this, value, startIndex, count, true);
				default:
					throw new ArgumentException("The string comparison type passed in is currently not supported.", "comparisonType");
				}
			}
		}

		// Token: 0x0600057F RID: 1407
		[PreserveDependency("CreateString(System.Char[])", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char[] value);

		// Token: 0x06000580 RID: 1408 RVA: 0x0001B0D4 File Offset: 0x000192D4
		private unsafe static string Ctor(char[] value)
		{
			if (value == null || value.Length == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(value.Length);
			fixed (char* ptr = &text._firstChar)
			{
				char* dmem = ptr;
				fixed (char[] array = value)
				{
					char* smem;
					if (value == null || array.Length == 0)
					{
						smem = null;
					}
					else
					{
						smem = &array[0];
					}
					string.wstrcpy(dmem, smem, value.Length);
					ptr = null;
				}
				return text;
			}
		}

		// Token: 0x06000581 RID: 1409
		[PreserveDependency("CreateString(System.Char[], System.Int32, System.Int32)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char[] value, int startIndex, int length);

		// Token: 0x06000582 RID: 1410 RVA: 0x0001B128 File Offset: 0x00019328
		private unsafe static string Ctor(char[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (startIndex > value.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text._firstChar)
			{
				char* dmem = ptr;
				fixed (char[] array = value)
				{
					char* ptr2;
					if (value == null || array.Length == 0)
					{
						ptr2 = null;
					}
					else
					{
						ptr2 = &array[0];
					}
					string.wstrcpy(dmem, ptr2 + startIndex, length);
					ptr = null;
				}
				return text;
			}
		}

		// Token: 0x06000583 RID: 1411
		[CLSCompliant(false)]
		[PreserveDependency("CreateString(System.Char*)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value);

		// Token: 0x06000584 RID: 1412 RVA: 0x0001B1C4 File Offset: 0x000193C4
		private unsafe static string Ctor(char* ptr)
		{
			if (ptr == null)
			{
				return string.Empty;
			}
			int num = string.wcslen(ptr);
			if (num == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(num);
			fixed (char* ptr2 = &text._firstChar)
			{
				string.wstrcpy(ptr2, ptr, num);
			}
			return text;
		}

		// Token: 0x06000585 RID: 1413
		[PreserveDependency("CreateString(System.Char*, System.Int32, System.Int32)", "System.String")]
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value, int startIndex, int length);

		// Token: 0x06000586 RID: 1414 RVA: 0x0001B208 File Offset: 0x00019408
		private unsafe static string Ctor(char* ptr, int startIndex, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			char* ptr2 = ptr + startIndex;
			if (ptr2 < ptr)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Pointer startIndex and length do not refer to a valid string.");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			if (ptr == null)
			{
				throw new ArgumentOutOfRangeException("ptr", "Pointer startIndex and length do not refer to a valid string.");
			}
			string text = string.FastAllocateString(length);
			fixed (char* ptr3 = &text._firstChar)
			{
				string.wstrcpy(ptr3, ptr2, length);
			}
			return text;
		}

		// Token: 0x06000587 RID: 1415
		[PreserveDependency("CreateString(System.SByte*)", "System.String")]
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value);

		// Token: 0x06000588 RID: 1416 RVA: 0x0001B290 File Offset: 0x00019490
		private unsafe static string Ctor(sbyte* value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			int num = new ReadOnlySpan<byte>((void*)value, int.MaxValue).IndexOf(0);
			if (num < 0)
			{
				throw new ArgumentException("The string must be null-terminated.");
			}
			return string.CreateStringForSByteConstructor((byte*)value, num);
		}

		// Token: 0x06000589 RID: 1417
		[CLSCompliant(false)]
		[PreserveDependency("CreateString(System.SByte*, System.Int32, System.Int32)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value, int startIndex, int length);

		// Token: 0x0600058A RID: 1418 RVA: 0x0001B2D4 File Offset: 0x000194D4
		private unsafe static string Ctor(sbyte* value, int startIndex, int length)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length cannot be less than zero.");
			}
			if (value == null)
			{
				if (length == 0)
				{
					return string.Empty;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				byte* ptr = (byte*)(value + startIndex);
				if (ptr < (byte*)value)
				{
					throw new ArgumentOutOfRangeException("value", "Pointer startIndex and length do not refer to a valid string.");
				}
				return string.CreateStringForSByteConstructor(ptr, length);
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001B341 File Offset: 0x00019541
		private unsafe static string CreateStringForSByteConstructor(byte* pb, int numBytes)
		{
			if (numBytes == 0)
			{
				return string.Empty;
			}
			return Encoding.UTF8.GetString(pb, numBytes);
		}

		// Token: 0x0600058C RID: 1420
		[CLSCompliant(false)]
		[PreserveDependency("CreateString(System.SByte*, System.Int32, System.Int32, System.Text.Encoding)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value, int startIndex, int length, Encoding enc);

		// Token: 0x0600058D RID: 1421 RVA: 0x0001B358 File Offset: 0x00019558
		private unsafe static string Ctor(sbyte* value, int startIndex, int length, Encoding enc)
		{
			if (enc == null)
			{
				return new string(value, startIndex, length);
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "StartIndex cannot be less than zero.");
			}
			if (value == null)
			{
				if (length == 0)
				{
					return string.Empty;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				byte* ptr = (byte*)(value + startIndex);
				if (ptr < (byte*)value)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Pointer startIndex and length do not refer to a valid string.");
				}
				return enc.GetString(new ReadOnlySpan<byte>((void*)ptr, length));
			}
		}

		// Token: 0x0600058E RID: 1422
		[PreserveDependency("CreateString(System.Char, System.Int32)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char c, int count);

		// Token: 0x0600058F RID: 1423 RVA: 0x0001B3D8 File Offset: 0x000195D8
		private unsafe static string Ctor(char c, int count)
		{
			if (count > 0)
			{
				string text = string.FastAllocateString(count);
				if (c != '\0')
				{
					fixed (char* ptr = &text._firstChar)
					{
						uint* ptr2 = (uint*)ptr;
						uint num = (uint)((uint)c << 16 | c);
						uint* ptr3 = ptr2;
						if (count >= 4)
						{
							count -= 4;
							do
							{
								*ptr3 = num;
								ptr3[1] = num;
								ptr3 += 2;
								count -= 4;
							}
							while (count >= 0);
						}
						if ((count & 2) != 0)
						{
							*ptr3 = num;
							ptr3++;
						}
						if ((count & 1) != 0)
						{
							*(short*)ptr3 = (short)c;
						}
					}
				}
				return text;
			}
			if (count == 0)
			{
				return string.Empty;
			}
			throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
		}

		// Token: 0x06000590 RID: 1424
		[PreserveDependency("CreateString(System.ReadOnlySpan`1<System.Char>)", "System.String")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(ReadOnlySpan<char> value);

		// Token: 0x06000591 RID: 1425 RVA: 0x0001B458 File Offset: 0x00019658
		private unsafe static string Ctor(ReadOnlySpan<char> value)
		{
			if (value.Length == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(value.Length);
			fixed (char* ptr = &text._firstChar)
			{
				char* dmem = ptr;
				fixed (char* reference = MemoryMarshal.GetReference<char>(value))
				{
					char* smem = reference;
					string.wstrcpy(dmem, smem, value.Length);
					ptr = null;
				}
				return text;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001B4A8 File Offset: 0x000196A8
		public static string Create<TState>(int length, TState state, SpanAction<char, TState> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (length > 0)
			{
				string text = string.FastAllocateString(length);
				action(new Span<char>(text.GetRawStringData(), length), state);
				return text;
			}
			if (length == 0)
			{
				return string.Empty;
			}
			throw new ArgumentOutOfRangeException("length");
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001B4F8 File Offset: 0x000196F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<char>(string value)
		{
			if (value == null)
			{
				return default(ReadOnlySpan<char>);
			}
			return new ReadOnlySpan<char>(value.GetRawStringData(), value.Length);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0000270D File Offset: 0x0000090D
		public object Clone()
		{
			return this;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001B524 File Offset: 0x00019724
		public unsafe static string Copy(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			string text = string.FastAllocateString(str.Length);
			fixed (char* ptr = &text._firstChar)
			{
				char* dmem = ptr;
				fixed (char* ptr2 = &str._firstChar)
				{
					char* smem = ptr2;
					string.wstrcpy(dmem, smem, str.Length);
					ptr = null;
				}
				return text;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001B570 File Offset: 0x00019770
		public unsafe void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count cannot be less than zero.");
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count > this.Length - sourceIndex)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Index and count must refer to a location within the string.");
			}
			if (destinationIndex > destination.Length - count || destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Index and count must refer to a location within the string.");
			}
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (char[] array = destination)
				{
					char* ptr3;
					if (destination == null || array.Length == 0)
					{
						ptr3 = null;
					}
					else
					{
						ptr3 = &array[0];
					}
					string.wstrcpy(ptr3 + destinationIndex, ptr2 + sourceIndex, count);
					ptr = null;
				}
				return;
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001B628 File Offset: 0x00019828
		public unsafe char[] ToCharArray()
		{
			if (this.Length == 0)
			{
				return Array.Empty<char>();
			}
			char[] array = new char[this.Length];
			fixed (char* ptr = &this._firstChar)
			{
				char* smem = ptr;
				fixed (char* ptr2 = &array[0])
				{
					string.wstrcpy(ptr2, smem, this.Length);
					ptr = null;
				}
				return array;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001B674 File Offset: 0x00019874
		public unsafe char[] ToCharArray(int startIndex, int length)
		{
			if (startIndex < 0 || startIndex > this.Length || startIndex > this.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (length > 0)
			{
				char[] array = new char[length];
				fixed (char* ptr = &this._firstChar)
				{
					char* ptr2 = ptr;
					fixed (char* ptr3 = &array[0])
					{
						string.wstrcpy(ptr3, ptr2 + startIndex, length);
						ptr = null;
					}
					return array;
				}
			}
			if (length == 0)
			{
				return Array.Empty<char>();
			}
			throw new ArgumentOutOfRangeException("length", "Index was out of range. Must be non-negative and less than the size of the collection.");
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001B6F2 File Offset: 0x000198F2
		[NonVersionable]
		public static bool IsNullOrEmpty(string value)
		{
			return value == null || 0 >= value.Length;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001B704 File Offset: 0x00019904
		public static bool IsNullOrWhiteSpace(string value)
		{
			if (value == null)
			{
				return true;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001B738 File Offset: 0x00019938
		internal ref char GetRawStringData()
		{
			return ref this._firstChar;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001B740 File Offset: 0x00019940
		internal unsafe static string CreateStringFromEncoding(byte* bytes, int byteLength, Encoding encoding)
		{
			int charCount = encoding.GetCharCount(bytes, byteLength, null);
			if (charCount == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(charCount);
			fixed (char* ptr = &text._firstChar)
			{
				char* chars = ptr;
				encoding.GetChars(bytes, byteLength, chars, charCount, null);
			}
			return text;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001B77F File Offset: 0x0001997F
		internal static string CreateFromChar(char c)
		{
			string text = string.FastAllocateString(1);
			text._firstChar = c;
			return text;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001B78E File Offset: 0x0001998E
		internal unsafe static void wstrcpy(char* dmem, char* smem, int charCount)
		{
			Buffer.Memmove((byte*)dmem, (byte*)smem, (uint)(charCount * 2));
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0000270D File Offset: 0x0000090D
		public override string ToString()
		{
			return this;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000270D File Offset: 0x0000090D
		public string ToString(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001B79A File Offset: 0x0001999A
		public CharEnumerator GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001B79A File Offset: 0x0001999A
		IEnumerator<char> IEnumerable<char>.GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001B79A File Offset: 0x0001999A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001B7A4 File Offset: 0x000199A4
		internal unsafe static int wcslen(char* ptr)
		{
			char* ptr2 = ptr;
			int num = IntPtr.Size - 1;
			while ((ptr2 & (uint)num) != 0U)
			{
				if (*ptr2 != '\0')
				{
					ptr2++;
				}
				else
				{
					IL_6E:
					int num2 = (int)((long)(ptr2 - ptr));
					if (ptr + num2 != ptr2)
					{
						throw new ArgumentException("The string must be null-terminated.");
					}
					return num2;
				}
			}
			for (;;)
			{
				if ((*(long*)ptr2 + 9223231297218904063L | 9223231297218904063L) == -1L)
				{
					ptr2 += 4;
				}
				else
				{
					if (*ptr2 == '\0')
					{
						goto IL_6E;
					}
					if (ptr2[1] == '\0')
					{
						goto IL_6A;
					}
					if (ptr2[2] == '\0')
					{
						goto IL_66;
					}
					if (ptr2[3] == '\0')
					{
						break;
					}
					ptr2 += 4;
				}
			}
			ptr2++;
			IL_66:
			ptr2++;
			IL_6A:
			ptr2++;
			goto IL_6E;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B83C File Offset: 0x00019A3C
		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B840 File Offset: 0x00019A40
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this, provider);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B849 File Offset: 0x00019A49
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this, provider);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B852 File Offset: 0x00019A52
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this, provider);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B85B File Offset: 0x00019A5B
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this, provider);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001B864 File Offset: 0x00019A64
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this, provider);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001B86D File Offset: 0x00019A6D
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this, provider);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001B876 File Offset: 0x00019A76
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this, provider);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001B87F File Offset: 0x00019A7F
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this, provider);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001B888 File Offset: 0x00019A88
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this, provider);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001B891 File Offset: 0x00019A91
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this, provider);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001B89A File Offset: 0x00019A9A
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this, provider);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001B8A3 File Offset: 0x00019AA3
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this, provider);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001B8AC File Offset: 0x00019AAC
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this, provider);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B8B5 File Offset: 0x00019AB5
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(this, provider);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B8BE File Offset: 0x00019ABE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001B8C8 File Offset: 0x00019AC8
		public bool IsNormalized()
		{
			return this.IsNormalized(NormalizationForm.FormC);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B8D1 File Offset: 0x00019AD1
		public bool IsNormalized(NormalizationForm normalizationForm)
		{
			return Normalization.IsNormalized(this, normalizationForm);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B8DA File Offset: 0x00019ADA
		public string Normalize()
		{
			return this.Normalize(NormalizationForm.FormC);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B8E3 File Offset: 0x00019AE3
		public string Normalize(NormalizationForm normalizationForm)
		{
			return Normalization.Normalize(this, normalizationForm);
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001B8EC File Offset: 0x00019AEC
		public int Length
		{
			get
			{
				return this._stringLength;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B8F4 File Offset: 0x00019AF4
		internal unsafe int IndexOfUnchecked(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 + count - length + 1;
					while (ptr4 != ptr5)
					{
						if (*ptr4 == *ptr3)
						{
							for (int i = 1; i < length; i++)
							{
								if (ptr4[i] != ptr3[i])
								{
									goto IL_7B;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_7B:
						ptr4++;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001B98F File Offset: 0x00019B8F
		[CLSCompliant(false)]
		public static string Concat(object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001B998 File Offset: 0x00019B98
		internal unsafe int IndexOfUncheckedIgnoreCase(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 + count - length + 1;
					char c = textInfo.ToUpper(*ptr3);
					while (ptr4 != ptr5)
					{
						if (textInfo.ToUpper(*ptr4) == c)
						{
							for (int i = 1; i < length; i++)
							{
								if (textInfo.ToUpper(ptr4[i]) != textInfo.ToUpper(ptr3[i]))
								{
									goto IL_A4;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_A4:
						ptr4++;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001BA60 File Offset: 0x00019C60
		internal unsafe int LastIndexOfUnchecked(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 - count + length - 1;
					char* ptr6 = ptr3 + length - 1;
					while (ptr4 != ptr5)
					{
						if (*ptr4 == *ptr6)
						{
							char* ptr7 = ptr4;
							while (ptr3 != ptr6)
							{
								ptr6--;
								ptr4--;
								if (*ptr4 != *ptr6)
								{
									ptr6 = ptr3 + length - 1;
									ptr4 = ptr7;
									goto IL_92;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_92:
						ptr4--;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001BB14 File Offset: 0x00019D14
		internal unsafe int LastIndexOfUncheckedIgnoreCase(string value, int startIndex, int count)
		{
			int length = value.Length;
			if (count < length)
			{
				return -1;
			}
			if (length == 0)
			{
				return startIndex;
			}
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			fixed (char* ptr = &this._firstChar)
			{
				char* ptr2 = ptr;
				fixed (string text = value)
				{
					char* ptr3 = text;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr4 = ptr2 + startIndex;
					char* ptr5 = ptr4 - count + length - 1;
					char* ptr6 = ptr3 + length - 1;
					char c = textInfo.ToUpper(*ptr6);
					while (ptr4 != ptr5)
					{
						if (textInfo.ToUpper(*ptr4) == c)
						{
							char* ptr7 = ptr4;
							while (ptr3 != ptr6)
							{
								ptr6--;
								ptr4--;
								if (textInfo.ToUpper(*ptr4) != textInfo.ToUpper(*ptr6))
								{
									ptr6 = ptr3 + length - 1;
									ptr4 = ptr7;
									goto IL_BB;
								}
							}
							return (int)((long)(ptr4 - ptr2));
						}
						IL_BB:
						ptr4--;
					}
					ptr = null;
				}
				return -1;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001BBF0 File Offset: 0x00019DF0
		internal bool StartsWithOrdinalUnchecked(string value)
		{
			return this.Length >= value.Length && this._firstChar == value._firstChar && (value.Length == 1 || SpanHelpers.SequenceEqual(Unsafe.As<char, byte>(this.GetRawStringData()), Unsafe.As<char, byte>(value.GetRawStringData()), (ulong)((long)value.Length * 2L)));
		}

		// Token: 0x060005C0 RID: 1472
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string FastAllocateString(int length);

		// Token: 0x060005C1 RID: 1473
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string InternalIsInterned(string str);

		// Token: 0x060005C2 RID: 1474
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string InternalIntern(string str);

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001BC4C File Offset: 0x00019E4C
		private unsafe static int FastCompareStringHelper(uint* strAChars, int countA, uint* strBChars, int countB)
		{
			char* ptr = (char*)strAChars;
			char* ptr2 = (char*)strBChars;
			char* ptr3 = ptr + Math.Min(countA, countB);
			while (ptr < ptr3)
			{
				if (*ptr != *ptr2)
				{
					return (int)(*ptr - *ptr2);
				}
				ptr++;
				ptr2++;
			}
			return countA - countB;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001BC88 File Offset: 0x00019E88
		private unsafe static void memset(byte* dest, int val, int len)
		{
			if (len < 8)
			{
				while (len != 0)
				{
					*dest = (byte)val;
					dest++;
					len--;
				}
				return;
			}
			if (val != 0)
			{
				val |= val << 8;
				val |= val << 16;
			}
			int num = dest & 3;
			if (num != 0)
			{
				num = 4 - num;
				len -= num;
				do
				{
					*dest = (byte)val;
					dest++;
					num--;
				}
				while (num != 0);
			}
			while (len >= 16)
			{
				*(int*)dest = val;
				*(int*)(dest + 4) = val;
				*(int*)(dest + (IntPtr)2 * 4) = val;
				*(int*)(dest + (IntPtr)3 * 4) = val;
				dest += 16;
				len -= 16;
			}
			while (len >= 4)
			{
				*(int*)dest = val;
				dest += 4;
				len -= 4;
			}
			while (len > 0)
			{
				*dest = (byte)val;
				dest++;
				len--;
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001BD32 File Offset: 0x00019F32
		private unsafe static void memcpy(byte* dest, byte* src, int size)
		{
			Buffer.Memcpy(dest, src, size);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001BD3C File Offset: 0x00019F3C
		internal unsafe static void bzero(byte* dest, int len)
		{
			string.memset(dest, 0, len);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001BD46 File Offset: 0x00019F46
		internal unsafe static void bzero_aligned_1(byte* dest, int len)
		{
			*dest = 0;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001BD4B File Offset: 0x00019F4B
		internal unsafe static void bzero_aligned_2(byte* dest, int len)
		{
			*(short*)dest = 0;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001BD50 File Offset: 0x00019F50
		internal unsafe static void bzero_aligned_4(byte* dest, int len)
		{
			*(int*)dest = 0;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001BD55 File Offset: 0x00019F55
		internal unsafe static void bzero_aligned_8(byte* dest, int len)
		{
			*(long*)dest = 0L;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001BD5B File Offset: 0x00019F5B
		internal unsafe static void memcpy_aligned_1(byte* dest, byte* src, int size)
		{
			*dest = *src;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001BD61 File Offset: 0x00019F61
		internal unsafe static void memcpy_aligned_2(byte* dest, byte* src, int size)
		{
			*(short*)dest = *(short*)src;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001BD67 File Offset: 0x00019F67
		internal unsafe static void memcpy_aligned_4(byte* dest, byte* src, int size)
		{
			*(int*)dest = *(int*)src;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001BD6D File Offset: 0x00019F6D
		internal unsafe static void memcpy_aligned_8(byte* dest, byte* src, int size)
		{
			*(long*)dest = *(long*)src;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001BD73 File Offset: 0x00019F73
		private unsafe string CreateString(sbyte* value)
		{
			return string.Ctor(value);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001BD7B File Offset: 0x00019F7B
		private unsafe string CreateString(sbyte* value, int startIndex, int length)
		{
			return string.Ctor(value, startIndex, length);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001BD85 File Offset: 0x00019F85
		private unsafe string CreateString(char* value)
		{
			return string.Ctor(value);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001BD8D File Offset: 0x00019F8D
		private unsafe string CreateString(char* value, int startIndex, int length)
		{
			return string.Ctor(value, startIndex, length);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001BD97 File Offset: 0x00019F97
		private string CreateString(char[] val, int startIndex, int length)
		{
			return string.Ctor(val, startIndex, length);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001BDA1 File Offset: 0x00019FA1
		private string CreateString(char[] val)
		{
			return string.Ctor(val);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001BDA9 File Offset: 0x00019FA9
		private string CreateString(char c, int count)
		{
			return string.Ctor(c, count);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001BDB2 File Offset: 0x00019FB2
		private unsafe string CreateString(sbyte* value, int startIndex, int length, Encoding enc)
		{
			return string.Ctor(value, startIndex, length, enc);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001BDBE File Offset: 0x00019FBE
		private string CreateString(ReadOnlySpan<char> value)
		{
			return string.Ctor(value);
		}

		// Token: 0x1700007F RID: 127
		[IndexerName("Chars")]
		public unsafe char this[int index]
		{
			[Intrinsic]
			get
			{
				if ((ulong)index >= (ulong)((long)this._stringLength))
				{
					ThrowHelper.ThrowIndexOutOfRangeException();
				}
				return *Unsafe.Add<char>(ref this._firstChar, index);
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001BDE5 File Offset: 0x00019FE5
		public static string Intern(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return string.InternalIntern(str);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001BDFB File Offset: 0x00019FFB
		public static string IsInterned(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return string.InternalIsInterned(str);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001BE14 File Offset: 0x0001A014
		private unsafe int LegacyStringGetHashCode()
		{
			int num = 5381;
			int num2 = num;
			fixed (string text = this)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char* ptr2 = ptr;
				int num3;
				while ((num3 = (int)(*ptr2)) != 0)
				{
					num = ((num << 5) + num ^ num3);
					num3 = (int)ptr2[1];
					if (num3 == 0)
					{
						break;
					}
					num2 = ((num2 << 5) + num2 ^ num3);
					ptr2 += 2;
				}
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x04000FEA RID: 4074
		private const int StackallocIntBufferSizeLimit = 128;

		// Token: 0x04000FEB RID: 4075
		private const int PROBABILISTICMAP_BLOCK_INDEX_MASK = 7;

		// Token: 0x04000FEC RID: 4076
		private const int PROBABILISTICMAP_BLOCK_INDEX_SHIFT = 3;

		// Token: 0x04000FED RID: 4077
		private const int PROBABILISTICMAP_SIZE = 8;

		// Token: 0x04000FEE RID: 4078
		[NonSerialized]
		private int _stringLength;

		// Token: 0x04000FEF RID: 4079
		[NonSerialized]
		private char _firstChar;

		// Token: 0x04000FF0 RID: 4080
		public static readonly string Empty;

		// Token: 0x020000CE RID: 206
		private enum TrimType
		{
			// Token: 0x04000FF2 RID: 4082
			Head,
			// Token: 0x04000FF3 RID: 4083
			Tail,
			// Token: 0x04000FF4 RID: 4084
			Both
		}

		// Token: 0x020000CF RID: 207
		[StructLayout(LayoutKind.Explicit, Size = 32)]
		private struct ProbabilisticMap
		{
		}
	}
}
