using System;
using System.Collections;
using System.Collections.Generic;

namespace Duckov.CreditsUtility
{
	// Token: 0x0200030E RID: 782
	public class CreditsLexer : IEnumerable<Token>, IEnumerable
	{
		// Token: 0x060019B1 RID: 6577 RVA: 0x0005E194 File Offset: 0x0005C394
		public CreditsLexer(string content)
		{
			this.content = content;
			this.cursor = 0;
			this.lineBegin = 0;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0005E1B1 File Offset: 0x0005C3B1
		public void Reset()
		{
			this.cursor = 0;
			this.lineBegin = 0;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		private void TrimLeft()
		{
			while ((int)this.cursor < this.content.Length)
			{
				char c = this.content[(int)this.cursor];
				if (!char.IsWhiteSpace(c))
				{
					return;
				}
				if (c == '\n')
				{
					return;
				}
				this.cursor += 1;
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0005E218 File Offset: 0x0005C418
		public Token Next()
		{
			this.TrimLeft();
			if ((int)this.cursor >= this.content.Length)
			{
				this.cursor += 1;
				return new Token(TokenType.End, null);
			}
			char c = this.content[(int)this.cursor];
			if (c == '\n')
			{
				this.cursor += 1;
				return new Token(TokenType.EmptyLine, null);
			}
			if (c == '#')
			{
				this.cursor += 1;
				int startIndex = (int)this.cursor;
				while ((int)this.cursor < this.content.Length && this.content[(int)this.cursor] != '\n')
				{
					this.cursor += 1;
				}
				this.cursor += 1;
				return new Token(TokenType.Comment, this.content.Substring(startIndex, (int)this.cursor));
			}
			if (c == '[')
			{
				this.cursor += 1;
				int num = (int)this.cursor;
				while ((int)this.cursor < this.content.Length)
				{
					if (this.content[(int)this.cursor] == ']')
					{
						string text = this.content.Substring(num, (int)this.cursor - num);
						while ((int)this.cursor < this.content.Length)
						{
							this.cursor += 1;
							if ((int)this.cursor >= this.content.Length)
							{
								break;
							}
							c = this.content[(int)this.cursor];
							if (c == '\n')
							{
								this.cursor += 1;
								break;
							}
							if (!char.IsWhiteSpace(c))
							{
								break;
							}
						}
						return new Token(TokenType.Instructor, text);
					}
					if (this.content[(int)this.cursor] == '\n')
					{
						this.cursor += 1;
						return new Token(TokenType.Invalid, this.content.Substring(num, (int)this.cursor - num));
					}
					this.cursor += 1;
				}
				return new Token(TokenType.Invalid, this.content.Substring(num - 1));
			}
			int num2 = (int)this.cursor;
			string raw;
			while ((int)this.cursor < this.content.Length)
			{
				c = this.content[(int)this.cursor];
				if (c == '\n')
				{
					raw = this.content.Substring(num2, (int)this.cursor - num2);
					this.cursor += 1;
					return new Token(TokenType.String, this.ConvertEscapes(raw));
				}
				if (c == '#')
				{
					raw = this.content.Substring(num2, (int)this.cursor - num2);
					return new Token(TokenType.String, this.ConvertEscapes(raw));
				}
				this.cursor += 1;
			}
			raw = this.content.Substring(num2, (int)this.cursor - num2);
			return new Token(TokenType.String, this.ConvertEscapes(raw));
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0005E503 File Offset: 0x0005C703
		private string ConvertEscapes(string raw)
		{
			return raw.Replace("\\n", "\n");
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0005E515 File Offset: 0x0005C715
		public IEnumerator<Token> GetEnumerator()
		{
			while ((int)this.cursor < this.content.Length)
			{
				Token token = this.Next();
				yield return token;
			}
			yield break;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0005E524 File Offset: 0x0005C724
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040012B8 RID: 4792
		private readonly string content;

		// Token: 0x040012B9 RID: 4793
		private ushort cursor;

		// Token: 0x040012BA RID: 4794
		private ushort lineBegin;
	}
}
