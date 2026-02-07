using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x02000995 RID: 2453
	[ComVisible(true)]
	[Serializable]
	public class StringInfo
	{
		// Token: 0x060057D8 RID: 22488 RVA: 0x0012877E File Offset: 0x0012697E
		public StringInfo() : this("")
		{
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x0012878B File Offset: 0x0012698B
		public StringInfo(string value)
		{
			this.String = value;
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x0012879A File Offset: 0x0012699A
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.m_str = string.Empty;
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x001287A7 File Offset: 0x001269A7
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_str.Length == 0)
			{
				this.m_indexes = null;
			}
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x001287C0 File Offset: 0x001269C0
		[ComVisible(false)]
		public override bool Equals(object value)
		{
			StringInfo stringInfo = value as StringInfo;
			return stringInfo != null && this.m_str.Equals(stringInfo.m_str);
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x001287EA File Offset: 0x001269EA
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return this.m_str.GetHashCode();
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060057DE RID: 22494 RVA: 0x001287F7 File Offset: 0x001269F7
		private int[] Indexes
		{
			get
			{
				if (this.m_indexes == null && 0 < this.String.Length)
				{
					this.m_indexes = StringInfo.ParseCombiningCharacters(this.String);
				}
				return this.m_indexes;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060057DF RID: 22495 RVA: 0x00128826 File Offset: 0x00126A26
		// (set) Token: 0x060057E0 RID: 22496 RVA: 0x0012882E File Offset: 0x00126A2E
		public string String
		{
			get
			{
				return this.m_str;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("String", Environment.GetResourceString("String reference not set to an instance of a String."));
				}
				this.m_str = value;
				this.m_indexes = null;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060057E1 RID: 22497 RVA: 0x00128856 File Offset: 0x00126A56
		public int LengthInTextElements
		{
			get
			{
				if (this.Indexes == null)
				{
					return 0;
				}
				return this.Indexes.Length;
			}
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x0012886C File Offset: 0x00126A6C
		public string SubstringByTextElements(int startingTextElement)
		{
			if (this.Indexes != null)
			{
				return this.SubstringByTextElements(startingTextElement, this.Indexes.Length - startingTextElement);
			}
			if (startingTextElement < 0)
			{
				throw new ArgumentOutOfRangeException("startingTextElement", Environment.GetResourceString("Positive number required."));
			}
			throw new ArgumentOutOfRangeException("startingTextElement", Environment.GetResourceString("Specified argument was out of the range of valid values."));
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x001288C0 File Offset: 0x00126AC0
		public string SubstringByTextElements(int startingTextElement, int lengthInTextElements)
		{
			if (startingTextElement < 0)
			{
				throw new ArgumentOutOfRangeException("startingTextElement", Environment.GetResourceString("Positive number required."));
			}
			if (this.String.Length == 0 || startingTextElement >= this.Indexes.Length)
			{
				throw new ArgumentOutOfRangeException("startingTextElement", Environment.GetResourceString("Specified argument was out of the range of valid values."));
			}
			if (lengthInTextElements < 0)
			{
				throw new ArgumentOutOfRangeException("lengthInTextElements", Environment.GetResourceString("Positive number required."));
			}
			if (startingTextElement > this.Indexes.Length - lengthInTextElements)
			{
				throw new ArgumentOutOfRangeException("lengthInTextElements", Environment.GetResourceString("Specified argument was out of the range of valid values."));
			}
			int num = this.Indexes[startingTextElement];
			if (startingTextElement + lengthInTextElements == this.Indexes.Length)
			{
				return this.String.Substring(num);
			}
			return this.String.Substring(num, this.Indexes[lengthInTextElements + startingTextElement] - num);
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x00128989 File Offset: 0x00126B89
		public static string GetNextTextElement(string str)
		{
			return StringInfo.GetNextTextElement(str, 0);
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x00128994 File Offset: 0x00126B94
		internal static int GetCurrentTextElementLen(string str, int index, int len, ref UnicodeCategory ucCurrent, ref int currentCharCount)
		{
			if (index + currentCharCount == len)
			{
				return currentCharCount;
			}
			int num;
			UnicodeCategory unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, index + currentCharCount, out num);
			if (CharUnicodeInfo.IsCombiningCategory(unicodeCategory) && !CharUnicodeInfo.IsCombiningCategory(ucCurrent) && ucCurrent != UnicodeCategory.Format && ucCurrent != UnicodeCategory.Control && ucCurrent != UnicodeCategory.OtherNotAssigned && ucCurrent != UnicodeCategory.Surrogate)
			{
				int num2 = index;
				for (index += currentCharCount + num; index < len; index += num)
				{
					unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, index, out num);
					if (!CharUnicodeInfo.IsCombiningCategory(unicodeCategory))
					{
						ucCurrent = unicodeCategory;
						currentCharCount = num;
						break;
					}
				}
				return index - num2;
			}
			int result = currentCharCount;
			ucCurrent = unicodeCategory;
			currentCharCount = num;
			return result;
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x00128A24 File Offset: 0x00126C24
		public static string GetNextTextElement(string str, int index)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			int length = str.Length;
			if (index >= 0 && index < length)
			{
				int num;
				UnicodeCategory unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, index, out num);
				return str.Substring(index, StringInfo.GetCurrentTextElementLen(str, index, length, ref unicodeCategory, ref num));
			}
			if (index == length)
			{
				return string.Empty;
			}
			throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("Index was out of range. Must be non-negative and less than the size of the collection."));
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x00128A8A File Offset: 0x00126C8A
		public static TextElementEnumerator GetTextElementEnumerator(string str)
		{
			return StringInfo.GetTextElementEnumerator(str, 0);
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x00128A94 File Offset: 0x00126C94
		public static TextElementEnumerator GetTextElementEnumerator(string str, int index)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			int length = str.Length;
			if (index < 0 || index > length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("Index was out of range. Must be non-negative and less than the size of the collection."));
			}
			return new TextElementEnumerator(str, index, length);
		}

		// Token: 0x060057E9 RID: 22505 RVA: 0x00128ADC File Offset: 0x00126CDC
		public static int[] ParseCombiningCharacters(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			int length = str.Length;
			int[] array = new int[length];
			if (length == 0)
			{
				return array;
			}
			int num = 0;
			int i = 0;
			int num2;
			UnicodeCategory unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, 0, out num2);
			while (i < length)
			{
				array[num++] = i;
				i += StringInfo.GetCurrentTextElementLen(str, i, length, ref unicodeCategory, ref num2);
			}
			if (num < length)
			{
				int[] array2 = new int[num];
				Array.Copy(array, array2, num);
				return array2;
			}
			return array;
		}

		// Token: 0x0400368C RID: 13964
		[OptionalField(VersionAdded = 2)]
		private string m_str;

		// Token: 0x0400368D RID: 13965
		[NonSerialized]
		private int[] m_indexes;
	}
}
