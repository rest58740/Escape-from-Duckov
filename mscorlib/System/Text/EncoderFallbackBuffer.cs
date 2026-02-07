using System;

namespace System.Text
{
	// Token: 0x020003A2 RID: 930
	public abstract class EncoderFallbackBuffer
	{
		// Token: 0x0600261F RID: 9759
		public abstract bool Fallback(char charUnknown, int index);

		// Token: 0x06002620 RID: 9760
		public abstract bool Fallback(char charUnknownHigh, char charUnknownLow, int index);

		// Token: 0x06002621 RID: 9761
		public abstract char GetNextChar();

		// Token: 0x06002622 RID: 9762
		public abstract bool MovePrevious();

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06002623 RID: 9763
		public abstract int Remaining { get; }

		// Token: 0x06002624 RID: 9764 RVA: 0x00087211 File Offset: 0x00085411
		public virtual void Reset()
		{
			while (this.GetNextChar() != '\0')
			{
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0008721B File Offset: 0x0008541B
		internal void InternalReset()
		{
			this.charStart = null;
			this.bFallingBack = false;
			this.iRecursionCount = 0;
			this.Reset();
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x00087239 File Offset: 0x00085439
		internal unsafe void InternalInitialize(char* charStart, char* charEnd, EncoderNLS encoder, bool setEncoder)
		{
			this.charStart = charStart;
			this.charEnd = charEnd;
			this.encoder = encoder;
			this.setEncoder = setEncoder;
			this.bUsedEncoder = false;
			this.bFallingBack = false;
			this.iRecursionCount = 0;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00087270 File Offset: 0x00085470
		internal char InternalGetNextChar()
		{
			char nextChar = this.GetNextChar();
			this.bFallingBack = (nextChar > '\0');
			if (nextChar == '\0')
			{
				this.iRecursionCount = 0;
			}
			return nextChar;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x0008729C File Offset: 0x0008549C
		internal unsafe virtual bool InternalFallback(char ch, ref char* chars)
		{
			int index = (chars - this.charStart) / 2 - 1;
			if (char.IsHighSurrogate(ch))
			{
				if (chars >= this.charEnd)
				{
					if (this.encoder != null && !this.encoder.MustFlush)
					{
						if (this.setEncoder)
						{
							this.bUsedEncoder = true;
							this.encoder._charLeftOver = ch;
						}
						this.bFallingBack = false;
						return false;
					}
				}
				else
				{
					char c = (char)(*chars);
					if (char.IsLowSurrogate(c))
					{
						if (this.bFallingBack)
						{
							int num = this.iRecursionCount;
							this.iRecursionCount = num + 1;
							if (num > 250)
							{
								this.ThrowLastCharRecursive(char.ConvertToUtf32(ch, c));
							}
						}
						chars += 2;
						this.bFallingBack = this.Fallback(ch, c, index);
						return this.bFallingBack;
					}
				}
			}
			if (this.bFallingBack)
			{
				int num = this.iRecursionCount;
				this.iRecursionCount = num + 1;
				if (num > 250)
				{
					this.ThrowLastCharRecursive((int)ch);
				}
			}
			this.bFallingBack = this.Fallback(ch, index);
			return this.bFallingBack;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0008739A File Offset: 0x0008559A
		internal void ThrowLastCharRecursive(int charRecursive)
		{
			throw new ArgumentException(SR.Format("Recursive fallback not allowed for character \\\\u{0:X4}.", charRecursive), "chars");
		}

		// Token: 0x04001DB3 RID: 7603
		internal unsafe char* charStart;

		// Token: 0x04001DB4 RID: 7604
		internal unsafe char* charEnd;

		// Token: 0x04001DB5 RID: 7605
		internal EncoderNLS encoder;

		// Token: 0x04001DB6 RID: 7606
		internal bool setEncoder;

		// Token: 0x04001DB7 RID: 7607
		internal bool bUsedEncoder;

		// Token: 0x04001DB8 RID: 7608
		internal bool bFallingBack;

		// Token: 0x04001DB9 RID: 7609
		internal int iRecursionCount;

		// Token: 0x04001DBA RID: 7610
		private const int iMaxRecursion = 250;
	}
}
