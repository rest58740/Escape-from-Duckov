using System;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000033 RID: 51
	internal class Tokenizer
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00005D69 File Offset: 0x00003F69
		public Tokenizer(string fmt)
		{
			this.formatString = fmt;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005D78 File Offset: 0x00003F78
		public int Position
		{
			get
			{
				return this.formatStringPosition;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00005D80 File Offset: 0x00003F80
		public int Length
		{
			get
			{
				string text = this.formatString;
				if (text == null)
				{
					return 0;
				}
				return text.Length;
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005D93 File Offset: 0x00003F93
		public string Substring(int startIndex, int length)
		{
			return this.formatString.Substring(startIndex, length);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005DA2 File Offset: 0x00003FA2
		public int Peek(int offset = 0)
		{
			if (this.formatStringPosition + offset >= this.Length)
			{
				return -1;
			}
			return (int)this.formatString[this.formatStringPosition + offset];
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005DCC File Offset: 0x00003FCC
		public int PeekUntil(int startOffset, int until)
		{
			int num = startOffset;
			int num2;
			do
			{
				num2 = this.Peek(num++);
				if (num2 == -1)
				{
					return 0;
				}
			}
			while (num2 != until);
			return num - startOffset;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005DF4 File Offset: 0x00003FF4
		public bool PeekOneOf(int offset, string s)
		{
			foreach (char c in s)
			{
				if (this.Peek(offset) == (int)c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005E29 File Offset: 0x00004029
		public void Advance(int characters = 1)
		{
			this.formatStringPosition = Math.Min(this.formatStringPosition + characters, this.formatString.Length);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005E49 File Offset: 0x00004049
		public bool ReadOneOrMore(int c)
		{
			if (this.Peek(0) != c)
			{
				return false;
			}
			while (this.Peek(0) == c)
			{
				this.Advance(1);
			}
			return true;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005E69 File Offset: 0x00004069
		public bool ReadOneOf(string s)
		{
			if (this.PeekOneOf(0, s))
			{
				this.Advance(1);
				return true;
			}
			return false;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005E80 File Offset: 0x00004080
		public bool ReadString(string s, bool ignoreCase = false)
		{
			if (this.formatStringPosition + s.Length > this.Length)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				char c2 = (char)this.Peek(i);
				if (ignoreCase)
				{
					if (char.ToLower(c) != char.ToLower(c2))
					{
						return false;
					}
				}
				else if (c != c2)
				{
					return false;
				}
			}
			this.Advance(s.Length);
			return true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005EEC File Offset: 0x000040EC
		public bool ReadEnclosed(char open, char close)
		{
			if (this.Peek(0) == (int)open)
			{
				int num = this.PeekUntil(1, (int)close);
				if (num > 0)
				{
					this.Advance(1 + num);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000088 RID: 136
		private string formatString;

		// Token: 0x04000089 RID: 137
		private int formatStringPosition;
	}
}
